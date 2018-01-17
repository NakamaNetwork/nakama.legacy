import { autoinject } from 'aurelia-framework';
import { HttpEngine } from '../../tools/http-engine';

@autoinject
export class HeartbeatQueryService {
    private http: HttpEngine;

    constructor(http: HttpEngine) {
        this.http = http;
    }

    heartbeat() {
        return this.http.get('/api/heartbeat');
    }
}