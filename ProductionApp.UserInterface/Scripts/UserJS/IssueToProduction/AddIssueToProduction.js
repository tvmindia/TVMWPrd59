var DataTables = {};
var _MaterialIssueDetail = [];
var _MaterialIssueDetailList = [];
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
         { "data": "", render: function (data, type, row) { debugger;return row }, "defaultContent": "<i></i>"},
         { "data": "Material.MaterialCode", render: function (data, type, row) { return data }, "defaultContent": "<i></i>" },
         { "data": "MaterialDesc", render: function (data, type, row) { return data }, "defaultContent": "<i></i>" },
         { "data": "UnitCode", render: function (data, type, row) { return data }, "defaultContent": "<i></i>" },
         { "data": "Qty", render: function (data, type, row) { return data }, "defaultContent": "<i></i>" },        
         { "data": null, "orderable": false, "defaultContent": '<a href="#" class="DeleteLink"  onclick="Delete(this)" ><i class="glyphicon glyphicon-trash" aria-hidden="true"></i></a> | <a href="#" class="actionLink"  onclick="MaterialEdit(this)" ><i class="glyphicon glyphicon-pencil" aria-hidden="true"></i></a>' },
         ], 
         columnDefs: [{ "targets": [0,1], "visible": false, searchable: false },                              
             { className: "text-left", "targets": [2, 3, 4, 5, 6] },
             {"targets":[2],"width":"2%"},
             { "targets": [3], "width": "5%" },
             { "targets": [4], "width": "15%" },
             { "targets": [7], "width": "3%" },
             {"targets":[6],"width":"4%"},
             { "targets": [4,5], "width": "5%" }
         ]
     });

        $("#MaterialID").change(function () {
            debugger;
            BindRawMaterialDetails(this.value)
        });
        if ($('#IsUpdate').val() == 'True')
        {
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
    debugger;
    $('#MaterialID').val('').select2();
    $('#MaterialIssueDetail_Material_MaterialCode').val('');
    $('#MaterialIssueDetail_MaterialDesc').val('');
    $('#MaterialIssueDetail_UnitCode').val('');
    $('#MaterialIssueDetail_Qty').val('');
    $('#AddIssueToProductionItemModal').modal('show');
}

function BindRawMaterialDetails(ID)
{
    debugger;
    var result = GetMaterial(ID);
    $('#MaterialIssueDetail_Material_MaterialCode').val(result.MaterialCode);
    $('#MaterialIssueDetail_MaterialDesc').val(result.Description);
    $('#MaterialIssueDetail_UnitCode').val(result.UnitCode);
    $('#MaterialIssueDetail_Qty').val(result.Qty);
}

