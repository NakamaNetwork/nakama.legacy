﻿import { autoinject } from 'aurelia-framework';
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
import { DonationQueryService } from '../../services/query/donation-query-service';
import { StageQueryService } from '../../services/query/stage-query-service';

@autoinject
export class AdminPage {
    httpEngine;
    teamQueryService;
    alert;
    bindingEngine;
    profileQueryService;
    donationQueryService;
    stageQueryService;

    profiles = [];

    searchModel: ProfileSearchModel;
    loading;

    allRoles = AccountService.allRoles;

    constructor(httpEngine: HttpEngine, teamQueryService: TeamQueryService, profileQueryService: ProfileQueryService,
        alertService: AlertService, bindingEngine: BindingEngine, donationQueryService: DonationQueryService,
        stageQueryService: StageQueryService) {
        this.httpEngine = httpEngine;
        this.teamQueryService = teamQueryService;
        this.stageQueryService = stageQueryService;
        this.profileQueryService = profileQueryService;
        this.alert = alertService;
        this.bindingEngine = bindingEngine;
        this.donationQueryService = donationQueryService;
        this.searchModel = new ProfileSearchModel().getDefault();
        this.searchModel = <ProfileSearchModel>this.searchModel.getCached();
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

    createTeam(suuuper: boolean) {
        this.stageQueryService.get().then(x => {
            var team = new TeamEditorModel();
            team.name = 'Random Team @' + moment().format('MM/DD/YY hh:mm:ss a');
            team.guide = '123456789 123456789 123456789 123456789 123456789 123456789 123456789 123456789 123456789 123456789';
            team.credits = '';
            team.shipId = (Math.floor(Math.random() * 30)) + 1;
            team.stageId = x[Math.floor((Math.random() * (suuuper ? 200 : 5))) + 1].id;
            for (var i = 0; i < 6; i++) {
                var randomLimit = suuuper ? 1000 : i < 3 ? 4 : 20;
                var unit = <ITeamUnitEditorModel>{
                    unitId: Math.floor(Math.random() * randomLimit) + 1,
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
        });
    }

    refreshDonations() {
        this.donationQueryService.refreshAll().then(x => {
            this.alert.success('Donations refreshed. See console for states.');
            console.log(x);
        }, x => {
            this.alert.danger('Failed to refresh donations.');
        });
    }
}