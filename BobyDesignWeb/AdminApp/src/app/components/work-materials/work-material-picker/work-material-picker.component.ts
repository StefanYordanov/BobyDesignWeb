import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { WorkMaterialModel } from 'src/app/models/work-materials.model';
import { WorkMaterialsService } from 'src/app/services/work-materials.service';

@Component({
  selector: 'app-work-material-picker',
  templateUrl: './work-material-picker.component.html',
  styleUrls: ['./work-material-picker.component.scss']
})
export class WorkMaterialPickerComponent implements OnInit {

  constructor(private workMaterialService: WorkMaterialsService) { }
  
  @Input() date?: Date;
  @Output() workMaterialChanged = new EventEmitter<WorkMaterialModel>()

  workMaterials?: WorkMaterialModel[];
  selectedWorkMaterial?: WorkMaterialModel;

  async ngOnInit() {
      this.workMaterials = await this.workMaterialService.getAllWorkMaterials() || undefined;
  }

  toggleWorkMaterial(workMaterial: WorkMaterialModel) {
    if(this.selectedWorkMaterial && this.selectedWorkMaterial.id === workMaterial.id) {
      this.selectedWorkMaterial = undefined;
    } else {
      this.selectedWorkMaterial = workMaterial
    }
    this.workMaterialChanged.emit(this.selectedWorkMaterial);
  }

  
}
