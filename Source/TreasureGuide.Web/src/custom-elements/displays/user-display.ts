import { bindable, computedFrom, customElement } from 'aurelia-framework';
import { UnitQueryService } from '../../services/query/unit-query-service';

@customElement('user-display')
export class UserDisplay {
    @bindable name: string;
    @bindable unitId: number;
    @bindable donor: boolean;

    @computedFrom('unitId')
    get image() {
        return UnitQueryService.getIcon(this.unitId);
    }

    @computedFrom('donor')
    get displayClass() {
        return this.donor ? 'donor-user' : '';
    }
}