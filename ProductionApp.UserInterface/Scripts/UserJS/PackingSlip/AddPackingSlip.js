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
var _SlNo = 1;
var pkgDetail = [];
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
                 { "data": "PkgQty", "defaultContent": "<i>-</i>" },
                 {
                     "data": "CurrentPkgQty", "defaultContent": "<i>-</i>",
                     'render': function (data, type, row) {
                         if (data == undefined)
                             Qty = 0;
                         else
                             Qty = row.CurrentPkgQty;
                         return '<input class="form-control description" name="Markup" value="'+Qty+'" type="text" onchange="textBoxValueChanged(this,1);">';
                     }
                 },
                 {
                     "data": "Weight", "defaultContent": "<i>-</i>",
                     'render': function (data, type, row) {
                         if (data == undefined)
                             Wt = 0.0;
                         else
                             Wt = row.Weight;
                         return '<input class="form-control description" name="Markup" value="' + Wt + '" type="text" onchange="textBoxValueChanged(this,2);">';
                     }
                 }
            ],
            columnDefs: [{ orderable: false, className: 'select-checkbox', "targets": 1 }
                , { className: "text-left", "targets": [2,3,4,5,6,7] }
                , { className: "text-right", "targets": [] }
                , { "targets": [0], "visible": false, "searchable": false }
                , { "targets": [2, 3, 4, 5, 6], "bSortable": false }],
            select: { style: 'multi', selector: 'td:first-child' },
            destroy: true
        });

        //PackingSlip Detail Tbl 
        DataTables.PackingSlipDetailTable = $('#tblPackingSlipDetail').DataTable(
     {
         dom: '<"pull-right"f>rt<"bottom"ip><"clear">',
         ordering: false,
         searching: false,
         paging: false,
         data: null,
         autoWidth: false,
         "bInfo": false,
         columns: [
         { "data": "ProductID", "defaultContent": "<i></i>" },
         {
             "data": "", render: function (data, type, row) {
                 debugger;
                 return _SlNo++
             }, "defaultContent": "<i></i>"
         },
         { "data": "Product", render: function (data, type, row) { return data }, "defaultContent": "<i></i>" },
         { "data": "CurrentPkgQty", render: function (data, type, row) { return data }, "defaultContent": "<i></i>" },
         { "data": "Weight", render: function (data, type, row) { return data }, "defaultContent": "<i></i>" },
         { "data": null, "orderable": false, "defaultContent": '<a href="#" class="DeleteLink"  onclick="Delete(this)" ><i class="glyphicon glyphicon-trash" aria-hidden="true"></i></a> | <a href="#" class="actionLink"  onclick="MaterialEdit(this)" ><i class="glyphicon glyphicon-pencil" aria-hidden="true"></i></a>' },
         ],
         columnDefs: [{ "targets": [0], "visible": false, searchable: false },
             { className: "text-left", "targets": [2, 3, 4, 5] },
             { "targets": [1], "width": "2%", },
             { "targets": [2], "width": "25%" },
             { "targets": [3], "width": "10%" },
             { "targets": [4], "width": "10%" },
             { "targets": [5], "width": "7%" }
         ],

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
    DataTables.ProductListTable.rows().select();
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
//TextBox value change in datatable
function textBoxValueChanged(thisObj, textBoxCode) {
    debugger;
    var IDs = selectedRowIDs();//identify the selected rows 
    var productDetailsVM = DataTables.ProductListTable.rows().data();
    var productDetailstable = DataTables.ProductListTable;
    var rowtable = productDetailstable.row($(thisObj).parents('tr')).data();
    for (var i = 0; i < productDetailsVM.length; i++) {
        if (productDetailsVM[i].ProductID == rowtable.ProductID) {
            if (textBoxCode == 1)//textBoxCode is the code to know, which textbox changed is triggered
                productDetailsVM[i].CurrentPkgQty = parseFloat(thisObj.value);
            if (textBoxCode == 2)
                productDetailsVM[i].Weight = parseFloat(thisObj.value);
        }
    }
    DataTables.ProductListTable.clear().rows.add(productDetailsVM).draw(false);
    selectCheckbox(IDs); //Selecting the checked rows with their ids taken 
}
//selected Checkbox
function selectCheckbox(IDs) {
    var productDetailsVM = DataTables.ProductListTable.rows().data()
    for (var i = 0; i < productDetailsVM.length; i++) {
        if (IDs.includes(productDetailsVM[i].ID)) {
            DataTables.ProductListTable.rows(i).select();
        }
        else {
            DataTables.ProductListTable.rows(i).deselect();
        }
    }
}
//Selected Rows
function selectedRowIDs() {
    var allData = DataTables.ProductListTable.rows(".selected").data();
    var arrIDs = "";
    for (var r = 0; r < allData.length; r++) {
        if (r == 0)
            arrIDs = allData[r].ID;
        else
            arrIDs = arrIDs + ',' + allData[r].ID;
    }
    return arrIDs;
}
//Add values to packingSlip detail Tbl
function AddPackingSlipDetailTbl() {
    _SlNo = 1;
    //Merging  the rows with same MaterialID
    var productDetailsVM = DataTables.ProductListTable.rows(".selected").data();
    AddPackingSlipDetailData(productDetailsVM);// adding values to packingSlipDetail
    DataTables.PackingSlipDetailTable.rows.add(pkgDetail).draw(false); //binding Detail table with new values
    $('#PackingSlipModal').modal('hide');
}
function AddPackingSlipDetailData(data)
{
    for (var r = 0; r < data.length; r++) {
        pkgDetailLink = new Object();
        pkgDetailLink.ProductID = data[r].ProductID;
        pkgDetailLink.ID = EmptyGuid; //[PKGDetailID]
        pkgDetailLink.Product = data[r].Product.Name;
        pkgDetailLink.CurrentPkgQty = data[r].CurrentPkgQty;
        pkgDetailLink.Weight = data[r].Weight;
        pkgDetail.push(pkgDetailLink);
    }
}