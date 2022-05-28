$(function () {
    $('#tblRole').DataTable({
        processing: true,
        serverSide: true,
        'searching': false,
        'orderable': false,
        'ajax': {
            url: '/Role/GetRoles',
            method: 'POST',
            async:false
        },
        'columns': [
            { "data": "sRoleName", 'orderable': false },
            { "data": "sDescription", 'orderable': false },
            {
                "data": "iRoleId", 'orderable': false, 'mRender': function (data, abc, full) {
                    return '<button type="button" onClick="AddRole('+data+')" class="btn btn-xs btn-warning"><i class="fa fa-edit"></i></button>'
                }
            }]
    })
});
function AddRole(iRoleId) {
    $('#ModalContainer').load('/Role/AddUpdateRole?iRoleId='+iRoleId, function () {
        $.validator.unobtrusive.parse('#frmSaveRole');
        $('#Modal').modal('show');
    });
}
$(document).on('submit', '#frmSaveRole', function (e) {
    e.preventDefault();
    var $this = $(this);
    if ($this.valid()) {
        $.ajax({
            url: $this.attr('action'),
            method: $this.attr('method'),
            data: $this.serialize(),
            success: function (res) {
                if (res.Status) {
                    toastr.success(res.Message);
                    $('#tblRole').DataTable().ajax.reload();
                    $('#Modal').modal('hide');
                }
                else {
                    toastr.error(res.Message);
                }
            }
        })
    }
})