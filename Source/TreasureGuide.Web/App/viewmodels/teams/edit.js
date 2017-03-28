define(['durandal/app', 'knockout', 'services/stageService', 'services/unitService', 'services/teamService'], function (app, ko, stageService, unitService, teamService) {
    return {
        DisplayName: 'Team Editor',
        TeamModel: {
            Id: ko.observable(0),
            Description: ko.observable(''),
            Credits: ko.observable(''),
            Units: ko.observableArray([]),
            Sockets: ko.observableArray([])
        },
        activate: function (id) {
            var self = this;
            console.debug("Editing Team with Id: " + id);
            self.TeamModel.Id(id || 0);
            if (id) {
                return teamService.get(id).then(function (result) {
                    result = ko.utils.arrayFirst(result);
                    self.TeamModel.Description(result.Description);
                    self.TeamModel.Credits(result.Credits);
                    self.TeamModel.Units(result.Units);
                    self.TeamModel.Sockets(result.Sockets);
                }, function (result) {
                    console.error("Could not find team.");
                });
            }
            return true;
        }
    };
});