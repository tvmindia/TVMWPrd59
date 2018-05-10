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
        $("#SupplierID,#PurchaseOrderID,#ReceivedBy").select2({});
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
                $('#ReceivedBy').val('').trigger('change');
                $('#SupplierID').val('').trigger('change');
                $('#PurchaseOrderID').val('').trigger('change');
                $('#ToDate').val('');
                $('#FromDate').val('');
                break;
            case 'Init':
                break;
            case 'Apply':
                break;
            case 'Export':
                DataTablePagingViewModel.Length = - 1;
                break;
            default:
                break;
        }

        MaterialReceiptAdvanceSearchViewModel.DataTablePaging = DataTablePagingViewModel;
        MaterialReceiptAdvanceSearchViewModel.SearchTerm = $('#SearchTerm').val();
        MaterialReceiptAdvanceSearchViewModel.FromDate = $('#FromDate').val();
        MaterialReceiptAdvanceSearchViewModel.ToDate = $('#ToDate').val();
        MaterialReceiptAdvanceSearchViewModel.SupplierID = $('#SupplierID').val();
        MaterialReceiptAdvanceSearchViewModel.PurchaseOrderID = $('#PurchaseOrderID').val();
        MaterialReceiptAdvanceSearchViewModel.ReceivedBy = $('#ReceivedBy').val();
        try {

        } catch (e) {
            console.log(e.message)
        }
        DataTables.MaterialReceiptList = $('#tblMaterialReceipt').DataTable(
        {
            dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
            buttons: [{
                extend: 'excel',
                exportOptions:
                             {
                                 columns: [1, 2, 3, 4, 5, 6]
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
                url: "GetAllMaterialReceipt/",
                data: { "MaterialReceiptAdvanceSearchVM": MaterialReceiptAdvanceSearchViewModel },
                type: 'POST'
            },
            pageLength: 10,
            columns: [
                { "data": "ID", "defaultContent": "<i>-</i>" },
                { "data": "ReceiptNo", "defaultContent": "<i>-</i>", "width": "10%" },
                { "data": "ReceiptDateFormatted", "defaultContent": "<i>-</i>", "width": "15%" },
                { "data": "PurchaseOrderNo", "defaultContent": "<i>-</i>", "width": "12%" },
                { "data": "Supplier.CompanyName", "defaultContent": "<i>-</i>", "width": "20%" },
                { "data": "Employee.Name", "defaultContent": "<i>-</i>", "width": "20%" },
                { "data": "GeneralNotes", "defaultContent": "<i>-</i>", "width": "20%" },
                {
                    "data": "ID", "orderable": false, render: function (data, type, row) {
                        return '<a href="/MaterialReceipt/NewMaterialReceipt?code=STR&id=' + data + '" class="actionLink" ><i class="glyphicon glyphicon-share-alt" aria-hidden="true"></i></a>'
                    }, "defaultContent": "<i>-</i>", "width": "3%"
                }
            ],
            columnDefs: [{ "targets": [0], "visible": false, "searchable": false },
                { className: "text-right", "targets": [] },
                { className: "text-left", "targets": [1, 3, 3, 4, 5, 6] },
                { className: "text-center", "targets": [2, 7] }],
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
                    BindOrReloadMaterialReceiptTable('Apply');
                }
            }
        });
        $(".buttons-excel").hide();

        $('#tblMaterialReceipt tbody').on('dblclick', 'td', function () {
            Edit(this);
        });
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


function Edit(curobj) {
    debugger;
    var MaterialReceiptViewModel = DataTables.MaterialReceiptList.row($(curobj).parents('tr')).data();
    window.location.replace("/MaterialReceipt/NewMaterialReceipt?code=STR&id=" + MaterialReceiptViewModel.ID);
}