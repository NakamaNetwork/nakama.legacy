import { autoinject, bindable, computedFrom } from 'aurelia-framework';
import { Router } from 'aurelia-router';
import { TeamQueryService } from '../../services/query/team-query-service';
import { ValidationControllerFactory, ValidationRules, ValidationController } from 'aurelia-validation';
import { ITeamEditorModel } from '../../models/imported';
import { TeamEditorModel } from '../../services/query/team-query-service';
import { BeauterValidationFormRenderer } from '../../renderers/beauter-validation-form-renderer';
import { AlertService } from '../../services/alert-service';

@autoinject
export class TeamEditPage {
    public static nameMinLength = 10;
    public static nameMaxLength = 250;
    public static guideMaxLength = 10000;
    public static creditMaxLength = 1000;

    private teamQueryService: TeamQueryService;
    private router: Router;
    private alert: AlertService;

    public controller: ValidationController;

    title = 'Create Team';
    @bindable team: ITeamEditorModel;

    constructor(teamQueryService: TeamQueryService, router: Router, alertService: AlertService, validFactory: ValidationControllerFactory) {
        this.controller = validFactory.createForCurrentScope();
        this.controller.addRenderer(new BeauterValidationFormRenderer());

        this.teamQueryService = teamQueryService;
        this.router = router;
        this.alert = alertService;

        this.team = new TeamEditorModel();
        this.controller.addObject(this.team);
    }

    activate(params) {
        var id = params.id;
        if (id) {
            this.teamQueryService.editor(id).then(result => {
                this.title = 'Edit Team';
                this.team = Object.assign(this.team, result);
                this.controller.validate();
            }).catch(error => {
                this.router.navigateToRoute('error', { error: 'The requested team could not be found for editing. It may not exist or you may not have permission to edit it.' });
            });
        }
        this.controller.validate();
    }

    submit() {
        this.controller.validate().then(x => {
            if (x.valid) {
                this.teamQueryService.save(this.team).then(results => {
                    this.alert.success('Successfully saved ' + this.team.name + ' to server!');
                    this.router.navigateToRoute('teamDetails', { id: results.id });
                }).catch(response => {
                    return response.text().then(msg => {
                        this.alert.danger(msg);
                    }).catch(error => {
                        this.alert.danger('An error has occurred. Please try again in a few moments.');
                    });
                });
            } else {
                x.results.filter(y => !y.valid && y.message).forEach(y => {
                    this.alert.danger(y.message);
                });
            }
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