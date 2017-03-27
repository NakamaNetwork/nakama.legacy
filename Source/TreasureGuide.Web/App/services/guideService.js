define(['services/queryService'], function (queryService) {
    return {
        get: function (id) {
            return queryService.get('/api/guide/get' + (id ? ('?id=' + id) : ''));
        }
    };
});