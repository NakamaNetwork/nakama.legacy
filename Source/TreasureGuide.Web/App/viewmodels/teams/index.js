define(['durandal/app', 'knockout', 'services/teamService'], function (app, ko, teamService) {
    return {
        displayName: 'Teams',
        Teams: ko.observableArray([]),
        activate: function () {
            var self = this;
            return teamService.get().then(function (results) {
                self.Teams(results);
            });
        }
    };
});