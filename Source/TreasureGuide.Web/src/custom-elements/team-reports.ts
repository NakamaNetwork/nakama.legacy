import { bindable, customElement } from 'aurelia-framework';
import { autoinject } from 'aurelia-framework';
import { RoleConstants } from '../models/imported';
import { ITeamReportStubModel } from '../models/imported';
import { AccountService } from '../services/account-service';
import { TeamQueryService } from '../services/query/team-query-service';
import { AlertService } from '../services/alert-service';

@customElement('team-reports')
@autoinject
export class TeamReports {
    private accountService: AccountService;
    private teamQueryService: TeamQueryService;
    private alertService: AlertService;

    @bindable
    team: number;

    reports: ITeamReportStubModel[] = [];

    constructor(accountService: AccountService, teamQueryService: TeamQueryService, alertService: AlertService) {
        this.accountService = accountService;
        this.teamQueryService = teamQueryService;
        this.alertService = alertService;
    }

    activate() {
    }

    teamChanged(newValue, oldValue) {
        if (newValue.reported &&
            this.accountService.isInRoles([RoleConstants.Administrator, RoleConstants.Moderator])) {
            this.getReports(newValue.id);
        }
    }

    getReports(id) {
        this.teamQueryService.reports(id).then(result => {
            this.reports = result;
        });
    }

    acknowledge(id) {
        this.teamQueryService.acknowledgeReport(id).then(result => {
            this.alertService.success('Report has been acknowledged.');
            var report = this.reports.find(x => x.id == id);
            if (report) {
                report.acknowledged = true;
            }
        });
    }
}