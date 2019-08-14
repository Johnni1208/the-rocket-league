import { Component } from '@angular/core';
import { NavbarType } from '../../Enums/navbar-types.enumeration';

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.scss']
})
export class UserComponent {
  public navbarType: NavbarType = NavbarType.User;
}
