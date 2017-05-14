import { autoinject } from 'aurelia-dependency-injection';
import { bindable } from 'aurelia-framework';
import { Router } from 'aurelia-router';
import { TeamQueryService, TeamEditorModel } from '../../services/query/team-query-service';
import { MdToastService, MdInputUpdateService, MaterializeFormValidationRenderer } from 'aurelia-materialize-bridge';
import { ValidationControllerFactory, ValidationRules, ValidationController } from 'aurelia-validation';

@autoinject
export class TeamEditPage {
    private teamQueryService: TeamQueryService;
    private toast: MdToastService;
    private inputUpdate: MdInputUpdateService;
    private router: Router;

    public controller: ValidationController;

    title = 'Create Team';
    @bindable team: TeamEditorModel;

    constructor(teamQueryService: TeamQueryService, router: Router, toast: MdToastService, inputUpdate: MdInputUpdateService, validFactory: ValidationControllerFactory) {
        this.controller = validFactory.createForCurrentScope();
        this.controller.addRenderer(new MaterializeFormValidationRenderer());

        this.teamQueryService = teamQueryService;
        this.toast = toast;
        this.inputUpdate = inputUpdate;
        this.router = router;

        this.team = new TeamEditorModel();
        this.inputUpdate.update();
        this.controller.validate();
    }

    activate(params) {
        var id = params.id;
        if (id) {
            this.teamQueryService.editor(id).then(result => {
                this.title = 'Edit Team';
                this.team = Object.assign(new TeamEditorModel(), result);
                this.inputUpdate.update();
                this.controller.validate();
            }).catch(error => {
                this.router.navigateToRoute('error', { error: 'The requested team could not be found for editing. It may not exist or you may not have permission to edit it.' });
            });
        }
    }

    submit() {
        this.controller.validate().then(x => {
            if (x.valid) {
                this.teamQueryService.save(this.team).then(results => {
                    this.toast.show('Successfully saved ' + this.team.name + ' to server!', 5000);
                    this.router.navigateToRoute('teamDetails', { id: results });
                }).catch(results => {
                    console.error(results);
                });
            } else {
                x.results.filter(y => !y.valid && y.message).forEach(y => {
                    this.toast.show(y.message, 5000);
                });
            }
        });
    }
}



ValidationRules
    .ensure((x: TeamEditorModel) => x.name)
    .required()
    .minLength(10)
    .maxLength(250)
    .ensure((x: TeamEditorModel) => x.description)
    .maxLength(1000)
    .ensure((x: TeamEditorModel) => x.credits)
    .maxLength(250)
    .ensure((x: TeamEditorModel) => x.guide)
    .maxLength(10000)
    .ensure((x: TeamEditorModel) => x.teamUnits)
    .required()
    .satisfies((x: any[]) => x.filter(y => !y.sub && y.id).length > 2)
    .withMessage('Must specify a full team!')
    .on(TeamEditorModel);