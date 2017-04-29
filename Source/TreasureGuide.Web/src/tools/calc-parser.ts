export class CalcParser {
    private unitMatchRegex = /[D,]{1}(\d+?):/ig;

    parse(linker: string) {
        var matches = [];
        if (linker) {
            var match;
            do {
                match = this.unitMatchRegex.exec(linker);
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
}