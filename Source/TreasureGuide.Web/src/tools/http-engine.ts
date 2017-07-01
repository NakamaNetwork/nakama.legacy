import { autoinject } from 'aurelia-dependency-injection';
import { HttpClient, json } from 'aurelia-fetch-client';

@autoinject
export class HttpEngine {
    private http: HttpClient;

    constructor(http: HttpClient) {
        this.http = http;
    }

    parameterize(endpoint: string, params: any) {
        var first = true;
        for (var property in params) {
            if (params.hasOwnProperty(property)) {
                var value = params[property];
                if (typeof (value) !== 'function' && value !== undefined && value !== null) {
                    endpoint += first ? '?' : '&';
                    endpoint += encodeURIComponent(property);
                    endpoint += '=';
                    endpoint += encodeURIComponent(value);
                    first = false;
                }
            }
        }
        return endpoint;
    }

    buildSecurityHeaders(accessToken?: string): Headers {
        var headers = new Headers();
        accessToken = accessToken || sessionStorage['access_token'];
        if (accessToken) {
            headers.append('Authorization', 'Bearer ' + accessToken);
        }
        return headers;
    };

    private execute(endpoint: string, data: any, headers: Headers, method: string): Promise<any> {
        headers = headers || this.buildSecurityHeaders();
        headers.set('content-type', 'application/json');
        headers.set('Accept', 'application/json');
        var options = {
            method: method || 'GET',
            body: data ? JSON.stringify(data) : null,
            headers: headers
        };
        return this.http.fetch(endpoint, options).then(response => {
            if (response.status >= 200 && response.status < 400) {
                return response.json().catch(error => {
                    throw error;
                });
            }
            throw response;
        });
    };

    get(endpoint: string, headers?: Headers) {
        return this.execute(endpoint, null, headers, 'GET');
    };

    post(endpoint: string, data?: any, headers?: Headers) {
        return this.execute(endpoint, data, headers, 'POST');
    };

    put(endpoint: string, data?: any, headers?: Headers) {
        return this.execute(endpoint, data, headers, 'PUT');
    };

    delete(endpoint: string, data?: any, headers?: Headers) {
        return this.execute(endpoint, data, headers, 'PUT');
    };
}