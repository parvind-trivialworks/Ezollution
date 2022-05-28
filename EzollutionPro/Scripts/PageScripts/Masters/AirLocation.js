$(function () {
    $('#tblAirLocation').DataTable({
        processing: true,
        serverSide: true,
        'searching': true,
        'orderable': false,
        'ajax': {
            url: '/AirLocation/GetLocations',
            method: 'POST',
            async: false
        },
        'columns': [
            {
                "data": "sCustomLocation", 'orderable': false
            },
            {
                "data": "sCustomCode", 'orderable': false
            },
            {
                "data": "sThreeLetterCode", 'orderable': false
            },
            {
                "data": "iLocationId", 'orderable': false, 'mRender': function (data, abc, full) {
                    return '<button type="button" onClick="AddAirLocation(' + data + ')" class="btn btn-xs btn-warning"><i class="fa fa-edit"></i></button>'
                }
            }]
    })
});

function AddAirLocation(iAirLocationId) {
    location.href = '/AirLocation/AddUpdateLocation?iLocationId=' + iAirLocationId;
}

