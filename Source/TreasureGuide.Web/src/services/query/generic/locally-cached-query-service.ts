import { HttpEngine } from '../../../tools/http-engine';
import { BaseQueryService } from './base-query-service';
import { CacheItem } from '../../../models/cache-item';
import * as moment from 'moment';

export abstract class LocallyCachedQueryService<TId, TEntity extends CacheItem> extends BaseQueryService {
    protected key: string;
    protected items: TEntity[] = [];

    constructor(controller: string, http: HttpEngine) {
        super(controller, http);
        this.key = 'controller_cache_' + controller;
        this.initializeCache();
    }

    get(id?: any): Promise<any> {
        return this.getCached('get', id);
    }

    protected initializeCache() {
        var json = localStorage.getItem(this.key);
        if (json) {
            this.items = JSON.parse(json);
        }
        this.getLatest();
    }

    private get newestDate() {
        return this.items.filter(x => x.editedDate).map(x => moment(x.editedDate)).sort((a, b) => b.diff(a)).find(() => true);
    }

    buildingCache = false;

    protected getLatest() {
        if (this.buildingCache) {
            return;
        }
        this.buildingCache = true;
        var date = this.newestDate;
        var param = date ? (date.unix() + 5).toString() : null;
        var endpoint = this.buildAddress(param);
        return this.http.get(endpoint).then(x => {
            var ids = x.map(y => y.id);
            var newSet = this.items.filter(x => ids.indexOf(x.id) === -1);
            x.forEach(y => {
                newSet.push(y);
            });
            newSet = newSet.sort((a, b) => a.id - b.id);

            this.items = newSet;
            var json = JSON.stringify(this.items);
            localStorage.setItem(this.key, json);

            this.buildingCache = false;
        }).catch(x => {
            // Try again shortly.
            setTimeout(this.getLatest, 5000);
        });
    }

    protected getCached(endpoint: string, id?: any) {
        return new Promise<any>((resolve, reject) => {
            if (this.buildingCache) {
                setTimeout(() => {
                    resolve(this.getCached(endpoint, id));
                }, 100);
            } else if (id) {
                var result = this.items.find(x => x.id === id);
                if (result) {
                    resolve(result);
                } else {
                    reject(`Could not find item with id '${id}'`);
                }
            } else {
                resolve(this.items);
            }
        });
    }
}