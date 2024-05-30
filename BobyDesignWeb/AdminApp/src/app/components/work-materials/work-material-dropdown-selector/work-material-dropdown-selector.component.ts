import { CurrencyPipe } from '@angular/common';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Subject, debounceTime, distinctUntilChanged } from 'rxjs';
import { MaterialPricingType, WorkMaterialModel } from 'src/app/models/work-materials.model';
import { DateService } from 'src/app/services/date.service';
import { WorkMaterialsService } from 'src/app/services/work-materials.service';

@Component({
  selector: 'app-work-material-dropdown-selector',
  templateUrl: './work-material-dropdown-selector.component.html',
  styleUrls: ['./work-material-dropdown-selector.component.scss']
})
export class WorkMaterialDropdownSelectorComponent implements OnInit {
  searchPhrase = '';
  workMaterialPricingTypes = MaterialPricingType;
  loadedWorkMaterials: WorkMaterialModel[] = [];
  userQuestionUpdate = new Subject<string>()
  @Input() pricingDate?: Date;
  @Input() showPriceColumn: 'sellingPrice' | 'purchasingPrice' = 'sellingPrice';
  @Output() workMaterialChanged = new EventEmitter<WorkMaterialModel>()
  @Input() selectedWorkMaterial? : WorkMaterialModel;

  constructor(private workMaterialsService: WorkMaterialsService, private dateService: DateService, private currencyPipe: CurrencyPipe) { 
    this.userQuestionUpdate.pipe(
      debounceTime(600),
      distinctUntilChanged())
      .subscribe(async (value) => {
        //alert(value);
        if(value.length < 3){
          this.loadedWorkMaterials = [];
          return;
        }
        this.loadedWorkMaterials = (await workMaterialsService.getBySearch(value, this.pricingDate ?? new Date())) || [];
      });

  }
  
  ngOnInit(): void {
  }
  deselectWorkMaterial(){
    this.workMaterialChanged.emit(undefined);
    this.selectedWorkMaterial = undefined;
  }

  clickWorkMaterial(workMaterial: WorkMaterialModel){
    this.workMaterialChanged.emit(workMaterial);
    this.selectedWorkMaterial = workMaterial;
  }

  workMaterialDetailsString(workMaterial: WorkMaterialModel): string{
    let result = workMaterial.name;
    if(!workMaterial.relevantPrice){
      return result;
    }

    if(workMaterial.pricingType === MaterialPricingType.Fixed){
      result+= ` - ${this.currencyPipe.transform(workMaterial.relevantPrice[this.showPriceColumn], 'BGN', 'лв.')} (фиксирано)`
    } else if(workMaterial.pricingType === MaterialPricingType.PerGram){
      result+= ` - ${this.currencyPipe.transform(workMaterial.relevantPrice[this.showPriceColumn], 'BGN', 'лв.')}/${workMaterial.measuringUnit}`
    }

    return result;
  }
}
