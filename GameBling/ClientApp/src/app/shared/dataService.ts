import { HttpClient, HttpParams, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { map, retry } from "rxjs/operators";
import { Observable } from "rxjs/Observable";
import 'rxjs/add/operator/map';
import { CookieService } from "ngx-cookie-service";

@Injectable()
export class DataService {

  public id: string = '';
  public userName: string = '';
  public userBalance: number = 0;

  public loggedInUser = null;
  public value: number = null;
  public amount: number = null;

  public canBet: boolean = false; // null for true and false for false

  public users = [];
  public user = {};
  public basic = '';
  public cards = [];

  constructor(private http: HttpClient, private cookie: CookieService) {
  }

  createAuthorizationHeader(headers: Headers, basic) {
    headers.append('Authorization', basic);
  }

  loadUsers() {
    return this.http.get("/api/users")
      .pipe(
        map((data: any[]) => {
          this.users = data;
          return true;
        }));
  }

  loadUserById(id) {
    let params = new HttpParams();
    params.append('id', id);
    return this.http.get("/api/users/getUserById/"+ id)
      .pipe(
      map((data: any[]) => {
        this.user = data;
          return true;
      }));
  }

  updateUser(user) {
    return this.http.post("/api/users/updateUser", user);
  }

  deleteUser(id) {
    return this.http.delete("/api/users/deleteUser/" + id);
  }

  loadCards() {
    return this.http.get("/api/cards")
      .pipe(
        map((data: any[]) => {
          this.cards = data;
          return true;
        }));
  }

  public get loginRequired(): boolean {
    let tokenExpiration = new Date(this.cookie.get('tokenExpiration'));
    let token = this.cookie.get('token');
    return token.length == 0 || tokenExpiration < new Date();
  }

  login(creds): Observable<boolean> {
    const hack = this.cookie;
    return this.http
      .post("/account/createtoken", creds)
      .map((data: any) => {
        if (creds.rememberMe) {
          hack.set('id', data.user.id);
          hack.set('token', data.token);
          hack.set('tokenExpiration', data.expiration);
        }
        else {
          hack.set('id', data.user.id, 1);
          hack.set('token', data.token, 1);
          hack.set('tokenExpiration', data.expiration, 1);
        }

        this.loggedInUser = data.user;
        return true;
      });
  }

  register(creds) {
    return this.http
      .post("/account/register", creds);
  }

  addCard(cardInfo) {
    return this.http
      .post("/api/cards", cardInfo);
  }

  UpdateFunds(id, balance) {
    return this.http
      .get("/api/users/funds/" + id + "/" + balance);
  }

  signout() {
    return this.http
      .get("/account/logout");
  }
}
