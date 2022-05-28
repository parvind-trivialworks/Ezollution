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
   
    $('#dtPaymentDate').datepicker({
        format: "dd/mm/yyyy",
        endDate: 'today',
        autoclose: true
    });

    $('#dtReceivedDate').datepicker({
        format: "dd/mm/yyyy",
        endDate: 'today',
        autoclose: true
    });

    $('#FromInvoiceDate').datepicker({
        format: "dd/mm/yyyy",
        endDate: 'today',
        autoclose: true
    });

    $('#ToInvoiceDate').datepicker({
        format: "dd/mm/yyyy",
        endDate: 'today',
        autoclose: true
    });


    $('#iClientId').selectpicker();
});

var InvoiceItemsTable;
function GetInvoiceItems(iInvoiceID) {
    InvoiceItemsTable = $('#tblInvoiceItems').DataTable({
        "processing": true,
        "searching": false,
        "info": true,
        "paging": false,
        "ajax": {
            url: "/Invoice/GetInvoiceItems",
            method: "POST",
            data: { iInvoiceID: iInvoiceID },
        },
        "columns": [
            { "data": "sHSN_SAC", "orderable": false, },
            { "data": "sHSN_Desc", "orderable": false },
            { "data": "sItemDescription", "orderable": false },
            { "data": "iQuantity", "orderable": false },
            {
                "data": null, "orderable": false,
                render: function (data, type, row, meta) {
                    return "<label>" + Format_Num_Decimal(data.dAmountPerUnit) + "</label>"
                }
            },
            {
                "data": null, "orderable": false,
                render: function (data, type, row, meta) {
                    return "<label>" + Format_Num_Decimal(data.dTotalAmount)+"</label>"
                }
            },
            {
                "data": null, "orderable": false,
                render: function (data, type, row, meta) {
                    return "<label>" + data.dCgstInPercent.toFixed(0) + "%</label>"
                }
            },
            {
                "data": null, "orderable": false,
                render: function (data, type, row, meta) {
                    return "<label>" + data.dSgstInPercent.toFixed(0) + "%</label>"
                }
            },
            {
                "data": null, "orderable": false,
                render: function (data, type, row, meta) {
                    return "<label>" + data.dIgstInPercent.toFixed(0) + "%</label>"
                }
            },
            {
                "data": null, "orderable": false,
                render: function (data, type, row, meta) {
                    return "<button class='btn btn-warning btn-xs' onclick='AddEditInvoiceItem(" + iInvoiceID+"," + data.iInvoiceItemID + ")'><i class='fa fa-edit'></i></button>";
                }
            },

        ],
        "footerCallback": function (row, data, start, end, display) {
            var TotalAmount = 0;
            if (data.length > 0) {
                TotalAmount = data.reduce(function (sum, tax) {
                    return sum + tax.dTotalAmount;
                }, 0);
            }
            $("#T_TotalAmount").html(TotalAmount.toFixed(2));
        }

    });

    
}



function AddEditInvoiceItem(iInvoiceID,iInvoiceItemID) {
    $('#ModalLgContainer').load('/Invoice/AddEditInvoiceItem?iInvoiceID=' + iInvoiceID + "&&iInvoiceItemID=" + iInvoiceItemID, function () {
        $.validator.unobtrusive.parse('#frmSaveInvoiceItem'); 
        $('#ModalLg').modal('show');
    });
}

