define(['plugins/dialog', 'durandal/app', 'knockout', 'services/unitService'], function (dialog, app, ko, unitService) {
    var UnitPickerDialog = function () {
        var self = this;

        self.Rows = ko.computed(function () {
            var rows = [];
            var units = ko.unwrap(unitService.Units);
            while (units.length) {
                rows.push(units.splice(0, 6));
            }
            return rows.splice(0, 2);
        });
        self.DisplayName = 'Pick a Unit';
        self.Result = ko.observable();
        self.select = function (id) {
            return function () {
                self.Result(id);
                self.ok();
            };
        };
    };

    UnitPickerDialog.prototype.ok = function () {
        dialog.close(this, ko.unwrap(this.Result));
    };

    UnitPickerDialog.prototype.cancel = function () {
        dialog.close(this);
    };

    UnitPickerDialog.show = function () {
        return app.showDialog(new UnitPickerDialog());
    };

    return UnitPickerDialog;
});