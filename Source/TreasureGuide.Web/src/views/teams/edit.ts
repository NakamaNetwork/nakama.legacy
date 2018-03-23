import { autoinject, bindable, computedFrom, BindingEngine } from 'aurelia-framework';
import { Router } from 'aurelia-router';
import { TeamQueryService } from '../../services/query/team-query-service';
import { ValidationControllerFactory, ValidationRules, ValidationController } from 'aurelia-validation';
import { BeauterValidationFormRenderer } from '../../renderers/beauter-validation-form-renderer';
import { ITeamEditorModel, ITeamUnitEditorModel, ITeamGenericSlotEditorModel, ITeamStubModel } from '../../models/imported';
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

    private controller: ValidationController;
    private bindingEngine: BindingEngine;

    title = 'Create Team';
    @bindable team: TeamEditorModel;
    loading: boolean;
    saved: boolean;

    similarLoading: boolean;
    similar: ITeamStubModel[] = [];

    constructor(teamQueryService: TeamQueryService,
        router: Router,
        alertService: AlertService,
        dialogService: DialogService,
        validFactory: ValidationControllerFactory,
        accountService: AccountService,
        bindingEngine: BindingEngine
    ) {
        this.controller = validFactory.createForCurrentScope();
        this.controller.addRenderer(new BeauterValidationFormRenderer());
        this.dialogService = dialogService;
        this.accountService = accountService;
        this.bindingEngine = bindingEngine;

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
        if (params.stageId) {
            this.team.stageId = params.stageId;
        }
        this.controller.validate();
        this.bindingEngine.propertyObserver(this, 'similarModel').subscribe((n, o) => {
            this.getSimilar(n);
        });
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
                this.team.teamGenericSlots = [];
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
        }).catch(response => this.alert.reportError(response));
    }

    get similarModel() {
        var similar = { teamId: this.team.id };
        for (var i = 0; i < 6; i++) {
            var unit = this.team.teamUnits.find(x => x.position === i && !x.sub);
            similar['unit' + (i + 1)] = unit ? unit.unitId : null;
        }
        return similar;
    }

    lastSimilar: string;

    getSimilar(model) {
        var json = JSON.stringify(model);
        if (this.lastSimilar !== json) {
            this.lastSimilar = json;
            this.similarLoading = true;
            this.teamQueryService.similar(model).then(x => {
                this.similar = x;
                this.similarLoading = false;
            }).catch(x => {
                this.similar = [];
                this.similarLoading = false;
            });
        }
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
    .ensure((x: TeamEditorModel) => x.stageId)
    .required()
    .withMessage('Please specify a stage in which to use this team.')
    .ensure((x: TeamEditorModel) => x.credits)
    .maxLength(TeamEditPage.creditMaxLength)
    .ensure((x: TeamEditorModel) => x.guide)
    .maxLength(TeamEditPage.guideMaxLength)
    .ensure((x: TeamEditorModel) => x.teamUnits)
    .required()
    .satisfies((x: ITeamUnitEditorModel[], m: TeamEditorModel) => x.filter(y => !y.sub && y.unitId).length > 1)
    .withMessage('Please include at least 2 non-generic and non-substitute units on your team!')
    .satisfies((x: ITeamUnitEditorModel[], m: TeamEditorModel) => {
        var sets = x.filter(y => y.unitId).map(y => y.position + ':' + y.unitId);
        return sets.every((y, i) => sets.indexOf(y) === i);
    })
    .withMessage('You have two of the same unit in a single slot. Please check your substitutes.')
    .satisfies((x: ITeamUnitEditorModel[], m: TeamEditorModel) => {
        return x.filter(y => !y.sub).length + m.teamGenericSlots.filter(y => !y.sub).length > 3;
    })
    .withMessage('You must have at least four non-substitute units on your team.')
    .ensure((x: TeamEditorModel) => x.teamGenericSlots)
    .required()
    .satisfies((x: ITeamGenericSlotEditorModel[], m: TeamEditorModel) => {
        var sets = x.map(y => y.position + ':' + y.role + ':' + y.type + ':' + y.class);
        return sets.every((y, i) => sets.indexOf(y) === i);
    })
    .withMessage('You have two of the same generic unit in a single slot. Please check your substitutes.')
    .on(TeamEditorModel);