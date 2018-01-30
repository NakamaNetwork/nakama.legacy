import {StringHelper} from './string-helper';

export class MetaTool {
    public static setTitle(title: string) {
        title = StringHelper.truncate(title, 80);
        title += ' | Nakama Network';
        document.title = title;
    }
}