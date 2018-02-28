
function GetApprovalHistory() {
    try {
        debugger;
        var DocumentID = "CCAF70C5-3D71-429A-B817-474461F2C29A";
        var DocumentTypeCode = "PO";
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
