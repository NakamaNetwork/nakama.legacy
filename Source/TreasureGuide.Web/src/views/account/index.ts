import { autoinject, computedFrom } from 'aurelia-framework';
import { Router } from 'aurelia-router';
import { ProfileQueryService } from '../../services/query/profile-query-service';
import { TeamQueryService } from '../../services/query/team-query-service';
import { IProfileDetailModel } from '../../models/imported';
import { TeamSearchModel } from '../../services/query/team-query-service';
import { AccountService } from '../../services/account-service';
import { BindingEngine } from 'aurelia-binding';
import { MetaService } from '../../services/meta-service';

@autoinject
export class ProfileDetailPage {
    private profileQueryService: ProfileQueryService;
    private teamQueryService: TeamQueryService;
    private router: Router;
    private accountService: AccountService;
    private metaService: MetaService;

    profile: IProfileDetailModel;
    loading: boolean;

    loadingTeams: boolean;
    teamSearchModel: TeamSearchModel;
    teams: any[] = [];

    constructor(profileQueryService: ProfileQueryService, teamQueryService: TeamQueryService, accountService: AccountService, bindingEngine: BindingEngine, router: Router, metaService: MetaService) {
        this.profileQueryService = profileQueryService;
        this.router = router;
        this.teamQueryService = teamQueryService;
        this.teamSearchModel = new TeamSearchModel();
        this.teamSearchModel.cacheKey = 'search-team-user';
        this.metaService = metaService;

        bindingEngine.propertyObserver(this.teamSearchModel, 'payload').subscribe((n, o) => {
            this.search(n);
        });
    }

    activate(params) {
        this.loading = true;
        this.profileQueryService.detail(params.id).then(result => {
            this.profile = result;
            this.loading = false;
            this.teamSearchModel.submittedBy = result.id;
            this.metaService.setTitle(this.profile.userName);
        }).catch(error => {
            this.router.navigateToRoute('error', { error: 'The requested account could not be found.' });
        });
    }

    @computedFrom('profile', 'profile.website')
    get websiteParsed() {
        if (!this.profile.website || this.profile.website.indexOf('//') >= 0) {
            return this.profile.website;
        }
        return '//' + this.profile.website;
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