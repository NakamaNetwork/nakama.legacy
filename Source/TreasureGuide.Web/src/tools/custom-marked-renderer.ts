import * as marked from 'marked';

export class CustomMarkedRenderer extends marked.Renderer {
    public image(href: string, title: string, text: string): string {
        var linkTitle = title || text || 'image';
        return '<a href="' + href + '" target="_blank"><i class="fa fa-fw fa-file-image-o"></i>' + linkTitle + '</a>';
    }

    private checkUnitClassLink(href: string, text: string): string {
        var unitClass = '';
        switch ((href || '').toLowerCase()) {
            case '/str':
                unitClass = 'str';
                break;
            case '/dex':
                unitClass = 'dex';
                break;
            case '/qck':
                unitClass = 'qck';
                break;
            case '/psy':
                unitClass = 'psy';
                break;
            case '/int':
                unitClass = 'int';
                break;
            case '/meat':
                unitClass = 'meat';
                break;
        }
        if (unitClass) {
            return '<strong class="_' + unitClass + 'Text">' + text + '</strong>';
        }
        return unitClass;
    }


    public link(href: string, title: string, text: string): string {
        var unitClass = this.checkUnitClassLink(href, text);
        return unitClass || super.link(href, title, text);
    }
}