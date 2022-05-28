$(function () {
    $('#tblBondMaster').DataTable({
        processing: true,
        serverSide: true,
        destroy: true,
        'searching': false,
        'orderable': false,
        'ajax': {
            url: '/BondMaster/GetBonds',
            method: 'POST',
            async: false,
            data: function (d) {
                d.iShippingId = $('#iShippingId').val() == undefined ? "" : $('#iShippingId').val() ;
                d.iPOD = $('#iPOD').val() == undefined ? "" : $('#iPOD').val() ;
                d.iFPOD = $('#iFPOD').val() == undefined ? "" : $('#iFPOD').val() ;
                d.sCargoMovement = $('#sCargoMovement').val() == undefined ? "" : $('#sCargoMovement').val() ;
                d.sModeOfTransport = $('#sModeOfTransport').val() == undefined ? "" : $('#sModeOfTransport').val() ;
                return d;
            }
        },
        'columns': [
            {
                "data": "nBondNo", 'orderable': false
            },
            {
                "data": "sCarrierCode", 'orderable': false
            },
            {
                "data": "sModeOfTransport", 'orderable': false, 'mRender': function (data,abc,full) {
                    if (data == "T")
                        return "Train";
                    else
                        return "Road";
                }
            },
            {
                "data": "sMLOCode", 'orderable': false
            },
            {
                "data": "iBondId", 'orderable': false, 'mRender': function (data, abc, full) {
                    return '<button type="button" onClick="AddUpdateBondMaster(' + data + ')" class="btn btn-xs btn-warning"><i class="fa fa-edit"></i></button>'
                }
            }]
    });
    $('#iShippingId').selectpicker();
    $('#iPOD').selectpicker();
    $('#iFPOD').selectpicker();
    $('#sCargoMovement').selectpicker();
    $('#sModeOfTransport').selectpicker();

});

function AddUpdateBondMaster(iBondId) {
    $('#ModalLgContainer').load('/BondMaster/AddUpdateBond?iBondId=' + iBondId, function () {
        $.validator.unobtrusive.parse('#frmSaveBond');
        $('#ModalLg').modal('show');
        if ($('#sCargoMovement').val() == "LC") {
            $('#sCFSCode').rules('add', 'required');
            $('#sCFSName').rules('add', 'required');
        }
        else {
            $('#sCFSCode').rules('remove', 'required');
            $('#sCFSName').rules('remove', 'required');
        }
        $('#frmSaveBond select').selectpicker();
    });
}
$(document).on('click', '#btnSearch', function () {
    $('#tblBondMaster').DataTable().ajax.reload();
});
$(document).on('submit', '#frmSaveBond', function (e) {
    debugger;
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
                    $('#tblBondMaster').DataTable().ajax.reload();
                    $('#ModalLg').modal('hide');
                }
                else {
                    toastr.error(res.Message);
                }
            }
        })
    }
});

$(document).on('change', '#iShippingId', function () {
    if ($(this).val() !== "")
        $.get('/BondMaster/GetMLOCode?iShippingId=' + $(this).val(), function (res) {
            $('#sMLOCode').val(res);
        });
    else
        $('#sMLOCode').val("");
});

$(document).on('change', '#sCargoMovement', function () {
    if ($('#sCargoMovement').val() == "LC") {
        $('#sCFSCode').rules('add', 'required');
        $('#sCFSName').rules('add', 'required');
    }
    else {
        $('#sCFSCode').rules('remove', 'required');
        $('#sCFSName').rules('remove', 'required');
    }
});
