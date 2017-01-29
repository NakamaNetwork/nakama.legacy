define(['services/queryService'], function (queryService) {
    return {
        get: function (id) {
            return queryService.get('/api/stage/get' + (id ? ('/' + id) : ''));
        }
    };
});