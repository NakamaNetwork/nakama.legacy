import { autoinject } from 'aurelia-framework';
import { TeamQueryService } from '../../services/query/team-query-service';
import { ITeamEditorModel, ITeamVideoModel, ITeamCreditModel, ITeamImportModel, TeamCreditType } from '../../models/imported';
import { CalcParser } from '../../tools/calc-parser';
import { VideoParser } from '../../tools/video-parser';
import { AlertService } from '../../services/alert-service';
import { AlertDialog } from '../../custom-elements/dialogs/alert-dialog';
import { DialogService } from 'aurelia-dialog';

@autoinject
export class TeamImportPage {
    private teamQueryService: TeamQueryService;
    private alertService: AlertService;
    private dialogService: DialogService;

    imports: TeamImportLineModel[] = [];
    strings: string = '';
    stageId: number;

    constructor(teamQueryService: TeamQueryService, alertService: AlertService, dialogService: DialogService) {
        this.teamQueryService = teamQueryService;
        this.alertService = alertService;
        this.dialogService = dialogService;
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
        var message = 'These teams will immediately be available to the public. Please make sure you have reviewed the submission table before committing!';
        this.dialogService.open({ viewModel: AlertDialog, model: { message: message, cancelable: true }, lock: true }).whenClosed(x => {
            if (!x.wasCancelled) {
                this.imports.filter(x => x.calc && !x.submitted).forEach((o, i) => {
                    setTimeout(() => {
                        var model = this.createModel(o);
                        this.teamQueryService.import(model).then(x => {
                            o.submitted = true;
                            o.failed = false;
                            this.alertService.success('Saved team #' + x);
                        }).catch(x => {
                            o.failed = true;
                            o.submitted = false;
                            this.alertService.danger('Failed to save a team. See the table and check your input.');
                        });
                    }, i * 250);
                });
            }
        });
    }

    convertStrings() {
        var lines = this.strings.toLowerCase().split(/\r?\n/g);
        var things = new Array<TeamImportLineModel>();
        this.alertService.info('Processing rows...');
        for (var i = 0; i < lines.length; i++) {
            var line = lines[i];
            if (!line) {
                continue;
            }
            var data = line.split(';');
            if (data.length == 0) {
                continue;
            }
            var calc = data[0];
            if (calc.indexOf('yout') !== -1) {
                continue;
            }
            var videos = '';
            if (data.length > 1) {
                for (var j = 1; j < data.length; j++) {
                    if (j > 1) {
                        videos += ',';
                    }
                    videos += data[j];
                }
                while (videos.indexOf(',,') !== -1) {
                    videos = videos.replace(',,', ',');
                }
                if (videos.length === 1) {
                    videos = '';
                }
            }
            var model = <TeamImportLineModel>{
                name: '',
                calc: calc,
                stage: this.stageId,
                desc: '',
                credit: 'From the Global Clear Rates sheet.',
                videos: videos,
                ref: 'GCR Import',
                type: TeamCreditType.GCR,
                submitted: false,
                failed: false
            };
            things.push(model);
        }
        this.alertService.info('Setting fields...');
        this.imports = (things);
        this.alertService.success('Ready for review.');
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
            stageId: value.stage,
            draft: false
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