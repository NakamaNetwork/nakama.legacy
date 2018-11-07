import { autoinject } from 'aurelia-framework';
import { HttpEngine } from '../../tools/http-engine';
import { SearchableQueryService } from './generic/searchable-query-service';
import { SearchModel } from '../../models/search-model';
import {
    ITeamCommentStubModel, ITeamCommentSearchModel, ITeamCommentDetailModel,
    ITeamCommentEditorModel, SearchConstants, ITeamCommentVoteModel, ITeamCommentReportModel
} from '../../models/imported';

@autoinject
export class TeamCommentQueryService extends SearchableQueryService {
    constructor(http: HttpEngine) {
        super('teamcomment', http);
    }

    vote(model: ITeamCommentVoteModel): Promise<number> {
        return this.http.post(this.buildAddress('vote'), model);
    }

    report(model: ITeamCommentReportModel): Promise<number> {
        return this.http.post(this.buildAddress('report'), model);
    }

    acknowledge(model: ITeamCommentReportModel): Promise<number> {
        return this.http.post(this.buildAddress('acknowledge'), model);
    }

    loadMore(id: number, currentCount: number): Promise<ITeamCommentStubModel[]> {
        var payload = { id: id, current: currentCount };
        var endpoint = this.http.parameterize(this.buildAddress('loadMore'), payload);
        return this.http.get(endpoint);
    }
}

export class TeamCommentSearchModel extends SearchModel implements ITeamCommentSearchModel {
    teamId: number;
    deleted: boolean;
    reported: boolean;

    public sortables: string[] = [
        SearchConstants.SortScore,
        SearchConstants.SortDate
    ];

    getDefault(): SearchModel {
        return new TeamCommentSearchModel();
    }
}


export class TeamCommentEditorModel implements ITeamCommentEditorModel {
    public parentId: number;
    public id: number;
    public teamId: number;
    public text: string;
}

export class TeamCommentStubModel implements ITeamCommentStubModel {
    public children: ITeamCommentDetailModel[];
    public childCount: number;
    public id: number;
    public teamId: number;
    public text: string;
    public deleted: boolean;
    public reported: boolean;
    public canEdit: boolean;
    public myVote: number;
    public score: number;
    public submittedById: string;
    public submittedByName: string;
    public submittedByUnitId: number;
    public submittedByIsDonor: boolean;
    public submittedDate: Date;
    public editedDate: Date;
}