var modelView = {
    id: undefined,
    controller: undefined,
    endpoint: undefined,
    redirect: undefined,
    isBusy: ko.observable(false),
    isError: ko.observable(false),
    isAuth: ko.observable(false),
    entity: ko.observable(),
    bidderEmail: ko.observable(),
    bidderCode: ko.observable(),
    bidderNickname: ko.observable(),
    executeEndpointLogic: function () { },
    callTimerIntervalID: undefined,
    callTimerElapse: ko.observable(0),
    initDetails: function (id, controller, endpoint = undefined, redirect = undefined) {
        var thisObj = this;
        thisObj.id = id;
        thisObj.controller = controller;
        thisObj.endpoint = endpoint;
        thisObj.redirect = redirect;
        thisObj.entity(new KoAuction(guid())); //??
    },
    updateAuctioneerDetails: function (auction) {
        var thisObj = this;
        thisObj.entity(new KoAuction(auction));
        thisObj.entity().Logs.reverse();
        if (null != thisObj.callTimerIntervalID) thisObj.stopCallTimer();
        thisObj.resetCallTimer(auction);
    },
    updateBidderDetails: function (auction) {
        var thisObj = this;
        thisObj.entity(new KoAuction(auction));
        thisObj.entity().Logs.reverse();
    },
    resetCallTimer: function (auction) {
        var thisObj = this;
        if (auction.Status === "Started") {
            thisObj.callTimerIntervalID = setInterval(function () {
                var startTime = moment(auction.ChangedDate).valueOf();
                var elapsedTime = Date.now() - startTime;
                var elapsedTimeInSeconds = (elapsedTime / 1000).toFixed(2);
                thisObj.callTimerElapse(elapsedTimeInSeconds);
                if (elapsedTimeInSeconds > auction.Interval) {
                    console.log(`${elapsedTimeInSeconds}: Time's up. Call nhawww!!!`);
                    thisObj.stopCallTimer();
                    thisObj.performCall(thisObj);
                }
            },
                1000);
        }
    },
    stopCallTimer: function () {
        var thisObj = this;
        clearInterval(thisObj.callTimerIntervalID);
        thisObj.callTimerIntervalID = undefined;
    },
    verifyBidder: function (thisObj, entity) {
        //console.log(thisObj);
        //console.log(entity);
        var data = JSON.stringify({ "BidderEmail": thisObj.bidderEmail(), "BidderCode": thisObj.bidderCode() });
        var endpoint = thisObj.buildEndpointUrl(`/${thisObj.controller}/VerifyBidder/${thisObj.id}`);
        thisObj.executeEndpointLogic = function (result) {
            thisObj.isAuth(result.Auth);
            thisObj.bidderNickname(result.Nickname);
        };
        thisObj.defultOperationEndpoint(data, endpoint, "POST", thisObj.redirect);
    },
    performBid: function (thisObj, entity) {
        //console.log(thisObj);
        //console.log(entity);
        var bidAmount = entity.Amount() + entity.Step();
        var data = JSON.stringify({
            "BidderEmail": thisObj.bidderEmail(),
            "BidderCode": thisObj.bidderCode(),
            "BidAmount": bidAmount
        });
        var endpoint = thisObj.buildEndpointUrl(`/${thisObj.controller}/PerformBid/${thisObj.id}`);
        thisObj.executeEndpointLogic = function () { };
        thisObj.defultOperationEndpoint(data, endpoint, "POST", thisObj.redirect);
    },
    performStart: function (thisObj, entity) {
        //console.log(thisObj);
        //console.log(entity);
        var data = JSON.stringify({});
        var endpoint = thisObj.buildEndpointUrl(`/${thisObj.controller}/PerformStart/${thisObj.id}`);
        thisObj.executeEndpointLogic = function () { };
        thisObj.defultOperationEndpoint(data, endpoint, "POST", thisObj.redirect);
    },
    performCall: function (thisObj, entity) {
        //console.log(thisObj);
        //console.log(entity);
        var data = JSON.stringify({});
        var endpoint = thisObj.buildEndpointUrl(`/${thisObj.controller}/PerformCall/${thisObj.id}`);
        thisObj.executeEndpointLogic = function () { };
        thisObj.defultOperationEndpoint(data, endpoint, "POST", thisObj.redirect);
    },
    buildEndpointUrl: function (defaultUrl) {
        var thisObj = this;
        return (undefined != thisObj.endpoint) ? thisObj.endpoint : defaultUrl;
    },
    buildRedirectUrl: function (defaultUrl) {
        var thisObj = this;
        return (undefined != thisObj.redirect) ? thisObj.redirect : defaultUrl;
    },
    defultOperationEndpoint: function (json, endpoint, verb, redirect) {
        var thisObj = this;
        thisObj.isBusy(true);
        send(json, endpoint, verb)
            .done(function (result) {
                console.log(result);
                thisObj.isBusy(false);
                thisObj.executeEndpointLogic(result);
                //window.location.href = redirect; //+successMessage
            }).fail(function (e) {
                console.log(e.status);
                console.log(e.statusText);
                console.log(e.responseText);
                thisObj.isBusy(false);
                //window.location.href = redirect; //+failMessage
            });
    }
};