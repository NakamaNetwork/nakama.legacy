import { HttpEngine } from '../../../tools/http-engine';

export abstract class GenericApiQueryService {
    protected http: HttpEngine;
    protected controller: string;

    constructor(controller: string, http: HttpEngine) {
        this.controller = controller;
        this.http = http;
    }

    get(id?: any) {
        return this.http.get(this.buildAddress('', id));
    }

    stub(id?: any) {
        return this.http.get(this.buildAddress('stub', id));
    }

    detail(id?: any) {
        return this.http.get(this.buildAddress('detail', id));
    }

    editor(id?: any) {
        return this.http.get(this.buildAddress('editor', id));
    }

    save(model, id?: any) {
        return this.http.post(this.buildAddress('', id), model);
    }

    delete(id?: any) {
        return this.http.delete(this.buildAddress('', id));
    }

    protected buildAddress(endpoint: string, id?): string {
        var address = 'api/' + this.controller;
        if (id !== null && id !== undefined) {
            address += '/' + id;
        }
        if (endpoint) {
            address += '/' + endpoint;
        }
        return address;
    }
}