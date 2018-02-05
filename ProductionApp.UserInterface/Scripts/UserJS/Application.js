var DataTables = {};
var EmptyGuid = "00000000-0000-0000-0000-000000000000";
var rowData;
//---------------------------------------Docuement Ready--------------------------------------------------//
$(document).ready(function () {
    ChangeButtonPatchView("Application", "ButtonPatchDiv", "List"); //ControllerName,id of the container div,Name of the action
    try {
        var ApplicationViewModel = new Object();
        DataTables.ApplicationTable = $('#tblList').DataTable(
         {
             dom: '<"pull-left"f>rt<"bottom"ip><"clear">',
             order: [],
             searching: true,
             paging: true,
             data: GetAllApplication(ApplicationViewModel),
             columns: [
               { "data": "ID" },
               { "data": "Name" },
                { "data": "commonDetails.CreatedDatestr", "defaultContent": "<i>-</i>" },
               { "data": null, "orderable": false, "defaultContent": '<a href="#" onclick="Edit(this)"<i class="glyphicon glyphicon-share-alt" aria-hidden="true"></i></a>' }
             ],
             columnDefs: [{ "targets": [0], "visible": false, "searchable": false },
                 { className: "text-left", "targets": [1] },
                 { className: "text-center", "targets": [2] }
             ]
         });
    }
    catch (e) {
        notyAlert('error', e.message);

    }
});

function Edit(currentObj) {
    rowData = DataTables.ApplicationTable.row($(currentObj).parents('tr')).data();
    if ((rowData != null) && (rowData.ID != null)) {
        fillApplication(rowData);
    }
    $("#ManageApplicationEditDiv").show();
    $("#ManageApplicationTableDiv").hide();
    ChangeButtonPatchView("Application", "ButtonPatchDiv", "Edit");
}

function Add() {
    ChangeButtonPatchView("Application", "ButtonPatchDiv", "Add");
    $("#ManageApplicationEditDiv").show();
    $("#ManageApplicationTableDiv").hide();
}

function fillApplication(thisObj) {
    if (thisObj != "") {
        $("#ID").val(thisObj.ID)
        $("#deleteId").val(thisObj.ID)
        $("#Name").val(thisObj.Name)
    }
}

function Back() {
    $("#ID").val(EmptyGuid);
    reset();
    ChangeButtonPatchView("Application", "ButtonPatchDiv", "List");
    $("#ManageApplicationEditDiv").hide();
    $("#ManageApplicationTableDiv").show();
}

function save() { 
        $("#btnInsertUpdate").click(); 
}

function DeleteClick() {
    $("#btnDelete").click();
} 

function reset() {
    //--------Form Reset Validation Errors----//
    var validator = $("#applicationform").validate();
    $('#applicationform').find('.field-validation-error span').each(function () {
        validator.settings.success($(this));
    });
    validator.resetForm();
    //------------------------------------------//

    if ($("#ID").val() != EmptyGuid) {
        ChangeButtonPatchView("Application", "ButtonPatchDiv", "Edit"); 
        fillApplication(rowData);
    }
    else {
        ChangeButtonPatchView("Application", "ButtonPatchDiv", "Add");
        $("#ID").val(EmptyGuid);
        $("#Name").val('');
    }
}

//function GetApplicationDetailsByID(id) {
//    try {
//        var data = { "ID": id };
//        var ds = {};
//        ds = GetDataFromServer("Application/GetApplicationDetailsByID/", data);
//        if (ds != '') {
//            ds = JSON.parse(ds);
//        }
//        if (ds.Result == "OK") {
//            return ds.Records;
//        }
//        if (ds.Result == "ERROR") {
//            notyAlertalert(ds.Message);
//        }
//    }
//    catch (e) {
//        notyAlert('error', e.message);
//    }
//}

function GetAllApplication() {
    try {
        var data = {};
        var ds = {};
        ds = GetDataFromServer("Application/GetAllApplication/", data);
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

function BindAllApplication() {
    try {
        DataTables.ApplicationTable.clear().rows.add(GetAllApplication(false)).draw(false);
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}

function ApplicationSaveSuccess(data, status) {
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
                    $("#deleteId").val(JsonResult.Records.ID);
                    rowData = "";
                }
                ChangeButtonPatchView("Application", "ButtonPatchDiv", "Edit");
                BindAllApplication();
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
            rowData = "";
            reset();
            BindAllApplication();
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



