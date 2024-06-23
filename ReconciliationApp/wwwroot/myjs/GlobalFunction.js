var additionalGridConfig = {
    isPaging: true,
    colGrouping: null,
    colGroupingRowSpan: null,
    flag: null,
    scrollY: null,
    isSearch: false
}

var isChecked = false;
var collapsedGroups = {};
var idGroupLoi = 1;
var selectedLOI = [];

function _fw_grid_web_method(id, url, paramData, columns, callError, additionalGridConfig = null) {
    $('#loadingPanel').css('display', 'block');
    var table = $('#' + id).dataTable({
        "bDestroy": true,
        "bFilter": (additionalGridConfig == null || additionalGridConfig == undefined) ? false : additionalGridConfig.isSearch,
        "paging": (additionalGridConfig == null || additionalGridConfig == undefined) ? true : additionalGridConfig.isPaging,
        "scrollX": true,
        "scrollY": (additionalGridConfig == null || additionalGridConfig == undefined) ? "60vh" : additionalGridConfig.scrollY,
        "autoWidth": false,
        "bScrollCollapse": true,
        "sPaginationType": "full_numbers",
        "bServerSide": true,
        "sAjaxSource": url,
        //"dom": 'lrtip',//delete search box
        "fnServerData": function (sSource, aoData, fnCallback) {
            logsRequest = $.ajax({
                type: "POST",
                url: sSource,
                data: getParamJSON(aoData, paramData),
                //contentType: "application/json; charset=utf-8",
                //dataType: "json",
                success: function (data) {
                    var json = jQuery.parseJSON(data);
                    fnCallback(json);
                    $('#loadingPanel').css('display', 'none');
                },
                error: function (xhr, statusCode, errorThrown) {
                    if (typeof callError === 'function') {
                        callError(xhr, statusCode, errorThrown);
                    }
                    $('#loadingPanel').css('display', 'none');
                }
            });
        },
        "aoColumns": columns
    });

    return table;
}

function reInitDataTables(idObject, sumLeftCol, sumRightIdx) {
    $('#' + idObject).DataTable({
        "bDestroy": true,
        "bFilter": false,
        "paging": true,
        "scrollX": true,
        "scrollY": "60vh",
        "autoWidth": false,
        "bScrollCollapse": true,
        "fixedColumns": {
            left: sumLeftCol,
            right: sumRightIdx
        },
    });

    $($.fn.dataTable.tables(true)).DataTable()
        .columns.adjust();
    //$($.fn.dataTable.tables(true)).DataTable().columns.adjust().responsive.recalc().scroller.measure().fixedColumns().relayout();
    return table;
}

