import * as moment from 'moment';

export class DateTimeFormatValueConverter {
    toView(value, format = 'MM/DD/YY @ h:mm:ss a') {
        return moment(value).format(format);
    }
}