export interface FilterModel {
  name: string,
  dateSort: Sort,
  nameSort: Sort,
  sizeSort: Sort,
  isRecycled: boolean,
  userId: string
}

enum Sort {
  none,
  ascending,
  descending
}
