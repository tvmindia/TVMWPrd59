
var DataTables = {};
var EmptyGuid = "00000000-0000-0000-0000-000000000000";
$(document).ready(function () {
    debugger;
    ValidateDocumentsApprovalPermission();
    try {
        DataTables.ApprovalHistoryTable = $('#tblApprovalHistory').DataTable(
{
    dom: '<"pull-right"f>rt<"bottom"ip><"clear">',
    ordering: false,
    searching: false,
    paging: false,
    bInfo : false,
    data: GetApprovalHistory(),//ApprovalHistory.js
    autoWidth: false,
    columns: [
    { "data": "ApproverName", "defaultContent": "<i>-</i>","width":"20%" },
    { "data": "ApproverLevel", "defaultContent": "<i>-</i>", "width": "5%" },
    { "data": "ApprovalDate", "defaultContent": "<i>-</i>", "width": "20%" },
    { "data": "Remarks", "defaultContent": "<i>-</i>", "width": "35%" },
    { "data": "ApprovalStatus", "defaultContent": "<i>-</i>", "width": "20%" },
    ],
    columnDefs: [
        { className: "text-center", "targets": [2] },
        { className: "text-right", "targets": [] },
        { className: "text-left", "targets": [0, 1, 3] }
    ]
});
            DataTables.ApprovalTableUnpostedProduct = $('#tblDetailUnpostedProduct').DataTable({
            dom: '<"pull-right"f>rt<"bottom"ip><"clear">',
            ordering: false,
            searching: false,
            paging: false,
            data: null,
            autoWidth: false,           
            columns: [
             { "data": "AdjustmentDateFormatted", "defaultContent": "<i></i>" },
             { "data": "Product.Description", "defaultContent": "<i></i>" },
             { "data": "SubComponent.Description", "defaultContent": "<i></i>" },
            { "data": "ProductionTracking.AcceptedQty", "defaultContent": "<i></i>" },

            ],
            columnDefs: [
            { "targets": [0], "width": "12%" },
            { "targets": [1], "width": "15%" },
            { "targets": [2], "width": "20%" },
            { "targets": [3], "width": "12%" },
            { className: "text-right", "targets": [3] },
            { className: "text-center", "targets": [0] },         
            { className: "text-left", "targets": [1,2] }
            ]
        });
    }
    catch (e) {
        console.log(e.message);
    }
});

function ApproveDocument() {
    notyConfirm('Are you sure to Approve Document?', 'ApproveDocumentConfirm()');
}

function ApproveDocumentConfirm() {
    try {
        debugger;
        var DocumentID = $("#DocumentID").val();
        var ApprovalLogID = $("#ID").val();
        var DocumentTypeCode = $("#DocumentType").val();
        var Remarks = $('#Remarks').val();

        var data = { "ApprovalLogID": ApprovalLogID, "DocumentID": DocumentID, "DocumentTypeCode": DocumentTypeCode, "Remarks": Remarks };
        var ds = {};
        ds = GetDataFromServer("DocumentApproval/ApproveDocumentInsert/", data);
        if (ds != '') {
            ds = JSON.parse(ds);
        }
        if (ds.Result == "OK") {
            notyAlert('success', ds.Records.Message);
            DataTables.ApprovalHistoryTable.clear().rows.add(GetApprovalHistory()).draw(false);
            DisableButtons();
            ReloadSummary(DocumentID, DocumentTypeCode);

        }
        if (ds.Result == "ERROR") {
            alert(ds.Message);
        }
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}

function RejectDocument() {
    var Remarks = $('#Remarks').val();
    if (Remarks == "")
        notyAlert('warning', 'Remarks Field is Empty');
    else
        notyConfirm('Are you sure to Reject Document?', 'RejectDocumentConfirm()');
}
function RejectDocumentConfirm()
{
    try
    {
        debugger;
        var DocumentID = $("#DocumentID").val();
        var ApprovalLogID = $("#ID").val();
        var DocumentTypeCode = $("#DocumentType").val();
        var Remarks = $('#Remarks').val();

        //if (Remarks == "")
        //    notyAlert('warning', 'Remarks Field is Empty');
        //else
        //{
            var data = { "ApprovalLogID": ApprovalLogID, "DocumentID": DocumentID, "DocumentTypeCode": DocumentTypeCode, "Remarks": Remarks };
            var ds = {};
            ds = GetDataFromServer("DocumentApproval/RejectDocument/", data);
            if (ds != '') {
                ds = JSON.parse(ds);
            }
            if (ds.Result == "OK") {
                notyAlert('success', ds.Records.Message);
                DataTables.ApprovalHistoryTable.clear().rows.add(GetApprovalHistory()).draw(false);
                DisableButtons();
                ReloadSummary(DocumentID, DocumentTypeCode);
            }
            if (ds.Result == "ERROR") {
                alert(ds.Message);
            }
        //}
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}

function ReloadSummary(DocumentID, DocumentTypeCode)
{
    $("#DocumentSummarydiv").load("./DocumentSummary?DocumentID=" + DocumentID +"&DocumentTypeCode=" + DocumentTypeCode);
}

function ValidateDocumentsApprovalPermission()
{
    debugger;
    var DocumentID = $("#DocumentID").val();
    var DocumentTypeCode = $("#DocumentType").val();

    var data = { "DocumentID": DocumentID, "DocumentTypeCode": DocumentTypeCode };
    var ds = {};
    ds = GetDataFromServer("DocumentApproval/ValidateDocumentsApprovalPermission/", data);
    if (ds != '') {
        ds = JSON.parse(ds);
    }
    debugger;

    if (ds.Records.Status == "False") {
        DisableButtons();
    } 
}

function DisableButtons()
{
    $("#Remarks").attr("disabled", "disabled");
    $("#btnApproveDocument").attr("disabled", "disabled");
    $("#btnApproveDocument").prop("onclick", null);
    $("#btnRejectDocument").attr("disabled", "disabled");
    $("#btnRejectDocument").prop("onclick", null);
}


function BindUnpostedData(id) {
    debugger;
    $('#UnpostedProductDetailModel').modal('show');
    DataTables.ApprovalTableUnpostedProduct.clear().rows.add(GetAllUnpostedData(id)).draw(true);
}
function GetAllUnpostedData(id) {
    try {
        debugger;    
        var data = { "AdjustmentID": id };
        var jsonData = {};
        var result = "";
        var message = "";
        jsonData = GetDataFromServer("FinishedGoodStockAdj/GetAllUnpostedData/", data);
        if (jsonData != '') {
            jsonData = JSON.parse(jsonData);

        }


        if (jsonData.Result == "OK") {
            return jsonData.Record;

        }
        if (jsonData.Result == "ERROR") {
            jsonData.Message;
        }
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}