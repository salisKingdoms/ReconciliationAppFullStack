$(document).ready(function () {
    console.log("yess document ready");
    var table = $('#listData').DataTable();
    $('#custID').css('disabled', 'false');
    searchDataListCustomer();


});

function validationSubmit() {
    var isokSubmit = true;

    if ($('#email').val() != "") {
        var email = $('#email').val();
        var emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

        if (!emailRegex.test(email)) {
            toastr.error("Format email not valid, please type like youraccount@mail.com");
            isokSubmit = false;
        }
    }

    if ($('#phone').val() == "" || parseInt($('#phone').val()) == 0) {
        console.log("Phone must be filled");
        toastr.error("Phone must be filled");
        isokSubmit = false;
    }

    if ($('#email').val() == "" || parseInt($('#email').val()) == 0) {
        toastr.error("Email must be filled");
        isokSubmit = false;
    }

    if ($('#custName').val() == "") {
        toastr.error("Customer Name must be filled");
        isokSubmit = false;
    }

    return isokSubmit;
}

function saveData() {

    var allowSave = validationSubmit();
    if (allowSave) {

        var dataHeader = {
            code: parseInt($('#custID').val()),
            name: $('#custName').val(),
            email: $('#email').val(),
            phone: $('#phone').val().toString(),
            address: $('#address').val(),
        };

        $.ajax({
            type: "POST",
            url: "/Customer/SubmitOrUpdateCustomer",
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
                        searchDataListCustomer();
                    }, 500);
                }
                else {
                    console.log("Customer failed to submitted");
                    toastr.error(respon.messageUI);

                }
            }

        });
    }
}

function getData() {
    $('#loadingPanel').css('display', 'block');
    var dataHeader = {
        custCode: parseInt($('#custID').val())
    };
    $.ajax({
        type: "GET",
        url: "/Customer/GetCustomerById",
        data: dataHeader,
        success: function (respon) {
            var dataresp = JSON.parse(respon);
            if (dataresp.is_ok) {

                $('#custID').val(dataresp.CustomerCode);
                $('#custName').val(dataresp.CustomerName);
                $('#email').val(dataresp.email);
                $('#phone').val(dataresp.phone);
                $('#address').val(dataresp.CustomerAddress);
                $('#loadingPanel').css('display', 'none');
            }
            else {
                console.log("customer not found");
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
    $('#custID').val(Id);
    getData();
}

function deleteDetail(obj) {
    $("#confirmModal").modal("show");
    var columnValue = $(obj).closest('tr').find('td');
    var Id = parseInt($(obj).attr('id'));
    $('#custID').val(Id);
}


function deleteChoosen() {
    var dataHeader = {
        custCode: parseInt($('#custID').val()),

    };
    $.ajax({
        type: "DELETE",
        url: "/Customer/DeleteCustomer",
        data: dataHeader,
        success: function (respon) {
            if (respon.is_ok) {

                toastr.success("Employee success to delete");
                setTimeout(() => {
                    $('#formCrud').css('display', 'none');
                }, 500);
            }
            else {

                toastr.success("Employee success to delete");

            }
            searchDataListCustomer();
            $("#confirmModal").modal("hide");
        }

    });
}

function onAddNew() {

    $('#formCrud').css('display', 'block');
    Reset();
    $('#formList').css('display', 'none');
}

function onClose() {
    $('#formList').css('display', 'block');
    $('#formCrud').css('display', 'none');
    Reset();
}

function searchDataListCustomer() {

    var paramData = {
        sSearch: $('#custName').val()
    };

    $('#listData').css('width', '100%');
    $('#listData').DataTable().destroy();
    additionalGridConfig = {
        isPaging: true,
        scrollY: "60vh",
        isSearch: true
    }
    return _fw_grid_web_method('listData', "GetCustomereList", paramData,
        [

            {
                "orderable": "false", "bSortable": "false",
                "data": "ID", "name": "-Action-", "targets": "1",
                "render": function (data, type, row) {
                    var html = "<div class='text-center'>" +
                        "<td  style='text-align: center;'>" + "<button id=" + row.CustomerCode + " class='btn btn-sm btn-primary' height='20'  type='submit' onclick=editDetail(this) >View/Edit</button></td>&nbsp;" +
                        "<td  style='text-align: center;'>" + "<button id=" + row.CustomerCode + " class='btn btn-sm btn-danger' height='20'  type='submit' onclick=deleteDetail(this) >Delete</button></td>" +
                    "</div>";
                    return html;

                }
            },
            { "data": "CustomerName", "name": "Name" },
            { "data": "email", "name": "Email" },
            { "data": "phone", "name": "Phone" },
            { "data": "CustomerAddress", "name": "Address" }

        ], function (err) {
        },
        additionalGridConfig);
}

function Reset() {
    $('#custID').val("");
    $('#custName').val("");
    $('#email').val("");
    $('#phone').val("");
    $('#address').val("");
}

function closeModal() {
    $("#confirmModal").modal("hide");
}