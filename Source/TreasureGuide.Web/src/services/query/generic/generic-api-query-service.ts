import { HttpEngine } from '../../../tools/http-engine';
import { BaseQueryService } from './base-query-service';

export abstract class GenericApiQueryService extends BaseQueryService {
    constructor(controller: string, http: HttpEngine) {
        super(controller, http);
    }

    get(id?: any) {
        return this.http.get(this.buildAddress('', id));
    }

    stub(id?: any) {
        return this.http.get(this.buildAddress('stub', id));
    }

    detail(id?: any) {
        return this.http.get(this.buildAddress('detail', id));
    }

    editor(id?: any) {
        return this.http.get(this.buildAddress('editor', id));
    }

    save(model, id?: any) {
        return this.http.post(this.buildAddress('', id), model);
    }

    delete(id?: any) {
        return this.http.delete(this.buildAddress('', id));
    }
}