var EmailTableTbl;
function GetEmails() {

    var iClientType = $("#iClientType").val();
    var iClientId = $("#iClientId").val();
    
    if (iClientId == null || iClientId == undefined || iClientId == "")
        iClientId = 0;

    EmailTableTbl = $('#EmailTable').DataTable({
        "processing": true,
        "searching": false,
        "info": true,
        "paging": true,
        "bDestroy": true,
        'ajax': {
            url: '/ClientMail/GetMail',
            method: 'POST',
            data: { iClientType: iClientType, iClientId: iClientId }
        },
        'columns': [
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
                    if (data.iClientType == 0) {
                        return '<span>SEA</span>'
                    }
                    else {
                        return '<span>AIR</span>'
                    }
                }
            },
            {
                "data": "sClientName", 'orderable': false
            },
            {
                "data": "sEmailPersonName", 'orderable': false
            },
            {
                "data": "sEmailId", 'orderable': false
            },
            {
                "data": null, "orderable": false,
                render: function (data, type, row, meta) {
                    if (data.bIsDefault == true) {
                        return "<span><input type='checkbox' checked='checked' disabled /></span>";
                    }
                    else {
                        return "<span><input type='checkbox'  disabled /></span>";
                    }
                }
            },
            {
                "data": null, "orderable": false,
                render: function (data, type, row, meta) {
                    return "<button class='btn btn-warning btn-xs' onclick='AddEditEmail(" + data.iClientId + "," + data.iMailId + ")'><i class='fa fa-edit'></i></button>";
                }
            }
        ]
    });
}

$(document).ready(function () {
    $("#iClientType").change(function () {
        var ClientType = $("#iClientType").val();
        if (ClientType == "") {
            $("#iClientId").html("")
            return;
        }

        $.ajax({
            type: "POST",
            url: "/ClientMail/GetClients",
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
    GetEmails();
    $("#btnSearchEmail").click(function () {
        GetEmails();
    });
});

function AddEditEmail(iClientId, iMailId) {
    $('#ModalLgContainer').load('/ClientMail/AddEditEmail?iClientId=' + iClientId + "&&iMailId=" + iMailId, function () {
        $.validator.unobtrusive.parse('#frmSaveEmail');
        $('#ModalLg').modal('show');
    });
}

$(document).on('submit', '#frmSaveEmail', function (e) {
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
                    EmailTableTbl.ajax.reload();
                    $('#ModalLg').modal('hide');
                }
                else {
                    toastr.error(res.Message);
                }
            }
        })
    }
});


