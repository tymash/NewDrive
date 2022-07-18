import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { FileService } from '../services/file.service';

@Component({
  selector: 'app-shared-download',
  templateUrl: './shared-download.component.html',
  styleUrls: ['./shared-download.component.css']
})
export class SharedDownloadComponent implements OnInit {
  fileName!: string;
  result: boolean = true;

  constructor(private route: ActivatedRoute, private fileService: FileService) {
    this.route.params.subscribe(params => {
      if (params['id'] && params['name']) {
        this.fileName = params['name'];
        this.downloadPublicItems(params['id'], params['name']);
      }
    }
    );
   }

  ngOnInit(): void {
  }

  downloadPublicItems(id: number, name: string) {
    this.fileService.downloadPublicFile(id)
      .subscribe({
        next: (data) => {
          const downloadedFile = new Blob([data], { type: data.type });
          const a = document.createElement('a');
          a.setAttribute('style', 'display:none;');
          document.body.appendChild(a);
          a.download = name;
          a.href = URL.createObjectURL(downloadedFile);
          a.target = '_blank';
          a.click();
          document.body.removeChild(a);
        },
        error: (error) => {
          console.log(error);
          this.result = false;
        }
      });
  }

}
