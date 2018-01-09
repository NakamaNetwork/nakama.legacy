import { autoinject, bindable, customElement } from 'aurelia-framework';
import { TeamSearchModel } from "../services/query/team-query-service";

@autoinject
@customElement('team-search')
export class TeamSearch {
    @bindable
    model: TeamSearchModel;

    @bindable
    userLocked: boolean;
}