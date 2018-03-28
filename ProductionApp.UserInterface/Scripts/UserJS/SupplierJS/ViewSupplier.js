
//*****************************************************************************
//*****************************************************************************
//Author: Jais
//CreatedDate:27-Mar-2018 
//LastModified: 27-Mar-2018 
//FileName: ViewSupplier.js
//Description: Client side coding for ViewSupplier
//******************************************************************************
//******************************************************************************

//--Global Declaration--//
var _dataTables = {};
var _emptyGuid = "00000000-0000-0000-0000-000000000000";

$(document).ready(function () {
    debugger;
    try {

        BindOrReloadSupplierTable('Init');

        $('#tblSupplier tbody').on('dblclick', 'td', function () {
            Edit(this);
        });
    }
    catch (e) {
        console.log(e.message);
    }
});


//--function bind the Supplier list checking search and filter--//
function BindOrReloadSupplierTable(action) {
    try {
        debugger;
        //creating advancesearch object
        SupplierViewModel = new Object();
        DataTablePagingViewModel = new Object();
        SupplierAdvanceSearchViewModel = new Object();
        DataTablePagingViewModel.Length = 0;
        //switch case to check the operation
        switch (action) {
            case 'Reset':
                $('#SearchTerm').val('');
                break;
            case 'Init':
                break;
            case 'Search':
                break;
            case 'Export':
                DataTablePagingViewModel.Length = -1;
                break;
            default:
                break;
        }
        SupplierAdvanceSearchViewModel.DataTablePaging = DataTablePagingViewModel;
        SupplierAdvanceSearchViewModel.SearchTerm = $('#SearchTerm').val();

        //apply datatable plugin on Supplier table
        _dataTables.supplierList = $('#tblSupplier').DataTable(
        {
            dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
            buttons: [{
                extend: 'excel',
                exportOptions:
                             {
                                 columns: [0,1, 2, 3, 4, 5]
                             }
            }],
            ordering: false,
            searching: false,
            paging: true,
            lengthChange: false,
            processing: true,
            language: {

                "processing": "<div class='spinner'><div class='bounce1'></div><div class='bounce2'></div><div class='bounce3'></div></div>"
            },
            serverSide: true,
            ajax: {
                url: "GetAllSupplier/",
                data: { "supplierAdvanceSearchVM": SupplierAdvanceSearchViewModel },
                type: 'POST'
            },
            pageLength: 10,
            columns: [
            { "data": "CompanyName", "defaultContent": "<i>-</i>" },
            { "data": "ContactPerson", "defaultContent": "<i>-<i>","width":"10%" },
            { "data": "Product", "defaultContent": "<i>-<i>" },
            { "data": "Mobile", "defaultContent": "<i>-<i>" },
            { "data": "TaxRegNo", "defaultContent": "<i>-<i>" },
            { "data": "PANNo", "defaultContent": "<i>-<i>", "width": "7%" },
             {
                 "data": "ID", "orderable": false, render: function (data, type, row) {
                     return '<a href="/Supplier/NewSupplier?code=MSTR&ID=' + data + '" class="actionLink" ><i class="glyphicon glyphicon-share-alt" aria-hidden="true"></i></a>'
                 }, "defaultContent": "<i>-</i>", "width": "3%"
             }
            ],
            columnDefs: [{ "targets": [], "visible": false, "searchable": false },
                { className: "text-right", "targets": [] },
                { className: "text-left", "targets": [0, 1, 2, 3, 4, 5,6] },
                { className: "text-center", "targets": [] }],
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
                    BindOrReloadSupplierTable('Search');
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
function ResetSupplierList() {
    BindOrReloadSupplierTable('Reset');
}
//function export data to excel
function ImportSupplierData() {
    BindOrReloadSupplierTable('Export');
}