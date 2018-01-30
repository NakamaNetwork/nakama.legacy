import { autoinject } from 'aurelia-framework';
import { StringHelper } from '../tools/string-helper';
import { MetaQueryService } from './query/meta-query-service';
import { IMetaResultModel } from '../models/imported';

@autoinject
export class MetaService {
    private metaQuery: MetaQueryService;

    constructor(metaQuery: MetaQueryService) {
        this.metaQuery = metaQuery;
    }

    public getSEOMetaData(): Promise<IMetaResultModel> {
        return new Promise<IMetaResultModel>((resolve) => {
            var hash = window.location.hash;
            if (hash) {
                this.metaQuery.get(window.location.hash).then(result => {
                    if (result) {
                        if (result.title) {
                            this.setTitle(result.title);
                        }
                        if (result.description) {
                            this.setDescription(result.description);
                        }
                    }
                    resolve(result);
                }).catch(() => {
                    resolve(null);
                });
            } else {
                resolve(null);
            }
        });
    }

    public setTitle(title: string) {
        if (title) {
            title = StringHelper.truncate(title, 60);
            title += ' | Nakama Network';
            document.title = title;
            this.setMetaProperty('og:site_name', title);
        }
    }

    public setDescription(desc: string) {
        desc = StringHelper.truncate(desc, 300);
        this.setMetaProperty('og:description', desc);
        this.setMetaName('description', desc);
    }

    private setMetaProperty(tag: string, content: string) {
        this.setMetaTag('property', tag, content);
    }

    private setMetaName(tag: string, content: string) {
        this.setMetaTag('name', tag, content);
    }

    private setMetaTag(attributeSearch: string, attribute: string, content: string) {
        if (content) {
            var metaList = document.getElementsByTagName('meta');
            for (var i = 0; i < metaList.length; i++) {
                var meta = metaList[i];
                var attr = meta.getAttribute(attributeSearch);
                if (attr === attribute) {
                    meta.setAttribute('content', content);
                }
            }
        }
    }
}