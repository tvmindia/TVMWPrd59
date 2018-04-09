
//*****************************************************************************
//*****************************************************************************
//Author: Jais
//CreatedDate: 6-Apr-2018 
//LastModified: 6-Apr-2018 
//FileName: ProductCategory.js
//Description: Client side coding for Product Category
//******************************************************************************
//******************************************************************************

//--Global Declaration--//
var _dataTable = {};
var _emptyGuid = "00000000-0000-0000-0000-000000000000";

//--Loading DOM--//
$(document).ready(function () {
    try {
        debugger;
        BindOrReloadProductCategoryTable('Init');
    }
    catch (e) {
        console.log(e.message);
    }
});


//--function bind the Product Category list checking search and filter--//
function BindOrReloadProductCategoryTable(action) {
    try {
        debugger;
        //creating advancesearch object
        ProductCategoryAdvanceSearchViewModel = new Object();
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
                DataTablePagingViewModel.Length = -1;
                break;
            default:
                break;
        }
        ProductCategoryAdvanceSearchViewModel.DataTablePaging = DataTablePagingViewModel;
        ProductCategoryAdvanceSearchViewModel.SearchTerm = $('#SearchTerm').val();
        debugger;
        //apply datatable plugin on Raw Material table
        _dataTable.productCategoryList = $('#tblProductCategory').DataTable(
        {

            dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
            buttons: [{
                extend: 'excel',
                exportOptions:
                             {
                                 columns: [0,1]
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
                url: "ProductCategory/GetAllProductCategory/",
                data: { "productCategoryAdvanceSearchVM": ProductCategoryAdvanceSearchViewModel },
                type: 'POST'
            },
            pageLength: 10,
            columns: [
            { "data": "Code", "defaultContent": "<i>-</i>" },
            { "data": "Description", "defaultContent": "<i>-<i>"},
            { "data": null, "orderable": false, "defaultContent": '<a href="#" onclick="DeleteProductCategoryMaster(this)"<i class="glyphicon glyphicon-trash" aria-hidden="true"></i></a>  <a href="#" onclick="EditProductCategoryMaster(this)"<i class="glyphicon glyphicon-share-alt" aria-hidden="true"></i></a>', "width": "4%" }
            ],
            columnDefs: [{ "targets": [], "visible": false, "searchable": false },
                { className: "text-right", "targets": [2] },
                { className: "text-left", "targets": [0, 1] },
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
                    BindOrReloadProductCategoryTable('Search');
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
function ResetProductCategoryList() {
    BindOrReloadProductCategoryTable('Reset');
}

//--function export data to excel--//
function ImportProductCategoryData() {
    BindOrReloadProductCategoryTable('Export');
}

//--edit Product Category--//
function EditProductCategoryMaster(this_obj) {
    ProductCategoryViewModel = _dataTable.productCategoryList.row($(this_obj).parents('tr')).data();
    GetMasterPartial("ProductCategory", ProductCategoryViewModel.Code);
    $('#h3ModelMasterContextLabel').text('Edit Product Category')
    $('#divModelMasterPopUp').modal('show');
}

//--Function To Confirm Product Category Deletion 
function DeleteProductCategoryMaster(this_obj) {
    debugger;
    productCategoryVM = _dataTable.productCategoryList.row($(this_obj).parents('tr')).data();
    notyConfirm('Are you sure to delete?', 'DeleteProductCategory("' + productCategoryVM.Code + '")');
}

//--Function To Delete Product Category
function DeleteProductCategory(code) {
    debugger;
    try {
        if (code) {
            var data = { "code": code };
            var jsonData = {};
            var message = "";
            var status = "";
            var result = "";
            jsonData = GetDataFromServer("ProductCategory/DeleteProductCategory/", data);
            if (jsonData != '') {
                jsonData = JSON.parse(jsonData);
                message = jsonData.Message;
                status = jsonData.Status;
                result = jsonData.Record;
            }
            switch (status) {
                case "OK":
                    notyAlert('success', result.Message);
                    BindOrReloadProductCategoryTable('Reset');
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
