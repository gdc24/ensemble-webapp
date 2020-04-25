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


//// on ready
//document.addEventListener("DOMContentLoaded", function (event) {
//    if (isInvalidPassword) {
//        passwordError();
//    }
//});