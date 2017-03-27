define(['durandal/app', 'knockout', 'services/unitService'], function (app, ko, unitService) {
    return function () {
        return {
            Id: ko.observable(),
            Name: ko.observable(),
            Type: ko.observable(),
            Class1: ko.observable(),
            Class2: ko.observable(),
            activate: function (model) {
                var self = this;
                var id = ko.unwrap(model.id);
                self.Id(id);
                return unitService.get(id).then(function (result) {
                    result = result[0];
                    self.Name(result.Name);
                    self.Type(result.Type);
                    self.Class1(result.Class1);
                    self.Class2(result.Class2);
                });
            }
        };
    };
});