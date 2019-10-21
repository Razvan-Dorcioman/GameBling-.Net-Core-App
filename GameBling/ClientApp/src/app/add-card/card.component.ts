import { Component, OnInit } from '@angular/core';
import { DataService } from '../shared/dataService';
import { Router } from '@angular/router';
import { NgForm, FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { CookieService } from 'ngx-cookie-service';


@Component({
  selector: "card",
  templateUrl: "card.component.html",
  styleUrls: ['./card.component.css']
})

export class CardComponent {

  constructor(private data: DataService, private router: Router, private cookie: CookieService) { }
 
  errorMessage: string = "";
  public cardInfo = {
    cardNumber: <number>null,
    expirationDate: <Date>null,
    CVC: <number>null,
    cardHolderName: <string>null,
    username: <string>this.data.userName
  };

  AddCard() {
    this.errorMessage = "";
    this.data.addCard(this.cardInfo)
      .subscribe(success => {
        if (success) {
          this.router.navigate([""]);
        }
      }, err => this.errorMessage = "Invalid card info")
  }
}
