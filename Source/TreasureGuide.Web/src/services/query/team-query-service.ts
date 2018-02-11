import { autoinject } from 'aurelia-framework';
import { HttpEngine } from '../../tools/http-engine';
import { SearchableQueryService } from './generic/searchable-query-service';
import {
    ITeamStubModel, ITeamEditorModel, ITeamSearchModel, ITeamVoteModel, ITeamReportModel,
    ITeamImportModel, ITeamVideoModel, ITeamReportStubModel, FreeToPlayStatus, SearchConstants,
    ITeamSocketEditorModel, ITeamUnitEditorModel, ITeamGenericSlotEditorModel, UnitType,
    UnitRole, UnitClass
} from '../../models/imported';
import { SearchModel } from '../../models/search-model';
import { NumberHelper } from '../../tools/number-helper';

@autoinject
export class TeamQueryService extends SearchableQueryService {
    constructor(http: HttpEngine) {
        super('team', http, false);
    }

    save(model: TeamEditorModel, id?): Promise<any> {
        var payload = model.getPayload();
        return super.save(payload, id);
    }

    trending(): Promise<ITeamStubModel[]> {
        return this.http.get(this.buildAddress('trending'));
    }

    latest(): Promise<ITeamStubModel[]> {
        return this.http.get(this.buildAddress('latest'));
    }

    bookmark(teamId: number): Promise<boolean> {
        return this.http.post(this.buildAddress('bookmark/' + teamId));
    }

    vote(model: ITeamVoteModel): Promise<number> {
        return this.http.post(this.buildAddress('vote'), model);
    }

    report(model: ITeamReportModel): Promise<number> {
        return this.http.post(this.buildAddress('report'), model);
    }

    reports(teamId: number): Promise<ITeamReportStubModel[]> {
        return this.http.get(this.buildAddress('reports/' + teamId));
    }

    acknowledgeReport(teamId: number): Promise<number> {
        return this.http.post(this.buildAddress('acknowledgeReport/' + teamId));
    }

    video(model: ITeamVideoModel): Promise<number> {
        return this.http.post(this.buildAddress('video'), model);
    }

    import(model: ITeamImportModel): Promise<number> {
        return this.http.post(this.buildAddress('import'), model);
    }

    similarId(id: number): Promise<ITeamStubModel[]> {
        return this.http.get(this.buildAddress(id + '/similar'));
    }

    similar(payload): Promise<ITeamStubModel[]> {
        var endpoint = this.http.parameterize(this.buildAddress('similar'), payload);
        return this.http.get(endpoint);
    }
}

export class TeamSearchModel extends SearchModel implements ITeamSearchModel {
    classes: UnitClass;
    types: UnitType;
    term: string;
    submittedBy: string;
    leaderId: number;
    noHelp: boolean;
    stageId: number;
    boxId: number;
    blacklist: boolean;
    global: boolean;
    freeToPlay: FreeToPlayStatus;
    deleted: boolean;
    bookmark: boolean;
    reported: boolean;
    draft: boolean;
    cacheKey: string = 'search-team';

    sortables: string[] = [
        SearchConstants.SortDate,
        SearchConstants.SortName,
        SearchConstants.SortScore,
        SearchConstants.SortStage,
        SearchConstants.SortLeader,
        SearchConstants.SortUser
    ];

    getDefault(): TeamSearchModel {
        return new TeamSearchModel();
    }

    static freeToPlayOptions = [
        {
            value: FreeToPlayStatus.None,
            name: '-----'
        }, {
            value: FreeToPlayStatus.All,
            name: 'F2P Team'
        }, {
            value: FreeToPlayStatus.Crew,
            name: 'F2P Crew'
        }];
};

export class TeamEditorModel implements ITeamEditorModel {
    id: number;
    name: string;
    credits: string;
    guide: string;
    shipId: number = 1;
    stageId: number;
    teamSockets: ITeamSocketEditorModel[] = [];
    teamUnits: ITeamUnitEditorModel[] = [];
    teamGenericSlots: ITeamGenericSlotEditorModel[] = [];
    deleted: boolean;
    draft: boolean;

    getPayload(): ITeamEditorModel {
        return <ITeamEditorModel>{
            id: NumberHelper.forceNumber(this.id),
            name: this.name,
            credits: this.credits,
            guide: this.guide,
            shipId: this.shipId,
            stageId: this.stageId,
            deleted: this.deleted,
            draft: this.draft,
            teamSockets: this.teamSockets
                .map(x => <ITeamSocketEditorModel>{
                    level: NumberHelper.forceNumber(x.level),
                    socketType: NumberHelper.forceNumber(x.socketType)
                })
                .filter(x => x.level),
            teamUnits: this.teamUnits
                .map(x => <ITeamUnitEditorModel>{
                    position: NumberHelper.forceNumber(x.position),
                    special: NumberHelper.forceNumber(x.special),
                    sub: x.sub,
                    unitId: NumberHelper.forceNumber(x.unitId)
                })
                .filter(x => x.unitId),
            teamGenericSlots: this.teamGenericSlots
                .map(x => <ITeamGenericSlotEditorModel>{
                    class: NumberHelper.forceNumber(x.class),
                    position: NumberHelper.forceNumber(x.position),
                    role: NumberHelper.forceNumber(x.role),
                    sub: x.sub,
                    type: NumberHelper.forceNumber(x.type)
                })
                .filter(x => x.role + x.class + x.type)
        };
    }
};

export class TeamGenericSlotEditorModel implements ITeamGenericSlotEditorModel {
    public sub: boolean;
    public role: UnitRole;
    public type: UnitType;
    public class: UnitClass;
    public position: number;
}