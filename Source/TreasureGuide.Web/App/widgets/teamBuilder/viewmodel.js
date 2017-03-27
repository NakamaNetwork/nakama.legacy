define(['durandal/app', 'knockout', 'services/unitService', 'modals/unitPicker'], function (app, ko, unitService, unitPicker) {
    return {
        displayName: 'Team Builder',
        Units: ko.observableArray([]),
        activate: function (units, enemy) {
            var self = this;
            console.log(units);
            self.Units = units;
            return;
        },
        addUnit: function () {
            console.debug('Adding unit');
            unitPicker.show().then(function (result) {
                console.debug("Picked " + result);
            });
        }
    };
});