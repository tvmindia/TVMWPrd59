
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
