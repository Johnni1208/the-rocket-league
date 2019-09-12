import { Component, OnInit } from '@angular/core';
import { NavbarType } from '../../Enums/navbar-types.enumeration';
import { AuthService } from '../../Services/auth/auth.service';
import { Rocket } from '../../Models/rocket';
import { UserService } from '../../Services/user/user.service';

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.scss']
})
export class UserComponent implements OnInit {
  public user = AuthService.getDecodedToken();
  public navbarType: NavbarType = NavbarType.User;
  public rocketList: Rocket[];

  public constructor(private userService: UserService) {}

  public ngOnInit(): void {
    this.userService.getUser(this.user.nameid).subscribe(res => {
      this.rocketList = res.rockets.reverse();
    });
  }
}
