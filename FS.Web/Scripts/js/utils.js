var hostURL = 'http://localhost:41492/';



var fs = fs || {};



function numbersonly(myfield, e, dec) {
    var key;
    var keychar;

    if (window.event)
        key = window.event.keyCode;
    else if (e)
        key = e.which;
    else
        return true;
    keychar = String.fromCharCode(key);

    // control keys
    if ((key == null) || (key == 0) || (key == 8) ||
        (key == 9) || (key == 13) || (key == 27))
        return true;

        // numbers
    else if ((("0123456789").indexOf(keychar) > -1))
        return true;

        // decimal point jump
    else if (dec && (keychar == ".")) {
        myfield.form.elements[dec].focus();
        return false;
    }
    else
        return false;
};

function printElement(elem, append, delimiter) {
    var domClone = elem.cloneNode(true);

    var $printSection = document.getElementById("printSection");

    if (!$printSection) {
        var $printSection = document.createElement("div");
        $printSection.id = "printSection";
        document.body.appendChild($printSection);
    }

    if (append !== true) {
        $printSection.innerHTML = "";
    }

    else if (append === true) {
        if (typeof (delimiter) === "string") {
            $printSection.innerHTML += delimiter;
        }
        else if (typeof (delimiter) === "object") {
            $printSection.appendChlid(delimiter);
        }
    }

    $printSection.appendChild(domClone);
}



function convertUTCDateToLocalDate(date) {


    return date.toDateString();
}
function readURL(input) {

    if (input.files && input.files[0]) {
        var reader = new FileReader();

        reader.onload = function (e) {
            $('#profilePic').attr('src', e.target.result);
            $('#profilePic').addClass("img-thumbnail");
        }

        reader.readAsDataURL(input.files[0]);
    }
}

$("#PicturePath").change(function () {

    readURL(this);
});


function readFileURL(input) {

    if (input.files && input.files[0]) {
        var reader = new FileReader();

        reader.onload = function (e) {
            $('#pdfLink').attr('href', 'file:///' + $(input).val().replace(/C:\\fakepath\\/i, ''));

        }

        reader.readAsDataURL(input.files[0]);
    }
}
$('#pdfLink').click(function (e) {
    if ($(this).attr('href') == undefined || $(this).attr('href') == "#") {
        alert('Please select a PDF file.');
        e.preventDefault();
    }
});

$("#userFile").change(function () {

    if ($(this).val() == undefined || $(this).val() == "")
        return;
    // Get the file upload control file extension
    var ext = $(this).val().split('.').pop().toLowerCase();

    // Create array with the files extensions to upload
    var fileListToUpload = new Array('pdf', 'doc', 'docx', 'wpd', 'wp', 'wp7', 'wp6', 'wp5', 'wp4', 'wp3', 'pages');

    //Check the file extension is in the array.               
    var isValidFile = $.inArray(ext, fileListToUpload);

    // isValidFile gets the value -1 if the file extension is not in the list.  
    if (isValidFile == -1) {
        alert('Please select a valid file of type pdf, word document, word perfect or pages.');
        $(this).val('');
        return false;
    }
    else {
        // Restrict the file size to 10 MB.
        if ($(this).get(0).files[0].size > ((1024 * 1024) * 10)) {
            alert('File size should not exceed 10 MB.');
            $(this).val('');
            return false;
        }
        else {
            readFileURL(this);
            $("#lblFname").text($(this).val());
            //uploadFile();
            ajaxFileUpload();
        }
    }
});

$("#userFilemain").change(function () {

    if ($(this).val() == undefined || $(this).val() == "")
        return;
    // Get the file upload control file extension
    var ext = $(this).val().split('.').pop().toLowerCase();

    // Create array with the files extensions to upload
    var fileListToUpload = new Array('pdf', 'doc', 'docx', 'wpd', 'wp', 'wp7', 'wp6', 'wp5', 'wp4', 'wp3', 'pages');

    //Check the file extension is in the array.               
    var isValidFile = $.inArray(ext, fileListToUpload);

    // isValidFile gets the value -1 if the file extension is not in the list.  
    if (isValidFile == -1) {
        alert('Please select a valid file of type pdf, word document, word perfect or pages.');
        $(this).val('');
        return false;
    }
    else {
        // Restrict the file size to 10 MB.
        if ($(this).get(0).files[0].size > ((1024 * 1024) * 10)) {
            alert('File size should not exceed 10 MB.');
            $(this).val('');
            return false;
        }
        else {
            readFileURL(this);
            $("#lblFname").text($(this).val());
            //uploadFile();
            additionalFileUpload();
            
        }
    }
});

