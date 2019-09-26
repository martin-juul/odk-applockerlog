export class Pagination {
    constructor(
       public totalCount: number,
       public totalPages: number,
       public prevLink?: string,
       public nextLink?: string,
    ) {}
}
