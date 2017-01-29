define(['queryService'], function (queryService) {
    return {
        get: function (id) {
            return queryService.get('/api/guides/get' + (id ? ('/' + id) : ''));
        }
    };
});