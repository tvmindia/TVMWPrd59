var DataTables = {};
var EmptyGuid = "00000000-0000-0000-0000-000000000000";
var _StockAdjustmentDetail = [];
var _StockAdjustmentDetailList = [];
var _SlNo = 1;
$(document).ready(function () {
    debugger;
    try {
        $("#EmployeeID").select2({
        });

        $("#MaterialID").select2({ dropdownParent: $("#StockAdjustmentDetailsModal") });

        DataTables.StockAdjustmentDetailTable = $('#tblStockAdjustmentDetail').DataTable(
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
          { "data": "", render: function (data, type, row) { return _SlNo++}, "defaultContent": "<i></i>" },
          { "data": "Material.MaterialCode", render: function (data, type, row) { return data }, "defaultContent": "<i></i>"},
          { "data": "Material.Description", render: function (data, type, row) { return data }, "defaultContent": "<i></i>"},
          { "data": "Material.UnitCode", render: function (data, type, row) { return data }, "defaultContent": "<i></i>" },
          { "data": "Qty", render: function (data, type, row) { return data }, "defaultContent": "<i></i>" },
          { "data": "Remarks", render: function (data, type, row) { return data }, "defaultContent": "<i></i>" },
          { "data": null, "orderable": false, "defaultContent": '<a href="#" class="actionLink"  onclick="MaterialEdit(this)" ><i class="glyphicon glyphicon-edit" aria-hidden="true"></i></a>  |  <a href="#" class="DeleteLink"  onclick="Delete(this)" ><i class="glyphicon glyphicon-trash" aria-hidden="true"></i></a>' },
         ],
         columnDefs: [{ "targets": [0, 1], "visible": false, searchable: false },
             { className: "text-right", "targets": [6] },
             { className: "text-center", "targets": [8] },
         { "targets": [7], "width": "30%" },
         { "targets": [2], "width": "5%" },
         { "targets": [3,5,6], "width": "10%" },
         { "targets": [4], "width": "15%" },        
         { "targets": [8], "width": "7%" }]
     });
        $("#MaterialID").change(function () {
            debugger;
            if (this.value != "") {
                BindStockAdjustmentDetails(this.value);
            }
            else {
                $('#MaterialStockAdjDetail_Material_MaterialCode').val('');
                $('#MaterialStockAdjDetail_Material_UnitCode').val('');
                $('#MaterialStockAdjDetail_Material_Description').val('');
                $('#MaterialStockAdjDetail_Material_Qty').val('');
                $('#MaterialStockAdjDetail_Material_Remarks').val('');
        }
        });
        if ($('#IsUpdate').val() == 'True') {
            BindStockAdjustmentByID()
            //ChangeButtonPatchView('MaterialStockAdj', 'divbuttonPatchAddStockAdj', 'Edit');
        }
        else {
            $('#lblStockAdjNo').text('Stock Adj.No# : New');
        }
    }
    catch (e) {
        console.log(e.message);
    }
});

function ShowStockAdjustmentDetailsModal() {
    try{
        debugger;
        if ($("#LatestApprovalStatus").val() == 3 || $("#LatestApprovalStatus").val() == 0) {
            $("#MaterialID").val('').select2();
            $('#MaterialStockAdjDetail_Material_MaterialCode').val('');
            $('#MaterialStockAdjDetail_Material_Description').val('');
            $('#MaterialStockAdjDetail_Material_UnitCode').val('');
            $('#MaterialStockAdjDetail_Qty').val('');
            $('#MaterialStockAdjDetail_Remarks').val('');

            $('#StockAdjustmentDetailsModal').modal('show');
        }
    }
    catch (e) {
        console.log(e.message);
    }
}

function MaterialEdit(curObj) {
    try{
        debugger;
        $('#StockAdjustmentDetailsModal').modal('show');

        var materialStockAdjDetailVM = DataTables.StockAdjustmentDetailTable.row($(curObj).parents('tr')).data();
        _SlNo = 1;
        BindStockAdjustmentDetails(materialStockAdjDetailVM.MaterialID);

        $("#MaterialID").val(materialStockAdjDetailVM.MaterialID).trigger('change');
        $('#MaterialStockAdjDetail_Qty').val(materialStockAdjDetailVM.Qty);
        $('#MaterialStockAdjDetail_Remarks').val(materialStockAdjDetailVM.Remarks);
    }
    catch (e) {
        console.log(e.message);
    }
}

