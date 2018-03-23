//*****************************************************************************
//*****************************************************************************
//Author: Angel
//CreatedDate: 21-Mar-2018 
//LastModified: 21-Mar-2018 
//FileName: NewMaterialReturn.js
//Description: Client side coding for NewMaterialReturn.
//******************************************************************************
//******************************************************************************

//--Global Declaration--//
var DataTables = {};
var EmptyGuid = "00000000-0000-0000-0000-000000000000";
var _SlNo = 1;
var _MaterialReturnDetail = [];
var _MaterialReturnDetailList = [];
$(document).ready(function () {
    debugger;
    try {
        $("#ReturnBy").select2({
        });
        $("#SupplierID").select2({
        });
        $("#MaterialID").select2({ dropdownParent: $("#AddReturnToSupplierItemModal") });
        $("#MaterialID").change(function () {
            BindRawMaterialDetails(this.value)
        });

        DataTables.MaterialReturnToSupplierDetailTable = $('#tblReturnToSupplierDetail').DataTable(
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
         { "data": "MaterialID", "defaultContent": "<i></i>" },
         {
             "data": "", render: function (data, type, row) {
                 debugger;
                 return _SlNo++
             }, "defaultContent": "<i></i>"
         },
         { "data": "Material.MaterialCode", render: function (data, type, row) { return data }, "defaultContent": "<i></i>" },
         { "data": "MaterialDesc", render: function (data, type, row) { return data }, "defaultContent": "<i></i>" },
         { "data": "UnitCode", render: function (data, type, row) { return data }, "defaultContent": "<i></i>" },
         { "data": "Rate", render: function (data, type, row) { return data }, "defaultContent": "<i></i>" },
         { "data": "Qty", render: function (data, type, row) { return data }, "defaultContent": "<i></i>" },
         { "data": "SGSTPerc", render: function (data, type, row) { return data }, "defaultContent": "<i></i>" },
         { "data": "CGSTPerc", render: function (data, type, row) { return data }, "defaultContent": "<i></i>" },
         { "data": "IGSTPerc", render: function (data, type, row) { return data }, "defaultContent": "<i></i>" },
         { "data": "Amount", render: function (data, type, row) { return data }, "defaultContent": "<i></i>" },
         { "data": null, "orderable": false, "defaultContent": '<a href="#" class="DeleteLink"  onclick="Delete(this)" ><i class="glyphicon glyphicon-trash" aria-hidden="true"></i></a> | <a href="#" class="actionLink"  onclick="MaterialEdit(this)" ><i class="glyphicon glyphicon-pencil" aria-hidden="true"></i></a>' },
         ],
         columnDefs: [{ "targets": [0, 1], "visible": false, searchable: false },
             { className: "text-left", "targets": [2, 3, 4, 5] },
             { className: "text-right", "targets": [6,7,8,9,10,11] },
             { className: "text-center", "targets": [12] },
             { "targets": [2], "width": "2%", },
             { "targets": [3], "width": "5%" },
             { "targets": [4], "width": "36%" },
             { "targets": [12], "width": "4%" },
             { "targets": [6], "width": "8%" },
             { "targets": [7], "width": "6%" },
             { "targets": [8], "width": "6%" },
             { "targets": [9], "width": "6%" },
             { "targets": [10], "width": "6%" },
             { "targets": [4, 5], "width": "5%" }
         ],

     });
        if ($('#IsUpdate').val() == 'True') {
            debugger;
            BindReturnToSupplier()
            ChangeButtonPatchView('MaterialReturn', 'divbuttonPatchReturnToSupplier', 'Edit');
        }
        else {
            $('#lblReturnSlipNo').text('Return To Supplier# : New');
        }

    }
    catch (e) {
        console.log(e.message);
    }
});
function ShowReturnToSupplierDetailsModal()
{
    $('#MaterialID').val('').select2();
    $('#MaterialReturnDetail_Material_MaterialCode').val('');
    $('#MaterialReturnDetail_MaterialDesc').val('');
    $('#MaterialReturnDetail_UnitCode').val('');
    $('#MaterialReturnDetail_Qty').val('');
    $('#MaterialReturnDetail_Rate').val('');
    $('#MaterialReturnDetail_CGSTPerc').val('');
    $('#MaterialReturnDetail_SGSTPerc').val('');
    $('#MaterialReturnDetail_IGSTPerc').val('');
    $('#AddReturnToSupplierItemModal').modal('show');
}
function BindRawMaterialDetails(ID) {
    debugger;
    var result = GetMaterial(ID);
    _SlNo = 1;
    $('#MaterialReturnDetail_Material_MaterialCode').val(result.MaterialCode);
    $('#MaterialReturnDetail_MaterialDesc').val(result.Description);
    $('#MaterialReturnDetail_UnitCode').val(result.UnitCode);
    $('#MaterialReturnDetail_Qty').val(result.Qty);
}

