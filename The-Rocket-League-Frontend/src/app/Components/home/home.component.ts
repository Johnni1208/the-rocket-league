import {Component, OnInit} from '@angular/core';
import {RocketService} from '../../Services/rocket/rocket.service';
import {AlertifyService} from '../../Services/alertify/alertify.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent {
  private clicked = 0;
  private readonly TIMES_IT_NEEDS_TO_BE_CLICKED = 2;
  public isRocketFiring: boolean;
  public isRocketFlying: boolean;

  constructor(private rocketService: RocketService, private alertify: AlertifyService) {
  }

  addRocket() {
    this.clicked++;
    this.isRocketFiring = true;

    if (this.clicked === this.TIMES_IT_NEEDS_TO_BE_CLICKED) {
      this.rocketService.addRocket().subscribe(() => {
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

  resetAnimation() {
    setTimeout(() => {
      this.isRocketFiring = false;
      this.isRocketFlying = false;
    }, 3000);
  }
}
