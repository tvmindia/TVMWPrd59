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
var _dataTable = {};
var _emptyGuid = "00000000-0000-0000-0000-000000000000";
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
            if (this.value != "") {
                BindRawMaterialDetails(this.value)
            }
            else {
                $('#MaterialReturnDetail_Material_MaterialCode').val('');
                $('#MaterialReturnDetail_MaterialDesc').val('');
                $('#MaterialReturnDetail_UnitCode').val('');
                $('#MaterialReturnDetail_Qty').val('');
            }
        });

        _dataTable.MaterialReturnToSupplierDetailTable = $('#tblReturnToSupplierDetail').DataTable(
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
         { "data": "TaxableAmt", render: function (data, type, row) { return data }, "defaultContent": "<i></i>" },
         { "data": "TaxTypeDescription", render: function (data, type, row) { if (row.TaxTypeDescription) { return data } else return "-" }, "defaultContent": "<i></i>" },
         { "data": "Amount", render: function (data, type, row) { return data }, "defaultContent": "<i></i>" },
         { "data": null, "orderable": false, "defaultContent": '<a href="#" class="actionLink"  onclick="MaterialEdit(this)" ><i class="glyphicon glyphicon-edit" aria-hidden="true"></i></a> | <a href="#" class="DeleteLink"  onclick="Delete(this)" ><i class="glyphicon glyphicon-trash" aria-hidden="true"></i></a>' },
         ],
         columnDefs: [{ "targets": [0, 1], "visible": false, searchable: false },
             { className: "text-left", "targets": [2, 3, 4,5,9] },
             { className: "text-right", "targets": [6,7,8,10] },
             { className: "text-center", "targets": [11] },
             { "targets": [2], "width": "2%", },
             { "targets": [3], "width": "8%" },
             { "targets": [4], "width": "38%" },
             { "targets": [11], "width": "7%" },
             { "targets": [6], "width": "8%" },
             { "targets": [7], "width": "8%" },
             { "targets": [8], "width": "8%" },
             { "targets": [9], "width": "6%" },
             { "targets": [10], "width": "8%" },
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
        $("#SupplierID").change(function () {
            SupplierDetails();
        });
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
    $('#TaxTypeCode').val('');
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
//bind supplier address 
function SupplierDetails() {
    debugger;
    var supplierid = $('#SupplierID').val();
    supplierVM = GetSupplierDetails(supplierid);
    $('#BillAddress').val(supplierVM.BillingAddress);
    $('#ShippingAddress').val(supplierVM.ShippingAddress);
}
function GetSupplierDetails(supplierid) {
    try {
        debugger;
        var data = { "supplierid": supplierid };
        var jsonData = {};
        var result = "";
        var message = "";
        var supplierVM = new Object();

        jsonData = GetDataFromServer("MaterialReturn/GetSupplierDetails/", data);
        if (jsonData != '') {
            jsonData = JSON.parse(jsonData);
            result = jsonData.Result;
            message = jsonData.Message;
            supplierVM = jsonData.Records;
        }
        if (result == "OK") {

            return supplierVM;
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
        AddMaterialReturn.TaxTypeCode = $('#TaxTypeCode').val();
        if ($('#TaxTypeCode').val() != "" && $('#TaxTypeCode').val() != undefined) {
            var taxTypeVM = GetTaxTypeByCode($('#TaxTypeCode').val());
            AddMaterialReturn.TaxTypeDescription = (taxTypeVM.Description);
            AddMaterialReturn.CGSTPerc = parseFloat(taxTypeVM.CGSTPercentage);
            AddMaterialReturn.SGSTPerc = parseFloat(taxTypeVM.SGSTPercentage);
            AddMaterialReturn.IGSTPerc = parseFloat(taxTypeVM.IGSTPercentage);
        }
        else {
            AddMaterialReturn.CGSTPerc = 0;
            AddMaterialReturn.SGSTPerc = 0;
            AddMaterialReturn.IGSTPerc = 0;
        }
        AddMaterialReturn.TaxableAmt = parseFloat(AddMaterialReturn.Qty) * parseFloat(AddMaterialReturn.Rate);
        AddMaterialReturn.Amount = roundoff(parseFloat(AddMaterialReturn.TaxableAmt) + (parseFloat(AddMaterialReturn.TaxableAmt) * parseFloat(AddMaterialReturn.CGSTPerc) / 100) + (parseFloat(AddMaterialReturn.TaxableAmt) * parseFloat(AddMaterialReturn.SGSTPerc) / 100) + (parseFloat(AddMaterialReturn.TaxableAmt) * parseFloat(AddMaterialReturn.IGSTPerc) / 100));
        _MaterialReturnDetail.push(AddMaterialReturn);

        if (_MaterialReturnDetail != null) {
            
            var materialReturnDetailList = _dataTable.MaterialReturnToSupplierDetailTable.rows().data();
            if (materialReturnDetailList.length > 0) {
                var checkPoint = 0;
                for (var i = 0; i < materialReturnDetailList.length; i++) {
                    if (materialReturnDetailList[i].MaterialID == $('#MaterialID').val())
                    {
                        materialReturnDetailList[i].MaterialDesc = $('#MaterialReturnDetail_MaterialDesc').val();
                        materialReturnDetailList[i].UnitCode = AddMaterialReturn.UnitCode;
                        materialReturnDetailList[i].Qty = AddMaterialReturn.Qty;
                        materialReturnDetailList[i].Rate = AddMaterialReturn.Rate;
                        materialReturnDetailList[i].TaxTypeCode = AddMaterialReturn.TaxTypeCode;
                        materialReturnDetailList[i].TaxTypeDescription = AddMaterialReturn.TaxTypeDescription;
                        materialReturnDetailList[i].TaxableAmt = AddMaterialReturn.TaxableAmt;
                        materialReturnDetailList[i].Amount = AddMaterialReturn.Amount;
                        checkPoint = 1;
                        break;
                    }
                }
                if (!checkPoint) {
                    _SlNo = _dataTable.MaterialReturnToSupplierDetailTable.rows().count() + 1;
                    _dataTable.MaterialReturnToSupplierDetailTable.rows.add(_MaterialReturnDetail).draw(false);
                }
                else {
                    _dataTable.MaterialReturnToSupplierDetailTable.clear().rows.add(materialReturnDetailList).draw(false);
                }
            }
            else {
               
                _dataTable.MaterialReturnToSupplierDetailTable.rows.add(_MaterialReturnDetail).draw(false);
            }
        }
        $('#AddReturnToSupplierItemModal').modal('hide');
        catlculateTotal();
    }
    else {
        notyAlert('warning', "Mandatory fields are missing ");
    }
}
function catlculateTotal() {
    debugger;
    var total = 0;
    var taxableAmt = 0;
    var totalTax = 0;
    var materialReturnDetailVM = new Object();
    materialReturnDetailVM=_dataTable.MaterialReturnToSupplierDetailTable.rows().data();
    for (var i = 0; i < materialReturnDetailVM.length; i++) {
        totalTax = totalTax + parseFloat(materialReturnDetailVM[i].TaxableAmt);
        total = total + parseFloat(materialReturnDetailVM[i].Amount);
    } 
    $('#lblTotal').text(roundoff(total));
    $('#lblTaxableAmt').text(roundoff(totalTax));
    $('#lblTaxAmt').text(roundoff(parseFloat(total) - parseFloat(totalTax)));
}
function MaterialEdit(curObj) {
    debugger;
    $('#AddReturnToSupplierItemModal').modal('show');

    var materialReturnDetailVM = _dataTable.MaterialReturnToSupplierDetailTable.row($(curObj).parents('tr')).data();
    _SlNo = 1;
    if ((materialReturnDetailVM != null) && (materialReturnDetailVM.MaterialID != null)) {
        $("#MaterialID").val(materialReturnDetailVM.MaterialID).select2();
        $('#MaterialReturnDetail_Material_MaterialCode').val(materialReturnDetailVM.Material.MaterialCode);
        $('#MaterialReturnDetail_MaterialDesc').val(materialReturnDetailVM.MaterialDesc);
        $('#MaterialReturnDetail_UnitCode').val(materialReturnDetailVM.UnitCode);
        $('#MaterialReturnDetail_Qty').val(materialReturnDetailVM.Qty);
        $('#MaterialReturnDetail_Rate').val(materialReturnDetailVM.Rate);
        $('#TaxTypeCode').val(materialReturnDetailVM.TaxTypeCode);
    }
}
function GetTaxTypeByCode(Code) {
    try {
        debugger;
        var data = { "Code": Code };
        var result = "";
        var message = "";
        var jsonData = {};
        var taxTypeVM = new Object();
        jsonData = GetDataFromServer("MaterialReturn/GetTaxtype/", data);
        if (jsonData != '') {
            jsonData = JSON.parse(jsonData);
            result = jsonData.Result;
            message = jsonData.Message;
            taxTypeVM = jsonData.Records;
        }
        if (result == "OK") {
            return taxTypeVM;
        }
        if (result == "ERROR") {
            alert(Message);
        }
    }
    catch (e) {
        notyAlert('error', e.message);
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
        notyAlert('warning', 'Please add item details!');
    }
}
function AddMaterialReturnDetailList() {
    debugger;
    var data = _dataTable.MaterialReturnToSupplierDetailTable.rows().data();
    for (var r = 0; r < data.length; r++) {
        MaterialReturnDetail = new Object();
        MaterialReturnDetail.ID = data[r].ID;
        MaterialReturnDetail.MaterialIssueID = data[r].MaterialIssueID;
        MaterialReturnDetail.MaterialID = data[r].MaterialID;
        MaterialReturnDetail.MaterialDesc = data[r].MaterialDesc;
        MaterialReturnDetail.UnitCode = data[r].UnitCode;
        MaterialReturnDetail.Qty = data[r].Qty;
        MaterialReturnDetail.Rate = data[r].Rate;
        MaterialReturnDetail.TaxTypeCode = data[r].TaxTypeCode;
        if (MaterialReturnDetail.TaxTypeCode != "" && MaterialReturnDetail.TaxTypeCode != undefined) {
            var taxTypeVM = GetTaxTypeByCode($('#TaxTypeCode').val());
            MaterialReturnDetail.CGSTPerc = parseFloat(taxTypeVM.CGSTPercentage);
            MaterialReturnDetail.SGSTPerc = parseFloat(taxTypeVM.SGSTPercentage);
            MaterialReturnDetail.IGSTPerc = parseFloat(taxTypeVM.IGSTPercentage);
        }
        else {
            MaterialReturnDetail.CGSTPerc = 0;
            MaterialReturnDetail.SGSTPerc = 0;
            MaterialReturnDetail.IGSTPerc = 0;
        }
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
    //$('#ReturnDateFormatted').val(result.ReturnDateFormatted);
    $('#ReturnDateFormatted').datepicker('setDate', result.ReturnDateFormatted);
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
    _dataTable.MaterialReturnToSupplierDetailTable.clear().rows.add(GetReturnToSupplierDetail(ID)).draw(true);
    catlculateTotal();
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
                materialReturnDetailVM[i].TaxableAmt = materialReturnDetailVM[i].Qty * materialReturnDetailVM[i].Rate;
                materialReturnDetailVM[i].Amount = roundoff(parseFloat(materialReturnDetailVM[i].TaxableAmt) + (parseFloat(materialReturnDetailVM[i].TaxableAmt) * parseFloat(materialReturnDetailVM[i].CGSTPerc) / 100) + (parseFloat(materialReturnDetailVM[i].TaxableAmt) * parseFloat(materialReturnDetailVM[i].SGSTPerc) / 100) + (parseFloat(materialReturnDetailVM[i].TaxableAmt) * parseFloat(materialReturnDetailVM[i].IGSTPerc) / 100));
            }
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
    var materialReturnDetailVM = _dataTable.MaterialReturnToSupplierDetailTable.row($(curObj).parents('tr')).data();
    var materialReturnDetailVMIndex = _dataTable.MaterialReturnToSupplierDetailTable.row($(curObj).parents('tr')).index();

    if ((materialReturnDetailVM != null) && (materialReturnDetailVM.ID != null)) {
        notyConfirm('Are you sure to delete?', 'DeleteItem("' + materialReturnDetailVM.ID + '")');

    }
    else {
        var res = notyConfirm('Are you sure to delete?', 'DeleteTempItem("' + materialReturnDetailVMIndex + '")');
    }
}

function DeleteTempItem(materialReturnDetailVMIndex) {
    debugger;
    var Itemtabledata = _dataTable.MaterialReturnToSupplierDetailTable.rows().data();
    Itemtabledata.splice(materialReturnDetailVMIndex, 1);
    _SlNo = 1;
    _dataTable.MaterialReturnToSupplierDetailTable.clear().rows.add(Itemtabledata).draw(false);
    notyAlert('success', 'Deleted successfully');
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
