﻿//*****************************************************************************
//*****************************************************************************
//Author:Sruthi
//CreatedDate: 08-May-2018 
//LastModified: 08-May-2018 
//FileName: PurchaseDetailReport.js
//Description: Client side coding for Purchase Detail Report
//******************************************************************************
//******************************************************************************

//Global Declaration
var DataTables = {};
var EmptyGuid = "00000000-0000-0000-0000-000000000000";
$(document).ready(function () {
    debugger;
    try {
       $("#Supplier,#Material").select2({
       });
       $('#SearchTerm').focus();
        BindOrReloadPurchaseDetailTable('Init');

    }
    catch (e) {
        console.log(e.message);
    }
    //$("#Material").select2({
    //});
});

//Click function for search
function RedirectSearchClick(e, this_obj) {
    debugger;
    var unicode = e.charCode ? e.charCode : e.keyCode
    if (unicode == 13) {
        $(this_obj).closest('.input-group').find('button').trigger('click')
    }
}

//bind purchase detail report
function
    BindOrReloadPurchaseDetailTable(action) {
    try {
        //creating advancesearch object       
        PurchaseDetailReportViewModel = new Object();
        DataTablePagingViewModel = new Object();
        DataTablePagingViewModel.Length = 0;
        var SearchValue = $('#hdnSearchTerm').val();
        var SearchTerm = $('#SearchTerm').val();
        $('#hdnSearchTerm').val($('#SearchTerm').val());

        //switch case to check the operation
        switch (action) {
            case 'Reset':
                debugger;
                $('#SearchTerm').val('');
                $('#FromDate').val('');
            
                $('#ToDate').val('');
                $('#POStatus').val('');
                $("#Supplier").val('').select2();
                $('#DateFilter').val('');
                $('#DelStatus').val('');
                $('#ApprovalStatus').val('');
                $('#Material').val('').select2();
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
                PurchaseDetailReportViewModel.FromDate = $('#FromDate').val();
                PurchaseDetailReportViewModel.ToDate = $('#ToDate').val();
                PurchaseDetailReportViewModel.Status = $('#POStatus').val();
                PurchaseDetailReportViewModel.SupplierID = $('#Supplier').val();
                PurchaseDetailReportViewModel.DateFilter = $('#DateFilter').val();
                PurchaseDetailReportViewModel.MaterialID = $('#Material').val();
              
                PurchaseDetailReportViewModel.DeliveryStatus = $('#DelStatus').val();
                PurchaseDetailReportViewModel.ApprovalStatus = $('#ApprovalStatus').val();
                break;
            case 'Export':
                DataTablePagingViewModel.Length = -1;
                PurchaseDetailReportViewModel.DataTablePaging = DataTablePagingViewModel;
                PurchaseDetailReportViewModel.SearchTerm = $('#SearchTerm').val() == "" ? null : $('#SearchTerm').val();
                PurchaseDetailReportViewModel.FromDate = $('#FromDate').val() == "" ? null : $('#FromDate').val();
                PurchaseDetailReportViewModel.ToDate = $('#ToDate').val() == "" ? null : $('#ToDate').val();
                PurchaseDetailReportViewModel.Status = $('#POStatus').val() == "" ? null : $('#POStatus').val();
                PurchaseDetailReportViewModel.SupplierID = $('#Supplier').val() == "" ? EmptyGuid : $('#Supplier').val();
                PurchaseDetailReportViewModel.MaterialID = $('#Material').val() == "" ? EmptyGuid : $('#Material').val();
                PurchaseDetailReportViewModel.DateFilter = $('#DateFilter').val() == "" ? null : $('#DateFilter').val();
                PurchaseDetailReportViewModel.DeliveryStatus = $('#DelStatus').val() == "" ? null : $('#DelStatus').val();
                PurchaseDetailReportViewModel.ApprovalStatus = $('#ApprovalStatus').val() == "" ? null : $('#ApprovalStatus').val();
                $('#AdvanceSearch').val(JSON.stringify(PurchaseDetailReportViewModel));
                $('#FormExcelExport').submit();
                return true;
                break;
            default:
                break;
        }
        PurchaseDetailReportViewModel.DataTablePaging = DataTablePagingViewModel;
        PurchaseDetailReportViewModel.SearchTerm = $('#SearchTerm').val();
        PurchaseDetailReportViewModel.FromDate = $('#FromDate').val();
        PurchaseDetailReportViewModel.ToDate = $('#ToDate').val();
        PurchaseDetailReportViewModel.Status = $('#POStatus').val();
        PurchaseDetailReportViewModel.SupplierID = $('#Supplier').val();
        PurchaseDetailReportViewModel.DateFilter = $('#DateFilter').val();
        PurchaseDetailReportViewModel.MaterialID = $('#Material').val();       
        PurchaseDetailReportViewModel.DeliveryStatus = $('#DelStatus').val();
        PurchaseDetailReportViewModel.ApprovalStatus = $('#ApprovalStatus').val();
        DataTables.PurchaseOrderList = $('#tblPurchaseDetailReport').DataTable(

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
                    url: "GetPurchaseDetailReport/",
                    data: { "purchaseDetailVM": PurchaseDetailReportViewModel },
                    type: 'POST'
                },
                pageLength: 15,
                columns: [
                   // { "data": "ID", "defaultContent": "<i>-</i>" },
                    { "data": "PurchaseOrderNo", "defaultContent": "<i>-</i>" },
                   // { "data": "Title", "defaultContent": "<i>-</i>" },
                    { "data": "PurchaseOrderDateFormatted", "defaultContent": "<i>-</i>" },
                    { "data": "Status", "defaultContent": "<i>-</i>" },
                    { "data": "Supplier.CompanyName", "defaultContent": "<i>-</i>" },
                    { "data": "Material.Description", "defaultContent": "<i>-</i>" },
                    { "data": "Material.UnitCode", "defaultContent": "<i>-</i>" },                   
                    { "data": "POQty", "defaultContent": "<i>-</i>" },
                    { "data": "PrevRcvQty", "defaultContent": "<i>-</i>" },
                    { "data": "ApprovalStatus", "defaultContent": "<i>-</i>" },
                    {"data" : "DeliveryStatus" ,"defaultContent": "<i>-</i>" }
                ],
                columnDefs: [{ "targets": [1, 2, 3, 8,9], "visible": false, "searchable": false },
                    { className: "text-left", "targets": [0, 4, 5] },
                    { className: "text-center", "targets": [6,7] },
                   //// { className: "text-right", "targets": [10] }
                    { width: "20%", "targets": [1] },
                    { width: "20%", "targets": [5] },
                    { width: "20%", "targets": [6] },
                    { width: "20%", "targets": [7] }
                    //{ width: "15%", "targets": [8] }
                   // { width: "15%", "targets": [9] },
                   // { width: "15%", "targets": [10] }
                ],

                destroy: true,
                //for performing the import operation after the data loaded              
                initComplete: function (settings, json) {
                    debugger;
                    $('.dataTables_wrapper div.bottom div').addClass('col-md-6');
                    $('#tblPurchaseDetailReport').fadeIn('slow');
                    if (action == undefined) {
                        $('.excelExport').hide();
                        OnServerCallComplete();
                    }

                },
                // grouping with multiple columns
                drawCallback: function (settings) {
                    var api = this.api();
                    var rows = api.rows({ page: 'current' }).nodes();
                    var last = null;

                    api.column(0, { page: 'current' }).data().each(function (group, i) {
                        if (last !== group) {
                            debugger;
                            var rowData = api.row(i).data();
                            $(rows).eq(i).before('<tr class="group "><td colspan="7" class="rptGrp">' + '<b>PO No</b> : ' + group + "&nbsp;&nbsp;&nbsp;" + "&nbsp;&nbsp;&nbsp;" + '<b>PO Date</b> : ' + rowData.PurchaseOrderDateFormatted + "&nbsp;&nbsp;&nbsp;" + '<b>PO Status</b> : ' + rowData.Status + '&nbsp;&nbsp;&nbsp;' + ' <b>Supplier</b> : ' + rowData.Supplier.CompanyName + "&nbsp;&nbsp;&nbsp;" + ' <b>Approval Status</b> : ' + rowData.ApprovalStatus + "&nbsp;&nbsp;&nbsp;" + ' <b>Delivery Status</b> : ' + rowData.DeliveryStatus + '</td></tr>');
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
    BindOrReloadPurchaseDetailTable('Reset');
}
//function export data to excel
function ExportReportData() {
    BindOrReloadPurchaseDetailTable('Export');
}
//function to filter data based on date
function DateFilterOnchange() {
    BindOrReloadPurchaseDetailTable('Apply');
}