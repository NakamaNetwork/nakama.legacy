export class CalcParser {
    private unitMatchRegex = /[D,]{1}(\d+?):/ig;

    parse(link: string) {
        var matches = [];
        if (link) {
            var match;
            do {
                match = this.unitMatchRegex.exec(link);
                if (Array.isArray(match) && match.length >= 2) {
                    match = match[1];
                    var number = Number.parseInt(match);
                    if (!Number.isNaN(number)) {
                        matches.push(number);
                    }
                }
            } while (match);
        }
        return matches;
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