$(document).on('submit', '#frmSaveInvoiceItem', function (e) {
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
                    InvoiceItemsTable.ajax.reload();
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
    GetInvoiceList();
    $("#iClientType").change(function () {
        var ClientType = $("#iClientType").val();
        if (ClientType == "") {
            $("#iClientId").html("")
            return;
        }
       
        $.ajax({
            type: "POST",
            url: "/Invoice/GetClients",
            contentType: "application/json; charset=utf-8",
            data: '{"ClientType":"' + $("#iClientType").val().trim() + '"}',
            dataType: "json",
            async: false,
            success: function (res) {
                var iClientId = $("#iClientId");
                iClientId.html("");
                iClientId.append($('<option>').text("[ SELECT ]").attr('value', ""));
                for (let li of res) {
                    iClientId.append($('<option>').text(li.Text).attr('value', li.Value));
                }
                iClientId.selectpicker('refresh'); 
            }
        })
    });
    var click = 0;
    $("#btnSearchInvoice").click(function () {
        invoiceTable = null;
        GetInvoiceList();
    });

    $("#btnSearchPayments").click(function () {
        InvoicePaymentsList = null;
        GetInvoicePaymentsList();
    });
});


var invoiceTable;
function GetInvoiceList() {
    var startDateRange = $("#FromDate").val();
    var endDateRange = $("#ToDate").val();
    var iClientType = $("#iClientType").val();
    var iClientID = $("#iClientId").val();
    var PaymentStatus = $("#PaymentStatus").val();
  
    if (iClientType == "" || iClientType == undefined) {
        return;
    }
    if (iClientID == null || iClientID == undefined || iClientID == "")
        iClientID = 0;

    invoiceTable = $('#tblInvoiceList').DataTable({
        "processing": true,
        "searching": false,
        "info": true,
        "paging": true,
        "bDestroy": true,
        "ajax": {
            url: "/Invoice/GetInvoiceList",
            method: "POST",
            data: function (d) {
                d.FromDate = startDateRange;
                d.ToDate = endDateRange;
                d.iClientType = iClientType;
                d.iClientID = iClientID;
                d.PaymentStatus = PaymentStatus;
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
                    return "<a href='/Invoice/AddEdit?InvoiceId=" + data.iInvoiceID + "'>" + data.sInvoiceNo + "</a>";
                }
            },
            { "data": "StrCreateDate", "orderable": false },
            { "data": "StrClientType", "orderable": false },
            { "data": "sClientCode", "orderable": false },
            { "data": "StrClientName", "orderable": false },
            //{ "data": "StrCompanyName", "orderable": false },
            //{ "data": "sPOS", "orderable": false },
            { "data": "FromInvoiceDate", "orderable": false },
            { "data": "ToInvoiceDate", "orderable": false },
            {
                "data": null, "orderable": false,
                render: function (data, type, row, meta) {
                    return "<label>" + Format_Num_Decimal(data.dTotalAmount) + "</label>"
                }
            },
            {
                "data": null, "orderable": false,
                render: function (data, type, row, meta) {
                    return "<label>" + Format_Num_Decimal(data.dPaidAmount) + "</label>"
                }
            },
            {
                "data": null, "orderable": false,
                render: function (data, type, row, meta) {
                    return "<label>" + Format_Num_Decimal(data.dTotalTds) + "</label>"
                }
            },
            {
                "data": null, "orderable": false,
                render: function (data, type, row, meta) {
                    return "<label>" + Format_Num_Decimal(data.dBalance) + "</label>"
                }
            },  
            {
                "data": null, "orderable": false,
                render: function (data, type, row, meta) {
                    return "<a href='/Invoice/Payments?InvoiceId=" + data.iInvoiceID + "'>" + data.StrPaymentStatus+"</a>";
                }
            }
        ]
    });
}

