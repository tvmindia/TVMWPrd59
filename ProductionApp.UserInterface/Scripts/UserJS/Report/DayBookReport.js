//*****************************************************************************
//*****************************************************************************
//Author:Gibin
//CreatedDate: 07-July-2018 
//LastModified: 
//FileName: DayBookReport.js
//Description: Daily Transactions count Report 
//******************************************************************************
//******************************************************************************

//Global Declaration
var _dataTables = {};
var _emptyGuid = "00000000-0000-0000-0000-000000000000";
var _todayDate;

var _jsonData = {};
var _result = "";
var _message = "";
var _dayBookVM = new Object();



$(document).ready(function () {
   
    try {
        debugger;
        var SearchValue = $('#hdnSearchTerm').val();
        var SearchTerm = $('#SearchTerm').val();
        $('#hdnSearchTerm').val($('#SearchTerm').val());
        _todayDate = $('#dayBookDate').val();
        $('#SearchTerm').keypress(function (event) {
            if (event.which === 13) {
                event.preventDefault();                
                 DayBookDateChanged();
              
            }
        });
        //DayBookTable
        _dataTables.DayBookTable = $('#tblDayBookReport').DataTable({
          //  dom: '<"pull-right"f>rt<"bottom"ip><"clear">',
            dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
            buttons: [{
                extend: 'excel',
                exportOptions:
                             {
                                 columns: [1, 2]
                             }
            }],
            ordering: false,
            searching: false,
            paging: false,
            "bInfo": false,
            data: GetTransactionsList(),//call function here
            pageLength: 25,
            language: {
                search: "_INPUT_",
                searchPlaceholder: "Search"
            },
            "aoColumnDefs": [{
                "aTargets": [0],
                "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                    if (sData.Count > 0)
                        $(nTd).addClass('details-control');
                    else
                        $(nTd).addClass('details-control-disable');
                }
            },
            { "targets": [], "visible": false, "searchable": false },
              { className: "text-center", "targets": [2] },
            ],
            columns: [
                 { "data": null, "defaultContent": "", "width": "5%" },
                 { "data": "TransactionName", "defaultContent": "", "width": "75%", render: function (data, type, row) {
                     return data;
                     }
                 },
                 { "data": "Count", "defaultContent": "<i>-</i>", "width": "20%" ,render: function (data, type, row) {
                     if (row.Count > 0)
                         return '<b>' + data + '</b>';
                     else
                         return data;
                    }
                 }
            ],
            destroy: true
           
        });
        $('#SearchTerm').focus();
        $(".buttons-excel").hide();

        // Add event listener for opening and closing details
        $('#tblDayBookReport tbody').on('click', 'td.details-control', function () {
            debugger;
            var rowData = _dataTables.DayBookTable.row($(this).parents('tr')).data();

            var tr = $(this).closest('tr');
            var row = _dataTables.DayBookTable.row(tr);
            if (row.child.isShown()) {
                // This row is already open - close it
                row.child.hide();
                tr.removeClass('shown');
            }
            else {
                ////Open this row
                //if (row.child() == undefined)
                //{ 
                row.child(CreateDynamicTable(rowData.TransactionCode), style = "padding-left:5%").show();
                //}
                //else
                //{
                //    row.child.show(); 
                //}
                tr.addClass('shown');
            }
        });  

    }
    catch (e) {
        console.log(e.message);
    }
    
});


function GetTransactionsList() {
    try {
        debugger;
        var searchTerm = $('#SearchTerm').val();
        var dayBookDate = $('#dayBookDate').val();
        var data = { "date": dayBookDate, "searchTerm": searchTerm };
        _jsonData = GetDataFromServer("DashboardReport/GetDayBookList/", data);
        if (_jsonData != '') {
            _jsonData = JSON.parse(_jsonData);
            _result = _jsonData.Result;
            _message = _jsonData.Message;
            _dayBookVM = _jsonData.Records;
        }
        if (_result == "OK") {

            return _dayBookVM.DayBookList;
        }
        if (_result == "ERROR") {
            alert(_message);
        }
    }
    catch (e) {
        //this will show the error msg in the browser console(F12) 
        console.log(e.message);
    }
}



function CreateDynamicTable(code) {
    var Records = GetDayBookDetailByCode(code);
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
        return  '<table id="DayBookChildTable' + code + '" class="table table-striped table-bordered table-hover" style="width:100%" > ' +
                ' <thead>' +  trHeader +'</thead>' +
                ' <tbody>' + tbodyrows +'</tbody>' +
                ' </table>'
    }
    else
        return '<div class="text-center"> No data available to List</div>';

}



function GetDayBookDetailByCode(code) {
    debugger;
    try {
            var dayBookDate = $('#dayBookDate').val();
            data = { "code": code, "date": dayBookDate };
            var ds = {};
            ds = GetDataFromServer("DashboardReport/GetDayBookDetailByCode/", data);
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

function DayBookDateChanged() {
    debugger;
    var SearchValue = $('#hdnSearchTerm').val();
    var SearchTerm = $('#SearchTerm').val();
    $('#hdnSearchTerm').val($('#SearchTerm').val());
    var dayBookDate = $('#dayBookDate').val();
    var dayBookDateValue = $('#hdndayBookDate').val();
    $('#hdndayBookDate').val($('#dayBookDate').val());
    if (_dataTables.DayBookTable != undefined)
        if ((SearchValue != SearchTerm) && (dayBookDate != dayBookDateValue)) {
            _dataTables.DayBookTable.clear().rows.add(GetTransactionsList()).draw(false);
        }
    if (SearchValue != SearchTerm && dayBookDate == dayBookDateValue) {
        _dataTables.DayBookTable.clear().rows.add(GetTransactionsList()).draw(false);
    }
    if ((SearchValue == SearchTerm) && dayBookDate != dayBookDateValue) {
         _dataTables.DayBookTable.clear().rows.add(GetTransactionsList()).draw(false);
        
    }
}



function ResetReportList() {
    debugger;
    $('#SearchTerm').val('');
   // $('#dayBookDate').val(_todayDate);
    $('#dayBookDate').datepicker('setDate', _todayDate);
    DayBookDateChanged(); 
}

//function export data to excel
function ExportReportData() {
    $(".buttons-excel").trigger('click');
}
