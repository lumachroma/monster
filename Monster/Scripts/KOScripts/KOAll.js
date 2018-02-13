var modelView = {
    model: undefined,
    isBusy: ko.observable(false),
    list: ko.observableArray([]),
    viewAll: function (model) {
        var thisObj = this;
        thisObj.model = model;
        thisObj.isBusy(true);
        get(`/${thisObj.model}/All`, true, {})
            .done(function (result) {
                console.log(result);
                thisObj.list(result);
                thisObj.isBusy(false);
            }).fail(function (e) {
                console.log(e.status);
                console.log(e.statusText);
                thisObj.isBusy(false);
            });
    }
};