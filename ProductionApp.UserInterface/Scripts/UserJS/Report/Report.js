
function RedirectSearchClick(e, this_obj) {
    debugger;
    var unicode = e.charCode ? e.charCode : e.keyCode
    if (unicode == 13) {
        $(this_obj).closest('.input-group').find('button').trigger('click')
    }
}
//Click function for search
function RefreshReportSummary() {
    try {
        debugger;
        var search = $("#SearchTerm").val();
        //GetAllReports();
        window.location.replace("DashboardReport?Code=RPT&searchTerm="+search);
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}


//function GetAllReports() {
//    try {
//        debugger;
//        if ($("#SearchTerm").val() != "")
//            var search = $("#SearchTerm").val();
//        var data = {"SearchTerm": search };
//        var ds = {};
//        ds = GetDataFromServer("DashboardReport/GetAllReport/", data);
//        if (ds != '') {
//            ds = JSON.parse(ds);
//        }
//        if (ds.Result == "OK") {
//            return ds.Records;
//        }
//        if (ds.Result == "ERROR") {
//            alert(ds.Message);
//        }
//    }
//    catch (e) {
//        notyAlert('error', e.message);
//    }
//}
