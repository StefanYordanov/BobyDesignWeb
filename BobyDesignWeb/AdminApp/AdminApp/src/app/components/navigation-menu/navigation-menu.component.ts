import { AfterViewInit, Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-navigation-menu',
  templateUrl: './navigation-menu.component.html',
  styleUrls: ['./navigation-menu.component.scss']
})
export class NavigationMenuComponent implements OnInit, AfterViewInit {

  userRolesPromise = this.userService.getUserRoles();

  @ViewChild('loginPartial') loginPartial!: ElementRef;
  constructor(private userService: UserService) { 

  }

  ngOnInit(): void {
    $('.built-in-header').hide()

  }

  ngAfterViewInit()
  {
    const loginContent = $('.built-in-header .login-partial');
    console.log(loginContent, this.loginPartial);
    $(this.loginPartial.nativeElement).append(loginContent);
  }

}
