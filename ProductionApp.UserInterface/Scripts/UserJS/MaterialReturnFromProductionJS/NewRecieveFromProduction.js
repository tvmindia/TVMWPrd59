//*****************************************************************************
//*****************************************************************************
//Author: Angel
//CreatedDate: 08-Mar-2018 
//LastModified: 09-Mar-2018 
//FileName: NewRecieveFromProduction.js
//Description: Client side coding for RecieveFromProduction.
//******************************************************************************
//******************************************************************************

//--Global Declaration--//
var DataTables = {};
var _MaterialReturnDetail = [];
var _MaterialReturnDetailList = [];
var _SlNo = 1;
var EmptyGuid = "00000000-0000-0000-0000-000000000000";
$(document).ready(function () {
    debugger;
    try 
    {
        $("#ReceivedBy").select2({
        });
        $("#ReturnBy").select2({
        });

        DataTables.MaterialReturnFromProductionDetailTable = $('#tblReturnFromProductionDetail').DataTable(
     {
         dom: '<"pull-right"f>rt<"bottom"ip><"clear">',
         ordering: false,
         searching: false,
         paging: false,
         data: null,
         autoWidth: false,
         "bInfo":false,
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
         { "data": "Qty", render: function (data, type, row) { return data }, "defaultContent": "<i></i>" },
         { "data": null, "orderable": false, "defaultContent": '<a href="#" class="actionLink"  onclick="MaterialEdit(this)" ><i class="glyphicon glyphicon-pencil" aria-hidden="true"></i></a> | <a href="#" class="DeleteLink"  onclick="Delete(this)" ><i class="glyphicon glyphicon-trash" aria-hidden="true"></i></a>' },
         ],
         columnDefs: [{ "targets": [0, 1], "visible": false, searchable: false },
             { className: "text-left", "targets": [2, 3, 4, 5] },
             { className: "text-right", "targets": [6] },
             { className: "text-center", "targets": [7] },
             { "targets": [2], "width": "2%", },
             { "targets": [3], "width": "5%" },
             { "targets": [4], "width": "15%" },
             { "targets": [7], "width": "3%" },
             { "targets": [6], "width": "4%" },
             { "targets": [4, 5], "width": "5%" }
         ],

     });

        $("#MaterialID").change(function () {
            debugger;
            BindRawMaterialDetails(this.value)
        });
        if ($('#IsUpdate').val() == 'True') {
            debugger;
            BindReturnFromProductionByID()
            ChangeButtonPatchView('MaterialReturnFromProduction', 'divbuttonPatchReturnFromProduction', 'Edit');
        }
        else {
            $('#lblReturnNo').text('Receive From Production# : New');
        }
    }
    catch (e) {
        console.log(e.message);
    }
});
//material popup
function ShowReturnFromProductionDetailsModal()
{
    debugger;
    $('#MaterialID').val('').select2();
    $('#MaterialReturnFromProductionDetail_Material_MaterialCode').val('');
    $('#MaterialReturnFromProductionDetail_MaterialDesc').val('');
    $('#MaterialReturnFromProductionDetail_UnitCode').val('');
    $('#MaterialReturnFromProductionDetail_Qty').val('');
    $('#AddReturnFromProductionItemModal').modal('show');
}
function BindRawMaterialDetails(ID) {
    debugger;
    var result = GetMaterial(ID);
    $('#MaterialReturnFromProductionDetail_Material_MaterialCode').val(result.MaterialCode);
    $('#MaterialReturnFromProductionDetail_MaterialDesc').val(result.Description);
    $('#MaterialReturnFromProductionDetail_UnitCode').val(result.UnitCode);
    $('#MaterialReturnFromProductionDetail_Qty').val(result.Qty);
}
function GetMaterial(ID) {
    try {
        debugger;
        var data = { "id": ID };
        var jsonData = {};
        var result = "";
        var message = "";
        var materialViewModel = new Object();
        jsonData = GetDataFromServer("MaterialReturnFromProduction/GetMaterial/", data);
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

function AddReturnFromProductionItem() {
    debugger;
    if ($('#MaterialID').val() != "" && $('#MaterialReturnFromProductionDetail_Qty').val() != "") {
        _MaterialReturnDetail = [];
        AddMaterialReturn = new Object();
        AddMaterialReturn.MaterialID = $('#MaterialID').val();
        AddMaterialReturn.Material = new Object();
        AddMaterialReturn.Material.MaterialCode = $('#MaterialReturnFromProductionDetail_Material_MaterialCode').val();
        AddMaterialReturn.MaterialDesc = $('#MaterialReturnFromProductionDetail_MaterialDesc').val();
        AddMaterialReturn.UnitCode = $('#MaterialReturnFromProductionDetail_UnitCode').val();
        AddMaterialReturn.Qty = $('#MaterialReturnFromProductionDetail_Qty').val();
        _MaterialReturnDetail.push(AddMaterialReturn);

        if (_MaterialReturnDetail != null) {
            var materialReturnDetailList = DataTables.MaterialReturnFromProductionDetailTable.rows().data();
            if (materialReturnDetailList.length > 0) {
                var checkPoint = 0;
                for (var i = 0; i < materialReturnDetailList.length; i++) {
                    if (materialReturnDetailList[i].MaterialID == $('#MaterialID').val()) {
                        materialReturnDetailList[i].MaterialDesc = $('#MaterialReturnFromProductionDetail_MaterialDesc').val();
                        materialReturnDetailList[i].UnitCode = $('#MaterialReturnFromProductionDetail_UnitCode').val();
                        materialReturnDetailList[i].Qty = $('#MaterialReturnFromProductionDetail_Qty').val();
                        checkPoint = 1;
                        break;
                    }
                }
                if (!checkPoint) {

                    DataTables.MaterialReturnFromProductionDetailTable.rows.add(_MaterialReturnDetail).draw(false);
                }
                else {
                    DataTables.MaterialReturnFromProductionDetailTable.clear().rows.add(materialReturnDetailList).draw(false);
                }
            }
            else {

                DataTables.MaterialReturnFromProductionDetailTable.rows.add(_MaterialReturnDetail).draw(false);
            }
        }
        $('#AddReturnFromProductionItemModal').modal('hide');
    }
    else {
        notyAlert('warning', "Material and Quantity fields are required ");
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
    var data = DataTables.MaterialReturnFromProductionDetailTable.rows().data();
    for (var r = 0; r < data.length; r++) {
        MaterialReturnDetail = new Object();
        MaterialReturnDetail.ID = data[r].ID;
        MaterialReturnDetail.HeaerID = data[r].HeaerID;
        MaterialReturnDetail.MaterialID = data[r].MaterialID;
        MaterialReturnDetail.MaterialDesc = data[r].MaterialDesc;
        MaterialReturnDetail.UnitCode = data[r].UnitCode;
        MaterialReturnDetail.Qty = data[r].Qty;
        _MaterialReturnDetailList.push(MaterialReturnDetail);
    }
}
function SaveSuccessReturnFromProduction(data, status) {
    debugger;
    var JsonResult = JSON.parse(data)
    switch (JsonResult.Result) {
        case "OK":
            $('#IsUpdate').val('True');
            $('#ID').val(JsonResult.Records.ID)
            notyAlert("success", JsonResult.Records.Message)
            BindReturnFromProductionByID();
            _SlNo = 1;
            
            ChangeButtonPatchView('MaterialReturnFromProduction', 'divbuttonPatchReturnFromProduction', 'Edit');
            break;
        case "ERROR":
            notyAlert("danger", JsonResult.Message)
            break;
        default:
            notyAlert("danger", JsonResult.Message)
            break;
    }
}

function Reset()
{
    BindReturnFromProductionByID();
}

function BindReturnFromProductionByID() {
    debugger;
    ChangeButtonPatchView('MaterialReturnFromProduction', 'divbuttonPatchReturnFromProduction', 'New');
    _SlNo = 1;
    var ID = $('#ID').val();
    var result = GetReturnFromProductionByID(ID);
    $('#ID').val(result.ID);
    $('#ReturnNo').val(result.ReturnNo);
    $('#ReturnDateFormatted').val(result.ReturnDateFormatted);
    $('#ReceivedBy').val(result.ReceivedBy).select2();
    $('#ReturnBy').val(result.ReturnBy).select2();
    $('#GeneralNotes').val(result.GeneralNotes);
    $('#lblReturnNo').text('Receive From Production# : ' + result.ReturnNo);
    BindMaterialReturnDetailTable(ID);
}
function GetReturnFromProductionByID(ID) {
    try {
        debugger;
        var data = { "id": ID };
        var jsonData = {};
        var result = "";
        var message = "";
        var materialReturnVM = new Object();
        jsonData = GetDataFromServer("MaterialReturnFromProduction/GetReturnFromProduction/", data);
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
    DataTables.MaterialReturnFromProductionDetailTable.clear().rows.add(GetReturnFromProductionDetail(ID)).draw(true);
}

function GetReturnFromProductionDetail(ID) {
    try {
        debugger;
        var data = { "id": ID };
        var jsonData = {};
        var result = "";
        var message = "";
        var materialReturnDetailVM = new Object();
        jsonData = GetDataFromServer("MaterialReturnFromProduction/GetReturnFromProductionDetail/", data);
        if (jsonData != '') {
            jsonData = JSON.parse(jsonData);
            result = jsonData.Result;
            message = jsonData.Message;
            materialReturnDetailVM = jsonData.Records;
        }
        if (result == "OK") {
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

function MaterialEdit(curObj) {
    debugger;
    $('#AddReturnFromProductionItemModal').modal('show');

    var materialReturnDetailVM = DataTables.MaterialReturnFromProductionDetailTable.row($(curObj).parents('tr')).data();
    _SlNo = 1;
    if ((materialReturnDetailVM != null) && (materialReturnDetailVM.MaterialID != null)) {
        $("#MaterialID").val(materialReturnDetailVM.MaterialID).select2();
        $('#MaterialReturnFromProductionDetail_Material_MaterialCode').val(materialReturnDetailVM.Material.MaterialCode);
        $('#MaterialReturnFromProductionDetail_MaterialDesc').val(materialReturnDetailVM.MaterialDesc);
        $('#MaterialReturnFromProductionDetail_UnitCode').val(materialReturnDetailVM.UnitCode);
        $('#MaterialReturnFromProductionDetail_Qty').val(materialReturnDetailVM.Qty);
    }
}

// Delete
function Delete(curObj) {
    debugger;
    var materialReturnDetailVM = DataTables.MaterialReturnFromProductionDetailTable.row($(curObj).parents('tr')).data();
    var materialReturnDetailVMIndex = DataTables.MaterialReturnFromProductionDetailTable.row($(curObj).parents('tr')).index();

    if ((materialReturnDetailVM != null) && (materialReturnDetailVM.ID != null)) {
        notyConfirm('Are you sure to delete?', 'DeleteItem("' + materialReturnDetailVM.ID + '")');

    }
    else {
        var res = notyConfirm('Are you sure to delete?', 'DeleteTempItem("' + materialReturnDetailVMIndex + '")');
    }
}

function DeleteTempItem(materialReturnDetailVMIndex) {
    debugger;
    var Itemtabledata = DataTables.MaterialReturnFromProductionDetailTable.rows().data();
    Itemtabledata.splice(materialReturnDetailVMIndex, 1);
    _SlNo = 1;
    DataTables.MaterialReturnFromProductionDetailTable.clear().rows.add(Itemtabledata).draw(false);
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
        jsonData = GetDataFromServer("MaterialReturnFromProduction/DeleteReturnFromProductionDetail/", data);
        if (jsonData != '') {
            jsonData = JSON.parse(jsonData);
            result = jsonData.Result;
            materialReturnDetailVM = jsonData.Records;
            message = jsonData.Message;
        }
        switch (result) {
            case "OK":
                notyAlert('success', message);
                BindReturnFromProductionByID();

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
    notyConfirm('Are you sure to delete?', 'DeleteIssueToProduction()');
}

function DeleteIssueToProduction() {
    try {
        debugger;
        var id = $('#ID').val();
        if (id != '' && id != null) {
            var data = { "id": id };
            var jsonData = {};
            var result = "";
            var message = "";
            var materialReturnVM = new Object();
            jsonData = GetDataFromServer("MaterialReturnFromProduction/DeleteReturnFromProduction/", data);
            if (jsonData != '') {
                jsonData = JSON.parse(jsonData);
                result = jsonData.Result;
                materialReturnVM = jsonData.Record;
                message = jsonData.Message;
            }
            if (result == "OK") {
                notyAlert('success', materialReturnVM.message);
                window.location.replace("NewRecieveFromProduction?code=STR");
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

function Reset()
{
    BindReturnFromProductionByID();
}
