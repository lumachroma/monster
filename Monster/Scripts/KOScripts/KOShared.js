function send(json, url, verb) {
    var tcs = new $.Deferred();
    $.ajax({
        type: verb,
        data: json,
        url: url,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        error: tcs.reject,
        success: tcs.resolve
    });
    return tcs.promise();
}

function get(url, cache, headers) {
    var tcs = new $.Deferred();
    $.ajax({
        type: "GET",
        cache: cache,
        headers: headers,
        url: url,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        error: tcs.reject,
        success: tcs.resolve
    });
    return tcs.promise();
}

function post(json, url, headers) {
    var tcs = new $.Deferred();
    $.ajax({
        type: "POST",
        data: json,
        headers: headers,
        url: url,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        error: tcs.reject,
        success: tcs.resolve
    });
    return tcs.promise();
}

function put(json, url, headers) {
    var tcs = new $.Deferred();
    $.ajax({
        type: "PUT",
        data: json,
        headers: headers,
        url: url,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        error: tcs.reject,
        success: tcs.resolve
    });
    return tcs.promise();
}

function patch(json, url, headers) {
    var tcs = new $.Deferred();
    $.ajax({
        type: "PATCH",
        data: json,
        headers: headers,
        url: url,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        error: tcs.reject,
        success: tcs.resolve
    });
    return tcs.promise();
}

function remove(url) {
    var tcs = new $.Deferred();
    $.ajax({
        type: "DELETE",
        data: "{}",
        url: url,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        error: tcs.reject,
        success: tcs.resolve
    });
    return tcs.promise();
}

//https://gist.github.com/jed/982883
function guid() {
    return function b(a) {
        return a
            ? (a ^ Math.random() * 16 >> a / 4).toString(16)
            : ([1e7] + -1e3 + -4e3 + -8e3 + -1e11).replace(/[018]/g, b);
    }();
}