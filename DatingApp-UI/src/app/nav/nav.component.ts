import { AlertifyService } from './../_services/alertify.service';
import { AuthService } from './../_services/auth.service';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model: any = {};

  constructor(public authService: AuthService, private alertify: AlertifyService) { }

  ngOnInit() {
  }

  Login() {
    this.authService.Login(this.model).subscribe(next => {
      this.alertify.Success('Logged Successfully');
    }, error => {
      this.alertify.Error(error);
    });
  }

  LoggedIn() {
    return this.authService.LoggedIn();
  }

  Logout() {
    localStorage.removeItem('token');
    this.alertify.Message('Logged Out');
  }
}
