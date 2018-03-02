var DataTables = {};
var EmptyGuid = "00000000-0000-0000-0000-000000000000";
//---------------------------------------Docuement Ready--------------------------------------------------//
$(document).ready(function () {
    try {
        var UserViewModel = new Object();
        DataTables.ObjectTable = $('#tblAppSubObjects').DataTable(
         {
             dom: '<"pull-right"f>rt<"bottom"ip><"clear">',
             order: [],
             searching: true,
             paging: false,
             data: null,
             columns: [
               { "data": "ID" },
               { "data": "AppSubObjectObj.SubObjName" },
               {"data":"AppObjectObj.ObjectName"},
               { "data": "AppObjectObj.AppName" },
               { "data": "Read" },
               { "data": "Write" }
             ],
             columnDefs: [{ "targets": [0], "visible": false, "searchable": false },
                 {
                     "targets": [4, 5],
                     "visible": true,
                     "searchable": false,
                     "render": function (data, type, full, meta) {
                         debugger;
                         var AccessSpec = "";
                         if (meta.col == 4) {
                             AccessSpec = "Read";
                         }
                         else if (meta.col == 5) {
                             AccessSpec = "Write";
                         }
                         if (data) {
                             var Checkbox = '<input class="check-box" name="' + AccessSpec + '" type="checkbox" onchange="ChangeAccess(this)" value="true" checked>'
                         }
                         else {
                             var Checkbox = '<input class="check-box" name="' + AccessSpec + '" onchange="ChangeAccess(this)" type="checkbox" value="false"> '
                         }

                         return Checkbox
                     }
                 }

             ]
         });
        //$('#hdnID').val(EmptyGuid);
        //$('#hdnAppID').val(EmptyGuid);
        $('SELECT').on('change', function () {
            if (($('#ddlObject').val()) && ($('#ddlRole').val()))
                var ManageSubObjectAccessViewModel = new Object();
            ManageSubObjectAccessViewModel.RoleID = $('#ddlRole').val();
            ManageSubObjectAccessViewModel.ObjectID = $('#ddlObject').val();
            TableBind(ManageSubObjectAccessViewModel);
            ChangeButtonPatchView("ManageAccess", "sectionManageAccessbtnPatch", "Default");
        });
    }
    catch (e) {
        notyAlert('error', e.message);

    }
});

function GobackMangeAccess() {
    debugger;
    window.location = $('#backlink>a').attr('href') + "?Appid=" + $('#backlink>a').attr('name');
}
function TableBind(ManageSubObjectAccessViewModel) {
    DataTables.ObjectTable.clear().rows.add(GetAllAppObjects(ManageSubObjectAccessViewModel)).draw(false);
}
function GetAllAppObjects(ManageSubObjectAccessViewModel) {
    try {
        debugger;
        var data = { "ObjectID": ManageSubObjectAccessViewModel.ObjectID, "RoleID": ManageSubObjectAccessViewModel.RoleID };
        var ds = {};
        ds = GetDataFromServer("ManageAccess/GetAllSubObjectAccess/", data);
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
function ChangeAccess(this_Obj) {
    debugger;
    ChangeButtonPatchView("ManageAccess", "sectionManageAccessbtnPatch", "Checked");
    var tabledatarow = DataTables.ObjectTable.row($(this_Obj).parents('tr')).data();
    var tabledata = DataTables.ObjectTable.rows().data();
    for (var i = 0; i < tabledata.length; i++) {
        if (tabledata[i].SubObjectID == tabledatarow.SubObjectID) {
            var text = this_Obj.name;
            var value = this_Obj.checked;
            if (text == "Read") {
                tabledata[i].Read = value;
            }
            if (text == "Write") {
                tabledata[i].Write = value;
            }

        }
    }
    DataTables.ObjectTable.clear().rows.add(tabledata).draw(false);
}
function SaveChanges() {
    debugger;
    var ManageSubObjectAccessList = [];
    var tabledata = DataTables.ObjectTable.rows().data();
    for (var i = 0; i < tabledata.length; i++) {
        var ManageSubObjectAccessViewModelObj = new Object();
        var AppSubObjectObj = new Object();
        AppSubObjectObj.ObjectID = $('#ddlObject').val();
        ManageSubObjectAccessViewModelObj.AppSubObjectObj = AppSubObjectObj;
        ManageSubObjectAccessViewModelObj.SubObjectID = tabledata[i].SubObjectID;
        ManageSubObjectAccessViewModelObj.RoleID = tabledata[i].RoleID;
        ManageSubObjectAccessViewModelObj.Read = tabledata[i].Read;
        ManageSubObjectAccessViewModelObj.Write = tabledata[i].Write;
        ManageSubObjectAccessList.push(ManageSubObjectAccessViewModelObj);
    }
    var ManageSubObjectAccessViewModel = new Object();
    ManageSubObjectAccessViewModel.ManageSubObjectAccessList = ManageSubObjectAccessList;
    var data = "{'manageSubObjectAccessVM':" + JSON.stringify(ManageSubObjectAccessViewModel) + "}";
    PostDataToServer('ManageAccess/AddSubObjectAccessChanges/', data, function (JsonResult) {

        var i = JsonResult
        switch (i.Result) {
            case "OK":
                notyAlert('success', i.Message);
                //DataTables.ObjectTable.clear().rows.add(GetAllAppObjects($('#hdnAppID').val())).draw(false);
                //$('#ObjectName').val('');
                //$('#hdnID').val(EmptyGuid);
                break;
            case "Error":
                notyAlert('error', i.Message);
                break;
            case "ERROR":
                notyAlert('error', i.Message);
                break;
            default:
                break;
        }
    })
}
function Reset() {
    if (($('#ddlApplication').val()) && ($('#ddlRole').val()))
        var ManageAccessViewModel = new Object();
    ManageAccessViewModel.RoleID = $('#ddlRole').val();
    ManageAccessViewModel.AppID = $('#ddlApplication').val();
    TableBind(ManageAccessViewModel);
    ChangeButtonPatchView("ManageAccess", "sectionManageAccessbtnPatch", "Default");
}