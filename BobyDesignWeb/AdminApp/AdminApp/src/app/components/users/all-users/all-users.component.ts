import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { PageView } from 'src/app/models/common.model';
import { RoleSearch, User } from 'src/app/models/user.model';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-all-users',
  templateUrl: './all-users.component.html',
  styleUrls: ['./all-users.component.scss']
})
export class AllUsersComponent implements OnInit {

  constructor(private activatedRoute: ActivatedRoute, private router: Router, private userService: UserService) { }
  userPage?: PageView<User>;
  userBeingEditedId?: string;
  searchPhrase: string = '';
  searchRole: RoleSearch = RoleSearch.All;

  ngOnInit(): void {
    this.activatedRoute.data.subscribe((response: any) => {
      this.userPage = response.userPage;
    });
    
    this.searchPhrase = this.activatedRoute.snapshot.queryParams["searchPhrase"];
    this.searchRole = this.activatedRoute.snapshot.queryParams["role"] || RoleSearch.All;
  }

  toggleUserBeingEdited(userId: string) {
    if(this.userBeingEditedId === userId){
      this.userBeingEditedId = undefined;
    } else {
      this.userBeingEditedId = userId;
    }
  }

  async applySearch() {
    const queryParams: Params = { 
      searchPhrase: this.searchPhrase,  
      role: this.searchRole,
  };
  this.router.navigate(
    [], 
    {
      relativeTo: this.activatedRoute,
      queryParams, 
      queryParamsHandling: 'merge', // remove to replace all query params by provided
    });
    this.userPage = await this.userService.searchUsers(0, this.searchRole, this.searchPhrase) || undefined;
  }

  async switchPage(pageNumber: number) {
    const queryParams: Params = { 
      page: pageNumber
  };
  this.router.navigate(
    [], 
    {
      relativeTo: this.activatedRoute,
      queryParams, 
      queryParamsHandling: 'merge', // remove to replace all query params by provided
    });
    this.userPage = await this.userService.searchUsers(pageNumber, this.searchRole, this.searchPhrase) || undefined;
  }
}
