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
var _productListTableData;
var _productListChildTableData;
var _packingSlipDetailList1 = [];
var packingDetail = [];
var _packingSlipDetailList = [];
var _packingSlipViewModel = new Object();
var colorFlag = 0;
var EditFlag = 0, selected = 1;

// while changing ApplyCurrentStock Please unComment/Comment  
//"if (salesOrder.Product.CurrentStock >= Bal)" in GetSalesOrderProductList method SalesOrderRepository
// used for allow negative stock
var ApplyCurrentStock = 1; //1 means current stock is not checked //0 means current stock is checked

var _isEditPkgSlipDetail = 0;
var _currentPkgQtyEdit = 0;
//Grouping 
var CheckedProducts = [];
var CheckedGroups = [];

$(document).ready(function () {
    try {
        $("#PackedBy").select2({
        });
        $("#DispatchedBy").select2({
        });
        if ($('#IsUpdate').val() == "False") {
            $("#SalesOrderID").select2({
            });
            $('#listSaleOrder').show();
            $('#lblSaleOrder').hide();
        }
        else {
            $('#listSaleOrder').hide();
            $('#lblSaleOrder').show();
        }
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
            "aoColumnDefs": [
                { "targets": [9], "visible": false, "searchable": false },
                { className: "text-right", "targets": [3, 4, 5, 6] }
                            , {
                                "aTargets": [0], "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                                    if (sData.GroupID) {
                                        $(nTd).addClass('details-control')
                                    }
                                }
                            },

            ],
            columns: [
                 { "data": null, "defaultContent": "", "width": "5%" },
                 {
                     "data": "Checkbox", "defaultContent": "", "width": "5%",
                     "render": function (data, type, row) {
                         if (row.GroupName != null) {
                             return '<input type="checkbox" id="' + row.GroupIdString + '" name="grouprow" style="vertical-align: -3px;margin-left: 20%;" onchange="CheckCategory(this)">';
                         }
                         else {
                             return '<input type="checkbox" id="' + row.ProductIdString + '" name="grouprow" style="vertical-align: -3px;margin-left: 20%;"  onchange="CheckCategory(this)" >'; //
                         }
                     }
                 },
                 {
                     "data": "Product.Name", "defaultContent": "<i>-</i>", "width": "40%",
                     'render': function (data, type, row) {
                         if (row.Product.Name != null)
                             return row.Product.Name
                         else
                             return '<input class="form-control description" id="GroupName_' + row.GroupID + '" name="Markup" value="' + row.GroupName + '" type="text" style="width:100%" onkeyup="textBoxValueOnChanged(this,1);">';
                     }
                 },
                 {
                     "data": "Product.CurrentStock", "defaultContent": "<i>-</i>", "width": "10%",
                     'render': function (data, type, row) {
                         if (row.Product.Name != null)
                             //   return data;
                         {
                             if (data <= 0)
                                 return '<i class="lblrequired">' + data + '</i>';
                             else
                                 return data;
                         }
                         else
                             return ' <i data-toggle="tooltip" title="Expand Group to view!">...</i>'

                     }
                 },
                 {
                     "data": "Quantity", "defaultContent": "<i>-</i>", "width": "10%",
                     'render': function (data, type, row) {
                         //  if (row.Product.Name != null)
                         return data;
                         // else
                         //      return '...'
                     }
                 },
                 {
                     "data": "PrevPkgQty", "defaultContent": "<i>-</i>", "width": "10%",
                     'render': function (data, type, row) {
                         if (row.Product.Name != null)
                             return data;
                         else
                             return ' <i data-toggle="tooltip" title="Expand Group to view!">...</i>'

                     }
                 },
                 {
                     "data": "ProductID", "defaultContent": "<i>-</i>", "width": "10%",
                     'render': function (data, type, row) {
                         if (row.Product.Name != null)
                             return BalQty = row.Quantity - row.PrevPkgQty;
                         else
                             return ' <i data-toggle="tooltip" title="Expand Group to view!">...</i>'
                     }
                 },
                 {
                     "data": "CurrentPkgQty", "defaultContent": "<i>-</i>", "width": "10%",
                     'render': function (data, type, row) {
                         debugger;
                         if (row.GroupID == EmptyGuid || row.GroupID == null)
                             return '<input class="form-control description text-right" name="Markup" value="' + data + '" onclick="SelectAllValue(this);" onkeypress = "return isNumber(event)", type="text" id="txt2_' + row.ProductID + '"  onkeyup="textBoxValueOnChanged(this,2);"style="width:100%">';
                         else
                             return '<input class="form-control description text-right" name="Markup" value="' + data + '" onclick="SelectAllValue(this);" onkeypress = "return isNumber(event)", type="text" id="txt2_' + row.GroupID + '"  onkeyup="textBoxValueOnChanged(this,2);"style="width:100%" >';//disabled="disabled"
                     }
                 },
                 {
                     "data": "PkgWt", "defaultContent": "<i>-</i>", "width": "10%",
                     'render': function (data, type, row) {
                         if (row.GroupID == EmptyGuid || row.GroupID == null)
                             return '<input class="form-control description text-right" name="Markup" value="' + data + '" onclick="SelectAllValue(this);" onkeypress = "return isNumber(event)", type="text" id="txt3_' + row.ProductID + '"   onkeyup="textBoxValueOnChanged(this,3);"style="width:100%">';
                         else
                             return '<input class="form-control description text-right" name="Markup" value="' + data + '" onclick="SelectAllValue(this);" onkeypress = "return isNumber(event)", type="text" id="txt3_' + row.GroupID + '"  onkeyup="textBoxValueOnChanged(this,3);"style="width:100%" >';//disabled="disabled"
                     }
                 },
                 { "data": "ChildCount", "defaultContent": "<i>-</i>" },
            ],
            destroy: true
        });
        // Add event listener for opening and closing details
        $('#tblProductList tbody').on('click', 'td.details-control', function () {

            var rowData = DataTables.ProductListTable.row($(this).parents('tr')).data();

            var tr = $(this).closest('tr');
            var row = DataTables.ProductListTable.row(tr);
            if (row.child.isShown()) {
                // This row is already open - close it
                row.child.hide();
                tr.removeClass('shown');
            }
            else {
                //Open this row
                //if (row.child() == undefined)
                //{ 
                row.child(formatPackingSlipProductGroup(row.data()), style = "padding-left:5%").show();
                ApplyProductChildDatatable(row);
                //}
                //else
                //{
                //    row.child.show();
                //    //check the checkbox here
                //    if ($('input[id=G_' + rowData.GroupID + ']').is(':checked'))
                //    {
                //        $('input[parent=G_' + rowData.GroupID + ']').each(function () {
                //            $(this).prop('checked', true);
                //        });
                //    }
                //    else if ($('input[id=G_' + rowData.GroupID + ']').prop("indeterminate"))
                //    {
                //       // var int=0; //do nothing;
                //    }
                //    else
                //    {
                //        $('input[parent=G_' + rowData.GroupID + ']').each(function () {
                //            $(this).prop('checked', false);
                //        });
                //    }
                //}
                tr.addClass('shown');
            }
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
         columnDefs: [{ "targets": [0, 1], "visible": false, searchable: false },
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
                         return '<input class="form-control description text-right" name="Markup" value="' + data + '" onclick="SelectAllValue(this);" onkeypress = "return isNumber(event)", type="text" id="Edttxt2_' + row.SalesOrder.SalesOrderDetail.ProductID + '" onchange="textBoxValueChangedEditTbl(this,1);"style="width:100%">';
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
                  { className: "text-right", "targets": [3, 4, 5, 6, 7] }
                , { className: "text-left", "targets": [2] }
                , { "targets": [0, 1], "visible": false, "searchable": false }
                , { "targets": [2, 3, 4, 5, 6], "bSortable": false }],
            destroy: true
        });

        //EditPackingSlip
        if ($('#ID').val() != EmptyGuid) {
            BindPkgSlip($('#ID').val());
        }
    }
    catch (e) {
        console.log(e.message);
    }
});

