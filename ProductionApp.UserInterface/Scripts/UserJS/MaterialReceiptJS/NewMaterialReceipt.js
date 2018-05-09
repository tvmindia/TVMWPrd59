//*****************************************************************************
//*****************************************************************************
//Author: Arul
//CreatedDate: 20-Feb-2018 
//LastModified: 24-Apr-2018
//FileName: NewMaterialReceipt.js
//Description: Client side coding for Adding / Updating Material Receipts
//******************************************************************************
//******************************************************************************

var DataTables = {};
var EmptyGuid = "00000000-0000-0000-0000-000000000000";
var _MaterialReceiptDetail = {};
var _MaterialReceiptDetailList = [];
var _IsInput = false;

$(document).ready(function () {
    debugger;
    try {
        $('#msgSupplier,#msgPurchase').hide();

        try {
            DataTables.MaterialReceiptDetailTable = $('#tblMaterialReceiptDetail').DataTable(
            {
                dom: '<"pull-right"f>rt<"bottom"ip><"clear">',
                ordering: false,
                searching: false,
                paging: false,
                data: null,
                autoWidth: false,
                columns: [
                    { "data": "ID", "defaultContent": "<i></i>" },
                    { "data": "MaterialID", "defaultContent": "<i></i>" },
                    { "data": "Material.MaterialCode", "defaultContent": "<i></i>" },
                    { "data": "MaterialDesc", "defaultContent": "<i></i>" },
                    { "data": "QtyInKG", render: function (data, type, row) { return data }, "defaultContent": "<i></i>" },
                    { "data": "Qty", render: function (data, type, row) { return data }, "defaultContent": "<i></i>" },
                    //{ "data": "UnitCode", render: function (data, type, row) { return data }, "defaultContent": "<i></i>" },
                    { "data": null, "orderable": false, "defaultContent": '<a href="#" class="actionLink"  onclick="DetailEdit(this)" ><i class="glyphicon glyphicon-pencil" aria-hidden="true"></i></a> | <a href="#" class="DeleteLink"  onclick="Delete(this)" ><i class="glyphicon glyphicon-trash" aria-hidden="true"></i></a>' },
                ],
                columnDefs: [{ "targets": [0, 1], "visible": false, searchable: false },
                    { className: "text-center", "targets": [6], "width": "7%" },
                    { className: "text-right", "targets": [4, 5], "width": "20%" },
                    { className: "text-left", "targets": [2, 3] }
                ]
            }
        );
        }
        catch (e) {
            console.log(e.message)
        }
        try {
            DataTables.PurchaseOrderDetailTable = $('#tblPurchaseOrderDetail').DataTable(
            {
                dom: '<"pull-right"f>rt<"bottom"ip><"clear">',
                order: [],
                searching: true,
                paging: true,
                data: null,
                pageLength: 5,
                language: {
                    search: "_INPUT_",
                    searchPlaceholder: "Search"
                },
                columns: [
                  { "data": "ID" },
                  { "data": "Checkbox", "defaultContent": "" },//1
                  { "data": "MaterialID", "defaultContent": "<i>-</i>" },
                  { "data": "MaterialCode", "defaultContent": "<i>-</i>" },//3
                  { "data": "MaterialDesc", "defaultContent": "<i>-</i>" },
                  { "data": "Qty", "defaultContent": "<i>-</i>" },//5
                  { "data": "UnitCode", "defaultContent": "<i>-</i>" },//6
                  { "data": "PrevRcvQtyInKG", "defaultContent": "<i>-</i>" },//7
                  { "data": "PrevRcvQty", "defaultContent": "<i>-</i>" },//7-8
                  {
                      "data": "Material.QtyInKG", render: function (data, type, row) {
                          return '<input class="form-control text-left " name="Markup" value="' + data + '" type="text" onclick="SelectAllValue(this);" onkeypress = "return isNumber(event)", onchange="QtyTextBoxValueChanged(this);">';
                      }, "defaultContent": "<i>-</i>"
                  },//9
                  {
                      "data": "Material.Qty", render: function (data, type, row) {
                          return '<input class="form-control text-right " name="Markup" value="' + data + '" type="text" onclick="SelectAllValue(this);" onkeypress = "return isNumber(event)", onchange="TextBoxValueChanged(this);">';
                      }, "defaultContent": "<i>-</i>"
                  },//9-10
                  //{ "data": null, "defaultContent": "Nos." },

                ],
                columnDefs: [{ orderable: false, className: 'select-checkbox', "targets": 1 },
                    { "targets": [0, 2], "visible": false, "searchable": false },
                    { className: "text-right", "targets": [5, 7, 8, 9], "width": "10%" },//, 9, 10
                    { className: "text-left", "targets": [3, 4, 6] },//, 9
                    { className: "text-center", "targets": [1] },
                    { "targets": [3, 4, 5, 6, 7, 8, 9], "bSortable": false }
                ],
                select: { style: 'multi', selector: 'td:first-child' },
                destroy: true
            });
        }
        catch (e) {
            console.log(e.message)
        }

        $('#MaterialID').select2({
            dropdownParent: $("#MaterialReceiptDetailModal")
        });

        $('#SupplierID,#EmployeeID').select2({});
        //PurchaseOrderOnChange();

        //$('#MaterialReceiptDetail_UnitCode').select2({
        //    width: '100%'
        //});

        $("#SupplierID").change(function () {
            LoadPurchaseOrderDropdownBySupplier();
            debugger;
            $('#hdnSupplierID').val($("#SupplierID").val());
            //PurchaseOrderOnChange();
        });

        $('#divPONo').hide();

        if ($('#ID').val !== EmptyGuid && $('#IsUpdate').val() === 'True') {
            BindMaterialReceipt();//Get MaterialReceipt By ID
            $('#lblReceiptNo').text('MRN#: ' + $('#ReceiptNo').val());
            debugger;
            ChangeButtonPatchView('MaterialReceipt', 'divButtonPatch', 'Edit');//divbuttonPatchAddMaterialReceipt
            $('#PurchaseOrderID').prop('disabled', true);
            $('#ExistingPurchaseOrder').prop('disabled', true);
        }
        else {
            $('#lblReceiptNo').text('MRN#: New');
        }

        $("#MaterialID").change(function () {
            BindMaterialDetails(this.value)
        });

        $("#SupplierID").change(function () {
            $('#msgSupplier').hide();
        });

    }
    catch (ex) {
        console.log(ex.message);
    }
});

