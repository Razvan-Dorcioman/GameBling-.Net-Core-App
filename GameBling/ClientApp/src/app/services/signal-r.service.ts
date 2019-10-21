import { Injectable } from '@angular/core';
import { BetModel } from '../_interfaces/betmodel.model';
import * as signalR from '@aspnet/signalr';
import { DataService } from '../shared/dataService';
import { HttpClient, HttpParams } from '@angular/common/http';

@Injectable()
export class SignalRService {

  public loaded: number = 0;
  public timer: string = '0.00';

  public allBetsData: BetModel[] = [];
  public firstBetsData: BetModel[] = [];
  public secondBetsData: BetModel[] = [];

  private hubConnection: signalR.HubConnection;

  public wonDisplay: string = 'none';
  public loseDisplay: string = 'none';
  public firstColor: string = 'initial';
  public secondColor: string = 'initial';

  constructor(private http: HttpClient, private data: DataService) {
  }

  private startHttpRequest = (connectionId) => {
    //console.log("CONNECTION ID" + connectionId);

    let params = new HttpParams();
    params.append('id', connectionId);

    this.http.get('/api/roulette/getInitial/' + connectionId)
      .subscribe(success => {
        if (success) {
          console.log("Current session has been loaded with success.");
        }
      }, err => console.log("Invalid link or sth " + err));
  }

  public startConnection = () => {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl("/echo")
      .build();

    this.hubConnection
      .start()
      .then(() => {
        //console.log('Connection started');
        this.hubConnection.invoke('GetConnectionId')
          .then(connectionId => this.startHttpRequest(connectionId))
          .catch(err => console.error(err));
      })
      .catch(err => console.log('Error while starting connection: ' + err));
  }

  public addRoundListener = () => {
    this.hubConnection.on('timer', (data) => {
      var sec = data / 100;
      this.timer = Number(sec).toFixed(2).toString();
    });
  }

  public broadcastBetData = (_bm) => {
    this.data.canBet = false;

    this.hubConnection.invoke('Echo', _bm)
      .catch(err => console.error(err));
  }

  public addBroadcastBetDataListener = () => {
    this.hubConnection.on('broadcastbetdata', (fetchedData) => {
      this.allBetsData = Object.assign([], fetchedData);

      this.firstBetsData.length = 0;
      this.secondBetsData.length = 0;

      for (var bet of fetchedData) {
        if (bet.value === 0) {
          this.firstBetsData.push(bet);
        } else {
          this.secondBetsData.push(bet);
        }
      }

      //console.log(fetchedData);
    })
  }

  public addBroadcastCanBetListener = () => {
    this.hubConnection.on('broadcastcanbet', (data) => {

      //console.log(data);

      if (data.canBet) {

        if (data.value == 0) {
          this.firstColor = 'green';
          this.secondColor = 'red';
        } else {
          this.firstColor = 'red';
          this.secondColor = 'green';
        }
        setTimeout(() => {
          this.firstColor = 'initial';
          this.secondColor = 'initial';
          // can bet so reset
          this.firstBetsData.length = 0;
          this.secondBetsData.length = 0;
          this.allBetsData.length = 0;
        }, 2500);

        // can bet
        this.data.canBet = null;
        // verify that user has a bet
        var userCurrentBet = this.allBetsData.find((bet) => bet.username.toLowerCase() == this.data.userName.toLowerCase());
        // update the balance if the user has a bat
        if (userCurrentBet) {
          if (userCurrentBet.value == data.value) {
            // player won
            this.wonDisplay = 'initial';
            setTimeout(() => {
              this.wonDisplay = 'none';
            }, 2500);

            this.data.userBalance += +userCurrentBet.amount;
          } else {
            // player lose
            this.loseDisplay = 'initial';
            setTimeout(() => {
              this.loseDisplay = 'none';
            }, 2500);

            this.data.userBalance -= +userCurrentBet.amount;
          }
        }
        // reset choosen value when timer is reset
        this.data.value = null;


      } else {
        // can not bet
        this.data.canBet = false;
      }

      // reset amount either way
      this.data.amount = null;

    });
  }

}
