import { autoinject } from 'aurelia-framework';
import { HttpEngine } from '../../tools/http-engine';

@autoinject
export class ProfileQueryService {
    private http: HttpEngine;

    constructor(http: HttpEngine) {
        this.http = http;
    }
    
    getProfile() {
        return this.http.get('/api/profile');
    }

    logout() {
        return this.http.post('/account/logout');
    }
}