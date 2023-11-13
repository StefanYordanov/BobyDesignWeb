import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { RoleItem } from 'src/app/models/user.model';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-user-rights',
  templateUrl: './user-rights.component.html',
  styleUrls: ['./user-rights.component.scss']
})
export class UserRightsComponent implements OnInit {

  @Output() rolesSavedEvent = new EventEmitter<boolean>();
  @Input() userId = '';
  roleItems?: RoleItem[];
  constructor( private userService: UserService) { }

  async ngOnInit(): Promise<void> {
    this.roleItems = await this.userService.getRolesByUserId(this.userId) || undefined
  }

  async saveRoleItems() {
    if(!this.roleItems) {
      return;
    }
    await this.userService.updateUserRoles(this.userId, this.roleItems);
    this.rolesSavedEvent.emit(true);
  }
}
