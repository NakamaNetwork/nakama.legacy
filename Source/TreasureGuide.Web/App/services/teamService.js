define(['services/queryService'], function (queryService) {
    return {
        get: function (id) {
            return queryService.get('/api/team/get' + (id ? ('/' + id) : ''));
        }
    };
});