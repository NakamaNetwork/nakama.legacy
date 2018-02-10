import { autoinject, bindable, computedFrom, BindingEngine } from 'aurelia-framework';
import { BoxQueryService } from '../../services/query/box-query-service';
import { AlertService } from '../../services/alert-service';
import { DialogService } from 'aurelia-dialog';
import { AlertDialog } from '../../custom-elements/dialogs/alert-dialog';
import { IBoxDetailModel } from '../../models/imported';
import { Router } from 'aurelia-router';
import { AccountService } from '../../services/account-service';

@autoinject
export class BoxDetailPage {
    public static nameMinLength = 5;
    public static nameMaxLength = 250;

    private dialogService: DialogService;
    private alertService: AlertService;
    private boxQueryService: BoxQueryService;
    private router: Router;
    private accountService: AccountService;

    private box: IBoxDetailModel;
    private loading: boolean;

    constructor(boxQueryService: BoxQueryService,
        alertService: AlertService,
        accountService: AccountService,
        dialogService: DialogService,
        router: Router
    ) {
        this.alertService = alertService;
        this.accountService = accountService;
        this.dialogService = dialogService;
        this.router = router;

        this.boxQueryService = boxQueryService;
    }

    activate(params) {
        var id = params.id;
        if (id) {
            this.loading = true;
            this.boxQueryService.detail(id).then(result => {
                this.box = result;
                this.loading = false;
            }).catch(error => {
                this.router.navigateToRoute('error', { error: 'The requested box could not be found. It may not exist or you may not have permission to view it.' });
            });
        }
    }

    @computedFrom('box', 'box.userId', 'accountService.userProfile', 'accountService.userProfile.id')
    get canEdit() {
        return this.box.userId === this.accountService.userProfile.id;
    }
}