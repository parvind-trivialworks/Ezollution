$(function () {
    $('#tblClient').DataTable({
        processing: true,
        serverSide: true,
        'searching': true,
        'orderable': false,
        'ajax': {
            url: '/Client/GetClients',
            method: 'POST',
            async: false
        },
        'columns': [
            {
                "data": "iSNo.", 'orderable': false
            },
            {
                "data": "sClientName", 'orderable': false
            },
            {
                "data": "sCARN", 'orderable': false
            },
            {
                "data": "sCompanyName", 'orderable': false
            },
            {
                "data": "sEmailID", 'orderable': false
            },
            {
                "data": "sGSTNNo", 'orderable': false
            },
            {
                "data": "iClientID", 'orderable': false, 'mRender': function (data, abc, full) {
                    return '<button type="button" onClick="AddClient(' + data + ')" class="btn btn-xs btn-warning"><i class="fa fa-edit"></i></button>'
                }
            }]
    })
});

function AddClient(iClientId) {
    $('#ModalLgContainer').load('/Client/AddUpdateClient?iClientId=' + iClientId, function () {
        $.validator.unobtrusive.parse('#frmSaveClient');
        $('#ModalLg').modal('show');
        $('#iStateId').selectpicker();
        $('#iCountryId').selectpicker();
    });
}
$(document).on('submit', '#frmSaveClient', function (e) {
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
                    $('#tblClient').DataTable().ajax.reload();
                    $('#ModalLg').modal('hide');
                }
                else {
                    toastr.error(res.Message);
                }
            }
        })
    }
});

$(document).on('change', '#iCountryId', function () {
    $.ajax({
        method: "GET",
        url: '/User/GetStates?iCountryId=' + $(this).val(),
        async: false,
        success: function (res) {
            var html = "<option>[ SELECT ]</option>";
            res.forEach(function (item, i) {
                html += "<option value=\"" + item.Value + "\">" + item.Text + "</option>";
            });
            $('#iStateId').html(html);
            $('#iStateId').selectpicker('refresh');
        }
    })
});