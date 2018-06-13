//*****************************************************************************
//*****************************************************************************
//Author:Sruthi
//CreatedDate: 17-May-2018 
//LastModified: 18-May-2018 
//FileName: StockLedgerReport.js
//Description: Client side coding for Stock Ledger Report
//******************************************************************************
//******************************************************************************
//Global Declaration
var DataTables = {};
var EmptyGuid = "00000000-0000-0000-0000-000000000000";
$(document).ready(function () {
    debugger;
    try {
        $("#MaterialType,#MaterialID").select2({
        });
        BindOrReloadStockLedgerTable('Init');

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

//bind stock ledger report
function 
    BindOrReloadStockLedgerTable(action) {
    debugger;
    try {
        //creating advancesearch object       
        StockLedgerReportViewModel = new Object();
        DataTablePagingViewModel = new Object();
        DataTablePagingViewModel.Length = 0;


        //switch case to check the operation
        switch (action) {
            case 'Reset':
                $('#SearchTerm').val('');                
                $('#FromDate').val('');
                $('#ToDate').val('');
                $('#MaterialType').val('').select2();
                $('#TransactionType').val('');               
                $('#DateFilter').val('30');
                $('#MaterialID').val('').select2();
                break;
            case 'Init':
                break;
            case 'Search':
                break;
            case 'Apply':
                StockLedgerReportViewModel.FromDate = $('#FromDate').val();
                StockLedgerReportViewModel.ToDate = $('#ToDate').val();
                StockLedgerReportViewModel.TransactionType = $('#TransactionType').val();
                StockLedgerReportViewModel.MaterialTypeCode = $('#MaterialType').val();
                StockLedgerReportViewModel.MaterialID = $('#MaterialID').val();
                if ((StockLedgerReportViewModel.FromDate == "") && (StockLedgerReportViewModel.ToDate == ""))
                {
                    StockLedgerReportViewModel.DateFilter = $('#DateFilter').val();
                }
                else
                {
                    $('#DateFilter').val('');
                }                
                
                break;
            case 'Export':
                DataTablePagingViewModel.Length = -1;
                break;
            default:
                break;
        }
        StockLedgerReportViewModel.DataTablePaging = DataTablePagingViewModel;
        StockLedgerReportViewModel.SearchTerm = $('#SearchTerm').val();
        StockLedgerReportViewModel.FromDate = $('#FromDate').val();
        StockLedgerReportViewModel.ToDate = $('#ToDate').val();
        StockLedgerReportViewModel.TransactionType = $('#TransactionType').val();
        StockLedgerReportViewModel.MaterialTypeCode = $('#MaterialType').val();
        StockLedgerReportViewModel.MaterialID = $('#MaterialID').val();
        StockLedgerReportViewModel.DateFilter = $('#DateFilter').val();
        DataTables.StockLedgerList = $('#tblStockLedgerReport').DataTable(

            {

                dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
                buttons: [{
                    extend: 'excel',
                    exportOptions:
                                 {
                                     columns: [1, 2, 3, 4, 5, 6, 7, 8, 9]
                                 }
                }],

                order: false,
                ordering: false,
                searching: false,
                paging: true,
                lengthChange: false,
                proccessing: true,
                serverSide: true,
                ajax: {
                    url: "GetStockLedgerReport/",
                    data: { "stockLedgerReportVM": StockLedgerReportViewModel },
                    type: 'POST'
                },
                pageLength: 15,
                columns: [
                    { "data": "MaterialID", "defaultContent": "<i>-</i>" },
                    { "data": "Description", "defaultContent": "<i>-</i>" },
                    { "data": "UnitCode", "defaultContent": "<i>-</i>" },
                    { "data": "OpeningStock", "defaultContent": "<i>-</i>" },
                    { "data": "ClosingStock", "defaultContent": "<i>-</i>" },
                    { "data": "TransactionType", "defaultContent": "<i>-</i>" },
                    { "data": "DocumentNo", "defaultContent": "<i>-</i>" },
                    { "data": "TransactionDateFormatted", "defaultContent": "<i>-</i>" },
                    { "data": "StockIn", "defaultContent": "<i>-</i>" },
                    { "data": "StockOut", "defaultContent": "<i>-</i>" },

                ],
                columnDefs: [{ "targets": [0, 3, 4], "visible": false, "searchable": false },
                    { className: "text-left", "targets": [1] },
                    { className: "text-center", "targets": [2,5,6,7] },
                    { className: "text-right", "targets": [8, 9] }
                ],

                destroy: true,
                //for performing the import operation after the data loaded
                initComplete: function (settings, json) {
                    if (action === 'Export') {
                        if (json.data.length > 0) {
                            if (json.data[0].TotalCount > 10000) {
                                MasterAlert("info", 'We are able to download maximum 10000 rows of data, There exist more than 10000 rows of data please filter and download')
                            }
                        }
                        $(".buttons-excel").trigger('click');
                        BindOrReloadStockLedgerTable('Search');
                    }
                },
                // grouping with  columns
                drawCallback: function (settings) {
                    var api = this.api();
                    var rows = api.rows({ page: 'current' }).nodes();
                    var last = null;

                    api.column(1, { page: 'current' }).data().each(function (group, i) {
                        if (last !== group) {
                            debugger;
                            var rowData = api.row(i).data();
                            $(rows).eq(i).before('<tr class="group"><td colspan="7" class="rptGrp">' +
                                        '<div class="col-md-4" style="padding:0px;"><b>Item</b> :' + group + '</div>' +
                                        '<div class="col-md-2"><b>OpeningStock</b> : ' + rowData.OpeningStock + '</div>' +
                                        '<div class="col-md-2"><b>ClosingStock</b> : ' + rowData.ClosingStock + '</div>' +
                                        '</td></tr>');
                            last = group;
                        }
                    });
                }
            });
        $(".buttons-excel").hide();       
    }
    catch (e) {
        console.log(e.message);
    }
}

//function reset the list to initial
function ResetReportList() {
    BindOrReloadStockLedgerTable('Reset');
}

//function export data to excel
function ExportReportData() {
    BindOrReloadStockLedgerTable('Export');
}

//function to filter data based on date
function DateFilterOnchange() {
    BindOrReloadStockLedgerTable('Apply');
}