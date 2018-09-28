//*****************************************************************************
//*****************************************************************************
//Author: Angel
//CreatedDate: 16-Apr-2018 
//LastModified: 17-Apr-2018
//FileName: Employee.js
//Description: Client side coding for NewAssembly
//******************************************************************************
// ##1--Global Declaration
// ##2--Document Ready Function
// #2.1-- Product Componet Table Creation
// #3--LoadComponentDetail Function
// #4--Save Function
// #4.1--Component Stock Checking Function
// #4.2--SaveSuccesssAssembly Function
// #5--BindAssembly Function
// #6--Delete Function
// #6--Reset Function
//******************************************************************************
//##1--Global Declaration---------------------------------------------##1 
var _dataTable = {};
var _emptyGuid = "00000000-0000-0000-0000-000000000000";
//##2--Document Ready Function----------------------------------------##2
$(document).ready(function () {
    debugger;
    try {
        $("#ProductID").select2({
        });
        $("#AssembleBy").select2({
        });
        //##2.1--Product Componet Table Creation----------------------------------------##2.1        
        _dataTable.ComponentListTable = $('#tblComponentList').DataTable(
            {
                dom: '<"pull-right"f>rt<"bottom"ip><"clear">',
                ordering: false,
                searching: false,
                "bInfo": false,
                paging: false,
                data: null,
                pageLength: 15,
                language: {
                    search: "_INPUT_",
                    searchPlaceholder: "Search"
                },
                columns: [
                  {
                      "data": "Product.Name", render: function (data, type, row) {
                          debugger;
                          if (data !== null && data !== "")
                              return data;
                          else
                              return row.Material.Description
                      }, "defaultContent": "<i>-</i>", "width": "30%"
                  },
                  { "data": "BOMQty", "defaultContent": "<i>-</i>", "width": "14%" },
                  { "data": "Stock", "defaultContent": "<i>-</i>", "width": "14%" },
                  { "data": "ReaquiredQty", "defaultContent": "<i>-</i>", "width": "14%" },
                  { "data": "Balance", "defaultContent": "<i>-</i>", "width": "14%" },
                  {
                      "data": "Status", "defaultContent": "<i>-</i>",
                      'render': function (data, type, row) {
                          if (row.Balance >= 0)
                              return "OK"
                          else
                              return "Not Enough Qty"
                      }, "width": "14%"
                  }
                ],
                columnDefs: [
                    { "targets": [1], "width": "2%", },
                     { className: "text-right", "targets": [1,2,3,4] },
                      { className: "text-left", "targets": [0,5] }
                ],
                rowCallback: function (row, data, index) {
                    debugger;
                    if (data.Balance < 0) {
                        $('td', row).css('color', 'Red');
                    }
                },

            });
        if ($('#IsUpdate').val() == 'True') {
            debugger;
            BindAssembling()
            ChangeButtonPatchView('Assembly', 'divbuttonPatchAssembly', 'Edit');
        }
        else {
            $('#lblEntryNo').text('Entry No. # : New');
        }
        $('#lblText').hide();
    }
    catch (e) {
        console.log(e.message);
    }
});
//##3--LoadComponentDetail Function-----------------------------------##3
function LoadComponentDetail()
{
    debugger;
    var $form = $('#AssemblyForm');
    if ($form.valid()) {
        var productid = $("#ProductID").val();
        var qty = $("#Qty").val();
        var assemId = $("#ID").val();
        var componentListVM = GetProductComponentDetails(productid, qty, assemId);
        _dataTable.ComponentListTable.clear().rows.add(componentListVM).draw(false);
    }
    else {
        notyAlert('warning', "Please Fill Required Fields ");
    }
}
function GetProductComponentDetails(productid, qty, assemId) {
    try {
        debugger;
        var data = { "id": productid, "qty": qty, "assemblyId": assemId };
        var jsonData = {};
        var result = "";
        var message = "";
        var componentListVM = new Object();

        jsonData = GetDataFromServer("Assembly/GetProductComponentList/", data);
        if (jsonData != '') {
            jsonData = JSON.parse(jsonData);
            result = jsonData.Result;
            message = jsonData.Message;
            componentListVM = jsonData.Records;
        }
        if (result == "OK") {

            return componentListVM;
        }
        if (result == "ERROR") {
            alert(message);
        }
    }
    catch (e) {
        //this will show the error msg in the browser console(F12) 
        console.log(e.message);
    }
}

