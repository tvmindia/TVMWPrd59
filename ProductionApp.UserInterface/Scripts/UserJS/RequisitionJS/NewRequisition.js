//*****************************************************************************
//*****************************************************************************
//Author: Gibin Jacob
//CreatedDate: 12-Feb-2018 
//LastModified: 05-Mar-2018 
//FileName: NewRequisition.js
//Description: Client side coding for New/Edit Requisition
//******************************************************************************
//******************************************************************************

//--Global Declaration--//
var DataTables = {};
var EmptyGuid = "00000000-0000-0000-0000-000000000000";
var _RequistionDetail = [];
var _RequistionDetailList = [];

$(document).ready(function () {
    debugger;
    try {
        $("#MaterialID").select2({
            dropdownParent: $("#RequisitionDetailsModal")
        });
        $("#EmployeeID").select2({
        });

        $('#btnUpload').click(function () {
            debugger;
            //Pass the controller name
            var FileObject = new Object;
            if ($('#hdnFileDupID').val() != EmptyGuid) {
                FileObject.ParentID = (($('#ID').val()) != EmptyGuid ? ($('#ID').val()) : $('#hdnFileDupID').val());
            }
            else {
                FileObject.ParentID = ($('#ID').val() == EmptyGuid) ? "" : $('#ID').val();
            }
            FileObject.ParentType = "Requisition";
            FileObject.Controller = "FileUpload";
            UploadFile(FileObject);
        });

        DataTables.RequisitionDetailTable = $('#tblRequisitionDetail').DataTable(
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
          { "data": "Material.MaterialCode", render: function (data, type, row) { return data }, "defaultContent": "<i></i>", "width": "10%" },
          { "data": "Description", render: function (data, type, row) { return data }, "defaultContent": "<i></i>", "width": "43%" },
          { "data": "Material.CurrentStock", render: function (data, type, row) { return data }, "defaultContent": "<i></i>" },
          { "data": "RequestedQty", render: function (data, type, row) { return data }, "defaultContent": "<i></i>" },
          { "data": "ApproximateRate", render: function (data, type, row) { return roundoff(data) }, "defaultContent": "<i></i>" },
          { "data": null, "orderable": false, "defaultContent": '<a href="#" class="actionLink"  onclick="MaterialEdit(this)" ><i class="glyphicon glyphicon-pencil" aria-hidden="true"></i></a>  |  <a href="#" class="DeleteLink"  onclick="Delete(this)" ><i class="glyphicon glyphicon-trash" aria-hidden="true"></i></a>' },
          ],
          columnDefs: [{ "targets": [0,1], "visible": false, searchable: false },
              { className: "text-center", "targets": [7],"width": "7%" },
              { className: "text-right", "targets": [4,5,6] },
              { className: "text-left", "targets": [2,3] }
          ]
      });

        $("#MaterialID").change(function () {
           BindMaterialDetails(this.value)
        });
        debugger;
        if( $('#IsUpdate').val()=='True')
        {
            BindRequisitionByID()
            ChangeButtonPatchView('Requisition', 'divbuttonPatchAddRequisition', 'Edit');
        }
        else
        {
            $('#lblReqNo').text('Requisition# : New');
        }

    }
    catch (e) {
        console.log(e.message);
    }
});

function ShowRequisitionDetailsModal()
{
    debugger;
    $("#MaterialID").val('').select2();
    $('#RequisitionDetail_Material_MaterialCode').val('');
    $('#RequisitionDetail_Material_CurrentStock').val('');
    $('#RequisitionDetail_Description').val('');
    $('#RequisitionDetail_ApproximateRate').val('');
    $('#RequisitionDetail_RequestedQty').val('');

    $('#RequisitionDetailsModal').modal('show');
}

function MaterialEdit(curObj)
{
    debugger;
    $('#RequisitionDetailsModal').modal('show');

    var rowData = DataTables.RequisitionDetailTable.row($(curObj).parents('tr')).data();
    BindMaterialDetails(rowData.MaterialID);
    $("#MaterialID").val(rowData.MaterialID).trigger('change');
    $('#RequisitionDetail_RequestedQty').val(rowData.RequestedQty);
    $('#RequisitionDetail_Description').val(rowData.Description);

}

