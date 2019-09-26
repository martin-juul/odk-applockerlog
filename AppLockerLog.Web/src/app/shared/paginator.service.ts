import { Injectable } from '@angular/core';
import { Http } from '@angular/http';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Subject } from 'rxjs/Subject';

@Injectable()
export class PaginatorService {
    constructor() {}

    /*public data: Pagination;

    setPaginationData(data: Pagination) {
        this.data = data;
    }

    getPaginationData() {
        return this.data;
    }*/

    // setEntries(pageEntries: Pagination[]) {
    //     this.pageEntries = pageEntries;
    // }

    // getPaginationData(page?: number) {
    //     if (!page) {
    //         return this.pageEntries;
    //     } else {
    //         // return meta data for current page.
    //     }
    // }

}
