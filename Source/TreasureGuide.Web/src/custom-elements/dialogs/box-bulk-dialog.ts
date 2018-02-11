import { autoinject, bindable, computedFrom } from 'aurelia-framework';
import { DialogController, DialogService } from 'aurelia-dialog';
import { NumberHelper } from '../../tools/number-helper';
import { AlertDialog, AlertDialogViewModel } from './alert-dialog';

@autoinject
export class BoxBulkDialog {
    private static NumberPattern = /\d+/g;
    private controller: DialogController;
    private dialogSerivce: DialogService;

    text: string = '';

    constructor(controller: DialogController, dialogService: DialogService) {
        this.controller = controller;
        this.dialogSerivce = dialogService;
    }

    @computedFrom('text')
    get parsed() {
        return (this.text.match(BoxBulkDialog.NumberPattern) || [])
            .map(x => NumberHelper.forceNumber(x))
            .filter(x => NumberHelper.isNumber(x))
            .filter((x, i, s) => s.indexOf(x) === i)
            .sort((a, b) => a - b);
    }

    add() {
        var message =
            'Are you sure you want to add all these units to the box?';
        this.submit(BulkDialogAction.Add, message);
    };

    remove() {
        var message =
            'Are you sure you want to remove all these units from the box?';
        this.submit(BulkDialogAction.Remove, message);
    };

    set() {
        var message =
            'Are you sure you want to set the contents of ' +
            'this box to the input units? This will remove ' +
            'all existing units from the box and cannot be undone!';
        this.submit(BulkDialogAction.Set, message);
    }

    submit(action: BulkDialogAction, message: string) {
        var alertModel = new AlertDialogViewModel();
        alertModel.message = message;
        alertModel.cancelable = true;
        this.dialogSerivce.open({ viewModel: AlertDialog, lock: true, model: alertModel }).whenClosed(x => {
            if (!x.wasCancelled) {
                var model = new BulkDialogResultModel();
                model.action = action;
                model.ids = this.parsed;
                this.controller.ok(model);
            }
        });
    }

    cancel() {
        this.controller.cancel();
    };
}

export class BulkDialogResultModel {
    action: BulkDialogAction;
    ids: number[];
}

export enum BulkDialogAction {
    None,
    Add,
    Remove,
    Set
}