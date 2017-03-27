define(['plugins/dialog', 'knockout', 'services/unitService'], function (dialog, ko, unitService) {
    var UnitPickerDialog = function () {
        var self = this;

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

    UnitPickerDialog.prototype.Units = ko.observable([]);

    UnitPickerDialog.show = function () {
        return dialog.show(new UnitPickerDialog());
    };

    unitService.get().then(function (results) {
        UnitPickerDialog.prototype.Units(results);
    });

    return UnitPickerDialog;
});