var paramSearchFilter = {
    trxDateStart: null,
    trxDateEnd: null,
    tellerID: null
};
$(document).ready(function () {
    $('#datePickerFrom').datetimepicker
        ({
            format: 'YYYY-MM-DD'
        });
    $('#datePickerTo').datetimepicker
        ({
            format: 'YYYY-MM-DD'
        });
    var table = $('#listData').DataTable();
    var table2 = $('#listDataBalance').DataTable();
    searchDataBalance();
    searchReconciliation();
});

function onCloseBalance() {
    $('#loadingPanel').css('display', 'block');
    var dataHeader = {};
    $.ajax({
        type: "POST",
        url: "/Transaction/ClosingBalance",
        data: dataHeader,
        success: function (respon) {
            var dataresp = JSON.parse(respon);
            if (dataresp.is_ok) {
                toastr.success("Closing Balance success");
                searchDataBalance();
            }
            else {
                toastr.error("Closing Balance Failed, Please contact administrator");
            }
            setTimeout(() => {
                $('#loadingPanel').css('display', 'none');
            }, 500);
        }

    });
}

function searchDataBalance() {
    var paramData = {

    };

    $('#listDataBalance').css('width', '100%');
    $('#listDataBalance').DataTable().destroy();
    additionalGridConfig = {
        isPaging: true,
        scrollY: "60vh",
        isSearch: false
    }
    return _fw_grid_web_method('listDataBalance', "GetClosingBalance", paramData,
        [
            { "data": "DateTransaction", "name": "Transaction Date" },
            { "data": "OpeningBalance", "name": "Opening Balance" },
            { "data": "ClosingBalance", "name": "Closing Balance" },

        ], function (err) {
        },
        additionalGridConfig);
}

function searchReconcile() {
    paramSearchFilter = {
        trxDateStart: $('#datePickerFrom').val(),
        trxDateEnd: $('#datePickerTo').val(),
        tellerID: $('#txtTellerID').val()
    };
    searchReconciliation();
}

function searchReconciliation() {
    var paramData = {
        trxDateStart: paramSearchFilter.trxDateStart,
        trxDateEnd: paramSearchFilter.trxDateEnd,
        tellerID: paramSearchFilter.tellerID
    };

    $('#listData').css('width', '100%');
    $('#listData').DataTable().destroy();
    additionalGridConfig = {
        isPaging: true,
        scrollY: "60vh",
        isSearch: false
    }
    return _fw_grid_web_method('listData', "GetReconsiliation", paramData,
        [
            { "data": "TellerID", "name": "Teller ID" },
            { "data": "TransactionDate", "name": "Transaction Date" },
            { "data": "TotalDeposits", "name": "Total Deposits" },
            { "data": "TotalWithdrawals", "name": "Total Withdrawals" },
        ], function (err) {
        },
        additionalGridConfig);
}