define(['durandal/app', 'knockout', 'services/guideService'], function (app, ko, guideService) {
    return {
        displayName: 'Guides',
        stages: ko.observableArray([]),
        activate: function () {
            var self = this;
            return guideService.get().then(function(results) {
                self.stages(results);
            });
        }
    };
});