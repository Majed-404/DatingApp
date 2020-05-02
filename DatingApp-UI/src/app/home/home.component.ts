import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  registerMode = true;
  constructor() { }

  ngOnInit() {
  }

  RegisterToggle() {
    this.registerMode = !this.registerMode;
  }

  CancelRegisterMode(registerMode: boolean) {
    this.registerMode = !this.registerMode;
  }

}
