import { bindable, computedFrom, customElement } from 'aurelia-framework';
import { autoinject } from 'aurelia-framework';
import { ITeamStubModel } from '../models/imported';
import { AccountService } from '../services/account-service';
import { RoleConstants } from '../models/imported';

@customElement('team-box')
@autoinject
export class TeamBox {
    private accountService: AccountService;

    @bindable
    team: ITeamStubModel;

    constructor(accountService: AccountService) {
        this.accountService = accountService;
    }

    @computedFrom('team', 'team.reported', 'team.deleted')
    get teamClass() {
        var className = '';
        if (this.accountService.isInRoles([RoleConstants.Administrator, RoleConstants.Moderator])) {
            if (this.team.reported) {
                className += 'reported ';
            }
        }
        if (this.team.deleted) {
            className += 'deleted ';
        }
        if (this.team.draft) {
            className += 'draft ';
        }
        return className;
    }
}