import { Injectable } from '@angular/core';
import { HttpClient, HttpEvent, HttpParams, HttpRequest } from '@angular/common/http';
import { saveAs } from 'file-saver';
import { JwtHelperService } from '@auth0/angular-jwt';
import { FileCreateModel, FileViewModel } from '../models/file.model';
import { UserService } from './user.service';
import { Observable } from 'rxjs';
import { FilterModel } from '../models/filter.model';

@Injectable()
export class FileService {
  private url = "https://localhost:7083/api/items";

  constructor(private http: HttpClient) { }

  getAllFiles(filter: FilterModel) {
    let params = new HttpParams();
    params = params.append('name', filter.name);
    if (filter.isRecycled != undefined) params = params.append('isRecycled', filter.isRecycled);
    if (filter.isPublic != undefined) params = params.append('isPublic', filter.isPublic);
    params = params.append('dateSort', filter.dateSort);
    params = params.append('nameSort', filter.nameSort);
    params = params.append('sizeSort', filter.sizeSort);
    if (filter.userId != undefined) params = params.append('userId', filter.userId);
    return this.http.get<FileViewModel[]>(this.url, { params: params });
  }

  userFiles(filter: FilterModel) {
    let params = new HttpParams();
    params = params.append('name', filter.name);
    if (filter.isRecycled != undefined) params = params.append('isRecycled', filter.isRecycled);
    if (filter.isPublic != undefined) params = params.append('isPublic', filter.isPublic);
    params = params.append('dateSort', filter.dateSort);
    params = params.append('nameSort', filter.nameSort);
    params = params.append('sizeSort', filter.sizeSort);
    if (filter.userId != undefined) params = params.append('userId', filter.userId);
    return this.http.get<FileViewModel[]>(this.url + '/user', { params: params });
  }

  downloadFile(fileId: number){
    return this.http.get(this.url + '/download/' + fileId, { responseType: 'blob' });
  }

  downloadPublicFile(fileId: number) {
    return this.http.get(this.url + '/download/shared/' + fileId, { responseType: 'blob' });
  }

  recycleFile(fileId: number){
    return this.http.put(this.url + '/' + fileId + '/recycle', null);
  }

  changeFileVisibility(fileId: number){
    return this.http.put(this.url + '/' + fileId + '/changePublic', null);
  }

  getRecycledFiles(){
    return this.http.get<FileViewModel[]>(this.url, {params: new HttpParams().set('isRecycled', true)})
  }

  restoreFile(fileId: number) {
    return this.http.put(this.url + '/' + fileId + '/restore', null);
  }

  deleteFile(fileId: number) {
    return this.http.delete(this.url + '/delete/' + fileId);
  }

}
