import { HttpEngine } from '../../../src/tools/http-engine';
import { HttpClient } from 'aurelia-fetch-client';

describe('http engine', () => {
    var client = new HttpClient();
    var httpEngine = new HttpEngine(client);

    it('parameterize converts payload to parameters', () => {
        var results = httpEngine.parameterize('test', { alpha: 'alpha', beta: 'beta', zeta: 'ze^^$#$!!!/o-0o0' });
        var expected = 'test?alpha=alpha&beta=beta&zeta=ze%5E%5E%24%23%24!!!%2Fo-0o0';
        expect(results).toBe(expected);
    });

    it('parameterize ignores null and undefined parameters', () => {
        var results = httpEngine.parameterize('test', { alpha: '0', beta: 0, delta: null, epsilon: false, zeta: undefined });
        var expected = 'test?alpha=0&beta=0&epsilon=false';
        expect(results).toBe(expected);
    });
});
