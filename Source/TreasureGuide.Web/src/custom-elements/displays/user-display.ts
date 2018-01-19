import { bindable, computedFrom, customElement } from 'aurelia-framework';
import {UnitQueryService} from '../../services/query/unit-query-service';

@customElement('user-display')
export class UserDisplay {
    @bindable name: string;
    @bindable unitId: number;

    @computedFrom('unitId')
    get image() {
        return UnitQueryService.getIcon(this.unitId);
    }
}