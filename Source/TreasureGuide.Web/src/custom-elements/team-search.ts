import { autoinject, bindable, computedFrom, customElement } from 'aurelia-framework';
import { TeamSearchModel } from "../services/query/team-query-service";
import { AccountService } from '../services/account-service';
import { RoleConstants } from '../models/imported';
import { BoxService } from '../services/box-service';

@autoinject
@customElement('team-search')
export class TeamSearch {
    accountService: AccountService;
    boxService: BoxService;

    @bindable
    model: TeamSearchModel;

    @bindable
    userLocked: boolean;

    @bindable
    stageLocked: boolean;

    @bindable
    boxLocked: boolean;

    constructor(accountService: AccountService, boxService: BoxService) {
        this.accountService = accountService;
        this.boxService = boxService;
    }

    bind() {
        if (this.model) {
            if (this.userLocked) {
                this.model.lockedFields.push('submittedBy');
            }
            if (this.stageLocked) {
                this.model.lockedFields.push('stageId');
            }
            if (this.boxLocked) {
                this.model.lockedFields.push('boxId');
                this.model.lockedFields.push('blacklist');
            }
        }
    }

    @computedFrom('model.boxId')
    get myBox() {
        return this.model.boxId !== undefined;
    }
    set myBox(value) {
        if (value && this.boxService.currentBox) {
            this.model.boxId = this.boxService.currentBox.id;
            this.model.blacklist = this.boxService.currentBox.blacklist;
        } else {
            this.model.boxId = undefined;
            this.model.blacklist = undefined;
        }
    }

    @computedFrom('boxService.currentBox')
    get myBoxDisabled() {
        return this.boxService.currentBox ? '' : 'disabled';
    }

    @computedFrom('boxService.currentBox')
    get myBoxTitle() {
        return this.boxService.currentBox ? 'Filter results based on your current box.' : 'You must open a box first.';
    }

    freeToPlayOptions = TeamSearchModel.freeToPlayOptions;
}