function GetMaterial(ID) {
    try {
        debugger;
        var data = { "ID": ID };
        var ds = {};
        ds = GetDataFromServer("IssueToProduction/GetMaterial/", data);
        if (ds != '') {
            ds = JSON.parse(ds);
        }
        if (ds.Result == "OK") {
            return ds.Records;
        }
        if (ds.Result == "ERROR") {
            alert(ds.Message);
        }
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}

function AddIssueToProductItem()
{
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
            var allData = DataTables.MaterialIssueDetailTable.rows().data();
            if(allData.length>0)
            {
                var checkPoint = 0;
                for(var i=0; i< allData.length;i++)
                {
                    if (allData[i].MaterialID == $('#MaterialID').val())
                    {                       
                        allData[i].MaterialDesc = $('#MaterialIssueDetail_MaterialDesc').val();
                        allData[i].UnitCode = $('#MaterialIssueDetail_UnitCode').val();
                        allData[i].Qty = $('#MaterialIssueDetail_Qty').val();
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
                    DataTables.MaterialIssueDetailTable.clear().rows.add(allData).draw(false);
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

function MaterialEdit(curObj)
{
    debugger;
    $('#AddIssueToProductionItemModal').modal('show');
    
    var rowData = DataTables.MaterialIssueDetailTable.row($(curObj).parents('tr')).data();
    //BindRawMaterialDetails(rowData.ID);
    if ((rowData != null) && (rowData.MaterialID != null))
    {
        $("#MaterialID").val(rowData.MaterialID).select2();
       $('#MaterialIssueDetail_Material_MaterialCode').val(rowData.Material.MaterialCode);
        $('#MaterialIssueDetail_MaterialDesc').val(rowData.MaterialDesc);
       $('#MaterialIssueDetail_UnitCode').val(rowData.UnitCode);
        $('#MaterialIssueDetail_Qty').val(rowData.Qty);
    }
}

function Delete(curObj)
{
    debugger;
    var rowData = DataTables.MaterialIssueDetailTable.row($(curObj).parents('tr')).data();
    var rowIndex = DataTables.MaterialIssueDetailTable.row($(curObj).parents('tr')).index();
    if ((rowData != null) && (rowData.ID != null))
    {
        notyConfirm('Are you sure to delete?', 'DeleteItem("' + rowData.ID + '")');
    }
    else
    {
        var res = notyConfirm('Are you sure to delete?', 'DeleteTempItem("' + rowIndex + '")');
    }
}

function DeleteTempItem(rowIndex)
{
    DataTables.MaterialIssueDetailTable.row(rowIndex).remove().draw(false);
    notyAlert('success', 'Deleted Successfully');
}

//-------------for delete from details table which saved in db-------
function DeleteItem(ID)
{
    try {
        debugger;
        var data = { "id": ID };
        var ds = {};
        ds = GetDataFromServer("IssueToProduction/DeleteIssueToProductionDetail/", data);
        if (ds != '') {
            ds = JSON.parse(ds);
        }
        switch (ds.Result) {
            case "OK":
                notyAlert('success', ds.Message);
                BindIssueToProductionByID();
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
            var ds = {};
            ds = GetDataFromServer("IssueToProduction/DeleteIssueToProduction/", data);
            if (ds != '') {
                ds = JSON.parse(ds);
            }
            if (ds.Result == "OK") {
                notyAlert('success', ds.Record.Message);
                window.location.replace("AddIssueToProduction?code=STR");
            }
            if (ds.Result == "ERROR") {
                notyAlert('error', ds.Message);
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
    }
    else
    {
        notyAlert('warning', 'Please Add Material Details!');
    }
}
function AddMaterialIssueDetailList()
{
    debugger;
    var data = DataTables.MaterialIssueDetailTable.rows().data();
    for(var r=0;r<data.length;r++)
    {
        MaterialIssueDetail = new Object();
        MaterialIssueDetail.ID = data[r].ID;
        MaterialIssueDetail.HeaerID = data[r].HeaerID;
        MaterialIssueDetail.MaterialID = data[r].MaterialID;
        MaterialIssueDetail.MaterialDesc = data[r].MaterialDesc;
        MaterialIssueDetail.UnitCode = data[r].UnitCode;
        MaterialIssueDetail.Qty = data[r].Qty;
        _MaterialIssueDetailList.push(MaterialIssueDetail);
    }
}
function SaveSuccessIssueToProduction(data,status)
{
    debugger;
    var JsonResult = JSON.parse(data)
    switch(JsonResult.Result)
    {
        case "OK":
            $('#IsUpdate').val('True');
            $('#ID').val(JsonResult.Records.ID)
            BindIssueToProductionByID();
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

function Reset()
{
    BindIssueToProductionByID();
}

function  BindIssueToProductionByID()
{
    debugger;
    ChangeButtonPatchView('IssueToProduction', 'divbuttonPatchAddIssueToProduction', 'New');

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

function GetIssueToProductionByID(ID)
{
    try
    {
        debugger;
        var data = { "id": ID };
        var ds = {};
        ds = GetDataFromServer("IssueToProduction/GetIssueToProduction/", data);
        if (ds != '') {
            ds = JSON.parse(ds);
        }
        if (ds.Result == "OK") {
            return ds.Records;
        }
        if (ds.Result == "ERROR") {
            alert(ds.Message);
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
    DataTables.MaterialIssueDetailTable.clear().rows.add(GetIssueToProductionDetail(ID)).draw(false);
}

function GetIssueToProductionDetail(ID)
{
    try
    {
        debugger;
        var data={"id":ID};
        var ds={};
        ds=GetDataFromServer("IssueToProduction/GetIssueToProductionDetail/",data);
        if (ds != '') {
            ds = JSON.parse(ds);
        }
        if (ds.Result == "OK") {
            return ds.Records;
        }
        if (ds.Result == "ERROR") {
            alert(ds.Message);
        }
    }
    catch (e)
    {
        notyAlert('error', e.message);
    }
}