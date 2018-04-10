var DataTables = {};
var _MaterialIssueDetail = [];
var _MaterialIssueDetailList = [];
var _SlNo=1;
var EmptyGuid = "00000000-0000-0000-0000-000000000000";
$(document).ready(function () {

    try {
        $("#IssueTo").select2({
            placeholder: "Select Employee..",

        });
        $("#IssuedBy").select2({
            placeholder: "Select Employee..",

        });       
        $("#MaterialID").select2({ dropdownParent: $("#AddIssueToProductionItemModal") });

        DataTables.MaterialIssueDetailTable = $('#tblIssueToProductionDetail').DataTable(
     {
         dom: '<"pull-right"f>rt<"bottom"ip><"clear">',
         ordering:false,
         searching: false,
         paging: false,
         data: null,
         autoWidth: false,
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
         { "data": null, "orderable": false, "defaultContent": '<a href="#" class="DeleteLink"  onclick="Delete(this)" ><i class="glyphicon glyphicon-trash" aria-hidden="true"></i></a> | <a href="#" class="actionLink"  onclick="MaterialEdit(this)" ><i class="glyphicon glyphicon-pencil" aria-hidden="true"></i></a>' },
         ], 
         columnDefs: [{ "targets": [0,1], "visible": false, searchable: false },                              
             { className: "text-left", "targets": [2, 3, 4, 5, 6] },
             {"targets":[2],"width":"2%",},
             { "targets": [3], "width": "5%" },
             { "targets": [4], "width": "15%" },
             { "targets": [7], "width": "3%" },
             {"targets":[6],"width":"4%"},
             { "targets": [4,5], "width": "5%" }
         ],
        
     });

            $("#MaterialID").change(function () {
                debugger;
            BindRawMaterialDetails(this.value)
            });
        if ($('#IsUpdate').val() == 'True') {
            debugger;
            BindIssueToProductionByID()
            ChangeButtonPatchView('IssueToProduction', 'divbuttonPatchAddIssueToProduction', 'Edit');
        }
        else {
            $('#lblIssueNo').text('Issue To Production# : New');
        }
}
    catch (x) {
        //this will show the error msg in the browser console(F12) 
    console.log(x.message);
}
    
});
function ShowIssueToProductionDetailsModal()
{
    try{
        debugger;
        $('#MaterialID').val('').select2();
        $('#MaterialIssueDetail_Material_MaterialCode').val('');
        $('#MaterialIssueDetail_MaterialDesc').val('');
        $('#MaterialIssueDetail_UnitCode').val('');
        $('#MaterialIssueDetail_Qty').val('');
        $('#AddIssueToProductionItemModal').modal('show');
    }
    catch(e)
    {
        console.log(e.message);
    }
}

function BindRawMaterialDetails(ID)
{
    try{
        debugger;
        var result = GetMaterial(ID);
        _SlNo = 1;
        $('#MaterialIssueDetail_Material_MaterialCode').val(result.MaterialCode);
        $('#MaterialIssueDetail_MaterialDesc').val(result.Description);
        $('#MaterialIssueDetail_UnitCode').val(result.UnitCode);
        $('#MaterialIssueDetail_Qty').val(result.Qty);
    }
    catch(e)
    {
        console.log(e.message);
    }
}

