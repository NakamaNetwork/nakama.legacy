import { bindable, customElement } from 'aurelia-framework';
import { autoinject } from 'aurelia-framework';
import { ITeamEditorModel } from '../models/imported';

@customElement('team-display')
@autoinject
export class TeamDisplay {
    @bindable team: ITeamEditorModel;
    @bindable editable = false;
}