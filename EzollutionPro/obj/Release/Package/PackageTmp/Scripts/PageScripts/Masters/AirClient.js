$(function () {
    $('#tblAirClient').DataTable({
        processing: true,
        serverSide: true,
        'searching': true,
        'orderable': false,
        'ajax': {
            url: '/AirClient/GetClients',
            method: 'POST',
            async: false
        },
        'columns': [
            {
                "data": "sClientName", 'orderable': false
            },
            {
                "data": "sCARNNo", 'orderable': false
            },
            {
                "data": "sCompanyName", 'orderable': false
            },
            {
                "data": "sEmailId", 'orderable': false
            },
            {
                "data": "sGSTNo", 'orderable': false
            },
            {
                "data": "iAirClientId", 'orderable': false, 'mRender': function (data, abc, full) {
                    return '<button type="button" onClick="AddAirClient(' + data + ')" class="btn btn-xs btn-warning"><i class="fa fa-edit"></i></button>'
                }
            }]
    })
});

function AddAirClient(iAirClientId) {
    location.href = '/AirClient/AddUpdateClient?iClientId=' + iAirClientId;
}

