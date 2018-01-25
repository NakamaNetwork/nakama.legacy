import { CalcParser } from '../../../src/tools/calc-parser';
import { ITeamDetailModel } from '../../../src/models/imported';

describe('calc parser', () => {
    var check_parsed = (results: any[], expected: any[]) => {
        expect(results.length).toBe(expected.length);
        for (var i = 0; i < expected.length; i++) {
            var a = results[i];
            var b = expected[i];
            for (var property in results[i]) {
                expect(a.hasOwnProperty(property)).toBeTruthy();
                expect(b.hasOwnProperty(property)).toBeTruthy();
                expect(a[property]).toBe(b[property]);
            }
        }
    }

    it('parses out unit ids', () => {
        var results = CalcParser.parse(
            'http://optc-db.github.io/damage/#/transfer/D1:5,2:20,3:18:0:36:0,4:99:74:32:80,5:20,6:35C3,10B5454D0E66Q0L0G0R0S100H');
        var expected = [1, 2, 3, 4, 5, 6];

        expect(results.units.length).toBe(expected.length);
        for (var i = 0; i < expected.length; i++) {
            expect(results.units[i]).toBe(expected[i]);
        }
    });

    it('parses out gaps properly', () => {
        var results = CalcParser.parse(
            'http://optc-db.github.io/damage/#/transfer/D12:99,13:20,7:50,9:20,!,8:99C1,10B0D0E0Q0L0G0R0S100H');
        var expected = [12, 13, 7, 9, null, 8];

        expect(results.units.length).toBe(expected.length);
        for (var i = 0; i < expected.length; i++) {
            expect(results.units[i]).toBe(expected[i]);
        }
    });

    it('parses out ship id', () => {
        var results = CalcParser.parse(
            'http://optc-db.github.io/damage/#/transfer/D1:5,2:20,3:18:0:36:0,4:99:74:32:80,5:20,6:35C31,10B5454D0E66Q0L0G0R0S100H');
        expect(results.ship).toBe(31);
    });

    it('parses team members',
        () => {
            var results = CalcParser.convert([12, 23, 34, 45, 56, 67]);
            var expected = [
                {
                    unitId: 12,
                    position: 0,
                    special: 0,
                    sub: false
                }, {
                    unitId: 23,
                    position: 1,
                    special: 0,
                    sub: false
                }, {
                    unitId: 34,
                    position: 2,
                    special: 0,
                    sub: false
                }, {
                    unitId: 45,
                    position: 3,
                    special: 0,
                    sub: false
                }, {
                    unitId: 56,
                    position: 4,
                    special: 0,
                    sub: false
                }, {
                    unitId: 67,
                    position: 5,
                    special: 0,
                    sub: false
                }
            ];
            check_parsed(results, expected);
        });

    it('makes extra units subs',
        () => {
            var results = CalcParser.convert([12, 23, 34, 45, 56, 67, 12, 54, 32]);
            var expected = [
                {
                    unitId: 12,
                    position: 0,
                    special: 0,
                    sub: false
                }, {
                    unitId: 23,
                    position: 1,
                    special: 0,
                    sub: false
                }, {
                    unitId: 34,
                    position: 2,
                    special: 0,
                    sub: false
                }, {
                    unitId: 45,
                    position: 3,
                    special: 0,
                    sub: false
                }, {
                    unitId: 56,
                    position: 4,
                    special: 0,
                    sub: false
                }, {
                    unitId: 67,
                    position: 5,
                    special: 0,
                    sub: false
                }, {
                    unitId: 12,
                    position: null,
                    special: 0,
                    sub: true
                }, {
                    unitId: 54,
                    position: null,
                    special: 0,
                    sub: true
                }, {
                    unitId: 32,
                    position: null,
                    special: 0,
                    sub: true
                }
            ];
            check_parsed(results, expected);
        });

    it('can export teams',
        () => {
            var expected = 'http://optc-db.github.io/damage/#/transfer/D1362:5,1362:7,!,1:5,408:56,418:99C4,10B0D0E0Q0L0G0R0S100H';
            var team = {
                teamUnits: [
                    {
                        unitId: 1362,
                        level: 5,
                        position: 0,
                        special: 0,
                        sub: false
                    }, {
                        unitId: 1362,
                        level: 7,
                        position: 1,
                        special: 0,
                        sub: false
                    }, {
                        unitId: 1,
                        level: 5,
                        position: 3,
                        special: 0,
                        sub: false
                    }, {
                        unitId: 408,
                        level: 56,
                        position: 4,
                        special: 0,
                        sub: false
                    }, {
                        unitId: 418,
                        level: 99,
                        position: 5,
                        special: 0,
                        sub: false
                    }
                ],
                shipId: 4
            };

            var parsed = CalcParser.export(<ITeamDetailModel>team);
            expect(parsed).toBe(expected);
        });


    it('can ignores subs on export',
        () => {
            var expected = 'http://optc-db.github.io/damage/#/transfer/D1362:5,1362:7,!,1:5,!,!C7,10B0D0E0Q0L0G0R0S100H';
            var team = {
                teamUnits: [
                    {
                        unitId: 1362,
                        level: 5,
                        position: 0,
                        special: 0,
                        sub: false
                    }, {
                        unitId: 1362,
                        level: 7,
                        position: 1,
                        special: 0,
                        sub: false
                    }, {
                        unitId: 1,
                        level: 5,
                        position: 3,
                        special: 0,
                        sub: false
                    }, {
                        unitId: 408,
                        level: 56,
                        position: 4,
                        special: 0,
                        sub: true
                    }, {
                        unitId: 418,
                        level: 99,
                        position: 5,
                        special: 0,
                        sub: true
                    }
                ],
                shipId: 7
            };

            var parsed = CalcParser.export(<ITeamDetailModel>team);
            expect(parsed).toBe(expected);
        });
});
