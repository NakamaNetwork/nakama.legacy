import { ITeamDetailModel } from '../models/imported';

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

    export(team: ITeamDetailModel): string {
        var prefix = 'http://optc-db.github.io/damage/#/transfer/D';
        var characters = '';
        for (var i = 0; i < 6; i++) {
            var unit = team.teamUnits.find(x => x.position === i);
            if (unit) {
                characters += unit.unitId + ':' + (unit.level || 1);
            } else {
                characters += '!';
            }
            if (i < 5) {
                characters += ',';
            }
        }
        var boatString = 'C' + team.shipId + ',10';
        var postfix = 'B0D0E0Q0L0G0R0S100H';
        return prefix + characters + boatString + postfix;
    }
}