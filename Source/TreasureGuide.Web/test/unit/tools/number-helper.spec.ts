import { UnitType } from '../../../src/models/imported';
import { NumberHelper } from '../../../src/tools/number-helper';
describe('number helper', () => {
    it('can split enums', () => {
        var enums = [UnitType.DEX, UnitType.STR, UnitType.QCK];
        var value = enums.reduce((x, y) => x |= y);
        var split = NumberHelper.splitEnum(value, UnitType);
        enums.forEach(x => {
            expect(split).toContain(x);
        });
    });
});
