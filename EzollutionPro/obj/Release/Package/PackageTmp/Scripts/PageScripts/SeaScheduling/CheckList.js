$(function () {
    var msg = $('#SuccessMessage').val();
    if (msg !== "") {
        toastr.success(msg);
    }

})

function DeleteMBL(iMBLId) {
    var res = confirm("Are you sure you want to delete MBL?");
    if (res) {
        $.ajax({
            url: '/SeaScheduling/DeleteMBL',
            method: 'POST',
            data: { iMBLId: iMBLId },
            success: function (res) {
                if (res.Status) {
                    toastr.success(res.Message);
                    setTimeout(function () {
                        location.reload()
                    }, 1000);
                }
                else
                    toastr.error(res.Message);
            }
        })
    }
}

function DeleteHBL(iHBLId) {
    var res = confirm("Are you sure you want to delete HBL?");
    if (res) {
        $.ajax({
            url: '/SeaScheduling/DeleteHBL',
            method: 'POST',
            data: { iHBLId: iHBLId },
            success: function (res) {
                if (res.Status) {
                    toastr.success(res.Message);
                    setTimeout(function () {
                        location.reload()
                    }, 1000);
                }
                else
                    toastr.error(res.Message);
            }
        })
    }
}

function DeleteContainer(iContainerId) {
    var res = confirm("Are you sure you want to delete Container?");
    if (res) {
        $.ajax({
            url: '/SeaScheduling/DeleteContainer',
            method: 'POST',
            data: { iContainerId: iContainerId },
            success: function (res) {
                if (res.Status) {
                    toastr.success(res.Message);
                    setTimeout(function () {
                        location.reload()
                    }, 1000);
                }
                else
                    toastr.error(res.Message);
            }
        })
    }
}
