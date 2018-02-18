var modelView = {
    model: undefined,
    action: "Action",
    id: undefined,
    endpoint: undefined,
    redirect: undefined,
    isBusy: ko.observable(false),
    entity: ko.observable(),
    viewDetails: function (model, action,
        id = undefined,
        endpoint = undefined,
        redirect = undefined) {
        var thisObj = this;
        thisObj.model = model;
        thisObj.action = action.toLowerCase();
        thisObj.id = id;
        thisObj.endpoint = endpoint;
        thisObj.redirect = redirect;
        var koModel = `Ko${model}`;
        var endpointUrl = thisObj.buildEndpointUrl(`/${thisObj.model}/Get/${thisObj.id}`);
        var redirectUrl = thisObj.buildRedirectUrl(`/${thisObj.model}`);
        if (null != id) {
            thisObj.isBusy(true);
            get(endpointUrl, true, {})
                .done(function (result) {
                    console.log(result);
                    thisObj.entity(new window[koModel](result));
                    thisObj.isBusy(false);
                }).fail(function (e) {
                    console.log(e.status);
                    console.log(e.statusText);
                    thisObj.isBusy(false);
                    window.location.href = redirectUrl;
                });
        } else {
            thisObj.entity(new window[koModel](guid()));
        }
    },
    createEntity: function (thisObj) {
        var data = ko.toJSON(thisObj.entity);
        var endpoint = thisObj.buildEndpointUrl(`/${thisObj.model}/Post`);
        var redirect = thisObj.buildRedirectUrl(`/${thisObj.model}`);
        var msg = "Successfully added.";
        thisObj.defultOperationEndpoint(data, endpoint, "POST", redirect, msg);
    },
    editEntity: function (thisObj) {
        var data = ko.toJSON(thisObj.entity);
        var endpoint = thisObj.buildEndpointUrl(`/${thisObj.model}/Put/${thisObj.id}`);
        var redirect = thisObj.buildRedirectUrl(`/${thisObj.model}`);
        var msg = "Successfully edited.";
        thisObj.defultOperationEndpoint(data, endpoint, "PUT", redirect, msg);
    },
    deleteEntity: function (thisObj) {
        var data = ko.toJSON(thisObj.entity);
        var endpoint = thisObj.buildEndpointUrl(`/${thisObj.model}/Delete/${thisObj.id}`);
        var redirect = thisObj.buildRedirectUrl(`/${thisObj.model}`);
        var msg = "Successfully deleted.";
        thisObj.defultOperationEndpoint(data, endpoint, "DELETE", redirect, msg);
    },
    buildEndpointUrl: function (defaultUrl) {
        var thisObj = this;
        return (undefined != thisObj.endpoint) ? thisObj.endpoint : defaultUrl;
    },
    buildRedirectUrl: function (defaultUrl) {
        var thisObj = this;
        return (undefined != thisObj.redirect) ? thisObj.redirect : defaultUrl;
    },
    defultOperationEndpoint: function (json, endpoint, verb, redirect, successMessage) {
        var thisObj = this;
        thisObj.isBusy(true);
        send(json, endpoint, verb)
            .done(function (result) {
                console.log(result);
                thisObj.isBusy(false);
                window.location.href = redirect; //+successMessage
            }).fail(function (e) {
                console.log(e.status);
                console.log(e.statusText);
                console.log(e.responseText);
                thisObj.isBusy(false);
                window.location.href = redirect; //+failMessage
            });
    }
};