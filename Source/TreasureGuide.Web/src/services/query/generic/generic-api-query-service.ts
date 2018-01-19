import { HttpEngine } from '../../../tools/http-engine';

export abstract class GenericApiQueryService {
    protected http: HttpEngine;
    protected controller: string;
    protected cached: boolean;

    constructor(controller: string, http: HttpEngine, cached?: boolean) {
        this.controller = controller;
        this.http = http;
        this.cached = cached;
    }

    get(id?: any) {
        return this.http.get(this.buildAddress('', id));
    }

    stub(id?: any) {
        if (this.cached) {
            return this.getCached('stub', id);
        } else {
            return this.http.get(this.buildAddress('stub', id));
        }
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

    buildingCache = false;

    protected getCached(endpoint: string, id?) {
        if (this.buildingCache) {
            return new Promise<any>((resolve, reject) => {
                setTimeout(() => {
                    resolve(this.getCached(endpoint, id));
                }, 100);
            });
        }
        var key = this.controller + '_' + endpoint;
        var json = sessionStorage[key];
        var result = null;
        if (json) {
            result = JSON.parse(json);
        }
        result = this.getFromCache(result, id);
        if (!result) {
            this.buildingCache = true;
            return this.http.get(this.buildAddress('stub')).then(x => {
                if (Array.isArray(x)) {
                    sessionStorage[key] = JSON.stringify(x);
                }
                this.buildingCache = false;
                return this.getFromCache(x, id);
            });
        }
        return new Promise<any>((resolve, reject) => {
            resolve(result);
        });
    }

    protected getFromCache(result: any[], id?: number) {
        if (id !== null && id !== undefined && Array.isArray(result)) {
            result = result.filter(x => {
                return x.id === id;
            });
            if (result.length === 1) {
                result = result[0];
            } else {
                result = null;
            }
        }
        return result;
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