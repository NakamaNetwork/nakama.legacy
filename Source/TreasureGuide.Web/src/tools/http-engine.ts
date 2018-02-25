import { autoinject } from 'aurelia-framework';
import { HttpClient, json } from 'aurelia-fetch-client';

@autoinject
export class HttpEngine {
    private http: HttpClient;

    constructor(http: HttpClient) {
        this.http = http;
        this.http.configure(config => {
            config.withDefaults({
                headers: {
                    'content-type': 'application/json',
                    'accept-encoding': 'application/gzip',
                    'Accept': 'application/json'
                }
            });
        });
    }

    parameterize(endpoint: string, params: any) {
        var first = true;
        for (var property in params) {
            if (params.hasOwnProperty(property)) {
                var value = params[property];
                if (typeof (value) !== 'function' && value !== null && value !== undefined && (!value.hasOwnProperty("length") || value.length > 0)) {
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

    private execute(endpoint: string, data: any, headers: Headers, method: string, debounce: boolean = false): Promise<any> {
        headers = headers || this.buildSecurityHeaders();
        var options = {
            method: method || 'GET',
            headers: headers
        };
        if (data) {
            options['body'] = JSON.stringify(data);
        }
        return this.http.fetch(endpoint, options).then(response => {
            if (response.status >= 200 && response.status < 400) {
                return response.json().catch(error => {
                    throw error;
                });
            } else {
                return response.text().catch(error => {
                    throw response;
                });
            }
        });
    };

    get(endpoint: string, headers?: Headers, debounce?: boolean) {
        return this.execute(endpoint, null, headers, 'GET', debounce);
    };

    post(endpoint: string, data?: any, headers?: Headers, debounce?: boolean) {
        return this.execute(endpoint, data, headers, 'POST', debounce);
    };

    put(endpoint: string, data?: any, headers?: Headers, debounce?: boolean) {
        return this.execute(endpoint, data, headers, 'PUT', debounce);
    };

    delete(endpoint: string, data?: any, headers?: Headers, debounce?: boolean) {
        return this.execute(endpoint, data, headers, 'DELETE', debounce);
    };
}