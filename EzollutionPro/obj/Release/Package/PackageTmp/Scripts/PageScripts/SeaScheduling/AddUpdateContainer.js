$(function () {
    $('#sISOCode').selectpicker();
    $('#sContainerStatus').selectpicker();
});
$(document).on('keyup', 'input[type="text"],textarea', function () {
    $(this).val($(this).val().toUpperCase());
});