import { Component, OnInit } from '@angular/core';
import { FileUploader } from 'ng2-file-upload';

@Component({
  selector: 'app-upload',
  templateUrl: './upload.component.html',
  styleUrls: ['./upload.component.css']
})

export class UploadComponent implements OnInit {
  hasBaseDropZoneOver = false;
  baseUrl = 'https://localhost:7083/api/';
  uploader: FileUploader = new FileUploader({
    url: this.baseUrl + 'items/upload/',
    authToken: 'Bearer ' + sessionStorage.getItem('token'),
    isHTML5: true,
    removeAfterUpload: true,
    autoUpload: false,
    maxFileSize: 500 * 1024 * 1024
  });

  constructor() { }

  ngOnInit() {
    this.uploader.onBeforeUploadItem = (file) => {
      file.withCredentials = false;
    };
  }

  fileOverBase(e: any): void {
    this.hasBaseDropZoneOver = e;
  }
}
