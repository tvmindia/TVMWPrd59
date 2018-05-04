
function GetApprovalHistory() {
    try {
        debugger;
        var DocumentID = $("#DocumentID").val();
        var DocumentTypeCode = $("#DocumentType").val();
        var data = { "DocumentID": DocumentID, "DocumentTypeCode": DocumentTypeCode };
        var ds = {};
        ds = GetDataFromServer("DocumentApproval/GetApprovalHistory/", data);
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

function BindApprovalHistoryTable() {
    try {
        debugger;
        DataTables.ApprovalHistoryTable = $('#tblApprovalHistory').DataTable({
            dom: '<"pull-right"f>rt<"bottom"ip><"clear">',
            "scrollY": "150px",
            "scrollCollapse": true,
            ordering: false,
            searching: false,
            paging: false,
            bInfo: false,
            data: GetApprovalHistory(),//ApprovalHistory.js
            autoWidth: false,
            columns: [
            { "data": "ApproverName", "defaultContent": "<i>-</i>", "width": "20%" },
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
    }
    catch (e) {
        console.log(e.message);
    }
}