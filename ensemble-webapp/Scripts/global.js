function showDiv(id) {
    var element = document.getElementById(id);
    if (element.style.display == "none") {
        element.style.display = "block";
    } else {
        element.style.display = "none";
    }
}

//var isInvalidPassword = false;

//function passwordError() {
//    var element = document.getElementById('edit-pass-row');
//    element.style.display = "block";
//}


// on ready
document.addEventListener("DOMContentLoaded", function (event) {

    jQuery('.datetimepicker').datetimepicker({
        minDate: 0,
        format: 'm/d/Y g:ia',
        formatTime: 'g:ia',
        validateOnBlur: false
    });

    jQuery('.timepicker').datetimepicker({
        datepicker: false,
        format: 'g:ia',
        formatTime: 'g:ia',
        validateOnBlur: false
    });

    //if (isInvalidPassword) {
    //    passwordError();
    //}
});

function AjaxCall(url, data, type) {
    return $.ajax({
        url: url,
        type: type ? type : 'GET',
        data: data,
        contentType: 'application/json',
        async: false
    })
}

function confirmSchedule() {
    AjaxCall('/Schedule/ConfirmSchedule', null, 'POST').done(function (response) {
        $('.not-yet-confirmed').hide();
        $('.confirmed').show();
    });
}

//id is actually start datetime's date in MMddyyyy format
function confirmSingleRehearsal(id) {
    var data = {
        strLocation: $("#" + id + "_strLocation").val(),
        strNotes: $("#" + id + "_strNotes").val(),
        dtmStart: new Date($("#" + id + "_hiddenDateStart").text()),
        dtmEnd: new Date($("#" + id + "_hiddenDateEnd").text())
    };
    console.log(data);
    AjaxCall('/Schedule/ConfirmSingleRehearsal', JSON.stringify(data), 'POST').done(function (response) {
        if (response) {
            // show confirmed row
            showConfirmedRow(id, data);
        } else {
            alert("failed");
        }
    });
}

function showConfirmedRow(id, data) {
    $('#' + id + '_hiddenConfirmLocation').text(data.strLocation);
    $('#' + id + '_hiddenConfirmLocation').css('color', 'green');
    $('#' + id + '_strLocation').hide();
    $('#' + id + '_hiddenConfirmNotes').text(data.strNotes);
    $('#' + id + '_hiddenConfirmNotes').css('color', 'green');
    $('#' + id + '_strNotes').hide();
    $('#' + id + '_confirmedButton').hide();
}