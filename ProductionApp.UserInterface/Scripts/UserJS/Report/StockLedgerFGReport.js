//*****************************************************************************
//*****************************************************************************
//Author:Sruthi
//CreatedDate: 23-May-2018 
//LastModified: 26-Sep-2018 
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
        $('#SearchTerm').focus();
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
        var SearchValue = $('#hdnSearchTerm').val();
        var SearchTerm = $('#SearchTerm').val();
        $('#hdnSearchTerm').val($('#SearchTerm').val());

        //switch case to check the operation
        switch (action) {
            case 'Reset':
                $('#SearchTerm').val('');
                $('#FromDate').val('');
                $('#ToDate').val('');               
                $('#TransactionType').val('');
                $('#Type').val('');
                $('#DateFilter').val('30');
                $('.datepicker').datepicker('setDate', null);

                break;
            case 'Init':
                break;
            case 'Search':
                if (SearchTerm == SearchValue) {
                    return false;
                }
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
                StockLedgerFGReportViewModel.DataTablePaging = DataTablePagingViewModel;
                StockLedgerFGReportViewModel.SearchTerm = $('#SearchTerm').val() == "" ? null : $('#SearchTerm').val();
                StockLedgerFGReportViewModel.FromDate = $('#FromDate').val() == "" ? null : $('#FromDate').val();
                StockLedgerFGReportViewModel.ToDate = $('#ToDate').val() == "" ? null : $('#ToDate').val();
                StockLedgerFGReportViewModel.TransactionType = $('#TransactionType').val() == "" ? null : $('#TransactionType').val();
                StockLedgerFGReportViewModel.ProductType = $('#Type').val() == "" ? null : $('#Type').val();
                StockLedgerFGReportViewModel.DateFilter = $('#DateFilter').val() == "" ? null : $('#DateFilter').val();
                $('#AdvanceSearch').val(JSON.stringify(StockLedgerFGReportViewModel));
                $('#FormExcelExport').submit();
                return true;
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
                columnDefs: [{ "targets": [2,3], "visible": false, "searchable": false },
                    { className: "text-left", "targets": [0,1,4,5] },
                    { className: "text-center", "targets": [6] },
                    { className: "text-right", "targets": [7,8] }
                ],

                destroy: true,
                //for performing the import operation after the data loaded
                initComplete: function (settings, json) {
                    debugger;
                    $('.dataTables_wrapper div.bottom div').addClass('col-md-6');
                    $('#tblStockLedgerFGReport').fadeIn('slow');
                    if (action == undefined) {
                        $('.excelExport').hide();
                        OnServerCallComplete();
                    }

                },
                // grouping with  columns
                drawCallback: function (settings) {
                    var api = this.api();
                    var rows = api.rows({ page: 'current' }).nodes();
                    var last = null;

                    api.column(0, { page: 'current' }).data().each(function (group, i) {
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