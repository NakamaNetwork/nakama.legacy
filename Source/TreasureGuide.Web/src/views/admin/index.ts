import { autoinject } from 'aurelia-framework';
import { BindingEngine } from 'aurelia-binding';
import { TeamQueryService } from '../../services/query/team-query-service';
import { HttpEngine } from '../../tools/http-engine';
import { AlertService } from '../../services/alert-service';
import { ProfileSearchModel } from '../../services/query/profile-query-service';
import { ProfileQueryService } from '../../services/query/profile-query-service';
import { AccountService } from '../../services/account-service';
import * as moment from 'moment';

@autoinject
export class AdminPage {
    httpEngine;
    teamQueryService;
    alert;
    bindingEngine;
    profileQueryService;

    profiles = [];

    resultCount = 0;
    pages = 0;

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
                this.resultCount = x.totalResults;
                this.pages = Math.ceil(x.totalResults / payload.pageSize);
                this.loading = false;
            }).catch((e) => {
                this.loading = false;
            });
        }
    }

    createTeam() {
        var team = {
            name: 'Random Team @' + moment().format('MM/DD/YY hh:mm:ss a'),
            description: '',
            guide: '',
            credits: '',
            teamUnits: [
                {
                    unitId: Math.floor(Math.random() * 5) + 1,
                    position: 0,
                    sub: false
                },
                {
                    unitId: Math.floor(Math.random() * 5) + 1,
                    position: 1,
                    sub: false
                },
                {
                    unitId: Math.floor(Math.random() * 1000) + 1,
                    position: 2,
                    sub: false
                },
                {
                    unitId: Math.floor(Math.random() * 1000) + 1,
                    position: 3,
                    sub: false
                },
                {
                    unitId: Math.floor(Math.random() * 1000) + 1,
                    position: 4,
                    sub: false
                },
                {
                    unitId: Math.floor(Math.random() * 1000) + 1,
                    position: 5,
                    sub: false
                }
            ],
            teamSockets: []
        };
        this.teamQueryService.save(team).then(x => {
            this.alert.success('Successfully created team \'' + team.name + '\'.');
        }, x => {
            this.alert.danger('Failed to create team \'' + team.name + '\'. Probably gave a bad unit or something.');
        });
    }
}