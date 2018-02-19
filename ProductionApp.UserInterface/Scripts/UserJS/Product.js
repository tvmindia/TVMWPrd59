
//*****************************************************************************
//*****************************************************************************
//Author: Jais
//CreatedDate: 17-Feb-2018 
//LastModified: 19-Feb-2018 
//FileName: Product.js
//Description: Client side coding for Product
//******************************************************************************
//******************************************************************************

//--Global Declaration--//
var DataTables = {};
var EmptyGuid = "00000000-0000-0000-0000-000000000000";

//--Loading DOM--//
$(document).ready(function () {
    try {
        debugger;
        BindOrReloadProductTable('Init');
    }
    catch (e) {
        console.log(e.message);
    }
});

//--function bind the Product list checking search and filter--//
function BindOrReloadProductTable(action) {
    try {
        debugger;
        //creating advancesearch object
        ProductAdvanceSearchViewModel = new Object();
        DataTablePagingViewModel = new Object();
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
                if ($('#SearchTerm').val() == "")
                    DataTablePagingViewModel.Length = -1;
                break;
            default:
                break;
        }
        ProductAdvanceSearchViewModel.DataTablePaging = DataTablePagingViewModel;
        ProductAdvanceSearchViewModel.SearchTerm = $('#SearchTerm').val();

        //apply datatable plugin on Raw Material table
        DataTables.productList = $('#tblProduct').DataTable(
        {
            dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
            buttons: [{
                extend: 'excel',
                exportOptions:
                             {
                                 columns: [1, 2, 3, 4, 5, 6,7]
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
                url: "Product/GetAllProduct/",
                data: { "productAdvanceSearchVM": ProductAdvanceSearchViewModel },
                type: 'POST'
            },
            pageLength: 10,
            columns: [
            { "data": "ID", "defaultContent": "<i>-</i>" },
            { "data": "Code", "defaultContent": "<i>-</i>" },
            { "data": "Name", "defaultContent": "<i>-</i>" },
            { "data": "Description", "defaultContent": "<i>-<i>" },
            { "data": "Unit.Description", "defaultContent": "<i>-<i>" },
            { "data": "Category", "defaultContent": "<i>-<i>" },
            { "data": "Rate", "defaultContent": "<i>-<i>" },
            { "data": "CurrentStock", "defaultContent": "<i>-<i>" },
            { "data": null, "orderable": false, "defaultContent": '<a href="#" onclick="EditProductMaster(this)"<i class="glyphicon glyphicon-share-alt" aria-hidden="true"></i></a>' }
            ],
            columnDefs: [{ "targets": [0], "visible": false, "searchable": false },
                { className: "text-right", "targets": [6,7] },
                { className: "text-left", "targets": [1,2, 3, 4, 5] },
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
                    ResetProductList();
                }
            }
        });
        $(".buttons-excel").hide();
    }
    catch (e) {
        console.log(e.message);
    }
}

//--function reset the list to initial--//
function ResetProductList() {
    BindOrReloadProductTable('Reset');
}

//--function export data to excel--//
function ImportProductData() {
    BindOrReloadProductTable('Export');
}

//-- add Product--//
function AddProductMaster() {
    GetMasterPartial("Product", "");
    $('#h3ModelMasterContextLabel').text('Add Product')
    $('#divModelMasterPopUp').modal('show');
}

//--edit Product--//
function EditProductMaster(this_obj) {
    rowData = DataTables.productList.row($(this_obj).parents('tr')).data();
    GetMasterPartial("Product", rowData.ID);
    $('#h3ModelMasterContextLabel').text('Edit Product')
    $('#divModelMasterPopUp').modal('show');
}

//-- Function After Save --//
function SaveSuccessProduct(data, status) {
    debugger;
    var JsonResult = JSON.parse(data)
    switch (JsonResult.Result) {
        case "OK":
            $('#IsUpdate').val('True');
            $('#ID').val(JsonResult.Records.ID);
            BindOrReloadProductTable('Reset');
            MasterAlert("success", JsonResult.Records.Message)
            break;
        case "ERROR":
            MasterAlert("danger", JsonResult.Message)
            break;
        default:
            MasterAlert("danger", JsonResult.Message)
            break;
    }
}
