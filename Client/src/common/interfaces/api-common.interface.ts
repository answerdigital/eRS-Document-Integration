export interface IPagedResult<T> {
    currentPage: number;
    pageCount: number;
    pageSize: number;
    rowCount: number;
    results: T[];
}

export interface IRequest<T> {
    pageNumber?: number;
    filters: T;
}