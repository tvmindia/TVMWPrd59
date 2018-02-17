var DataTables = {};
var EmptyGuid = "00000000-0000-0000-0000-000000000000";
$(document).ready(function () {
    debugger;
    try {
        $("#SupplierID").select2({
        });
        $("#Status").select2({
        });
        BindOrReloadPurchaseOrderTable('Init');
    }
    catch (e) {
        console.log(e.message);
    }
});
//bind purchae order list
function BindOrReloadPurchaseOrderTable(action) {
    try{
        //creating advancesearch object
        debugger;
    PurchaseOrderAdvanceSearchViewModel = new Object();
    DataTablePagingViewModel = new Object();
    SupplierViewModel = new Object();
    DataTablePagingViewModel.Length = 0;
    //switch case to check the operation
    switch (action) {
        case 'Reset':
            $('#SearchTerm').val('');
            $('#FromDate').val('');
            $('#ToDate').val('');
            $('#SupplierID').val('');
            $('#Status').val('');
            break;
        case 'Init':
            break;
        case 'Apply':
            break;
        case 'Export':
            DataTablePagingViewModel.Length = -1;
            break;
        default:
            break;
    }
    PurchaseOrderAdvanceSearchViewModel.DataTablePaging = DataTablePagingViewModel;
    PurchaseOrderAdvanceSearchViewModel.SearchTerm = $('#SearchTerm').val();
    PurchaseOrderAdvanceSearchViewModel.FromDate = $('#FromDate').val();
    PurchaseOrderAdvanceSearchViewModel.ToDate = $('#ToDate').val();
    SupplierViewModel.ID = $('#SupplierID').val();
    PurchaseOrderAdvanceSearchViewModel.Supplier = SupplierViewModel;
    PurchaseOrderAdvanceSearchViewModel.Status = $('#Status').val();
    DataTables.PurchaseOrderList = $('#tblPurchaseOrder').DataTable(
        {
            dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
            buttons: [{
                extend: 'excel',
                exportOptions:
                             {
                                 columns: [1,2,3,4,5,6]
                             }
            }],
            order:false,
            searching: false,
            paging: true,
            lengthChange: false,
            proccessing: true,
            serverSide: true,
            ajax: {
                url: "GetAllPurchaseOrder/",
                data: { "purchaseOrderAdvanceSearchVM": PurchaseOrderAdvanceSearchViewModel },
                type: 'POST'
            },
            pageLength: 10,
            columns: [
                { "data": "ID", "defaultContent": "<i>-</i>" },
                { "data": "PurchaseOrderNo", "defaultContent": "<i>-</i>" },
                { "data": "PurchaseOrderDateFormatted", "defaultContent": "<i>-</i>" },
                { "data": "PurchaseOrderIssuedDateFormatted", "defaultContent": "<i>-</i>" },
                { "data": "Supplier", "defaultContent": "<i>-</i>" },
                { "data": "PurchaseOrderStatus", "defaultContent": "<i>-</i>" },
                { "data": "PurchaseOrderTitle", "defaultContent": "<i>-</i>" }
            ],
            columnDefs: [{ "targets": [0], "visible": false, "searchable": false },
                { className: "text-left", "targets": [1,4,5,6] },
                { className: "text-center", "targets": [2,3] }],           
            destroy: true,
            //for performing the import operation after the data loaded
            initComplete: function (settings, json) {
                if (action === 'Export')
                {
                    $(".buttons-excel").trigger('click');
                    ResetPurchaseOrderList();
                }
            }
        });
        $(".buttons-excel").hide();
    }
    catch(e)
    {
        console.log(e.message);
    }    
}

//function reset the list to initial
function ResetPurchaseOrderList() {
    BindOrReloadPurchaseOrderTable('Reset');
}
//function export data to excel
function ImportPurchaseOrderData() {
    BindOrReloadPurchaseOrderTable('Export');
}
