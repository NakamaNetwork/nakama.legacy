import { autoinject } from 'aurelia-framework';
import { HttpEngine } from '../../tools/http-engine';
import { IGCREditorModel, IGCRUnitEditModel, IGCRStageEditModel } from '../../models/imported';

@autoinject
export class GCRQueryService {
    private http: HttpEngine;

    constructor(http: HttpEngine) {
        this.http = http;
    }

    get(): Promise<IGCREditorModel> {
        return this.http.get('/api/gcr');
    }

    saveUnits(units: IGCRUnitEditModel[]): Promise<number> {
        return this.http.post('/api/gcr/units', units);
    }

    saveStages(stages: IGCRStageEditModel[]): Promise<number> {
        return this.http.post('/api/gcr/stages', stages);
    }
}