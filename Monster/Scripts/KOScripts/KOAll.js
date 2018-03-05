var modelView = {
    model: undefined,
    endpoint: undefined,
    isBusy: ko.observable(false),
    list: ko.observableArray([]),
    viewAll: function (model, endpoint) {
        var thisObj = this;
        thisObj.model = model;
        thisObj.endpoint = endpoint;
        thisObj.isBusy(true);
        var endpointUrl = thisObj.buildEndpointUrl("/" + thisObj.model + "/All");
        get(endpointUrl, true, {})
            .done(function (result) {
                console.log(result);
                thisObj.list(result);
                thisObj.isBusy(false);
            }).fail(function (e) {
                console.log(e.status);
                console.log(e.statusText);
                thisObj.isBusy(false);
            });
    },
    buildEndpointUrl: function (defaultUrl) {
        var thisObj = this;
        return (undefined != thisObj.endpoint) ? thisObj.endpoint : defaultUrl;
    },
    refresh: function (thisObj) {
        thisObj.viewAll(thisObj.model, thisObj.endpoint);
    }
};