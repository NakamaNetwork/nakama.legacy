import { autoinject } from 'aurelia-framework';
import { HttpEngine } from '../../tools/http-engine';
import { SearchableQueryService } from './generic/searchable-query-service';
import {
    ITeamStubModel, ITeamEditorModel, ITeamSearchModel, ITeamVoteModel, ITeamReportModel,
    ITeamImportModel, ITeamVideoModel, ITeamReportStubModel, FreeToPlayStatus, SearchConstants,
    ITeamSocketEditorModel, ITeamUnitEditorModel, ITeamGenericSlotEditorModel
} from '../../models/imported';
import { SearchModel } from '../../models/search-model';

@autoinject
export class TeamQueryService extends SearchableQueryService {
    constructor(http: HttpEngine) {
        super('team', http, false);
    }

    save(model: TeamEditorModel, id?): Promise<any> {
        model.teamUnits = model.teamUnits.filter(x => x.unitId);
        return super.save(model, id);
    }

    trending(): Promise<ITeamStubModel[]> {
        return this.http.get(this.buildAddress('trending'));
    }

    latest(): Promise<ITeamStubModel[]> {
        return this.http.get(this.buildAddress('latest'));
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
}

export class TeamSearchModel extends SearchModel implements ITeamSearchModel {
    term: string;
    submittedBy: string;
    leaderId: number;
    stageId: number;
    myBox: boolean;
    global: boolean;
    freeToPlay: FreeToPlayStatus = FreeToPlayStatus.None;
    deleted: boolean;
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
            name: 'All'
        }, {
            value: FreeToPlayStatus.Crew,
            name: 'Crew'
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
};