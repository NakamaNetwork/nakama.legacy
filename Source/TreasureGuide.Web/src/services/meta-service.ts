import { autoinject } from 'aurelia-framework';
import { NavModel } from 'aurelia-router';
import {StringHelper} from '../tools/string-helper';

@autoinject
export class MetaService {
    private navModel: NavModel;

    constructor(navModel: NavModel) {
        this.navModel = navModel;
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