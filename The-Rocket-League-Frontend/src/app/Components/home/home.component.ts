import { Component, OnInit } from '@angular/core';
import {RocketService} from '../../Services/rocket/rocket.service';
import {AlertifyService} from '../../Services/alertify/alertify.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {

  constructor(private rocketService: RocketService, private alertify: AlertifyService) { }

  ngOnInit() {
  }

  addRocket() {
    this.rocketService.addRocket().subscribe(() => {
      this.alertify.success('Rocket getrunken.');
    }, error => {
      this.alertify.error('Konnte nicht abheben.');
    });
  }
}
