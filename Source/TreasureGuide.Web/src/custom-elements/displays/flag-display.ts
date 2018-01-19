import { bindable, customElement } from 'aurelia-framework';
import { ITeamStubModel } from '../../models/imported';

@customElement('flag-display')
export class FlagDisplay {
    @bindable team: ITeamStubModel;
}