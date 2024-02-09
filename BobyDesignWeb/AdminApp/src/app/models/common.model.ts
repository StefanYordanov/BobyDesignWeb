export interface PageView<T> {
    pagesCount: number;
    currentPage: number;
    items: T[];
}

export interface DateOnlyModel {
  year: number;
  month: number;
  day: number
}