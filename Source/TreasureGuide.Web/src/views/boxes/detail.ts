import { autoinject, bindable, computedFrom, BindingEngine } from 'aurelia-framework';
import { BoxQueryService } from '../../services/query/box-query-service';
import { AlertService } from '../../services/alert-service';
import { DialogService } from 'aurelia-dialog';
import { AlertDialog } from '../../custom-elements/dialogs/alert-dialog';
import { IBoxDetailModel, IBoxUpdateModel, IUnitStubModel } from '../../models/imported';
import { Router } from 'aurelia-router';
import { AccountService } from '../../services/account-service';
import { UnitSearchModel, UnitQueryService } from '../../services/query/unit-query-service';
import { BoxService } from '../../services/box-service';
import { BoxBulkDialog, BulkDialogResultModel, BulkDialogAction } from '../../custom-elements/dialogs/box-bulk-dialog';

@autoinject
export class BoxDetailPage {
    public static nameMinLength = 5;
    public static nameMaxLength = 250;

    private dialogService: DialogService;
    private alertService: AlertService;
    private boxQueryService: BoxQueryService;
    private unitQueryService: UnitQueryService;
    private router: Router;
    private accountService: AccountService;
    private bindingEngine: BindingEngine;
    private boxService: BoxService;

    private box: IBoxDetailModel;
    private loading: boolean;

    private searchModel: UnitSearchModel;
    private units: IUnitStubModel[] = [];
    private unitsLoading: boolean;

    constructor(boxQueryService: BoxQueryService,
        unitQueryService: UnitQueryService,
        alertService: AlertService,
        accountService: AccountService,
        dialogService: DialogService,
        router: Router,
        bindingEngine: BindingEngine,
        boxService: BoxService
    ) {
        this.alertService = alertService;
        this.accountService = accountService;
        this.unitQueryService = unitQueryService;
        this.dialogService = dialogService;
        this.router = router;
        this.bindingEngine = bindingEngine;
        this.boxService = boxService;

        this.boxQueryService = boxQueryService;
        this.searchModel = new UnitSearchModel();
    }

    activate(params) {
        var id = params.id;
        if (id) {
            this.searchModel.box = id;
            this.loading = true;
            this.boxQueryService.detail(id).then(result => {
                this.box = result;
                this.loading = false;
                this.bindingEngine.propertyObserver(this.searchModel, 'payload').subscribe((n, o) => {
                    this.search(n);
                });
                this.search(this.searchModel.payload);
            }).catch(error => {
                this.router.navigateToRoute('error', { error: 'The requested box could not be found. It may not exist or you may not have permission to view it.' });
            });
        }
    }

    search(payload: UnitSearchModel) {
        if (this.unitQueryService) {
            this.unitsLoading = true;
            this.unitQueryService.search(payload).then(x => {
                this.units = x.results;
                this.searchModel.totalResults = x.totalResults;
                this.unitsLoading = false;
            }).catch((e) => {
                this.unitsLoading = false;
            });
        }
    }

    @computedFrom('box', 'box.userId', 'accountService.userProfile', 'accountService.userProfile.id')
    get canEdit() {
        return this.box.userId === this.accountService.userProfile.id;
    }

    bulk() {
        if (this.canEdit) {
            this.dialogService.open({ viewModel: BoxBulkDialog, lock: true }).whenClosed(result => {
                if (!result.wasCancelled) {
                    var returnValue = result.output as BulkDialogResultModel;
                    var action;
                    var model = <IBoxUpdateModel>{ id: this.box.id, added: [], removed: [] };
                    switch (returnValue.action) {
                        case BulkDialogAction.Add:
                            model.added = returnValue.ids;
                            if (model.added.length === 0) {
                                return;
                            }
                            action = this.boxQueryService.update(model);
                            break;
                        case BulkDialogAction.Remove:
                            model.removed = returnValue.ids;
                            if (model.removed.length === 0) {
                                return;
                            }
                            action = this.boxQueryService.update(model);
                            break;
                        case BulkDialogAction.Set:
                            model.added = returnValue.ids;
                            action = this.boxQueryService.set(model);
                            break;
                    }
                    if (action) {
                        this.loading = true;
                        action.then(x => {
                            this.units = [];
                            this.search(this.searchModel);
                            this.alertService.success('The box operation has completed successfully!');
                            this.loading = false;
                        }).catch(x => {
                            this.alertService.danger('There was an error performing the box operation. Please try again later.');
                            this.loading = false;
                        });
                    }

                }
            });
        }
    }
}