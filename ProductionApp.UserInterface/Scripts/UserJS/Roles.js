var DataTables = {};
var EmptyGuid = "00000000-0000-0000-0000-000000000000";
//---------------------------------------Docuement Ready--------------------------------------------------//
$(document).ready(function () {
   
    ChangeButtonPatchView("Roles", "ButtonPatchDiv", "List"); //ControllerName,id of the container div,Name of the action
    try {
        var RolesViewModel = new Object();
        DataTables.RolesTable = $('#tblList').DataTable(
         {
             dom: '<"pull-left"f>rt<"bottom"ip><"clear">',
             order: [],
             searching: true,
             paging: true,
             data: GetAllRoles(RolesViewModel),
             columns: [
               { "data": "ID" },
                  { "data": "RoleName" },
               { "data": "ApplicationName" },
               { "data": "RoleDescription", "defaultContent": "<i>-</i>" },
               { "data": "commonDetails.CreatedDatestr", "defaultContent": "<i>-</i>" },
               { "data": null, "orderable": false, "defaultContent": '<a href="#" onclick="Edit(this)"<i class="glyphicon glyphicon-share-alt" aria-hidden="true"></i></a>' }
             ],
             columnDefs: [{ "targets": [0], "visible": false, "searchable": false },
                 { className: "text-left", "targets": [1, 2, 3] },
                 { className: "text-center", "targets": [4] }
            ]
         });
    }
    catch (e) {
        notyAlert('error', e.message);

    }
});

function Edit(currentObj) {
    var rowData = DataTables.RolesTable.row($(currentObj).parents('tr')).data();
    if ((rowData != null) && (rowData.ID != null)) {
        fillRoles(rowData.ID);
    }
    $("#ManageRolesEditDiv").show();
    $("#ManageRolesTableDiv").hide();
    ChangeButtonPatchView("Roles", "ButtonPatchDiv", "Edit");
}

function Add() {
    ChangeButtonPatchView("Roles", "ButtonPatchDiv", "Add");
    $("#ManageRolesEditDiv").show();
    $("#ManageRolesTableDiv").hide();
}


function fillRoles(id) {
    debugger;
    var thisObj = GetRolesDetailsByID(id); //Binding Data
    $("#ID").val(thisObj.ID)
    $("#deleteId").val(thisObj.ID)
    $("#RoleName").val(thisObj.RoleName)
    $("#RoleDescription").val(thisObj.RoleDescription) 
    $("#AppID").val(thisObj.AppID);
  
}



function Back() {
    $("#ID").val(EmptyGuid);
    reset();
    ChangeButtonPatchView("Roles", "ButtonPatchDiv", "List");
    $("#ManageRolesEditDiv").hide();
    $("#ManageRolesTableDiv").show();
}

function save() {
        $("#btnInsertUpdate").click();
}

function DeleteClick() {
    $("#btnDelete").click();
}


function reset() {
    //--------Form Reset Validation Errors----//
    var validator = $("#Rolesform").validate();
    $('#Rolesform').find('.field-validation-error span').each(function () {
        validator.settings.success($(this));
    });
    validator.resetForm();
    //------------------------------------------//

    if ($("#ID").val() != EmptyGuid) {
        ChangeButtonPatchView("Roles", "ButtonPatchDiv", "Edit");
        fillRoles($("#ID").val());
    }
    else {
        ChangeButtonPatchView("Roles", "ButtonPatchDiv", "Add");
        $("#ID").val(EmptyGuid);
        $("#Name").val('');
        $("#LoginName").val('');
     
        $("#deleteId").val('')
        $("#RoleName").val('')
        $("#RoleDescription").val('')
        $("#AppID").val('');
    }
}

function GetRolesDetailsByID(id) {
    try {
        var data = { "ID": id };
        var ds = {};
        ds = GetDataFromServer("Roles/GetRolesDetailsByID/", data);
        if (ds != '') {
            ds = JSON.parse(ds);
        }
        if (ds.Result == "OK") {
            return ds.Records;
        }
        if (ds.Result == "ERROR") {
            notyAlertalert(ds.Message);
        }
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}

function GetAllRoles() {
    try {
        debugger;
        var data = {};
        var ds = {};
        ds = GetDataFromServer("Roles/GetAllRoles/", data);
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

function BindAllRoles() {
    try {
        DataTables.RolesTable.clear().rows.add(GetAllRoles(false)).draw(false);
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}

function RolesSaveSuccess(data, status) {
    debugger;
    var JsonResult = JSON.parse(data)
    if (JsonResult.Records.Status == 2) {
        notyAlert('error', JsonResult.Records.Message);
    }
    else {
        switch (JsonResult.Result) {
            case "OK":
                if ($("#ID").val() == EmptyGuid) {


                    $("#ID").val(JsonResult.Records.ID);
                    fillRoles(JsonResult.Records.ID);

                }
                else {
                    fillRoles($("#ID").val());
                }
                debugger;
                ChangeButtonPatchView("Roles", "ButtonPatchDiv", "Edit");
                BindAllRoles();
                notyAlert('success', JsonResult.Records.Message);
                break;
            case "ERROR":
                notyAlert('error', "Error!");
                break;
            default:
                notyAlert('error', JsonResult.Message);
                break;
        }

    }

}
function DeleteSuccess(data, status) {
    debugger;
    var JsonResult = JSON.parse(data)
    switch (JsonResult.Result) {
        case "OK":
            $("#ID").val(EmptyGuid);
            reset();
            BindAllRoles();
            notyAlert('success', JsonResult.Records.Message);
            break;
        case "ERROR":
            notyAlert('error', "Error!");
            break;
        default:
            notyAlert('error', JsonResult.Message);
            break;
    }
}




