export class CalcParser {
    private unitMatchRegex = /[D,]{1}(\d+?):/ig;
    private shipMatchRegex = /C(\d+?),/ig;

    parse(link: string) {
        var units = [];
        var ship = 0;
        if (link) {
            var match;
            do {
                match = this.unitMatchRegex.exec(link);
                if (Array.isArray(match) && match.length >= 2) {
                    match = match[1];
                    var number = Number.parseInt(match);
                    if (!Number.isNaN(number)) {
                        units.push(number);
                    }
                }
            } while (match);
            do {
                match = this.shipMatchRegex.exec(link);
                if (Array.isArray(match) && match.length >= 2) {
                    match = match[1];
                    var number = Number.parseInt(match);
                    if (!Number.isNaN(number)) {
                        ship = number;
                    }
                }
            } while (match);
        }
        return { units: units, ship: ship };
    }

    convert(ids: number[]) {
        return ids.map((x, i) => {
            return {
                unitId: x,
                position: i < 6 ? i : null,
                specialLevel: 0,
                sub: !(i < 6)
            };
        });
    };
}