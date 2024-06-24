var paramSearchFilter = {
    custCode: 0,
    trxID: 0,
    trxType: null,
    currency: null,
    amount: 0,
    trxDateStart: null,
    trxDateEnd: null,
    pic: null
};

$(document).ready(function () {
    //$('#trxDate').datetimepicker
    //    ({
    //        format: 'YYYY-MM-DD'
    //    });
    //$('#datePickerFrom').datetimepicker
    //    ({
    //        format: 'YYYY-MM-DD'
    //    });
    //$('#datePickerTo').datetimepicker
    //    ({
    //        format: 'YYYY-MM-DD'
    //    });
    console.log("tes ok ya");
    var table = $('#listData').DataTable();
    $('#tellerID').prop('disabled', false);
    searchDataListTransaction();
});

function validationSubmit() {
    var isokSubmit = true;

    if ($('#pic').val() == "") {
        toastr.error("Bank PIC must be filled");
        isokSubmit = false;
    }

    if ($('#trxDate').val() == "") {
        toastr.error("Transaction Date must be filled");
        isokSubmit = false;
    }

    if ($('#amount').val() == "" || parseInt($('#amount').val()) == 0) {
        toastr.error("Amount must be filled and must be greater than zero");
        isokSubmit = false;
    }

    if ($('#denomination').val() == "" || parseInt($('#denomination').val()) == 0) {
        toastr.error("Denomination must be filled and must be greater than zero");
        isokSubmit = false;
    }

    if ($('#currencyType').val() == "") {
        toastr.error("Currency Type must be filled");
        isokSubmit = false;
    }

    if ($('#transType').val() == "") {
        toastr.error("Transaction Type must be filled");
        isokSubmit = false;
    }

    if ($('#customerList').val() == "") {
        toastr.error("Customer Code must be filled");
        isokSubmit = false;
    }

    if ($('#tellerID').val() == "") {
        toastr.error("Teller ID must be filled");
        isokSubmit = false;
    }

    return isokSubmit;
}

function loadCustomer(isEdit, obj) {
    $.ajax({
        type: "GET",
        url: "/Transaction/GetCustomerList",
        success: function (response) {
            dataCmb = null;
            dataCmb = JSON.parse(response);
            if (!dataCmb.is_ok) {
                $("#customerList").empty();
                toastr.error(dataCmb.message);
                setTimeout(function () {
                    window.top.close();
                }, 5000);
            }
            else {
                $("#customerList").empty();
                $("#customerList").append($("<option></option>").val(this['']).html('-Select Customer Code-'));
                $.each(dataCmb.data, function () {
                    $("#customerList").append($("<option></option>").val(this['CustomerCode']).html(this['CustomerName']));
                });


            }
        }
    });
}

function getCustomerCode() {
    var selectedValue = $('#customerList').val();

    console.log(selectedValue);
}

function saveData() {

    var allowSave = validationSubmit();
    if (allowSave) {

        var dataHeader = {
            transID: parseInt($('#transID').val()),
            tellerID: $('#tellerID').val(),
            customerCode: parseInt($('#customerList').val()),
            transType: $('#transType').val(),
            currency: $('#currencyType').val(),
            denomination: parseFloat($('#denomination').val()),
            amount: parseFloat($('#amount').val()),
            transDate: $('#trxDate').val(),
            pic: $('#pic').val()
        };

        $.ajax({
            type: "POST",
            url: "/Transaction/SubmitOrUpdateTransaction",
            data: dataHeader,
            success: function (respon) {
                if (respon.is_ok) {

                    if (respon.messageUIError != "") {
                        toastr.error(respon.messageUIError);
                    }
                    else {
                        toastr.success(respon.messageUI);
                    }

                    setTimeout(() => {
                        $('#formCrud').css('display', 'none');
                        $('#formList').css('display', 'block');
                        searchDataListTransaction();
                    }, 500);
                }
                else {
                    console.log("Transaction failed to submitted");
                    toastr.error(respon.messageUI);

                }
            }

        });
    }
}

function getData() {
    $('#loadingPanel').css('display', 'block');
    var dataHeader = {
        transID: parseInt($('#transID').val())
    };
    $.ajax({
        type: "GET",
        url: "/Transaction/GetTransactionById",
        data: dataHeader,
        success: function (respon) {
            var dataresp = JSON.parse(respon);
            if (dataresp.is_ok) {
                $('#tellerID').prop('disabled', true);
                $('#transID').val(dataresp.TransactionID);
                $('#tellerID').val(dataresp.TellerID);
                $('#customerList').val(dataresp.CustomerCode).trigger('change.select2');
                //$('#customerList').val(dataresp.CustomerCode);
                getCustomerCode();
                // 
                $('#transType').val(dataresp.TransactionType);
                $('#currencyType').val(dataresp.Currency);
                $('#denomination').val(dataresp.Denomination);
                $('#amount').val(dataresp.Amount);
                $('#trxDate').val(moment(dataresp.TransactionDateTime).format('YYYY-MM-DD'));
                $('#pic').val(dataresp.BankPIC);
                $('#loadingPanel').css('display', 'none');
            }
            else {
                console.log("Transaction not found");
                toastr.error(dataresp.messageUI);
                $('#loadingPanel').css('display', 'none');
            }
        }

    });
}


