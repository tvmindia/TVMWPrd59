//*****************************************************************************
//*****************************************************************************
//Author:Sruthi
//CreatedDate: 10-May-2018 
//LastModified: 11-May-2018 
//FileName: PurchaseRegisterReport.js
//Description: Client side coding for Purchase Register Report
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
        BindOrReloadPurchaseRegisterTable('Init');

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

//bind purchase register report
function
    BindOrReloadPurchaseRegisterTable(action) {
    try {
        //creating advancesearch object       
        PurchaseRegisterReportViewModel = new Object();
        DataTablePagingViewModel = new Object();
        DataTablePagingViewModel.Length = 0;


        //switch case to check the operation
        switch (action) {
            case 'Reset':
                $('#SearchTerm').val('');
                $('#FromDate').val('');
                $('#ToDate').val('');
                $('#InvoiceStatus').val('');
                $('#PaymentStatus').val('');
                $("#Supplier").val('').select2();               
                $('#DateFilter').val('');
                $('#Status').val('');
                break;
            case 'Init':
                break;
            case 'Search':
                break;
            case 'Apply':
                PurchaseRegisterReportViewModel.FromDate = $('#FromDate').val();
                PurchaseRegisterReportViewModel.ToDate = $('#ToDate').val();
                PurchaseRegisterReportViewModel.InvoiceStatus = $('#InvoiceStatus').val();
                PurchaseRegisterReportViewModel.PaymentStatus = $('#PaymentStatus').val();
                PurchaseRegisterReportViewModel.SupplierID = $('#Supplier').val();               
                PurchaseRegisterReportViewModel.DateFilter = $('#DateFilter').val();
                PurchaseRegisterReportViewModel.Status = $('#Status').val();
                break;
            case 'Export':
                DataTablePagingViewModel.Length = -1;
                break;
            default:
                break;
        }
        PurchaseRegisterReportViewModel.DataTablePaging = DataTablePagingViewModel;
        PurchaseRegisterReportViewModel.SearchTerm = $('#SearchTerm').val();
        PurchaseRegisterReportViewModel.FromDate = $('#FromDate').val();
        PurchaseRegisterReportViewModel.ToDate = $('#ToDate').val();
        PurchaseRegisterReportViewModel.InvoiceStatus = $('#InvoiceStatus').val();
        PurchaseRegisterReportViewModel.PaymentStatus = $('#PaymentStatus').val();
        PurchaseRegisterReportViewModel.SupplierID = $('#Supplier').val();       
        PurchaseRegisterReportViewModel.DateFilter = $('#DateFilter').val();
        PurchaseRegisterReportViewModel.Status = $('#Status').val();
        DataTables.PurchaseOrderList = $('#tblPurchaseRegisterReport').DataTable(

            {

                dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
                buttons: [{
                    extend: 'excel',
                    exportOptions:
                                 {
                                     columns: [1, 2, 3, 4, 5, 6, 7, 8,9,10,11]
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
                    url: "GetPurchaseRegisterReport/",
                    data: { "purchaseRegisterVM": PurchaseRegisterReportViewModel },
                    type: 'POST'
                },
                pageLength: 15,
                columns: [
                    { "data": "ID", "defaultContent": "<i>-</i>" },
                    { "data": "PurchaseOrderNo", "defaultContent": "<i>-</i>" },
                    { "data": "PurchaseOrderDateFormatted", "defaultContent": "<i>-</i>" },
                    { "data": "Supplier.CompanyName", "defaultContent": "<i>-</i>" },                     
                    { "data": "Discount", "defaultContent": "<i>-</i>" },
                    { "data": "GSTPerc", "defaultContent": "<i>-</i>" },
                    { "data": "GSTAmt", "defaultContent": "<i>-</i>" },
                    { "data": "NetAmount", "defaultContent": "<i>-</i>" },
                    { "data": "TaxableAmount", "defaultContent": "<i>-</i>" },
                    { "data": "GrossAmount", "defaultContent": "<i>-</i>" },
                                       
                    { "data": "InvoicedAmount", "defaultContent": "<i>-</i>" },
                    { "data": "PaidAmount", "defaultContent": "<i>-</i>" }             
                                        
                ],
                columnDefs: [{ "targets": [0], "visible": false, "searchable": false },
                    { className: "text-left", "targets": [1,3]},
                    { className: "text-center", "targets": [2] },
                    { className: "text-right", "targets": [4,5,6,7,8,9,10,11] }
                    
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
                        BindOrReloadPurchaseRegisterTable('Search');
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
    BindOrReloadPurchaseRegisterTable('Reset');
}

//function export data to excel
function ExportReportData() {
    BindOrReloadPurchaseRegisterTable('Export');
}

//function to filter data based on date
function DateFilterOnchange() {
    BindOrReloadPurchaseRegisterTable('Apply');
}