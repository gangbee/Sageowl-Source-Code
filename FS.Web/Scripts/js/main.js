


fs.RegistrationEvents = function () {


    //var onGetStartedClick = function () {
        
    //    $("#registerModels").modal('show');
    //    fs.Service.showInfo(null, function (data) {

    //        $("#register-content").html(data);

    //    });
    //};


    //var onStepOne = function (param) {
       
    //    fs.Service.showStepOne(null, function (data) {

    //        $("#register-content").html(data);

    //    });

    //};


    //var onStepTwo = function (param) {
    //    if (param == null)
    //        param = $('#cmbReason').val();
    //    fs.Service.showStepTwo(param, function (data) {


    //        $("#register-content").html(data);

    //    });
    //}

        

    //    var onStepThree = function (param) {
    //        var names=[];
    //        $('#chkFiles input[type="checkbox"]:checked').each(function (i, el){
    //            names.push(el.value);
    //        });

    //        var frmdata = $("#frmFileList").serialize();
    //        fs.Service.saveFiles(frmdata, function (data) {

            
    //            $("#register-content").html(data);

    //        });
    //    };

    //    var saveUser = function (param) {
           
    //        //$('#frmRegister').submit(function (evt) {
    //        //    evt.preventDefault();
    //        //    var $form = $(this);
              
    //        //    if ($form.valid()) {
    //        //        //Ajax call here


                   
    //        //    }
    //        //});
    //        ////$('#frmRegister').validate();
    //       // $('#frmRegister').submit();
    //        var frmdata = $("#frmRegister").serialize();
    //        fs.Service.saveUser(frmdata, function (data) {


    //            $("#register-content").html(data);

    //        });
    //    };

        var OnAddInstituteClick = function () {

            $("#instituteModels").modal('show');
            
        };
        return {
            OnAddInstituteClick:OnAddInstituteClick
        //onGetStartedClick: onGetStartedClick,
        //onStepOne: onStepOne,
        //onStepTwo: onStepTwo,
        //onStepThree: onStepThree,
        //saveUser: saveUser
    };
}();