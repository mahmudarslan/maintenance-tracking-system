import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-scheduler',
  templateUrl: './scheduler.component.html',
  styleUrls: ['./scheduler.component.scss']
})
export class SchedulerComponent implements OnInit {

  appointmentsData: null;
  currentDate: Date = new Date(2021, 4, 27);

  constructor() { }

  ngOnInit(): void {
  }

}
