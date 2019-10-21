import { Component, OnInit } from '@angular/core';
import { HttpClient, HttpParams, HttpHeaders } from "@angular/common/http";
import { DataService } from '../shared/dataService';
import { Router } from '@angular/router';
import { CookieService } from 'ngx-cookie-service';

@Component({
  selector: "deposit",
  templateUrl: "deposit.component.html",
  styleUrls: ['./deposit.component.css']
})

export class DepositComponent {

  constructor(private data: DataService, private router: Router, private cookie: CookieService) { }

  public funds: number = 0;
  public errorMessage: string = "";


  UpdateFunds() {
    this.errorMessage = "";
    this.data.UpdateFunds(this.data.loggedInUser.id, this.funds)
      .subscribe(success => {
        if (success) {
          this.router.navigate([""]);
        }
      }, err => this.errorMessage = "Can't add funds!")
  }

  WithdrawFunds() {
    this.errorMessage = "";
    this.data.UpdateFunds(this.data.loggedInUser.id, (this.funds * (-1)))
      .subscribe(success => {
        if (success) {
          this.router.navigate([""]);
        }
        else {
          this.errorMessage = "Can't withdraw funds!"
        }
      }, err => this.errorMessage = "")
  }
  
}
