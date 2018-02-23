var KoAuction = function (optionOrWebid) {

    var model = {
        Id: ko.observable("0"),
        Title: ko.observable(),
        Status: ko.observable(),
        ProductName: ko.observable(),
        ProductImageUrl: ko.observable(),
        ProductUrl: ko.observable(),
        ProductDescription: ko.observable(),
        ProductPrice: ko.observable(),
        Amount: ko.observable(),
        Step: ko.observable(),
        Call: ko.observable(),
        Interval: ko.observable(),
        StartDateTime: ko.observable(),
        StopDateTime: ko.observable(),
        Logs: ko.observableArray([]),
        Bidders: ko.observableArray([]),
        WebId: ko.observable(),

        addChildItem: function (list, type) {
            if (typeof type === "function") {
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
        if (optionOrWebid.Title) {
            model.Title(optionOrWebid.Title);
        }
        if (optionOrWebid.Status) {
            model.Status(optionOrWebid.Status);
        }
        if (optionOrWebid.ProductName) {
            model.ProductName(optionOrWebid.ProductName);
        }
        if (optionOrWebid.ProductImageUrl) {
            model.ProductImageUrl(optionOrWebid.ProductImageUrl);
        }
        if (optionOrWebid.ProductUrl) {
            model.ProductUrl(optionOrWebid.ProductUrl);
        }
        if (optionOrWebid.ProductDescription) {
            model.ProductDescription(optionOrWebid.ProductDescription);
        }
        if (optionOrWebid.ProductPrice) {
            model.ProductPrice(optionOrWebid.ProductPrice);
        }
        if (optionOrWebid.Amount) {
            model.Amount(optionOrWebid.Amount);
        }
        if (optionOrWebid.Step) {
            model.Step(optionOrWebid.Step);
        }
        if (optionOrWebid.Call) {
            model.Call(optionOrWebid.Call);
        }
        if (optionOrWebid.Interval) {
            model.Interval(optionOrWebid.Interval);
        }
        if (optionOrWebid.StartDateTime) {
            model.StartDateTime(optionOrWebid.StartDateTime);
        }
        if (optionOrWebid.StopDateTime) {
            model.StopDateTime(optionOrWebid.StopDateTime);
        }
        if (optionOrWebid.Logs) {
            var logsList = $.map(optionOrWebid.Logs, function (v, i) {
                return new KoLog(v);
            });
            model.Logs(logsList);
        }
        if (optionOrWebid.Bidders) {
            var biddersList = $.map(optionOrWebid.Bidders, function (v, i) {
                return new KoBidder(v);
            });
            model.Bidders(biddersList);
        }
    }

    if (optionOrWebid && typeof optionOrWebid === "string") {
        model.WebId(optionOrWebid);
    }

    return model;
};

var KoLog = function (optionOrWebid) {

    var model = {
        Id: ko.observable("0"),
        Nickname: ko.observable(),
        Email: ko.observable(),
        Text: ko.observable(),
        WebId: ko.observable(),

        addChildItem: function (list, type) {
            if (typeof type === "function") {
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
        if (optionOrWebid.Nickname) {
            model.Nickname(optionOrWebid.Nickname);
        }
        if (optionOrWebid.Email) {
            model.Email(optionOrWebid.Email);
        }
        if (optionOrWebid.Text) {
            model.Text(optionOrWebid.Text);
        }
    }

    if (optionOrWebid && typeof optionOrWebid === "string") {
        model.WebId(optionOrWebid);
    }

    return model;
};

var KoBidder = function (optionOrWebid) {

    var model = {
        Id: ko.observable("0"),
        Nickname: ko.observable(),
        Email: ko.observable(),
        Code: ko.observable(),
        WebId: ko.observable(),

        addChildItem: function (list, type) {
            if (typeof type === "function") {
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
        if (optionOrWebid.Nickname) {
            model.Nickname(optionOrWebid.Nickname);
        }
        if (optionOrWebid.Email) {
            model.Email(optionOrWebid.Email);
        }
        if (optionOrWebid.Code) {
            model.Code(optionOrWebid.Code);
        }
    }

    if (optionOrWebid && typeof optionOrWebid === "string") {
        model.WebId(optionOrWebid);
    }

    return model;
};