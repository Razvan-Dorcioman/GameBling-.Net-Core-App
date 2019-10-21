import { Component } from "@angular/core";
import { DataService } from "../shared/dataService";
import { Router } from "@angular/router";

@Component({
  selector: "login",
  templateUrl: "login.component.html",
  styleUrls: ['./login.component.css']
})

export class LoginComponent {

  constructor(private data: DataService, private router: Router) { }

  errorMessage: string = "";
  public creds = {
    username: "",
    password: "",
    rememberMe: false
  };

  onLogin() {
    this.errorMessage = "";
    this.data.login(this.creds)
      .subscribe(success => {
        if (success) {
          this.router.navigate([""]);
        }
      }, err => this.errorMessage = "Invalid credentials " + err);
  }
}
