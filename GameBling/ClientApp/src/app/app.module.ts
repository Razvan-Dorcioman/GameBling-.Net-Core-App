import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { CookieService } from 'ngx-cookie-service';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { AdminComponent } from './admin/admin.component';
import { DataService } from './shared/dataService';
import { SignalRService } from './services/signal-r.service';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { CardComponent } from './add-card/card.component';
import { DepositComponent } from './deposit/deposit.component';
import { CardModule } from 'ngx-card/ngx-card';
import { EditUserComponent } from './editUser/editUser.component';
 
@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    AdminComponent,
    LoginComponent,
    RegisterComponent,
    CardComponent,
    DepositComponent,
    EditUserComponent
  ],
  imports: [
    BrowserModule,
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    CardModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'admin', component: AdminComponent },
      { path: 'login', component: LoginComponent },
      { path: 'register', component: RegisterComponent},
      { path: 'card', component: CardComponent },
      { path: 'deposit', component: DepositComponent },
      { path: 'admin/edit/:id', component: EditUserComponent }
    ])
  ],
  providers: [
    DataService,
    CookieService,
    SignalRService
  ],
  bootstrap: [AppComponent]


})
export class AppModule { }
