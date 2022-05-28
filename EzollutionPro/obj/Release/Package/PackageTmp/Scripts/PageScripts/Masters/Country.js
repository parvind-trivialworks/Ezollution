$(function () {
    $('#tblCountry').DataTable({
        processing: true,
        serverSide: true,
        'searching': false,
        'orderable': false,
        'ajax': {
            url: '/Country/GetCountries',
            method: 'POST',
            async: false
        },
        'columns': [
            {
                "data": "sCountryCode", 'orderable': false
            },        
            {
                "data": "sCountryName", 'orderable': false
            },        
            {
                "data": "sCountryPhoneCode", 'orderable': false
            },   
            {
                "data": "sCurrencyCode", 'orderable': false
            },       
            {
                "data": "sCountryDescription", 'orderable': false
            }, 
            {
                "data": "sCurrencyDescription", 'orderable': false
            },
            {
                "data": "iCountryId", 'orderable': false, 'mRender': function (data, abc, full) {
                    return '<button type="button" onClick="AddCountry(' + data + ')" class="btn btn-xs btn-warning"><i class="fa fa-edit"></i></button>'
                }
            }]
    })
});

function AddCountry(iCountryId) {
    $('#ModalLgContainer').load('/Country/AddUpdateCountry?iCountryId=' + iCountryId, function () {
        $.validator.unobtrusive.parse('#frmSaveCountry');
        $('#ModalLg').modal('show');
    });
}
$(document).on('submit', '#frmSaveCountry', function (e) {
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
                    $('#tblCountry').DataTable().ajax.reload();
                    $('#ModalLg').modal('hide');
                }
                else {
                    toastr.error(res.Message);
                }
            }
        })
    }
});
$(document).on('change', '#CountryImage', function (e) {
    var size = this.files[0].size / 1024 / 1024;
    if (size > 4) {
        toastr.error('Max file size limit is 4 MB.');
        $(this).val('');
    }
});