import { autoinject } from 'aurelia-framework';
import { HttpEngine } from '../../tools/http-engine';
import { SearchableQueryService } from './generic/searchable-query-service';
import { ITeamEditorModel, ITeamSearchModel, ITeamVoteModel, ITeamReportModel } from '../../models/imported';
import { SearchModel } from '../../models/search-model';
import { ITeamReportStubModel } from '../../models/imported';

@autoinject
export class TeamQueryService extends SearchableQueryService {
    constructor(http: HttpEngine) {
        super('team', http, false);
    }

    save(model: TeamEditorModel, id?): Promise<any> {
        model.teamUnits = model.teamUnits.filter(x => x.unitId);
        return super.save(model, id);
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
}

export class TeamSearchModel extends SearchModel implements ITeamSearchModel {
    term: string;
    submittedBy: string;
    leaderId: number;
    stageId: number;
    myBox: boolean;
    global: boolean;
    freeToPlay: boolean;
    deleted: boolean;
    reported: boolean;

    getDefault(): TeamSearchModel {
        return new TeamSearchModel();
    }

    getCacheKey(): string {
        return 'search-team';
    }
};

export class TeamEditorModel implements ITeamEditorModel {
    id: number;
    name: string;
    credits: string;
    guide: string;
    shipId: number = 1;
    stageId: number;
    teamSockets: any[] = [];
    teamUnits: any[] = [];
    deleted: boolean;
};