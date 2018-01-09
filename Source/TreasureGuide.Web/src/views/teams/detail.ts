import { autoinject } from 'aurelia-framework';
import { Router } from 'aurelia-router';
import { TeamQueryService } from '../../services/query/team-query-service';
import { ITeamDetailModel } from '../../models/imported';

@autoinject
export class TeamDetailPage {
    private teamQueryService: TeamQueryService;
    private router: Router;

    team: ITeamDetailModel;
    loading: boolean;

    constructor(teamQueryService: TeamQueryService, router: Router) {
        this.teamQueryService = teamQueryService;
        this.router = router;
    }

    activate(params) {
        var id = params.id;
        if (id) {
            this.loading = true;
            this.teamQueryService.detail(id).then(result => {
                this.team = result;
                this.loading = false;
            }).catch(error => {
                this.router.navigateToRoute('error', { error: 'The requested team could not be found.' });
            });
        } else {
            this.router.navigateToRoute('error', { error: 'The requested team could not be found.' });
        }
    }
}