import { bindable } from 'aurelia-framework';
import { autoinject } from 'aurelia-framework';
import { ShipQueryService } from '../services/query/ship-query-service';
import { IShipStubModel } from '../models/imported';

@autoinject
export class ShipPicker {
    private shipQueryService: ShipQueryService;
    @bindable shipId = 0;

    @bindable ship;
    ships: any[];

    constructor(shipQueryService: ShipQueryService) {
        this.shipQueryService = shipQueryService;
        this.ships = [];

        this.shipQueryService.stub().then(x => {
            this.ships = x;
            if (this.shipId != null) {
                this.ship = this.ships.find(x => x.id == this.shipId);
            }
        });
    }

    activate(viewModel) {
        this.shipId = viewModel.shipId;
    }

    shipSort(ships: IShipStubModel[]) {
        return ships.sort((a, b) => a.name.localeCompare(b.name));
    }

    shipName(ship: IShipStubModel) {
        return ship ? ship.name : '';
    }

    shipChanged(newValue: any, oldValue: any) {
        if (newValue) {
            this.shipId = newValue.id;
        } else {
            this.shipId = 0;
        }
    }

    getIcon(id: number) {
        if (id) {
            return this.shipQueryService.getIcon(id);
        }
        return null;
    }
}