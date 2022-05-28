$(function () {
    $('#tblCity').DataTable({
        processing: true,
        serverSide: true,
        'searching': false,
        'orderable': false,
        'ajax': {
            url: '/City/GetCities',
            method: 'POST',
            async: false
        },
        'columns': [
            {
                "data": "sCityName", 'orderable': false
            },        
            {
                "data": "sStateName", 'orderable': false
            },        
            {
                "data": "sCountryName", 'orderable': false
            },   
            {
                "data": "sCityDescription", 'orderable': false
            },
            {
                "data": "iCityId", 'orderable': false, 'mRender': function (data, abc, full) {
                    return '<button type="button" onClick="AddCity(' + data + ')" class="btn btn-xs btn-warning"><i class="fa fa-edit"></i></button>'
                }
            }]
    })
});

function AddCity(iCityId) {
    $('#ModalLgContainer').load('/City/AddUpdateCity?iCityId=' + iCityId, function () {
        $.validator.unobtrusive.parse('#frmSaveCity');
        $('#ModalLg').modal('show');
        $('#iStateId').selectpicker();
        $('#iCountryId').selectpicker();
    });
}
$(document).on('submit', '#frmSaveCity', function (e) {
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
                    $('#tblCity').DataTable().ajax.reload();
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