import { Component, OnInit, ViewChild, ElementRef, NgModule } from '@angular/core';
import { HttpEventType, HttpErrorResponse } from '@angular/common/http';
import { of } from 'rxjs';
import { catchError, map, takeWhile } from 'rxjs/operators';
import { ItemService } from '../item.service';
import { FormGroup, FormControl } from '@angular/forms';
import { Validators } from '@angular/forms';
import { DomSanitizer } from '@angular/platform-browser';

@Component({
  selector: 'app-item',
  templateUrl: './item.component.html',
  styleUrls: ['./item.component.css']
})
export class ItemComponent implements OnInit {
  @ViewChild("fileUpload", { static: false }) fileUpload: ElementRef; files = [];
  constructor(private itemService: ItemService, private sanitizer: DomSanitizer) { }
  ngOnInit(): void {
    this.getItems();
  }

  alive = true;
  itemsList: any;
  showGrid = false;

  itemForm = new FormGroup({
    itemName: new FormControl('', [Validators.required, Validators.maxLength(50)]),
    itemDescription: new FormControl('', [Validators.required, Validators.maxLength(200)]),
    itemPrice: new FormControl('', [Validators.required, Validators.min(1)])
  });

  ngDestroy() {
    this.alive = false;
  }

  /**
   * execute an event on image upload click
   */
  onUploadImageClick() {
    const fileUpload = this.fileUpload.nativeElement; fileUpload.onchange = () => {
      for (let index = 0; index < fileUpload.files.length; index++) {
        const file = fileUpload.files[index];
        this.files.push({ data: file, inProgress: false, progress: 0 });
      }
    };
    fileUpload.click();
  };

  /**
   * Perform operation on add button click
   * Add item
   */
  onAddClick() {
    const item = {
      itemName: this.itemForm.controls['itemName'].value ? this.itemForm.controls['itemName'].value : null,
      itemDescription: this.itemForm.controls['itemDescription'].value ? this.itemForm.controls['itemDescription'].value : null,
      itemPrice: this.itemForm.controls['itemPrice'].value ? this.itemForm.controls['itemPrice'].value : null
    }

    this.addItem(this.files[0], item);
  };

  /**
   * Add item
   */
  addItem(file, item: any) {
    const formData = new FormData();
    formData.append('file', file != null ? file.data : null);
    formData.append('itemData', JSON.stringify(item));
    if (file != null) {
      file.inProgress = true;
    }

    this.itemService.addItem(formData).pipe(
      map(event => {
        switch (event.type) {
          case HttpEventType.UploadProgress:
            file.progress = Math.round(event.loaded * 100 / event.total);
            break;
          case HttpEventType.Response:
            return event;
        }
      }),
      catchError((error: HttpErrorResponse) => {
        if (file != null) {
          file.inProgress = false;
        }
        return of(`upload failed.`);
      })).subscribe(
        data => this.onComplete(data, file),
        error => console.log(error)
      );
  }

  actionMessage: any;
  onComplete(data: any, file:any) {
    file.inProgress = false;
    if (typeof (data) === 'object' && data != null) {
      this.actionMessage = data["body"].Message;
      this.getItems();
    }
  }

  /**
   * Get all items i.e. grid data
   */
  getItems(): void {
    this.itemService.getItems().pipe(takeWhile(() => this.alive))
      .subscribe(response => {
        this.itemsList = response.data;
        this.itemForm.reset();

        if(this.itemsList != null)
        {
          this.itemsList.forEach(element => {
            element.ImageURL = this.sanitizer.bypassSecurityTrustUrl(response.backendServerUrl + element.ImageURL);
          });
          this.showGrid = true;
        }else{
          this.showGrid = false;
        }
        
      });
  };

  get form() { return this.itemForm.controls; }
}
