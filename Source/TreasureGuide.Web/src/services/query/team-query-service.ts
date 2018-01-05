﻿import { autoinject, computedFrom } from 'aurelia-framework';
import { HttpEngine } from '../../tools/http-engine';
import { SearchableQueryService } from './generic/searchable-query-service';
import { ITeamEditorModel, ITeamSearchModel } from '../../models/imported';
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
}

export class TeamSearchModel extends SearchModel implements ITeamSearchModel {
    term: string;
    submittedBy: string;
    leaderId: number;
    stageId: number;
    myBox: boolean;
    global: boolean;
    freeToPlay: boolean;

    getDefault(): TeamSearchModel {
        return new TeamSearchModel();
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
};