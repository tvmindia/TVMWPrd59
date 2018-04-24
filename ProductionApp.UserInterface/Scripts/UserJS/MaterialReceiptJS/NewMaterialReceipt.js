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

        $('#SupplierID').select2({});
        PurchaseOrderOnChange();

        $("#SupplierID").change(function () {
            LoadPurchaseOrderDropdownBySupplier();
            PurchaseOrderOnChange();
        });

        if ($('#ID').val !== EmptyGuid && $('#IsUpdate').val() === 'True') {
            BindMaterialReceipt();//Get MaterialReceipt By ID
            $('#lblReceiptNo').text('MRN#: ' + $('#ReceiptNo').val());
            debugger;
            ChangeButtonPatchView('MaterialReceipt', 'divButtonPatch', 'Edit');//divbuttonPatchAddMaterialReceipt
        }
        else {
            $('#lblReceiptNo').text('MRN#: New');

        }

        $('#divPONo,#msgSupplier,#msgPurchase').hide();

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

function ExistingPurchaseOrderOnCheckChanged() {
    debugger;
    if ($('#IsExisting:checked').val() === "true") {
        $('#divPONo').hide();
        $('#divPOID').show();
    } else {
        $('#divPONo').show();
        $('#PurchaseOrderID').val("").trigger('change');
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
    //$("#MaterialID").val('').select2();
    $('#MaterialReceiptDetailModal').modal('show');
}

function BindMaterialDetails(id) {
    debugger;
    var materialVM = GetMaterial(id);
    $('#MaterialReceiptDetail_Material_MaterialCode').val(materialVM.MaterialCode);
    $('#MaterialReceiptDetail_Material_CurrentStock').val(materialVM.CurrentStock);
    //$('#MaterialReceiptDetail_Qty').val(MaterialViewModel.CurrentStock);
    $('#MaterialReceiptDetail_MaterialDesc').val(materialVM.Description);
    $('#MaterialReceiptDetail_UnitCode').val(materialVM.UnitCode);
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
        notyAlert('error', e.message);
    }
}

