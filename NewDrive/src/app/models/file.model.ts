export interface FileCreateModel {
  userId: string,
  extension: string,
  name: string,
  isRecycled: boolean,
  isPublic: boolean,
  path: string
}

export interface FileViewModel {
  id: number,
  userId: string,
  createdOn: Date,
  extension: string,
  name: string,
  size: number,
  isRecycled: boolean,
  isPublic: boolean,
  path: string
}

export interface FileEditModel {
  id: number,
  userId: string,
  createdOn: Date,
  extension: string,
  name: string,
  size: number,
  isRecycled: boolean,
  isPublic: boolean,
  path: string
}