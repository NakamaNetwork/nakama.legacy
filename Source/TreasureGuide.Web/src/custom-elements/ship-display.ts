import { bindable, computedFrom, customElement } from 'aurelia-framework';
import { autoinject } from 'aurelia-framework';
import { ShipQueryService } from '../services/query/ship-query-service';
import { DialogService } from 'aurelia-dialog';

@autoinject
@customElement('ship-display')
export class ShipDisplay {
    private element: Element;
    private shipQueryService: ShipQueryService;
    private dialogService: DialogService;

    @bindable shipId = 0;
    @bindable editable = false;
    ship;

    constructor(shipQueryService: ShipQueryService, dialogService: DialogService, element: Element) {
        this.shipQueryService = shipQueryService;
        this.element = element;
        this.dialogService = dialogService;
    }

    shipIdChanged(newValue: number, oldValue: number) {
        return this.shipQueryService.stub(newValue).then(result => {
            this.ship = result;
        }).catch(error => {
            console.error(error);
        });
    };

    @computedFrom('shipId')
    get imageUrl() {
        return this.shipQueryService.getIcon(this.shipId);
    }
}