import { Injectable } from '@angular/core';
import { ApiService, Params } from './api.service';
import { RoleItem, User } from '../models/user.model';
import { PageView } from '../models/common.model';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  userRoles?: string[];

  constructor(private apiService: ApiService) {

  }

  async getUserRoles(): Promise<string[]> {
    if (this.userRoles !== undefined) {
      return this.userRoles;
    }
    this.userRoles = await this.apiService.get('users/roles') || [];
    console.log(this.userRoles);
    return this.userRoles;
  }

  async searchUsers(page: number, role?: string, searchPhrase?: string): Promise<PageView<User> | null> {
    console.log({ page, role, searchPhrase });
    let params: Params = {
      page
    };
    if (role) {
      params['role'] = role
    }
    if (searchPhrase) {
      params['searchPhrase'] = searchPhrase
    }
    const users = await this.apiService.get<PageView<User>>('users/search', params);
    console.log(users);
    return users;
  }

  async getRolesByUserId(userId: string) : Promise<RoleItem[] | null> {
    let params: Params = {
      userId
    };
    return await this.apiService.get<RoleItem[]>('users/rolesByUserId', params)
  }

  async updateUserRoles(userId: string, roles: RoleItem[]) {
    const response = await this.apiService.post('users/editUserRoles', {userId, roles})
    console.log(response);
  }
}
