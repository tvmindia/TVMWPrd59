//*****************************************************************************
//*****************************************************************************
//Author: Angel
//CreatedDate: 14-Mar-2018 
//LastModified: 20-Mar-2018 
//FileName: AddPackingSlip.js
//Description: Client side coding for AddPackingSlip.
//******************************************************************************
//******************************************************************************

//--Global Declaration--//
var DataTables = {};
var EmptyGuid = "00000000-0000-0000-0000-000000000000";
var _SlNo = 1;
var packingDetail = [];
var _packingSlipDetailList = [];
var _packingSlipViewModel = new Object();
var colorFlag = 0;
var EditFlag = 0,selected=1;
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
                 { "data": "Product.Name", "defaultContent": "<i>-</i>", "width": "30%" },
                 { "data": "Product.CurrentStock", "defaultContent": "<i>-</i>", "width": "10%" },
                 { "data": "Quantity", "defaultContent": "<i>-</i>", "width": "10%" },
                 { "data": "PrevPkgQty", "defaultContent": "<i>-</i>", "width": "10%" },
                 {
                     "data": "BalQty", "defaultContent": "<i>-</i>", "width": "10%",
                     'render': function (data, type, row) {
                         return BalQty = row.Quantity - row.PrevPkgQty;
                     }
                 },
                 {
                     "data": "CurrentPkgQty", "defaultContent": "<i>-</i>", "width": "10%",
                     'render': function (data, type, row) {
                         return '<input class="form-control description text-right" name="Markup" value="' + data + '" onclick="SelectAllValue(this);" onkeypress = "return isNumber(event)", type="text" id="txt' + row.ProductID + '"  onchange="textBoxValueChanged(this,1);"style="width:100%">';
                     }
                 },
                 {
                     "data": "PkgWt", "defaultContent": "<i>-</i>", "width": "15%",
                     'render': function (data, type, row) {
                         Wt = row.PkgWt;
                         return '<input class="form-control description text-right" name="Markup" value="' + Wt + '" onclick="SelectAllValue(this);" onkeypress = "return isNumber(event)", type="text"  onchange="textBoxValueChanged(this,2);"style="width:100%">';
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
         { "data": "ID", "defaultContent": "<i></i>" },
         { "data": "ProductID", "defaultContent": "<i></i>" },
         {
             "data": "", render: function (data, type, row) {
                
                 return _SlNo++
             }, "defaultContent": "<i></i>"
         },
         { "data": "Name", render: function (data, type, row) { return data }, "defaultContent": "<i></i>" },
         { "data": "Qty", render: function (data, type, row) { return data }, "defaultContent": "<i></i>" },
         { "data": "Weight", render: function (data, type, row) { return data }, "defaultContent": "<i></i>" },
         { "data": null, "orderable": false, "defaultContent": '<a href="#" class="actionLink"  onclick="EditPkgSlipDetailTable(this)" ><i class="glyphicon glyphicon-edit" aria-hidden="true"></i></a> | <a href="#" class="DeleteLink"  onclick="Delete(this)" ><i class="glyphicon glyphicon-trash" aria-hidden="true"></i></a>' },
         ],
         columnDefs: [{ "targets": [0,1], "visible": false, searchable: false },
             { className: "text-left", "targets": [2, 3] },
             { className: "text-right", "targets": [4, 5] },
             { className: "text-center", "targets": [6] },
             { "targets": [2], "width": "3%" },
             { "targets": [3], "width": "50%" },
             { "targets": [4], "width": "20%" },
             { "targets": [5], "width": "20%" },
             { "targets": [6], "width": "7%" }
         ],

     });

        //EditPkgSlipDetailEditTbl
        DataTables.PkgSlipDetailEditTable = $('#tblPkgSlipDetailsEdit').DataTable({
            dom: '<"pull-right"f>rt<"bottom"ip><"clear">',
            ordering: false,
            searching: false,
            paging: false,
            "bInfo": false,
            data: null,
            pageLength: 15,
            language: {
                search: "_INPUT_",
                searchPlaceholder: "Search"
            },
            columns: [
                 { "data": "ID", "defaultContent": "<i>-</i>" },
                 { "data": "SalesOrder.SalesOrderDetail.ProductID", "defaultContent": "<i>-</i>" },
                 { "data": "SalesOrder.SalesOrderDetail.Product.Name", "defaultContent": "<i>-</i>" },
                 { "data": "SalesOrder.SalesOrderDetail.Product.CurrentStock", "defaultContent": "<i>-</i>" },
                 { "data": "SalesOrder.SalesOrderDetail.Quantity", "defaultContent": "<i>-</i>" },
                 { "data": "SalesOrder.SalesOrderDetail.PrevPkgQty", "defaultContent": "<i>-</i>" },
                 {
                     "data": "BalQty", "defaultContent": "<i>-</i>",
                     'render': function (data, type, row) {
                         return BalQty = row.SalesOrder.SalesOrderDetail.Quantity - row.SalesOrder.SalesOrderDetail.PrevPkgQty;
                     }
                 },
                 {
                     "data": "SalesOrder.SalesOrderDetail.CurrentPkgQty", "defaultContent": "<i>-</i>",
                     'render': function (data, type, row) {
                         return '<input class="form-control description text-right" name="Markup" value="' + data + '" onclick="SelectAllValue(this);" onkeypress = "return isNumber(event)", type="text" id="txt' + row.ProductID + '" onchange="textBoxValueChangedEditTbl(this,1);"style="width:100%">';
                     }
                 },
                 {
                     "data": "SalesOrder.SalesOrderDetail.PkgWt", "defaultContent": "<i>-</i>",
                     'render': function (data, type, row) {
                         Wt = row.SalesOrder.SalesOrderDetail.PkgWt;
                         return '<input class="form-control description text-right" name="Markup" value="' + Wt + '" onclick="SelectAllValue(this);" onkeypress = "return isNumber(event)", type="text"  onchange="textBoxValueChangedEditTbl(this,2);"style="width:100%">';
                     }
                 }
            ],
            columnDefs: [
                  { className: "text-right", "targets": [3, 4, 5, 6, 7,8] }
                , { className: "text-left", "targets": [2] }
                , { "targets": [0,1], "visible": false, "searchable": false }
                , { "targets": [2, 3, 4, 5, 6], "bSortable": false }],
            destroy: true
        });

        //EditPackingSlip
        debugger;
        if ($('#ID').val() != EmptyGuid) {
            BindPkgSlip($('#ID').val());
        }
    }
    catch (e) {
        console.log(e.message);
    }
});