function editDetail(obj) {
    $('#formCrud').css('display', 'block');
    $('#formList').css('display', 'none');
    var columnValue = $(obj).closest('tr').find('td');
    var Id = parseInt($(obj).attr('id'));
    $('#transID').val(Id);
    loadCustomer(false, "");
    getData();
}

function deleteDetail(obj) {
    $("#confirmModal").modal("show");
    var columnValue = $(obj).closest('tr').find('td');
    var Id = parseInt($(obj).attr('id'));
    $('#transID').val(Id);
}


function deleteChoosen() {
    var dataHeader = {
        transID: parseInt($('#transID').val())

    };
    $.ajax({
        type: "DELETE",
        url: "/Transaction/DeleteTransaction",
        data: dataHeader,
        success: function (respon) {
            if (respon.is_ok) {

                toastr.success("Transaction success to delete");
                setTimeout(() => {
                    $('#formCrud').css('display', 'none');
                }, 500);
            }
            else {

                toastr.success("Transaction success to delete");

            }
            searchDataListTransaction();
            $("#confirmModal").modal("hide");
        }

    });
}

function onAddNew() {

    $('#formCrud').css('display', 'block');
    Reset();
    ResetFilter();
    loadCustomer(false, "");
    $('#formList').css('display', 'none');
}

function onClose() {
    $('#formList').css('display', 'block');
    $('#formCrud').css('display', 'none');
    Reset();
    ResetFilter();
}

function searchFilter() {

    paramSearchFilter = {
        custCode: $('#txtcustCode').val(),
        trxID: $('#txtNO').val(),
        trxType: $('#typeFilter').val(),
        currency: $('#currencyFilter').val(),
        amount: $('#txtAmount').val(),
        trxDateStart: $('#datePickerFrom').val(),
        trxDateEnd: $('#datePickerTo').val(),
        pic: $('#txtPIC').val()
    };

    searchDataListTransaction();
}

function searchDataListTransaction() {

    var paramData = {
        // sSearch: "",
        custCode: paramSearchFilter.custCode,
        trxID: paramSearchFilter.trxID,
        trxType: paramSearchFilter.trxType,
        currency: paramSearchFilter.currency,
        amount: paramSearchFilter.amount,
        trxDateStart: paramSearchFilter.trxDateStart,
        trxDateEnd: paramSearchFilter.trxDateEnd,
        pic: paramSearchFilter.pic
    };

    $('#listData').css('width', '100%');
    $('#listData').DataTable().destroy();
    additionalGridConfig = {
        isPaging: true,
        scrollY: "60vh",
        isSearch: false
    }
    return _fw_grid_web_method('listData', "GetTransactionList", paramData,
        [

            {
                "orderable": "false", "bSortable": "false",
                "data": "TransactionID", "name": "-Action-", "targets": "1",
                "render": function (data, type, row) {
                    var html = "<div class='text-center'>" +
                        "<button class='btn btn-sm btn-success'  id=" + row.TransactionID + " height='10' type='button' data-toggle='tooltip' data-placement='top' title='Detail Data' name='btnDetail' onclick=editDetail(this); ><em class='fa fa-edit'></em></button>&nbsp;" +
                        "<button class='btn btn-sm btn-danger'  id=" + row.TransactionID + " height='10' type='button' data-toggle='tooltip' data-placement='top' title='Delete Data' name='btnDeleted' onclick=deleteDetail(this); ><em class='fa fa-trash'></em></button>"
                    "</div>";
                    return html;

                }
            },
            { "data": "TransactionID", "name": "No" },
            { "data": "CustomerCode", "name": "Customer Code" },
            { "data": "TransactionType", "name": "Transaction Type" },
            { "data": "Currency", "name": "Currency" },
            { "data": "Amount", "name": "Amount" },
            { "data": "TransactionDateTime", "name": "Transaction Date" },
            { "data": "BankPIC", "name": "Bank PIC" }

        ], function (err) {
        },
        additionalGridConfig);
}

function Reset() {
    $('#tellerID').val("");
    $('#customerList').val("");
    $('#transType').val("");
    $('#currencyType').val("");
    $('#denomination').val("");
    $('#amount').val("");
    $('#trxDate').val("");
    $('#pic').val("");
    $('#transID').val("");
    $('#tellerID').prop('disabled', false);
}

function ResetFilter() {
    $('#txtcustCode').val("");
    $('#txtNO').val("");
    $('#typeFilter').val("");
    $('#currencyFilter').val("");
    $('#txtAmount').val("");
    $('#datePickerFrom').val("");
    $('#datePickerTo').val("");
    $('#txtPIC').val("");

}

function onOpeningBalance() {
    $('#loadingPanel').css('display', 'block');
    var dataHeader = {};
    $.ajax({
        type: "POST",
        url: "/Transaction/OpeningBalance",
        data: dataHeader,
        success: function (respon) {
            if (respon.is_ok) {
                toastr.success("Opening Balance success");
            }
            else {
                toastr.error("Opening Balance Failed, Please contact administrator");
            }
            setTimeout(() => {
                $('#loadingPanel').css('display', 'none');
            }, 500);
        }

    });
}