
var DataTables = {};
var EmptyGuid = "00000000-0000-0000-0000-000000000000";
$(document).ready(function () {
    debugger;
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
    { "data": "ApproverName", "defaultContent": "<i>-</i>" },
    { "data": "ApproverLevel", "defaultContent": "<i>-</i>" },
    { "data": "ApprovalDate", "defaultContent": "<i>-</i>" },
    { "data": "ApprovalStatus", "defaultContent": "<i>-</i>" },
    ],
    columnDefs: [
        { className: "text-center", "targets": [2] },
        { className: "text-right", "targets": [] },
        { className: "text-left", "targets": [0, 1, 3] }
    ]
});
    }
    catch (e) {
        console.log(e.message);
    }
});



function ApproveDocument() {
    try {
        debugger;
        var DocumentID = $("#DocumentID").val();
        var ApprovalLogID = $("#ID").val();
        var DocumentTypeCode = $("#DocumentType").val();

        var data = { "ApprovalLogID": ApprovalLogID, "DocumentID": DocumentID, "DocumentTypeCode": DocumentTypeCode};
        var ds = {};
        ds = GetDataFromServer("DocumentApproval/ApproveDocumentInsert/", data);
        if (ds != '') {
            ds = JSON.parse(ds);
        }
        if (ds.Result == "OK") {
            notyAlert('success', ds.Records.Message);
            window.location.replace("ViewPendingDocuments?Code=APR&Name=MyApprovals");
        }
        if (ds.Result == "ERROR") {
            alert(ds.Message);
        }
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}
function RejectDocument()
{
    try
    {
        debugger;
        var DocumentID = $("#DocumentID").val();
        var ApprovalLogID = $("#ID").val();
        var DocumentTypeCode = $("#DocumentType").val();
        var Remarks = $('#Remarks').val();

        if (Remarks == "")
            notyAlert('warning', 'Remarks Field is Empty');
        else
        {
            var data = { "ApprovalLogID": ApprovalLogID, "DocumentID": DocumentID, "DocumentTypeCode": DocumentTypeCode, "Remarks": Remarks };
            var ds = {};
            ds = GetDataFromServer("DocumentApproval/RejectDocument/", data);
            if (ds != '') {
                ds = JSON.parse(ds);
            }
            if (ds.Result == "OK") {
                notyAlert('success', ds.Records.Message);
                window.location.replace("ViewPendingDocuments?Code=APR&Name=MyApprovals");

            }
            if (ds.Result == "ERROR") {
                alert(ds.Message);
            }
        }
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}
