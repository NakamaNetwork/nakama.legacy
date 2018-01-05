import { autoinject } from 'aurelia-framework';
import { DialogController } from 'aurelia-dialog';

@autoinject
export class AlertDialog {
    private controller: DialogController;
    private viewModel = new AlertDialogViewModel();

    constructor(controller: DialogController) {
        this.controller = controller;
    }

    activate(viewModel: AlertDialogViewModel) {
        if (viewModel) {
            this.viewModel = Object.assign(this.viewModel, viewModel);
        }
    }

    okay() {
        this.controller.ok();
    };

    cancel() {
        this.controller.cancel();
    };

    dismiss() {
        this.viewModel.cancelable ? this.cancel() : this.okay();
    }
}

export class AlertDialogViewModel {
    public iconClass: string = 'fa-warning';
    public title: string = 'Alert';
    public message: string;
    public okayMessage: string = 'Okay';
    public cancelMessage: string = 'Cancel';
    public cancelable: boolean;
}