var DataTables = {};
var EmptyGuid = "00000000-0000-0000-0000-000000000000";
var rowData;
//---------------------------------------Docuement Ready--------------------------------------------------//
$(document).ready(function () {
    try {
        //var UserViewModel = new Object();
        DataTables.ObjectTable = $('#tblAppObjects').DataTable(
         {
             dom: '<"pull-right"f>rt<"bottom"ip><"clear">',
             order: [],
             searching: true,
             paging: true,
             data: null,
             columns: [
               { "data": "ID" },
               { "data": "ObjectName" },
               { "data": "AppName" },
               { "data": "commonDetails.CreatedDatestr", "defaultContent": "<i>-</i>" },
               { "data": null, "orderable": false, "defaultContent": '<a href="#" onclick="EditObject(this)"><i class="fa fa-pencil-square-o" aria-hidden="true"></i></a><span> | </span><a href="#" onclick="DeleteObject(this)"><i class="fa fa-trash-o" aria-hidden="true"></i></a>' }
             , { "data": null, "orderable": false}
             ],
             columnDefs: [{ "targets": [0], "visible": false, "searchable": false }, {
                 "targets": [5], "render": function (data, type, row) {
                     return '<a href="/AppObject/Subobjects/' + row.ID + '?appId=' + row.AppID + '" >Manage Sub-objects</a>'
                 } 
             }
             ]
         });
        $('#hdnID').val(EmptyGuid);
        $('#hdnAppID').val(EmptyGuid);
        debugger;
        if ($('#ddlApplication').val() != "")
        {
            DataTables.ObjectTable.clear().rows.add(GetAllAppObjects($('#ddlApplication').val())).draw(false); 
        }
    }
    catch (e) {
        notyAlert('error', e.message);

    }
});
function ChangeObjectData(this_obj)
{
    debugger;
    $('#formEdit').hide();
    $('#hdnAppID').val(this_obj.value);
    ChangeButtonPatchView("AppObject", "btnAppObjectPatch", "select");
    DataTables.ObjectTable.clear().rows.add(GetAllAppObjects(this_obj.value)).draw(false);
}
function GetAllAppObjects(id) {
    try {
        debugger;
        if (id != "") {
            var data = { "ID": id };
            var ds = {};
            ds = GetDataFromServer("AppObject/GetAllAppObjects/", data);
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
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}
function AddNewObject()
{
    debugger;
    rowData = null;
    ChangeButtonPatchView("AppObject", "btnAppObjectPatch", "Edit");
    $('#ObjectName').val('');
    $('#hdnID').val(EmptyGuid);
    $('#formEdit').show();
    $('#AppObjecttbldiv').hide();

}

function goback() {
    debugger;
    $('#AppObjecttbldiv').show();
    $('#formEdit').hide();
    ChangeButtonPatchView("AppObject", "btnAppObjectPatch", "select");
}

function Reset()
{
    debugger;
    if (rowData != null) {
        $('#ObjectName').val(rowData.ObjectName);
        $('#hdnID').val(rowData.ID);
    }
    else {
        $('#ObjectName').val('');
        $('#hdnID').val('');
    }

  
}


function EditObject(this_obj)
{
    debugger;
    ChangeButtonPatchView("AppObject", "btnAppObjectPatch", "Edit");
    $('#formEdit').show();
    $('#AppObjecttbldiv').hide();
    rowData = DataTables.ObjectTable.row($(this_obj).parents('tr')).data();
    $('#ObjectName').val(rowData.ObjectName);
    $('#hdnID').val(rowData.ID);
}
function SaveSuccess(data, status, xhr)
{
    debugger;
    var i = JSON.parse(data)
    switch (i.Result) {
        case "OK":
            notyAlert('success', i.Message);
            $('#hdnID').val(i.Records.ID);
            DataTables.ObjectTable.clear().rows.add(GetAllAppObjects($('#ddlApplication').val())).draw(false);
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
function DeleteObject(this_obj)
{
    debugger;
    var rowData = DataTables.ObjectTable.row($(this_obj).parents('tr')).data();
    $('#hdnAppIDDelete').val(rowData.AppID);
    $('#hdnIDDelete').val(rowData.ID);
    $('#btnDelete').click();
}
function DeleteSuccess(data, status, xhr) {
    debugger;
    var i = JSON.parse(data)
    switch (i.Result) {
        case "OK":
            notyAlert('success', i.Message);            
            DataTables.ObjectTable.clear().rows.add(GetAllAppObjects($('#ddlApplication').val())).draw(false);
            $('#ObjectName').val('');
            $('#hdnID').val(EmptyGuid);
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