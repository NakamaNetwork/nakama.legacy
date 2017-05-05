import { autoinject } from 'aurelia-dependency-injection';
import { Router } from 'aurelia-router';
import { TeamQueryService } from '../../services/query/team-query-service';
import { MdToastService, MdInputUpdateService } from 'aurelia-materialize-bridge';

@autoinject
export class TeamEditPage {
    private teamQueryService: TeamQueryService;
    private toast: MdToastService;
    private inputUpdate: MdInputUpdateService;
    private router: Router;

    title = 'Create Team';
    team = {
        name: '',
        description: '',
        guide: '',
        credits: '',
        teamUnits: [],
        teamSockets: []
    };

    constructor(teamQueryService: TeamQueryService, router: Router, toast: MdToastService, inputUpdate: MdInputUpdateService) {
        this.teamQueryService = teamQueryService;
        this.toast = toast;
        this.inputUpdate = inputUpdate;
        this.router = router;
    }

    activate(params) {
        var id = params.id;
        if (id) {
            this.teamQueryService.editor(id).then(result => {
                this.title = 'Edit Stage';
                this.team = result;
                this.inputUpdate.update();
            }).catch(error => {
                this.router.navigateToRoute('error', { error: 'The requested team could not be found for editing. It may not exist or you may not have permission to edit it.' });
            });
        }
    }

    submit() {
        this.teamQueryService.save(this.team).then(results => {
            this.toast.show('Successfully saved ' + this.team.name + ' to server!', 2000);
            this.router.navigateToRoute('teamEdit', { id: results });
        }).catch(results => {
            console.error(results);
        });
    }
}