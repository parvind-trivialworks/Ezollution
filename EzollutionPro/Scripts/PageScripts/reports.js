$(function () {
    
    $('#sFromDate').datepicker({
        format: "dd/mm/yyyy",
        endDate: 'today',
        autoclose: true
    });
    $('#sToDate').datepicker({
        format: "dd/mm/yyyy",
        endDate: 'today',
        autoclose: true
    });

    $('#iClientId').selectpicker();
});
$("#ReportName").click(function () {
    var filterby = $("#filterby");
    var ClientName = $("#clientName");
    var clienttype = $("#clienttype");
    var ReportName = $("#ReportName").val();
    if (ReportName == "INV") {
        filterby.hide();
        ClientName.hide();
        clienttype.show();
    }
    else if (ReportName == "DWI") {
        filterby.hide();
        clienttype.hide();
        ClientName.hide();
    }
    else if (ReportName == "PWI") {
        ClientName.show();
        clienttype.show();
        filterby.hide();
    }
    else {
        ClientName.show();
        clienttype.show();
        filterby.show();
    }
});

$(document).ready(function () {
   
    $("iframe").width(800);
    $("#ClientType").change(function () {
        var ClientType = $("#ClientType").val();
        if (ClientType == "") {
            $("#iClientId").html("");
            return;
        }
       
        $.ajax({
            type: "POST",
            url: "/ClientReport/GetClients",
            contentType: "application/json; charset=utf-8",
            data: '{"ClientType":"' + $("#ClientType").val().trim() + '"}',
            dataType: "json",
            async: false,
            success: function (res) {
                var iClientId = $("#iClientId");
                var filterBy = $("#filterBy");
                var reportName = $("#ReportName");
                filterBy.html("");
                reportName.html("");
                iClientId.html("");
                iClientId.append($('<option>').text("[ SELECT ]").attr('value', ""));
                for (let li of res) {
                    iClientId.append($('<option>').text(li.Text).attr('value', li.Value));
                }
                iClientId.selectpicker('refresh'); 
                if (ClientType == "sea") {
                   // console.log("hi");
                    console.log(reportName);
                    filterBy.append($('<option>').text("IGM").attr('value', 'IGM'));
                    reportName.append($('<option>').text("MBL WISE REPORT").attr('value', 'MWR'));
                    reportName.append($('<option>').text("INVOICE").attr('value', 'INV'));
                    reportName.append($('<option>').text("Party wise Invoice").attr('value', 'PWI'));
                    reportName.append($('<option>').text("Date wise Invoice").attr('value', 'DWI'));
                }
                else {
                    console.log(reportName);
                    filterBy.append($('<option>').text("Create Date").attr('value', 'CD'));
                    reportName.append($('<option>').text("MAWB Report").attr('value', 'MAWB'));
                    reportName.append($('<option>').text("INVOICE").attr('value', 'INV'));
                    reportName.append($('<option>').text("Party wise Invoice").attr('value', 'PWI'));
                    reportName.append($('<option>').text("Date wise Invoice").attr('value', 'DWI'));
                }
            }
        })
    });
});
