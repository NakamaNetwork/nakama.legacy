import { autoinject } from 'aurelia-dependency-injection';
import { HttpEngine } from '../../tools/http-engine';

@autoinject
export class AccountQueryService {
    private http: HttpEngine;

    constructor(http: HttpEngine) {
        this.http = http;
    }

    getExternalLoginProviders() {
        return this.http.get('/account/GetExternalLoginProviders');
    }

    login(provider: string, returnUrl?: string) {
        return this.http.post('/account/externalLogin', { provider: provider, returnUrl: returnUrl || '/' });
    }

    register(userName: string, emailAddress: string) {
        return this.http.post('/account/register', { userName: userName, emailAddress: emailAddress });
    }

    getUserInfo() {
        return this.http.get('/account/userinfo');
    }
}