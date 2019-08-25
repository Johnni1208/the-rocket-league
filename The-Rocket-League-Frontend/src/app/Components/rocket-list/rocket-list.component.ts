import { Component, Input } from '@angular/core';
import { Rocket } from '../../Models/rocket';
import * as moment from 'moment';

@Component({
  selector: 'app-rocket-list',
  templateUrl: './rocket-list.component.html',
  styleUrls: ['./rocket-list.component.scss']
})
export class RocketListComponent {

  @Input()
  public rocketList: Rocket[];

  public getTimeAgo(rocket: Rocket): string {
    return moment(rocket.dateAdded).fromNow();
  }
}