var InvoicePayments;
function GetInvoicePayments(iInvoiceID) {
    InvoicePayments = $('#tblInvoicePayments').DataTable({
        "processing": true,
        "searching": false,
        "info": true,
        "paging": false,
        "ajax": {
            url: "/Invoice/GetInvoicePayments",
            method: "POST",
            data: { iInvoiceID: iInvoiceID },
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
            { "data": "sCreateDate", "orderable": false, },
            {
                "data": null, "orderable": false,
                render: function (data, type, row, meta) {
                    return "<label>" + Format_Num_Decimal(data.dAmount) + "</label>"
                }
            },
            {
                "data": null, "orderable": false,
                render: function (data, type, row, meta) {
                    return "<label>" + Format_Num_Decimal(data.dTds) + "</label>"
                }
            },
            { "data": "dtReceivedDate", "orderable": false, },
            { "data": "sPaymentMode", "orderable": false, },
            { "data": "StrPaymentStatus", "orderable": false, },
            { "data": "sCheckNeftNo", "orderable": false, },
            { "data": "dtCheckDate", "orderable": false, },
            {
                "data": null, "orderable": false,
                render: function (data, type, row, meta) {
                    return "<button class='btn btn-warning btn-xs' onclick='AddEditInvoicePayment(" + iInvoiceID + "," + data.iInvoicePaymentID + ")'><i class='fa fa-edit'></i></button>";
                }
            },

        ],
        "footerCallback": function (row, data, start, end, display) {
            var TotalAmount = 0;
            if (data.length > 0) {
                TotalAmount = data.reduce(function (sum, tax) {
                    return sum + tax.dAmount;
                }, 0);
            }

            var TotalTds = 0;
            if (data.length > 0) {
                TotalTds = data.reduce(function (sum, tax) {
                    return sum + tax.dTds;
                }, 0);
            }
            $("#Total_Amount").html(TotalAmount.toFixed(2));
            $("#Total_Tds").html(TotalTds.toFixed(2));
        }

    });


}

function AddEditInvoicePayment(iInvoiceID, iInvoicePaymentID) {
    $('#ModalLgContainer').load('/Invoice/AddEditInvoicePayment?iInvoiceID=' + iInvoiceID + "&&iInvoicePaymentID=" + iInvoicePaymentID, function () {
        $.validator.unobtrusive.parse('#frmSaveInvoicePayment');
        $('#ModalLg').modal('show');
    });
}

$(document).on('submit', '#frmSaveInvoicePayment', function (e) {
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
                console.log(res);
                if (res.Status) {
                    toastr.success(res.Message);
                    InvoicePayments.ajax.reload();
                    $('#ModalLg').modal('hide');
                }
                else {
                    toastr.error(res.Message);
                }
            }
        })
    }
});


var InvoicePaymentsList;
function GetInvoicePaymentsList() {
    var startDateRange = $("#FromDate").val();
    var endDateRange = $("#ToDate").val();
    var iClientType = $("#iClientType").val();
    var iClientID = $("#iClientId").val();
    var sInvoiceNo = $("#sInvoiceNo").val();
    if (iClientType == "" || iClientType == undefined) {
        return;
    }
    if (iClientID == null || iClientID == undefined || iClientID == "")
        iClientID = 0;

    InvoicePaymentsList = $('#tblInvoicePaymentsList').DataTable({
        "processing": true,
        "searching": false,
        "info": true,
        "paging": true,
        "bDestroy": true,
        "ajax": {
            url: "/Invoice/GetPaymentsList",
            method: "POST",
            data: function (d) {
                d.FromDate = startDateRange;
                d.ToDate = endDateRange;
                d.iClientType = iClientType;
                d.iClientID = iClientID;
                d.sInvoiceNo = sInvoiceNo;
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
            { "data": "sCreateDate", "orderable": false },
            { "data": "sInvoiceNo", "orderable": false },
            { "data": "StrClientType", "orderable": false },
            { "data": "sClientCode", "orderable": false },
            { "data": "StrClientName", "orderable": false },
            {
                "data": null, "orderable": false,
                render: function (data, type, row, meta) {
                    return "<label>" + Format_Num_Decimal(data.dAmount) + "</label>"
                }
            },
            {
                "data": null, "orderable": false,
                render: function (data, type, row, meta) {
                    return "<label>" + Format_Num_Decimal(data.dTds) + "</label>"
                }
            },
            { "data": "dtReceivedDate", "orderable": false, },
            { "data": "sPaymentMode", "orderable": false, },
            { "data": "StrPaymentStatus", "orderable": false, },
            { "data": "sCheckNeftNo", "orderable": false, },
            { "data": "dtCheckDate", "orderable": false, },

        ]
    });
}