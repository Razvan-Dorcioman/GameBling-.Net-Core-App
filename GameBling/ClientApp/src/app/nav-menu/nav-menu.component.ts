import { Component, OnInit } from '@angular/core';
import { DataService } from '../shared/dataService';
import { CookieService } from 'ngx-cookie-service';
import { Router } from '@angular/router';

@Component({
  selector: 'nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent implements OnInit {

  constructor(private cookie: CookieService, private router: Router, public data: DataService) {
  }

  ngOnInit(): void {
  }

  onSignOut() {
    this.data.signout()
      .subscribe(success => {
        if (success) {
          this.cookie.delete('id');
          this.cookie.delete('token');
          this.cookie.delete('tokenExpiration');
          this.data.loggedInUser = null;
          this.router.navigate(["/login"]);
        }
      }, err => console.log("Error on signed out: " + err));
  }

  adminDashboard() {
    if (!this.data.loginRequired) {
      this.router.navigate(["/admin"]);
    }
    else {
      this.router.navigate(["/login"]);
    }
  }
  
}
