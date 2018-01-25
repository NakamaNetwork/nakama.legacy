import { StringHelper } from './string-helper';
import { NameIdPair } from '../models/name-id-pair';

export class NumberHelper {
    static isNumber(n): boolean { return !isNaN(parseFloat(n)) && !isNaN(n - 0) }

    static splitEnum(n: number, enumerable, prettify: boolean = false, includeZero: boolean = false): NameIdPair[] {
        var values = NumberHelper.getPairs(enumerable, prettify, includeZero).filter(x => (n & x.id) !== 0);
        return values;
    }

    static getPairs(enumerable, prettify: boolean = false, includeZero: boolean = false): NameIdPair[] {
        return Object.keys(enumerable).map((k) => {
            return { id: enumerable[k], name: prettify ? StringHelper.prettifyEnum(k) : k };
        }).filter(x => !Number.isNaN(Number(x.id)) && (includeZero || x.id !== 0));
    }

    static combine(n: number[]) {
        var sum = 0;
        n.forEach(x => sum += x);
        return sum;
    }

    static forceNumber(input) {
        var number = Number(input);
        return Number.isNaN(number) ? 0 : number;
    }
}