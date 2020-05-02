import { AlertifyService } from './../_services/alertify.service';
import { AuthService } from './../_services/auth.service';
import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { THIS_EXPR } from '@angular/compiler/src/output/output_ast';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  @Output() cancelRegister = new EventEmitter();
  model: any = {};

  constructor(private authService: AuthService, private alertify: AlertifyService) { }

  ngOnInit() {
  }

  Register() {
    this.authService.Register(this.model).subscribe(() => {
      this.alertify.Success('successfull register');
    }, error => {
      this.alertify.Error(error);
    });
  }

  Cancel() {
    this.cancelRegister.emit(false);
    console.log('Canceld');
  }

}
