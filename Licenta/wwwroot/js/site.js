// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function loadServerPartialView(container, baseUrl) {
    return $.get(baseUrl, function (responseData) {
        $(container).html(responseData);

    });
}

function closeModal(modal) {
    $('#' + modal).modal('hide');
}

function Validate() {
    var correctForm = true;

    if (document.getElementById("Name").value == "") {

        correctForm = false;
    }


    if (document.getElementById("Email").value == "") {

        correctForm = false;
    }


    if (document.getElementById("PhoneNumber").value == "") {

        correctForm = false;
    }

    if (correctForm == false) {

        $("#EditBtn").prop('disabled', true);


    }
    else {
        $("#EditBtn").prop('disabled', false);

    }
}