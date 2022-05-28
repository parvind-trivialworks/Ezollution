$(function () {
    $('#tblUser').DataTable({
        processing: true,
        serverSide: true,
        'searching': false,
        'orderable': false,
        'ajax': {
            url: '/User/GetUsers',
            method: 'POST'
        },
        'columns': [
            { "data": "sFirstName", 'orderable': false },
            { "data": "sLastName", 'orderable': false },
            { "data": "sEmailID", 'orderable': false },
            { "data": "sRoleName", 'orderable': false },
            { "data": "sPhoneNo", 'orderable': false },
            { "data": "sCityName", 'orderable': false },
            { "data": "sStateName", 'orderable': false },
            { "data": "sCountryName", 'orderable': false },
            { "data": "sZipCode", 'orderable': false },
            {
                "data": "iUserId", 'orderable': false, 'mRender': function (data, abc, full) {
                    return '<button type="button" onClick="AddUser(' + data + ')" class="btn btn-xs btn-warning"><i class="fa fa-edit"></i></button>'
                }
            }]
    })
});
function AddUser(iUserId) {
    $('#ModalLgContainer').load('/User/AddUpdateUser?iUserId=' + iUserId, function () {
        $.validator.unobtrusive.parse('#frmSaveUser');
        $('#iStateId').selectpicker();
        $('#iCityId').selectpicker();
        $('#iCountryId').selectpicker();
        $('#iRoleId').selectpicker();
        $('#ModalLg').modal('show');
    });
}
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
$(document).on('change', '#iStateId', function () {
    $.ajax({
        method: "GET",
        url: '/User/GetCities?iStateId=' + $(this).val(),
        async: false,
        success: function (res) {
            var html = "<option>[ SELECT ]</option>";
            res.forEach(function (item,i) {
                html += "<option value=\"" + item.Value + "\">" + item.Text + "</option>";
            });
            $('#iCityId').html(html);
            $('#iCityId').selectpicker('refresh');
        }
    })
});

$(document).on('submit', '#frmSaveUser', function (e) {
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
                    $('#tblUser').DataTable().ajax.reload();
                    $('#ModalLg').modal('hide');
                }
                else {
                    toastr.error(res.Message);
                }
            }
        })
    }
});

$(document).on('change', '#Picture', function (e) {
    var size = this.files[0].size / 1024 / 1024;
    if (size > 4) {
        toastr.error('Max file size limit is 4 MB.');
        $(this).val('');
    }
});

