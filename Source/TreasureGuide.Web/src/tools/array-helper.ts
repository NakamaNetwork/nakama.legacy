export class ArrayHelper {
    public static groupBy(array: any[], key: string) {
        return array.reduce((rv, x) => {
            (rv[x[key]] = rv[x[key]] || []).push(x);
            return rv;
        }, {});
    }

    public static binBy(array: any[], key: string) {
        var results = ArrayHelper.groupBy(array, key);
        var bins = [];
        for (var prop in results) {
            if (results.hasOwnProperty(prop)) {
                bins.push({ key: prop, value: results[prop] });
            }
        }
        return bins;
    }
}