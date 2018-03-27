
//*****************************************************************************
//*****************************************************************************
//Author: Jais
//CreatedDate: 17-Feb-2018 
//LastModified: 5-Mar-2018 
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
        UnitViewModel = new Object();
        DataTablePagingViewModel.Length = 0;
        //switch case to check the operation
        switch (action) {
            case 'Reset':
                $('#SearchTerm').val('');
                $('#UnitCode').val('');
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
        ProductAdvanceSearchViewModel.DataTablePaging = DataTablePagingViewModel;
        UnitViewModel.Code = $('#UnitCode').val();
        ProductAdvanceSearchViewModel.Unit = UnitViewModel;
        ProductAdvanceSearchViewModel.SearchTerm = $('#SearchTerm').val();

        //apply datatable plugin on Raw Material table
        DataTables.productList = $('#tblProduct').DataTable(
        {
            dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
            buttons: [{
                extend: 'excel',
                exportOptions:
                             {
                                 columns: [0,1, 2, 3, 4, 5,6,7,8,9,10,11,12,13]
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
                url: "Product/GetAllProduct/",
                data: { "productAdvanceSearchVM": ProductAdvanceSearchViewModel },
                type: 'POST'
            },
            pageLength: 10,
            columns: [
            { "data": "Code", "defaultContent": "<i>-</i>" ,"width":"5%"},
            {
                "data": "Name", "defaultContent": "<i>-</i>", "width": "10%",
             'render': function (data, type, row) {
            if (row.IsInvoiceInKG)
                return data +'</br>(<b>Invoice in Kg </b>)'
            else
                return data
        }},
            { "data": "Description", "defaultContent": "<i>-<i>", "width": "10%" },
            { "data": "Unit.Description", "defaultContent": "<i>-<i>", "width": "5%" },
            { "data": "Category", "defaultContent": "<i>-<i>", "width": "5%" },
            { "data": "Type", "defaultContent": "<i>-<i>", "width": "5%" },
            {
                 "data": "Rate", render: function (data, type, row) {
                     if (data == 0)
                         return '-'
                     else
                         return roundoff(data, 1);
                 }, "defaultContent": "<i>-</i>", "width": "5%"
            },
            { "data": "CurrentStock", "defaultContent": "<i>-<i>", "width": "5%" },
            { "data": "HSNNo", "defaultContent": "<i>-<i>", "width": "5%" },
            { "data": "WeightInKG", "defaultContent": "<i>-<i>", "width": "5%" },
            { "data": "CostPrice", "defaultContent": "<i>-<i>", "width": "5%" },
            { "data": "SellingPrice", "defaultContent": "<i>-<i>", "width": "5%" },
            { "data": "SellingPriceInKG", "defaultContent": "<i>-<i>", "width": "5%" },
            { "data": "SellingPricePerPiece", "defaultContent": "<i>-<i>", "width": "5%" },
            { "data": null, "orderable": false, "defaultContent": '<a href="#" onclick="DeleteProductMaster(this)"<i class="glyphicon glyphicon-trash" aria-hidden="true"></i></a>  <a href="#" onclick="EditProductMaster(this)"<i class="glyphicon glyphicon-share-alt" aria-hidden="true"></i></a>', "width": "4%" }
            ],
            columnDefs: [{ "targets": [12,13], "visible": false, "searchable": false },
                { className: "text-right", "targets": [6,7,8,9,10,11,12,13] },
                { className: "text-left", "targets": [0,1,2, 3, 4,5,8] },
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
                    BindOrReloadProductTable('Search');
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
//--edit Product--//
function EditProductMaster(this_obj) {
    ProductViewModel = DataTables.productList.row($(this_obj).parents('tr')).data();
    GetMasterPartial("Product", ProductViewModel.ID);
    $('#h3ModelMasterContextLabel').text('Edit Product')
    $('#divModelMasterPopUp').modal('show');
}


//--Function To Confirm Product Deletion 
function DeleteProductMaster(this_obj) {
    debugger;
    productVM = DataTables.productList.row($(this_obj).parents('tr')).data();
    notyConfirm('Are you sure to delete?', 'DeleteProduct("' + productVM.ID + '")');
}

//--Function To Delete Product
function DeleteProduct(id) {
    debugger;
    try {
        if (id) {
            var data = { "id": id };
            var jsonData = {};
            var message = "";
            var status = "";
            var result = "";
            jsonData = GetDataFromServer("Product/DeleteProduct/", data);
            if (jsonData != '') {
                jsonData = JSON.parse(jsonData);
                message = jsonData.Message;
                status = jsonData.Status;
                result = jsonData.Record;
            }
            switch (status) {
                case "OK":
                    notyAlert('success', result.Message);
                    BindOrReloadProductTable('Reset');
                    break;
                case "ERROR":
                    notyAlert('error', message);
                    break;
                default:
                    break;
            }
        }
    }
    catch (e) {
        //this will show the error msg in the browser console(F12) 
        console.log(e.message);
    }
}

