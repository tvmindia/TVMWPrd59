var DataTables = {};
var EmptyGuid = "00000000-0000-0000-0000-000000000000";
//---------------------------------------Docuement Ready--------------------------------------------------//
$(document).ready(function () {
    try {
        var UserViewModel = new Object();
        DataTables.ObjectTable = $('#tblAppObjects').DataTable(
         {
             dom: '<"pull-right"f>rt<"bottom"ip><"clear">',
             order: [],
             searching: true,
             paging: false,
             data: null,
             columns: [
               { "data": "ID" },
               { "data": "AppObjectObj.ObjectName" },
               { "data": "AppObjectObj.AppName" },
               { "data": "Read" },
               { "data": "Write" },
               { "data": "Delete" },
               { "data": null, "orderable": false }
             ],
             columnDefs: [{ "targets": [0], "visible": false, "searchable": false },
                 {
                     "targets": [3,4,5],
                     "visible": true,
                     "searchable": false,
                     "render": function (data, type, full, meta) {
                         
                         var AccessSpec = "";
                         if (meta.col == 3) {
                             AccessSpec = "Read";
                         }
                         else if (meta.col == 4) {
                             AccessSpec = "Write";
                         }
                         else if (meta.col == 5) {
                             AccessSpec = "Delete";
                         }
                         if (data) {
                             var Checkbox = '<input class="check-box" name="' + AccessSpec + '" type="checkbox" onchange="ChangeAccess(this)" value="true" checked>'
                         }
                         else {
                             var Checkbox = '<input class="check-box" name="' + AccessSpec + '" onchange="ChangeAccess(this)" type="checkbox" value="false"> '
                         }

                         return Checkbox
                     }
                 },
                 {
                     "targets": [6], "render": function (data, type, row) {
                         return '<a href="/ManageAccess/SubobjectIndex/'+row.ObjectID+'?appId=' + $('#ddlApplication').val() + '" >Manage Sub-objects</a>'
                     }
                 }

             ]
         });
        //$('#hdnID').val(EmptyGuid);
        //$('#hdnAppID').val(EmptyGuid);
        $('SELECT').on('change', function () {
            if (($('#ddlApplication').val()) && ($('#ddlRole').val()))
            var ManageAccessViewModel = new Object();
            ManageAccessViewModel.RoleID = $('#ddlRole').val();
            ManageAccessViewModel.AppID = $('#ddlApplication').val();
            TableBind(ManageAccessViewModel);
            ChangeButtonPatchView("ManageAccess", "sectionManageAccessbtnPatch", "Default");
        });
        
    }
    catch (e) {
        notyAlert('error', e.message);
    }
    try
    {
        $('select#ddlRole').trigger('change');
    }
    catch(e)
    {

    }
    
});
function GetAllAppRoles() {
    try {
      
        var data = { "AppID": $('#ddlApplication').val() };
        var ds = {};
        ds = GetDataFromServer("ManageAccess/GetAllAppRoles/", data);
        if (ds != '') {
            ds = JSON.parse(ds);
        }
        if (ds.Result == "OK") {
            //return ds.Records;
          
            $("select#ddlRole").empty();
            $("select#ddlRole").append($("<option>")
             .val('')
             .html("---Select Role---")
             );
            for (var i = 0; i < ds.Records.length; i++)
            {
            
             $("select#ddlRole").append($("<option>")
            .val(ds.Records[i].Value)
            .html(ds.Records[i].Text)
            );
            }
            
        }
        if (ds.Result == "ERROR") {
            alert(ds.Message);
        }
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}
function GobackMangeAccess()
{
  
    window.location = $('#aLinkBack').attr('href');
}
function TableBind(ManageAccessViewModel)
{
    DataTables.ObjectTable.clear().rows.add(GetAllAppObjects(ManageAccessViewModel)).draw(false);
}
function GetAllAppObjects(ManageAccessViewModel) {
    try {
    
        var data = { "AppID": ManageAccessViewModel.AppID, "RoleID": ManageAccessViewModel.RoleID };
        var ds = {};
        ds = GetDataFromServer("ManageAccess/GetAllObjectAccess/", data);
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
function ChangeAccess(this_Obj)
{
  
    ChangeButtonPatchView("ManageAccess", "sectionManageAccessbtnPatch", "Checked");
    var tabledatarow = DataTables.ObjectTable.row($(this_Obj).parents('tr')).data();
    var tabledata = DataTables.ObjectTable.rows().data();
    for (var i = 0; i < tabledata.length; i++) {
        if(tabledata[i].ObjectID==tabledatarow.ObjectID)
        {
            var text=this_Obj.name;
            var value = this_Obj.checked;
            if (text == "Read")
            {
                tabledata[i].Read = value;
            }
            if (text == "Write") {
                tabledata[i].Write = value;
            }
            if (text == "Delete") {
                tabledata[i].Delete = value;
            }
                
        }
    }
    DataTables.ObjectTable.clear().rows.add(tabledata).draw(false);
}
function SaveChanges()
{
  
    var ManageAccessList = [];
    var tabledata = DataTables.ObjectTable.rows().data();
    for (var i = 0; i < tabledata.length; i++)
    {
        var ManageAccessViewModelObj = new Object();
        var AppObjectObj = new Object();
        AppObjectObj.AppID = $('#ddlApplication').val();
        ManageAccessViewModelObj.AppObjectObj = AppObjectObj;
        ManageAccessViewModelObj.ObjectID = tabledata[i].ObjectID;
        ManageAccessViewModelObj.RoleID = tabledata[i].RoleID;
        ManageAccessViewModelObj.Read = tabledata[i].Read;
        ManageAccessViewModelObj.Write = tabledata[i].Write;
        ManageAccessViewModelObj.Delete = tabledata[i].Delete;
        ManageAccessList.push(ManageAccessViewModelObj);
    }
    var ManageAccessViewModel = new Object();
    ManageAccessViewModel.ManageAccessList = ManageAccessList;
    var data = "{'manageAccessViewModelObj':" + JSON.stringify(ManageAccessViewModel) + "}";
    PostDataToServer('ManageAccess/AddAccessChanges/', data, function (JsonResult) {
 
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
function Reset()
{
    if (($('#ddlApplication').val()) && ($('#ddlRole').val()))
    var ManageAccessViewModel = new Object();
    ManageAccessViewModel.RoleID = $('#ddlRole').val();
    ManageAccessViewModel.AppID = $('#ddlApplication').val();
    TableBind(ManageAccessViewModel);
    ChangeButtonPatchView("ManageAccess", "sectionManageAccessbtnPatch", "Default");
}