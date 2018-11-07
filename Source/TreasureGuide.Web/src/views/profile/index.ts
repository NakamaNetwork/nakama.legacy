import { autoinject, computedFrom } from 'aurelia-framework';
import { Router } from 'aurelia-router';
import { ProfileQueryService } from '../../services/query/profile-query-service';
import { TeamQueryService } from '../../services/query/team-query-service';
import { IProfileDetailModel } from '../../models/imported';
import { TeamSearchModel } from '../../services/query/team-query-service';
import { AccountService } from '../../services/account-service';
import { BindingEngine } from 'aurelia-binding';
import { MetaTool } from '../../tools/meta-tool';
import { BoxQueryService } from '../../services/query/box-query-service';
import { BoxSearchModel } from '../../services/query/box-query-service';
import { RoleConstants } from '../../models/imported';

@autoinject
export class ProfileDetailPage {
    private profileQueryService: ProfileQueryService;
    private teamQueryService: TeamQueryService;
    private router: Router;
    private accountService: AccountService;
    private boxQueryService: BoxQueryService;
    private bindingEngine: BindingEngine;

    profile: IProfileDetailModel;
    loading: boolean;

    loadingTeams: boolean;
    teamSearchModel: TeamSearchModel;
    teams: any[] = [];

    loadingBoxes: boolean;
    boxSearchModel: BoxSearchModel;
    boxes: any[] = [];

    constructor(
        profileQueryService: ProfileQueryService,
        teamQueryService: TeamQueryService,
        accountService: AccountService,
        boxQueryService: BoxQueryService,
        bindingEngine: BindingEngine,
        router: Router
    ) {
        this.profileQueryService = profileQueryService;
        this.router = router;
        this.teamQueryService = teamQueryService;
        this.teamSearchModel = new TeamSearchModel().getDefault();
        this.teamSearchModel = <TeamSearchModel>this.teamSearchModel.getCached();
        this.bindingEngine = bindingEngine;
        this.accountService = accountService;

        this.boxQueryService = boxQueryService;
    }

    activate(params) {
        this.loading = true;
        this.profileQueryService.detail(params.id).then(result => {
            this.profile = result;
            this.loading = false;
            this.teamSearchModel.submittedBy = result.id;

            this.bindingEngine.propertyObserver(this.teamSearchModel, 'payload').subscribe((n, o) => {
                this.search(n);
            });
            this.search(this.teamSearchModel.payload)
            MetaTool.setTitle(this.profile.userName);

            this.boxSearchModel = new BoxSearchModel().getDefault();
            this.boxSearchModel.userId = result.id;
            this.bindingEngine.propertyObserver(this.boxSearchModel, 'payload').subscribe((n, o) => {
                this.boxSearch(n);
            });
            this.boxSearch(this.boxSearchModel.payload);
        }).catch(error => {
            this.router.navigateToRoute('error', { error: 'The requested profile could not be found.' });
        });
    }

    @computedFrom('profile', 'profile.website')
    get websiteParsed() {
        if (!this.profile.website || this.profile.website.indexOf('//') >= 0) {
            return this.profile.website;
        }
        return '//' + this.profile.website;
    }

    @computedFrom('profile', 'profile.userRoles')
    get donor() {
        return this.profile && this.profile.userRoles.some(x => x === RoleConstants.Donor);
    }

    @computedFrom('donor')
    get donorClass() {
        return this.donor ? 'donor-user' : '';
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

    @computedFrom('profile', 'profile.userId', 'accountService.userProfile', 'accountService.userProfile.id')
    get canEdit() {
        return this.profile && this.accountService.userProfile && this.profile.id === this.accountService.userProfile.id && this.accountService.isInRoles(RoleConstants.Administrator);
    }

    boxSearch(payload) {
        if (this.boxQueryService) {
            this.loadingBoxes = true;
            this.boxQueryService.search(payload).then(x => {
                this.boxes = x.results;
                this.boxSearchModel.totalResults = x.totalResults;
                this.loadingBoxes = false;
            }).catch((e) => {
                this.loadingBoxes = false;
            });
        }
    }
}