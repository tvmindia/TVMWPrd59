
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
var _dataTable = {};
var _emptyGuid = "00000000-0000-0000-0000-000000000000";

//--Loading DOM--//
$(document).ready(function () {
    try {
        debugger;
        BindOrReloadProductTable('Init');
    }
    catch (e) {
        console.log(e.message);
    }
    $("#Unit_Code").select2({
    });
    $("#ProductCategory_Code").select2({
    });
    
});

//--function bind the Product list checking search and filter--//
function BindOrReloadProductTable(action) {
    try {
        debugger;
        //creating advancesearch object
        ProductAdvanceSearchViewModel = new Object();
        DataTablePagingViewModel = new Object();
        UnitViewModel = new Object();
        ProductCategoryViewModel = new Object();
        DataTablePagingViewModel.Length = 0;
        //switch case to check the operation
        switch (action) {
            case 'Reset':
                $('#SearchTerm').val('');
                $('#Unit_Code').val('').select2();
                $('#ProductCategory_Code').val('').select2();
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
        UnitViewModel.Code = $("#Unit_Code").val();
        ProductAdvanceSearchViewModel.Unit = UnitViewModel;
        ProductCategoryViewModel.Code = $("#ProductCategory_Code").val();
        ProductAdvanceSearchViewModel.ProductCategory = ProductCategoryViewModel;
        ProductAdvanceSearchViewModel.SearchTerm = $('#SearchTerm').val();
        debugger;
        //apply datatable plugin on Raw Material table
        _dataTable.productList = $('#tblProduct').DataTable(
        {
           
            dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
            buttons: [{
                extend: 'excel',
                exportOptions:
                             {
                                 columns: [0,1, 2, 3, 4, 5,6,7,8,9,10,11,12]
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
                    debugger;
                 if (row.IsInvoiceInKG) {
                     if(row.Type == "COM"){
                         return data + '</br>(<b>Invoice in Kg </b>)' + '</br>(<b>Component</b>)'
                     }
                     else{
                         return data + '</br>(<b>Invoice in Kg </b>)' + '</br>(<b>Product</b>)'
                     }
                 }
                 else{
                     if(row.Type == "COM"){
                         return data  + '</br>(<b>Component</b>)'
                     }
                     else{
                         return data + '</br>(<b>Product</b>)'
                     }
                 }
                }, "width": "10%"
            },
            { "data": "Description", "defaultContent": "<i>-<i>", "width": "10%" },
            { "data": "Unit.Description", "defaultContent": "<i>-<i>", "width": "5%" },
            { "data": "ProductCategory.Description", "defaultContent": "<i>-<i>", "width": "5%" },
            //{ "data": "Type", "defaultContent": "<i>-<i>", "width": "5%" },
            {"data": "ReorderQty", "defaultContent": "<i>-<i>", "width": "5%" },
            { "data": "CurrentStock", "defaultContent": "<i>-<i>", "width": "5%" },
            { "data": "HSNNo", "defaultContent": "<i>-<i>", "width": "5%" },
            { "data": "WeightInKG", "defaultContent": "<i>-<i>", "width": "5%" },
            { "data": "CostPrice", render: function (data, type, row) {
                if (data == 0)
                    return '-'
                else
                    return roundoff(data, 1);
            }, "defaultContent": "<i>-<i>", "width": "7%" },
            { "data": "CostPricePerPiece" , render: function (data, type, row) {
                if (data == 0)
                    return '-'
                else
                    return roundoff(data, 1);
            }, "defaultContent": "<i>-<i>", "width": "5%" },
            {
                "data": "SellingPriceInKG", render: function (data, type, row) {
                    if (data == 0)
                        return '-'
                    else
                        return roundoff(data, 1);
                }, "defaultContent": "<i>-<i>", "width": "5%"
            },
            {
                "data": "SellingPricePerPiece", render: function (data, type, row) {
                    if (data == 0)
                        return '-'
                    else
                        return roundoff(data, 1);
                }, "defaultContent": "<i>-<i>", "width": "5%"
            },
            { "data": null, "orderable": false, "defaultContent": '<a href="#" onclick="EditProductMaster(this)"<i class="glyphicon glyphicon-edit" aria-hidden="true"></i></a> <a href="#" onclick="DeleteProductMaster(this)"<i class="glyphicon glyphicon-trash" aria-hidden="true"></i></a>  ', "width": "4%" }
            ],
            columnDefs: [{ "targets": [], "visible": false, "searchable": false },
                { className: "text-right", "targets": [5,6,8,9,10,11,12] },
                { className: "text-left", "targets": [0,1,2, 3, 4,7] },
                { className: "text-center", "targets": [13] }],
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
    ProductViewModel = _dataTable.productList.row($(this_obj).parents('tr')).data();
    GetMasterPartial("Product", ProductViewModel.ID);
    $('#h3ModelMasterContextLabel').text('Edit Product/Component')
    $('#divModelMasterPopUp').modal('show');
    $('#hdnMasterCall').val('MSTR');
}


//--Function To Confirm Product Deletion 
function DeleteProductMaster(this_obj) {
    debugger;
    productVM = _dataTable.productList.row($(this_obj).parents('tr')).data();
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

