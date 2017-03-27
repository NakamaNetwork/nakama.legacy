define(['services/queryService'], function (queryService) {
    return {
        get: function (id) {
            return queryService.get('/api/unit/get' + (id ? ('/' + id) : ''));
        }
    };
});