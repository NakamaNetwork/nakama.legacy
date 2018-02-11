import { autoinject } from 'aurelia-framework';
import { DialogController } from 'aurelia-dialog';

@autoinject
export class BoxUnitsDialog {
    private controller: DialogController;

    constructor(controller: DialogController) {
        this.controller = controller;
    }

    okay() {
        this.controller.ok();
    };

    cancel() {
        this.controller.cancel();
    };
}