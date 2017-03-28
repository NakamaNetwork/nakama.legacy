define(['durandal/app', 'knockout', 'services/unitService', 'modals/unitPicker'], function (app, ko, unitService, unitPicker) {
    return {
        DisplayName: 'Team Builder',
        Units: ko.observableArray([]),
        activate: function (model) {
            var self = this;
            var units = model.Units;
            if (units) {
                self.Units = units;
            }
            return;
        },
        addUnit: function () {
            var self = this;
            unitPicker.show().then(function (result) {
                if (result) {
                    self.Units.push({
                        Id: result,
                        Position: 0
                    });
                }
            });
        }
    };
});