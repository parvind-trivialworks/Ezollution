function isNumber(evt) {
    evt = (evt) ? evt : window.event;
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
    }
    return true;
}


function Format_Num(num) {
    if (num == null || num == undefined || num == "") {
        return "";
    }
    else {
        return num.toFixed(2);
    }
}
function Format_Num_Decimal(num) {
    if (num == null || num == undefined || num == "") {
        return "0.00";
    }
    else {
        return num.toFixed(2);
    }
}