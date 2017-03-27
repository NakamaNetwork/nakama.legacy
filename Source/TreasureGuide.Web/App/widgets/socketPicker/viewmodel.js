define(['durandal/app', 'knockout', 'services/unitService'], function (app, ko, unitService) {
    return {
        Sockets: ko.observableArray([]),
        activate: function (sockets) {
            var self = this;
            self.Sockets = sockets;
            return;
        }
    };
});