function BindStockAdjustmentDetails(id)
{
    try{
        debugger;
        var result = GetMaterial(id);
        _SlNo = 1;
        $('#MaterialStockAdjDetail_Material_MaterialCode').val(result.MaterialCode);
        $('#MaterialStockAdjDetail_Material_UnitCode').val(result.UnitCode);
        $('#MaterialStockAdjDetail_Material_Description').val(result.Description);
        $('#MaterialStockAdjDetail_Material_Qty').val(result.Qty);
        $('#MaterialStockAdjDetail_Material_Remarks').val(result.Remarks);
    }
    catch (e) {
        console.log(e.message);
    }
}

function GetMaterial(id) {
    try {
        debugger;
        var data = { "id": id };
        var jsonData = {};
        var result = "";
        var message = "";
        var materialViewModel = new Object();
        jsonData = GetDataFromServer("MaterialStockAdj/GetMaterial/", data);
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

function AddStockAdjustmentDetails()
{
    try{
        debugger;
        if ($('#MaterialID').val() != "" && $('#MaterialStockAdjDetail_Qty').val() != "" && $('#MaterialStockAdjDetail_Remarks').val()!="")
        {
            _StockAdjustmentDetail = [];
            AddStockAdjustment = new Object();
            AddStockAdjustment.MaterialID = $('#MaterialID').val();
            AddStockAdjustment.Material = new Object();
            AddStockAdjustment.Material.MaterialCode = $('#MaterialStockAdjDetail_Material_MaterialCode').val();
            AddStockAdjustment.Material.Description = $('#MaterialStockAdjDetail_Material_Description').val();
            AddStockAdjustment.Material.UnitCode = $('#MaterialStockAdjDetail_Material_UnitCode').val();
            AddStockAdjustment.Qty = $('#MaterialStockAdjDetail_Qty').val();
            AddStockAdjustment.Remarks = $('#MaterialStockAdjDetail_Remarks').val();
            _StockAdjustmentDetail.push(AddStockAdjustment);

            if (_StockAdjustmentDetail != null)
            {
                var materialStockAdjDetailList = DataTables.StockAdjustmentDetailTable.rows().data();
                if (materialStockAdjDetailList.length > 0)
                {
                    var checkPoint = 0;
                    for (var i = 0; i < materialStockAdjDetailList.length; i++)
                    {
                        if (materialStockAdjDetailList[i].MaterialID == $('#MaterialID').val())
                        {
                            materialStockAdjDetailList[i].Material.MaterialCode = $('#MaterialStockAdjDetail_Material_MaterialCode').val();
                            materialStockAdjDetailList[i].Material.Description = $('#MaterialStockAdjDetail_Material_Description').val();
                            materialStockAdjDetailList[i].Material.UnitCode = $('#MaterialStockAdjDetail_Material_UnitCode').val();
                            materialStockAdjDetailList[i].Qty = $('#MaterialStockAdjDetail_Qty').val();
                            materialStockAdjDetailList[i].Remarks = $('#MaterialStockAdjDetail_Remarks').val();
                            checkPoint = 1;
                            break;
                        }
                    }
                    if (!checkPoint)
                    {
                        _SlNo = DataTables.StockAdjustmentDetailTable.rows().count() + 1;
                        DataTables.StockAdjustmentDetailTable.rows.add(_StockAdjustmentDetail).draw(false);
                    }
                    else
                    {
                        DataTables.StockAdjustmentDetailTable.clear().rows.add(materialStockAdjDetailList).draw(false);
                    }
                }
                else
                {
                    DataTables.StockAdjustmentDetailTable.rows.add(_StockAdjustmentDetail).draw(false);
                }
            }
            $('#StockAdjustmentDetailsModal').modal('hide');
        }
        else
        {
            notyAlert('warning', "Mandatory fields are missing ");
        }
    }
    catch (e) {
        console.log(e.message);
    }
}

function Save() {
    try{
        debugger;
        $("#DetailJSON").val('');
        _StockAdjustmentDetailList = [];
        AddStockAdjustmentDetailList();
        if (_StockAdjustmentDetailList.length > 0) {
            var result = JSON.stringify(_StockAdjustmentDetailList);
            $("#DetailJSON").val(result);
            $('#btnSave').trigger('click');
            _SlNo = 1;
        }
        else {
            notyAlert('warning', 'Please add item details!');
        }
    }
    catch (e) {
        console.log(e.message);
    }

}

function AddStockAdjustmentDetailList()
{
    try{
        debugger;
        var stockAdjDetailData = DataTables.StockAdjustmentDetailTable.rows().data();
        for (var r = 0; r < stockAdjDetailData.length; r++)
        {
            StockAdjustmentDetail = new Object();
            //Material = new Object();
            //Material.Description = stockAdjDetailData[r].Description;
            StockAdjustmentDetail.ID = stockAdjDetailData[r].ID;
            StockAdjustmentDetail.AdjustmentID = stockAdjDetailData[r].AdjustmentID;
            StockAdjustmentDetail.MaterialID = stockAdjDetailData[r].MaterialID;
            //StockAdjustmentDetail.Material.Description = Material;
            StockAdjustmentDetail.Qty = stockAdjDetailData[r].Qty;
            StockAdjustmentDetail.Remarks = stockAdjDetailData[r].Remarks;
            _StockAdjustmentDetailList.push(StockAdjustmentDetail);
        }
    }
    catch (e) {
        console.log(e.message);
    }
}

function SaveSuccessStockAdjustment(data,status)
{
    try{
        debugger;
        var JsonResult = JSON.parse(data)
        switch(JsonResult.Result)
        {
            case "OK":
                $('#IsUpdate').val('True');
                $('#ID').val(JsonResult.Records.ID)
                BindStockAdjustmentByID();
                _SlNo = 1;
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
    catch (e) {
        console.log(e.message);
    }
}

function Reset()
{
    BindStockAdjustmentByID();
}

function BindStockAdjustmentByID()
{
    try{
        _SlNo = 1;
        var id = $('#ID').val();
        var result = GetStockAdjustmentByID(id);
        debugger;
        $('#ID').val(result.ID);
        $('#AdjustmentNo').val(result.AdjustmentNo);
        $('#EmployeeID').val(result.EmployeeID).select2();
        //$('#AdjustmentDateFormatted').val(result.AdjustmentDateFormatted);
        $('#AdjustmentDateFormatted').datepicker('setDate', result.AdjustmentDateFormatted);
        $('#Remarks').val(result.Remarks);
        $('#lblStockAdjNo').text('Stock Adj.No# :' + result.AdjustmentNo);
        $('#LatestApprovalStatus').val(result.LatestApprovalStatus);
        $('#LatestApprovalID').val(result.LatestApprovalID);
        $('#lblApprovalStatus').text(result.ApprovalStatus);

        if (result.LatestApprovalStatus == 3 || result.LatestApprovalStatus == 0)
        {
            ChangeButtonPatchView('MaterialStockAdj', 'divbuttonPatchAddStockAdj', 'Edit');
            EnableDisableFields(false)       
        }
        else
        {
            ChangeButtonPatchView('MaterialStockAdj', 'divbuttonPatchAddStockAdj', 'Disable');
            EnableDisableFields(true)
        }
        $('#divApprovalHistory').load("../DocumentApproval/AboutApprovalHistory?id=" + $('#ID').val() + "&docType=SSADJ");
        BindStockAdjustmentDetailTable(id);
    }
    catch (e) {
        console.log(e.message);
    }
}

function EnableDisableFields(value)
{
    try{
        $('#btnAddStockAdjustmentItems').attr('disabled', value);
        $('#EmployeeID').attr('disabled', value);
        $('#AdjustmentDateFormatted').attr('disabled', value);
        $('#Remarks').attr('disabled', value);
        DataTables.StockAdjustmentDetailTable.column(8).visible(!value);
    }
    catch (e) {
        console.log(e.message);
    }
}

function GetStockAdjustmentByID(id)
{
    try
    {
        debugger;
        var data = { "id": id };
        var jsonData = {};
        var result = "";
        var message = "";
        var materialStockAdjVM = new Object();
        jsonData = GetDataFromServer("MaterialStockAdj/GetMaterialStockAdjustment/", data);
        if (jsonData != '') {
            jsonData = JSON.parse(jsonData);
            result = jsonData.Result;
            materialStockAdjVM = jsonData.Record;
            message = jsonData.Message;
        }
        if (result == "OK") {
            return materialStockAdjVM;
        }
        if (result == "ERROR") {
            alert(message);
        }
    }
    catch(e)
    {
        notyAlert('error', e.message);
    }
}

function BindStockAdjustmentDetailTable(id)
{
    try{
        debugger;
        DataTables.StockAdjustmentDetailTable.clear().rows.add(GetMaterialStockAdjustmentDetail(id)).draw(false);
    }
    catch (e) {
        console.log(e.message);
    }
}

function GetMaterialStockAdjustmentDetail(id)
{
    try {
        debugger;
        var data = { "id": id };
        var jsonData = {};
        var result = "";
        var message = "";
        var materialStockAdjDetailVM = new Object();
        jsonData = GetDataFromServer("MaterialStockAdj/GetMaterialStockAdjustmentDetail/", data);
        if (jsonData != '') {
            jsonData = JSON.parse(jsonData);
            result = jsonData.Result;
            message = jsonData.Message;
            materialStockAdjDetailVM = jsonData.Records;
        }
        if (result == "OK") {
            return materialStockAdjDetailVM;
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
    try{
        debugger;
        var materialStockAdjDetailVM = DataTables.StockAdjustmentDetailTable.row($(curObj).parents('tr')).data();
        var materialStockAdjDetailVMIndex = DataTables.StockAdjustmentDetailTable.row($(curObj).parents('tr')).index();

        if ((materialStockAdjDetailVM != null) && (materialStockAdjDetailVM.ID != null)) {
            notyConfirm('Are you sure to delete?', 'DeleteItem("' + materialStockAdjDetailVM.ID + '")');

        }
        else {
            var res = notyConfirm('Are you sure to delete?', 'DeleteTempItem("' + materialStockAdjDetailVMIndex + '")');
        }
    }
    catch (e) {
        console.log(e.message);
    }
}

function DeleteTempItem(materialStockAdjDetailVMIndex) {
    try{
        debugger;
        var Itemtabledata = DataTables.StockAdjustmentDetailTable.rows().data();
        Itemtabledata.splice(materialStockAdjDetailVMIndex, 1);
        _SlNo = 1;
        DataTables.StockAdjustmentDetailTable.clear().rows.add(Itemtabledata).draw(false);
        notyAlert('success', 'Deleted successfully');
    }
    catch (e) {
        console.log(e.message);
    }
}

function DeleteItem(id)
{
    try {
        debugger;
        var data = { "id": id };
        var jsonData = {};
        var result = "";
        var message = "";
        var materialStockAdjDetailVM = new Object();
        jsonData = GetDataFromServer("MaterialStockAdj/DeleteMaterialStockAdjustmentDetail/", data);
        if (jsonData != '') {
            jsonData = JSON.parse(jsonData);
            result = jsonData.Result;
            materialStockAdjDetailVM = jsonData.Records;
            message = jsonData.Message;
        }
        switch (result) {
            case "OK":
                notyAlert('success', message);
                BindStockAdjustmentByID();

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
    debugger;
    notyConfirm('Are you sure to delete?', 'DeleteStockAdjustment()');
}

function DeleteStockAdjustment()
{
    try {
        debugger;
        var id = $('#ID').val();
        if (id != '' && id != null) {
            var data = { "id": id };
            var jsonData = {};
            var result = "";
            var message = "";
            var materialStockAdjVM = new Object();
            jsonData = GetDataFromServer("MaterialStockAdj/DeleteMaterialStockAdjustment/", data);
            if (jsonData != '') {
                jsonData = JSON.parse(jsonData);
                result = jsonData.Result;
                materialStockAdjVM = jsonData.Record;
                message = jsonData.Message;
            }
            if (result == "OK") {
                notyAlert('success', materialStockAdjVM.message);
                window.location.replace("NewStockAdjustment?code=STR");
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

function ShowSendForApproval(documentTypeCode) {
    try{
        debugger;
        if ($('#LatestApprovalStatus').val() == 3) {
            var documentID = $('#ID').val();
            var latestApprovalID = $('#LatestApprovalID').val();
            ReSendDocForApproval(documentID, documentTypeCode, latestApprovalID);
            BindStockAdjustmentByID();
        }
        else {
            $('#SendApprovalModal').modal('show');
        }
    }
    catch (e) {
        console.log(e.message);
    }
}

function SendForApproval(documentTypeCode) {
    try{
        debugger;
        var documentID = $('#ID').val();
        var approversCSV;
        var count = $('#ApproversCount').val();

        for (i = 0; i < count; i++) {
            if (i == 0)
                approversCSV = $('#ApproverLevel' + i).val();
            else
                approversCSV = approversCSV + ',' + $('#ApproverLevel' + i).val();
        }
        SendDocForApproval(documentID, documentTypeCode, approversCSV);
        $('#SendApprovalModal').modal('hide');
        BindStockAdjustmentByID();
    } catch (e) {
        console.log(e.message);
    }
}