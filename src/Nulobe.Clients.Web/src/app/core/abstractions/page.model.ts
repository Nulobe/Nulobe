export interface PageModel<T> {
    items: T[];
    pageNumber: number;
    count: number;
}