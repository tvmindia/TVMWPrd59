var DataTables = {};
var EmptyGuid = "00000000-0000-0000-0000-000000000000";
var _FinishedGoodStockAdjDetail = [];
var _FinishedGoodStockAdjDetailList = [];
var _SlNo = 1;
$(document).ready(function (){
debugger;
    try
    {
        $('#EmployeeID').select2({
        });

        $('#ProuctID').select2({ dropdownParent: $('#finishedGoodStockAdjDetailsModal') });
    DataTables.FinishedGoodStockAdjTable=$('#tblFinishedGoodStockAdjDetail').DataTable({
        dom: '<"pull-right"f>rt<"bottom"ip><"clear">',
        ordering: false,
        searching: false,
        paging: false,
        data: null,
        autoWidth: false,
        "bInfo": false,
        columns: [
            { "data": "ID", "defaultContent": "<i></i>" },
         { "data": "ProductID", "defaultContent": "<i></i>" },
         { "data": "", render: function (data, type, row) { return _SlNo++}, "defaultContent": "<i></i>" },
         { "data": "Product.Code", render: function (data, type, row) { return data }, "defaultContent": "<i></i>"},
         { "data": "Product.Description", render: function (data, type, row) { return data }, "defaultContent": "<i></i>"},
         { "data": "Product.UnitCode", render: function (data, type, row) { return data }, "defaultContent": "<i></i>" },
         { "data": "Qty", render: function (data, type, row) { return data }, "defaultContent": "<i></i>" },
         { "data": "Remarks", render: function (data, type, row) { return data }, "defaultContent": "<i></i>" },
         { "data": null, "orderable": false, "defaultContent": '<a href="#" class="actionLink"  onclick="ProductEdit(this)" ><i class="glyphicon glyphicon-pencil" aria-hidden="true"></i></a>  |  <a href="#" class="DeleteLink"  onclick="Delete(this)" ><i class="glyphicon glyphicon-trash" aria-hidden="true"></i></a>' },
        ],
        columnDefs: [{ "targets": [0, 1], "visible": false, searchable: false },
        { "targets": [7], "width": "30%" },
        { "targets": [2], "width": "5%" },
        { "targets": [3,5,6], "width": "10%" },
        { "targets": [4], "width": "15%" },        
        { "targets": [8], "width": "4%" }]
    });
    $("#ProductID").change(function () {
        debugger;
           
        BindFinishedGoodStockAdjDetails(this.value)
    });

    if ($('#IsUpdate').val() == 'True') {
        BindFinishedGoodStockAdjByID()
        
    }
    else {
        $('#lblFGStockAdjNo').text('Finished Good Stock Adj.No# : New');
    }
}
catch(e)
{
    console.log(e.message);
}
});

function ShowFinishedGoodStockAdjDetailsModal() {
    try{
        debugger;
        if ($("#LatestApprovalStatus").val() == 3 || $("#LatestApprovalStatus").val() == 0)
        {
            $('#ProductID').val('').select2();

            $('#FinishedGoodStockAdjDetail_Product_Code').val('');
            $('#FinishedGoodStockAdjDetail_Product_Description').val('');
            $('#FinishedGoodStockAdjDetail_Product_UnitCode').val('');
            $('#FinishedGoodStockAdjDetail_Qty').val('');
            $('#FinishedGoodStockAdjDetail_Remarks').val('');

            $('#finishedGoodStockAdjDetailsModal').modal('show');
        }
    }
    catch (e) {
        console.log(e.message);
    }
}

function ProductEdit(curObj)
{
    try{
        debugger;
        $('#finishedGoodStockAdjDetailsModal').modal('show');

        var finishedGoodStockAdjDetailVM = DataTables.FinishedGoodStockAdjTable.row($(curObj).parents('tr')).data();
        _SlNo = 1;
        BindFinishedGoodStockAdjDetails(finishedGoodStockAdjDetailVM.ProductID);
    
        $('#ProductID').val(finishedGoodStockAdjDetailVM.ProductID).trigger('change');
        $('#FinishedGoodStockAdjDetail_Qty').val(finishedGoodStockAdjDetailVM.Qty);
        $('#FinishedGoodStockAdjDetail_Remarks').val(finishedGoodStockAdjDetailVM.Remarks);
    }
    catch (e) {
        console.log(e.message);
    }
}

