import { bindable, customElement } from 'aurelia-framework';
import { autoinject } from 'aurelia-framework';
import {ITeamStubModel} from '../models/imported';

@customElement('team-box')
@autoinject
export class TeamBox {
    private element: Element;

    @bindable
    team: ITeamStubModel;

    constructor(element: Element) {
        this.element = element;
    }
}