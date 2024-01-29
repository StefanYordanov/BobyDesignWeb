import { AfterViewInit, Component, Input, OnInit } from '@angular/core';

import { fabric } from 'fabric'
import { WebContentService } from '../services/web-content.service';
import { CanvasBackground } from '../models/web-content.model';

enum CanvasMenuMode {
  Drawing,
  Common
}

@Component({
  selector: 'app-drawing-canvas',
  templateUrl: './drawing-canvas.component.html',
  styleUrls: ['./drawing-canvas.component.scss']
})
export class DrawingCanvasComponent implements AfterViewInit, OnInit {

  @Input() initialBackground?: CanvasBackground;
  canvas?: fabric.Canvas;
  canvasMenuModes = CanvasMenuMode;
  currentCanvasMenuMode = CanvasMenuMode.Common;

  canvasBackgrounds: CanvasBackground[] = [];
  currentCanvasBackgroundUrl?: string;
  currentCanvasBackground?: fabric.Image;

  constructor(private webContentService: WebContentService) { }
  
  
  async ngOnInit() {
    this.canvasBackgrounds= await this.webContentService.getCanvasBackgrounds();
    if(this.initialBackground){
      this.canvasBackgrounds.push(this.initialBackground)
      this.currentCanvasBackgroundUrl = this.initialBackground.url;
    }
  }
  ngAfterViewInit(): void {
    this.canvas = new fabric.Canvas('drawingCanvas', { })
    console.log('CANVAS!', this.canvas)
  }

  onChangeBackground(newBackground?: string) {
    if(!this.canvas){
      return;
    }

    this.currentCanvasBackgroundUrl = newBackground;

    if(this.currentCanvasBackgroundUrl) {
      this.canvas.setBackgroundImage(this.currentCanvasBackgroundUrl, 
        this.canvas.renderAll.bind(this.canvas), {
          width: this.canvas.width,
          height: this.canvas.height,
          // Needed to position overlayImage at 0/0
          originX: 'left',
          originY: 'top'});
    } else {
      const emptyImage = new fabric.Image('');
      this.canvas.setBackgroundImage(emptyImage, 
        this.canvas.renderAll.bind(this.canvas), {
          width: this.canvas.width,
          height: this.canvas.height,
          // Needed to position overlayImage at 0/0
          originX: 'left',
          originY: 'top'});
    }
  }

  changeMenuMode(menuMode: CanvasMenuMode) {
    this.currentCanvasMenuMode = menuMode;
    if(this.canvas){
      this.canvas.isDrawingMode = this.currentCanvasMenuMode === CanvasMenuMode.Drawing;
    }
  }

  addTextBox() {
    if(this.canvas) {
      const textBox = new fabric.Textbox('text', {
        editable: true,
        fill: this.canvas.freeDrawingBrush.color
      });
      this.canvas.add(textBox);
      this.canvas.add()
    }
  }

  removeActiveObjects() {
    if(!this.canvas) {
      return;
    }

    const activeObjects = this.canvas.getActiveObjects();
    this.canvas.remove(...activeObjects);
  }

  downloadCanvasContent() {
    if(!this.canvas) {
      return;
    }
    const dataURL = this.canvas.toDataURL({
      width: this.canvas.width,
      height: this.canvas.height,
      left: 0,
      top: 0,
      format: 'png',
    });

    const anchor = document.createElement('a')
    anchor.download = 'image.png';
    anchor.href = dataURL;
    document.body.appendChild(anchor);
    anchor.click();
    document.body.removeChild(anchor);
  }

  readUploadImage(event: Event) {
    
    const self = this;
    const inputforupload = event.target as any;
    const readerobj = new FileReader();

    readerobj.onload = function(){
      
      var imgElement = document.createElement('img');
      imgElement.src = readerobj.result?.toString() || '';

      imgElement.onload = function() {
    /** seltsam aber scheinbar muss alles in die onload Funktion gepackt werden damit die Bildbröße verfügbar ist...
        ausserhalb kommen die Variablen für die Bildgröße nicht an... **/

          console.log(imgElement.width);
          console.log(imgElement.height);

          /** Methode um ein Bild in fabric.js einzufügen **/
          var imageinstance = new fabric.Image(imgElement, {
                angle: 0,
                opacity: 1,
                cornerSize: 30,
              });
      /** Bild skalieren damit es in das Canvas Objekt reinpasst */
      /** check ob canvas portrait oder landscape format ist **/
      const cw = $(".canvas-container").width()!;
      const ch = $(".canvas-container").height()!;
      if(cw > ch){
        /** canvas ist landscape **/
        imageinstance.scaleToWidth($(".canvas-container").width()! - 200);
        imageinstance.scaleToHeight($(".canvas-container").height()! - 200);

      }else{
        /** canvas ist portrait **/
      imageinstance.scaleToHeight($(".canvas-container").height()! - 200);
      imageinstance.scaleToWidth($(".canvas-container").width()! - 200);

      }
      //imageinstance.cornerSize(40);
    //  imageinstance["cornerSize"] = parseFloat(40);
// removes the right top control
      if(!self.canvas) {
        return;
      }
      self.canvas.add(imageinstance);
      self.canvas.centerObject(imageinstance);


    };
    };

    readerobj.readAsDataURL(inputforupload.files[0]);
}

  getCanvasAsBase64String() {
    if(!this.canvas) {
      return;
    }
    const dataURL = this.canvas.toDataURL({
      width: this.canvas.width,
      height: this.canvas.height,
      left: 0,
      top: 0,
      format: 'png',
    });

    return dataURL;
  }
}