function getParamJSON(aoData, paramData) {
    var strdata = "{"
        + buildParam(paramData)
        + "'sEcho': '" + getValueFromArray(aoData, "sEcho")
        + "', 'sSearch': '" + getValueFromArray(aoData, "sSearch")
        + "', 'iDisplayLength': '" + getValueFromArray(aoData, "iDisplayLength")
        + "', 'iDisplayStart': '" + getValueFromArray(aoData, "iDisplayStart")
        + "', 'iColumns': '" + getValueFromArray(aoData, "iColumns")
        + "', 'iSortingCols': '" + getValueFromArray(aoData, "iSortingCols")
        + "', 'sColumns': '" + getValueFromArray(aoData, "sColumns")
        + "', 'iSortCol': '" + getValueFromArray(aoData, "iSortCol_0")
        + "', 'sSortDir': '" + getValueFromArray(aoData, "sSortDir_0") + "'}"
    strdata = strdata.replace(/'/g, '"')
    //console.log(strdata)
    var jsondata = jQuery.parseJSON(strdata);
    
    return jsondata
}

function getParamJSON2(aoData, paramData) {
    var strdata = "{"
        + buildParam(paramData)
        + "'sEcho': '" + getValueFromArray(aoData, "draw")
        + "', 'sSearch': '" + getValueFromArray(aoData, "search").value
        + "', 'iDisplayLength': '" + getValueFromArray(aoData, "length")
        + "', 'iDisplayStart': '" + getValueFromArray(aoData, "start")
        + "'}"
    strdata = strdata.replace(/'/g, '"')
    //console.log(strdata)
    var jsondata = jQuery.parseJSON(strdata);
    
    return jsondata
}


function buildParam(paramData) {
    var json = "";
    for (var key in paramData) {
        if (paramData.hasOwnProperty(key)) {
            json += "'" + key + "': '" + (paramData[key] === null ? '0' : paramData[key]) + "',"
        }
    }
    return json;
}

function getValueFromArray(aoData, Key) {
    for (i = 0; i < aoData.length; i++) {
        if (aoData[i].name == Key) {
            return aoData[i].value;
        }
    }
}

function scrollToObject(idObject) {
    $('html, body').animate({
        scrollTop: $(idObject).offset().top
    }, 2000);
}

function HideShowObject(idObject, value) {
    $("#" + idObject).css("display", value);
}

function formatCurrency(angka, prefix) {
    var number_string = angka.toString(),
        split = number_string.split(','),
        sisa = split[0].length % 3,
        rupiah = split[0].substr(0, sisa),
        ribuan = split[0].substr(sisa).match(/\d{3}/gi);

    // tambahkan titik jika yang di input sudah menjadi angka ribuan
    if (ribuan) {
        separator = sisa ? '.' : '';
        if (rupiah >= 0) {
            rupiah += separator + ribuan.join('.');
        }
        else {//jika("-"/minus)
            rupiah += ribuan;
        }
    }

    rupiah = split[1] != undefined ? rupiah + ',' + split[1] : rupiah;
    return prefix == undefined ? rupiah : (rupiah ? 'Rp. ' + rupiah : '');
}


function formatCurrencyDashboard(angka, prefix) {
    var number_string = angka.toString(),
        split = number_string.split(','), sisa = 0, rupiah = 0, ribuan = 0;

    if (number_string.includes("-")) {
        split = split[0].split("-");
        sisa = split[1].length % 3;
        rupiah = split[1].substr(0, sisa);
        ribuan = split[1].substr(sisa).match(/\d{3}/gi);
    }
    else {
        sisa = split[0].length % 3;
        rupiah = split[0].substr(0, sisa);
        ribuan = split[0].substr(sisa).match(/\d{3}/gi)
    }

    // tambahkan titik jika yang di input sudah menjadi angka ribuan
    if (ribuan) {
        separator = sisa ? '.' : '';
        if (rupiah >= 0) {
            rupiah += separator + ribuan.join('.');
        }
        else {//jika("-"/minus)
            rupiah += ribuan;
        }
    }

    rupiah = (number_string.includes("-")) ? ("-" + rupiah) : (split[1] != undefined ? rupiah + ',' + split[1] : rupiah);
    return prefix == undefined ? rupiah : (rupiah ? 'Rp. ' + rupiah : '');
}

function firstSequence(paramType, MenuType) {
    //set param data
    var paramData = {
        firstSeqCat: paramType,
        firstSeqMenuCat: MenuType
    };

    $.ajax({
        type: "post",
        url: "/Global/FirstSequence",
        data: paramData,
        async: false,
        success: function (response) {
            if (!response.is_ok) {
                //error
                if (paramType == "Button") {
                    $('#btnAdd').hide();
                }
                else {
                    if (response.message != "") {
                        toastr.error(response.message);
                        setTimeout(function () {
                            window.top.close();
                        }, 5000);
                    }
                }
            }
            else {
                //success
                if (paramType == "Button") {
                    $('#btnAdd').show();
                }
            }
        }
    });
}

function NumericInput(inp, locale) {
    var numericKeys = '0123456789.';

    // restricts input to numeric keys 0-9
    inp.addEventListener('keypress', function (e) {
        var event = e || window.event;
        var target = event.target;

        if (event.charCode == 0) {
            return;
        }

        if (-1 == numericKeys.indexOf(event.key)) {
            // Could notify the user that 0-9 is only acceptable input.
            event.preventDefault();
            return;
        }
    });

    // add the thousands separator when the user keyup
    inp.addEventListener('keyup', function (e) {
        var event = e || window.event;
        var target = event.target;

        var tmp = target.value.replace(/,/g, '');
        var val = Number(tmp).toLocaleString(locale);

        if (tmp == '') {
            target.value = '';
        } else {
            target.value = val;
        }
    });

    // add the thousands separator when the user blurs
    inp.addEventListener('blur', function (e) {
        var event = e || window.event;
        var target = event.target;

        var tmp = target.value.replace(/,/g, '');
        var val = Number(tmp).toLocaleString(locale);

        if (tmp == '') {
            target.value = '';
        } else {
            target.value = val;
        }
    });

    // strip the thousands separator when the user puts the input in focus.
    inp.addEventListener('focus', function (e) {
        var event = e || window.event;
        var target = event.target;
        var val = target.value.replace(/[,.]/g, '');

        target.value = val;
    });
}

