import { Component, OnInit } from '@angular/core';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-main-page',
  templateUrl: './main-page.component.html',
  styleUrls: ['./main-page.component.scss']
})
export class MainPageComponent implements OnInit {

  userRoles?: string[];
  constructor(private userService: UserService) { }

  async ngOnInit() {
    this.userRoles = await this.userService.getUserRoles();
  }

}
