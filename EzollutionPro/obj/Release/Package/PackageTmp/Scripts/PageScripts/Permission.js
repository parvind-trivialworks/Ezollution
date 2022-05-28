$(document).on('click', '.permission', function () {
    var permissionIds = [];
    $('.permission:checked').each(function (i, val) {
        permissionIds.push($(val).data('id'));
    });
    $.ajax({
        url: '/Permission/AddUpdatePermissions',
        method:'POST',
        data: { permissions: permissionIds, RoleId: $('#iRoleId').val() },
        async: false,
        success: function (res) {
            if (res.Status)
                toastr.success(res.Message);
            else
                toastr.error(res.Message);
        }
    });
});

$(document).on('change', '#iRoleId', function () {
    $('.permission').removeAttr('checked');
    if ($(this).val() !== "") {
        $.ajax({
            url: '/Permission/GetPermissions',
            data: { iRoleId: $('#iRoleId').val() },
            async: false,
            success: function (res) {
                res.forEach(function (val, i) {
                    $('.permission[data-id="' + val + '"]').attr('checked', 'checked');
                })
            }
        });
    }
})