function ExistingPurchaseOrderChanged() {
    debugger;
    if ($('#ExistingPurchaseOrder').val() === "YES") {
        $('#divPONo').hide();
        $('#PurchaseOrderNo').val("");
        $('#btnLoadPO').removeClass('disabled');
        $('#divPOID').show();
    } else {
        $('#divPONo').show();
        $('#PurchaseOrderID').val("").trigger('change');
        $('#btnLoadPO').addClass('disabled');
        $('#divPOID').hide();
    }
}

function AddMaterialReceiptDetail() {
    debugger;
    $('#MaterialID').val("").trigger('change');
    $('#MaterialReceiptDetail_Material_MaterialCode').val('');
    $('#MaterialReceiptDetail_Material_CurrentStock').val('');
    $('#MaterialReceiptDetail_Qty').val(0);
    $('#MaterialReceiptDetail_QtyInKG').val(0);
    $('#MaterialReceiptDetail_MaterialDesc').val('');
    //$('#MaterialReceiptDetail_UnitCode').val("").trigger("change");
    $("#WeightInKG").val("0");
    $('#MaterialReceiptDetailModal').modal('show');
}

function BindMaterialDetails(id) {
    try {
        debugger;

        if (id !== "") {
            var materialVM = GetMaterial(id);
            $('#MaterialReceiptDetail_Material_MaterialCode').val(materialVM.MaterialCode);
            $('#MaterialReceiptDetail_Material_CurrentStock').val(materialVM.CurrentStock);
            $('#MaterialReceiptDetail_Qty').val(0);
            $('#MaterialReceiptDetail_QtyInKG').val(0);
            $('#MaterialReceiptDetail_MaterialDesc').val(materialVM.Description);
            //$('#MaterialReceiptDetail_UnitCode').val("Nos.");
            $('#WeightInKG').val(materialVM.WeightInKG);
            $('#MaterialReceiptDetail_QtyInKG').change(function () {
                if (parseFloat($('#WeightInKG').val()) > 0)
                    if ($('#MaterialReceiptDetail_QtyInKG').val() != "")
                        $('#MaterialReceiptDetail_Qty').val(Math.floor(parseFloat($('#MaterialReceiptDetail_QtyInKG').val()) / parseFloat(($('#WeightInKG').val() !== "0" && $('#WeightInKG').val() !== null) ? $('#WeightInKG').val() : 1)));
                //Math.floor used to round downward to nearest integer
            });

        }
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}

function GetMaterial(id) {
    try {
        debugger;
        var data = { "id": id };
        var result = "";
        var message = "";
        var materialVM = new Object();
        var jsonData = GetDataFromServer("MaterialReceipt/GetMaterial/", data);
        if (jsonData != '') {
            jsonData = JSON.parse(jsonData);
            result = jsonData.Result;
            materialVM = jsonData.Record;
            message = jsonData.Message;
        }
        if (result == "OK") {
            return materialVM;
        }
        if (result == "ERROR") {
            alert(message);
        }
    }
    catch (e) {
        console.log('error', e.message);
    }
}

function AddMaterialReceiptDetailToTable() {
    try {
        debugger;
        if ($("#MaterialID").val() != "") {
            _MaterialReceiptDetailList = [];
            MaterialReceiptDetailVM = new Object();
            MaterialReceiptDetailVM.MaterialID = $("#MaterialID").val();
            MaterialReceiptDetailVM.Material = new Object();
            MaterialReceiptDetailVM.Material.MaterialCode = $('#MaterialReceiptDetail_Material_MaterialCode').val();
            MaterialReceiptDetailVM.Material.CurrentStock = $('#MaterialReceiptDetail_Material_CurrentStock').val();
            MaterialReceiptDetailVM.MaterialDesc = $('#MaterialReceiptDetail_MaterialDesc').val();
            MaterialReceiptDetailVM.ID = $('#MaterialReceiptDetail_ID').val();
            MaterialReceiptDetailVM.Qty = $('#MaterialReceiptDetail_Qty').val();
            MaterialReceiptDetailVM.QtyInKG = $('#MaterialReceiptDetail_QtyInKG').val();
            //MaterialReceiptDetailVM.UnitCode = $('#MaterialReceiptDetail_UnitCode').val();
            //switch (MaterialReceiptDetailVM.UnitCode) {
            //    case "kg":
            //        MaterialReceiptDetailVM.Qty = Math.floor(MaterialReceiptDetailVM.Qty / parseFloat(($('#WeightInKG').val() !== "0" && $('#WeightInKG').val() !== null) ? $('#WeightInKG').val() : 1));
            //        MaterialReceiptDetailVM.UnitCode = "Nos.";
            //        break;
            //    default:
            //        MaterialReceiptDetailVM.UnitCode = "Nos.";
            //        break;
            //}
            _MaterialReceiptDetailList.push(MaterialReceiptDetailVM);

            debugger;
            if (_MaterialReceiptDetailList != null) {
                var materialReceiptDetailList = DataTables.MaterialReceiptDetailTable.rows().data();
                if (materialReceiptDetailList.length > 0) {
                    var checkPoint = 0;
                    for (var i = 0; i < materialReceiptDetailList.length; i++) {
                        if (materialReceiptDetailList[i].MaterialID == MaterialReceiptDetailVM.MaterialID) {
                            materialReceiptDetailList[i].MaterialDesc = MaterialReceiptDetailVM.MaterialDesc;
                            materialReceiptDetailList[i].QtyInKG = MaterialReceiptDetailVM.QtyInKG;
                            materialReceiptDetailList[i].Qty = MaterialReceiptDetailVM.Qty;
                            //materialReceiptDetailList[i].UnitCode = MaterialReceiptDetailVM.UnitCode;
                            checkPoint = 1;
                            break;
                        }
                    }
                    if (!checkPoint) {
                        DataTables.MaterialReceiptDetailTable.rows.add(_MaterialReceiptDetailList).draw(false);
                    }
                    else {
                        DataTables.MaterialReceiptDetailTable.clear().rows.add(materialReceiptDetailList).draw(false);
                    }
                }
                else {
                    DataTables.MaterialReceiptDetailTable.rows.add(_MaterialReceiptDetailList).draw(false);
                }
            }
            $('#MaterialReceiptDetailModal').modal('hide');
        }
        else {
            notyAlert('warning', "Material is Empty");
        }


    } catch (e) {
        console.log('error: ' + e.message);
    }
}

function Save() {
    try {
        debugger;
        var check = 0;
        $("#PurchaseOrderNo").prop('disabled', false)
        $('#PurchaseOrderID').prop('disabled', false);
        if ($('#PurchaseOrderNo').val() === "" && ($('#PurchaseOrderID').val() === "" || $('#PurchaseOrderID').val() === undefined)) {
            $('#msgPurchase').show();
            $('#PurchaseOrderNo,#PurchaseOrderID').change(function () {
                $('#msgPurchase').hide();
            });
            check = 1;
        }

        $('#SupplierID').prop('disabled', false);
        if ($('#SupplierID').val() === "") {
            $('#msgSupplier').show();
            $('#SupplierID').change(function () {
                $('#msgSupplier').hide();
            });
            check = 1;
        }

        $("#DetailJSON").val('');
        _MaterialReceiptDetailList = [];
        AddMaterialReceiptDetailList();
        if (_MaterialReceiptDetailList.length > 0) {
            var result = JSON.stringify(_MaterialReceiptDetailList);
            $("#DetailJSON").val(result);
            if (check !== 1) {
                $('#btnSave').trigger('click');
            }
        }
        else {
            notyAlert('warning', 'Please Add MaterialReceipt Details!');
        }
    }
    catch (ex) {
        console.log(ex.message);
    }
}

function SaveSuccess(data, status) {
    debugger;
    try {
        var JsonResult = JSON.parse(data)
        var result = JsonResult.Result;
        var message = JsonResult.Message;
        var materialReceiptVM = new Object();
        materialReceiptVM = JsonResult.Records;
        switch (result) {
            case "OK":
                $('#IsUpdate').val('True');
                $('#ID').val(JsonResult.Records.ID)
                message = JsonResult.Records.Message;
                notyAlert("success", message);
                ChangeButtonPatchView('MaterialReceipt', 'divButtonPatch', 'Edit');//divbuttonPatchAddMaterialReceipt
                BindMaterialReceiptDetailTable($('#ID').val());
                PurchaseOrderOnChange()

                break;
            case "ERROR":
                notyAlert("error", message);
                break;
            default:
                notyAlert("error", message);
                break;
        }
    } catch (ex) {
        notyAlert("error", ex.message);
    }
}

function AddMaterialReceiptDetailList() {
    debugger;
    var materialReceiptDetailVM = DataTables.MaterialReceiptDetailTable.rows().data();
    for (var r = 0; r < materialReceiptDetailVM.length; r++) {
        MaterialReceiptDetail = new Object();
        MaterialReceiptDetail.ID = materialReceiptDetailVM[r].ID;
        MaterialReceiptDetail.MaterialID = materialReceiptDetailVM[r].MaterialID;
        MaterialReceiptDetail.MaterialDesc = materialReceiptDetailVM[r].MaterialDesc;
        MaterialReceiptDetail.Qty = materialReceiptDetailVM[r].Qty;
        MaterialReceiptDetail.QtyInKG = materialReceiptDetailVM[r].QtyInKG;
        MaterialReceiptDetail.UnitCode = materialReceiptDetailVM[r].UnitCode;
        _MaterialReceiptDetailList.push(MaterialReceiptDetail);
    }
}

function LoadPODetail() {
    debugger;
    $('#PurchaseOrderID').prop('disabled', false);
    if ($('#PurchaseOrderID').val() !== "" && $('#PurchaseOrderID').val() !== undefined) {
        $('#PurchaseOrderDetailsModal').modal('show');
        var id = $('#PurchaseOrderID').val();
        BindPurchaseOrderDetailTable(id);
    } else {
        if ($('#PurchaseOrderNo').val() !== "") {
            notyAlert('warning', 'Purchase Order Not selcted!');
        }
        else {
            notyAlert('warning', 'Purchase Order Not selcted!');
        }
    }
    if ($("#IsUpdate").val() === "True") {
        $('#PurchaseOrderID').prop('disabled', true);
    }
}

function BindPurchaseOrderDetailTable(id) {
    try {

        debugger;
        var purchaseOrderDetailList = GetPurchaseOrderItem(id);
        var materialReceiptDetailList = DataTables.MaterialReceiptDetailTable.rows().data();
        //To exclude the materials already in Details table
        for (j = 0; j < materialReceiptDetailList.length; j++) {//To remove the existing items in Details from PO items
            for (i = 0; i < purchaseOrderDetailList.length; i++) {
                debugger;
                if (purchaseOrderDetailList[i].MaterialID === materialReceiptDetailList[j].MaterialID) {
                    purchaseOrderDetailList.splice(i, 1);//Removes the ith element
                }
            }
        }
        //To give material details to the purchase order detail
        for (i = 0; i < purchaseOrderDetailList.length; i++) {
            debugger;
            purchaseOrderDetailList[i].Material = new Object();
            purchaseOrderDetailList[i].Material = GetMaterial(purchaseOrderDetailList[i].MaterialID)
            purchaseOrderDetailList[i].Material.UnitCode = purchaseOrderDetailList[i].UnitCode;
            purchaseOrderDetailList[i].Material.QtyInKG = 0;
            switch (purchaseOrderDetailList[i].UnitCode) {
                case "kg":
                    purchaseOrderDetailList[i].Material.QtyInKG = purchaseOrderDetailList[i].Qty;
                    purchaseOrderDetailList[i].Material.Qty = Math.floor(purchaseOrderDetailList[i].Material.WeightInKG !== 0 ? purchaseOrderDetailList[i].Qty / purchaseOrderDetailList[i].Material.WeightInKG : 0);
                    break;
                case "Ton":
                    purchaseOrderDetailList[i].Material.QtyInKG = purchaseOrderDetailList[i].Qty*1000;
                    purchaseOrderDetailList[i].Material.Qty = Math.floor(purchaseOrderDetailList[i].Material.WeightInKG !== 0 ? purchaseOrderDetailList[i].Qty * 1000 / purchaseOrderDetailList[i].Material.WeightInKG : 0);
                    break;
                default:
                    purchaseOrderDetailList[i].Material.Qty = purchaseOrderDetailList[i].Qty;
                    break;
            }

        }
        DataTables.PurchaseOrderDetailTable.clear().rows.add(purchaseOrderDetailList).select().draw(false);
    }
    catch (e) {
        console.log(e.message);
    }
}

function GetPurchaseOrderItem(id) {
    try {
        debugger;
        var data = { "id": id };
        var purchaseOrderDetailList = [];
        var result = "";
        var message = "";
        var jsonData = GetDataFromServer("MaterialReceipt/GetAllPurchaseOrderItem/", data);
        if (jsonData != '') {
            jsonData = JSON.parse(jsonData);
            result = jsonData.Result;
            message = jsonData.Message;
            purchaseOrderDetailList = jsonData.Records;
        }
        if (result == "OK") {
            return purchaseOrderDetailList;
        }
        if (result == "ERROR") {
            notyAlert('error', message);
        }
    }
    catch (e) {
        console.log(e.message);
    }
}

function AddPOItems() {
    try {
        debugger;
        //Merging  the rows with same MaterialID
        var purchaseOrderItemList = DataTables.PurchaseOrderDetailTable.rows(".selected").data();
        var mergedRows = []; //to store rows after merging
        var currentMaterial;

        if (purchaseOrderItemList.length > 0) {
            debugger;
            for (var r = 0; r < purchaseOrderItemList.length; r++) {
                currentMaterial = purchaseOrderItemList[r].MaterialID
                for (var j = r + 1; j < purchaseOrderItemList.length; j++) {
                    if (purchaseOrderItemList[j].MaterialID == currentMaterial) {
                        purchaseOrderItemList[r].Qty += parseFloat(purchaseOrderItemList[j].Qty);
                        purchaseOrderItemList.splice(j, 1);//removing duplicate after adding value 
                        j = j - 1;// for avoiding skipping row while checking
                    }
                }
                mergedRows.push(purchaseOrderItemList[r])// adding rows to merge array
            }

            LoadPOItems(mergedRows);
            $('#PurchaseOrderDetailsModal').modal('hide');

        } else {
            notyAlert('warning', 'No Items are selcted!');
        }
    } catch (ex) {
        console.log(ex.message)
    }
}

function LoadPOItems(purchaseOrderDetailList) {
    try {
        debugger;
        _MaterialReceiptDetailList = [];

        for (var i = 0; i < purchaseOrderDetailList.length; i++) {
            materialReceiptDetailVM = new Object();
            materialReceiptDetailVM.MaterialID = purchaseOrderDetailList[i].MaterialID;
            materialReceiptDetailVM.Material = new Object();
            materialReceiptDetailVM.Material.MaterialCode = purchaseOrderDetailList[i].MaterialCode;
            materialReceiptDetailVM.MaterialDesc = purchaseOrderDetailList[i].MaterialDesc;
            materialReceiptDetailVM.ID = EmptyGuid;
            materialReceiptDetailVM.Qty = Math.floor(parseFloat(purchaseOrderDetailList[i].Material.Qty));
            materialReceiptDetailVM.QtyInKG = (parseFloat(purchaseOrderDetailList[i].Material.QtyInKG));
            materialReceiptDetailVM.UnitCode = "Nos.";

            _MaterialReceiptDetailList.push(materialReceiptDetailVM);
        }

        RebindMaterialReceiptDetailTable(_MaterialReceiptDetailList);

    }
    catch (ex) {
        console.log(ex.message)
    }
}

function RebindMaterialReceiptDetailTable(_MaterialReceiptDetailList) {
    try {
        debugger;
        var checkPoint = 0;
        if (_MaterialReceiptDetailList != null) {
            for (var r = 0; r < _MaterialReceiptDetailList.length; r++) {
                var materialReceiptDetailList = DataTables.MaterialReceiptDetailTable.rows().data();
                if (materialReceiptDetailList.length > 0) {

                    for (var i = 0; i < materialReceiptDetailList.rows().data().length; i++) {
                        if (materialReceiptDetailList[i].MaterialID === _MaterialReceiptDetailList[r].MaterialID) {
                            materialReceiptDetailList[i].MaterialDesc = _MaterialReceiptDetailList[r].MaterialDesc;
                            materialReceiptDetailList[i].Qty = _MaterialReceiptDetailList[r].Qty;
                            materialReceiptDetailList[i].UnitCode = _MaterialReceiptDetailList[r].UnitCode;
                            checkPoint = 1;
                            _MaterialReceiptDetailList.splice(r, 1);
                            break;
                        }
                    }
                    if (checkPoint) {
                        DataTables.MaterialReceiptDetailTable.clear().rows.add(materialReceiptDetailList).draw(false);
                    }
                }
            }
            DataTables.MaterialReceiptDetailTable.rows.add(_MaterialReceiptDetailList).draw(false);
        }
    }
    catch (ex) {
        console.log(ex.message);
    }
}

function DetailEdit(curObj) {
    try {
        debugger;
        AddMaterialReceiptDetail();
        var materialReceiptDetailViewModel = DataTables.MaterialReceiptDetailTable.row($(curObj).parents('tr')).data();
        $("#MaterialID").val(materialReceiptDetailViewModel.MaterialID).trigger("change");
        debugger;
        $('#MaterialReceiptDetail_Material_MaterialCode').val(materialReceiptDetailViewModel.Material.MaterialCode);
        $('#MaterialReceiptDetail_MaterialDesc').val(materialReceiptDetailViewModel.MaterialDesc);
        $('#MaterialReceiptDetail_ID').val(materialReceiptDetailViewModel.ID);
        $('#MaterialReceiptDetail_Qty').val(materialReceiptDetailViewModel.Qty);
        $('#MaterialReceiptDetail_QtyInKG').val(materialReceiptDetailViewModel.QtyInKG);
        $('#MaterialReceiptDetail_UnitCode').val(materialReceiptDetailViewModel.UnitCode).trigger("change");
    }
    catch (e) {
        console.log(e.message)
    }
}

function Delete(curobj) {
    debugger;
    var MaterialReceiptDetailVM = DataTables.MaterialReceiptDetailTable.row($(curobj).parents('tr')).data();
    var rowindex = DataTables.MaterialReceiptDetailTable.row($(curobj).parents('tr')).index();

    if ((MaterialReceiptDetailVM != null) && (MaterialReceiptDetailVM.ID != EmptyGuid)) {
        notyConfirm('Are you sure to delete?', 'DeleteItem("' + MaterialReceiptDetailVM.ID + '")');
    }
    else {
        var res = notyConfirm('Are you sure to delete?', 'DeleteTempItem("' + rowindex + '")');
    }
}

function DeleteTempItem(rowindex) {
    debugger;
    DataTables.MaterialReceiptDetailTable.row(rowindex).remove().draw(true);
    notyAlert('success', 'Deleted Successfully');
}

function DeleteItem(id) {

    try {
        debugger;
        var data = { "id": id };
        var result = "";
        var message = "";
        var materialReceiptDetailVM = new Object();
        var jsonData = GetDataFromServer("MaterialReceipt/DeleteMaterialReceiptDetail/", data);
        if (jsonData != '') {
            jsonData = JSON.parse(jsonData);
            result = jsonData.Result;
            message = jsonData.Message;
            materialReceiptDetailVM = jsonData.Record;
        }
        switch (result) {
            case "OK":
                notyAlert('success', message);
                Reset();
                break;
            case "ERROR":
                notyAlert('error', message);
                break;
            default:
                break;
        }
        return materialReceiptDetailVM;
    }
    catch (e) {

        notyAlert('error', e.message);
    }
}

function DeleteClick() {
    try {
        debugger;
        var id = $('#ID').val();
        if (id !== EmptyGuid)
            notyConfirm('Are you sure to delete?', 'DeleteMaterialReceipt("' + id + '")');
        else
            notyAlert('error', "Cannot Delete")
    }
    catch (e) {
        console.log(e.message)
    }
}

function DeleteMaterialReceipt(id) {

    try {
        debugger;
        var data = { "id": id };
        var result = "";
        var message = "";
        var materialReceiptVM = new Object();
        var jsonData = GetDataFromServer("MaterialReceipt/DeleteMaterialReceipt/", data);
        if (jsonData != '') {
            jsonData = JSON.parse(jsonData);
            result = jsonData.Result;
            message = jsonData.Message;
            materialReceiptVM = jsonData.Record;
        }
        switch (result) {
            case "OK":
                notyAlert('success', message);
                window.location.replace("NewMaterialReceipt?code=STR");
                break;
            case "ERROR":
                notyAlert('error', message);
                break;
            default:
                break;
        }
        return materialReceiptVM;
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}

function BindMaterialReceipt() {
    var id = $('#ID').val();
    var MaterialReceiptViewModel = {};
    debugger;
    MaterialReceiptViewModel = GetMaterialReceipt(id);
    $('#ReceiptNo').val(MaterialReceiptViewModel.ReceiptNo);
    $('#ReceiptDateFormatted').val(MaterialReceiptViewModel.ReceiptDateFormatted);
    $('#SupplierID').val(MaterialReceiptViewModel.SupplierID).trigger('change');
    $('#PurchaseOrderNo').val(MaterialReceiptViewModel.PurchaseOrderNo);
    $('#hdnPurchaseOrderID').val(MaterialReceiptViewModel.PurchaseOrderID);
    $('#PurchaseOrderID').val(MaterialReceiptViewModel.PurchaseOrderID).select2();
    $('#GeneralNotes').val(MaterialReceiptViewModel.GeneralNotes);
    BindMaterialReceiptDetailTable(id);//Get All MaterialReceiptDetails By HeaderID
}

function GetMaterialReceipt(id) {
    try {
        debugger;
        var data = { "id": id };
        var result = "";
        var message = "";
        var materialReceiptVM = new Object();
        var jsonData = GetDataFromServer("MaterialReceipt/GetMaterialReceipt/", data);
        if (jsonData != '') {
            jsonData = JSON.parse(jsonData);
            result = jsonData.Result;
            message = jsonData.Message;
            materialReceiptVM = jsonData.Records;
        }
        switch (result) {
            case "OK":
                break;
            case "ERROR":
                notyAlert('error', message);
                break;
            default:
                break;
        }
        return materialReceiptVM;
    }
    catch (e) {
        console.log(e.message);
    }
}

function BindMaterialReceiptDetailTable(id) {
    DataTables.MaterialReceiptDetailTable.clear().rows.add(GetMaterialReceiptDetail(id)).draw(false);
}

function GetMaterialReceiptDetail(id) {
    try {
        debugger;
        var data = { "id": id };
        var materialReceiptDetailList = [];
        var result = "";
        var message = "";
        var jsonData = GetDataFromServer("MaterialReceipt/GetAllMaterialReceiptDetail/", data);
        if (jsonData != '') {
            jsonData = JSON.parse(jsonData);
            result = jsonData.Result;
            message = jsonData.Message;
            materialReceiptDetailList = jsonData.Records;
        }
        if (result == "OK") {
            return materialReceiptDetailList;
        }
        if (result == "ERROR") {
            alert(message);
        }
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}

function Reset() {
    try {
        var id = $('#ID').val();
        if (id !== EmptyGuid)
            window.location.replace("NewMaterialReceipt?code=STR&id=" + id + "");
        else
            window.location.replace("NewMaterialReceipt?code=STR");
    }
    catch (ex) {
        console.log(ex.message)
    }
}

function ReceiptNoOnChange(curObj) {
    if ($('#ReceiptNo').val() !== "") {
        $('#lblReceiptNo').text('MRN#: ' + $('#ReceiptNo').val());
    }
    else {
        $('#lblReceiptNo').text('MRN#: New');
    }
}

function LoadPurchaseOrderDropdownBySupplier() {
    try {
        debugger;
        if ($('#SupplierID').val() != "") {
            $("#divPOID").load('/PurchaseOrder/PurchaseOrderDropdown?SupplierID=' + $('#SupplierID').val());
        }
        else {
            $("#divPOID").empty();
            $("#divPOID").append('<input class="form-control HeaderBox text-box single-line" disabled="disabled" id="PurchaseOrderNo" name="PurchaseOrderNo" type="text" value="">');
        }
    }
    catch (ex) {
        console.log(ex.message);
    }
}

function PurchaseOrderOnChange() {
    try {
        debugger;
        $("#PurchaseOrderID").change(function () {
            //$("#PurchaseOrderNo").val($('#PurchaseOrderID').find('option:selected').text());
            if (DataTables.MaterialReceiptDetailTable.rows().data().length > 0) {
                notyConfirm('Are you sure to clear Items in Detail Table?', 'ClearDetailTable()');
            }
            $('#hdnPurchaseOrderID').val($('#PurchaseOrderID').val());
            $('#msgPurchase').hide();
        });
        $('#hdnPurchaseOrderID').val($('#PurchaseOrderID').val());

        if ($('#IsUpdate').val() === "True") {
            //$('#btnLoadPO').addClass('disabled');
            $('#ExistingPurchaseOrder').prop('disabled', true);
            $('#PurchaseOrderID').prop('disabled', true);
            //$('#divSupplierDropdown .input-group-addon').each(function () {
            //    $(this).parent().css("width", "100%");
            //    $(this).hide();
            //});
            $('#SupplierID').prop('disabled', true);
            if ($('#hdnPurchaseOrderID').val() === EmptyGuid || $('#hdnPurchaseOrderID').val() === "" || $('#hdnPurchaseOrderID').val() === null) {
                debugger;
                $('#divPONo').show();
                $('#divPOID').hide();
                $("#PurchaseOrderNo").prop('disabled', true)
            }
        }
    }
    catch (ex) {
        console.log(ex.message);
    }
}

function ClearDetailTable() {
    try {
        debugger;
        DataTables.MaterialReceiptDetailTable.clear().draw(false);
        notyAlert('success', 'Cleared Successfully');
    } catch (ex) {
        console.log(ex.message);
    }
}

function TextBoxValueChanged(thisObj) {
    try {
        debugger;
        var _MaterialReceiptDetail = DataTables.PurchaseOrderDetailTable.row($(thisObj).parents('tr')).data();
        var _MaterialReceiptDetailList = DataTables.PurchaseOrderDetailTable.rows().data();
        var rows = DataTables.PurchaseOrderDetailTable.rows('.selected').indexes();
        for (var i = 0; i < _MaterialReceiptDetailList.length; i++) {
            if (_MaterialReceiptDetailList[i].ID === _MaterialReceiptDetail.ID) {
                _MaterialReceiptDetailList[i].Material.Qty = Math.floor(parseFloat(thisObj.value));
                _MaterialReceiptDetailList[i].Material.UnitCode = "Nos.";
            }
        }
        DataTables.PurchaseOrderDetailTable.clear().rows.add(_MaterialReceiptDetailList).draw(true);
        for (var x = 0; x < rows.length; x++) {
            DataTables.PurchaseOrderDetailTable.rows(rows[x]).select();
        }

    } catch (ex) {
        console.log(ex.message);
    }
}

// onchange for Qty TextBox Value
function QtyTextBoxValueChanged(thisObj) {
    try {
        debugger;
        var table = DataTables.PurchaseOrderDetailTable;
        var _MaterialReceiptDetail = table.row($(thisObj).parents('tr')).data();
        var _MaterialReceiptDetailList = table.rows().data();
        if (!Number.isNaN(parseFloat(thisObj.value))) {
            for (var i = 0; i < _MaterialReceiptDetailList.length; i++) {
                if (_MaterialReceiptDetailList[i].ID === _MaterialReceiptDetail.ID) {
                    _MaterialReceiptDetailList[i].Material.QtyInKG = (parseFloat(thisObj.value));
                    _MaterialReceiptDetailList[i].Material.Qty = Math.floor(_MaterialReceiptDetailList[i].Material.WeightInKG !== 0 ? _MaterialReceiptDetailList[i].Material.QtyInKG / _MaterialReceiptDetailList[i].Material.WeightInKG : 0);
                    _MaterialReceiptDetailList[i].Material.UnitCode = _MaterialReceiptDetailList[i].UnitCode;
                }
            }
        }
        DataTables.PurchaseOrderDetailTable.clear().rows.add(_MaterialReceiptDetailList).select().draw(true);
    } catch (ex) {
        console.log(ex.message);
    }
}

