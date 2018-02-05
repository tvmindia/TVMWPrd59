var DataTables = {};
var EmptyGuid = "00000000-0000-0000-0000-000000000000";
var rowData;
//---------------------------------------Docuement Ready--------------------------------------------------//
$(document).ready(function () {
    try {
        debugger; 
        var SubObjectViewModel = new Object();
        DataTables.SubObjectTable = $('#tblsubObjects').DataTable(
         {
             dom: '<"pull-right"f>rt<"bottom"ip><"clear">',
             order: [],
             searching: true,
             paging: true,
             data: GetAllAppObjects(SubObjectViewModel),
             columns: [
               { "data": "ID" },
               { "data": "AppName" },
               { "data": "ObjectName" },
               { "data": "SubObjName" }, 
               { "data": "commonDetails.CreatedDatestr", "defaultContent": "<i>-</i>" },
               { "data": null, "orderable": false, "defaultContent": '<a href="#" onclick="EditObject(this)"><i class="fa fa-pencil-square-o" aria-hidden="true"></i></a><span> | </span><a href="#" onclick="DeleteObject(this)"><i class="fa fa-trash-o" aria-hidden="true"></i></a>' }
             ],
             columnDefs: [{ "targets": [0], "visible": false, "searchable": false } 
             ]
         }); 
    }
    catch (e) {
        notyAlert('error', e.message);

    }
});



function GetAllAppObjects() {
    try {
        debugger;
        var id = $('#ddlApplication').val()
        var data = { "ID": id };
        var ds = {};
        ds = GetDataFromServer("AppObject/GetAllAppSubObjects/", data);
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


function EditObject(this_obj) {
    debugger;
    ChangeButtonPatchView("AppObject", "btnAppObjectPatch", "subEdit");
    rowData = DataTables.SubObjectTable.row($(this_obj).parents('tr')).data();
    $('#SubObjName').val(rowData.SubObjName);
    $('#ddlObject').val(rowData.ObjectID);
    $('#ID').val(rowData.ID); 

}

function Reset() {
    debugger;
    $('#ID').val(EmptyGuid);
    $('#ddlObject').val('');
    $('#SubObjName').val('');
}



function GoBack() {
    debugger;
    window.location = '/AppObject/Index?appId=' + $('#ddlApplication').val();
}


function SaveSuccess(data, status, xhr) {
    debugger;
    var i = JSON.parse(data)
    switch (i.Result) {
        case "OK":
            notyAlert('success', i.Message);
            $('#ID').val(i.Records.ID);
            $('#hdnIDDelete').val(i.Records.ID);
            DataTables.SubObjectTable.clear().rows.add(GetAllAppObjects(false)).draw(false);
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
}
function DeleteObject(this_obj) {
    debugger;
    rowData = DataTables.SubObjectTable.row($(this_obj).parents('tr')).data();
    $('#hdnIDDelete').val(rowData.ID);
    $('#btnDelete').click();
}
function DeleteSuccess(data, status, xhr) {
    debugger;
    var i = JSON.parse(data)
    switch (i.Result) {
        case "OK":
            notyAlert('success', i.Message);
            DataTables.SubObjectTable.clear().rows.add(GetAllAppObjects(false)).draw(false);
            $('#ObjectID').val('');
            $('#SubObjName').val('');
            $('#ID').val(EmptyGuid);
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
}