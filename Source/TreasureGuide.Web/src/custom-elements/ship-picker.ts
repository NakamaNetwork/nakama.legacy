import { bindable } from 'aurelia-framework';
import { autoinject } from 'aurelia-framework';
import { DialogController } from 'aurelia-dialog';
import { ShipQueryService } from '../services/query/ship-query-service';

@autoinject
export class ShipPicker {
    private controller: DialogController;
    private shipQueryService: ShipQueryService;
    @bindable shipId = 0;

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