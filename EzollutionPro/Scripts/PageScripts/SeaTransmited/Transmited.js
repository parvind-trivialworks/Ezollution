var startDateRange, endDateRange;
$(function () {
    $('#daterange-btn').daterangepicker(
        {
            ranges: {
                'Today': [moment(), moment()],
                'Yesterday': [moment().subtract(1, 'days'), moment().subtract(1, 'days')],
                'Last 7 Days': [moment().subtract(6, 'days'), moment()],
                'Last 30 Days': [moment().subtract(29, 'days'), moment()],
                'This Month': [moment().startOf('month'), moment().endOf('month')],
                'Last Month': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')]
            },
            startDate: moment().subtract(29, 'days'),
            endDate: moment(),
            minDate: '01/01/2015',
            format: 'DD/MM/YYYY',
            showDropdowns: true,
            showWeekNumbers: true,
            opens: 'left',
            locale: {
                applyLabel: 'Apply',
                fromLabel: 'From',
                toLabel: 'To',
                customRangeLabel: 'Custom Range',
                daysOfWeek: ['Su', 'Mo', 'Tu', 'We', 'Th', 'Fr', 'Sa'],
                monthNames: ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'],
                firstDay: 1
            }
        },
        function (start, end) {
            $('#daterange-btn span').html(start.format('D MMMM, YYYY') + ' - ' + end.format('D MMMM, YYYY'));
            startDateRange = start;
            endDateRange = end;
        }
    );
    //Set the initial state of the picker label
    GetSchedulingData();

});
$(document).on('click', '#btnApplyDateRange', function () {
    if (startDateRange == null || startDateRange == '') {
        alert('Please select the Date Range !');
    }
    else {
        schedulerTable.ajax.reload();
    }
});
$(document).on('click', '#btnRemoveDateRange', function () {
    startDateRange = '';
    endDateRange = '';
    schedulerTable.ajax.reload();
    $('#daterange-btn span').html('<i class="fa fa-calendar"></i>&nbsp;Select Date range');
});

var schedulerTable;
function GetSchedulingData() {
    schedulerTable = $('#tblScheduler').DataTable({
        "processing": true,
        "searching": true,
        "info": true,
        "paging":false,
        "ajax": {
            url: "/SeaTransmited/GetTransmitedData",
            method: "POST",
            data: function (d) {
                d.minDate = (startDateRange != null && startDateRange != "" && startDateRange != undefined) ? startDateRange.format('DD/MM/YYYY') : "";
                d.maxDate = (endDateRange != null && endDateRange != "" && endDateRange != undefined) ? endDateRange.format('DD/MM/YYYY') : "";
                return d;
            }
            //data: {minDate:}
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
            { "data": "iSchedulingId", "id": 'rowid_SchedulingId', "orderable": false, "visible": false },
            { "data": "sMBLNumber", "orderable": false },
            { "data": "sClientName", "orderable": true, },
            { "data": "sEDA", "orderable": true },
            { "data": null, "orderable": false, render: function (data, type, row, meta) { return "" + data.sCheckListSent + " / " + data.sCheckListApproved + ""; } },
            {
                "data": null, "orderable": false,
                render: function (data, type, row, meta) {
                    return "<button class='btn btn-success btn-xs' onclick='DownloadFile(" + data.iSchedulingId + ",\"" + data.sMBLNumber + "\")'><i class='fa fa - download'></i>&nbsp;Transmit & Download</button>";
}
            },
        ],
    "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
        debugger;
        if (aData.iSAction == 1) {
            $('td', nRow).css('background-color', '#ffffff');
            if (aData.sCheckListSent == "Y" && aData.sCheckListApproved == "Y") {
                $('td:nth-child(10)', nRow).attr('style', 'background-color:#58D68D');
            }
        }
        else if (aData.iSAction == 2) {
            $(nRow).children('td').each(function (i, val) {
                $(val).attr('style', 'background-color:#F7DC6F;');
            });
        }
        else if (aData.iSAction == 3) {
            $(nRow).children('td').each(function (i, val) {
                $(val).attr('style', 'background-color:black;color:white;');
            });
        }
        else if (aData.iSAction == 4) {
            $(nRow).children('td').each(function (i, val) {
                $(val).attr('style', 'background-color:#82E0AA');
            });
        }
        else {
            $(nRow).children('td').each(function (i, val) {
                $(val).attr('style', 'background-color:#ffffff');
            });
        }
    }
    });
}

