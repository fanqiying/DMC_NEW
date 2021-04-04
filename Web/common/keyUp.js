$(document).ready(function () {
    $("#divSearch").keyup(function () {
        var a = window.event.keyCode;
        if ((a == 13)) {
            Search('ByAdvanced');
        }
    });
    $("#divOperation").keyup(function () {
        var a = window.event.keyCode;
        if ((a == 13)) {
            Search('ByKey');
        }
    });
});