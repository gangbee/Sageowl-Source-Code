(function (fs) {
    "use strict";

    //var culture = $('#hdnCulture').val();

    var serviceBase = hostURL ,
        getSvcUrl = function (controller, method) { return serviceBase + controller + "/" + method; };

    var getSvcUrlWithParam = function (controller, method,param) { return serviceBase + controller + "/" + method + "/" + param; };

    fs.ajaxService = (function () {
        var ajaxGetJson = function (controller, method, param, callback) {
            $.ajax({
                url: getSvcUrl(controller, method),
                type: "GET",
                cache: false,
                data: param,
                dataType: "json",
                contentType: 'application/json; charset=utf-8',
                beforeSend: function () {
                    $('#loader').show();
                },
                success: function (json) {
                    callback(json);
                    $('#loader').hide();
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    $('#loader').hide();
                    alert(xhr.status);
                    alert(thrownError);
                }
            });
        },

         ajaxGetJsonByParam = function (controller, method, param, callback) {
            $.ajax({
                url: getSvcUrlWithParam(controller, method, param),
                type: "GET",
                cache: false,
                //data: param,
                dataType: "json",
                contentType: 'application/json; charset=utf-8',
                beforeSend: function () {
                    $('#loader').show();
                },
                success: function (json) {
                    callback(json);
                    $('#loader').hide();
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    $('#loader').hide();
                    alert(xhr.status);
                    alert(thrownError);
                }
            });
        },
        ajaxPostJson = function (controller, method, jsonIn, callback,errorCallback) {
                $.ajax({
                    url: getSvcUrl(controller, method),
                    type: "POST",
                    data: JSON.stringify(jsonIn),
                    dataType: "json",
                    contentType: 'application/json; charset=utf-8',
                    beforeSend: function () {
                        $('#loader').show();
                    },
                    success: function (json) {
                        callback(json);
                        $('#loader').hide();
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        $('#loader').hide();
                        if (errorCallback != undefined)
                            errorCallback(xhr);
                        alert(thrownError);
                    }
                });
            },
        ajaxGetData = function (controller, method, param, callback) {
            $.ajax({
                url: getSvcUrl(controller, method),
                type: "GET",
                cache: false,
                data: param,
                beforeSend: function () {
                    $('#loader').show();
                },
                success: function (data) {
                    callback(data);
                    $('#loader').hide();
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    $('#loader').hide();
                    alert(xhr.status);
                    alert(thrownError);
                }
            });
        };

        var ajaxGetDataWithParam = function (controller, method, param, callback) {
            $.ajax({
                url: getSvcUrlWithParam(controller, method, param),
                type: "GET",
                cache: false,
                data: param,
                beforeSend: function () {
                    $('#loader').show();
                },
                success: function (data) {
                    callback(data);
                    $('#loader').hide();
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    $('#loader').hide();
                    alert(xhr.status);
                    alert(thrownError);
                }
            });
        };

        var ajaxPostData = function (controller, method, param, callback) {
            $.ajax({
                url: getSvcUrl(controller, method),
                type: "POST",
                cache: false,
                data: param,
                beforeSend: function () {
                    $('#loader').show();
                },
                success: function (data) {
                    callback(data);
                    $('#loader').hide();
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    $('#loader').hide();
                    alert(xhr.status);
                    alert(thrownError);
                }
            });
        };

        return {
            ajaxGetJson: ajaxGetJson,
            ajaxPostJson: ajaxPostJson,
            ajaxGetData: ajaxGetData,
            ajaxPostData: ajaxPostData,
            ajaxGetJsonByParam: ajaxGetJsonByParam,
            ajaxGetDataWithParam: ajaxGetDataWithParam
        };
    })();
}(fs));