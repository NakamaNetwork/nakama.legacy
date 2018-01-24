export class NumberHelper {
    static isNumber(n): boolean { return !isNaN(parseFloat(n)) && !isNaN(n - 0) }

    static splitEnum(n: number, enumerable): number[] {
        var values = Object.keys(enumerable)
            .map(x => enumerable[x])
            .filter(x => !Number.isNaN(Number(x)))
            .filter(x => (n & x) !== 0);
        return values;
    }
}