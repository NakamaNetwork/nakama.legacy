import { CalcParser } from '../../../src/tools/calc-parser';
describe('calc parser', () => {
    var parser = new CalcParser();

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

    it('parses out numbers', () => {
        var results = parser.parse(
            'http://optc-db.github.io/damage/#/transfer/D1:5,2:20,3:18:0:36:0,4:99:74:32:80,5:20,6:35C3,10B5454D0E66Q0L0G0R0S100H');
        var expected = [1, 2, 3, 4, 5, 6];

        expect(results.length).toBe(expected.length);
        for (var i = 0; i < expected.length; i++) {
            expect(results[i]).toBe(expected[i]);
        }
    });

    it('parses team members',
        () => {
            var results = parser.convert([12, 23, 34, 45, 56, 67]);
            var expected = [
                {
                    unitId: 12,
                    position: 0,
                    specialLevel: 0,
                    sub: false
                }, {
                    unitId: 23,
                    position: 1,
                    specialLevel: 0,
                    sub: false
                }, {
                    unitId: 34,
                    position: 2,
                    specialLevel: 0,
                    sub: false
                }, {
                    unitId: 45,
                    position: 3,
                    specialLevel: 0,
                    sub: false
                }, {
                    unitId: 56,
                    position: 4,
                    specialLevel: 0,
                    sub: false
                }, {
                    unitId: 67,
                    position: 5,
                    specialLevel: 0,
                    sub: false
                }
            ];
            check_parsed(results, expected);
        });

    it("makes extra units subs",
        () => {
            var results = parser.convert([12, 23, 34, 45, 56, 67, 12, 54, 32]);
            var expected = [
                {
                    unitId: 12,
                    position: 0,
                    specialLevel: 0,
                    sub: false
                }, {
                    unitId: 23,
                    position: 1,
                    specialLevel: 0,
                    sub: false
                }, {
                    unitId: 34,
                    position: 2,
                    specialLevel: 0,
                    sub: false
                }, {
                    unitId: 45,
                    position: 3,
                    specialLevel: 0,
                    sub: false
                }, {
                    unitId: 56,
                    position: 4,
                    specialLevel: 0,
                    sub: false
                }, {
                    unitId: 67,
                    position: 5,
                    specialLevel: 0,
                    sub: false
                }, {
                    unitId: 12,
                    position: null,
                    specialLevel: 0,
                    sub: true
                }, {
                    unitId: 54,
                    position: null,
                    specialLevel: 0,
                    sub: true
                }, {
                    unitId: 32,
                    position: null,
                    specialLevel: 0,
                    sub: true
                }
            ];
            check_parsed(results, expected);
        });
});
