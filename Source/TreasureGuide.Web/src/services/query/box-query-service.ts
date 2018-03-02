import { autoinject } from 'aurelia-framework';
import { HttpEngine } from '../../tools/http-engine';
import { SearchableQueryService } from './generic/searchable-query-service';
import { IBoxDetailModel, IBoxEditorModel, IBoxSearchModel, IBoxUpdateModel, IBoxUnitDetailModel } from '../../models/imported';
import { SearchModel } from '../../models/search-model';
import { IUnitStubModel, IBoxUnitUpdateModel } from '../../models/imported';
import { IndividualUnitFlags } from '../../models/imported';

@autoinject
export class BoxQueryService extends SearchableQueryService {
    constructor(http: HttpEngine) {
        super('box', http);
    }

    update(model: IBoxUpdateModel): Promise<any> {
        return this.http.post(this.buildAddress('update'), model);
    }

    set(model: IBoxUpdateModel): Promise<any> {
        return this.http.post(this.buildAddress('set'), model);
    }

    focus(id: number): Promise<IBoxDetailModel> {
        return this.http.post(this.buildAddress('focus' + (id ? '/' + id : '')));
    }
}

export class BoxSearchModel extends SearchModel implements IBoxSearchModel {
    userId: string;
    blacklist: boolean;

    public getDefault(): SearchModel {
        return new BoxSearchModel();
    }
}

export class BoxEditorModel implements IBoxEditorModel {
    id: number;
    name: string;
    friendId: number;
    global: boolean = false;
    public: boolean = true;
    blacklist: boolean = false;

    deleted: boolean = false;
}

export class BoxDetailModel implements IBoxDetailModel {
    boxUnits: IBoxUnitDetailModel[];
    userId: string;
    userName: string;
    userUnitId: number;
    userIsDonor: boolean;
    id: number;
    name: string;
    friendId: number;
    global: boolean;
    public: boolean;
    blacklist: boolean;

    get unitIds(): number[] {
        return this.boxUnits.map(x => x.unitId);
    }

    saved() {
        this.added = [];
        this.removed = [];
        this.updated = [];
    }

    get dirty(): boolean {
        return this.added.length > 0 || this.removed.length > 0 || this.updated.length > 0;
    }

    get boxUpdateModel(): IBoxUpdateModel {
        return <IBoxUpdateModel>{
            id: this.id,
            added: this.added,
            removed: this.removed,
            updated: this.updated
        }
    }

    toggle(unitId: number) {
        if (this.boxUnits.find(x => x.unitId === unitId)) {
            this.added = this.added.filter(x => x !== unitId);
            this.removed.push(unitId);
            this.boxUnits = this.boxUnits.filter(x => x.unitId !== unitId);
        } else {
            this.added.push(unitId);
            this.removed = this.removed.filter(x => x !== unitId);
            this.boxUnits.push(<IBoxUnitDetailModel>{ unitId: unitId });
        }
    }

    update(unitId: number, flags: IndividualUnitFlags) {
        var existing = this.boxUnits.find(x => x.unitId === unitId);
        if (existing) {
            existing.flags = flags;
            var upEx = this.updated.find(x => x.id === unitId);
            if (upEx) {
                upEx.flags = flags;
            } else {
                this.updated.push(<IBoxUnitUpdateModel>{
                    id: unitId,
                    flags: flags
                });
            }
        }
    }

    added: number[] = [];
    removed: number[] = [];
    updated: IBoxUnitUpdateModel[] = [];
}