function BindMaterialDetails(ID)
{
    debugger;
    var result = GetMaterial(ID);
    $('#RequisitionDetail_Material_MaterialCode').val(result.MaterialCode);
    $('#RequisitionDetail_Material_CurrentStock').val(result.CurrentStock);
    $('#RequisitionDetail_Description').val(result.Description);
    $('#RequisitionDetail_ApproximateRate').val(result.Rate);
}

function GetMaterial(ID) {
    try {
        debugger;
        var data = { "ID": ID };
        var ds = {};
        ds = GetDataFromServer("Requisition/GetMaterial/", data);
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

function AddRequisitionDetails()
{
    debugger;
    if ($("#MaterialID").val() != "" && $('#RequisitionDetail_RequestedQty').val()!="")
    {
        _RequistionDetail = [];
        RequisitionMaterial = new Object();
        RequisitionMaterial.MaterialID = $("#MaterialID").val();
        RequisitionMaterial.Material = new Object();
        RequisitionMaterial.Material.MaterialCode = $('#RequisitionDetail_Material_MaterialCode').val();
        RequisitionMaterial.Description = $('#RequisitionDetail_Description').val();
        RequisitionMaterial.RequestedQty = $('#RequisitionDetail_RequestedQty').val();
        RequisitionMaterial.Material.CurrentStock = $('#RequisitionDetail_Material_CurrentStock').val();
        RequisitionMaterial.ApproximateRate = $('#RequisitionDetail_ApproximateRate').val();
        _RequistionDetail.push(RequisitionMaterial);

        if (_RequistionDetail != null)
        {
            //check product existing or not if soo update the new
            var allData = DataTables.RequisitionDetailTable.rows().data();
            if (allData.length > 0) {
                var checkPoint = 0;
                for (var i = 0; i < allData.length; i++) {
                    if (allData[i].MaterialID == $("#MaterialID").val()) {
                        allData[i].Description = $('#RequisitionDetail_Description').val();
                        allData[i].CurrentStock = $('#RequisitionDetail_Material_CurrentStock').val();
                        allData[i].RequestedQty = $('#RequisitionDetail_RequestedQty').val();
                        allData[i].ApproximateRate = $('#RequisitionDetail_ApproximateRate').val();
                        checkPoint = 1;
                        break;
                    }
                }
                if (!checkPoint) {
                    DataTables.RequisitionDetailTable.rows.add(_RequistionDetail).draw(false);
                }
                else {
                    DataTables.RequisitionDetailTable.clear().rows.add(allData).draw(false);
                }
            }
            else {
                DataTables.RequisitionDetailTable.rows.add(_RequistionDetail).draw(false);
            }
        }      
        $('#RequisitionDetailsModal').modal('hide');
    }
    else
    {
        notyAlert('warning', "Material and Quantity fields are required ");
    }
}

function Save()
{
    debugger;
    $("#DetailJSON").val('');
    _RequistionDetailList = [];
    AddRequistionDetailList();
    if (_RequistionDetailList.length > 0) {
        var result = JSON.stringify(_RequistionDetailList);
        $("#DetailJSON").val(result);
        $('#btnSave').trigger('click');
    }
    else {
        notyAlert('warning', 'Please Add Requistion Details!');
    }

}
function AddRequistionDetailList() {
    debugger;
    var data = DataTables.RequisitionDetailTable.rows().data();
    for (var r = 0; r < data.length; r++) {
        RequisitionDetail = new Object();
        RequisitionDetail.ID = data[r].ID;
        RequisitionDetail.ReqID = data[r].ReqID;
        RequisitionDetail.MaterialID = data[r].MaterialID;
        RequisitionDetail.Description = data[r].Description;
        RequisitionDetail.RequestedQty = data[r].RequestedQty;
        RequisitionDetail.ApproximateRate = data[r].ApproximateRate;
        _RequistionDetailList.push(RequisitionDetail);
    }
}

function SaveSuccessRequisition(data, status)
{
    debugger;
    var JsonResult = JSON.parse(data)
    switch (JsonResult.Result) {
        case "OK":
            $('#IsUpdate').val('True');
            $('#ID').val(JsonResult.Records.ID)
            BindRequisitionByID();
            notyAlert("success", JsonResult.Records.Message)
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
    BindRequisitionByID();
}
function BindRequisitionByID()
{
    var ID = $('#ID').val();
    var result = GetRequisitionByID(ID);
    debugger;
    $('#ID').val(result.ID);
    $('#Title').val(result.Title);
    $('#ReqNo').val(result.ReqNo);
    $('#ReqDateFormatted').val(result.ReqDateFormatted);
    $('#EmployeeID').val(result.EmployeeID).select2();
    $('#ReqStatus').val(result.ReqStatus);
    $('#lblReqNo').text('Requisition# : ' + result.ReqNo);
    $('#lblReqStatus').text(result.ReqStatus);
    $('#lblApprovalStatus').text(result.ApprovalStatus);
    
    //detail Table values binding with header id
    BindRequisitionDetailTable(ID);
    PaintImages(ID);//bind attachments
}

function GetRequisitionByID(ID)
{
    try {
        debugger;
        var data = { "ID": ID };
        var ds = {};
        ds = GetDataFromServer("Requisition/GetRequisition/", data);
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

function  BindRequisitionDetailTable(ID)
{
    DataTables.RequisitionDetailTable.clear().rows.add(GetRequisitionDetail(ID)).draw(false);
}

function GetRequisitionDetail(ID) {
    try {
        debugger;
        var data = { "ID": ID };
        var ds = {};
        ds = GetDataFromServer("Requisition/GetRequisitionDetail/", data);
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

function Delete(curobj)
{
    debugger;
    var rowData = DataTables.RequisitionDetailTable.row($(curobj).parents('tr')).data();
    var Rowindex = DataTables.RequisitionDetailTable.row($(curobj).parents('tr')).index();

    if ((rowData != null) && (rowData.ID != null)) {
        notyConfirm('Are you sure to delete?', 'DeleteItem("' + rowData.ID + '")');
    }
    else {
        var res = notyConfirm('Are you sure to delete?', 'DeleteTempItem("' + Rowindex + '")');

    }
}

function DeleteTempItem(Rowindex)
{
    debugger;
    //var Itemtabledata = DataTables.RequisitionDetailList.rows().data();
    //Itemtabledata.splice(Rowindex, 1);
    //DataTables.RequisitionDetailList.clear().rows.add(Itemtabledata).draw(false);
    DataTables.RequisitionDetailTable.row(Rowindex).remove().draw(false);
    notyAlert('success', 'Deleted Successfully');
}

function DeleteItem(ID) {

    try {
        debugger;
        var data = { "ID": ID };
        var ds = {};
        ds = GetDataFromServer("Requisition/DeleteRequisitionDetail/", data);
        if (ds != '') {
            ds = JSON.parse(ds);
        }
        switch (ds.Result) {
            case "OK":
                notyAlert('success', ds.Message);
                BindRequisitionByID();
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

function DeleteClick() {
    debugger;
    notyConfirm('Are you sure to delete?', 'DeleteRequisition()');
}


function DeleteRequisition() {
    try {
        debugger;
        var id = $('#ID').val();
        if (id != '' && id != null) {
            var data = { "ID": id };
            var ds = {};
            ds = GetDataFromServer("Requisition/DeleteRequisition/", data);
            if (ds != '') {
                ds = JSON.parse(ds);
            }
            if (ds.Result == "OK") {
                notyAlert('success', ds.Record.Message);
                window.location.replace("NewRequisition?code=PURCH");
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

function ShowSendForApproval() {
    debugger;

    $('#SendApprovalModal').modal('show');
}

function SendForApproval(documentTypeCode) {
    debugger;
    var documentID = $('#ID').val();
    var approversCSV;
    var count = $('#ApproversCount').val();

    for (i = 0; i < count; i++) {
        if (i == 0)
            approversCSV = $('#ApproverLevel' + (i + 1)).val();
        else
            approversCSV = approversCSV + ',' + $('#ApproverLevel'+i).val();
    }
    SendDocForApproval(documentID,documentTypeCode, approversCSV);
    $('#SendApprovalModal').modal('hide');
}

