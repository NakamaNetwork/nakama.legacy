import { HomePage } from '../../../src/views/index';

describe('home page', () => {
    it('says hello', () => {
        expect(new HomePage().message).toBe('Shukko da!');
    });
});
