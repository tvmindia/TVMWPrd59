//*****************************************************************************
//*****************************************************************************
//Author: Arul
//CreatedDate: 20-Feb-2018 
//LastModified: 02-Mar-2018
//FileName: NewMaterialReceipt.js
//Description: Client side coding for Adding / Updating Material Receipts
//******************************************************************************
//******************************************************************************

var DataTables = {};
var EmptyGuid = "00000000-0000-0000-0000-000000000000";
var _MaterialReceiptDetail = [];
var _MaterialReceiptDetailList = [];

$(document).ready(function () {
    debugger;
    try {

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
                    { "data": "Material.MaterialCode", render: function (data, type, row) { return data }, "defaultContent": "<i></i>" },
                    { "data": "MaterialDesc", render: function (data, type, row) { return data }, "defaultContent": "<i></i>" },
                    { "data": "Qty", render: function (data, type, row) { return roundoff(data) }, "defaultContent": "<i></i>" },
                    { "data": "UnitCode", render: function (data, type, row) { return data }, "defaultContent": "<i></i>" },
                    { "data": null, "orderable": false, "defaultContent": '<a href="#" class="actionLink"  onclick="DetailEdit(this)" ><i class="glyphicon glyphicon-pencil" aria-hidden="true"></i></a> | <a href="#" class="DeleteLink"  onclick="Delete(this)" ><i class="glyphicon glyphicon-trash" aria-hidden="true"></i></a>' },
                ],
                columnDefs: [{ "targets": [0, 1], "visible": false, searchable: false },
                    { className: "text-center", "targets": [6], "width": "7%" },
                    { className: "text-right", "targets": [4] },
                    { className: "text-left", "targets": [2, 3, 5] }
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
                  { "data": "Checkbox", "defaultContent": "" },
                  { "data": "MaterialID", "defaultContent": "<i>-</i>" },
                  { "data": "MaterialCode", "defaultContent": "<i>-</i>" },
                  { "data": "MaterialDesc", "defaultContent": "<i>-</i>" },
                  { "data": "Qty", "defaultContent": "<i>-</i>" },
                  { "data": "UnitCode", "defaultContent": "<i>-</i>" },
                ],
                columnDefs: [{ orderable: false, className: 'select-checkbox', "targets": 1 },
                    { "targets": [0, 2], "visible": false, "searchable": false },
                    { className: "text-right", "targets": [5] },
                    { className: "text-left", "targets": [3, 4, 6] },
                    { className: "text-center", "targets": [1] },
                    { "targets": [3, 4, 5, 6], "bSortable": false }
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

        $('#PurchaseOrderID,#SupplierID').select2({});
        if ($('#ID').val !== EmptyGuid && $('#IsUpdate').val() === 'True') {
            BindMaterialReceipt();//Get MaterialReceipt By ID
            $('#lblReceiptNo').text('MRN#: ' + $('#ReceiptNo').val());
            debugger;
            ChangeButtonPatchView('MaterialReceipt', 'divButtonPatch', 'Edit');//divbuttonPatchAddMaterialReceipt
        }
        else {
            $('#lblReceiptNo').text('MRN#: New');

        }

        $('#divPONo').hide();
        $("#PurchaseOrderID").change(function () {
            $("#PurchaseOrderNo").val($('#PurchaseOrderID').find('option:selected').text());
        });
        $("#MaterialID").change(function () {
            BindMaterialDetails(this.value)
        });
        $("#SupplierID").change(function () {
            $('#SupID').val($("#SupplierID").val());
        });

    }
    catch (ex) {
        console.log(ex.message);
    }
});

function OnCheckChanged() {
    debugger;
    if ($('#IsExisting:checked').val() === "true") {
        $('#divPONo').hide();
        $('#divPOID').show();
    } else {
        $('#divPONo').show();
        $('#PurchaseOrderID').val('').select2();
        $('#divPOID').hide();
    }
}

function AddMaterialReceiptDetail() {
    debugger;
    $('#MaterialReceiptDetail_Material_MaterialCode').val('');
    $('#MaterialReceiptDetail_Material_CurrentStock').val('');
    $('#MaterialReceiptDetail_Qty').val('');
    $('#MaterialReceiptDetail_MaterialDesc').val('');
    $('#MaterialReceiptDetail_UnitCode').val('');
    $("#MaterialID").val('').select2();
    $('#MaterialReceiptDetailModal').modal('show');
}

