var DataTables = {};
var EmptyGuid = "00000000-0000-0000-0000-000000000000";
//---------------------------------------Docuement Ready--------------------------------------------------//
$(document).ready(function () {
   
    ChangeButtonPatchView("Privileges", "ButtonPatchDiv", "List"); //ControllerName,id of the container div,Name of the action
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
               { "data": null, "orderable": false, "defaultContent": '<a href="#" onclick="Edit(this)"<i class="glyphicon glyphicon-share-alt" aria-hidden="true"></i></a>' }
             ],
             columnDefs: [{ "targets": [0], "visible": false, "searchable": false },
                 { className: "text-left", "targets": [1, 2, 3, 4, 5] },
                 { className: "text-center", "targets": [6] }]
         });
    }
    catch (e) {
        notyAlert('error', e.message);

    }
});

function Edit(currentObj) {
    var rowData = DataTables.PrivilegesTable.row($(currentObj).parents('tr')).data();
    if ((rowData != null) && (rowData.ID != null)) {
        fillPrivileges(rowData.ID);
    }
    $("#ManagePrivilegesEditDiv").show();
    $("#ManagePrivilegesTableDiv").hide();
    ChangeButtonPatchView("Privileges", "ButtonPatchDiv", "Edit");
}

function Add() {
    ChangeButtonPatchView("Privileges", "ButtonPatchDiv", "Add");
    $("#ManagePrivilegesEditDiv").show();
    $("#ManagePrivilegesTableDiv").hide();
}


function fillPrivileges(id) {
    debugger;
    var thisObj = GetPrivilegesDetailsByID(id); //Binding Data
    $("#ID").val(thisObj.ID)
    $("#deleteId").val(thisObj.ID)
    $("#RoleID").val(thisObj.RoleID)
    $("#AppID").val(thisObj.AppID)
    $("#AccessDescription").val(thisObj.AccessDescription)
    $("#ModuleName").val(thisObj.ModuleName)
}



function Back() {
    $("#ID").val(EmptyGuid);
    reset();
    ChangeButtonPatchView("Privileges", "ButtonPatchDiv", "List");
    $("#ManagePrivilegesEditDiv").hide();
    $("#ManagePrivilegesTableDiv").show();
}

function save() { 
    $("#btnInsertUpdate").click(); 
}

function DeleteClick() {
    $("#btnDelete").click();
}


function reset() {
    //--------Form Reset Validation Errors----//
    var validator = $("#Privilegesform").validate();
    $('#Privilegesform').find('.field-validation-error span').each(function () {
        validator.settings.success($(this));
    });
    validator.resetForm();
    //------------------------------------------//

    if ($("#ID").val() != EmptyGuid) {
        ChangeButtonPatchView("Privileges", "ButtonPatchDiv", "Edit");
        fillPrivileges($("#ID").val());
    }
    else {
        ChangeButtonPatchView("Privileges", "ButtonPatchDiv", "Add");
        $("#ID").val(EmptyGuid);
        $("#RoleID").val('');
        $("#AppID").val('');
        $("#AccessDescription").val('');
        $("#ModuleName").val('');
        $("#deleteId").val('');
    }
}

function GetPrivilegesDetailsByID(id) {
    try {
        var data = { "ID": id };
        var ds = {};
        ds = GetDataFromServer("Privileges/GetPrivilegesDetailsByID/", data);
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

function GetAllPrivileges() {
    try {
        var data = {};
        var ds = {};
        ds = GetDataFromServer("Privileges/GetAllPrivileges/", data);
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

function BindAllPrivileges() {
    try {
        DataTables.PrivilegesTable.clear().rows.add(GetAllPrivileges(false)).draw(false);
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}

function PrivilegesSaveSuccess(data, status) {
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
                    fillPrivileges(JsonResult.Records.ID); 
                }
                else {
                    fillPrivileges($("#ID").val());
                }
                debugger;
                ChangeButtonPatchView("Privileges", "ButtonPatchDiv", "Edit");
                BindAllPrivileges();
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
            BindAllPrivileges();
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



