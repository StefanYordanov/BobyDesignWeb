import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { MaterialPricingType, WorkMaterialModel } from 'src/app/models/work-materials.model';
import { WorkMaterialsService } from 'src/app/services/work-materials.service';

@Component({
  selector: 'app-work-materials',
  templateUrl: './work-materials.component.html',
  styleUrls: ['./work-materials.component.scss']
})
export class WorkMaterialsComponent implements OnInit {

  constructor(private activatedRoute: ActivatedRoute, private workMaterialService: WorkMaterialsService, private toastr: ToastrService ) { }

  showNewWorkMaterialMenu: boolean = false;
  newWorkMaterial?: WorkMaterialModel;

  currentlyEditedMaterialId?: number;
  currentlyEditedMaterial?: WorkMaterialModel;

  workMaterials?: WorkMaterialModel[];

  ngOnInit(): void {
    this.activatedRoute.data.subscribe((response: any) => {
      this.workMaterials = response.workMaterials;
    });
  }

  toggleEditWorkMaterial(workMaterial: WorkMaterialModel) {
    if (this.currentlyEditedMaterialId === workMaterial.id) {
      this.currentlyEditedMaterial = undefined;
      this.currentlyEditedMaterialId = undefined;
    } else {
      this.currentlyEditedMaterialId = workMaterial.id
      this.currentlyEditedMaterial = {
        ...workMaterial,
        relevantPrice: undefined
      };
    }
  }

  toggleEditMaterialPrice(workMaterial: WorkMaterialModel) {

    if(this.currentlyEditedMaterial!.relevantPrice) {
      this.currentlyEditedMaterial!.relevantPrice = undefined;
    } else {
      if(workMaterial.relevantPrice) {
        this.currentlyEditedMaterial!.relevantPrice = {
          ...workMaterial.relevantPrice,
          id: 0,
          lastUpdatedOn: new Date()
        }
      } else {
        this.currentlyEditedMaterial!.relevantPrice = {
          id: 0,
          purchasingPrice: 0,
          sellingPrice: 0,
          workMaterialId: workMaterial.id,
          lastUpdatedOn: new Date()
        }
      }
    }
  }

  toggleNewMaterialMenu() {
    this.showNewWorkMaterialMenu = !this.showNewWorkMaterialMenu;
    if (this.showNewWorkMaterialMenu) {
      this.newWorkMaterial = {
        id: 0,
        name: '',
        measuringUnit: '',
        pricingType: MaterialPricingType.Fixed,
        quantity: 0,
        reservedQuantity: 0,
        relevantPrice: {
          id: 0,
          purchasingPrice: 0,
          sellingPrice: 0,
          workMaterialId: 0,
          lastUpdatedOn: new Date()
        }
      }
    } else {
      this.newWorkMaterial = undefined;
    }
  }

  toggleNewMaterialPrice() {
    if(this.newWorkMaterial!.relevantPrice) {
      this.newWorkMaterial!.relevantPrice = undefined;
    } else {
      this.newWorkMaterial!.relevantPrice = {
        id: 0,
        purchasingPrice: 0,
        sellingPrice: 0,
        workMaterialId: 0,
        lastUpdatedOn: new Date()
      }
    }
  }

  async submitEditedEntry() {
    if (!this.currentlyEditedMaterial) {
      return;
    }
    if (this.currentlyEditedMaterial.relevantPrice && this.currentlyEditedMaterial.relevantPrice.purchasingPrice > this.currentlyEditedMaterial.relevantPrice?.sellingPrice) {
      this.toastr.error("Цена продажба не може да е по-малка от цена замяна");
      return;
    }

    const editedEntry = await this.workMaterialService.editWorkMaterial(this.currentlyEditedMaterial)
    this.currentlyEditedMaterial = undefined;
    this.currentlyEditedMaterialId = undefined;

    if(!this.workMaterials || !editedEntry) {
      return
    }
    this.workMaterials = this.workMaterials.map(x => {
      if(x.id === editedEntry.id) {
        return editedEntry
      }
      return x;
    });

    this.toastr.success("Успешно редактиране на материал")
  }

  cancelEditEntry() {
    this.currentlyEditedMaterial = undefined;
    this.currentlyEditedMaterialId = undefined;
  }

  async submitNewEntry() {
    if (!this.newWorkMaterial) {
      return;
    }
    if (!this.newWorkMaterial.relevantPrice?.purchasingPrice && !this.newWorkMaterial.relevantPrice?.sellingPrice) {
      this.newWorkMaterial!.relevantPrice = undefined;
    } else if (this.newWorkMaterial.relevantPrice?.purchasingPrice > this.newWorkMaterial.relevantPrice?.sellingPrice) {
      this.toastr.error("Цена продажба не може да е по-малка от цена замяна");
      return;
    }

    const model = await this.workMaterialService.addNewWorkMaterial(this.newWorkMaterial);
    if (model && this.workMaterials) {
      this.workMaterials.push(model);
    }
    this.toggleNewMaterialMenu();
    this.toastr.success("Успешно създаване на материал");
  }
}

