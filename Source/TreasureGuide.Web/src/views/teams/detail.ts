import { autoinject, computedFrom } from 'aurelia-framework';
import { Router } from 'aurelia-router';
import { TeamQueryService } from '../../services/query/team-query-service';
import { ITeamDetailModel } from '../../models/imported';
import { CalcParser } from '../../tools/calc-parser';

@autoinject
export class TeamDetailPage {
    private teamQueryService: TeamQueryService;
    private router: Router;
    private calcParser: CalcParser;

    team: ITeamDetailModel;
    loading: boolean;

    constructor(teamQueryService: TeamQueryService, router: Router, calcParser: CalcParser) {
        this.teamQueryService = teamQueryService;
        this.router = router;
        this.calcParser = calcParser;
    }

    @computedFrom('team', 'team.teamUnits', 'team.teamShip')
    get calcLink() {
        if (this.team) {
            return this.calcParser.export(this.team);
        }
        return '';
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