import { autoinject, bindable } from 'aurelia-framework';
import { DialogController } from 'aurelia-dialog';
import { ShipQueryService } from '../../services/query/ship-query-service';

@autoinject
export class ShipPicker {
    private controller: DialogController;
    private shipQueryService: ShipQueryService;
    @bindable shipId = 0;

    ship;
    ships: any[];

    constructor(shipQueryService: ShipQueryService, controller: DialogController) {
        this.controller = controller;

        this.shipQueryService = shipQueryService;

        this.shipQueryService.stub().then(x => {
            this.ships = x.sort((a, b) => a.name.localeCompare(b.name));
        });
    }

    activate(viewModel) {
        this.shipId = viewModel.shipId;
    }

    submit() {
        this.controller.ok(this.shipId);
    };

    cancel() {
        this.controller.cancel();
    };

    clicked(shipId) {
        this.shipId = shipId;
        this.submit();
    }

    getIcon(id: number) {
        if (id) {
            return this.shipQueryService.getIcon(id);
        }
        return null;
    }
}