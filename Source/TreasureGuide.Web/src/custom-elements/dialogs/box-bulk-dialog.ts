import { autoinject, bindable, computedFrom } from 'aurelia-framework';
import { DialogController, DialogService } from 'aurelia-dialog';
import { NumberHelper } from '../../tools/number-helper';
import { AlertDialog, AlertDialogViewModel } from './alert-dialog';
import { IBoxDetailModel, IBoxUpdateModel } from '../../models/imported';
import { AlertService } from '../../services/alert-service';
import { BoxQueryService } from '../../services/query/box-query-service';

@autoinject
export class BoxBulkDialog {
    private static NumberPattern = /\d+/g;
    private controller: DialogController;
    private dialogSerivce: DialogService;
    private alertService: AlertService;
    private boxQueryService: BoxQueryService;

    text: string = '';
    box: IBoxDetailModel;

    constructor(controller: DialogController, dialogService: DialogService, boxQueryService: BoxQueryService, alertService: AlertService) {
        this.controller = controller;
        this.dialogSerivce = dialogService;
        this.alertService = alertService;
        this.boxQueryService = boxQueryService;
    }

    activate(box: IBoxDetailModel) {
        this.box = box;
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
                this.doSubmit(model);
            }
        });
    }

    doSubmit(result: BulkDialogResultModel) {
        var action;
        var model = <IBoxUpdateModel>{ id: this.box.id, added: [], removed: [] };
        switch (result.action) {
            case BulkDialogAction.Add:
                model.added = result.ids;
                if (model.added.length === 0) {
                    this.controller.ok(model);
                    return;
                }
                action = this.boxQueryService.update(model);
                break;
            case BulkDialogAction.Remove:
                model.removed = result.ids;
                if (model.removed.length === 0) {
                    this.controller.ok(model);
                    return;
                }
                action = this.boxQueryService.update(model);
                break;
            case BulkDialogAction.Set:
                model.added = result.ids;
                action = this.boxQueryService.set(model);
                break;
        }
        if (action) {
            action.then(x => {
                this.alertService.success('The box operation has completed successfully!');
                this.controller.ok(model);
            }).catch(response => this.alertService.reportError(response));
        }
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