function AddEditScheduling(iSchedulingId) {
    $('#ModalLgContainer').load('/SeaScheduling/AddUpdateSchedule?iScheduleId=' + iSchedulingId, function () {
        $.validator.unobtrusive.parse('#frmSaveScheduling');
        if ($('#sCheckListSent').val() == "No") {
            $("#iSAction option[value*='2']").attr('disabled', 'disabled');
            $("#sCheckListApproved").attr('disabled', 'disabled');
        }
        if ($('#sCheckListApproved').val() == "No") {
            $("#iSAction option[value*='2']").attr('disabled', 'disabled');
        }
        var value = $('#iSAction').val();
        if (value == 2) {
            $("#iSAction option[value*='3']").attr('disabled', 'disabled');
            $("#iSAction option[value*='4']").removeAttr('disabled');
            $("#sCheckListSent").attr('disabled', 'disabled');
            $("#sCheckListApproved").attr('disabled', 'disabled');
        }
        else {
            $("#iSAction option[value*='3']").attr('disabled');
            $("#iSAction option[value*='4']").prop('disabled', 'disabled');
        }
        $('#ModalLg').modal('show');
        $('#iShippingId').selectpicker();
        $('#iClientId').selectpicker();
        $('#iPODId').selectpicker();
        $('#iFPODId').selectpicker();
        $('#sCheckListSent').selectpicker();
        $('#sCheckListApproved').selectpicker();
        $('#iSAction').selectpicker();
        $('#sEDA').datepicker({
            autoclose: true,
            format: "dd/mm/yyyy",
        });
        $('#sRecieveOn').datepicker({
            autoclose: true,
            format: "dd/mm/yyyy",
        });
    });
}

$(document).on('change', '#iSAction', function () {
    if ($('#SAction').val() == 1) {
        $("#sCheckListApproved").removeAttr('disabled');
        $("#sCheckListSent").removeAttr('disabled');
    }
});
$(document).on('change', '#sCheckListApproved', function () {
    if ($("#sCheckListApproved").val() != "Yes") {
        $("#iSAction option[value*='2']").attr('disabled', 'disabled');
        $("#iSAction").selectpicker('refresh');
    }
    if ($("#sCheckListApproved").val() == "Yes" && $("#sCheckListSent").val() == "Yes") {
        $("#iSAction option[value*='2']").removeAttr('disabled');
        $("#iSAction").selectpicker('refresh');
    }
});

$(document).on('change', '#sCheckListSent', function () {
    if ($("#sCheckListSent").val() != "Yes") {
        $("#sCheckListApproved").val("No");
        $("#sCheckListApproved").attr('disabled', 'disabled');
        $("#sCheckListApproved").selectpicker('refresh');
        $("#iSAction option[value*='2']").attr('disabled', 'disabled');
        $("#iSAction").selectpicker('refresh');
    }
    else {
        $("#sCheckListApproved").removeAttr('disabled');
        $("#sCheckListApproved").selectpicker('refresh');

    } if ($("#sCheckListApproved").val() == "Yes" && $("#sCheckListSent").val() == "Yes") {
        $("#iSAction option[value*='2']").removeAttr('disabled');
        $("#iSAction").selectpicker('refresh');
    }
});
$(document).on('submit', '#frmSaveScheduling', function (e) {
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
                    schedulerTable.ajax.reload();
                    $('#ModalLg').modal('hide');
                }
                else {
                    toastr.error(res.Message);
                }
            }
        })
    }
});
function DownloadFile(iSchedulingId, sMBLNumber) {
    location.href = "/SeaTransmited/TransmitFile?iSchedulingId=" + iSchedulingId + "&sMBLNumber=" + sMBLNumber;
    $.ajax({
        url: "/SeaTransmited/TransmitFile?iSchedulingId=" + iSchedulingId + "&sMBLNumber=" + sMBLNumber,
        type: 'GET',
        success: function (res) {
            schedulerTable.ajax.reload();
        }
    });
}