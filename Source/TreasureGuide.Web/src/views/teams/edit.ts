import { autoinject, bindable, computedFrom } from 'aurelia-framework';
import { Router } from 'aurelia-router';
import { TeamQueryService } from '../../services/query/team-query-service';
import { ValidationControllerFactory, ValidationRules, ValidationController } from 'aurelia-validation';
import { BeauterValidationFormRenderer } from '../../renderers/beauter-validation-form-renderer';
import { ITeamEditorModel } from '../../models/imported';
import { TeamEditorModel } from '../../services/query/team-query-service';
import { AlertService } from '../../services/alert-service';
import { TeamImportView } from '../../custom-elements/dialogs/team-import';
import { DialogService } from 'aurelia-dialog';
import { AlertDialog, AlertDialogViewModel } from '../../custom-elements/dialogs/alert-dialog';
import { AccountService } from '../../services/account-service';

@autoinject
export class TeamEditPage {
    public static nameMinLength = 10;
    public static nameMaxLength = 250;
    public static guideMaxLength = 40000;
    public static creditMaxLength = 2000;

    private teamQueryService: TeamQueryService;
    private router: Router;
    private alert: AlertService;
    private dialogService: DialogService;
    private accountService: AccountService;

    public controller: ValidationController;

    title = 'Create Team';
    @bindable team: ITeamEditorModel;
    loading: boolean;
    saved: boolean;

    constructor(teamQueryService: TeamQueryService,
        router: Router,
        alertService: AlertService,
        dialogService: DialogService,
        validFactory: ValidationControllerFactory,
        accountService: AccountService
    ) {
        this.controller = validFactory.createForCurrentScope();
        this.controller.addRenderer(new BeauterValidationFormRenderer());
        this.dialogService = dialogService;
        this.accountService = accountService;

        this.teamQueryService = teamQueryService;
        this.router = router;
        this.alert = alertService;

        this.team = new TeamEditorModel();
        this.controller.addObject(this.team);
    }

    activate(params) {
        var id = params.id;
        if (id) {
            this.loading = true;
            this.teamQueryService.editor(id).then(result => {
                this.title = 'Edit Team';
                this.team = Object.assign(this.team, result);
                this.controller.validate();
                this.loading = false;
            }).catch(error => {
                this.router.navigateToRoute('error', { error: 'The requested team could not be found for editing. It may not exist or you may not have permission to edit it.' });
            });
        }
        this.controller.validate();
    }

    canDeactivate() {
        return new Promise((resolve, reject) => {
            if (!this.checkDirty()) {
                return resolve();
            }
            this.dialogService.open({
                viewModel: AlertDialog,
                model: <AlertDialogViewModel>{
                    message: 'Are you sure you want to leave this page? Any unsaved changes will be lost!',
                    cancelable: true
                },
                lock: true
            }).whenClosed(result => {
                if (!result.wasCancelled) {
                    return resolve();
                }
                return reject();
            });
        });
    }

    openImport() {
        this.dialogService.open({ viewModel: TeamImportView, lock: true }).whenClosed(result => {
            if (!result.wasCancelled) {
                this.team.teamUnits = result.output.team;
                this.team.shipId = result.output.ship;
            }
        });
    };

    submit() {
        this.controller.validate().then(x => {
            if (x.valid) {
                if (this.team.deleted) {
                    var message = 'Are you sure you want to delete this team?';
                    this.dialogService.open({ viewModel: AlertDialog, model: { message: message, cancelable: true }, lock: true }).whenClosed(x => {
                        if (!x.wasCancelled) {
                            this.doSubmit();
                        }
                    });
                } else {
                    this.doSubmit();
                }
            } else {
                x.results.filter(y => !y.valid && y.message).forEach(y => {
                    this.alert.danger(y.message);
                });
            }
        });
    }

    checkDirty() {
        if (!this.accountService.isLoggedIn || this.saved) {
            return false;
        }
        return true;
    }

    doSubmit() {
        this.teamQueryService.save(this.team).then(results => {
            this.saved = true;
            if (this.team.deleted) {
                this.alert.success('Successfully deleted ' + this.team.name + '.');
                this.router.navigateToRoute('teams');
            } else {
                this.alert.success('Successfully saved ' + this.team.name + '!');
                this.router.navigateToRoute('teamDetails', { id: results.id });
            }
        }).catch(response => {
            return response.text().then(msg => {
                if (msg) {
                    this.alert.danger(msg);
                } else {
                    this.alert.danger('An error has occurred. Please try again in a few moments.');
                }
            }).catch(error => {
                this.alert.danger('An error has occurred. Please try again in a few moments.');
            });
        });
    }

    @computedFrom('team.name')
    get nameLength() {
        return (this.team.name || '').length + '/' + TeamEditPage.nameMaxLength;
    }

    @computedFrom('team.credits')
    get creditLength() {
        return (this.team.credits || '').length + '/' + TeamEditPage.creditMaxLength;
    }

    @computedFrom('team.guide')
    get guideLength() {
        return (this.team.guide || '').length + '/' + TeamEditPage.guideMaxLength;
    }
}

ValidationRules
    .ensure((x: TeamEditorModel) => x.name)
    .required()
    .minLength(TeamEditPage.nameMinLength)
    .maxLength(TeamEditPage.nameMaxLength)
    .ensure((x: TeamEditorModel) => x.credits)
    .maxLength(TeamEditPage.creditMaxLength)
    .ensure((x: TeamEditorModel) => x.guide)
    .maxLength(TeamEditPage.guideMaxLength)
    .ensure((x: TeamEditorModel) => x.teamUnits)
    .required()
    .satisfies((x: any[], m: TeamEditorModel) => x.filter(y => !y.sub && y.unitId).length > 3)
    .withMessage('Please include at least 4 units on your team!')
    .on(TeamEditorModel);