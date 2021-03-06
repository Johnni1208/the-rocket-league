import { Component, Input, OnInit } from '@angular/core';
import { NavbarType } from '../../Enums/navbar-types.enumeration';
import { Unicon } from '../../../assets/unicons-mapping';
import { AuthService } from '../../Services/auth/auth.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss']
})
export class NavbarComponent implements OnInit {
  @Input()
  navbarType: NavbarType;

  public rightSideLink: string;
  public rightSideIcon: Unicon;

  public leftSideLink: string;
  public leftSideIcon: Unicon;

  private navBarTypeDict: { [key in NavbarType]: () => void } = {
    [NavbarType.Home]: () => this.onHome(),
    [NavbarType.User]: () => this.onOwnUserProfile(),
    [NavbarType.RankingList]: () => {},
  };

  public ngOnInit(): void {
    this.navBarTypeDict[this.navbarType]();
  }

  private onHome(): void {
    this.leftSideLink = '';
    this.leftSideIcon = Unicon.Trophy;

    const userId = AuthService.getDecodedToken().nameid;

    this.rightSideLink = `/user/${userId}`;
    this.rightSideIcon = Unicon.User;
  }

  private onOwnUserProfile(): void {
    this.leftSideLink = '/';
    this.leftSideIcon = Unicon.ArrowLeft;

    this.rightSideLink = '/login';
    this.rightSideIcon = Unicon.Logout;
  }

}
