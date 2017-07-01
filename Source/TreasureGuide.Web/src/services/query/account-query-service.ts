import { autoinject } from 'aurelia-dependency-injection';
import { HttpEngine } from '../../tools/http-engine';

@autoinject
export class AccountQueryService {
    private http: HttpEngine;

    constructor(http: HttpEngine) {
        this.http = http;
    }
    
    getUserInfo() {
        return this.http.get('/account/userinfo');
    }
}