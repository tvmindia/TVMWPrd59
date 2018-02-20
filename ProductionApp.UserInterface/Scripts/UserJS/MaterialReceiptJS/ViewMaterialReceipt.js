//*****************************************************************************
//*****************************************************************************
//Author: Arul
//CreatedDate: 19-Feb-2018 
//LastModified: 20-Feb-2018
//FileName: ViewMaterialReceipt.js
//Description: Client side coding for Material Receipts
//******************************************************************************
//******************************************************************************

//--Global Declaration--//
var DataTables = {};
var EmptyGuid = "00000000-0000-0000-0000-000000000000";
//-------------------------------------------------------------
$(document).ready(function () {
    debugger;
    try {
        $("#SupplierID").select2({
        });
        $("#PurchaseOrderID").select2({
        });
        BindOrReloadMaterialReceiptTable('Init');
    }
    catch (e) {
        console.log(e.message);
    }
});
//function bind the MaterialReceipt list checking search and filter
function BindOrReloadMaterialReceiptTable(action) {
    try {
        debugger;
        MaterialReceiptAdvanceSearchViewModel = new Object();
        DataTablePagingViewModel = new Object();
        PurchaseOrderViewModel = new Object();
        SupplierViewModel = new Object();
        DataTablePagingViewModel.Length = 0;
        switch (action) {
            case 'Reset':
                $('#SearchTerm').val('');
                $('#SupplierID').val('').select2();
                $('#PurchaseOrderID').val('').select2();
                $('#ToDate').val('');
                $('#FromDate').val('');
                break;
            case 'Init':
                break;
            case 'Apply':
                break;
            case 'Export':
                if ($('#SearchTerm').val() == "" && $('#SupplierID').val() == "" && $('#PurchaseOrderID').val() == "" && $('#ToDate').val() == "" && $('#FromDate').val() == "")
                    DataTablePagingViewModel.Length = -1;
                break;
            default:
                break;
        }

        MaterialReceiptAdvanceSearchViewModel.DataTablePaging = DataTablePagingViewModel;
        MaterialReceiptAdvanceSearchViewModel.SearchTerm = $('#SearchTerm').val();
        MaterialReceiptAdvanceSearchViewModel.FromDate = $('#FromDate').val();
        MaterialReceiptAdvanceSearchViewModel.ToDate = $('#ToDate').val();
        SupplierViewModel.ID = $('#SupplierID').val();
        MaterialReceiptAdvanceSearchViewModel.Supplier = SupplierViewModel;
        PurchaseOrderViewModel.ID = $('#PurchaseOrderID').val();
        MaterialReceiptAdvanceSearchViewModel.PurchaseOrder = PurchaseOrderViewModel;

        DataTables.BankList = $('#tblMaterialReceipt').DataTable(
        {
            dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
            buttons: [{
                extend: 'excel',
                exportOptions:
                             {
                                 columns: [1, 2, 3, 4, 5]
                             }
            }],
            order: false,
            searching: false,
            paging: true,
            lengthChange: false,
            processing: true,
            language: {
                "processing": "<div class='spinner'><div class='bounce1'></div><div class='bounce2'></div><div class='bounce3'></div></div>"
            },
            serverSide: true,
            ajax: {
                url: "MaterialReceipt/GetAllMaterialReceipt/",
                data: { "MaterialReceiptAdvanceSearchVM": MaterialReceiptAdvanceSearchViewModel },
                type: 'POST'
            },
            pageLength: 10,
            columns: [
                { "data": "ID", "defaultContent": "<i>-</i>" },
                { "data": "ReceiptNo", "defaultContent": "<i>-</i>" },
                { "data": "ReceiptDateFormatted", "defaultContent": "<i>-</i>" },
                { "data": "PurchaseOrderNo", "defaultContent": "<i>-</i>" },
                { "data": "Supplier.CompanyName", "defaultContent": "<i>-</i>" },
                { "data": "GeneralNotes", "defaultContent": "<i>-</i>" },
                { "data": null, "orderable": false, "defaultContent": '<a href="#" onclick="EditMaterialReceipt(this)"<i class="glyphicon glyphicon-share-alt" aria-hidden="true"></i></a>' }
            ],
            columnDefs: [{ "targets": [0], "visible": false, "searchable": false },
                { className: "text-right", "targets": [] },
                { className: "text-left", "targets": [1, 3, 3, 4] },
                { className: "text-center", "targets": [2] }],
            destroy: true,
            //for performing the import operation after the data loaded
            initComplete: function (settings, json) {
                debugger;
                if (action === 'Export') {
                    if (json.data.length > 0) {
                        if (json.data[0].TotalCount > 10000) {
                            MasterAlert("info", 'We are able to download maximum 10000 rows of data, There exist more than 10000 rows of data please filter and download')
                        }
                    }
                    $(".buttons-excel").trigger('click');
                    ResetMaterialReceipt();
                }
            }
        });
        $(".buttons-excel").hide();
    }
    catch (e) {
        console.log(e.message);
    }
}
//Reset Function of Material Receipts
function ResetMaterialReceipt(){
    BindOrReloadMaterialReceiptTable('Reset');
}
//Import table values into excel
function ImportMaterialReceipt() {
    BindOrReloadMaterialReceiptTable('Export');
}