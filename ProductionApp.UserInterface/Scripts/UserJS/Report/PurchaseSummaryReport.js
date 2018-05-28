//*****************************************************************************
//*****************************************************************************
//Author:Sruthi
//CreatedDate: 07-May-2018 
//LastModified: 08-May-2018 
//FileName: PurchaseSummaryReport.js
//Description: Client side coding for Purchase Summary Report
//******************************************************************************
//******************************************************************************
//Global Declaration
var DataTables = {};
var EmptyGuid = "00000000-0000-0000-0000-000000000000";
$(document).ready(function () {
    debugger;
    try {
        $("#Supplier").select2({
        });
        BindOrReloadPurchaseTable('Init');

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

//bind purchase summary report
function 
    BindOrReloadPurchaseTable(action) {
    try {
        //creating advancesearch object       
        PurchaseSummaryReportViewModel = new Object();
        DataTablePagingViewModel = new Object();
        DataTablePagingViewModel.Length = 0;


        //switch case to check the operation
        switch (action) {
            case 'Reset':
                $('#SearchTerm').val('');
                $('#FromDate').val('');
                $('#ToDate').val('');
                $('#Status').val('');
                $("#Supplier").val('').select2();
                $('#DateFilter').val('');
                $('#EmailedYN').val('');
                break;
            case 'Init':
                break;
            case 'Search':
                break;
            case 'Apply':
                PurchaseSummaryReportViewModel.FromDate = $('#FromDate').val();
                PurchaseSummaryReportViewModel.ToDate = $('#ToDate').val();
                PurchaseSummaryReportViewModel.Status = $('#Status').val();
                PurchaseSummaryReportViewModel.SupplierID = $('#Supplier').val();
                PurchaseSummaryReportViewModel.DateFilter = $('#DateFilter').val();
                PurchaseSummaryReportViewModel.EmailedYN = $('#EmailedYN').val();
                break;
            case 'Export':
                DataTablePagingViewModel.Length = -1;
                break;
            default:
                break;
        }
        PurchaseSummaryReportViewModel.DataTablePaging = DataTablePagingViewModel;
        PurchaseSummaryReportViewModel.SearchTerm = $('#SearchTerm').val();
        PurchaseSummaryReportViewModel.FromDate = $('#FromDate').val();
        PurchaseSummaryReportViewModel.ToDate = $('#ToDate').val();
        PurchaseSummaryReportViewModel.Status = $('#Status').val();
        PurchaseSummaryReportViewModel.SupplierID = $('#Supplier').val();
        PurchaseSummaryReportViewModel.DateFilter = $('#DateFilter').val();
        PurchaseSummaryReportViewModel.EmailedYN = $('#EmailedYN').val();
        DataTables.PurchaseOrderList = $('#tblPurchaseSummaryReport').DataTable(

            {

                dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
                buttons: [{
                    extend: 'excel',
                    exportOptions:
                                 {
                                     columns: [1, 2, 3, 4, 5, 6, 7,8]
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
                    url: "GetPurchaseSummaryReport/",
                    data: { "purchaseSummaryVM": PurchaseSummaryReportViewModel },
                    type: 'POST'
                },
                pageLength: 15,
                columns: [
                    { "data": "ID", "defaultContent": "<i>-</i>" },
                    { "data": "PurchaseOrderNo", "defaultContent": "<i>-</i>" },
                    { "data": "PurchaseOrderDateFormatted", "defaultContent": "<i>-</i>" },
                     { "data": "PurchaseOrderIssuedDateFormatted", "defaultContent": "<i>-</i>" },
                     { "data": "PurchaseOrderTitle", "defaultContent": "<i>-</i>" },
                    { "data": "Supplier", "defaultContent": "<i>-</i>" },
                    { "data": "PurchaseOrderStatus", "defaultContent": "<i>-</i>" },
                    { "data": "EmailSentYN", "defaultContent": "<i>-</i>" },
                    { "data": "GrossAmount", "defaultContent": "<i>-</i>" } 
                   
                   

                ],
                columnDefs: [{ "targets": [0], "visible": false, "searchable": false },
                    { className: "text-left", "targets": [1, 4,5] },
                    { className: "text-center", "targets": [2,3, 6,7] },
                    { className: "text-right", "targets": [8] },
                    { width: "10%", "targets": [1] },
                    { width: "15%", "targets": [3] },
                    { width: "15%", "targets": [4] },
                    {
                        targets: [7],
                        render: function (data, type, row) {
                            switch (data) {
                                case 'True': return 'Yes'; break;
                                case 'False': return 'No'; break;
                                default: return '-';
                            }
                        }
                    }
                   
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
                        BindOrReloadPurchaseTable('Search');
                    }
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
    BindOrReloadPurchaseTable('Reset');
}

//function export data to excel
function ExportReportData() {
    BindOrReloadPurchaseTable('Export');
}

//function to filter data based on date
function DateFilterOnchange() {
    BindOrReloadPurchaseTable('Apply');
}