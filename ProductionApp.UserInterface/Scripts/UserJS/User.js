var DataTables = {};
var EmptyGuid = "00000000-0000-0000-0000-000000000000";
//---------------------------------------Docuement Ready--------------------------------------------------//
$(document).ready(function ()
{
    ChangeButtonPatchView("User", "ButtonPatchDiv", "List"); //ControllerName,id of the container div,Name of the action
    try {
        var UserViewModel = new Object();
        DataTables.userTable = $('#tblUsersList').DataTable(
         {
             dom: '<"pull-left"f>rt<"bottom"ip><"clear">',
             order: [],
             searching: true,
             paging: true,
             data: GetAllUsers(UserViewModel),
             columns: [
               { "data": "ID" },
               { "data": "UserName" },
               { "data": "LoginName" },
               { "data": "Email", "defaultContent": "<i>-</i>" },
               { "data": "RoleCSV", "defaultContent": "<i>-</i>" },//simple,configurable
               { "data": "Active", "defaultContent": "<i>-</i>" },
               { "data": "commonDetails.CreatedDatestr", "defaultContent": "<i>-</i>" },
               { "data": null, "orderable": false, "defaultContent": '<a href="#" onclick="Edit(this)"<i class="glyphicon glyphicon-share-alt" aria-hidden="true"></i></a>' }
             ],
             columnDefs: [{ "targets": [0], "visible": false, "searchable": false },
                 { className: "text-left", "targets": [1, 2, 3,4,5, 6] },
                 { className: "text-center", "targets": [7] }
             ,{ "targets": [5],"render": function (data, type, row) {
                     if (row.Active == true) {
                         return 'Yes';
                     }
                     else {
                         return 'No';
                     }
                 }
            
         }]
         });
    }
    catch (e) {
        notyAlert('error', e.message);

    }    
});

function Edit(currentObj) {
    var rowData = DataTables.userTable.row($(currentObj).parents('tr')).data();
    if ((rowData != null) && (rowData.ID != null)) {
        fillUser(rowData.ID);
    }
    $("#ManageUserEditDiv").show();
    $("#ManageUserTableDiv").hide();
    ChangeButtonPatchView("User", "ButtonPatchDiv", "Edit");
}

function Add() { 
    ChangeButtonPatchView("User", "ButtonPatchDiv", "Add");
    $("#ManageUserEditDiv").show();
    $("#ManageUserTableDiv").hide();
    $('#ddlApplication').val('');
    $('#ddlApplication').trigger('change');
}


function fillUser(id)
{
    debugger;
    var thisObj = GetUserDetailsByID(id); //Binding Data
    $("#ID").val(thisObj.ID)
    $("#deleteId").val(thisObj.ID) 
    $("#UserName").val(thisObj.UserName)
    $("#LoginName").val(thisObj.LoginName) 
    $("#Email").val(thisObj.Email) 
    if (thisObj.Active) { 
        $("#Active").val('true');
    }
    else {
        $("#Active").val('false');
    }
    if (thisObj.RoleIDCSV != null) {
        //bind check boxes here, by spliting Related Categories CSV
        var CSVarray = thisObj.RoleIDCSV.split(",");
        $('input:checkbox').prop('checked', false);
        for (var i = 0 ; i < CSVarray.length; i++) {
            $("#Role_" + CSVarray[i].toLowerCase().trim()).prop('checked', true);
        }
    } 
    $("#Password").val('');
    $("#ConfirmPassword").val('');
}



function Back() {
    $("#ID").val(EmptyGuid);
    reset();
    ChangeButtonPatchView("User", "ButtonPatchDiv", "List");
    $("#ManageUserEditDiv").hide();
    $("#ManageUserTableDiv").show();
}

function save() { 
    var res = Validation();
    if (res) {
        $("#btnInsertUpdateUser").click();
    } 
}

function DeleteClick() {
    $("#btnuserFormDelete").click();
}

function Validation() { 
    /*--------------Checking the Checkboxes Checked 
        and ID's saved in array. array value filled in RolesCSV---------------------*/
    var checkboxCount = $("input:checked").length;
    if (checkboxCount > 0) {
        var checked = [];
        $(":checkbox").each(function () {
            if (this.checked) {
                var cat_Id = new Array();
                cat_Id = this.id.split("_");
                checked.push(cat_Id[1]);
            }
        });
        var CSV = checked.toString();
        $("#RoleCSV").val(CSV);
        //return true;
    }
    //else {
    //    notyAlert('error', 'Please Checked Roles');
    //    return false;
    //}
    return true;
}
function ChangeRolesData(this_Obj)
{
    debugger;
    try {
        $('#divRoleList').empty();
        if (this_Obj.value != "") {
            var data = { "ID": this_Obj.value };
            var ds = {};
            ds = GetDataFromServer("User/GetRolesView/", data);
            $('#divRoleList').html(ds);
            fillUser($("#ID").val());
        }
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}
function reset() {
    //--------Form Reset Validation Errors----//
        var validator = $("#userform").validate();
        $('#userform').find('.field-validation-error span').each(function () {
            validator.settings.success($(this));
        });
        validator.resetForm();
    //------------------------------------------//

        if ($("#ID").val() != EmptyGuid) {
            ChangeButtonPatchView("User", "ButtonPatchDiv", "Edit");
            fillUser($("#ID").val());
        }
        else {
            ChangeButtonPatchView("User", "ButtonPatchDiv", "Add");
            $("#ID").val(EmptyGuid);
            $("#UserName").val('');
            $("#LoginName").val('');
            $("#Password").val('');
            $("#ConfirmPassword").val('');
            $("#Email").val('');  
            $("#Active").val('false');
            $('input:checkbox').prop('checked', false);
        } 
}

function GetUserDetailsByID(id) {
    try {
        var data = { "ID": id };
        var ds = {};
        ds = GetDataFromServer("User/GetUserDetailsByID/", data);
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

function GetAllUsers() {
    try {
        var data = {};
        var ds = {};
        ds = GetDataFromServer("User/GetAllUsers/", data);
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

function BindAllUsers() {
    try {
        DataTables.userTable.clear().rows.add(GetAllUsers(false)).draw(false);
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}

function UserSaveSuccess(data, status) {
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
                    fillUser(JsonResult.Records.ID);

                }
                else {
                    fillUser($("#ID").val());
                }
                debugger;
                ChangeButtonPatchView("User", "ButtonPatchDiv", "Edit");
                BindAllUsers();
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
            BindAllUsers();
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



