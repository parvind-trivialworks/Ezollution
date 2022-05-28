$(function () {
    $('#tblPOFD').DataTable({
        processing: true,
        serverSide: true,
        'searching': true,
        'orderable': false,
        'ajax': {
            url: '/POFD/GetPOFDs',
            method: 'POST',
            async: false
        },
        'columns': [
            {
                "data": "iSNo.", 'orderable': false
            },
            {
                "data": "sPortName", 'orderable': false
            },
            {
                "data": "sPortCode", 'orderable': false
            },
            {
                "data": "iPortID", 'orderable': false, 'mRender': function (data, abc, full) {
                    return '<button type="button" onClick="AddPOFD(' + data + ')" class="btn btn-xs btn-warning"><i class="fa fa-edit"></i></button>'
                }
            }]
    })
});

function AddPOFD(iPOFDId) {
    $('#ModalLgContainer').load('/POFD/AddUpdatePOFD?iPortId=' + iPOFDId, function () {
        $.validator.unobtrusive.parse('#frmSavePOFD');
        $('#ModalLg').modal('show');
    });
}
$(document).on('submit', '#frmSavePOFD', function (e) {
    e.preventDefault();
    var $this = $(this);
    var data = new FormData(this);
    if ($this.valid()) {
        $.ajax({
            url: $this.attr('action'),
            method: $this.attr('method'),
            contentType: false, // Not to set any content header  
            processData: false,
            data: data,
            success: function (res) {
                if (res.Status) {
                    toastr.success(res.Message);
                    $('#tblPOFD').DataTable().ajax.reload();
                    $('#ModalLg').modal('hide');
                }
                else {
                    toastr.error(res.Message);
                }
            }
        })
    }
});
