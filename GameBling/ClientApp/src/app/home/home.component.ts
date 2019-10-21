import { Component, OnInit } from '@angular/core';
import { DataService } from '../shared/dataService';
import { CookieService } from 'ngx-cookie-service';
import { SignalRService } from '../services/signal-r.service';
import { BetModel } from '../_interfaces/betmodel.model';
import { User } from '../_interfaces/user';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})

export class HomeComponent implements OnInit {

  public users: User[];
  public errMessage: String = "";

  constructor(private cookie: CookieService, public data: DataService, public signalRService: SignalRService) {
    this.users = data.users;
  }

  ngOnInit(): void {

    this.data.id = this.cookie.get('id') || null;

    // if user is remembered aka stored in cookies, refresh his personal data in cookies an so on
    if (this.data.id != null) {

      this.data.loadUserById(this.data.id)
        .subscribe(success => {
          if (success) {
            this.data.loggedInUser = this.data.user;
            this.data.userBalance = this.data.loggedInUser.balance || 0;
            this.data.userName = this.data.loggedInUser.userName || 'Guest';
          }
        });

    } else {
      this.data.userName = 'Guest';
      this.data.userBalance = 0;
    }

    this.data.loadUsers()
      .subscribe(succeess => {
        if (succeess) {
          this.users = this.data.users;
        }
      });

    // initialize websocket connection
    if (this.signalRService.loaded == 0) {
      this.signalRService.loaded = 1;
      this.signalRService.startConnection();
      this.signalRService.addRoundListener();
      this.signalRService.addBroadcastBetDataListener();
      this.signalRService.addBroadcastCanBetListener();
      //this.signalRService.startHttpRequest();
    }
  }

  placeBet() {
    if (this.data.amount == null || this.data.amount == 0) {
      this.errMessage = "Please enter a bet value!";
    } else if (this.data.value == null) {
      this.errMessage = "Please select your bet!";
    } else if (this.data.userBalance < this.data.amount) {
      this.errMessage = "Not enough money!";
    } else {
      var bm: BetModel = { username: this.data.loggedInUser.userName, value: this.data.value, amount: this.data.amount };
      this.signalRService.broadcastBetData(bm);
      this.errMessage = "";
    }
  }

}