function AddMaterialReceiptDetailToTable() {
    try {
        debugger;
        if ($("#MaterialID").val() != "")
        {
            _MaterialReceiptDetail = [];
            MaterialReceiptDetailVM = new Object();
            MaterialReceiptDetailVM.MaterialID = $("#MaterialID").val();
            MaterialReceiptDetailVM.Material = new Object();
            MaterialReceiptDetailVM.Material.MaterialCode = $('#MaterialReceiptDetail_Material_MaterialCode').val();
            MaterialReceiptDetailVM.Material.CurrentStock = $('#MaterialReceiptDetail_Material_CurrentStock').val();
            MaterialReceiptDetailVM.MaterialDesc = $('#MaterialReceiptDetail_MaterialDesc').val();
            MaterialReceiptDetailVM.ID = $('#MaterialReceiptDetail_ID').val();
            MaterialReceiptDetailVM.Qty = $('#MaterialReceiptDetail_Qty').val();
            MaterialReceiptDetailVM.UnitCode = $('#MaterialReceiptDetail_UnitCode').val();
            _MaterialReceiptDetail.push(MaterialReceiptDetailVM);

            debugger;
            if (_MaterialReceiptDetail != null) {
                var materialReceiptDetailList = DataTables.MaterialReceiptDetailTable.rows().data();
                if (materialReceiptDetailList.length > 0) {
                    var checkPoint = 0;
                    for (var i = 0; i < materialReceiptDetailList.length; i++) {
                        if (materialReceiptDetailList[i].MaterialID == $("#MaterialID").val()) {
                            materialReceiptDetailList[i].MaterialDesc = $('#MaterialReceiptDetail_MaterialDesc').val();
                            materialReceiptDetailList[i].Qty = parseFloat($('#MaterialReceiptDetail_Qty').val());
                            materialReceiptDetailList[i].UnitCode = $('#MaterialReceiptDetail_UnitCode').val();
                            checkPoint = 1;
                            break;
                        }
                    }
                    if (!checkPoint) {
                        DataTables.MaterialReceiptDetailTable.rows.add(_MaterialReceiptDetail).draw(false);
                    }
                    else {
                        DataTables.MaterialReceiptDetailTable.clear().rows.add(materialReceiptDetailList).draw(false);
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
        var check = 0;

        if ($('#PurchaseOrderNo').val() === "" && ($('#PurchaseOrderID').val() === "" || $('#PurchaseOrderID').val() === undefined)) {
            $('#msgPurchase').show();
            $('#PurchaseOrderNo,#PurchaseOrderID').change(function () {
                $('#msgPurchase').hide();
            });
            check = 1;
        }

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
    try{
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
        MaterialReceiptDetail.UnitCode = materialReceiptDetailVM[r].UnitCode;
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
                        purchaseOrderItemList[r].Qty = parseFloat(purchaseOrderItemList[r].Qty) + parseFloat(purchaseOrderItemList[j].Qty);
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
        _MaterialReceiptDetail = [];

        for (var i = 0; i < purchaseOrderDetailList.length; i++) {
            materialReceiptDetailVM = new Object();
            materialReceiptDetailVM.MaterialID = purchaseOrderDetailList[i].MaterialID;
            materialReceiptDetailVM.Material = new Object();
            materialReceiptDetailVM.Material.MaterialCode = purchaseOrderDetailList[i].MaterialCode;
            materialReceiptDetailVM.MaterialDesc = purchaseOrderDetailList[i].MaterialDesc;
            materialReceiptDetailVM.ID = EmptyGuid;
            materialReceiptDetailVM.Qty = parseFloat(purchaseOrderDetailList[i].Qty);
            materialReceiptDetailVM.UnitCode = purchaseOrderDetailList[i].UnitCode;

            _MaterialReceiptDetail.push(materialReceiptDetailVM);
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
                var materialReceiptDetailList = DataTables.MaterialReceiptDetailTable.rows().data();
                if (materialReceiptDetailList.length > 0) {
                    
                    for (var i = 0; i < materialReceiptDetailList.length; i++) {
                        if (materialReceiptDetailList[i].MaterialID === _MaterialReceiptDetail[r].MaterialID) {
                            materialReceiptDetailList[i].MaterialDesc = _MaterialReceiptDetail[r].MaterialDesc;
                            materialReceiptDetailList[i].Qty = _MaterialReceiptDetail[r].Qty;
                            materialReceiptDetailList[i].UnitCode = _MaterialReceiptDetail[r].UnitCode;
                            checkPoint = 1;
                            _MaterialReceiptDetail.splice(r, 1);
                            break;
                        }
                    }
                    if (checkPoint) {
                        DataTables.MaterialReceiptDetailTable.clear().rows.add(materialReceiptDetailList).draw(false);
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
        var materialReceiptDetailViewModel = DataTables.MaterialReceiptDetailTable.row($(curObj).parents('tr')).data();
        $("#MaterialID").val(materialReceiptDetailViewModel.MaterialID).trigger("change");
        debugger;
        $('#MaterialReceiptDetail_Material_MaterialCode').val(materialReceiptDetailViewModel.Material.MaterialCode);
        $('#MaterialReceiptDetail_MaterialDesc').val(materialReceiptDetailViewModel.MaterialDesc);
        $('#MaterialReceiptDetail_ID').val(materialReceiptDetailViewModel.ID);
        $('#MaterialReceiptDetail_Qty').val(materialReceiptDetailViewModel.Qty);
        $('#MaterialReceiptDetail_UnitCode').val(materialReceiptDetailViewModel.UnitCode);
    }
    catch(e)
    {
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
        if ($('#SupplierID').val() != ""){
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
    try{
        debugger;
        $("#PurchaseOrderID").change(function () {
            $("#PurchaseOrderNo").val($('#PurchaseOrderID').find('option:selected').text());
            $('#msgPurchase').hide();
        });
        $('#hdnPurchaseOrderID').val($('#PurchaseOrderID').val());

    }
    catch (ex) {
        console.log(ex.message);
    }
}