function AddPackingSlipDetail() {
    debugger;
    CheckedProducts = [];
    CheckedGroups = [];
    if ($('#IsUpdate').val() == "False") {
        var saleno = $("#SalesOrderID option:selected").text();
    }
    else {
        var saleno = $("#OrderId").val();
    }
    var saleOrderNo=saleno.split("-");
    $('#modelContextLabel').text('Add Packing Slip Details');
    if ($('#DateFormatted').val() && $('#hdnSalesOrderID').val()) {
        debugger;
        $('#lblOrderNo').text("SalesOrder# : " + saleOrderNo[0]);
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
    try {
        if($("#IsUpdate").val()=="False")
            $('#hdnSalesOrderID').val($('#SalesOrderID').val());

        var saleId = $('#hdnSalesOrderID').val();
        var SalesOrderVm = GetOrderDetails(saleId);
        $('#lblCustomer').text(SalesOrderVm.CustomerName);
        $('#lblExpDate').text(SalesOrderVm.ExpectedDeliveryDateFormatted);
    }
    catch (e) {
        console.log(e.message);
    }
}
function GetOrderDetails(saleId) {
    try {
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
        var id = $('#hdnSalesOrderID').val();
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
//function textBoxValueChanged(thisObj, textBoxCode) {
//    debugger;

//    var IDs = selectedRowIDs();//identify the selected rows 
//    var productDetailsVM = DataTables.ProductListTable.rows().data();
//    var productDetailstable = DataTables.ProductListTable;
//    var rowtable = productDetailstable.row($(thisObj).parents('tr')).data();
//    for (var i = 0; i < productDetailsVM.length; i++) {
//        if (productDetailsVM[i].ProductID == rowtable.ProductID) {
//            if (textBoxCode == 1)//textBoxCode is the code to know, which textbox changed is triggered
//                {
//                if (((thisObj.value <= productDetailsVM[i].Product.CurrentStock)||ApplyCurrentStock) && (thisObj.value != ""))
//                    productDetailsVM[i].CurrentPkgQty = parseFloat(thisObj.value);
//                else
//                    productDetailsVM[i].CurrentPkgQty = parseFloat(productDetailsVM[i].Product.CurrentStock) - parseFloat(productDetailsVM[i].PrevPkgQty)
//            }
//            if (textBoxCode == 2)
//            {
//                if (thisObj.value != "")
//                    productDetailsVM[i].PkgWt = parseFloat(thisObj.value);
//                else
//                    productDetailsVM[i].PkgWt = 0;
//            }
//        }
//    }
//    debugger;

//    DataTables.ProductListTable.clear().rows.add(productDetailsVM).draw(false);
//    selectCheckbox(IDs); //Selecting the checked rows with their ids taken 
//    productDetailsVM = DataTables.ProductListTable.rows(".selected").data();
//    CheckProductDetails(productDetailsVM);
//}
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
    var res = CheckProductList(productDetailsVM);//Checking all details are entered or not
    if (selected == 1)
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
function CheckProductDetails(producDetails) {
    var flag = 0;
    if ((producDetails) && (producDetails.length > 0)) {
        selected = 1;
        for (var r = 0; r < producDetails.length; r++) {
            if (producDetails[r].CurrentPkgQty == 0) {
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
function CheckProductList(producDetails) {
    if (producDetails.length == 0) {
        notyAlert('warning', "Please Select Product");
        selected = 0;
        return false;
    }
    var result = CheckProductDetails(producDetails);
    return result;
}
function AddPackingSlipDetailData(data) {
    var flag = 0;
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
    else {
        notyAlert('warning', "Please Select Product");
        return false;
    }
}
//Save
function Save() {
    debugger
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
            debugger;
            var SlipDetailCount = DataTables.PackingSlipDetailTable.rows().count();
            if (CheckedGroups.length > 0 || CheckedProducts.length > 0 || SlipDetailCount > 0) {
                _packingSlipViewModel.ID = $('#ID').val();
                _packingSlipViewModel.SlipNo = $('#SlipNo').val();
                _packingSlipViewModel.Date = $('#DateFormatted').val();
                _packingSlipViewModel.IssueToDispatchDate = $('#IssueToDispatchDateFormatted').val();
                _packingSlipViewModel.SalesOrderID = $('#hdnSalesOrderID').val();
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
                _SlNo = 1;
                var result;
                $("#GroupDetailJSON").val('');
                if (CheckedGroups.length > 0) {
                    result = JSON.stringify(CheckedGroups);
                    $("#GroupDetailJSON").val(result);
                }
                _packingSlipViewModel.GroupDetailJSON = $("#GroupDetailJSON").val();

                $("#ProductDetailJSON").val('');
                if (CheckedProducts.length > 0) {
                    result = JSON.stringify(CheckedProducts);
                    $("#ProductDetailJSON").val(result);
                }
                _packingSlipViewModel.ProductDetailJSON = $("#ProductDetailJSON").val();

                var data = "{'packingSlipVM':" + JSON.stringify(_packingSlipViewModel) + "}";
                PostDataToServer("PackingSlip/InsertUpdatePackingSlip/", data, function (JsonResult) {
                    switch (JsonResult.Result) {
                        case "OK":
                            notyAlert('success', JsonResult.Records.Message);
                            ChangeButtonPatchView('PackingSlip', 'divbuttonPatchAddPkgSlip', 'Edit');
                            if (JsonResult.Records.ID) {
                                $("#ID").val(JsonResult.Records.ID);
                                $('#IsUpdate').val('True');
                                BindPkgSlip($("#ID").val());
                            } else {
                                Reset();
                            }
                            packingDetail = [];
                            CheckedProducts = [];
                            CheckedGroups = [];
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
                notyAlert('warning', "Item list is empty");
            }
        }
        else {
            notyAlert('warning', "Please Fill Required Fields ");
        }
    }
}
//PackingSlipDetail data 
function AddPackingSlipDetailList() {
    if (EditFlag == 0) {
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
    else {
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
        _SlNo = 1;
        var pkgData = GetPkgSlipByID(ID)
        if (pkgData) {
            debugger;
            $('#listSaleOrder').hide();
            $('#lblSaleOrder').show();
            $('#SlipNo').val(pkgData.SlipNo);
            $('#DateFormatted').val(pkgData.DateFormatted);
            $('#PackedBy').val(pkgData.PackedBy).select2();
            $('#OrderId').val(pkgData.SalesOrder.OrderNo + "-" + pkgData.SalesOrder.CustomerName);
            $('#hdnSalesOrderID').val(pkgData.SalesOrderID);
            $('#IssueToDispatchDateFormatted').val(pkgData.IssueToDispatchDateFormatted);
            $('#TotalPackageWeight').val(pkgData.TotalPackageWeight);
            $('#lblPaySlipNo').text("Slip # :" + pkgData.SlipNo);
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
            $('#OrderId').attr("disabled", true);
        }
    }
    catch (e) {
        notyAlert('error', e.Message);
    }
}
function GetPkgSlipByID(ID) {
    try {
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

function BindPkgSlipDetail() {
    debugger;
    var id = $("#ID").val()
    DataTables.PackingSlipDetailTable.clear().rows.add(GetPkgDetail(id)).draw(true);
}

function GetPkgDetail(id) {
    try {
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
    catch (e) {
        notyAlert('error', e.message);
    }
}

//Edit PkgDetail
function EditPkgSlipDetailTable(curObj) {
    _isEditPkgSlipDetail = 1;
    CheckedGroups = [];
    CheckedProducts = [];
    var rowData = DataTables.PackingSlipDetailTable.row($(curObj).parents('tr')).data();
    _SlNo = 1;
    if (rowData.GroupID == EmptyGuid) {
        $('#EditPkgSlipDetailsModal').modal('show');
        EditPkgSlipDetailByID(rowData.ID)
    }
    if (rowData.GroupID != EmptyGuid) {
        $('#PackingSlipModal').modal('show');
        BindEditGroupProductListTable(rowData.GroupID);
    }

}
function EditPkgSlipDetailByID(ID) {
    try {
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
        var currStock = productDetailsVM[0].SalesOrder.SalesOrderDetail.Product.CurrentStock;
        var balPkgQty = productDetailsVM[0].SalesOrder.SalesOrderDetail.Quantity - productDetailsVM[0].SalesOrder.SalesOrderDetail.PrevPkgQty;
        // var currPkgQty = productDetailsVM[0].SalesOrder.SalesOrderDetail.CurrentPkgQty;//has to tak bal packing qty right
        var prodID = productDetailsVM[0].SalesOrder.SalesOrderDetail.ProductID;

        //currStock is negative  tak as 0
        if (currStock < 0)
            currStock = 0;
        
        if ((parseFloat(thisObj.value) <= currStock || ApplyCurrentStock) && parseFloat(thisObj.value) <= balPkgQty) {
            if (currStock > balPkgQty || ApplyCurrentStock)
                productDetailsVM[0].SalesOrder.SalesOrderDetail.CurrentPkgQty = parseFloat(thisObj.value);
            else {
                productDetailsVM[0].SalesOrder.SalesOrderDetail.CurrentPkgQty = currStock;
                $('#Edttxt2_' + prodID).val(currStock);
            }
        }
        else {
            if (currStock > balPkgQty || ApplyCurrentStock) {
                productDetailsVM[0].SalesOrder.SalesOrderDetail.CurrentPkgQty = balPkgQty;
                $('#Edttxt2_' + prodID).val(balPkgQty);
            }
            else {
                productDetailsVM[0].SalesOrder.SalesOrderDetail.CurrentPkgQty = currStock;
                $('#Edttxt2_' + prodID).val(currStock);
            }
        }
    }
    if (textBoxCode == 2)
        productDetailsVM[0].SalesOrder.SalesOrderDetail.PkgWt = parseFloat(thisObj.value);
    DataTables.PkgSlipDetailEditTable.clear().rows.add(productDetailsVM).draw(false);
}
//Add Edited values to packingSlip detail Tbl
function AddPackingSlipDetailEditTbl() {
    _SlNo = 1;
    EditFlag = 1;
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
    var rowData = DataTables.PackingSlipDetailTable.row($(curobj).parents('tr')).data();
    var Rowindex = DataTables.PackingSlipDetailTable.row($(curobj).parents('tr')).index();
    var isGroupItem = 0;
    if (rowData.ID == rowData.GroupID)
        isGroupItem = 1;
    _SlNo = 1;
    if ((rowData != null) && (rowData.ID != null)) {
        notyConfirm('Are you sure to delete?', 'DeleteItem("' + rowData.ID + '","' + isGroupItem + '")');
    }
    else {
        var res = notyConfirm('Are you sure to delete?', 'DeleteTempItem("' + Rowindex + '")');

    }
}

function DeleteTempItem(Rowindex) {
    _SlNo = 1;
    DataTables.PackingSlipDetailTable.row(Rowindex).remove().draw(false);
    notyAlert('success', 'Deleted Successfully');
}
function DeleteItem(ID, isGroupItem) {

    try {
        var data = { "ID": ID, "isGroupItem": isGroupItem };
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
    notyConfirm('Are you sure to delete?', 'DeletePackingSlip()');
}
function DeletePackingSlip() {
    try {
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
                notyAlert('error', message);
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

//---------------------------------Grouping---------------------------------------//
/* Formatting function for row details - modify as you need */
function formatPackingSlipProductGroup(d) {
    return '<table id="ProductChildTable' + d.GroupID + '" class="table table-striped table-bordered table-hover" style="width:90%" > ' +
    ' <thead>' +
    '<tr>' +
        '<th></th>' +
        '<th>Product</th>' +
        ' <th>Current Stock</th>' +
        ' <th>Sale Order Qty (A)</th>' +
        ' <th>Prev. Pckd. Qty (B)</th>' +
        '<th>Bal. To Pck. Qty (A-B)</th>' +
        '<th>Current Pkg. Qty</th>' +
        ' <th>Current Pkg. Weight (KGs)</th>' +
    '</tr>' +
    '</thead>' +
    ' </table>'
}

function ApplyProductChildDatatable(row) {

    DataTables.ProductChildTable = $('#ProductChildTable' + row.data().GroupID + '').DataTable(
         {
             dom: '<"pull-right"f>rt<"bottom"ip><"clear">',
             ordering: false,
             searching: false,
             paging: false,
             "bInfo": false,
             lengthChange: false,
             scrollCollapse: true,
             data: GetProductListForPackingSlipByGroupID(row.data().GroupID),
             columns: [
                     {
                         "data": "Checkbox", "defaultContent": "", "width": "5%",
                         "render": function (data, type, row, meta) {
                             var groupChecked = false;
                             CheckedProducts.forEach(function (element) {
                                 if (row.ProductID == element.ProductID) {
                                     groupChecked = true;
                                 }
                             });
                             var currentEnteredValue = $('#txt2_' + row.GroupID).val();
                             if ((currentEnteredValue <= row.Product.CurrentStock || ApplyCurrentStock) && currentEnteredValue <= (row.Quantity - row.PrevPkgQty)) {
                                 if (groupChecked) {
                                     return '<input parent="G_' + row.GroupID + '" type="checkbox" id="' + row.ProductIdString + '" name="unitrow" style="vertical-align: -3px;margin-left: 20%;"  onchange="CheckCategory(this)"  checked >';
                                 }
                                 else {
                                     return '<input parent="G_' + row.GroupID + '" type="checkbox" id="' + row.ProductIdString + '" name="unitrow" style="vertical-align: -3px;margin-left: 20%;"  onchange="CheckCategory(this)" >';
                                 }
                             }
                             else {
                                 remove(CheckedProducts, row.ProductIdString);
                                 //remove group also if all group items removed from CheckedProducts
                                 if (CheckedProducts.findIndex(x => x.GroupID === row.GroupID) == -1) {
                                     remove(CheckedGroups, 'G_' + row.GroupID);
                                     $('input[id=G_' + row.GroupID + ']').prop('indeterminate', false);
                                     $('input[id=G_' + row.GroupID + ']').prop('checked', false);
                                 }
                                 return '';
                             }
                         }
                     },
                     {
                         "data": "Product.Name", "defaultContent": "<i>-</i>", "width": "30%",
                         'render': function (data, type, row) {
                             var currentEnteredValue = $('#txt2_' + row.GroupID).val();
                             if (currentEnteredValue <= row.Product.CurrentStock && currentEnteredValue <= (row.Quantity - row.PrevPkgQty)) {
                                 return data;
                             }
                             else {
                                 if ((row.Quantity - row.PrevPkgQty) == 0)
                                     return data;
                                 else
                                     return '<i class="lblrequired">' + data + '</i>';
                             }
                         }
                     },
                     {
                         "data": "Product.CurrentStock", "defaultContent": "<i>-</i>", "width": "10%"
                        , 'render': function (data, type, row) {
                            var currentEnteredValue = $('#txt2_' + row.GroupID).val();
                            if (currentEnteredValue <= row.Product.CurrentStock && currentEnteredValue <= (row.Quantity - row.PrevPkgQty)) {
                                return data;
                            }
                            else {
                                if ((row.Quantity - row.PrevPkgQty) == 0)
                                    return data;
                                else
                                    return '<i class="lblrequired">' + data + '</i>';
                            }
                        }
                     },
                     { "data": "Quantity", "defaultContent": "<i>-</i>", "width": "10%" },
                     { "data": "PrevPkgQty", "defaultContent": "<i>-</i>", "width": "10%" },
                     {
                         "data": "BalQty", "defaultContent": "<i>-</i>", "width": "10%",
                         'render': function (data, type, row) {
                             var currentEnteredValue = $('#txt2_' + row.GroupID).val();
                             var balQty = (row.Quantity - row.PrevPkgQty);
                             if (currentEnteredValue <= row.Product.CurrentStock && currentEnteredValue <= (row.Quantity - row.PrevPkgQty)) {
                                 return balQty;
                             }
                             else {
                                 if (balQty == 0)
                                     return balQty;
                                 else
                                     return '<i class="lblrequired">' + balQty + '</i>';
                             }
                         }
                     },
                     {
                         "data": "CurrentPkgQty", "defaultContent": "<i>-</i>", "width": "10%",
                         'render': function (data, type, row) {
                             var productChecked;
                             CheckedProducts.forEach(function (element) {
                                 if (row.ProductID == element.ProductID) {
                                     productChecked = true;
                                 }
                             });
                             if (productChecked) {
                                 return $('#txt2_' + row.GroupID).val();
                             }
                             else {
                                 return 0;
                             }
                         }
                     },
                     {
                         "data": "PkgWt", "defaultContent": "<i>-</i>", "width": "15%",
                         'render': function (data, type, row) {
                             return $('#txt3_' + row.GroupID).val();
                         }
                     }
             ],
             columnDefs:
                 [{ "targets": [7], "visible": false, "searchable": false },
                 { className: "text-right", "targets": [2, 3, 4, 5, 6] },
                 { className: "text-left", "targets": [] },
                 { className: "text-center", "targets": [] }],
         });
}

//--To  Get Product List For Packing Slip By GroupID from server --// 
function GetProductListForPackingSlipByGroupID(groupID) {
    try {
        var id = $('#hdnSalesOrderID').val();
        var packingSlipID = $('#ID').val();
        var data = { "id": id, "packingSlipID": packingSlipID, "groupID": groupID };
        var ds = {};
        ds = GetDataFromServer("PackingSlip/GetProductListForPackingSlipByGroupID/", data);     
        if (ds != '') {
            ds = JSON.parse(ds);
        }
        if (ds.Result == "OK") {
            return ds.Records;
        }
        if (ds.Result == "ERROR") {

            notyAlert('error', ds.Message);
        }
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}

function CheckCategory(this_Obj) {
    switch (this_Obj.name) {
        case "grouprow":
            if ($(this_Obj).is(":checked")) {
                var groupQtyCheck;
                var checkedItemsCount = 0;
                var rowData = DataTables.ProductListTable.row($(this_Obj).parents('tr')).data();
                var childCount = rowData.ChildCount;



                if (this_Obj.id.search('G_') !== -1) {
                    var productExists = 0;
                    //find child rows with group id and add 
                    var packingSlipDetailVM;
                    if ($('#ProductChildTable' + rowData.GroupID).DataTable().rows.length == 0)
                        packingSlipDetailVM = GetProductListForPackingSlipByGroupID(rowData.GroupID);
                    else
                        packingSlipDetailVM = $('#ProductChildTable' + rowData.GroupID).DataTable().rows.data();

                    groupQtyCheck = rowData.CurrentPkgQty; //to check while child rows added
                    for (i = 0; i < packingSlipDetailVM.length ; i++) {
                        if ((groupQtyCheck <= packingSlipDetailVM[i].Product.CurrentStock || ApplyCurrentStock) && groupQtyCheck <= packingSlipDetailVM[i].Quantity - packingSlipDetailVM[i].PrevPkgQty) {
                            productExists = productExists + 1;
                            packingSlipDetail = new Object();
                            packingSlipDetail.ProductID = packingSlipDetailVM[i].ProductID == null ? EmptyGuid : packingSlipDetailVM[i].ProductID;
                            packingSlipDetail.GroupID = packingSlipDetailVM[i].GroupID == null ? EmptyGuid : packingSlipDetailVM[i].GroupID;
                            packingSlipDetail.RemoveID = packingSlipDetailVM[i].ProductIdString;
                            packingSlipDetail.GroupName = packingSlipDetailVM[i].GroupName;
                            packingSlipDetail.ID = EmptyGuid;
                            packingSlipDetail.Qty = packingSlipDetailVM[i].CurrentPkgQty;                          
                            packingSlipDetail.CurrentStock = packingSlipDetailVM[i].Product.CurrentStock
                            packingSlipDetail.Weight = packingSlipDetailVM[i].PkgWt;
                            packingSlipDetail.BalQuantity = packingSlipDetailVM[i].Quantity - packingSlipDetailVM[i].PrevPkgQty
                            CheckedProducts.findIndex(x => x.RemoveID === packingSlipDetailVM[i].ProductIdString) == -1 ? CheckedProducts.push(packingSlipDetail) : "";
                            //CheckedProducts.push(packingSlipDetail);
                        }
                    }
                    /*.....................add Group row items to CheckedGroups as PackingslipDetail Object if vaild product exists...................*/
                    if (productExists != 0) {
                        packingSlipDetail = new Object();
                        packingSlipDetail.ProductID = rowData.ProductID == null ? EmptyGuid : rowData.ProductID;
                        packingSlipDetail.GroupID = rowData.GroupID == null ? EmptyGuid : rowData.GroupID;
                        packingSlipDetail.RemoveID = rowData.GroupIdString;
                        packingSlipDetail.GroupName = rowData.GroupName;
                        packingSlipDetail.ID = EmptyGuid;
                        packingSlipDetail.Qty = rowData.CurrentPkgQty;
                        packingSlipDetail.Weight = rowData.PkgWt;
                        CheckedGroups.findIndex(x => x.RemoveID === rowData.GroupIdString) == -1 ? CheckedGroups.push(packingSlipDetail) : "";
                        // CheckedGroups.push(packingSlipDetail);
                    }

                    $('#ProductChildTable' + rowData.GroupID).DataTable().clear().rows.add(GetProductListForPackingSlipByGroupID(rowData.GroupID)).draw(false);


                    //proper checkbox checking after vaildation if table exists

                    checkedItemsCount = productExists;//$('input[parent=' + rowData.GroupIdString + ']:checked').length;
                    if (checkedItemsCount == 0) {
                        $('input[id=' + rowData.GroupIdString + ']').prop('indeterminate', false);
                        $('input[id=' + rowData.GroupIdString + ']').prop('checked', false);
                    }
                    else if (childCount > checkedItemsCount)
                        $('input[id=' + rowData.GroupIdString + ']').prop('indeterminate', true);
                    else if (childCount == checkedItemsCount) {
                        $('input[id=' + rowData.GroupIdString + ']').prop('indeterminate', false);
                        $('input[id=' + rowData.GroupIdString + ']').prop('checked', true);
                    }

                }

                if (this_Obj.id.search('P_') !== -1) {
                    /*.....................add this items to CheckedProducts as PackingslipDetail Object...................*/
                    packingSlipDetail = new Object();
                    packingSlipDetail.ProductID = rowData.ProductID == null ? EmptyGuid : rowData.ProductID;
                    packingSlipDetail.GroupID = rowData.GroupID == null ? EmptyGuid : rowData.GroupID;
                    packingSlipDetail.RemoveID = rowData.ProductIdString;
                    packingSlipDetail.GroupName = rowData.GroupName;
                    packingSlipDetail.ID = EmptyGuid;
                    packingSlipDetail.Qty = rowData.CurrentPkgQty;
                    packingSlipDetail.Weight = rowData.PkgWt;
                    // CheckedProducts.findIndex(x => x.RemoveID === rowData.ProductIdString) !== -1 ? CheckedProducts.push(packingSlipDetail) : "";
                    CheckedProducts.push(packingSlipDetail);
                }
            }
            else {
                var rowData = DataTables.ProductListTable.row($(this_Obj).parents('tr')).data();
                if (this_Obj.id.search('G_') !== -1) {
                    //'Group items removed'
                    remove(CheckedGroups, this_Obj.id)
                    //$('input[parent=' + this_Obj.id + ']').each(function () {
                    //    $(this).prop('checked', false);
                    //    remove(CheckedProducts, this.id)
                    //});
                    for (k = 0; k < rowData.ChildCount; k++) {//removing all product when group unchecked
                        removeProductsList(CheckedProducts, rowData.GroupID);
                    }
                    $('#ProductChildTable' + rowData.GroupID).DataTable().clear().rows.add(GetProductListForPackingSlipByGroupID(rowData.GroupID)).draw(false);
                }
                if (this_Obj.id.search('P_') !== -1) {
                    //'Product removed'
                    remove(CheckedProducts, this_Obj.id);
                }
            }
            break;
        case "unitrow":
            var childCount = DataTables.ProductListTable.row($('#' + $('#' + this_Obj.id).attr("parent")).parents('tr')).data().ChildCount;
            var guid_GroupID = DataTables.ProductListTable.row($('#' + $('#' + this_Obj.id).attr("parent")).parents('tr')).data().GroupID;
            var groupID = $(this_Obj).attr("parent");//'G_4e87a14-3679-4138-bba2-2c65e8eb5a84'
            var checkedItemsCount = 0;
            checkedItemsCount = $('input[parent=' + groupID + ']:checked').length;
            var rowData = $('#ProductChildTable' + guid_GroupID).DataTable().row($(this_Obj).parents('tr')).data();
            var rowDataParent = DataTables.ProductListTable.row($('#' + $('#' + this_Obj.id).attr("parent")).parents('tr')).data();

            if ($(this_Obj).is(":checked")) {
                if (this_Obj.id.search('C_') !== -1) {
                    packingSlipDetail = new Object();
                    packingSlipDetail.ProductID = rowData.ProductID == null ? EmptyGuid : rowData.ProductID;
                    packingSlipDetail.GroupID = rowData.GroupID == null ? EmptyGuid : rowData.GroupID;
                    packingSlipDetail.RemoveID = rowData.ProductIdString;
                    packingSlipDetail.GroupName = rowData.GroupName;
                    packingSlipDetail.ID = EmptyGuid;
                    packingSlipDetail.Qty = rowData.CurrentPkgQty;
                    packingSlipDetail.CurrentStock = rowData.Product.CurrentStock;
                    packingSlipDetail.Weight = rowData.PkgWt;
                    packingSlipDetail.BalQuantity = rowData.Quantity - rowData.PrevPkgQty;
                    // CheckedProducts.findIndex(x => x.RemoveID === rowData.ProductIdString)==-1?CheckedProducts.push(packingSlipDetail):"";
                    CheckedProducts.push(packingSlipDetail);

                    if (childCount > checkedItemsCount)
                        $('input[id=' + groupID + ']').prop('indeterminate', true);
                    else if (childCount == checkedItemsCount) {
                        $('input[id=' + groupID + ']').prop('indeterminate', false);
                        $('input[id=' + groupID + ']').prop('checked', true);
                    }
                    //add grouphead  
                    packingSlipDetail = new Object();
                    packingSlipDetail.ProductID = rowDataParent.ProductID;
                    packingSlipDetail.GroupID = rowDataParent.GroupID;
                    packingSlipDetail.RemoveID = rowDataParent.GroupIdString;
                    packingSlipDetail.GroupName = rowDataParent.GroupName;
                    packingSlipDetail.ID = EmptyGuid;
                    packingSlipDetail.Qty = rowDataParent.CurrentPkgQty;
                    packingSlipDetail.CurrentStock = rowDataParent.Product.CurrentStock
                    packingSlipDetail.Weight = rowDataParent.PkgWt;
                    // checking grouphead is existing or not  
                    CheckedGroups.findIndex(x => x.RemoveID === rowDataParent.GroupIdString) == -1 ? CheckedGroups.push(packingSlipDetail) : "";
                    //CheckedGroups.push(packingSlipDetail);
                    $('#ProductChildTable' + rowDataParent.GroupID).DataTable().clear().rows.add(GetProductListForPackingSlipByGroupID(rowDataParent.GroupID)).draw(false);
                }
            }
            else {

                //'Product removed'
                remove(CheckedProducts, this_Obj.id);

                if (this_Obj.id.search('C_') !== -1) {
                    if (0 < checkedItemsCount && checkedItemsCount < childCount)
                        $('input[id=' + groupID + ']').prop('indeterminate', true);
                    else if (checkedItemsCount == 0) {
                        $('input[id=' + groupID + ']').prop('indeterminate', false);
                        $('input[id=' + groupID + ']').prop('checked', false);
                        //have to remove the group of this 
                        remove(CheckedGroups, groupID);
                    }
                }
                $('#ProductChildTable' + rowDataParent.GroupID).DataTable().clear().rows.add(GetProductListForPackingSlipByGroupID(rowDataParent.GroupID)).draw(false);

            }
            break;
        default:
            "";
    }
}

function remove(arr, what) {
    arr.forEach(function (element) {
        if (element.RemoveID == what) {
            arr.splice(arr.indexOf(element), 1);
        }
    });
}
function removeProductsList(arr, what) {
    arr.forEach(function (element) {
        if (element.GroupID == what) {
            arr.splice(arr.indexOf(element), 1);
        }
    });
}


function textBoxValueOnChanged(curObj, textboxNo) {
    debugger;
    var rowData = DataTables.ProductListTable.row($(curObj).parents('tr')).data();
    var rowindex = DataTables.ProductListTable.row($(curObj).parents('tr')).index();
    _productListTableData = DataTables.ProductListTable.rows().data();//parent Table

    if (curObj.value != "") {

        if (rowData.GroupID == EmptyGuid || rowData.GroupID == null) {//Case Product row(Parent Table)
            for (i = 0; i < _productListTableData.data().count() ; i++) {
                if (_productListTableData[i].ProductID == rowData.ProductID) {
                    if (textboxNo == 2)//textboxNo is the code to know, which textbox changed is triggered
                    {
                        var balOrderQty = parseFloat(_productListTableData[i].Quantity) - parseFloat(_productListTableData[i].PrevPkgQty)
                        if ((parseFloat(curObj.value) <= _productListTableData[i].Product.CurrentStock || ApplyCurrentStock) && parseFloat(curObj.value) <= balOrderQty) {
                            if (rowData.Product.CurrentStock > balOrderQty || ApplyCurrentStock)
                                _productListTableData[i].CurrentPkgQty = parseFloat(curObj.value);
                            else {
                                _productListTableData[i].CurrentPkgQty = rowData.Product.CurrentStock;
                                $('#txt2_' + rowData.ProductID).val(rowData.Product.CurrentStock);
                            }
                        }
                        else {
                            if (rowData.Product.CurrentStock > balOrderQty || ApplyCurrentStock) {
                                _productListTableData[i].CurrentPkgQty = balOrderQty;
                                $('#txt2_' + rowData.ProductID).val(balOrderQty);
                            }
                            else {
                                _productListTableData[i].CurrentPkgQty = rowData.Product.CurrentStock;
                                $('#txt2_' + rowData.ProductID).val(rowData.Product.CurrentStock);
                            }
                        }
                    }
                    if (textboxNo == 3) {
                        _productListTableData[i].PkgWt = parseFloat(curObj.value);
                    }
                }
            }
            // DataTables.ProductListTable.clear().rows.add(_productListTableData).draw(false);
        }
        else {// case  Group Head row (Parent Table)
            var productChildTable = $('#ProductChildTable' + rowData.GroupID).DataTable().rows().data();
            if (textboxNo != 1) {
                if (!_isEditPkgSlipDetail)
                    var balOrderQty = parseFloat(rowData.Quantity) - parseFloat(rowData.PrevPkgQty)
                else
                    var balOrderQty = parseFloat(rowData.Quantity) - parseFloat(rowData.PrevPkgQty) + parseFloat(_currentPkgQtyEdit)

                if (productChildTable.data().count() > 0) {//while Child Table Exisitng
                    for (i = 0; i < productChildTable.data().count() ; i++) {


                        if (textboxNo == 2)//textboxNo is the code to know, which textbox is changed
                        {
                            if ((parseFloat(curObj.value) <= rowData.Product.CurrentStock || ApplyCurrentStock) && parseFloat(curObj.value) <= balOrderQty)
                                if (rowData.Product.CurrentStock > balOrderQty || ApplyCurrentStock)
                                    productChildTable[i].CurrentPkgQty = parseFloat(curObj.value);
                                else
                                    productChildTable[i].CurrentPkgQty = rowData.Product.CurrentStock;
                            else {
                                if (rowData.Product.CurrentStock > balOrderQty || ApplyCurrentStock) {
                                    productChildTable[i].CurrentPkgQty = parseFloat(balOrderQty);
                                    $('#txt2_' + rowData.GroupID).val(balOrderQty)
                                }
                                else {
                                    productChildTable[i].CurrentPkgQty = rowData.Product.CurrentStock;
                                    $('#txt2_' + rowData.GroupID).val(rowData.Product.CurrentStock);
                                }
                            }

                            $('#txt2_' + rowData.GroupID).val(productChildTable[i].CurrentPkgQty);
                            //Parent Table Change
                            if (_productListTableData[rowindex].GroupID == productChildTable[i].GroupID)
                                _productListTableData[rowindex].CurrentPkgQty = productChildTable[i].CurrentPkgQty;
                        }
                        if (textboxNo == 3) {
                            productChildTable[i].PkgWt = parseFloat(curObj.value);
                            //Parent Table Change
                            if (_productListTableData[rowindex].GroupID == productChildTable[i].GroupID)
                                _productListTableData[rowindex].PkgWt = parseFloat(curObj.value);
                            break;
                        }
                    }

                    $('#ProductChildTable' + rowData.GroupID).DataTable().clear().rows.add(productChildTable).draw(false);
                }
                else//
                { //while Child Table Not Exisitng.

                    if (textboxNo == 2) {
                        if ((parseFloat(curObj.value) <= rowData.Product.CurrentStock || ApplyCurrentStock) && parseFloat(curObj.value) <= balOrderQty) {
                            if (rowData.Product.CurrentStock > balOrderQty || ApplyCurrentStock) {
                                rowData.CurrentPkgQty = parseFloat(curObj.value);
                                $('#txt2_' + rowData.GroupID).val(parseFloat(curObj.value));
                            }
                            else {
                                rowData.CurrentPkgQty = parseFloat(rowData.Product.CurrentStock);
                                $('#txt2_' + rowData.GroupID).val(parseFloat(rowData.Product.CurrentStock));
                            }
                            //Parent Table Change
                            _productListTableData[rowindex].CurrentPkgQty = rowData.CurrentPkgQty;
                        }
                        else {
                            if (rowData.Product.CurrentStock > balOrderQty || ApplyCurrentStock) {
                                rowData.CurrentPkgQty = parseFloat(balOrderQty);
                                $('#txt2_' + rowData.GroupID).val(balOrderQty)
                            }
                            else {
                                _productListTableData[i].CurrentPkgQty = rowData.Product.CurrentStock;
                                $('#txt2_' + rowData.GroupID).val(rowData.Product.CurrentStock);
                            }
                        }
                    }
                    if (textboxNo == 3) {
                        //Parent Table Change
                        _productListTableData[rowindex].PkgWt = parseFloat(curObj.value);
                    }
                }
            }
            else {
                //group name change
                for (i = 0; i < _productListTableData.data().count() ; i++) {
                    if (_productListTableData[i].GroupID == rowData.GroupID) {
                        if (textboxNo == 1) {
                            //Parent Table Change
                            _productListTableData[i].GroupName = curObj.value;
                        }
                    }
                }
            }
        }
    }
    else {
        if (textboxNo == 2) {
            _productListTableData[rowindex].CurrentPkgQty = 0;
            if (rowData.GroupID == EmptyGuid || rowData.GroupID == null) {
                $('#txt2_' + rowData.ProductID).val(0);
            }
            else {
                $('#txt2_' + rowData.GroupID).val(0);
            }
        }
        if (textboxNo == 3) {
            _productListTableData[rowindex].PkgWt = 0;
            if (rowData.GroupID == EmptyGuid || rowData.GroupID == null) {
                $('#txt3_' + rowData.ProductID).val(0);
            }
            else {
                $('#txt3_' + rowData.GroupID).val(0);
            }
        }
    }
}

function AddPackingSlipDetailFromSaleOrderGroups() {
    debugger;
    var packingSlipID = $('#ID').val();
    var isDataVaild = 1;
    var message = "";

    if (_productListTableData == undefined)
        _productListTableData = DataTables.ProductListTable.rows().data();

    if (CheckedGroups.length > 0) {
        CheckedGroups.forEach(function (element) {
            for (i = 0; i < _productListTableData.length; i++) {
                if (_productListTableData[i].GroupID == element.GroupID) {
                    element.GroupName = _productListTableData[i].GroupName;
                    element.Qty = _productListTableData[i].CurrentPkgQty;
                    element.Weight = _productListTableData[i].PkgWt;
                    if (element.Qty == 0) {
                        isDataVaild = 0;
                        message = 'Have no Quantity';
                    }
                }
            }
        });
    }
    if (CheckedProducts.length > 0) {
        CheckedProducts.forEach(function (element) {
            for (i = 0; i < _productListTableData.data().count() ; i++) {
                if (_productListTableData[i].ProductID == element.ProductID &&( element.GroupID == '00000000-0000-0000-0000-000000000000'|| element.GroupID ==null)) {
                    element.Qty = $('#txt2_' + element.ProductID).val(); //_productListTableData[i].CurrentPkgQty;
                    element.Weight = $('#txt3_' + element.ProductID).val(); // _productListTableData[i].PkgWt;
                    if (element.Qty == 0) {
                        isDataVaild = 0;
                        message = 'Have no Quantity';
                    }
                }
                if (_productListTableData[i].GroupID == element.GroupID) {
                    element.GroupName = _productListTableData[i].GroupName;
                    element.Qty = $('#txt2_' + element.GroupID).val(); //_productListTableData[i].CurrentPkgQty;
                    element.Weight = $('#txt3_' + element.GroupID).val();//_productListTableData[i].PkgWt;
                    if (element.Qty == 0) {
                        isDataVaild = 0;
                        message = 'Have no Quantity';
                    }
                    if (!ApplyCurrentStock)
                        if (element.CurrentStock < _productListTableData[i].CurrentPkgQty) {
                            isDataVaild = 0;
                            message = 'Have no enough Stock';
                        }
                    if (element.BalQuantity < _productListTableData[i].CurrentPkgQty) {
                        isDataVaild = 0;
                        message = 'Balance Order Quantity is less than entered Quantity';
                    }
                }
            }
        });
    }
    if (isDataVaild && (CheckedGroups.length != 0 || CheckedProducts.length != 0)) {
        $('#PackingSlipModal').modal('hide');
        Save();
        _isEditPkgSlipDetail = 0;
        _currentPkgQtyEdit = 0;

    }
    else {
        if (CheckedGroups.length == 0 && CheckedProducts.length == 0)
            notyAlert('warning', "No rows selected ");
        else
            notyAlert('warning', message + " for selected rows");

    }

}
function BindEditGroupProductListTable(groupID) {

    CheckedGroups = [];

    $('#modelContextLabel').text('Edit Packing Slip Details');
    var productList = GetProductListForGroupEdit(groupID);
    debugger;
    _currentPkgQtyEdit = productList[0].CurrentPkgQty;
    DataTables.ProductListTable.clear().rows.add(productList).draw(false);
    //loop datatable for checkbox checking by the child count
    var pkgDetailVM = DataTables.ProductListTable.rows().data();
    for (i = 0; i < pkgDetailVM.length ; i++) {
        if (pkgDetailVM[i].ChildCount == pkgDetailVM[i].PkgSlipChildCount) {
            $('input[id=G_' + groupID + ']').prop('checked', true);//G_6389eca3-9268-4428-8e2e-a25f222e6705
            //group row checked to checkedGroups

            packingSlipDetail = new Object();
            packingSlipDetail.ProductID = pkgDetailVM[i].ProductID == null ? EmptyGuid : pkgDetailVM[i].ProductID;
            packingSlipDetail.GroupID = pkgDetailVM[i].GroupID == null ? EmptyGuid : pkgDetailVM[i].GroupID;
            packingSlipDetail.RemoveID = pkgDetailVM[i].GroupIdString;
            packingSlipDetail.GroupName = pkgDetailVM[i].GroupName;
            packingSlipDetail.ID = EmptyGuid;
            packingSlipDetail.Qty = pkgDetailVM[i].CurrentPkgQty;
            packingSlipDetail.CurrentStock = pkgDetailVM[i].Product.CurrentStock
            packingSlipDetail.Weight = pkgDetailVM[i].PkgWt;
            //  CheckedGroups.findIndex(x => x.RemoveID === rowData.GroupIdString) !== -1 ? CheckedGroups.push(packingSlipDetail) : "";
            CheckedGroups.push(packingSlipDetail);

            //CheckedProducts need to be added
            var packingSlipDetailVM = GetProductListForPackingSlipByGroupID(groupID); //childTable rows and filter existing rows 
            for (j = 0; j < packingSlipDetailVM.length ; j++) {
                if (packingSlipDetailVM[j].isExists)//if already exists else checked false
                {

                    packingSlipDetail = new Object();
                    packingSlipDetail.ProductID = packingSlipDetailVM[j].ProductID == null ? EmptyGuid : packingSlipDetailVM[j].ProductID;
                    packingSlipDetail.GroupID = packingSlipDetailVM[j].GroupID == null ? EmptyGuid : packingSlipDetailVM[j].GroupID;
                    packingSlipDetail.RemoveID = packingSlipDetailVM[j].ProductIdString;
                    packingSlipDetail.GroupName = packingSlipDetailVM[j].GroupName;
                    packingSlipDetail.ID = EmptyGuid;
                    packingSlipDetail.Qty = packingSlipDetailVM[j].CurrentPkgQty;
                    packingSlipDetail.CurrentStock = packingSlipDetailVM[j].Product.CurrentStock
                    //debugger;
                    //packingSlipDetail.BalQuantity = packingSlipDetailVM[j].Quantity - packingSlipDetailVM[j].PrevPkgQty
                    packingSlipDetail.Weight = packingSlipDetailVM[j].PkgWt;
                    //  CheckedProducts.findIndex(x => x.RemoveID === rowData.ProductIdString) !== -1 ? CheckedProducts.push(packingSlipDetail) : "";
                    CheckedProducts.push(packingSlipDetail);

                }
            }


        }
        else if (pkgDetailVM[i].PkgSlipChildCount == 0) {
            $('input[id=G_' + groupID + ']').prop('checked', false);
        }
        else {
            $('input[id=G_' + groupID + ']').prop('indeterminate', true);
            //full items insert into checked products from PackingSlipDetail table
            var packingSlipDetailVM = GetProductListForPackingSlipByGroupID(groupID); //childTable rows and filter existing rows 
            for (j = 0; j < packingSlipDetailVM.length ; j++) {
                if (packingSlipDetailVM[j].isExists)//if already exists else checked false
                {
                    packingSlipDetail = new Object();
                    packingSlipDetail.ProductID = packingSlipDetailVM[j].ProductID == null ? EmptyGuid : packingSlipDetailVM[j].ProductID;
                    packingSlipDetail.GroupID = packingSlipDetailVM[j].GroupID == null ? EmptyGuid : packingSlipDetailVM[j].GroupID;
                    packingSlipDetail.RemoveID = packingSlipDetailVM[j].ProductIdString;
                    packingSlipDetail.GroupName = packingSlipDetailVM[j].GroupName;
                    packingSlipDetail.ID = EmptyGuid;
                    packingSlipDetail.Qty = packingSlipDetailVM[j].CurrentPkgQty;
                    packingSlipDetail.CurrentStock = packingSlipDetailVM[j].Product.CurrentStock
                    packingSlipDetail.Weight = packingSlipDetailVM[j].PkgWt;
                    //  CheckedProducts.findIndex(x => x.RemoveID === rowData.ProductIdString) !== -1 ? CheckedProducts.push(packingSlipDetail) : "";
                    CheckedProducts.push(packingSlipDetail);

                }
            }
            //while intermediate group row also added to checked groups
            packingSlipDetail = new Object();
            packingSlipDetail.ProductID = pkgDetailVM[i].ProductID == null ? EmptyGuid : pkgDetailVM[i].ProductID;
            packingSlipDetail.GroupID = pkgDetailVM[i].GroupID == null ? EmptyGuid : pkgDetailVM[i].GroupID;
            packingSlipDetail.RemoveID = pkgDetailVM[i].GroupIdString;
            packingSlipDetail.GroupName = pkgDetailVM[i].GroupName;
            packingSlipDetail.ID = EmptyGuid;
            packingSlipDetail.Qty = pkgDetailVM[i].CurrentPkgQty;
            packingSlipDetail.CurrentStock = pkgDetailVM[i].Product.CurrentStock
            packingSlipDetail.Weight = pkgDetailVM[i].PkgWt;
            //  CheckedGroups.findIndex(x => x.RemoveID === rowData.GroupIdString) !== -1 ? CheckedGroups.push(packingSlipDetail) : "";
            CheckedGroups.push(packingSlipDetail);
        }
    }
    //if checked,or intermediate add grouprow to checked groups
}

function GetProductListForGroupEdit(groupID) {
    //GetPackingSlipDetailGroupEdit
    try {
        debugger;
        var packingSlipID = $('#ID').val();
        var id = $('#hdnSalesOrderID').val();
        var saleOrderID = $('#hdnSalesOrderID').val();
        var data = { "groupID": groupID, "packingSlipID": packingSlipID, "saleOrderID": saleOrderID };
        var jsonData = {};
        var result = "";
        var message = "";
        var productListVM = new Object();

        jsonData = GetDataFromServer("PackingSlip/GetPackingSlipDetailGroupEdit/", data);
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

function EditPackingSlipDetailProduct() {
    _SlNo = 1;
    EditFlag = 1;
    CheckedProducts = [];

    var pkgDetailsVM = DataTables.PkgSlipDetailEditTable.rows().data();
    var res = CheckProductDetailsEditTbl(pkgDetailsVM);//Checking all details are entered or not
    if (res) {
        packingSlipDetail = new Object();
        packingSlipDetail.ProductID = pkgDetailsVM[0].SalesOrder.SalesOrderDetail.ProductID;
        packingSlipDetail.ID = pkgDetailsVM[0].ID; //[PKGDetailID]
        packingSlipDetail.Name = pkgDetailsVM[0].SalesOrder.SalesOrderDetail.Product.Name;
        packingSlipDetail.CurrentPkgQty = pkgDetailsVM[0].SalesOrder.SalesOrderDetail.CurrentPkgQty;
        packingSlipDetail.PkgWt = pkgDetailsVM[0].SalesOrder.SalesOrderDetail.PkgWt;
        packingSlipDetail.Qty = pkgDetailsVM[0].SalesOrder.SalesOrderDetail.CurrentPkgQty;
        packingSlipDetail.Weight = pkgDetailsVM[0].SalesOrder.SalesOrderDetail.PkgWt;
        CheckedProducts.push(packingSlipDetail);// adding values to packingSlipDetail
        _SlNo = 1;
        // DataTables.PackingSlipDetailTable.clear().rows.add(packingDetail).draw(false); //binding Detail table with new values   

        $('#EditPkgSlipDetailsModal').modal('hide');
        Save();

    }
    else
        notyAlert('warning', "Please Enter Packing Quantity");
}