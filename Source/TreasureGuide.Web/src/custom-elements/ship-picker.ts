import { bindable } from 'aurelia-framework';
import { autoinject } from 'aurelia-dependency-injection';
import { DialogController } from 'aurelia-dialog';
import { ShipQueryService } from '../services/query/ship-query-service';

@autoinject
export class ShipPicker {
    private controller: DialogController;
    private shipQueryService: ShipQueryService;
    @bindable unitId = 0;

    ship;
    ships: any[];

    resultCount = 0;
    pages = 0;

    constructor(shipQueryService: ShipQueryService, controller: DialogController) {
        this.controller = controller;
        this.controller.settings.centerHorizontalOnly = true;
        this.shipQueryService = shipQueryService;

        this.shipQueryService.stub().then(x => {
            this.ships = x;
        });
    }

    activate(viewModel) {
        this.unitId = viewModel.unitId;
    }

    submit() {
        this.controller.ok(this.ship.id);
    };

    cancel() {
        this.controller.cancel();
    };

    clicked(ship) {
        this.ship = ship;
        this.submit();
    }

    getIcon(id: number) {
        if (id) {
            return this.shipQueryService.getIcon(id);
        }
        return null;
    }
}