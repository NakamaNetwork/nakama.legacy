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
import { BoxUnitsDialog } from '../../custom-elements/dialogs/box-units-dialog';
import { RoleConstants } from '../../models/imported';
import { BoxDetailModel } from '../../services/query/box-query-service';
import { BoxFlagsDialog } from '../../custom-elements/dialogs/box-flags-dialog';
import { IndividualUnitFlags } from '../../models/imported';

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

    private box: BoxDetailModel;
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
        this.searchModel = new UnitSearchModel().getDefault();
        this.searchModel = <UnitSearchModel>this.searchModel.getCached();
    }

    activate(params) {
        var id = params.id;
        if (id) {
            this.refresh(id);
        }
    }

    @computedFrom('box', 'box.boxUnits')
    get featuredUnits() {
        if (this.box && this.box.boxUnits) {
            return this.box.boxUnits.filter(x => (x.flags & IndividualUnitFlags.Favorite) !== 0);
        }
        return [];
    }

    refresh(id) {
        this.loading = true;
        return this.boxQueryService.detail(id).then(result => {
            this.box = Object.assign(new BoxDetailModel(), result);
            if (this.boxService.currentBox && this.boxService.currentBox.id === result.id) {
                this.boxService.currentBox = this.box;
            }
            this.searchModel.myBox = false;
            this.searchModel.limitTo = this.box.unitIds;
            this.bindingEngine.propertyObserver(this.searchModel, 'payload').subscribe((n, o) => {
                this.search(n);
            });
            this.search(this.searchModel.payload);
            this.loading = false;
        }).catch(error => {
            this.router.navigateToRoute('error', { error: 'The requested box could not be found. It may not exist or you may not have permission to view it.' });
        });
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
        return this.box && this.accountService.userProfile && this.box.userId === this.accountService.userProfile.id && this.accountService.isInRoles(RoleConstants.BoxUser);
    }

    openUnits() {
        if (this.canEdit) {
            this.dialogService.open({ viewModel: BoxUnitsDialog, model: this.box, lock: true }).whenClosed(x => {
                if (!x.wasCancelled) {
                    this.refresh(this.box.id);
                }
            });
        }
    }

    openBulk() {
        if (this.canEdit) {
            this.dialogService.open({ viewModel: BoxBulkDialog, model: this.box, lock: true }).whenClosed(result => {
                if (!result.wasCancelled) {
                    this.refresh(this.box.id);
                }
            });
        }
    }

    openFlags() {
        if (this.canEdit) {
            this.dialogService.open({ viewModel: BoxFlagsDialog, model: this.box, lock: true }).whenClosed(result => {
                if (!result.wasCancelled) {
                    this.refresh(this.box.id);
                }
            });
        }
    }
}