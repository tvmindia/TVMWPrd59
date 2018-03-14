//*****************************************************************************
//*****************************************************************************
//Author: Angel
//CreatedDate: 14-Mar-2018 
//LastModified: 14-Mar-2018 
//FileName: AddPackingSlip.js
//Description: Client side coding for AddPackingSlip.
//******************************************************************************
//******************************************************************************

//--Global Declaration--//
var DataTables = {};
var EmptyGuid = "00000000-0000-0000-0000-000000000000";
$(document).ready(function () {
    debugger;
    try {
        $("#PackedBy").select2({
        });
        $("#DispatchedBy").select2({
        });
        $("#SalesOrderID").select2({
        });

        //ProductListTable
        DataTables.ProductListTable = $('#tblProductList').DataTable({
            dom: '<"pull-right"f>rt<"bottom"ip><"clear">',
            ordering: false,
            searching: false,
            paging: true,
            "bInfo": false,
            data: null,
            pageLength: 15,
            language: {
                search: "_INPUT_",
                searchPlaceholder: "Search"
            },
            columns: [
                 { "data": "ProductID", "defaultContent": "<i>-</i>" },
                 { "data": "Checkbox", "defaultContent": "", "width": "5%" },
                 { "data": "Product.Name", "defaultContent": "<i>-</i>" },
                 { "data": "Product.CurrentStock", "defaultContent": "<i>-</i>" },
                 { "data": "Quantity", "defaultContent": "<i>-</i>" },
                 {
                     "data": null, "defaultContent": "<i>-</i>",
                     'render': function (data, type, row) {
                        
                         return '<input class="form-control description" name="Markup" value="" type="text" onchange="textBoxValueChanged(this,1);">';
                     }
                 },
                 {
                     "data": null, "defaultContent": "<i>-</i>",
                     'render': function (data, type, row) {
                         return '<input class="form-control description" name="Markup" value="" type="text" onchange="textBoxValueChanged(this,1);">';
                     }
                 }
            ],
            columnDefs: [{ orderable: false, className: 'select-checkbox', "targets": 1 }
                , { className: "text-left", "targets": [2,3,4,5] }
                , { className: "text-right", "targets": [6] }
                , { "targets": [0], "visible": false, "searchable": false }
                , { "targets": [2, 3, 4, 5, 6], "bSortable": false }],
            select: { style: 'multi', selector: 'td:first-child' },
            destroy: true
        });

    }
    catch (e) {
        console.log(e.message);
    }
});

function AddPackingSlipDetail()
{
    $('#PackingSlipModal').modal('show');
    BindProductListTable();
}
//bind product List
function BindProductListTable() {
    var productList = GetProductList();
    DataTables.ProductListTable.clear().rows.add(productList).draw(false);
}
function GetProductList() {
    try {
        debugger;
        var id = $('#SalesOrderID').val();
        var data = { "id": id };
        var jsonData = {};
        var result = "";
        var message = "";
        var productListVM = new Object();

        jsonData = GetDataFromServer("PackingSlip/GetProductList/", data);
        if (jsonData != '') {
            jsonData = JSON.parse(jsonData);
            result = jsonData.Result;
            message = jsonData.Message;
            productListVM = jsonData.Records;
        }
        if (result == "OK") {

            return productListVM;
        }
        if (result == "ERROR") {
            alert(message);
        }
    }
    catch (e) {
        //this will show the error msg in the browser console(F12) 
        console.log(e.message);
    }
}