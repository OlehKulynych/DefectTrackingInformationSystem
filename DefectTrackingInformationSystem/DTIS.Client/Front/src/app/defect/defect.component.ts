import { Component, NgModule } from '@angular/core';
import { NgxGalleryModule } from 'ngx-gallery';
import { NgxGalleryOptions, NgxGalleryImage, NgxGalleryAnimation } from 'ngx-gallery';
@NgModule({
  imports: [
    NgxGalleryModule
  ],

})
export class AppModule { }
@Component({
  selector: 'app-defect',
  templateUrl: './defect.component.html',
  styleUrls: ['./defect.component.css']
})
export class DefectComponent {
  galleryOptions: NgxGalleryOptions[];
  galleryImages: NgxGalleryImage[];

  constructor() {
    this.galleryOptions = [
      {
        width: '600px',
        height: '400px',
        thumbnailsColumns: 4,
        imageAnimation: NgxGalleryAnimation.Slide
      },
      // Додаткові параметри налаштування галереї
    ];

    this.galleryImages = [
      {
        small: 'assets/img/small/image-1.jpg',
        medium: 'assets/img/medium/image-1.jpg',
        big: 'assets/img/big/image-1.jpg'
      },
      {
        small: 'assets/img/small/image-2.jpg',
        medium: 'assets/img/medium/image-2.jpg',
        big: 'assets/img/big/image-2.jpg'
      },
      // Додаткові зображення галереї
    ];
  }
}

