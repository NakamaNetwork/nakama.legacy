import { autoinject, computedFrom } from 'aurelia-framework';
import { HttpEngine } from '../../tools/http-engine';
import { SearchableQueryService } from './generic/searchable-query-service';
import { ITeamEditorModel, ITeamSearchModel } from '../../models/imported';

@autoinject
export class TeamQueryService extends SearchableQueryService {
    constructor(http: HttpEngine) {
        super('team', http, false);
    }

    save(model: TeamEditorModel, id?): Promise<any> {
        model.teamUnits = model.teamUnits.filter(x => x.unitId);
        return super.save(model, id);
    }
}

export class TeamSearchModel implements ITeamSearchModel {
    term: string;
    leaderId: number;
    stageId: number;
    myBox: boolean = false;
    global: boolean = false;
    freeToPlay: boolean = false;
    page: number = 1;
    pageSize: number = 25;

    @computedFrom('term', 'leaderId', 'stageId', 'myBox', 'global', 'freeToPlay', 'page', 'pageSize')
    get payload() {
        var text = JSON.stringify({
            term: this.term,
            leaderId: this.leaderId,
            stageId: this.stageId,
            myBox: this.myBox,
            global: this.global,
            freeToPlay: this.freeToPlay,
            page: this.page,
            pageSize: this.pageSize
        });
        var output = JSON.parse(text);
        return output;
    }
};

export class TeamEditorModel implements ITeamEditorModel {
    id: number;
    name: string;
    description: string;
    credits: string;
    guide: string;
    shipId: number = 1;
    stageId: number;
    teamSockets: any[] = [];
    teamUnits: any[] = [];
};