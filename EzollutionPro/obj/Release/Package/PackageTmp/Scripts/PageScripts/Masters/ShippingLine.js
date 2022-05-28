$(function () {
    $('#tblShippingLine').DataTable({
        processing: true,
        serverSide: true,
        'searching': true,
        'orderable': false,
        'ajax': {
            url: '/ShippingLine/GetShippingLines',
            method: 'POST',
            async: false
        },
        'columns': [
            {
                "data": "iSNo.", 'orderable': false
            },
            {
                "data": "sShippingLineName", 'orderable': false
            },
            {
                "data": "sMLOCode", 'orderable': false
            },
            {
                "data": "iShippingID", 'orderable': false, 'mRender': function (data, abc, full) {
                    return '<button type="button" onClick="AddShippingLine(' + data + ')" class="btn btn-xs btn-warning"><i class="fa fa-edit"></i></button>'
                }
            }]
    })
});

function AddShippingLine(iShippingLineId) {
    $('#ModalLgContainer').load('/ShippingLine/AddUpdateShippingLine?iShippingLineId=' + iShippingLineId, function () {
        $.validator.unobtrusive.parse('#frmSaveShippingLine');
        $('#ModalLg').modal('show');
        $('#iStateId').selectpicker();
        $('#iCountryId').selectpicker();
    });
}
$(document).on('submit', '#frmSaveShippingLine', function (e) {
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
                    $('#tblShippingLine').DataTable().ajax.reload();
                    $('#ModalLg').modal('hide');
                }
                else {
                    toastr.error(res.Message);
                }
            }
        })
    }
});