function GetMaterial(ID) {
    try {
        debugger;
        var data = { "id": ID };
        var jsonData = {};
        var result = "";
        var message = "";
        var materialViewModel = new Object();
        jsonData = GetDataFromServer("MaterialReturn/GetMaterial/", data);
        if (jsonData != '') {
            jsonData = JSON.parse(jsonData);
            result = jsonData.Result;
            materialViewModel = jsonData.Records;
            message = jsonData.Message;
        }
        if (result == "OK") {
            return materialViewModel;
        }
        if (result == "ERROR") {
            alert(message);
        }
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}
function AddReturnToSupplierItem()
{
    debugger;
    if ($('#MaterialID').val() != "" && $('#MaterialReturnDetail_Qty').val() != "" && $('#MaterialReturnDetail_Rate').val() != "" && $('#MaterialReturnDetail_Rate').val() != 0 && $('#MaterialReturnDetail_Qty').val() != 0) {
        _MaterialReturnDetail = [];
        AddMaterialReturn = new Object();
        AddMaterialReturn.MaterialID = $('#MaterialID').val();
        AddMaterialReturn.Material = new Object();
        AddMaterialReturn.Material.MaterialCode = $('#MaterialReturnDetail_Material_MaterialCode').val();
        AddMaterialReturn.MaterialDesc = $('#MaterialReturnDetail_MaterialDesc').val();
        AddMaterialReturn.UnitCode = $('#MaterialReturnDetail_UnitCode').val();
        AddMaterialReturn.Qty = $('#MaterialReturnDetail_Qty').val();
        AddMaterialReturn.Rate = $('#MaterialReturnDetail_Rate').val();
        if ($('#MaterialReturnDetail_CGSTPerc').val() != "")
            AddMaterialReturn.CGSTPerc = $('#MaterialReturnDetail_CGSTPerc').val();
        else
            AddMaterialReturn.CGSTPerc = 0;
        if ($('#MaterialReturnDetail_SGSTPerc').val() != "")
            AddMaterialReturn.SGSTPerc = $('#MaterialReturnDetail_SGSTPerc').val();
        else
            AddMaterialReturn.SGSTPerc = 0;
        if ($('#MaterialReturnDetail_IGSTPerc').val() != "")
            AddMaterialReturn.IGSTPerc = $('#MaterialReturnDetail_IGSTPerc').val();
        else
            AddMaterialReturn.IGSTPerc = 0;
        AddMaterialReturn.Amount = parseFloat(AddMaterialReturn.Qty) * parseFloat(AddMaterialReturn.Rate);
        AddMaterialReturn.Amount = roundoff(parseFloat(AddMaterialReturn.Amount) + (parseFloat(AddMaterialReturn.Amount) * parseFloat(AddMaterialReturn.CGSTPerc) / 100) + (parseFloat(AddMaterialReturn.Amount) * parseFloat(AddMaterialReturn.SGSTPerc) / 100) + (parseFloat(AddMaterialReturn.Amount) * parseFloat(AddMaterialReturn.IGSTPerc) / 100));
        _MaterialReturnDetail.push(AddMaterialReturn);

        if (_MaterialReturnDetail != null) {
            
            var materialReturnDetailList = DataTables.MaterialReturnToSupplierDetailTable.rows().data();
            if (materialReturnDetailList.length > 0) {
                var checkPoint = 0;
                for (var i = 0; i < materialReturnDetailList.length; i++) {
                    if (materialReturnDetailList[i].MaterialID == $('#MaterialID').val()) {
                        materialReturnDetailList[i].MaterialDesc = $('#MaterialReturnDetail_MaterialDesc').val();
                        materialReturnDetailList[i].UnitCode = $('#MaterialReturnDetail_UnitCode').val();
                        materialReturnDetailList[i].Qty = $('#MaterialReturnDetail_Qty').val();
                        materialReturnDetailList[i].Rate = $('#MaterialReturnDetail_Rate').val();
                        materialReturnDetailList[i].CGSTPerc = $('#MaterialReturnDetail_CGSTPerc').val();
                        materialReturnDetailList[i].SGSTPerc = $('#MaterialReturnDetail_SGSTPerc').val();
                        materialReturnDetailList[i].IGSTPerc = $('#MaterialReturnDetail_IGSTPerc').val();
                        materialReturnDetailList[i].Amount = parseFloat(materialReturnDetailList[i].Qty) * parseFloat(materialReturnDetailList[i].Rate);
                        materialReturnDetailList[i].Amount = parseFloat(materialReturnDetailList[i].Amount) + (parseFloat(materialReturnDetailList[i].Amount) * parseFloat(materialReturnDetailList[i].CGSTPerc) / 100) + (parseFloat(materialReturnDetailList[i].Amount) * parseFloat(materialReturnDetailList[i].SGSTPerc) / 100) + (parseFloat(materialReturnDetailList[i].Amount) * parseFloat(materialReturnDetailList[i].IGSTPerc) / 100);
                        checkPoint = 1;
                        break;
                    }
                }
                if (!checkPoint) {
                    
                    DataTables.MaterialReturnToSupplierDetailTable.rows.add(_MaterialReturnDetail).draw(false);
                }
                else {
                    DataTables.MaterialReturnToSupplierDetailTable.clear().rows.add(materialReturnDetailList).draw(false);
                }
            }
            else {
               
                DataTables.MaterialReturnToSupplierDetailTable.rows.add(_MaterialReturnDetail).draw(false);
            }
        }
        $('#AddReturnToSupplierItemModal').modal('hide');
        catlculateTotal();
    }
    else {
        notyAlert('warning', "Material,Quantity and Rate fields are required ");
    }
}
function catlculateTotal() {
    debugger;
    var total = 0;
    var taxableAmt = 0;
    var materialReturnDetailVM = new Object();
    materialReturnDetailVM=DataTables.MaterialReturnToSupplierDetailTable.rows().data();
    for (var i = 0; i < materialReturnDetailVM.length; i++) {
        materialReturnDetailVM[i].Amount = materialReturnDetailVM[i].Qty * materialReturnDetailVM[i].Rate;
        taxableAmt = materialReturnDetailVM[i].Qty * materialReturnDetailVM[i].Rate;
        materialReturnDetailVM[i].Amount = roundoff(parseFloat(materialReturnDetailVM[i].Amount) + (parseFloat(materialReturnDetailVM[i].Amount) * parseFloat(materialReturnDetailVM[i].CGSTPerc) / 100) + (parseFloat(materialReturnDetailVM[i].Amount) * parseFloat(materialReturnDetailVM[i].SGSTPerc) / 100) + (parseFloat(materialReturnDetailVM[i].Amount) * parseFloat(materialReturnDetailVM[i].IGSTPerc) / 100));
        total =total + taxableAmt + (taxableAmt * parseFloat(materialReturnDetailVM[i].CGSTPerc) / 100) + (taxableAmt * parseFloat(materialReturnDetailVM[i].SGSTPerc) / 100) + (taxableAmt * parseFloat(materialReturnDetailVM[i].IGSTPerc) / 100);
    }
    $('#lblTotal').text('₹ '+roundoff(total));
}
function MaterialEdit(curObj) {
    debugger;
    $('#AddReturnToSupplierItemModal').modal('show');

    var materialReturnDetailVM = DataTables.MaterialReturnToSupplierDetailTable.row($(curObj).parents('tr')).data();
    _SlNo = 1;
    if ((materialReturnDetailVM != null) && (materialReturnDetailVM.MaterialID != null)) {
        $("#MaterialID").val(materialReturnDetailVM.MaterialID).select2();
        $('#MaterialReturnDetail_Material_MaterialCode').val(materialReturnDetailVM.Material.MaterialCode);
        $('#MaterialReturnDetail_MaterialDesc').val(materialReturnDetailVM.MaterialDesc);
        $('#MaterialReturnDetail_UnitCode').val(materialReturnDetailVM.UnitCode);
        $('#MaterialReturnDetail_Qty').val(materialReturnDetailVM.Qty);
        $('#MaterialReturnDetail_Rate').val(materialReturnDetailVM.Rate);
        $('#MaterialReturnDetail_CGSTPerc').val(materialReturnDetailVM.CGSTPerc);
        $('#MaterialReturnDetail_SGSTPerc').val(materialReturnDetailVM.SGSTPerc);
        $('#MaterialReturnDetail_IGSTPerc').val(materialReturnDetailVM.IGSTPerc);
    }
}
function Save() {
    debugger;
    $("#DetailJSON").val('');
    _MaterialReturnDetailList = [];
    AddMaterialReturnDetailList();
    if (_MaterialReturnDetailList.length > 0) {
        debugger;
        var result = JSON.stringify(_MaterialReturnDetailList);
        $("#DetailJSON").val(result);
        $('#btnSave').trigger('click');
        _SlNo = 1;
    }
    else {
        notyAlert('warning', 'Please Add Material Details!');
    }
}
function AddMaterialReturnDetailList() {
    debugger;
    var data = DataTables.MaterialReturnToSupplierDetailTable.rows().data();
    for (var r = 0; r < data.length; r++) {
        MaterialReturnDetail = new Object();
        MaterialReturnDetail.ID = data[r].ID;
        MaterialReturnDetail.MaterialIssueID = data[r].MaterialIssueID;
        MaterialReturnDetail.MaterialID = data[r].MaterialID;
        MaterialReturnDetail.MaterialDesc = data[r].MaterialDesc;
        MaterialReturnDetail.UnitCode = data[r].UnitCode;
        MaterialReturnDetail.Qty = data[r].Qty;
        MaterialReturnDetail.Rate = data[r].Rate;
        MaterialReturnDetail.CGSTPerc = data[r].CGSTPerc;
        MaterialReturnDetail.SGSTPerc = data[r].SGSTPerc;
        MaterialReturnDetail.IGSTPerc = data[r].IGSTPerc;
        _MaterialReturnDetailList.push(MaterialReturnDetail);
    }
}
function SaveSuccessReturnToSupplier(data, status) {
    debugger;
    var JsonResult = JSON.parse(data)
    switch (JsonResult.Result) {
        case "OK":
            $('#IsUpdate').val('True');
            $('#ID').val(JsonResult.Records.ID)
            BindReturnToSupplier();
            _SlNo = 1;
            notyAlert("success", JsonResult.Records.Message)
            ChangeButtonPatchView('MaterialReturn', 'divbuttonPatchReturnToSupplier', 'Edit');
            break;
        case "ERROR":
            notyAlert("danger", JsonResult.Message)
            break;
        default:
            notyAlert("danger", JsonResult.Message)
            break;
    }
}
function Reset() {
    BindReturnToSupplier();
}

function BindReturnToSupplier() {
    debugger;
    ChangeButtonPatchView('MaterialReturn', 'divbuttonPatchAddIssueToProduction', 'New');
    _SlNo = 1;
    var ID = $('#ID').val();
    var result = GetReturnToSupplier(ID);
    $('#ID').val(result.ID);
    $('#ReturnSlipNo').val(result.ReturnSlipNo);
    $('#ReturnDateFormatted').val(result.ReturnDateFormatted);
    $('#ReturnBy').val(result.ReturnBy).select2();
    $('#SupplierID').val(result.SupplierID).select2();
    $('#GeneralNotes').val(result.GeneralNotes);
    $('#BillAddress').val(result.BillAddress);
    $('#ShippingAddress').val(result.ShippingAddress);
    $('#lblReturnSlipNo').text('Return To Supplier# : ' + result.ReturnSlipNo);
    BindMaterialReturnDetailTable(ID);
}

function GetReturnToSupplier(ID) {
    try {
        debugger;
        var data = { "id": ID };
        var jsonData = {};
        var result = "";
        var message = "";
        var materialReturnVM = new Object();
        jsonData = GetDataFromServer("MaterialReturn/GetMaterialReturn/", data);
        if (jsonData != '') {
            jsonData = JSON.parse(jsonData);
            result = jsonData.Result;
            materialReturnVM = jsonData.Record;
            message = jsonData.Message;
        }
        if (result == "OK") {
            return materialReturnVM;
        }
        if (result == "ERROR") {
            alert(message);
        }
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}

function BindMaterialReturnDetailTable(ID) {
    debugger;
    var MaterialReturnDetail = new Object();
    MaterialReturnDetail = GetReturnToSupplierDetail(ID);
    for (var i = 0; i < MaterialReturnDetail.length; i++)
    {

    }
    DataTables.MaterialReturnToSupplierDetailTable.clear().rows.add(GetReturnToSupplierDetail(ID)).draw(true);
}

function GetReturnToSupplierDetail(ID) {
    try {
        debugger;
        var data = { "id": ID };
        var jsonData = {};
        var result = "";
        var message = "";
        var Tot = 0;
        var taxableAmt=0;
        var materialReturnDetailVM = new Object();
        jsonData = GetDataFromServer("MaterialReturn/GetMaterialReturnDetail/", data);
        if (jsonData != '') {
            jsonData = JSON.parse(jsonData);
            result = jsonData.Result;
            message = jsonData.Message;
            materialReturnDetailVM = jsonData.Records;
            
        }
        if (result == "OK") {
            for (var i = 0; i < materialReturnDetailVM.length; i++) {
                materialReturnDetailVM[i].Amount = materialReturnDetailVM[i].Qty * materialReturnDetailVM[i].Rate;
                taxableAmt = materialReturnDetailVM[i].Qty * materialReturnDetailVM[i].Rate;
                materialReturnDetailVM[i].Amount = roundoff(parseFloat(materialReturnDetailVM[i].Amount) + (parseFloat(materialReturnDetailVM[i].Amount) * parseFloat(materialReturnDetailVM[i].CGSTPerc) / 100) + (parseFloat(materialReturnDetailVM[i].Amount) * parseFloat(materialReturnDetailVM[i].SGSTPerc) / 100) + (parseFloat(materialReturnDetailVM[i].Amount) * parseFloat(materialReturnDetailVM[i].IGSTPerc) / 100));
                Tot = Tot + taxableAmt + (taxableAmt * parseFloat(materialReturnDetailVM[i].CGSTPerc) / 100) + (taxableAmt * parseFloat(materialReturnDetailVM[i].SGSTPerc) / 100) + (taxableAmt * parseFloat(materialReturnDetailVM[i].IGSTPerc) / 100);
            }
            $('#lblTotal').text('₹ '+roundoff(Tot));
            return materialReturnDetailVM;
        }
        if (result == "ERROR") {
            alert(message);
        }
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}
function Delete(curObj) {
    debugger;
    var materialReturnDetailVM = DataTables.MaterialReturnToSupplierDetailTable.row($(curObj).parents('tr')).data();
    var materialReturnDetailVMIndex = DataTables.MaterialReturnToSupplierDetailTable.row($(curObj).parents('tr')).index();

    if ((materialReturnDetailVM != null) && (materialReturnDetailVM.ID != null)) {
        notyConfirm('Are you sure to delete?', 'DeleteItem("' + materialReturnDetailVM.ID + '")');

    }
    else {
        var res = notyConfirm('Are you sure to delete?', 'DeleteTempItem("' + materialReturnDetailVMIndex + '")');
    }
}

function DeleteTempItem(materialReturnDetailVMIndex) {
    debugger;
    var Itemtabledata = DataTables.MaterialReturnToSupplierDetailTable.rows().data();
    Itemtabledata.splice(materialReturnDetailVMIndex, 1);
    _SlNo = 1;
    DataTables.MaterialReturnToSupplierDetailTable.clear().rows.add(Itemtabledata).draw(false);
    notyAlert('success', 'Deleted Successfully');
}
//-------------for delete from details table which saved in db-------
function DeleteItem(ID) {
    try {
        debugger;
        var data = { "id": ID };
        var jsonData = {};
        var result = "";
        var message = "";
        var materialReturnDetailVM = new Object();
        jsonData = GetDataFromServer("MaterialReturn/DeleteMaterialReturnDetail/", data);
        if (jsonData != '') {
            jsonData = JSON.parse(jsonData);
            result = jsonData.Result;
            materialReturnDetailVM = jsonData.Records;
            message = jsonData.Message;
        }
        switch (result) {
            case "OK":
                notyAlert('success', message);
                BindReturnToSupplier();

                break;
            case "ERROR":
                notyAlert('error', message);
                break;
            default:
                break;
        }
        return jsonData.Records;
    }
    catch (e) {

        notyAlert('error', e.message);
    }
}

function DeleteClick() {
    notyConfirm('Are you sure to delete?', 'DeleteMaterialReturn()');
}
function DeleteMaterialReturn() {
    try {
        debugger;
        var id = $('#ID').val();
        if (id != '' && id != null) {
            var data = { "id": id };
            var jsonData = {};
            var result = "";
            var message = "";
            var materialReturnVM = new Object();
            jsonData = GetDataFromServer("MaterialReturn/DeleteMaterialReturn/", data);
            if (jsonData != '') {
                jsonData = JSON.parse(jsonData);
                result = jsonData.Result;
                materialReturnVM = jsonData.Record;
                message = jsonData.Message;
            }
            if (result == "OK") {
                notyAlert('success', materialReturnVM.message);
                window.location.replace("NewMaterialReturn?code=STR");
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
