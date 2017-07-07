import { autoinject, bindable, computedFrom, customAttribute } from 'aurelia-framework';
import { AccountService } from '../services/account-service';

@autoinject
@customAttribute('auth-req')
export class AuthReq {
    private element: Element;
    private accountService: AccountService;

    constructor(accountService: AccountService, element: Element) {
        this.accountService = accountService;
        this.element = element;
    }

    valueChanged(newValue, oldValue) {
        var visibility = this.accountService.isInRoles(newValue) ? 'visible' : 'hidden';
        (<any>this.element).style.visibility = visibility;
    }
}