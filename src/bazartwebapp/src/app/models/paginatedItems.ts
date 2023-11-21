export interface PaginatedItems<T> {
    items: T[]
    pageNumber: number
    totalPages: number
}