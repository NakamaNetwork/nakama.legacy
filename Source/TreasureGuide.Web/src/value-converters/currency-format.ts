import { NumberHelper } from '../tools/number-helper';

export class CurrencyFormatValueConverter {
    toView(value) {
        return NumberHelper.forceNumber(value).toFixed(2);
    }
}