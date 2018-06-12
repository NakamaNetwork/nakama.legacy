import { HttpEngine } from '../../../tools/http-engine';
import { BaseQueryService } from './base-query-service';
import { CacheItem } from '../../../models/cache-item';
import * as moment from 'moment';
import { NumberHelper } from '../../../tools/number-helper';

export abstract class LocallyCachedQueryService<TId, TEntity extends CacheItem> extends BaseQueryService {
    protected key: string;
    protected dateKey: string;
    protected items: TEntity[] = [];

    protected dict: any = {};

    constructor(controller: string, http: HttpEngine) {
        super(controller, http);
        this.key = 'controller_cache_' + controller;
        this.dateKey = 'controller_cache_date_' + controller;
        this.initializeCache();
    }

    get(id?: any): Promise<any> {
        return this.getCached('get', id);
    }

    protected initializeCache() {
        var json = localStorage.getItem(this.key);
        if (json) {
            this.items = JSON.parse(json);
            this.rebuildDictionary();
        }
        this.getLatest();
    }

    private get newestDate() {
        var date = localStorage.getItem(this.dateKey);
        if (date) {
            return moment.unix(NumberHelper.forceNumber(date));
        }
        return null;
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
            if (x) {
                localStorage.setItem(this.key, x.items);
                if (x.timestamp) {
                    localStorage.setItem(this.dateKey, moment(x.timestamp).unix().toString());
                }
                this.items = JSON.parse(x.items);
                this.rebuildDictionary();
            }
            this.buildingCache = false;
        }).catch(x => {
            this.buildingCache = false;
            // Try again shortly.
            setTimeout(() => {
                this.getLatest();
            }, 5000);
        });
    }

    protected getCached(endpoint: string, id?: any) {
        return new Promise<any>((resolve, reject) => {
            if (this.buildingCache) {
                setTimeout(() => {
                    resolve(this.getCached(endpoint, id));
                }, 100);
            } else if (id) {
                var result = this.dict[id];
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

    protected rebuildDictionary() {
        this.dict = {};
        this.items.forEach(x => {
            this.dict[x.id] = x;
        });
    }
}