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
        $('#SearchTerm').focus();
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
        var SearchValue = $('#hdnSearchTerm').val();
        var SearchTerm = $('#SearchTerm').val();
        $('#hdnSearchTerm').val($('#SearchTerm').val());

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
                PurchaseRegisterReportViewModel.DataTablePaging = DataTablePagingViewModel;
                PurchaseRegisterReportViewModel.SearchTerm = $('#SearchTerm').val() == "" ? null : $('#SearchTerm').val();
                PurchaseRegisterReportViewModel.FromDate = $('#FromDate').val() == "" ? null : $('#FromDate').val();
                PurchaseRegisterReportViewModel.ToDate = $('#ToDate').val() == "" ? null : $('#ToDate').val();
                PurchaseRegisterReportViewModel.InvoiceStatus = $('#InvoiceStatus').val() == "" ? null : $('#InvoiceStatus').val();
                PurchaseRegisterReportViewModel.PaymentStatus = $('#PaymentStatus').val() == "" ? null : $('#PaymentStatus').val();
                PurchaseRegisterReportViewModel.SupplierID = $('#Supplier').val() == "" ? EmptyGuid : $('#Supplier').val();             
                PurchaseRegisterReportViewModel.DateFilter = $('#DateFilter').val() == "" ? null : $('#DateFilter').val();
                PurchaseRegisterReportViewModel.Status = $('#Status').val() == "" ? null : $('#Status').val();                
                $('#AdvanceSearch').val(JSON.stringify(PurchaseRegisterReportViewModel));
                $('#FormExcelExport').submit();
                return true;
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
                    //{ "data": "ID", "defaultContent": "<i>-</i>" },
                    { "data": "PurchaseOrderNo", "defaultContent": "<i>-</i>" },
                    { "data": "PurchaseOrderDateFormatted", "defaultContent": "<i>-</i>" },
                    { "data": "Supplier.CompanyName", "defaultContent": "<i>-</i>" },
                    { "data": "TaxableAmount", "defaultContent": "<i>-</i>" },
                    { "data": "GSTAmt", "defaultContent": "<i>-</i>" },
                    { "data": "GrossAmount", "defaultContent": "<i>-</i>" },
                    { "data": "Discount", "defaultContent": "<i>-</i>" },
                    //{ "data": "GSTPerc", "defaultContent": "<i>-</i>" },                  
                    { "data": "NetAmount", "defaultContent": "<i>-</i>" },                
                                                          
                    { "data": "InvoicedAmount", "defaultContent": "<i>-</i>" },
                    { "data": "PaidAmount", "defaultContent": "<i>-</i>" }             
                                        
                ],
                columnDefs: [
                    { className: "text-left", "targets": [0,2]},
                    { className: "text-center", "targets": [1] },
                    { className: "text-right", "targets": [3,4,5,6,7,8,9] }
                    
                ],

                destroy: true,
                //for performing the import operation after the data loaded            

                initComplete: function (settings, json) {
                    debugger;
                    $('.dataTables_wrapper div.bottom div').addClass('col-md-6');
                    $('#tblPurchaseRegisterReport').fadeIn('slow');
                    if (action == undefined) {
                        $('.excelExport').hide();
                        OnServerCallComplete();
                    }

                },
            });      
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