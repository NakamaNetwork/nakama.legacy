import * as moment from 'moment';

export class AgeFormatValueConverter {
    toView(value) {
        return moment(value).fromNow();
    }
}