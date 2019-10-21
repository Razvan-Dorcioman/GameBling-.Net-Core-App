import { Component, OnInit } from '@angular/core';
import { DataService } from '../shared/dataService';
import { Router } from '@angular/router';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-editUser',
  templateUrl: './editUser.component.html',
  styleUrls: ['./editUser.component.css'],
})

export class EditUserComponent implements OnInit {

  public user = {
  };
  
  constructor(public data: DataService, private router: Router, private _route: ActivatedRoute) {
    
  }

  ngOnInit(): void {
    const id = this._route.snapshot.params['id'];
    
    this.data.loadUserById(id)
      .subscribe(success => {
        if (success) {      
          this.user = this.data.user;
        }
      });
  }

  saveEditUser(): void {
    /*this.data.updateUser(this.data.user)
      .subscribe(success => {
        if (success) {
          this.router.navigate(["/admin"]);
        }
      });*/
  }

}
