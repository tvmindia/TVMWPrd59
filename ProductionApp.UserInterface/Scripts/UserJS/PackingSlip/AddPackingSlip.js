//*****************************************************************************
//*****************************************************************************
//Author: Angel
//CreatedDate: 14-Mar-2018 
//LastModified: 17-Mar-2018 
//FileName: AddPackingSlip.js
//Description: Client side coding for AddPackingSlip.
//******************************************************************************
//******************************************************************************

//--Global Declaration--//
var DataTables = {};
var EmptyGuid = "00000000-0000-0000-0000-000000000000";
var _SlNo = 1;
var pkgDetail = [];
var _packingSlipDetailList = [];
var _packingSlipViewModel = new Object();
var colorFlag = 0;
$(document).ready(function () {
    debugger;
    try {
        $("#PackedBy").select2({
        });
        $("#DispatchedBy").select2({
        });
        $("#SalesOrderID").select2({
        });
        debugger;
        var PkAccess = $('#ShowPkgSec').val();
        var DispAccess = $('#ShowDispatcherSec').val();
        //disable fieds according to role
        if ($('#ShowPkgSec').val() == 'True' && $('#ShowDispatcherSec').val() == 'True') {
            $('#divPack').find('input, textarea, button, select').prop('disabled', false);
            $('#divDespatch').find('input, textarea, button, select').prop('disabled', false);
            $('#btnAddPackingSlip').attr("disabled", false);
        }
        else if ($('#ShowDispatcherSec').val() == 'True') {
            $('#divPack').find('input, textarea, button, select').prop('disabled', true);
            $('#btnAddPackingSlip').attr("disabled", true);
            $('#divDespatch').find('input, textarea, button, select').prop('disabled', false);
        }
        else {
            $('#divPack').find('input, textarea, button, select').prop('disabled', false);
            $('#btnAddPackingSlip').attr("disabled", false);
            $('#divDespatch').find('input, textarea, button, select').prop('disabled', true);
        }
        $('#SlipNo').attr("disabled", true);
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
                 { "data": "PrevPkgQty", "defaultContent": "<i>-</i>" },
                 {
                     "data": "BalQty", "defaultContent": "<i>-</i>",
                     'render': function (data, type, row) {
                         return BalQty = row.Quantity - row.PrevPkgQty;
                     }
                 },
                 {
                     "data": "CurrentPkgQty", "defaultContent": "<i>-</i>",
                     'render': function (data, type, row) {
                         return '<input class="form-control description" name="Markup" value="' + data + '" type="text" onchange="textBoxValueChanged(this,1);">';
                     }
                 },
                 {
                     "data": "PkgWt", "defaultContent": "<i>-</i>",
                     'render': function (data, type, row) {
                         Wt = row.PkgWt;
                         return '<input class="form-control description" name="Markup" value="' + Wt + '" type="text" id="txt' + row.ProductID + '" onchange="textBoxValueChanged(this,2);">';
                     }
                 }
            ],
            columnDefs: [{ orderable: false, className: 'select-checkbox', "targets": 1 }
                , { className: "text-right", "targets": [ 3, 4, 5, 6, 7,8] }
                , { className: "text-left", "targets": [2] }
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
                
                 return _SlNo++
             }, "defaultContent": "<i></i>"
         },
         { "data": "Product", render: function (data, type, row) { return data }, "defaultContent": "<i></i>" },
         { "data": "CurrentPkgQty", render: function (data, type, row) { return data }, "defaultContent": "<i></i>" },
         { "data": "PkgWt", render: function (data, type, row) { return data }, "defaultContent": "<i></i>" },
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
    var $form = $('#AddPackingSlipForm');
    if ($form.valid()) {
        $('#lblOrderNo').text("SalesOrder No: " + $("#SalesOrderID option:selected").text());
        $('#PackingSlipModal').modal('show');
        BindProductListTable();
    }
    else {
        notyAlert('warning', "Please Fill Required Fields,To Add Items ");
    }
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
                {
                if (thisObj.value <= productDetailsVM[i].Product.CurrentStock)
                    productDetailsVM[i].CurrentPkgQty = parseFloat(thisObj.value);
                else
                    productDetailsVM[i].CurrentPkgQty = parseFloat(productDetailsVM[i].Product.CurrentStock) - parseFloat(productDetailsVM[i].PrevPkgQty)
            }
            if (textBoxCode == 2)
                productDetailsVM[i].PkgWt = parseFloat(thisObj.value);
        }
    }
    DataTables.ProductListTable.clear().rows.add(productDetailsVM).draw(false);
    selectCheckbox(IDs); //Selecting the checked rows with their ids taken 
    productDetailsVM = DataTables.ProductListTable.rows(".selected").data();
    CheckProductDetails(productDetailsVM);
}
//selected Checkbox
function selectCheckbox(IDs) {
    var productDetailsVM = DataTables.ProductListTable.rows().data()
    for (var i = 0; i < productDetailsVM.length; i++) {
        if (IDs.includes(productDetailsVM[i].ProductID)) {
            DataTables.ProductListTable.rows(i).select();
        }
        else {
            DataTables.ProductListTable.rows(i).deselect();
        }
    }
}
//Selected Rows
function selectedRowIDs() {
    var productVM = DataTables.ProductListTable.rows(".selected").data();
    var productIDs = "";
    for (var r = 0; r < productVM.length; r++) {
        if (r == 0)
            productIDs = productVM[r].ProductID;
        else
            productIDs = productIDs + ',' + productVM[r].ProductID;
    }
    return productIDs;
}
//Add values to packingSlip detail Tbl
function AddPackingSlipDetailTbl() {
    _SlNo = 1;
    var productDetailsVM = DataTables.ProductListTable.rows(".selected").data();
    debugger;
    var res = CheckProductDetails(productDetailsVM);//Checking all details are entered or not
    if (res) {
        AddPackingSlipDetailData(productDetailsVM);// adding values to packingSlipDetail
        DataTables.PackingSlipDetailTable.clear().rows.add(pkgDetail).draw(false); //binding Detail table with new values   
        //if(res)
        $('#PackingSlipModal').modal('hide');
    }
    else
        notyAlert('warning', "Please Enter Weight of Product(s)");
}
function CheckProductDetails(producDetails)
{
    var flag = 0;
    if ((producDetails) && (producDetails.length > 0)) {
        
        for (var r = 0; r < producDetails.length; r++) {
            if (producDetails[r].PkgWt == 0) {
                flag = 1;
                $("#txt" + producDetails[r].ProductID).attr('style','border-color:red;')
            }
        }
        if (flag == 1)
            return false
        else
        return true;
    }
}

