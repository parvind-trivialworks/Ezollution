$(function () {
    $('#sHouseBillofLadingDate').datepicker({
        format: "dd/mm/yyyy",
        endDate: 'today',
        autoclose: true
    });
    $('#sPackageCode').selectpicker();
});