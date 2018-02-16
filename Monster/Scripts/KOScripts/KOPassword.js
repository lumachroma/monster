var modelView = {
    model: undefined,
    action: "Action",
    id: undefined,
    isBusy: ko.observable(false),
    entity: ko.observable(),
    viewDetails: function (model, action, id = null) {
        var thisObj = this;
        thisObj.model = model;
        thisObj.action = action.toLowerCase();
        thisObj.id = id;
        var koModel = `Ko${model}`;
        if (null != id) {
            thisObj.isBusy(true);
            get(`/${thisObj.model}/Get/${thisObj.id}`, true, {})
                .done(function (result) {
                    console.log(result);
                    thisObj.entity(new window[koModel](result));
                    thisObj.isBusy(false);
                }).fail(function (e) {
                    console.log(e.status);
                    console.log(e.statusText);
                    thisObj.isBusy(false);
                    window.location.href = `/${thisObj.model}`;
                });
        } else {
            thisObj.entity(new window[koModel](guid()));
        }
    },
    setEntityPasssword: function (thisObj) {
        var data = ko.toJSON(thisObj.entity);
        var endpoint = `/${thisObj.model}/SetPassword/${thisObj.id}`;
        var redirect = `/${thisObj.model}`;
        var msg = "Successfully set password.";
        //console.log(data);
        thisObj.defultOperationEndpoint(data, endpoint, "PATCH", redirect, msg);
    },
    defultOperationEndpoint: function (json, endpoint, verb, redirect, successMessage) {
        var thisObj = this;
        thisObj.isBusy(true);
        send(json, endpoint, verb)
            .done(function (result) {
                console.log(result);
                thisObj.isBusy(false);
                //app.showMessage(successMessage, config.application_name, ["OK"])
                //    .done(function (result) {
                //        if (result == "OK") {
                window.location.href = redirect;
                //        }
                //    });
            }).fail(function (e) {
                console.log(e.status);
                console.log(e.statusText);
                console.log(e.responseText);
                thisObj.isBusy(false);
                //app.showMessage(failMessage, config.application_name, ["OK"])
                //    .done(function (result) {
                //        if (result == "OK") {
                window.location.href = redirect;
                //        }
                //    });
            });
    }
};