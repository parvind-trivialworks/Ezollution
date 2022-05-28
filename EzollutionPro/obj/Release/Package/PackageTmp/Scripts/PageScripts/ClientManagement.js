$(function () {

    $('#FromDate').datepicker({
        format: "dd/mm/yyyy",
        endDate: 'today',
        autoclose: true
    });

    $('#ToDate').datepicker({
        format: "dd/mm/yyyy",
        endDate: 'today',
        autoclose: true
    });


    $('#dtNextFollowUpDate').datepicker({
        format: "dd/mm/yyyy",
        autoclose: true
    });

    $('#dtDate').datepicker({
        format: "dd/mm/yyyy",
        autoclose: true
    });

    $('#dtBusinessStartDate').datepicker({
        format: "dd/mm/yyyy",
        autoclose: true
    });

});

var FollowupTable;
function GetFollowups(iClientManagementId) {
    FollowupTable = $('#FollowupTable').DataTable({
        "processing": true,
        "searching": false,
        "info": true,
        "paging": false,
        "ajax": {
            url: "/ClientManagement/GetClientManagementFollowUps",
            method: "POST",
            data: { iClientManagementID: iClientManagementId },
        },
        "columns": [
            { "data": "sFollowUpType", "orderable": false, },
            { "data": "sDifference", "orderable": false },
            { "data": "sRemark", "orderable": false },
            { "data": "sManagementRemark", "orderable": false },
            {
                "data": null, "orderable": false,
                render: function (data, type, row, meta) {
                    return "<label>"+ Format_Num(data.dRateOffered)+"</label>"
                }
            },
            {
                "data": null, "orderable": false,
                render: function (data, type, row, meta) {
                    return "<label>" + Format_Num(data.dFinalRate)  + "</label>"
                }
            },
            {
                "data": null, "orderable": false,
                render: function (data, type, row, meta) {
                    return "<label>" + Format_Num(data.dRevenueExpected)+ "</label>"
                }
            },
            {
                "data": null, "orderable": false,
                render: function (data, type, row, meta) {
                    return "<label>" + Format_Num(data.dActualRevenue)+ "</label>"
                }
            },
            { "data": "dtNextFollowUpDate", "orderable": false },
            { "data": "dtAddedOn", "orderable": false },
            {
                "data": null, "orderable": false,
                render: function (data, ype, row, meta) {
                    return "<button class='btn btn-warning btn-xs' onclick='AddEditFollowUp(" + iClientManagementId + "," + data.iClientManagementFollowupId + ")'><i class='fa fa-edit'></i></button>";
                }
            },

        ]
    });
}


function AddEditFollowUp(iClientManagementId, iClientManagementFollowupId) {
    $('#ModalLgContainer').load('/ClientManagement/AddEditClientManagementFollowUp?iClientManagementID=' + iClientManagementId + "&&iClientManagementFollowupId=" + iClientManagementFollowupId, function () {
        $.validator.unobtrusive.parse('#frmSaveClientManagementFollowUp'); 
        $('#ModalLg').modal('show');
    });
}

$(document).on('submit', '#frmSaveClientManagementFollowUp', function (e) {
    e.preventDefault();
    $('#loading').show();
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
                $('#loading').hide();
                if (res.Status) {
                    toastr.success(res.Message);
                    FollowupTable.ajax.reload();
                    $('#ModalLg').modal('hide');
                }
                else {
                    toastr.error(res.Message);
                }
            }
        })
    }
});




$(document).ready(function () {
    GeClientManagementListFun();
    $("#btnSearchClientManagement").click(function () {
        ClientManagementTable = null;
        GeClientManagementListFun();
    });
});


var ClientManagementTable;
function GeClientManagementListFun() {
  
    var startDateRange = $("#FromDate").val();
    var endDateRange = $("#ToDate").val();

    ClientManagementTable = $('#tblClientManagementList').DataTable({
        "processing": true,
        "searching": false,
        "info": true,
        "paging": true,
        "bDestroy": true,
         "responsive": true,
        "ajax": {
            url: "/ClientManagement/GetClientManagementList",
            method: "POST",
            data: function (d) {
                d.FromDate = startDateRange;
                d.ToDate = endDateRange;
                return d;
            }
        },
        "columns": [
            {
                "data": null,
                render: function (data, type, row, meta) {
                    return meta.row + meta.settings._iDisplayStart + 1;
                },
                "searchable": false,
                "orderable": false
            },
            {
                "data": null, "orderable": false,
                render: function (data, type, row, meta) {
                    return "<a href='/ClientManagement/AddEdit?iClientManagementId=" + data.iClientManagementId + "'>" + data.sClientCode + "</a>";
                }
            },
            { "data": "dtDate", "orderable": false },
            { "data": "sNewClientName", "orderable": false },
            { "data": "sExistingClientName", "orderable": false },
            { "data": "sContactPersonName", "orderable": false },
            { "data": "sContactPersonContactNo", "orderable": false },
            { "data": "sDecisionMakerName", "orderable": false },
            { "data": "sDecisionMakerContactNo", "orderable": false },
            { "data": "sReference", "orderable": false },
            { "data": "sBranch", "orderable": false },
            { "data": "sAddress", "orderable": false },
            { "data": "dtNextFollowUpDate", "orderable": false },
            { "data": "sEmailId", "orderable": false },
            { "data": "sServiceType", "orderable": false },
            {
                "data": null, "orderable": false,
                render: function (data, type, row, meta) {
                    return "<label>" + Format_Num(data.dRateOffered) + "</label>"
                }
            },
            {
                "data": null, "orderable": false,
                render: function (data, type, row, meta) {
                    return "<label>" + Format_Num(data.dFinalRate) + "</label>"
                }
            },
            {
                "data": null, "orderable": false,
                render: function (data, type, row, meta) {
                    return "<label>" + Format_Num(data.dRevenueExpected) + "</label>"
                }
            },
            {
                "data": null, "orderable": false,
                render: function (data, type, row, meta) {
                    return "<label>" + Format_Num(data.dActualRevenue) + "</label>"
                }
            },
            { "data": "dtBusinessStartDate", "orderable": false },
            { "data": "dtAddedOn", "orderable": false },
        ]
    });
}