function CallChangefunc(productid)
{
    debugger;
    try {
        debugger;
        var productid = $("#ProductID").val();
        var data = { "id": productid};
        var jsonData = {};
        var result = "";
        var message = "";
        var componentListVM = new Object();

        jsonData = GetDataFromServer("Assembly/GetPossibleItemQuantityForAssembly/", data);
        if (jsonData != '') {
            jsonData = JSON.parse(jsonData);
            result = jsonData.Result;
            message = jsonData.Message;
            componentListVM = jsonData.Records;
            $('#lblText').show();
            $('#lblValue').text(jsonData.Records[0].MaxAvailableQuantity);
        }
        if (result == "OK") {
           
            return componentListVM;
        }
        if (result == "ERROR") {
            alert(message);
        }
    }
    catch (e) {
        //this will show the error msg in the browser console(F12) 
        console.log(e.message);
    }
}



//##4--Save---------------------------------------------------------------##4
function Save() {
    debugger;
    var res = CheckComponentList();
    if (res)
    {
        $('#btnSave').trigger('click');
        $('#lblText').hide();
        $('#lblValue').hide();
    }
    else
    {
        notyAlert('warning', 'Component(s) Out Of Stock!');
    }

}
//##4.1--Component Stock Checking Function-----------------------------------##4.1
function CheckComponentList()
{
    var componentDetailsVM = _dataTable.ComponentListTable.rows().data();
    for (var r = 0; r < componentDetailsVM.length; r++) {
        if(componentDetailsVM[r].Balance < 0)
        {
            return false;
        }
    }
    return true;
}
//4.2--Save Success Function-------------------------------------------------##4.2
function SaveSuccessAssembly(data, status) {
    debugger;
    var JsonResult = JSON.parse(data)
    switch (JsonResult.Status) {
        case "OK":
            $('#IsUpdate').val('True');
            $('#ID').val(JsonResult.Record.ID)
            notyAlert("success", JsonResult.Record.Message)
            BindAssembling()
            ChangeButtonPatchView('Assembly', 'divbuttonPatchAssembly', 'Edit');
            break;
        case "ERROR":
            notyAlert("danger", JsonResult.Message)
            break;
        default:
            notyAlert("danger", JsonResult.Message)
            break;
    }
}
//#5--BindAssembling Function---------------------------------------------------##5
function BindAssembling()
{
    ChangeButtonPatchView('Assembly', 'divbuttonPatchAssembly', 'Edit');
    var ID = $('#ID').val();
    var assemblyVM = GetAssembly(ID);
    $('#ID').val(assemblyVM.ID);
    $('#EntryNo').val(assemblyVM.EntryNo);
    $('#AssemblyDateFormatted').val(assemblyVM.AssemblyDateFormatted);
    $('#AssembleBy').val(assemblyVM.AssembleBy).select2();
    $('#ProductID').val(assemblyVM.ProductID).select2();
    $('#Qty').val(assemblyVM.Qty);
    $('#lblEntryNo').text('Entry No. # : ' + assemblyVM.EntryNo);
    $('#lblText').hide();
    LoadComponentDetail();
   
}
function GetAssembly(ID)
{
    try {
        debugger;
        var data = { "id": ID };
        var jsonData = {};
        var result = "";
        var message = "";
        var assemblyVM = new Object();
        jsonData = GetDataFromServer("Assembly/GetAssembly/", data);
        if (jsonData != '') {
            jsonData = JSON.parse(jsonData);
            result = jsonData.Result;
            assemblyVM = jsonData.Records;
            message = jsonData.Message;
        }
        if (result == "OK") {
            return assemblyVM;
        }
        if (result == "ERROR") {
            alert(message);
        }
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}
//#6--Delete Assembly Function------------------------------------------------##6
function DeleteClick() {
    notyConfirm('Are you sure to delete?', 'DeleteAssembly()');
}
function DeleteAssembly() {
    try {
        debugger;
        var id = $('#ID').val();
        if (id != '' && id != null) {
            var data = { "id": id };
            var jsonData = {};
            var result = "";
            var message = "";
            var assemblyVM = new Object();
            jsonData = GetDataFromServer("Assembly/DeleteAssembly/", data);
            if (jsonData != '') {
                jsonData = JSON.parse(jsonData);
                result = jsonData.Status;
                assemblyVM = jsonData.Record;
                message = jsonData.Message;
            }
            if (result == "OK") {
                notyAlert('success', assemblyVM.message);
                window.location.replace("NewAssembly?code=PROD");
            }
            if (result == "ERROR") {
                notyAlert('error', message);
                return 0;
            }
            if (result == "AUTH") {
                notyAlert('error', message);
                return 0;
            }
            return 1;
        }
    }
    catch (e) {
        notyAlert('error', e.message);
        return 0;
    }
}

//#7--Reset Function-------------------------------------------------------##7
function Reset()
{
    BindAssembling();
}