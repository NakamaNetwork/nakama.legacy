export class NumberHelper {
    static isNumber(n): boolean { return !isNaN(parseFloat(n)) && !isNaN(n - 0) }
}