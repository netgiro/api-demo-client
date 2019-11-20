// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(document).ready(function () {
    $("#clientUsed").val(window.location.pathname == "/" ? "RestSharpClient" : window.location.pathname.substr(1));
    $("#clientUsed").change(function (a) {
        window.location = a.currentTarget.value;
    });
});