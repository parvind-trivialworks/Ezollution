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


function GetSchedulingData() {
    schedulerTable = $('#tblScheduler').DataTable({
        "processing": true,
        "searching": true,
        "info": true,
        "serverSide": true,
        "paging": true,
        "pageLength": 50,
        "ajax": {
            url: "/MAWB/GetTransmitteds",
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
                "data": "SNo", orderable: false
            },
            {
                "data": "sClientName", orderable: false
            },
            {
                "data": "sMAWBNo", "orderable": false
            },
            {
                "data": "sCustomLocation", orderable: false
            },
            {
                "data": "sActionDate", orderable: false
            },
            {
                "data": "username", orderable: false
            },
            {
                "data": "iMAWBId", "orderable": false,
                render: function (data, type, row, meta) {
                    var html = "";
                    if (!row.bIsAirClient) {
                        html += " <button class='btn btn-warning btn-xs' onclick='location.href=\"/MAWB/AddUpdateMAWB?iMAWBId=" + data + "\"'><i class='fa fa-edit'></i> Edit</button>";
                        if (row.bIsTransmitted)
                            html += " <button class='btn btn-success btn-xs' onclick='Transmit(" + data + ")'><i class='fa fa-rss'></i> Re-Transmit</button>";
                        else {
                            html += " <button class='btn btn-success btn-xs' onclick='Transmit(" + data + ")'><i class='fa fa-rss'></i> Transmit</button>";
                        }
                    }
                    return html;
                }
            },
        ],
        //"fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
        //    debugger;
        //    if (aData.iSAction == 1) {
        //        $('td', nRow).css('background-color', '#ffffff');
        //        if (aData.sCheckListSent == "Y" && aData.sCheckListApproved == "Y") {
        //            $('td:nth-child(10)', nRow).attr('style', 'background-color:#58D68D');
        //        }
        //    }
        //    else if (aData.iSAction == 2) {
        //        $(nRow).children('td').each(function (i, val) {
        //            $(val).attr('style', 'background-color:#F7DC6F;');
        //        });
        //    }
        //    else if (aData.iSAction == 3) {
        //        $(nRow).children('td').each(function (i, val) {
        //            $(val).attr('style', 'background-color:black;color:white;');
        //        });
        //    }
        //    else if (aData.iSAction == 4) {
        //        $(nRow).children('td').each(function (i, val) {
        //            $(val).attr('style', 'background-color:#82E0AA');
        //        });
        //    }
        //    else {
        //        $(nRow).children('td').each(function (i, val) {
        //            $(val).attr('style', 'background-color:#ffffff');
        //        });
        //    }
        //}
    });
}
function Transmit(data) {
    location.href = "/MAWB/Transmit?iMAWBId=" + data;
    setTimeout(function () {
        schedulerTable.ajax.reload();
    }, 1500);
}