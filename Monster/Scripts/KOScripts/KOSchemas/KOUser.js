var KoUser = function (optionOrWebid) {

    var model = {
        Id: ko.observable("0"),
        Username: ko.observable(),
        Password: ko.observable(),
        Email: ko.observable(),
        Location: ko.observable(),
        Country: ko.observable(),
        Roles: ko.observableArray([]),
        WebId: ko.observable(),

        addChildItem: function (list, type) {
            if (typeof type === "object") {
                return function () {
                    list.push(new type(guid()));
                };
            }
            return function () {
                list.push(type);
            };
        },

        removeChildItem: function (list, obj) {
            return function () {
                list.remove(obj);
            };
        }
    };

    if (typeof optionOrWebid === "object") {
        if (optionOrWebid.Id) {
            model.Id(optionOrWebid.Id);
        }
        if (optionOrWebid.WebId) {
            model.WebId(optionOrWebid.WebId);
        }
        if (optionOrWebid.Username) {
            model.Username(optionOrWebid.Username);
        }
        if (optionOrWebid.Password) {
            model.Password(optionOrWebid.Password);
        }
        if (optionOrWebid.Email) {
            model.Email(optionOrWebid.Email);
        }
        if (optionOrWebid.Location) {
            model.Location(optionOrWebid.Location);
        }
        if (optionOrWebid.Country) {
            model.Country(optionOrWebid.Country);
        }
        if (optionOrWebid.Roles) {
            model.Roles(optionOrWebid.Roles);
        }
    }

    if (optionOrWebid && typeof optionOrWebid === "string") {
        model.WebId(optionOrWebid);
    }

    return model;
};