//*****************************************************************************
//*****************************************************************************
//Author:Gibin
//CreatedDate: 21-Jul-2018 
//LastModified: 
//FileName: MovementAnalysisReport.js
//Description: Client side coding for Sales Analysis Report
//******************************************************************************
//******************************************************************************
//Global Declaration
var DataTables = {};
var EmptyGuid = "00000000-0000-0000-0000-000000000000";
$(document).ready(function () {
    debugger;
    try {
        $('#ProductID').select2({
        });
        $('#EmployeeID').select2({
        });
        DynamicTableBinding();
    }
    catch (e) {
        console.log(e.message);
    }
});

//Click function for search
function RedirectSearchClick(e, this_obj) {
    debugger;
    var unicode = e.charCode ? e.charCode : e.keyCode
    if (unicode == 13) {
        $(this_obj).closest('.input-group').find('button').trigger('click')
    }
}

function DynamicTableBinding() {
    $('#divMovementAnalysisReport').empty();
    $('#divMovementAnalysisReport').append(CreateDynamicTable());
    FireDatatable();
}

function FilterOnchange() {
    DynamicTableBinding();
}
//function reset the list to initial
function ResetReportList() {
    $('#EmployeeID').val('').select2();
    $('#ProductID').val('').select2();
    $('#FromDate').val('');
    $('#ToDate').val('');
    $('#MonthFilter').val(3);
    DynamicTableBinding();
}



//function to filter data based on date
function DateFilterOnchange() {

}


function CreateDynamicTable() {
    var Records = GetMovementAnalysisReport();
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
            return '<table id="tblMovementAnalysisReport" class="table table-striped table-bordered table-hover" style="width:100%" > ' +
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
    DataTables.MovementAnalysisReport = $('#tblMovementAnalysisReport').DataTable({
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
        order: false,
    });
    $(".buttons-excel").hide();
}

//function export data to excel
function ExportReportData() {
    $(".buttons-excel").trigger('click');
}

function GetMovementAnalysisReport() {
    debugger;
    try {

        var SalesPerson = $('#EmployeeID').val();
        var ProductID = $('#ProductID').val();
        var FromDate = $('#FromDate').val();
        var ToDate = $('#ToDate').val();
        var MonthFilter = $('#MonthFilter').val();

        data = { "SalesPerson": SalesPerson, "FromDate": FromDate, "ToDate": ToDate, "MonthFilter": MonthFilter, "ProductID": ProductID };
        var ds = {};
        ds = GetDataFromServer("DashboardReport/GetMovementAnalysisReport/", data);
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