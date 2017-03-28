"use strict"

define(['durandal/system', 'services/queryService'], function (system, queryService) {
    var item = {
        Timestamp: Math.random(),
        Units: ko.observableArray([]),
        get: function (id) {
            var self = this;
            var units = ko.unwrap(self.Units);
            if (id) {
                var unit = ko.utils.arrayFirst(units,
                    function (unit) {
                        return unit.Id === id;
                    });
                return system.defer(function (deferred) {
                    return deferred.resolve(unit);
                });
            }
            return system.defer(function (deferred) {
                return deferred.resolve(units);
            });
        },
        query: function (id) {
            return queryService.get('/api/unit/get' + (id ? ('?id=' + id) : ''));
        },
        activate: function () {
            var self = this;
            self.query().then(function (result) {
                    self.Units(result);
                });
        }
    }
    item.activate();
    return item;
});