import { CalcParser } from '../../../src/tools/calc-parser';
describe('calc parser', () => {
    var parser = new CalcParser();

    it('parses out numbers', () => {
        var results = parser.parse(
            'http://optc-db.github.io/damage/#/transfer/D1:5,2:20,3:18:0:36:0,4:99:74:32:80,5:20,6:35C3,10B5454D0E66Q0L0G0R0S100H');
        var expected = [1, 2, 3, 4, 5, 6];

        expect(results.length).toBe(expected.length);
        for (var i = 0; i < expected.length; i++) {
            expect(results[i]).toBe(expected[i]);
        }
    });
});
