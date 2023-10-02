export interface PageView<T> {
    pagesCount: number;
    currentPage: number;
    items: T[];
}