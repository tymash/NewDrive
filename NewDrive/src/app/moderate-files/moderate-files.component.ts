import { Component, OnInit, TemplateRef } from '@angular/core';
import { Router } from '@angular/router';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { FileViewModel, FileEditModel } from '../models/file.model';
import { FilterModel } from '../models/filter.model';
import { UserViewModel } from '../models/user.model';
import { FileService } from '../services/file.service';
import { UserService } from '../services/user.service';

@Component({
  selector: 'app-moderate-files',
  templateUrl: './moderate-files.component.html',
  styleUrls: ['./moderate-files.component.css']
})
export class ModerateFilesComponent implements OnInit {
  isToggled!: boolean;
  files?: FileViewModel[];
  selectedFile?: FileEditModel;
  modalRef!: BsModalRef;
  filter!: FilterModel;
  users?: UserViewModel[];

  constructor(private fileService: FileService, private modalService: BsModalService,
    private userService: UserService, private router: Router) { }

  ngOnInit() {
    if (this.userService.getAuthenticatedUserRole() != "Administrator") this.router.navigate(['']);
    this.filter = {
      name: "",
      dateSort: 0,
      nameSort: 0,
      sizeSort: 0
    };

    this.getUsers();
    this.getFiles();
  }

  getFiles() {
    this.fileService.getAllFiles(this.filter).subscribe(items => this.files = items);
  }

  getUsers() {
    this.userService.getAll().subscribe(users => this.users = users);
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

  private changeVisibility(id: number) {
    this.fileService.changeFileVisibility(id).subscribe({
      next: (result) => {
        console.log(result);
      },
      error: (error) => console.log(error)
    });
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
      error: (error) => console.log(error)}
    );
  }

  private moveToRecycleBin(id: number) {
    this.fileService.recycleFile(id).subscribe({
      next: (result) => {
        console.log(result);
      },
      error: (error) => console.log(error)
    }
    );
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
    }
    );
  }

}
