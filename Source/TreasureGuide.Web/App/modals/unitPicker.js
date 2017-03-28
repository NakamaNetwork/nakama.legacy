define(['plugins/dialog', 'knockout', 'services/unitService'], function (dialog, ko, unitService) {
    var UnitPickerDialog = function () {
        var self = this;

        self.Rows = ko.computed(function () {
            var rows = [];
            var units = ko.unwrap(self.Units);
            while (units.length) {
                rows.push(units.splice(0, 10));
            }
            return rows;
        });
        self.displayName = 'Pick a Unit';
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
        return dialog.show(new UnitPickerDialog());
    };

    UnitPickerDialog.prototype.Units = ko.observable([]);

    unitService.get().then(function (results) {
        UnitPickerDialog.prototype.Units(results);
    });

    return UnitPickerDialog;
});