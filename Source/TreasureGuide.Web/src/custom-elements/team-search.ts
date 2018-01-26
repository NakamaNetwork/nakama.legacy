import { autoinject, bindable, computedFrom, customElement } from 'aurelia-framework';
import { TeamSearchModel } from "../services/query/team-query-service";
import { AccountService } from '../services/account-service';
import { RoleConstants } from '../models/imported';

@autoinject
@customElement('team-search')
export class TeamSearch {
    accountService: AccountService;

    @bindable
    model: TeamSearchModel;

    @bindable
    userLocked: boolean;

    @bindable
    stageLocked: boolean;

    constructor(accountService: AccountService) {
        this.accountService = accountService;
    }

    freeToPlayOptions = TeamSearchModel.freeToPlayOptions;

    @computedFrom('model', 'model.submittedBy', 'accountService.userProfile', 'accountService.userProfile.id')
    get canDraft() {
        return this.accountService.userProfile && (this.model.submittedBy === this.accountService.userProfile.id
            || this.accountService.isInRoles([RoleConstants.Administrator, RoleConstants.Moderator]));
    }
}