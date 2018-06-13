//*****************************************************************************
//*****************************************************************************
//Author:Sruthi
//CreatedDate: 23-May-2018 
//LastModified: 23-May-2018 
//FileName: StockLedgerFGReport.js
//Description: Client side coding for Stock Ledger FG Report
//******************************************************************************
//******************************************************************************
//Global Declaration
var DataTables = {};
var EmptyGuid = "00000000-0000-0000-0000-000000000000";
$(document).ready(function () {
    debugger;
    try {
        
        BindOrReloadStockLedgerFGTable('Init');

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
    BindOrReloadStockLedgerFGTable(action) {
    debugger;
    try {
        //creating advancesearch object       
        StockLedgerFGReportViewModel = new Object();
        DataTablePagingViewModel = new Object();
        DataTablePagingViewModel.Length = 0;


        //switch case to check the operation
        switch (action) {
            case 'Reset':
                $('#SearchTerm').val('');
                $('#FromDate').val('');
                $('#ToDate').val('');               
                $('#TransactionType').val('');
                $('#Type').val('');
                $('#DateFilter').val('30');
                break;
            case 'Init':
                break;
            case 'Search':
                break;
            case 'Apply':
                StockLedgerFGReportViewModel.FromDate = $('#FromDate').val();
                StockLedgerFGReportViewModel.ToDate = $('#ToDate').val();
                StockLedgerFGReportViewModel.TransactionType = $('#TransactionType').val();
                StockLedgerFGReportViewModel.ProductType = $('#Type').val();

                if ((StockLedgerFGReportViewModel.FromDate == "") && (StockLedgerFGReportViewModel.ToDate == "")) {
                    StockLedgerFGReportViewModel.DateFilter = $('#DateFilter').val();
                }
                else {
                    $('#DateFilter').val('');
                }

                break;
            case 'Export':
                DataTablePagingViewModel.Length = -1;
                break;
            default:
                break;
        }
        StockLedgerFGReportViewModel.DataTablePaging = DataTablePagingViewModel;
        StockLedgerFGReportViewModel.SearchTerm = $('#SearchTerm').val();
        StockLedgerFGReportViewModel.FromDate = $('#FromDate').val();
        StockLedgerFGReportViewModel.ToDate = $('#ToDate').val();
        StockLedgerFGReportViewModel.TransactionType = $('#TransactionType').val();
        StockLedgerFGReportViewModel.ProductType = $('#Type').val();
        StockLedgerFGReportViewModel.DateFilter = $('#DateFilter').val();
        DataTables.StockLedgerFGList = $('#tblStockLedgerFGReport').DataTable(

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
                    url: "GetStockLedgerFGReport/",
                    data: { "stockLedgerFGReportVM": StockLedgerFGReportViewModel },
                    type: 'POST'
                },
                pageLength: 15,
                columns: [
                    { "data": "ProductID", "defaultContent": "<i>-</i>" },
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
                columnDefs: [{ "targets": [3, 4, 0], "visible": false, "searchable": false },
                    { className: "text-left", "targets": [1] },
                    { className: "text-center", "targets": [2, 5, 6, 7] },
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
                        BindOrReloadStockLedgerFGTable('Search');
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
    BindOrReloadStockLedgerFGTable('Reset');
}

//function export data to excel
function ExportReportData() {
    BindOrReloadStockLedgerFGTable('Export');
}

//function to filter data based on date
function DateFilterOnchange() {
    BindOrReloadStockLedgerFGTable('Apply');
}