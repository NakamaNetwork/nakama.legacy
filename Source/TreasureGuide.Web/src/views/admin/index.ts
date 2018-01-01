import { autoinject } from 'aurelia-framework';
import { TeamQueryService } from '../../services/query/team-query-service';
import { HttpEngine } from '../../tools/http-engine';
import { AlertService } from '../../services/alert-service';

@autoinject
export class AdminPage {
    message = 'Admin Page';
    httpEngine;
    teamQueryService;
    alert;

    constructor(httpEngine: HttpEngine, teamQueryService: TeamQueryService, alertService: AlertService) {
        this.httpEngine = httpEngine;
        this.teamQueryService = teamQueryService;
        this.alert = alertService;
    }

    createTeam() {
        var team = {
            name: 'Random Team @' + new Date().toJSON(),
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