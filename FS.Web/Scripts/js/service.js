(function (fs) {
    "use strict";

    fs.Service = {
        showInfo : function (param, callback) {
            fs.ajaxService.ajaxGetData("Home", "Info", param, callback);
        },
        showStepOne : function (param, callback) {
            fs.ajaxService.ajaxGetData("Home", "Reason", param, callback);
        },

        showStepTwo: function (param, callback) {
            fs.ajaxService.ajaxGetDataWithParam("Home", "FileList", param, callback);
        },

        saveFiles : function (param, callback) {
            fs.ajaxService.ajaxPostData("Home", "SaveFileList", param, callback);
        },

        saveUser : function (param, callback) {
            fs.ajaxService.ajaxPostData("UserLogin", "SaveUser", param, callback);
        },
    };

}(fs));