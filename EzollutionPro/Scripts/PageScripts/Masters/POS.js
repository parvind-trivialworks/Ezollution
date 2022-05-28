$(function () {
    $('#tblPOS').DataTable({
        processing: true,
        serverSide: true,
        'searching': true,
        'orderable': false,
        'ajax': {
            url: '/POS/GetPOSs',
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
                    return '<button type="button" onClick="AddPOS(' + data + ')" class="btn btn-xs btn-warning"><i class="fa fa-edit"></i></button>'
                }
            }]
    })
});

function AddPOS(iPOSId) {
    $('#ModalLgContainer').load('/POS/AddUpdatePOS?iPortId=' + iPOSId, function () {
        $.validator.unobtrusive.parse('#frmSavePOS');
        $('#ModalLg').modal('show');
    });
}
$(document).on('submit', '#frmSavePOS', function (e) {
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
                    $('#tblPOS').DataTable().ajax.reload();
                    $('#ModalLg').modal('hide');
                }
                else {
                    toastr.error(res.Message);
                }
            }
        })
    }
});
