import { Injectable } from "@angular/core";
import { ApiService } from "./api.service";
import { CanvasBackground } from "../models/web-content.model";

@Injectable({
    providedIn: 'root'
  })
  export class WebContentService {

    constructor(private apiService: ApiService) {}

    async getCanvasBackgrounds(): Promise<CanvasBackground[]> {
        const backgrounds = await this.apiService.get<CanvasBackground[]>('webContent/getCanvasBackgrounds') || [];
        console.log(backgrounds);
        return backgrounds;
      }
  }