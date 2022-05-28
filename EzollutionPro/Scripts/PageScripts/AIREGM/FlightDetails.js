$(function () {
    $('#tblAirFlight').DataTable({
        processing: true,
        serverSide: true,
        'searching': true,
        'orderable': false,
        'ajax': {
            url: '/AirEGMFlight/GetFlights',
            method: 'POST',
            async: false
        },
        'columns': [
            {
                "data": "sNo", 'orderable': false
            },
            {
                "data": "sClientName", 'orderable': false
            },
            {
                "data": "sFlightNo", 'orderable': false, 'mRender': function (data, abc, full) {
                    return '<a href=\'/AirEGMFlight/ViewFlightDetails?iFlightId=' + full.iFlightId + '\'>' + data + '</a>';
                }
            },
            {
                "data": "sEGMNo", 'orderable': false
            },
            {
                "data": "iFlightId", 'orderable': false, 'mRender': function (data, abc, full) {
                    return '<button type="button" onClick="AddUpdateAirEGMFlight(' + data + ')" class="btn btn-xs btn-warning"><i class="fa fa-edit"></i></button>'
                        + ' <button type="button" onClick="ProceedToTransmit(' + data + ')" class="btn btn-xs btn-success"><i class="fa fa-rss"></i></button>';
                }
            }]
    })
});


function AddUpdateAirEGMFlight(iFlightId) {
    location.href = '/AirEGMFlight/AddUpdateFlightDetails?iFlightId=' + iFlightId
}

function ProceedToTransmit(data) {
    $.ajax({
        url: "/AirEGMFlight/ProceedToTransmit",
        data: { iFlightId: data },
        type:"POST",
        success: function (res) {
            if (res.Status) {
                $('#tblAirFlight').DataTable().ajax.reload();
            }
            else {
                toastr.error(res.Message);
            }
        }
    })
}
