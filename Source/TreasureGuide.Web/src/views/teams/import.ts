import { autoinject } from 'aurelia-framework';
import { TeamQueryService } from '../../services/query/team-query-service';
import { ITeamEditorModel, ITeamVideoModel, ITeamCreditModel, ITeamImportModel } from '../../models/imported';
import { CalcParser } from '../../tools/calc-parser';
import { VideoParser } from '../../tools/video-parser';

@autoinject
export class TeamImportPage {
    private teamQueryService: TeamQueryService;
    private calcParser: CalcParser;

    imports: TeamImportLineModel[] = [];
    strings: string = '';

    constructor(teamQueryService: TeamQueryService, calcParser: CalcParser) {
        this.teamQueryService = teamQueryService;
        this.calcParser = calcParser;
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
        for (var i = 0; i < lines.length; i += 8) {
            var model = <TeamImportLineModel>{
                name: lines[i],
                calc: lines[i + 1],
                stage: parseInt(lines[i + 2]),
                desc: lines[i + 3],
                credit: lines[i + 4],
                videos: lines[i + 5],
                ref: lines[i + 6],
                type: parseInt(lines[i + 7]),
                submitted: false,
                failed: false
            };
            this.imports.push(model);
        }
    }

    createModel(value: TeamImportLineModel): ITeamImportModel {
        var item = <ITeamImportModel>{};
        item.team = this.createTeam(value);
        item.videos = this.createVideos(value);
        item.credit = this.createCredit(value);
        return item;
    }


    createTeam(value: TeamImportLineModel): ITeamEditorModel {
        var teamIds = this.calcParser.parse(value.calc);
        var teamUnits = this.calcParser.convert(teamIds.units);
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