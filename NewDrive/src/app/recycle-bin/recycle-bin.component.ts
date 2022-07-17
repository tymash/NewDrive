import { Component, OnInit, TemplateRef } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { FileEditModel, FileViewModel } from '../models/file.model';
import { FilterModel } from '../models/filter.model';
import { FileService } from '../services/file.service';

@Component({
  selector: 'app-recycle-bin',
  templateUrl: './recycle-bin.component.html',
  styleUrls: ['./recycle-bin.component.css']
})
export class RecycleBinComponent implements OnInit {
  recycledFiles!: FileViewModel[];
  isToggled: boolean = false;
  filter!: FilterModel;
  modalRef!: BsModalRef;
  selectedFile?: FileViewModel;

  constructor(private fileService: FileService,
    private modalService: BsModalService) { }

  ngOnInit() {
    this.filter = {
      isRecycled: true,
      name: "",
      dateSort: 0,
      nameSort: 0,
      sizeSort: 0,
      userId: ""
    };

    this.getAllRecycledItems();
  }

  getAllRecycledItems() {
    this.fileService.userFiles(this.filter).subscribe(items => this.recycledFiles = items);
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

  openModal(template: TemplateRef<any>, item: FileEditModel) {
    this.selectedFile = item;
    this.modalRef = this.modalService.show(template, { class: 'modal-sm' });
  }

  confirmRestore(): void {
    this.restoreItem(this.selectedFile!.id);
    this.selectedFile = undefined;
    this.modalRef.hide();
    window.location.reload();
  }

  private restoreItem(id: number) {
    this.fileService.restoreFile(id).subscribe({
      next: (result) => {
        console.log(result);
      },
      error: (error) => console.log(error)
    });
  }

  confirmDelete(): void {
    this.deleteItem(this.selectedFile!.id);
    this.selectedFile = undefined;
    this.modalRef.hide();
    window.location.reload();
  }

  private deleteItem(id: number) {
    this.fileService.deleteFile(id).subscribe({
      next: (result) => {
        console.log(result);
      },
      error: (error) => console.log(error)
    });
  }

  decline(): void {
    this.modalRef.hide();
  }
}
