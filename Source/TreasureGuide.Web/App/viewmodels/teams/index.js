define(['durandal/app', 'knockout', 'services/teamService'], function (app, ko, teamService) {
    return {
        DisplayName: 'Teams',
        Teams: ko.observableArray([]),
        activate: function () {
            var self = this;
            return teamService.get().then(function (results) {
                self.Teams(results);
            });
        }
    };
});