function BindFinishedGoodStockAdjDetails(id)
{
    try{
        debugger;
        var result = GetProduct(id); _SlNo = 1; 
        $('#FinishedGoodStockAdjDetail_Product_Code').val(result.Code);
        $('#FinishedGoodStockAdjDetail_Product_UnitCode').val(result.UnitCode);
        $('#FinishedGoodStockAdjDetail_Product_Description').val(result.Description);
    }
    catch (e) {
        console.log(e.message);
    }
}

function GetProduct(id) {
    try {
        debugger;
        var data = { "id": id };
        var jsonData = {};
        var result = "";
        var message = "";
        var productViewModel = new Object();
        jsonData = GetDataFromServer("FinishedGoodStockAdj/GetProduct/", data);
        if (jsonData != '') {
            jsonData = JSON.parse(jsonData);
            result = jsonData.Result;
            productViewModel = jsonData.Records;
            message = jsonData.Message;
        }
        if (result == "OK") {
            return productViewModel;
        }
        if (result == "ERROR") {
            alert(message);
        }
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}

function AddFinishedGoodStockAdjDetails()
{
    try{
        debugger;
        if ($('#ProductID').val() != "" && $('#FinishedGoodStockAdjDetail_Qty').val()!= "" && $('#FinishedGoodStockAdjDetail_Remarks').val()!="")
        {
            _FinishedGoodStockAdjDetail = [];
            AddFinishedGoodStockAdj = new Object();
            AddFinishedGoodStockAdj.ProductID = $('#ProductID').val();
            AddFinishedGoodStockAdj.Product = new Object();
            AddFinishedGoodStockAdj.Product.Code = $('#FinishedGoodStockAdjDetail_Product_Code').val();
            AddFinishedGoodStockAdj.Product.UnitCode = $('#FinishedGoodStockAdjDetail_Product_UnitCode').val();
            AddFinishedGoodStockAdj.Product.Description = $('#FinishedGoodStockAdjDetail_Product_Description').val();
            AddFinishedGoodStockAdj.Qty = $('#FinishedGoodStockAdjDetail_Qty').val();
            AddFinishedGoodStockAdj.Remarks = $('#FinishedGoodStockAdjDetail_Remarks').val();
            _FinishedGoodStockAdjDetail.push(AddFinishedGoodStockAdj);

            if(_FinishedGoodStockAdjDetail!=null)
            {
                var finishedGoodStockAdjDetailList = DataTables.FinishedGoodStockAdjTable.rows().data();
                if(finishedGoodStockAdjDetailList.length>0)
                {
                    var checkPoint = 0;
                    for(var i=0;i<finishedGoodStockAdjDetailList.length;i++)
                    {
                        if(finishedGoodStockAdjDetailList[i].ProductID==$('#ProductID').val())
                        {
                            finishedGoodStockAdjDetailList[i].Product.Code = $('#FinishedGoodStockAdjDetail_Product_Code').val();
                            finishedGoodStockAdjDetailList[i].Product.UnitCode = $('#FinishedGoodStockAdjDetail_Product_UnitCode').val();
                            finishedGoodStockAdjDetailList[i].Product.Description = $('#FinishedGoodStockAdjDetail_Product_Description').val();
                            finishedGoodStockAdjDetailList[i].Qty = $('#FinishedGoodStockAdjDetail_Qty').val();
                            finishedGoodStockAdjDetailList[i].Remarks = $('#FinishedGoodStockAdjDetail_Remarks').val();
                            checkPoint = 1;
                            break;
                        }
                    }
                    if(!checkPoint)
                    {
                        DataTables.FinishedGoodStockAdjTable.rows.add(_FinishedGoodStockAdjDetail).draw(false);
                    }
                    else
                    {
                        DataTables.FinishedGoodStockAdjTable.clear().rows.add(finishedGoodStockAdjDetailList).draw(false);
                    }
                }
                else
                {
                    DataTables.FinishedGoodStockAdjTable.rows.add(_FinishedGoodStockAdjDetail).draw(false);
                }
            }
            $('#finishedGoodStockAdjDetailsModal').modal('hide');
        }
        else
        {
            notyAlert('warning', "Product,Quantity and Remark fields are required ");
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
        _FinishedGoodStockAdjDetailList = [];
        AddFinishedGoodStockAdjDetailList();
        if (_FinishedGoodStockAdjDetailList.length > 0) {
            var result = JSON.stringify(_FinishedGoodStockAdjDetailList);
            $("#DetailJSON").val(result);
            $('#btnSave').trigger('click');
            _SlNo = 1;
        }
        else {
            notyAlert('warning', 'Please Add Finished Good Stock Adjustment Details!');
        }
    }
    catch (e) {
        console.log(e.message);
    }
}

function AddFinishedGoodStockAdjDetailList()
{
    try{
        debugger;
        var finishedGoodStockAdjDetailData = DataTables.FinishedGoodStockAdjTable.rows().data();
        for(var r=0;r<finishedGoodStockAdjDetailData.length;r++)
        {
            finishedGoodStockAdjDetail = new Object();

            finishedGoodStockAdjDetail.ID = finishedGoodStockAdjDetailData[r].ID;
            finishedGoodStockAdjDetail.AdjustmentID = finishedGoodStockAdjDetailData[r].AdjustmentID;
            finishedGoodStockAdjDetail.ProductID = finishedGoodStockAdjDetailData[r].ProductID;       
            finishedGoodStockAdjDetail.Qty = finishedGoodStockAdjDetailData[r].Qty;
            finishedGoodStockAdjDetail.Remarks = finishedGoodStockAdjDetailData[r].Remarks;
            _FinishedGoodStockAdjDetailList.push(finishedGoodStockAdjDetail);
        }
    }
    catch (e) {
        console.log(e.message);
    }
}

function SaveSuccessFinishedGoodStockAdj(data,status)
{
    try{
        debugger;
        var JsonResult = JSON.parse(data)
        switch (JsonResult.Result) {
            case "OK":
                $('#IsUpdate').val('True');
                $('#ID').val(JsonResult.Records.ID)
                BindFinishedGoodStockAdjByID();
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
    BindFinishedGoodStockAdjByID();
}

function BindFinishedGoodStockAdjByID()
{
    try{
        _SlNo = 1;
        var id = $('#ID').val();
        var result = GetFinishedGoodStockAdjByID(id);
        debugger;
        $('#ID').val(result.ID);
        $('#AdjustmentNo').val(result.AdjustmentNo);
        $('#EmployeeID').val(result.EmployeeID).select2();
        $('#AdjustmentDateFormatted').val(result.AdjustmentDateFormatted);
        $('#Remarks').val(result.Remarks);
        $('#lblFGStockAdjNo').text('Finished Good Stock Adj.No# :' + result.AdjustmentNo);
        $('#LatestApprovalStatus').val(result.LatestApprovalStatus);
        $('#LatestApprovalID').val(result.LatestApprovalID);
        $('#lblApprovalStatus').text(result.ApprovalStatus);

        if (result.LatestApprovalStatus == 3 || result.LatestApprovalStatus == 0) {
            ChangeButtonPatchView('FinishedGoodStockAdj', 'divbuttonPatchAddFGStockAdj', 'Edit');
            EnableDisableFields(false)
        }
        else {
            ChangeButtonPatchView('FinishedGoodStockAdj', 'divbuttonPatchAddFGStockAdj', 'Disable');
            EnableDisableFields(true)
        }
        BindFinishedGoodStockAdjDetailTable(id);
    }
    catch (e) {
        console.log(e.message);
    }
}

function EnableDisableFields(value) {
    try{
        $('#btnAddFinishedGoodStockAdjItems').attr('disabled', value);
        $('#EmployeeID').attr('disabled', value);
        $('#AdjustmentDateFormatted').attr('disabled', value);
        $('#Remarks').attr('disabled', value);
        DataTables.FinishedGoodStockAdjTable.column(8).visible(!value);
    }
    catch (e) {
        console.log(e.message);
    }
}

function GetFinishedGoodStockAdjByID(id)
{
    try {
        debugger;
        var data = { "id": id };
        var jsonData = {};
        var result = "";
        var message = "";
        var finishedGoodStockAdjVM = new Object();
        jsonData = GetDataFromServer("FinishedGoodStockAdj/GetFinishedGoodStockAdj/", data);
        if (jsonData != '') {
            jsonData = JSON.parse(jsonData);
            result = jsonData.Result;
            finishedGoodStockAdjVM = jsonData.Record;
            message = jsonData.Message;
        }
        if (result == "OK") {
            return finishedGoodStockAdjVM;
        }
        if (result == "ERROR") {
            alert(message);
        }
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}

function BindFinishedGoodStockAdjDetailTable(id) {
    debugger;
    DataTables.FinishedGoodStockAdjTable.clear().rows.add(GetFinishedGoodStockAdjDetail(id)).draw(false);
}

function GetFinishedGoodStockAdjDetail(id)
{
    try {
        debugger;
        var data = { "id": id };
        var jsonData = {};
        var result = "";
        var message = "";
        var finishedGoodStockAdjDetailVM = new Object();
        jsonData = GetDataFromServer("FinishedGoodStockAdj/GetFinishedGoodStockAdjDetail/", data);
        if (jsonData != '') {
            jsonData = JSON.parse(jsonData);
            result = jsonData.Result;
            message = jsonData.Message;
            finishedGoodStockAdjDetailVM = jsonData.Records;
        }
        if (result == "OK") {
            return finishedGoodStockAdjDetailVM;
        }
        if (result == "ERROR") {
            alert(message);
        }
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}

function Delete(curObj)
{
    try{
        debugger;
        var finishedGoodStockAdjDetailVM = DataTables.FinishedGoodStockAdjTable.row($(curObj).parents('tr')).data();
        var finishedGoodStockAdjDetailVMIndex = DataTables.FinishedGoodStockAdjTable.row($(curObj).parents('tr')).index();
        if((finishedGoodStockAdjDetailVM!=null)&&(finishedGoodStockAdjDetailVM.ID!=null))
        {
            notyConfirm('Are you sure to delete?', 'DeleteItem("' + finishedGoodStockAdjDetailVM.ID + '")');
        }
        else
        {
            var tempDelete = notyConfirm('Are you sure to delete?', 'DeleteTempItem("' + finishedGoodStockAdjDetailVMIndex + '")');
        }
    }
    catch (e) {
        console.log(e.message);
    }
}

function DeleteTempItem(finishedGoodStockAdjDetailVMIndex) {
    try{
        debugger;
        var Itemtabledata = DataTables.FinishedGoodStockAdjTable.rows().data();
        Itemtabledata.splice(finishedGoodStockAdjDetailVMIndex, 1);
        _SlNo = 1;
        DataTables.FinishedGoodStockAdjTable.clear().rows.add(Itemtabledata).draw(false);
        notyAlert('success', 'Deleted Successfully');
    }
    catch (e) {
        console.log(e.message);
    }
}

function DeleteItem(id) {
    try {
        debugger;
        var data = { "id": id };
        var jsonData = {};
        var result = "";
        var message = "";
        var finishedGoodStockAdjDetailVM = new Object();
        jsonData = GetDataFromServer("FinishedGoodStockAdj/DeleteFinishedGoodStockAdjDetail/", data);
        if (jsonData != '') {
            jsonData = JSON.parse(jsonData);
            result = jsonData.Result;
            finishedGoodStockAdjDetailVM = jsonData.Records;
            message = jsonData.Message;
        }
        switch (result) {
            case "OK":
                notyAlert('success', message);
                BindFinishedGoodStockAdjByID();

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
    debugger;
    notyConfirm('Are you sure to delete?', 'DeleteFinishedGoodStockAdj()');
}

function DeleteFinishedGoodStockAdj() {
    try {
        debugger;
        var id = $('#ID').val();
        if (id != '' && id != null) {
            var data = { "id": id };
            var jsonData = {};
            var result = "";
            var message = "";
            var finishedGoodStockAdjVM = new Object();
            jsonData = GetDataFromServer("FinishedGoodStockAdj/DeleteFinishedGoodStockAdj/", data);
            if (jsonData != '') {
                jsonData = JSON.parse(jsonData);
                result = jsonData.Result;
                finishedGoodStockAdjVM = jsonData.Record;
                message = jsonData.Message;
            }
            if (result == "OK") {
                notyAlert('success', finishedGoodStockAdjVM.message);
                window.location.replace("NewFinishedGoodStockAdj?code=STR");
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
        BindFinishedGoodStockAdjByID();
    }
    catch (e) {
        console.log(e.message);
    }
}