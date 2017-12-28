import { bindable, computedFrom } from 'aurelia-framework';
import { autoinject } from 'aurelia-framework';
import { ShipQueryService } from '../services/query/ship-query-service';
import { IShipStubModel } from '../models/imported';

@autoinject
export class ShipPicker {
    private shipQueryService: ShipQueryService;
    @bindable shipId = 0;

    ship;
    ships: any[];

    constructor(shipQueryService: ShipQueryService) {
        this.shipQueryService = shipQueryService;
        this.ships = [];

        this.shipQueryService.stub().then(x => {
            this.ships = x;
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

    getIcon(id: number) {
        if (id) {
            return this.shipQueryService.getIcon(id);
        }
        return null;
    }
}