import { autoinject, computedFrom } from 'aurelia-framework';
import { Router } from 'aurelia-router';
import { TeamQueryService } from '../../services/query/team-query-service';
import { ITeamDetailModel } from '../../models/imported';
import { CalcParser } from '../../tools/calc-parser';
import { DialogService } from 'aurelia-dialog';
import { VideoPicker } from '../../custom-elements/dialogs/video-picker';
import { ITeamVideoModel } from '../../models/imported';
import { AlertService } from '../../services/alert-service';
import * as moment from 'moment';
import { MetaService } from '../../services/meta-service';
import {MetaTool} from '../../tools/meta-tool';

@autoinject
export class TeamDetailPage {
    private teamQueryService: TeamQueryService;
    private router: Router;
    private dialogService: DialogService;
    private alertService: AlertService;
    private metaService: MetaService;

    team: ITeamDetailModel;
    loading: boolean;

    constructor(teamQueryService: TeamQueryService, router: Router, dialogService: DialogService, alertService: AlertService, metaService: MetaService) {
        this.teamQueryService = teamQueryService;
        this.router = router;
        this.dialogService = dialogService;
        this.alertService = alertService;
        this.metaService = metaService;
    }

    @computedFrom('team', 'team.teamUnits', 'team.teamShip')
    get calcLink() {
        if (this.team) {
            return CalcParser.export(this.team);
        }
        return '';
    }

    @computedFrom('team', 'team.teamVideos', 'team.submittedById')
    get sortedVideos() {
        return this.team.teamVideos.sort((a, b) => {
            return moment(b.submittedDate).diff(moment(a.submittedDate));
        });
    }

    @computedFrom('sortedVideos', 'team', 'team.submittedById')
    get ownerVideos() {
        return this.team.teamVideos.filter(x => x.userId === this.team.submittedById);
    }

    @computedFrom('sortedVideos', 'team', 'team.submittedById')
    get otherVideos() {
        return this.team.teamVideos.filter(x => x.userId !== this.team.submittedById);
    }

    submitVideo() {
        this.dialogService.open({ viewModel: VideoPicker, lock: true }).whenClosed(result => {
            if (!result.wasCancelled) {
                var model = <ITeamVideoModel>{
                    teamId: this.team.id,
                    videoLink: result.output
                };
                this.teamQueryService.video(model).then(x => {
                    this.alertService.success('Successfully uploaded a video!');
                    this.reload(this.team.id);
                }).catch(x => {
                    this.alertService.success('Successfully deleted video.');
                });
            }
        });
    }

    activate(params) {
        var id = params.id;
        if (id) {
            this.reload(id);
        } else {
            this.router.navigateToRoute('error', { error: 'The requested team could not be found.' });
        }
    }

    reload(id) {
        this.loading = true;
        this.teamQueryService.detail(id).then(result => {
            this.team = result;
            this.loading = false;
            MetaTool.setTitle(this.team.name);
        }).catch(error => {
            this.router.navigateToRoute('error', { error: 'The requested team could not be found.' });
        });
    }
}