import { Component } from "@angular/core";
import { DataService } from "../shared/dataService";
import { Router } from "@angular/router";

@Component({
  selector: "register",
  templateUrl: "register.component.html",
  styleUrls: ['./register.component.css']
})

export class RegisterComponent {

  constructor(private data: DataService, private router: Router) { }

  errorMessage: string = "";
  public creds = {
    username: "",
    password: "",
    email: "",
    confirmPassword: ""
  };

  onRegister() {
    this.errorMessage = "";
    this.data.register(this.creds)
      .subscribe(success => {
        if (success) {
          this.router.navigate([""]);
        }
      }, err => this.errorMessage = "Invalid credentials")
  }
}
