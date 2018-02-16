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
    createEntity: function (thisObj) {
        var data = ko.toJSON(thisObj.entity);
        var endpoint = `/${thisObj.model}/Post`;
        var redirect = `/${thisObj.model}`;
        var msg = "Successfully added.";
        //console.log(data);
        thisObj.defultOperationEndpoint(data, endpoint, "POST", redirect, msg);
    },
    editEntity: function (thisObj) {
        var data = ko.toJSON(thisObj.entity);
        var endpoint = `/${thisObj.model}/Put/${thisObj.id}`;
        var redirect = `/${thisObj.model}`;
        var msg = "Successfully edited.";
        //console.log(data);
        thisObj.defultOperationEndpoint(data, endpoint, "PUT", redirect, msg);
    },
    deleteEntity: function (thisObj) {
        var data = ko.toJSON(thisObj.entity);
        var endpoint = `/${thisObj.model}/Delete/${thisObj.id}`;
        var redirect = `/${thisObj.model}`;
        var msg = "Successfully deleted.";
        //console.log(data);
        //app.showMessage("Are you sure you want to delete?", config.application_name, ["Yes", "No"])
        //    .done(function (result) {
        //        if (result === "No") {
        //            return;
        //        }
        thisObj.defultOperationEndpoint(data, endpoint, "DELETE", redirect, msg);
        //    });
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