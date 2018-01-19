import * as moment from 'moment';

export class TimeFormatValueConverter {
    toView(value, format = 'h:mm a') {
        return moment(value).format(format);
    }
}