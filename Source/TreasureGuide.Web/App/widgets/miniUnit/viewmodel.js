define(['durandal/app', 'knockout', 'services/unitService'], function (app, ko, unitService) {
    return function () {
        return {
            Id: ko.observable(),
            Size: ko.observable('md'),
            Name: ko.observable(),
            Type: ko.observable(),
            Class1: ko.observable(),
            Class2: ko.observable(),
            BackgroundImage: ko.observable('https://onepiece-treasurecruise.com/wp-content/themes/onepiece-treasurecruise/images/noimage.png'),
            activate: function (model) {
                var self = this;
                var id = ko.unwrap(model.id);
                var size = ko.unwrap(model.size);
                self.Id(id);
                self.Size(size);
                return unitService.get(id).then(function (result) {
                    if (result) {
                        self.Name(result.Name);
                        self.Type(result.Type);
                        self.Class1(result.Class1);
                        self.Class2(result.Class2);
                        var images = ("0000" + id).slice(-4);
                        self.BackgroundImage('https://onepiece-treasurecruise.com/wp-content/uploads/f' + images + '.png');
                    }
                });
            }
        };
    };
});