import { Component, OnInit, TemplateRef } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { FileEditModel, FileViewModel } from '../models/file.model';
import { FilterModel } from '../models/filter.model';
import { FileService } from '../services/file.service';

@Component({
  selector: 'app-public',
  templateUrl: './public.component.html',
  styleUrls: ['./public.component.css']
})

export class PublicComponent implements OnInit {
  isToggled!: boolean;
  files!: FileViewModel[];
  selectedFile?: FileEditModel;
  modalRef!: BsModalRef;
  filter!: FilterModel;

  constructor(private fileService: FileService, private modalService: BsModalService) { }

  ngOnInit() {
    console.log("a");
    this.filter = {
      isRecycled: false,
      isPublic: true,
      name: "",
      dateSort: 0,
      nameSort: 0,
      sizeSort: 0,
      userId: ""
    };

    this.getFiles();
  }

  getFiles() {
    this.fileService.userFiles(this.filter).subscribe(items => this.files = items);
  }

  toggle() {
    this.isToggled = !this.isToggled;
  }

  resetFilters() {
    this.filter.name = "";
    this.filter.dateSort = 0;
    this.filter.sizeSort = 0;
    this.filter.nameSort = 0;
  }

  openModal(template: TemplateRef<any>, file: FileEditModel) {
    this.selectedFile = file;
    this.modalRef = this.modalService.show(template, { class: 'modal-dialog-centered' });
  }

  confirmRecycling(): void {
    this.moveToRecycleBin(this.selectedFile!.id);
    this.selectedFile = undefined;
    this.modalRef.hide();
    window.location.reload();
  }

  confirmVisChange(): void {
    this.changeVisibility(this.selectedFile!.id);
    this.selectedFile = undefined;
    this.modalRef.hide();
    window.location.reload();
  }

  decline(): void {
    this.modalRef.hide();
  }

  downloadActualItems(id: number, name: string) {
    this.fileService.downloadFile(id)
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
        }
      });
  }

  private moveToRecycleBin(id: number) {
    this.fileService.recycleFile(id).subscribe({
      next: (result) => {
        console.log(result);
      },
      error: (error) => console.log(error)
    });
  }

  private changeVisibility(id: number) {
    this.fileService.changeFileVisibility(id).subscribe({
      next: (result) => {
        console.log(result);
      },
      error: (error) => console.log(error)
    });
  }
}