$(".imagefiles").change(function () {

    if ($(this).val() == undefined || $(this).val() == "")
        return;
    // Get the file upload control file extension
    var ext = $(this).val().split('.').pop().toLowerCase();

    if ($(this).get(0).files[0].size > (1024 * 1024) * 4) {
        alert('File size should not exceed 4 MB.');
        $(this).val('');
        return false;
    }
    else {
        readFileURL(this);
    }
});
var selected = "";
$("#cmbDocs").change(function () {

    $("select option:selected").each(function () {
        $("#txtDocName").val($(this).text());
        selected = $(this).val();
    });

});
$("#fileUpload").click(function (e) {
    $('#userFile').trigger('click');
});
$("#fileUploadmain").click(function (e) {
    $('#userFilemain').trigger('click');
});
$("#profilePic").click(function (e) {
    $('#PicturePath').trigger('click');
});
$("#careREciverPic").click(function (e) {
    $('#PicturePath').trigger('click');
});
//function UploadUserFiles() {
//    $('#userFile').fileupload({
//        dataType: 'json',
//        url: '/UploadCenter/SpecialDocument',
//        autoUpload: false,
//        done: function (e, data) {
//            //$('.file_name').html(data.result.name);
//            //$('.file_type').html(data.result.type);
//            //$('.file_size').html(data.result.size);
//        }
//    }).on('fileuploadprogressall', function (e, data) {
//        var progress = parseInt(data.loaded / data.total * 100, 10);
//        $('.progress .progress-bar').css('width', progress + '%');
//    });

//}

//$("#fileUpload").click(function () {

//});
function uploadFile() {

    $("#uploadProgress").show();
    var formData = $("#frmUpload").serialize();
    //var totalFiles = document.getElementById("userFile").files.length;
    //for (var i = 0; i < totalFiles; i++) {
    //    var file = document.getElementById("userFile").files[i];

    //    formData.append("userFile", file);
    //}

    //var docKey= document.getElementById("docKey").Value;
    //formData.append("DocumentKey", docKey);

    $.ajax({
        type: "POST",
        url: '/UploadCenter/SpecialDocument',
        data: formData,
        dataType: 'json',
        enctype: "multipart/form-data",
        contentType: false,
        processData: false,
        beforeSend: function () {
            $("#uploadProgress").show()
        },
        success: function (response) {
            alert('succes!!');
            $("#uploadProgress").hide()
        },
        error: function (error) {
            alert("errror");
            $("#uploadProgress").hide()
        }
    });

}

//$("#uploadProgress").show();
//$.ajax({
//    url: "/UploadCenter/SpecialDocument",
//    type: "POST",
//    data: $("#frmUpload").serialize(),
//    async: false,
//    cache: false,
//    beforeSend: function () {
//        $("#uploadProgress").show()
//    },
//    complete: function () {
//       // $("#uploadProgress").html("Upload completed");
//    },
//    success: function (msg) {

//        //if (msg == "ok")
//        //    $("#uploadProgress").hide();
//        //else
//        //    alert("Error while uploading");
//         $("#uploadProgress").hide();

//    }


//});


function ajaxFileUpload() {

    $("#loading")
.ajaxStart(function () {
    $(this).show();

    

})
.ajaxComplete(function () {
    $(this).hide();
});

    var jobid = $("#frmUpload input#docKey").val();
    var urlstr = '/UploadCenter/DocumentUpload/' + jobid;
    $.ajaxFileUpload
(
    {


        url: urlstr,
        secureuri: false,
        fileElementId: 'userFile',
        dataType: 'json',
        success: function (data, status) {
            if (typeof (data.error) != 'undefined') {
                if (data.error != '') {
                    alert(data.error);
                } else
                {
                    //alert(data.msg);
                    var deleteLink = "/UploadCenter/DeleteDocument/" + data.id;
                    var showLink = "/Document/" + data.userId + "_" + data.docKey + "/" + data.filename;
                    $("#delPdf").attr("href", deleteLink);
                    $("#pdfLink").attr("href", showLink);
                    $("#filenamediv").html(data.filename);
                    $("#links").show();
                }
            }
        },
        error: function (data, status, e) {
            alert(e);
        }
    }
)

    return false;

}


function additionalFileUpload() {

    $("#loading")
.ajaxStart(function () {
    $(this).show();



})
.ajaxComplete(function () {
    $(this).hide();
});

    //var jobid = $('#frmAdditonalFiles input#docKey').val();
    //$("select option:selected").each(function () {
    //    jobid = $(this).val();
    //});

    if (selected == "" || selected == null || selected == undefined) {
        alert("Please select a document");
        return;
    }



    var urlstr = '/UploadCenter/DocumentAdditionalUpload/' + selected;
    $.ajaxFileUpload
(
    {


        url: urlstr,
        secureuri: false,
        fileElementId: 'userFilemain',
        dataType: 'json',
        success: function (data) {
            if (typeof (data.error) != 'undefined') {
                if (data.error != '') {
                    alert(data.error);
                } else {
                    //alert(data.msg);
                    //var deleteLink = "/UploadCenter/DocumentAdditionalUpload/" + data.id;
                    //var showLink = "/Document/" + data.userId + "_" + data.docKey + "/" + data.filename;
                  
                    //$("#divFileList").html(data);

                    window.location.href = "/UploadCenter/AdditionalFiles/";
                  
                    selected = "";
                }
            }
        },
        error: function (data, status, e) {
            alert(e);
        }
    }
)

    return false;

}