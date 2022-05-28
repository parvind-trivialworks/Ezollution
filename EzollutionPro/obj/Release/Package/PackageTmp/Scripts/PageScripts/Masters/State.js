$(function () {
    $('#tblState').DataTable({
        processing: true,
        serverSide: true,
        'searching': false,
        'orderable': false,
        'ajax': {
            url: '/State/GetStates',
            method: 'POST',
            async: false
        },
        'columns': [
            {
                "data": "sStateCode", 'orderable': false
            },        
            {
                "data": "sStateName", 'orderable': false
            },        
            {
                "data": "sCountryName", 'orderable': false
            },   
            {
                "data": "sStateDescription", 'orderable': false
            },
            {
                "data": "iStateId", 'orderable': false, 'mRender': function (data, abc, full) {
                    return '<button type="button" onClick="AddState(' + data + ')" class="btn btn-xs btn-warning"><i class="fa fa-edit"></i></button>'
                }
            }]
    })
});

function AddState(iStateId) {
    $('#ModalLgContainer').load('/State/AddUpdateState?iStateId=' + iStateId, function () {
        $.validator.unobtrusive.parse('#frmSaveState');
        $('#ModalLg').modal('show');
        $('#iCountryId').selectpicker();
    });
}
$(document).on('submit', '#frmSaveState', function (e) {
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
                    $('#tblState').DataTable().ajax.reload();
                    $('#ModalLg').modal('hide');
                }
                else {
                    toastr.error(res.Message);
                }
            }
        })
    }
});
