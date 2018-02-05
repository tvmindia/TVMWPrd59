var DataTables = {};
var EmptyGuid = "00000000-0000-0000-0000-000000000000";
//---------------------------------------Docuement Ready--------------------------------------------------//
$(document).ready(function () {

   // ChangeButtonPatchView("Privileges", "ButtonPatchDiv", "List"); //ControllerName,id of the container div,Name of the action
    try {
        var PrivilegesViewModel = new Object();
        DataTables.PrivilegesTable = $('#tblPrivilegesList').DataTable(
         {
             dom: '<"pull-left"f>rt<"bottom"ip><"clear">',
             order: [],
             searching: true,
             paging: true,
             data: GetAllPrivileges(PrivilegesViewModel),
             columns: [
               { "data": "ID" },
               { "data": "ApplicationName" },
               { "data": "RoleName" },
               { "data": "ModuleName", "defaultContent": "<i>-</i>" },
               { "data": "AccessDescription", "defaultContent": "<i>-</i>" },
               { "data": "commonDetails.CreatedDatestr", "defaultContent": "<i>-</i>" },
             ],
             columnDefs: [{ "targets": [0], "visible": false, "searchable": false },
                 { className: "text-left", "targets": [1, 2, 3, 4, 5] }
                ]
         });
    }
    catch (e) {
        notyAlert('error', e.message);

    }
});

function GetAllPrivileges() {
    try {
        var data = {};
        var ds = {};
        ds = GetDataFromServer("Privileges/GetAllPrivilegesForPV/", data);
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