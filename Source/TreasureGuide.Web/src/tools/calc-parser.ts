import { ITeamDetailModel, ITeamUnitEditorModel } from '../models/imported';

export class CalcParser {
    private static unitMatchRegex = /[D,]{1}(\d*?)[:!]/ig;
    private static shipMatchRegex = /C(\d+?),/ig;

    static parse(link: string) {
        var units = [];
        var ship = 0;
        if (link) {
            var match;
            do {
                match = CalcParser.unitMatchRegex.exec(link);
                if (Array.isArray(match) && match.length >= 1) {
                    var matched = match[1];
                    var number = Number.parseInt(matched);
                    if (!Number.isNaN(number)) {
                        units.push(number);
                    } else {
                        units.push(null);
                    }
                }
            } while (match);
            do {
                match = CalcParser.shipMatchRegex.exec(link);
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

    static convert(ids: number[]): ITeamUnitEditorModel[] {
        return ids.map((x, i) => {
            return <ITeamUnitEditorModel>{
                unitId: x,
                position: i < 6 ? i : null,
                sub: !(i < 6)
            };
        });
    };

    static export(team): string {
        var prefix = 'http://optc-db.github.io/damage/#/transfer/D';
        var characters = '';
        for (var i = 0; i < 6; i++) {
            var unit = team.teamUnits.find(x => x.position === i && !x.sub);
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