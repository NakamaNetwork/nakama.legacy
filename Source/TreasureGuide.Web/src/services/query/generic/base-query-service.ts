import { HttpEngine } from '../../../tools/http-engine';

export class BaseQueryService {
    protected http: HttpEngine;
    protected controller: string;

    constructor(controller: string, http: HttpEngine) {
        this.http = http;
        this.controller = controller;
    }

    protected buildAddress(endpoint: string, id?): string {
        var address = '/api/' + this.controller;
        if (id !== null && id !== undefined) {
            address += '/' + id;
        }
        if (endpoint) {
            address += '/' + endpoint;
        }
        return address;
    }
}