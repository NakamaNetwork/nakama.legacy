import { autoinject, computedFrom, BindingEngine } from 'aurelia-framework';
import { Router } from 'aurelia-router';
import { StageQueryService } from '../../services/query/stage-query-service';
import { TeamQueryService, TeamSearchModel } from '../../services/query/team-query-service';
import { IStageDetailModel } from '../../models/imported';

@autoinject
export class StageDetailPage {
    private stageQueryService: StageQueryService;
    private teamQueryService: TeamQueryService;
    private router: Router;

    stage: IStageDetailModel;
    loading: boolean;

    loadingTeams: boolean;
    teamSearchModel: TeamSearchModel;
    teams: any[] = [];

    constructor(stageQueryService: StageQueryService, teamQueryService: TeamQueryService, router: Router, bindingEngine: BindingEngine) {
        this.stageQueryService = stageQueryService;
        this.teamQueryService = teamQueryService;
        this.router = router;

        this.teamSearchModel = new TeamSearchModel();

        bindingEngine.propertyObserver(this.teamSearchModel, 'payload').subscribe((n, o) => {
            this.search(n);
        });
    }

    activate(params) {
        var id = params.id;
        if (id) {
            this.reload(id);
        } else {
            this.router.navigateToRoute('error', { error: 'The requested stage could not be found.' });
        }
    }

    reload(id) {
        this.loading = true;
        this.stageQueryService.detail(id).then(result => {
            this.stage = result;
            this.loading = false;
            this.teamSearchModel.stageId = result.id;
        }).catch(error => {
            this.router.navigateToRoute('error', { error: 'The requested stage could not be found.' });
        });
    }

    search(payload) {
        if (this.teamQueryService) {
            this.loadingTeams = true;
            this.teamQueryService.search(payload).then(x => {
                this.teams = x.results;
                this.teamSearchModel.totalResults = x.totalResults;
                this.loadingTeams = false;
            }).catch((e) => {
                this.loadingTeams = false;
            });
        }
    }
}