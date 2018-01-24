import { autoinject } from 'aurelia-framework';
import { TeamQueryService } from '../../services/query/team-query-service';
import { ITeamEditorModel, ITeamVideoModel, ITeamCreditModel, ITeamImportModel } from '../../models/imported';
import { CalcParser } from '../../tools/calc-parser';
import { VideoParser } from '../../tools/video-parser';

@autoinject
export class TeamImportPage {
    private teamQueryService: TeamQueryService;

    imports: TeamImportLineModel[] = [];
    strings: string = '';

    constructor(teamQueryService: TeamQueryService) {
        this.teamQueryService = teamQueryService;
    }

    add() {
        this.imports.push(new TeamImportLineModel());
    }

    remove(team) {
        var index = this.imports.indexOf(team);
        if (index >= 0) {
            this.imports = this.imports.filter((x, i) => i !== index);
        }
    }

    submit() {
        this.imports.filter(x => x.name && !x.submitted).forEach(o => {
            var model = this.createModel(o);
            this.teamQueryService.import(model).then(x => {
                o.submitted = true;
                o.failed = false;
            }).catch(x => {
                o.failed = true;
                o.submitted = false;
            });
        });
    }

    convertStrings() {
        var lines = this.strings.split('|');
        if (lines.length % 8 !== 0) {
            return;
        }
        var things = new Array<TeamImportLineModel>();
        for (var i = 0; i < lines.length; i += 8) {
            var stage = parseInt(lines[i + 2]);
            if (Number.isNaN(stage)) {
                stage = null;
            }
            var type = parseInt(lines[i + 7]);
            if (Number.isNaN(type)) {
                type = 0;
            }
            var model = <TeamImportLineModel>{
                name: lines[i],
                calc: lines[i + 1],
                stage: stage,
                desc: lines[i + 3],
                credit: lines[i + 4],
                videos: lines[i + 5],
                ref: lines[i + 6],
                type: type,
                submitted: false,
                failed: false
            };
            things.push(model);
        }
        this.imports = (things);
    }

    createModel(value: TeamImportLineModel): ITeamImportModel {
        var item = <ITeamImportModel>{};
        item.team = this.createTeam(value);
        item.videos = this.createVideos(value);
        item.credit = this.createCredit(value);
        return item;
    }


    createTeam(value: TeamImportLineModel): ITeamEditorModel {
        var teamIds = CalcParser.parse(value.calc);
        var teamUnits = CalcParser.convert(teamIds.units);
        var item = <ITeamEditorModel>{
            name: value.name,
            guide: value.desc,
            credits: value.credit,
            teamUnits: teamUnits,
            shipId: teamIds.ship,
            stageId: value.stage
        };
        return item;
    }

    createVideos(value: TeamImportLineModel): ITeamVideoModel[] {
        if (value.videos) {
            var items = value.videos.split(',').map(x => <ITeamVideoModel>{
                videoLink: VideoParser.parse(x)
            });
            return items;
        }
        return [];
    }

    createCredit(value: TeamImportLineModel): ITeamCreditModel {
        var item = <ITeamCreditModel>{
            credit: value.ref,
            type: value.type
        };
        return item;
    }
}

export class TeamImportLineModel {
    name: string;
    calc: string;
    desc: string;
    credit: string;
    videos: string;
    stage?: number;
    ref: string;
    type: number;
    submitted: boolean = false;
    failed: boolean = false;
}