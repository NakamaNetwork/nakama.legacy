import { autoinject } from 'aurelia-dependency-injection';
import { Router } from 'aurelia-router';
import { TeamQueryService } from '../../services/query/team-query-service';

@autoinject
export class TeamEditPage {
    private teamQueryService: TeamQueryService;
    private router: Router;

    title = 'Create Team';
    team = {
        name: '',
        description: '',
        credits: '',
        teamUnits: [],
        teamSockets: []
    };

    constructor(teamQueryService: TeamQueryService, router: Router) {
        this.teamQueryService = teamQueryService;
        this.router = router;
    }

    activate(params) {
        var id = params.id;
        if (params.id) {
            this.teamQueryService.stub().then(result => {
                this.team = result;
            }).catch(error => {
                this.router.navigateToRoute('error', { error: 'The requested team could not be found for editing. It may not exist or you may not have permission to edit it.' });
            });
        }
    }

    submit() {
        this.teamQueryService.save(this.team).then(results => {
            this.router.navigateToRoute('teamEdit', { id: results });
        }).catch(results => {
            console.error(results);
        });
    }
}