function GetMaterial(ID) {
    try {
        debugger;
        var data = { "id": ID };
        var jsonData = {};
        var result = "";
        var message = "";
        var materialViewModel = new Object();
        jsonData = GetDataFromServer("IssueToProduction/GetMaterial/", data);
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

function AddIssueToProductItem()
{
    try{
        debugger;
        if ($('#MaterialID').val() != "" && $('#MaterialIssueDetail_Qty').val()!="")
        {
            _MaterialIssueDetail = [];
            AddMaterialIssue = new Object();
            AddMaterialIssue.MaterialID = $('#MaterialID').val();
            AddMaterialIssue.Material = new Object();
            AddMaterialIssue.Material.MaterialCode = $('#MaterialIssueDetail_Material_MaterialCode').val();
            AddMaterialIssue.MaterialDesc = $('#MaterialIssueDetail_MaterialDesc').val();
            AddMaterialIssue.UnitCode = $('#MaterialIssueDetail_UnitCode').val();
            AddMaterialIssue.Qty = $('#MaterialIssueDetail_Qty').val();
            _MaterialIssueDetail.push(AddMaterialIssue);

            if(_MaterialIssueDetail!=null)
            {
                var materialIssueDetailList = DataTables.MaterialIssueDetailTable.rows().data();
                if (materialIssueDetailList.length > 0)
                {
                    var checkPoint = 0;
                    for (var i = 0; i < materialIssueDetailList.length; i++)
                    {
                        if (materialIssueDetailList[i].MaterialID == $('#MaterialID').val())
                        {                       
                            materialIssueDetailList[i].MaterialDesc = $('#MaterialIssueDetail_MaterialDesc').val();
                            materialIssueDetailList[i].UnitCode = $('#MaterialIssueDetail_UnitCode').val();
                            materialIssueDetailList[i].Qty = $('#MaterialIssueDetail_Qty').val();
                            checkPoint = 1;
                            break;
                        }
                    }
                    if(!checkPoint)
                    {
                  
                        DataTables.MaterialIssueDetailTable.rows.add(_MaterialIssueDetail).draw(false);
                    }
                    else
                    {
                        DataTables.MaterialIssueDetailTable.clear().rows.add(materialIssueDetailList).draw(false);
                    }
                }
                else
                {
                
                    DataTables.MaterialIssueDetailTable.rows.add(_MaterialIssueDetail).draw(false);
                }
            }
            $('#AddIssueToProductionItemModal').modal('hide');
        }
        else
        {
            notyAlert('warning', "Material and Quantity fields are required ");
        }
    }
    catch (e) {
        console.log(e.message);
    }

}

function MaterialEdit(curObj)
{
    try{
        debugger;
        $('#AddIssueToProductionItemModal').modal('show');
    
        var materialIssueDetailVM = DataTables.MaterialIssueDetailTable.row($(curObj).parents('tr')).data();
        _SlNo = 1;
        //DataTables.MaterialIssueDetailTable.clear().rows.add(Itemtabledata).draw(false);
        if ((materialIssueDetailVM != null) && (materialIssueDetailVM.MaterialID != null))
        {
            $("#MaterialID").val(materialIssueDetailVM.MaterialID).select2();
            $('#MaterialIssueDetail_Material_MaterialCode').val(materialIssueDetailVM.Material.MaterialCode);
            $('#MaterialIssueDetail_MaterialDesc').val(materialIssueDetailVM.MaterialDesc);
            $('#MaterialIssueDetail_UnitCode').val(materialIssueDetailVM.UnitCode);
            $('#MaterialIssueDetail_Qty').val(materialIssueDetailVM.Qty);
        }
    }
    catch (e) {
        console.log(e.message);
    }
}

function Delete(curObj)
{
    try
    {
        debugger;
        var materialIssueDetailVM = DataTables.MaterialIssueDetailTable.row($(curObj).parents('tr')).data();
        var materialIssueDetailVMIndex = DataTables.MaterialIssueDetailTable.row($(curObj).parents('tr')).index();

        if ((materialIssueDetailVM != null) && (materialIssueDetailVM.ID != null))
        {
            notyConfirm('Are you sure to delete?', 'DeleteItem("' + materialIssueDetailVM.ID + '")');
        
        }
        else
        {
            var res = notyConfirm('Are you sure to delete?', 'DeleteTempItem("' + materialIssueDetailVMIndex + '")');
        }
    }
    catch (e) {
        console.log(e.message);
    }
}

function DeleteTempItem(materialIssueDetailVMIndex)
{
    try
    {
        debugger;
        var Itemtabledata = DataTables.MaterialIssueDetailTable.rows().data();
        Itemtabledata.splice(materialIssueDetailVMIndex, 1);
        _SlNo = 1;
        DataTables.MaterialIssueDetailTable.clear().rows.add(Itemtabledata).draw(false);    
        notyAlert('success', 'Deleted Successfully');
    }
    catch (e) {
        console.log(e.message);
    }
}

//-------------for delete from details table which saved in db-------
function DeleteItem(ID)
{
    try {
        debugger;
        var data = { "id": ID };
        var jsonData = {};
        var result = "";
        var message = "";
        var materialIssueDetailVM = new Object();
        jsonData = GetDataFromServer("IssueToProduction/DeleteIssueToProductionDetail/", data);
        if (jsonData != '') {
            jsonData = JSON.parse(jsonData);
            result = jsonData.Result;
            materialIssueDetailVM = jsonData.Records;
            message = jsonData.Message;
        }
        switch (result) {
            case "OK":
                notyAlert('success', message);
                BindIssueToProductionByID();
               
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

function DeleteClick()
{
    notyConfirm('Are you sure to delete?', 'DeleteIssueToProduction()');
}

function DeleteIssueToProduction()
{
    try {
        debugger;
        var id = $('#ID').val();
        if (id != '' && id != null) {
            var data = { "id": id };
            var jsonData = {};
            var result = "";
            var message = "";
            var materialIssueVM = new Object();
            jsonData = GetDataFromServer("IssueToProduction/DeleteIssueToProduction/", data);
            if (jsonData != '') {
                jsonData = JSON.parse(jsonData);
                result = jsonData.Result;
                materialIssueVM = jsonData.Record;
                message = jsonData.Message;
            }
            if (result == "OK") {
                notyAlert('success', materialIssueVM.message);
                window.location.replace("AddIssueToProduction?code=STR");
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

function Save()
{
    try
    {
        debugger;
        $("#DetailJSON").val('');
        _MaterialIssueDetailList = [];
        AddMaterialIssueDetailList();
        if (_MaterialIssueDetailList.length > 0)
        {
            debugger;
            var result = JSON.stringify(_MaterialIssueDetailList);
            $("#DetailJSON").val(result);
            $('#btnSave').trigger('click');
            _SlNo = 1;
        }
        else
        {
            notyAlert('warning', 'Please Add Material Details!');
        }
    }
    catch (e) {
        console.log(e.message);
    }
}
function AddMaterialIssueDetailList()
{
    try{
        debugger;
        var data = DataTables.MaterialIssueDetailTable.rows().data();
        for(var r=0;r<data.length;r++)
        {
            MaterialIssueDetail = new Object();
            MaterialIssueDetail.ID = data[r].ID;
            MaterialIssueDetail.MaterialIssueID = data[r].MaterialIssueID;
            MaterialIssueDetail.MaterialID = data[r].MaterialID;
            MaterialIssueDetail.MaterialDesc = data[r].MaterialDesc;
            MaterialIssueDetail.UnitCode = data[r].UnitCode;
            MaterialIssueDetail.Qty = data[r].Qty;
            _MaterialIssueDetailList.push(MaterialIssueDetail);
        }
    }
    catch (e) {
        console.log(e.message);
    }
}
function SaveSuccessIssueToProduction(data,status)
{
    try
    {
        debugger;
        var JsonResult = JSON.parse(data)
        switch(JsonResult.Result)
        {
            case "OK":
                $('#IsUpdate').val('True');
                $('#ID').val(JsonResult.Records.ID)
                BindIssueToProductionByID();
                _SlNo = 1;
                notyAlert("success", JsonResult.Records.Message)
                ChangeButtonPatchView('IssueToProduction', 'divbuttonPatchAddIssueToProduction', 'Edit');
                break;
            case "ERROR":
                notyAlert("danger", JsonResult.Message)
                break;
            default:
                notyAlert("danger", JsonResult.Message)
                break;
        }
    }
    catch (e) {
        console.log(e.message);
    }
}

function Reset()
{    
    BindIssueToProductionByID();
}

function  BindIssueToProductionByID()
{
    try{
        debugger;
        ChangeButtonPatchView('IssueToProduction', 'divbuttonPatchAddIssueToProduction', 'New');
        _SlNo = 1;
        var ID = $('#ID').val();
        var result = GetIssueToProductionByID(ID);
        $('#ID').val(result.ID);
        $('#IssueNo').val(result.IssueNo);
        $('#IssueDateFormatted').val(result.IssueDateFormatted);
        $('#IssuedBy').val(result.IssuedBy).select2();
        $('#IssueTo').val(result.IssueTo).select2();
        $('#GeneralNotes').val(result.GeneralNotes);
        $('#lblIssueNo').text('Issue To Production#:' + result.IssueNo);
        BindMaterialIssueDetailTable(ID);
    }
    catch (e) {
        console.log(e.message);
    }
}

function GetIssueToProductionByID(ID)
{
    try
    {
        debugger;
        var data = { "id": ID };
        var jsonData = {};
        var result = "";
        var message = "";
        var materialIssueVM = new Object();
        jsonData = GetDataFromServer("IssueToProduction/GetIssueToProduction/", data);
        if (jsonData != '') {
            jsonData = JSON.parse(jsonData);
            result = jsonData.Result;
            materialIssueVM = jsonData.Record;
            message = jsonData.Message;
        }
        if (result == "OK") {
            return materialIssueVM;
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

function BindMaterialIssueDetailTable(ID)
{
    debugger;
    DataTables.MaterialIssueDetailTable.clear().rows.add(GetIssueToProductionDetail(ID)).draw(true);
}

function GetIssueToProductionDetail(ID)
{
    try
    {
        debugger;
        var data={"id":ID};
        var jsonData = {};
        var result = "";
        var message = "";
        var materialIssueDetailVM = new Object();
        jsonData = GetDataFromServer("IssueToProduction/GetIssueToProductionDetail/", data);
        if (jsonData != '') {
            jsonData = JSON.parse(jsonData);
            result = jsonData.Result;
            message = jsonData.Message;
            materialIssueDetailVM = jsonData.Records;
        }
        if (result == "OK") {
            return materialIssueDetailVM;
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