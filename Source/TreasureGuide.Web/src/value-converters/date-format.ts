import * as moment from 'moment';

export class DateFormatValueConverter {
    toView(value, format = 'MM/DD/YY') {
        return moment(value).format(format);
    }
}