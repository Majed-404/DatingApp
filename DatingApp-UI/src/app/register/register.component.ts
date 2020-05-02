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

  constructor(private authService: AuthService) { }

  ngOnInit() {
  }

  Register() {
    this.authService.Register(this.model).subscribe(() => {
      console.log('successfull register');
    }, error => {
      console.log(error);
    });
  }

  Cancel() {
    this.cancelRegister.emit(false);
    console.log('Canceld');
  }

}
