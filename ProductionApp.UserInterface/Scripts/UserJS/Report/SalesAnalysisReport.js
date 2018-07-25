//*****************************************************************************
//*****************************************************************************
//Author:Gibin
//CreatedDate: 21-Jul-2018 
//LastModified: 
//FileName: SalesAnalysisReport.js
//Description: Client side coding for Sales Analysis Report
//******************************************************************************
//******************************************************************************
//Global Declaration
var DataTables = {};
var EmptyGuid = "00000000-0000-0000-0000-000000000000";
$(document).ready(function () {
    debugger;
    try {
        DynamicTableBinding();
         
    }
    catch (e) {
        console.log(e.message);
    }
});

//Click function for search
debugger;
function RedirectSearchClick(e, this_obj) {
    debugger;
    var unicode = e.charCode ? e.charCode : e.keyCode
    if (unicode == 13) {
        $(this_obj).closest('.input-group').find('button').trigger('click')
    }
}

//function reset the list to initial
function ResetReportList() {
    $("#isInvoiced").prop("checked", true);
    $('#FromDate').val('');
    $('#ToDate').val('');
    $('#DateFilter').val('');
    DynamicTableBinding();
}



////function to filter data based on date
//function DateFilterOnchange() {
//    $('#DateFilter').val('');
//    DynamicTableBinding();

//}

function FilterOnchange(code) {
    debugger;
    if (code == 1) {

    }
    if (code == 2) {
        $('#DateFilter').val('');
    }
    if (code == 3) {
        $('#FromDate').val('');
        $('#ToDate').val('');
    }
    DynamicTableBinding();

}
//function export data to excel
function ExportReportData() {
   $(".buttons-excel").trigger('click');
}


function DynamicTableBinding() {
    $('#divSalesAnalysisReport').empty();
    $('#divSalesAnalysisReport').append(CreateDynamicTable());
    FireDatatable();
}


function CreateDynamicTable() {
    var Records = GetSalesAnalysisReport();
    debugger;
    if (Records.Table != undefined) {
        if (Records.Table.length != 0) {
            var Header = [];
            var trHeader = '';
            var tbodyrows = '';

            $.each(Records.Table, function (index, Records) {
                if (Header.length == 0) {
                    $.each(Records, function (key, value) {
                        Header.push(key);
                    });
                    var i = 0;
                    for (i = 0; i < Header.length; i++) {
                        trHeader = trHeader + '<th>' + Header[i] + ' </th>';
                    }
                }
                var html = '';
                $.each(Records, function (key, value) {
                    html = html + "<td>" + ((value != null && value != 0) ? value : "-") + "</td>"
                });
                tbodyrows = tbodyrows + '<tr>' + html + '</tr>';
            });
            return '<table id="tblSalesAnalysisReport" class="table table-striped table-bordered table-hover">' +
                    ' <thead>' + trHeader + '</thead>' +
                    ' <tbody>' + tbodyrows + '</tbody>' +
                    ' </table>'
        }
        else
            return '<div class="text-center">  &nbsp;</br><b> No data available to List</b></div>';
    }
    else
        return '<div class="text-center">  &nbsp;</br><b> No data available to List</b></div>';

}
function FireDatatable() {
    DataTables.SalesAnalysisReport = $('#tblSalesAnalysisReport').DataTable({
        dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
        buttons: [{
            extend: 'excel',
            exportOptions:
                         {
                             columns: ':visible'
                         }
        }],
        scrollY: true,
        scrollX: true,
        scrollCollapse: true,
        paging: false,
        fixedColumns: {
            leftColumns: 1,
            rightColumns: 0
        },
        columnDefs: [
    {
        width: "100px",
        "targets": "_all"
    },
        ],
        searching: false,
        ordering: false,
        "bInfo": false,
        order: false 
    });
    $(".buttons-excel").hide();

}



function GetSalesAnalysisReport() {
    debugger;
    try {
        var IsInvoicedOnly;
        if ($("#isInvoiced").prop('checked')) {
            IsInvoicedOnly = true;
        }
        else {
            IsInvoicedOnly = false;
        }

        var FromDate    = $('#FromDate').val();
        var ToDate      = $('#ToDate').val();
        var DateFilter  = $('#DateFilter').val();
     

        data = { "IsInvoicedOnly": IsInvoicedOnly, "FromDate": FromDate, "ToDate": ToDate, "DateFilter": DateFilter };
        var ds = {};
        ds = GetDataFromServer("DashboardReport/GetSalesAnalysisReport/", data);
        if (ds != '') {
            ds = JSON.parse(ds);
        }
        if (ds.Result == "OK") {
            return ds.Records;
        }
        if (ds.Result == "ERROR") {
            notyAlert('error', ds.Message);
        }
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}