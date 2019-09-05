import { Component, OnInit } from '@angular/core';
import { RocketService } from '../../Services/rocket/rocket.service';
import { AlertifyService } from '../../Services/alertify/alertify.service';
import { Rocket } from '../../Models/rocket';
import * as moment from 'moment';
import { NavbarType } from '../../Enums/navbar-types.enumeration';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {
  public navbarType: NavbarType = NavbarType.Home;

  public rocketList: Rocket[];

  public isRocketFiring: boolean;
  public isRocketFlying: boolean;

  private clicked = 0;
  private readonly TIMES_IT_NEEDS_TO_BE_CLICKED = 2;

  public ngOnInit(): void {
    this.getAllRockets();
  }

  public constructor(
    private rocketService: RocketService,
    private alertify: AlertifyService
  ) { }

  public addRocket() {
    this.clicked++;
    this.isRocketFiring = true;
    this.resetClickAfterTime();

    if (this.clicked === this.TIMES_IT_NEEDS_TO_BE_CLICKED) {
      this.rocketService.addRocket().subscribe(() => {
        this.getAllRockets();
        this.isRocketFlying = true;
        this.clicked = 0;
        this.resetAnimation();
      }, error => {
        this.alertify.error('Konnte nicht abheben.');
        this.isRocketFiring = false;
        this.clicked = 0;
      });
    }
  }

  private resetClickAfterTime(timeInSec: number = 3) {
    setTimeout(() => {
      if (this.clicked > 0 && this.clicked < 2) {
        this.clicked--;
        this.isRocketFiring = false;
      }
    }, timeInSec * 1000);
  }

  private getAllRockets() {
    this.rocketService.getAllRockets().subscribe(res => {
      this.rocketList = res.reverse();
    });
  }

  private resetAnimation() {
    setTimeout(() => {
      this.isRocketFiring = false;
      this.isRocketFlying = false;
    }, 3000);
  }
}
