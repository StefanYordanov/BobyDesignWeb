import { AfterViewInit, Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { JewelryShopsService } from 'src/app/services/jewelry-shops.service';
import { SuppliersService } from 'src/app/services/suppliers.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-navigation-menu',
  templateUrl: './navigation-menu.component.html',
  styleUrls: ['./navigation-menu.component.scss']
})
export class NavigationMenuComponent implements OnInit, AfterViewInit {

  userRolesPromise = this.userService.getUserRoles();
  selectedJewelryShopId?: number;
  jewelryShopsDataPromise = Promise.all([this.jewelryShopsService.getAll(), 
    this.jewelryShopsService.getUserActiveShop()]).then(data => {
      this.selectedJewelryShopId = data[1]?.id;
      return {
        all: data[0],
        selected: data[1]
      }
    })
    selectedSupplierId?: number
    suppliersDataPromise = Promise.all([this.suppliersService.getAll(), 
      this.suppliersService.getDefaultActiveSupplier()]).then(data => {
        this.selectedSupplierId = data[1]?.id;
        return {
          all: data[0],
          selected: data[1]
        }
      })

  @ViewChild('loginPartial') loginPartial!: ElementRef;
  constructor(private userService: UserService, private jewelryShopsService: JewelryShopsService, private suppliersService: SuppliersService, private toastr: ToastrService) { 

  }

  async changeJewelryShop(){
    const response = await this.jewelryShopsService.setUserActiveShop(this.selectedJewelryShopId || null);
    if(this.selectedJewelryShopId == response?.id) {
      this.toastr.success('Успешно сменихте текущия си магазин');
    }
  }


  ngOnInit(): void {
    $('.built-in-header').hide()

  }

  ngAfterViewInit()
  {
    const loginContent = $('.built-in-header .login-partial');
    $(this.loginPartial.nativeElement).append(loginContent);
  }

}
