import { bindable, computedFrom, customElement } from 'aurelia-framework';
import { autoinject } from 'aurelia-framework';
import { ShipQueryService } from '../services/query/ship-query-service';
import { DialogService } from 'aurelia-dialog';
import { ShipPicker } from './dialogs/ship-picker';

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
        return this.shipQueryService.get(newValue).then(result => {
            this.ship = result;
        }).catch(error => {
            console.error(error);
        });
    };

    @computedFrom('shipId')
    get imageUrl() {
        return ShipQueryService.getIcon(this.shipId);
    }

    @computedFrom('ship', 'ship.eventShip', 'ship.eventShipActive')
    get iconClass() {
        return this.ship ? this.ship.eventShipActive ? 'fa-star' : this.ship.eventShip ? 'fa-star-o' : 'fa-ship' : '';
    }

    clicked() {
        if (this.editable) {
            this.dialogService.open({ viewModel: ShipPicker, model: { shipId: this.shipId }, lock: true }).whenClosed(result => {
                if (!result.wasCancelled) {
                    this.shipId = result.output;
                }
            });
        }
    }
}