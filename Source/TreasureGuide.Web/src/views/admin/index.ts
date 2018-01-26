import { autoinject } from 'aurelia-framework';
import { BindingEngine } from 'aurelia-binding';
import { TeamQueryService } from '../../services/query/team-query-service';
import { HttpEngine } from '../../tools/http-engine';
import { AlertService } from '../../services/alert-service';
import { ProfileSearchModel } from '../../services/query/profile-query-service';
import { ProfileQueryService } from '../../services/query/profile-query-service';
import { AccountService } from '../../services/account-service';
import * as moment from 'moment';
import { TeamEditorModel } from '../../services/query/team-query-service';
import { ITeamUnitEditorModel } from '../../models/imported';

@autoinject
export class AdminPage {
    httpEngine;
    teamQueryService;
    alert;
    bindingEngine;
    profileQueryService;

    profiles = [];

    searchModel = new ProfileSearchModel().getCached();
    loading;

    allRoles = AccountService.allRoles;

    constructor(httpEngine: HttpEngine, teamQueryService: TeamQueryService, profileQueryService: ProfileQueryService, alertService: AlertService, bindingEngine: BindingEngine) {
        this.httpEngine = httpEngine;
        this.teamQueryService = teamQueryService;
        this.profileQueryService = profileQueryService;
        this.alert = alertService;
        this.bindingEngine = bindingEngine;
        bindingEngine.propertyObserver(this.searchModel, 'payload').subscribe((n, o) => {
            this.search(n);
        });
        this.search(this.searchModel.payload);
    }

    search(payload) {
        if (this.teamQueryService) {
            this.loading = true;
            this.profileQueryService.search(payload).then(x => {
                this.profiles = x.results;
                this.searchModel.totalResults = x.totalResults;
                this.loading = false;
            }).catch((e) => {
                this.loading = false;
            });
        }
    }

    createTeam() {
        var team = new TeamEditorModel();
        team.name = 'Random Team @' + moment().format('MM/DD/YY hh:mm:ss a');
        team.guide = '';
        team.guide = '';
        team.credits = '';
        for (var i = 0; i < 6; i++) {
            var unit = <ITeamUnitEditorModel>{
                unitId: Math.floor(Math.random() * 1000) + 1,
                position: i,
                sub: false
            };
            team.teamUnits.push(unit);
        };
        this.teamQueryService.save(team).then(x => {
            this.alert.success('Successfully created team \'' + team.name + '\'.');
        }, x => {
            this.alert.danger('Failed to create team \'' + team.name + '\'. Probably gave a bad unit or something.');
        });
    }
}