function AddPackingSlipDetailData(data)
{
    var flag = 0;
    debugger;
    //var packingDetailsVM = DataTables.PackingSlipDetailTable.rows().data();
    if ((data) && (data.length > 0)) {
        
        for (var r = 0; r < data.length; r++) {
            if ((data[r].CurrentPkgQty != undefined) || (data[r].PkgWt != 0)) {
                        pkgDetailLink = new Object();
                        pkgDetailLink.ProductID = data[r].ProductID;
                        pkgDetailLink.ID = EmptyGuid; //[PKGDetailID]
                        pkgDetailLink.Product = data[r].Product.Name;
                        pkgDetailLink.CurrentPkgQty = data[r].CurrentPkgQty;
                        pkgDetailLink.PkgWt = data[r].PkgWt;
                        pkgDetailLink.Qty = data[r].CurrentPkgQty;
                        pkgDetailLink.Weight = data[r].PkgWt;
                        pkgDetail.push(pkgDetailLink);
            }
            else {
                flag = flag + 1;
                }
        }
        if (flag == data.length) {
            notyAlert('warning', "Please Enter Pkg Qty and Weight of Product(s)");
            return false;
        }
       
    }
    else
    {
        notyAlert('warning', "Please Select Product");
        return false;
    }
}
//Save
function Save() {
    debugger;
    //validation main form 
    _packingSlipDetailList = [];
    AddPackingSlipDetailList();
    if (_packingSlipDetailList.length > 0) {
        _packingSlipViewModel.ID = $('#ID').val();
        _packingSlipViewModel.SlipNo = $('#SlipNo').val();
        _packingSlipViewModel.Date = $('#DateFormatted').val();
        _packingSlipViewModel.IssueToDispatchDate = $('#IssueToDispatchDateFormatted').val();
        _packingSlipViewModel.SalesOrderID = $('#SalesOrderID').val();
        _packingSlipViewModel.PackingRemarks = $('#PackingRemarks').val();
        _packingSlipViewModel.PackedBy = $('#PackedBy').val();
        _packingSlipViewModel.TotalPackageWeight = $('#TotalPackageWeight').val();
        _packingSlipViewModel.PackingSlipDetail = pkgDetail;
        _SlNo = 1;
        var data = "{'packingSlipVM':" + JSON.stringify(_packingSlipViewModel) + "}";

        PostDataToServer("PackingSlip/InsertUpdatePackingSlip/", data, function (JsonResult) {
            debugger;
            switch (JsonResult.Result) {
                case "OK":
                    notyAlert('success', JsonResult.Records.Message);
                    ChangeButtonPatchView('PackingSlip', 'divbuttonPatchAddPkgSlip', 'Edit');
                    if (JsonResult.Records.ID) {
                        $("#ID").val(JsonResult.Records.ID);
                        BindPkgSlip($("#ID").val());
                    } else {
                        Reset();
                    }
                    pkgDetail = [];
                    break;
                case "Error":
                    notyAlert('error', JsonResult.Message);
                    break;
                case "ERROR":
                    notyAlert('error', JsonResult.Message);
                    break;
                default:
                    break;
            }
        })
    }
    else {
        notyAlert('warning', 'Please Add item Details!');
    }
}
//PackingSlipDetail data 
function AddPackingSlipDetailList() {
    debugger;
    var packingSlipDetail = DataTables.PackingSlipDetailTable.rows().data();
    for (var r = 0; r < packingSlipDetail.length; r++) {
        pkgDetail = new Object();
        pkgDetail.ProductID = packingSlipDetail[r].ProductID;
        pkgDetail.Qty = packingSlipDetail[r].CurrentPkgQty;
        pkgDetail.Weight = packingSlipDetail[r].PkgWt;
        _packingSlipDetailList.push(pkgDetail);
    }
}