function BindMaterialDetails(id) {
    debugger;
    var MaterialViewModel = GetMaterial(id);
    $('#MaterialReceiptDetail_Material_MaterialCode').val(MaterialViewModel.MaterialCode);
    $('#MaterialReceiptDetail_Material_CurrentStock').val(MaterialViewModel.CurrentStock);
    //$('#MaterialReceiptDetail_Qty').val(MaterialViewModel.CurrentStock);
    $('#MaterialReceiptDetail_MaterialDesc').val(MaterialViewModel.Description);
    $('#MaterialReceiptDetail_UnitCode').val(MaterialViewModel.UnitCode);
}

function GetMaterial(id) {
    try {
        debugger;
        var data = { "id": id };
        var result = "";
        var message = "";
        var MaterialViewModel = new Object();
        var jsonData = GetDataFromServer("MaterialReceipt/GetMaterial/", data);
        if (jsonData != '') {
            jsonData = JSON.parse(jsonData);
            result = jsonData.Result;
            MaterialViewModel = jsonData.Record;
            message = jsonData.Message;
        }
        if (result == "OK") {
            return MaterialViewModel;
        }
        if (result == "ERROR") {
            alert(message);
        }
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}

function AddMaterialReceiptDetailToTable() {
    try {
        debugger;
        if ($("#MaterialID").val() != "")
        {
            _MaterialReceiptDetail = [];
            MaterialReceiptData = new Object();
            MaterialReceiptData.MaterialID = $("#MaterialID").val();
            MaterialReceiptData.Material = new Object();
            MaterialReceiptData.Material.MaterialCode = $('#MaterialReceiptDetail_Material_MaterialCode').val();
            MaterialReceiptData.Material.CurrentStock = $('#MaterialReceiptDetail_Material_CurrentStock').val();
            MaterialReceiptData.MaterialDesc = $('#MaterialReceiptDetail_MaterialDesc').val();
            MaterialReceiptData.ID = $('#MaterialReceiptDetail_ID').val();
            MaterialReceiptData.Qty = $('#MaterialReceiptDetail_Qty').val();
            MaterialReceiptData.UnitCode = $('#MaterialReceiptDetail_UnitCode').val();
            _MaterialReceiptDetail.push(MaterialReceiptData);

            debugger;
            if (_MaterialReceiptDetail != null) {
                var MaterialReceiptDetailList = DataTables.MaterialReceiptDetailTable.rows().data();
                if (MaterialReceiptDetailList.length > 0) {
                    var checkPoint = 0;
                    for (var i = 0; i < MaterialReceiptDetailList.length; i++) {
                        if (MaterialReceiptDetailList[i].MaterialID == $("#MaterialID").val()) {
                            MaterialReceiptDetailList[i].MaterialDesc = $('#MaterialReceiptDetail_MaterialDesc').val();
                            MaterialReceiptDetailList[i].Qty = parseFloat($('#MaterialReceiptDetail_Qty').val());
                            MaterialReceiptDetailList[i].UnitCode = $('#MaterialReceiptDetail_UnitCode').val();
                            checkPoint = 1;
                            break;
                        }
                    }
                    if (!checkPoint) {
                        DataTables.MaterialReceiptDetailTable.rows.add(_MaterialReceiptDetail).draw(false);
                    }
                    else {
                        DataTables.MaterialReceiptDetailTable.clear().rows.add(MaterialReceiptDetailList).draw(false);
                    }
                }
                else {
                    DataTables.MaterialReceiptDetailTable.rows.add(_MaterialReceiptDetail).draw(false);
                }
            }
            $('#MaterialReceiptDetailModal').modal('hide');
        }
        else {
            notyAlert('warning', "Material is Empty");
        }

    } catch (e) {
        console.log('error: '+ e.message);
    }
}

function Save() {
    try{
        debugger;
        $("#DetailJSON").val('');
        _MaterialReceiptDetailList = [];
        AddMaterialReceiptDetailList();
        if (_MaterialReceiptDetailList.length > 0) {
            var result = JSON.stringify(_MaterialReceiptDetailList);
            $("#DetailJSON").val(result);
            $('#btnSave').trigger('click');
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
    var JsonResult = JSON.parse(data)
    switch (JsonResult.Result) {
        case "OK":
            $('#IsUpdate').val('True');
            $('#ID').val(JsonResult.Records.ID)
            notyAlert("success", JsonResult.Records.Message)
            ChangeButtonPatchView('MaterialReceipt', 'divButtonPatch', 'Edit');//divbuttonPatchAddMaterialReceipt
            break;
        case "ERROR":
            notyAlert("danger", JsonResult.Message)
            break;
        default:
            notyAlert("danger", JsonResult.Message)
            break;
    }
}

function AddMaterialReceiptDetailList() {
    debugger;
    var data = DataTables.MaterialReceiptDetailTable.rows().data();
    for (var r = 0; r < data.length; r++) {
        MaterialReceiptDetail = new Object();
        MaterialReceiptDetail.ID = data[r].ID;
        MaterialReceiptDetail.MaterialID = data[r].MaterialID;
        MaterialReceiptDetail.MaterialDesc = data[r].MaterialDesc;
        MaterialReceiptDetail.Qty = data[r].Qty;
        MaterialReceiptDetail.UnitCode = data[r].UnitCode;
        _MaterialReceiptDetailList.push(MaterialReceiptDetail);
    }
}

function LoadPODetail() {
    debugger;
    if ($('#PurchaseOrderID').val() !== "") {
        $('#PurchaseOrderDetailsModal').modal('show');
        var id = $('#PurchaseOrderID').val();
        BindPurchaseOrderDetailTable(id);
    } else {
        notyAlert('warning', 'Purchase Order Not selcted!');
    }
}

function BindPurchaseOrderDetailTable(id){
    debugger;
    DataTables.PurchaseOrderDetailTable.clear().rows.add(GetPurchaseOrderItem(id)).select().draw(false);
}

function GetPurchaseOrderItem(id) {
    try {
        debugger;
        var data = { "id": id };
        var PurchaseOrderItemList = [];
        var result = "";
        var message = "";
        var jsonData = GetDataFromServer("MaterialReceipt/GetAllPurchaseOrderItem/", data);
        if (jsonData != '') {
            jsonData = JSON.parse(jsonData);
            result = jsonData.Result;
            message = jsonData.Message;
            PurchaseOrderItemList = jsonData.Records;
        }
        if (result == "OK") {
            return PurchaseOrderItemList;
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
        var PurchaseOrderItemList = DataTables.PurchaseOrderDetailTable.rows(".selected").data();
        var mergedRows = []; //to store rows after merging
        var currentMaterial;

        if (PurchaseOrderItemList.length > 0) {
            debugger;
            for (var r = 0; r < PurchaseOrderItemList.length; r++) {
                currentMaterial = PurchaseOrderItemList[r].MaterialID
                for (var j = r + 1; j < PurchaseOrderItemList.length; j++) {
                    if (PurchaseOrderItemList[j].MaterialID == currentMaterial) {
                        PurchaseOrderItemList[r].Qty = parseFloat(PurchaseOrderItemList[r].Qty) + parseFloat(PurchaseOrderItemList[j].Qty);
                        PurchaseOrderItemList.splice(j, 1);//removing duplicate after adding value 
                        j = j - 1;// for avoiding skipping row while checking
                    }
                }
                mergedRows.push(PurchaseOrderItemList[r])// adding rows to merge array
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
        _MaterialReceiptDetail = [];

        for (var i = 0; i < purchaseOrderDetailList.length; i++) {
            MaterialReceiptDetailViewModel = new Object();
            MaterialReceiptDetailViewModel.MaterialID = purchaseOrderDetailList[i].MaterialID;
            MaterialReceiptDetailViewModel.Material = new Object();
            MaterialReceiptDetailViewModel.Material.MaterialCode = purchaseOrderDetailList[i].MaterialCode;
            MaterialReceiptDetailViewModel.MaterialDesc = purchaseOrderDetailList[i].MaterialDesc;
            MaterialReceiptDetailViewModel.ID = EmptyGuid;
            MaterialReceiptDetailViewModel.Qty = parseFloat(purchaseOrderDetailList[i].Qty);
            MaterialReceiptDetailViewModel.UnitCode = purchaseOrderDetailList[i].UnitCode;

            _MaterialReceiptDetail.push(MaterialReceiptDetailViewModel);
        }

        RebindMaterialReceiptDetailTable(_MaterialReceiptDetail);

    }
    catch (ex) {
        console.log(ex.message)
    }
}

function RebindMaterialReceiptDetailTable(_MaterialReceiptDetail) {
    try {
        debugger;
        var checkPoint = 0;
        if (_MaterialReceiptDetail != null) {
            for (var r = 0; r < _MaterialReceiptDetail.length; r++) {
                var MaterialReceiptDetailList = DataTables.MaterialReceiptDetailTable.rows().data();
                if (MaterialReceiptDetailList.length > 0) {
                    
                    for (var i = 0; i < MaterialReceiptDetailList.length; i++) {
                        if (MaterialReceiptDetailList[i].MaterialID === _MaterialReceiptDetail[r].MaterialID) {
                            MaterialReceiptDetailList[i].MaterialDesc = _MaterialReceiptDetail[r].MaterialDesc;
                            MaterialReceiptDetailList[i].Qty = _MaterialReceiptDetail[r].Qty;
                            MaterialReceiptDetailList[i].UnitCode = _MaterialReceiptDetail[r].UnitCode;
                            checkPoint = 1;
                            _MaterialReceiptDetail.splice(r, 1);
                            break;
                        }
                    }
                    if (checkPoint) {
                        DataTables.MaterialReceiptDetailTable.clear().rows.add(MaterialReceiptDetailList).draw(false);
                    }
                }
            }
            DataTables.MaterialReceiptDetailTable.rows.add(_MaterialReceiptDetail).draw(false);
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
        var MaterialReceiptDetailViewModel = DataTables.MaterialReceiptDetailTable.row($(curObj).parents('tr')).data();
        $("#MaterialID").val(MaterialReceiptDetailViewModel.MaterialID).trigger("change");
        $('#MaterialReceiptDetail_Material_MaterialCode').val(MaterialReceiptDetailViewModel.Material.MaterialCode);
        $('#MaterialReceiptDetail_MaterialDesc').val(MaterialReceiptDetailViewModel.MaterialDesc);
        $('#MaterialReceiptDetail_ID').val(MaterialReceiptDetailViewModel.ID);
        $('#MaterialReceiptDetail_Qty').val(MaterialReceiptDetailViewModel.Qty);
        $('#MaterialReceiptDetail_UnitCode').val(MaterialReceiptDetailViewModel.UnitCode);
    }
    catch(e)
    {
        console.log(e.message)
    }
}

function Delete(curobj) {
    debugger;
    var MaterialReceiptDetailViewModel = DataTables.MaterialReceiptDetailTable.row($(curobj).parents('tr')).data();
    var rowindex = DataTables.MaterialReceiptDetailTable.row($(curobj).parents('tr')).index();

    if ((MaterialReceiptDetailViewModel != null) && (MaterialReceiptDetailViewModel.ID != EmptyGuid)) {
        notyConfirm('Are you sure to delete?', 'DeleteItem("' + MaterialReceiptDetailViewModel.ID + '")');
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
        var status = new Object();
        var jsonData = GetDataFromServer("MaterialReceipt/DeleteMaterialReceiptDetail/", data);
        if (jsonData != '') {
            jsonData = JSON.parse(jsonData);
            result = jsonData.Result;
            message = jsonData.Message;
            status = jsonData.Record;
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
        return status;
    }
    catch (e) {

        notyAlert('error', e.message);
    }
}

function DeleteClick() {
    try{
        debugger;
        var id = $('#ID').val();
        if (id !== EmptyGuid)
            notyConfirm('Are you sure to delete?', 'DeleteMaterialReceipt("' + id + '")');
        else
            notyAlert('error',"Cannot Delete")
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
        var status = new Object();
        var jsonData = GetDataFromServer("MaterialReceipt/DeleteMaterialReceipt/", data);
        if (jsonData != '') {
            jsonData = JSON.parse(jsonData);
            result = jsonData.Result;
            message = jsonData.Message;
            status = jsonData.Record;
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
        return status;
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
    $('#PurchaseOrderNo').val(MaterialReceiptViewModel.PurchaseOrderNo);
    $('#PurchaseOrderID').val(MaterialReceiptViewModel.PurchaseOrderID).select2();
    $('#SupplierID').val(MaterialReceiptViewModel.SupplierID).select2();
    $('#GeneralNotes').val(MaterialReceiptViewModel.GeneralNotes);
    BindMaterialReceiptDetailTable(id);//Get All MaterialReceiptDetails By HeaderID
}

function GetMaterialReceipt(id) {
    try {
        debugger;
        var data = { "id": id };
        var result = "";
        var message = "";
        var MaterialReceiptViewModel = new Object();
        var jsonData = GetDataFromServer("MaterialReceipt/GetMaterialReceipt/", data);
        if (jsonData != '') {
            jsonData = JSON.parse(jsonData);
            result = jsonData.Result;
            message = jsonData.Message;
            MaterialReceiptViewModel = jsonData.Records;
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
        return MaterialReceiptViewModel;
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
        var MaterialReceiptDetailList = [];
        var result = "";
        var message = "";
        var jsonData = GetDataFromServer("MaterialReceipt/GetAllMaterialReceiptDetail/", data);
        if (jsonData != '') {
            jsonData = JSON.parse(jsonData);
            result = jsonData.Result;
            message = jsonData.Message;
            MaterialReceiptDetailList = jsonData.Records;
        }
        if (result == "OK") {
            return MaterialReceiptDetailList;
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
