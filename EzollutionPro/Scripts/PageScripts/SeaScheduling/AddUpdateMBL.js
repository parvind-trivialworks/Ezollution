$(function () {
    $('#sIGMDate').datepicker({
        format: "dd/mm/yyyy",
        endDate: 'today',
        autoclose: true
    });
    $('#sMBLDate').datepicker({
        format: "dd/mm/yyyy",
        endDate: 'today',
        autoclose: true
    });
    if ($('#sCargoMovement').val() == "LC") {
        $('#sCFSCode').rules('add', 'required')
    }
    else {
        $('#sCFSCode').rules('remove', 'required')
    }
    $('#iPOSId').selectpicker();
    $('#sCargoMovement').selectpicker();
}); 
$(document).on('change', '#sCargoMovement', function () {
    if ($('#sCargoMovement').val() == "LC") {
        $('#sCFSCode').rules('add', 'required')
    }
    else {
        $('#sCFSCode').rules('remove', 'required')
    }
});