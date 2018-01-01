import { autoinject, bindable, computedFrom } from 'aurelia-framework';
import { Router } from 'aurelia-router';
import { TeamQueryService } from '../../services/query/team-query-service';
import { ValidationControllerFactory, ValidationRules, ValidationController } from 'aurelia-validation';
import { ITeamEditorModel } from '../../models/imported';
import { TeamEditorModel } from '../../services/query/team-query-service';
import { BeauterValidationFormRenderer } from '../../renderers/beauter-validation-form-renderer';

@autoinject
export class TeamEditPage {
    public static nameMinLength = 10;
    public static nameMaxLength = 250;
    public static descriptionMaxLength = 1000;
    public static guideMaxLength = 10000;
    public static creditMaxLength = 250;

    private teamQueryService: TeamQueryService;
    private router: Router;

    public controller: ValidationController;

    title = 'Create Team';
    @bindable team: ITeamEditorModel;

    constructor(teamQueryService: TeamQueryService, router: Router, validFactory: ValidationControllerFactory) {
        this.controller = validFactory.createForCurrentScope();
        this.controller.addRenderer(new BeauterValidationFormRenderer());

        this.teamQueryService = teamQueryService;
        this.router = router;

        this.team = new TeamEditorModel();
    }

    activate(params) {
        var id = params.id;
        if (id) {
            this.teamQueryService.editor(id).then(result => {
                this.title = 'Edit Team';
                this.team = Object.assign(new TeamEditorModel(), result);
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
                    // this.toast.show('Successfully saved ' + this.team.name + ' to server!', 5000);
                    this.router.navigateToRoute('teamDetails', { id: results });
                }).catch(results => {
                    console.error(results);
                });
            } else {
                x.results.filter(y => !y.valid && y.message).forEach(y => {
                    // this.toast.show(y.message, 5000);
                });
            }
        });
    }

    @computedFrom('team.description')
    get descLength() {
        return (this.team.description || '').length + '/' + TeamEditPage.descriptionMaxLength;
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
    .ensure((x: TeamEditorModel) => x.description)
    .maxLength(TeamEditPage.descriptionMaxLength)
    .ensure((x: TeamEditorModel) => x.credits)
    .maxLength(TeamEditPage.creditMaxLength)
    .ensure((x: TeamEditorModel) => x.guide)
    .maxLength(TeamEditPage.guideMaxLength)
    .ensure((x: TeamEditorModel) => x.teamUnits)
    .required()
    .satisfies((x: any[]) => x.filter(y => !y.sub && y.unitId).length > 3)
    .withMessage('Please specify at least 3 units for this team.')
    .on(TeamEditorModel);