function AddPackingSlipDetail()
{
    if($('#PackedBy').val() && $('#DateFormatted').val() && $('#SalesOrderID').val()){
        $('#lblOrderNo').text("SalesOrder# : " + $("#SalesOrderID option:selected").text());
        $('#PackingSlipModal').modal('show');
        BindProductListTable();
    }
    else {
        notyAlert('warning', "Please Fill Required Fields,To Add Items ");
    }
}
//SalesOrder Details
function OrderDetails() {
    debugger;
    var saleId = $('#SalesOrderID').val();
    var SalesOrderVm = GetOrderDetails(saleId);
    $('#lblCustomer').text(SalesOrderVm.CustomerName);
    $('#lblExpDate').text(SalesOrderVm.ExpectedDeliveryDateFormatted);
}
function GetOrderDetails(saleId) {
    try {
        debugger;
        var data = { "salesOrderId": saleId };
        var result = "";
        var message = "";
        var jsonData = {};
        var salesDetailVM = new Object();
        jsonData = GetDataFromServer("PackingSlip/GetSalesOrderCustomer/", data);
        if (jsonData != '') {
            jsonData = JSON.parse(jsonData);
            result = jsonData.Result;
            salesDetailVM = jsonData.Records;
            message = jsonData.Message;
        }
        if (result == "OK") {
            return salesDetailVM;

        }
        if (result == "ERROR") {
            alert(Message);
        }
    }
    catch (e) {
        //this will show the error msg in the browser console(F12) 
        console.log(e.Message);
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
        var packingSlipID = $('#ID').val();
        var data = { "id": id, "packingSlipID": packingSlipID };
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
                if ((thisObj.value <= productDetailsVM[i].Product.CurrentStock) && (thisObj.value != ""))
                    productDetailsVM[i].CurrentPkgQty = parseFloat(thisObj.value);
                else
                    productDetailsVM[i].CurrentPkgQty = parseFloat(productDetailsVM[i].Product.CurrentStock) - parseFloat(productDetailsVM[i].PrevPkgQty)
            }
            if (textBoxCode == 2)
            {
                if (thisObj.value != "")
                    productDetailsVM[i].PkgWt = parseFloat(thisObj.value);
                else
                    productDetailsVM[i].PkgWt = 0;
            }
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
    var res = CheckProductList(productDetailsVM);//Checking all details are entered or not
    if(selected==1)
    if (res) {
        AddPackingSlipDetailData(productDetailsVM);// adding values to packingSlipDetail
        _SlNo = 1;
        DataTables.PackingSlipDetailTable.clear().rows.add(packingDetail).draw(false); //binding Detail table with new values   
        $('#PackingSlipModal').modal('hide');
        Save();
    }
    else
        notyAlert('warning', "Please Enter Packing Quantity of Product(s)");
}
function CheckProductDetails(producDetails)
{
   debugger;
   var flag = 0;
    if ((producDetails) && (producDetails.length > 0)) {
        selected = 1;
        for (var r = 0; r < producDetails.length; r++) {
            if (producDetails[r].CurrentPkgQty == 0)  {
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
function CheckProductList(producDetails)
{
    debugger;
    if (producDetails.length == 0) {
        notyAlert('warning', "Please Select Product");
        selected = 0;
        return false;
    }
    var result = CheckProductDetails(producDetails);
    return result;
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
                        pkgDetailLink.Name = data[r].Product.Name;
                        pkgDetailLink.CurrentPkgQty = data[r].CurrentPkgQty;
                        pkgDetailLink.PkgWt = data[r].PkgWt;
                        pkgDetailLink.Qty = data[r].CurrentPkgQty;
                        pkgDetailLink.Weight = data[r].PkgWt;
                        packingDetail.push(pkgDetailLink);
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
    var valid = 0;
    //validation main form 
    if ($('#ShowDispatcherSec').val() == 'True' && $('#ShowPkgSec').val() == 'False') {
        if (!($('#DispatchedDateFormatted').val())) {
            notyAlert('warning', "Please Fill Dispatched date");
            valid = 1;
        }
    }
    if (valid == 0) {
        var $form = $('#AddPackingSlipForm');
        if ($form.valid()) {
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
                _packingSlipViewModel.CheckedPackageWeight = $('#CheckedPackageWeight').val();
                _packingSlipViewModel.DispatchedBy = $('#DispatchedBy').val();
                _packingSlipViewModel.DispatchedDate = $('#DispatchedDateFormatted').val();
                _packingSlipViewModel.VehiclePlateNo = $('#VehiclePlateNo').val();
                _packingSlipViewModel.DriverName = $('#DriverName').val();
                _packingSlipViewModel.ReceivedBy = $('#ReceivedBy').val();
                _packingSlipViewModel.ReceivedDate = $('#ReceivedDateFormatted').val();
                _packingSlipViewModel.DispatchRemarks = $('#DispatchRemarks').val();
                _packingSlipViewModel.PackingSlipDetail = packingDetail;
                _SlNo = 1;
                $("#DetailJSON").val('');
                var result = JSON.stringify(_packingSlipDetailList);
                $("#DetailJSON").val(result);
                _packingSlipViewModel.DetailJSON = $("#DetailJSON").val();
                var data = "{'packingSlipVM':" + JSON.stringify(_packingSlipViewModel) + "}";
                PostDataToServer("PackingSlip/InsertUpdatePackingSlip/", data, function (JsonResult) {
                    debugger;
                    switch (JsonResult.Result) {
                        case "OK":
                            notyAlert('success', JsonResult.Records.Message);
                            ChangeButtonPatchView('PackingSlip', 'divbuttonPatchAddPkgSlip', 'Edit');
                            debugger;
                            if (JsonResult.Records.ID) {
                                $("#ID").val(JsonResult.Records.ID);
                                $('#IsUpdate').val('True');
                                BindPkgSlip($("#ID").val());
                            } else {
                                Reset();
                            }
                            packingDetail = [];
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
        else {
            notyAlert('warning', "Please Fill Required Fields ");
        }
    }
}
//PackingSlipDetail data 
function AddPackingSlipDetailList() {
    debugger;
    if(EditFlag==0){
    var packingSlipDetail = DataTables.PackingSlipDetailTable.rows().data();
    for (var r = 0; r < packingSlipDetail.length; r++) {
        pkgDetail = new Object();
        pkgDetail.PackingSlipID = $("#ID").val();
        pkgDetail.ID = packingSlipDetail[r].ID;
        pkgDetail.ProductID = packingSlipDetail[r].ProductID;
        pkgDetail.Qty = packingSlipDetail[r].Qty;
        pkgDetail.Weight = packingSlipDetail[r].Weight;
        _packingSlipDetailList.push(pkgDetail);
    }
    }
    else{
    var packingSlipEditDetail = DataTables.PkgSlipDetailEditTable.rows().data();
    //for (var r = 0; r < packingSlipDetail.length; r++) {
        packingDetail = new Object();
        packingDetail.PackingSlipID = $("#ID").val();
        packingDetail.ID = packingSlipEditDetail[0].ID;
        packingDetail.ProductID = packingSlipEditDetail[0].SalesOrder.SalesOrderDetail.ProductID;
        packingDetail.Qty = packingSlipEditDetail[0].SalesOrder.SalesOrderDetail.CurrentPkgQty;
        packingDetail.Weight = packingSlipEditDetail[0].SalesOrder.SalesOrderDetail.PkgWt;
        _packingSlipDetailList.push(packingDetail);
        EditFlag = 0;
    //}
}
}
//Bind PkgSlipData
function BindPkgSlip(ID) {
    try {
        debugger;
        _SlNo = 1;
        var pkgData = GetPkgSlipByID(ID)
        if (pkgData) {

            $('#SlipNo').val(pkgData.SlipNo);
            $('#DateFormatted').val(pkgData.DateFormatted);
            $('#PackedBy').val(pkgData.PackedBy).select2();
            $('#SalesOrderID').val(pkgData.SalesOrderID).select2();
            $('#IssueToDispatchDateFormatted').val(pkgData.IssueToDispatchDateFormatted);
            $('#TotalPackageWeight').val(pkgData.TotalPackageWeight);
            $('#PackingRemarks').val(pkgData.PackingRemarks);
            $('#CheckedPackageWeight').val(pkgData.CheckedPackageWeight);
            $('#DispatchedBy').val(pkgData.DispatchedBy).select2();
            $('#DispatchedDateFormatted').val(pkgData.DispatchedDateFormatted);
            $('#VehiclePlateNo').val(pkgData.VehiclePlateNo);
            $('#DriverName').val(pkgData.DriverName);
            $('#ReceivedBy').val(pkgData.ReceivedBy);
            $('#ReceivedDateFormatted').val(pkgData.ReceivedDateFormatted);
            $('#DispatchRemarks').val(pkgData.DispatchRemarks);
            ChangeButtonPatchView('PackingSlip', 'divbuttonPatchAddPkgSlip', 'Edit');
            BindPkgSlipDetail();
            OrderDetails();
            $('#SalesOrderID').attr("disabled", true);
        }
    }
    catch (e) {
        notyAlert('error', e.Message);
    }
}
function GetPkgSlipByID(ID) {
    try {
        debugger;
        var id = $('#ID').val();
        var data = { "ID": id };
        var result = "";
        var message = "";
        var jsonData = {};
        var pkgDetailVM = new Object();
        jsonData = GetDataFromServer("PackingSlip/GetPackingSlip/", data);
        if (jsonData != '') {
            jsonData = JSON.parse(jsonData);
            result = jsonData.Result;
            pkgDetailVM = jsonData.Record;
            message = jsonData.Message;
        }
        if (result == "OK") {
            return pkgDetailVM;

        }
        if (result == "ERROR") {
            alert(Message);
        }
    }
    catch (e) {
        //this will show the error msg in the browser console(F12) 
        console.log(e.Message);
    }
}

function BindPkgSlipDetail()
{
    debugger;
    var id = $("#ID").val()
    DataTables.PackingSlipDetailTable.clear().rows.add(GetPkgDetail(id)).draw(true);
}

function GetPkgDetail(id)
{
    try
    {
        debugger;
        var data = { "id": id };
        var jsonData = {};
        var result = "";
        var message = "";
        var pkgDetailVM = new Object();
        jsonData = GetDataFromServer("PackingSlip/GetPackingSlipDetail/", data);
        if (jsonData != '') {
            jsonData = JSON.parse(jsonData);
            result = jsonData.Result;
            message = jsonData.Message;
            pkgDetailVM = jsonData.Records;
        }
        if (result == "OK") {
            return pkgDetailVM;
        }
        if (result == "ERROR") {
            alert(message);
        }
    }
    catch (e)
    {
        notyAlert('error', e.message);
    }
}

//Edit PkgDetail
function EditPkgSlipDetailTable(curObj) {
    debugger;
    $('#EditPkgSlipDetailsModal').modal('show');
    var rowData = DataTables.PackingSlipDetailTable.row($(curObj).parents('tr')).data();
    _SlNo = 1;
    EditPkgSlipDetailByID(rowData.ID)
    EditPOdetailID = rowData.ID// to set PODetailID

}
function EditPkgSlipDetailByID(ID) {
    try {
        debugger;
        _SlNo = 1;
        var EditDetail = GetEditPkgSlipDetail(ID);
        DataTables.PkgSlipDetailEditTable.clear().rows.add(GetEditPkgSlipDetail(ID)).draw(false);
    }
    catch (e) {
        //this will show the error msg in the browser console(F12) 
        console.log(e.message);
    }
}
function GetEditPkgSlipDetail(ID) {
    try {
        debugger;
        var data = { ID };
        var result = "";
        var message = "";
        var jsonData = {};
        var pkgDetailVM = new Object();
        jsonData = GetDataFromServer("PackingSlip/PackingSlipDetailByPackingSlipDetailID/", data);
        if (jsonData != '') {
            jsonData = JSON.parse(jsonData);
            pkgDetailVM = jsonData.Records;
            result = jsonData.Result;
            message = jsonData.Message;
        }
        if (result == "OK") {
            return pkgDetailVM;
        }
        if (result == "ERROR") {
            alert(Message);
        }
    }
    catch (e) {
        //this will show the error msg in the browser console(F12) 
        console.log(e.message);
    }
}
//TextBox value change in EditPkgSlipdatatable
function textBoxValueChangedEditTbl(thisObj, textBoxCode) {
    debugger;
    var productDetailsVM = DataTables.PkgSlipDetailEditTable.rows().data();
    var productDetailstable = DataTables.PkgSlipDetailEditTable;
            if (textBoxCode == 1)//textBoxCode is the code to know, which textbox changed is triggered
            {
                if (thisObj.value <= productDetailsVM[0].SalesOrder.SalesOrderDetail.Product.CurrentStock)
                    productDetailsVM[0].SalesOrder.SalesOrderDetail.CurrentPkgQty = parseFloat(thisObj.value);
                else
                    productDetailsVM[0].SalesOrder.SalesOrderDetail.CurrentPkgQty = parseFloat(productDetailsVM[0].SalesOrder.SalesOrderDetail.Product.CurrentStock) - parseFloat(productDetailsVM[0].SalesOrder.SalesOrderDetail.PrevPkgQty)
            }
            if (textBoxCode == 2)
                productDetailsVM[0].SalesOrder.SalesOrderDetail.PkgWt = parseFloat(thisObj.value);
    DataTables.PkgSlipDetailEditTable.clear().rows.add(productDetailsVM).draw(false);
}
//Add Edited values to packingSlip detail Tbl
function AddPackingSlipDetailEditTbl() {
    _SlNo = 1;
    EditFlag = 1;
    debugger;
    var pkgDetailsVM = DataTables.PkgSlipDetailEditTable.rows().data();
    var res = CheckProductDetailsEditTbl(pkgDetailsVM);//Checking all details are entered or not
    if (res) {
        pkgDetailLink = new Object();
        pkgDetailLink.ProductID = pkgDetailsVM[0].SalesOrder.SalesOrderDetail.ProductID;
        pkgDetailLink.PackingSlipID = pkgDetailsVM[0].ID; //[PKGDetailID]
        pkgDetailLink.Name = pkgDetailsVM[0].SalesOrder.SalesOrderDetail.Product.Name;
        pkgDetailLink.CurrentPkgQty = pkgDetailsVM[0].SalesOrder.SalesOrderDetail.CurrentPkgQty;
        pkgDetailLink.PkgWt = pkgDetailsVM[0].SalesOrder.SalesOrderDetail.PkgWt;
        pkgDetailLink.Qty = pkgDetailsVM[0].SalesOrder.SalesOrderDetail.CurrentPkgQty;
        pkgDetailLink.Weight = pkgDetailsVM[0].SalesOrder.SalesOrderDetail.PkgWt;
        packingDetail.push(pkgDetailLink);// adding values to packingSlipDetail
        _SlNo = 1;
        DataTables.PackingSlipDetailTable.clear().rows.add(packingDetail).draw(false); //binding Detail table with new values   
        //if(res)
        $('#EditPkgSlipDetailsModal').modal('hide');
        Save();
    }
    else
        notyAlert('warning', "Please Enter Packing Quantity");
}
function CheckProductDetailsEditTbl(producDetails) {
    var flag = 0;
    if ((producDetails) && (producDetails.length > 0)) {

        for (var r = 0; r < producDetails.length; r++) {
            if (producDetails[r].SalesOrder.SalesOrderDetail.CurrentPkgQty == 0) {
                flag = 1;
                $("#txt" + producDetails[r].ProductID).attr('style', 'border-color:red;')
            }
        }
        if (flag == 1)
            return false
        else
            return true;
    }
}
//Delete PkgSlipDetail
function Delete(curobj) {
    debugger;
    var rowData = DataTables.PackingSlipDetailTable.row($(curobj).parents('tr')).data();
    var Rowindex = DataTables.PackingSlipDetailTable.row($(curobj).parents('tr')).index();
    _SlNo = 1;
    if ((rowData != null) && (rowData.ID != null)) {
        notyConfirm('Are you sure to delete?', 'DeleteItem("' + rowData.ID + '")');
    }
    else {
        var res = notyConfirm('Are you sure to delete?', 'DeleteTempItem("' + Rowindex + '")');

    }
}

function DeleteTempItem(Rowindex) {
    debugger;
    _SlNo = 1;
    DataTables.PackingSlipDetailTable.row(Rowindex).remove().draw(false);
    notyAlert('success', 'Deleted Successfully');
}
function DeleteItem(ID) {

    try {
        debugger;
        var data = { "ID": ID };
        var ds = {};
        ds = GetDataFromServer("PackingSlip/DeletePackingSlipDetail/", data);
        if (ds != '') {
            ds = JSON.parse(ds);
        }
        switch (ds.Result) {
            case "OK":
                notyAlert('success', ds.Message);
                BindPkgSlip($('#ID').val());
                break;
            case "ERROR":
                notyAlert('error', ds.Message);
                break;
            default:
                break;
        }
        return ds.Record;
    }
    catch (e) {

        notyAlert('error', e.message);
    }
}
//Delete PurchaseOrder
function DeleteClick() {
    debugger;
    notyConfirm('Are you sure to delete?', 'DeletePackingSlip()');
}
function DeletePackingSlip() {
    try {
        debugger;
        var id = $('#ID').val();
        if (id != '' && id != null) {
            var data = { "ID": id };
            var result = "";
            var message = "";
            var jsonData = {};
            jsonData = GetDataFromServer("PackingSlip/DeletePackingSlip/", data);
            if (jsonData != '') {
                jsonData = JSON.parse(jsonData);
                result = jsonData.Result;
                message = jsonData.Message;
            }
            if (result == "OK") {
                notyAlert('success', message);
                window.location.replace("AddPackingSlip?code=SALE");
            }
            if (result == "ERROR") {
                alert(message);
                return 0;
            }
            return 1;
        }
    }
    catch (e) {
        notyAlert('error', e.message);
        return 0;
    }
}
function Reset() {
    BindPkgSlip($("#ID").val());
}