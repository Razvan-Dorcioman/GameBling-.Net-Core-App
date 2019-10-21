import { Component, OnInit } from '@angular/core';
import { DataService } from '../shared/dataService';
import { Router } from '@angular/router';
import { Card } from '../_interfaces/card';
import { Route } from '@angular/compiler/src/core';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.css']
})
export class AdminComponent implements OnInit {

  public users = [];
  public cards = [];

  constructor(private data: DataService, private router: Router) {
    this.cards = data.cards;
  }

  ngOnInit(): void {
    this.data.loadUsers()
      .subscribe(success => {
        if (success) {
          this.users = this.data.users;
        }
      });

    this.data.loadCards()
      .subscribe(succeess => {
        if (succeess) {
          this.cards = this.data.cards;
        }
      });
  }

  editUser(id: string) {
    if (!this.data.loginRequired) {
      this.router.navigate(["/admin/edit",id]);
    }
    else {
      this.router.navigate(["/login"]);
    }
  }

  deleteUser(id: string): void {
    console.log(id);
    this.data.deleteUser(id)
      .subscribe(success => {
        if (success) {
          location.reload();
        }
      });
  }

}
