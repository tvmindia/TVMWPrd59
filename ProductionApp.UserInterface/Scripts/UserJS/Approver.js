
//*****************************************************************************
//*****************************************************************************
//Author: Jais
//CreatedDate: 20-Feb-2018 
//LastModified: 5-Mar-2018 
//FileName: Approver.js
//Description: Client side coding for Approver
//******************************************************************************
//******************************************************************************

//--Global Declaration--//
var DataTables = {};
var EmptyGuid = "00000000-0000-0000-0000-000000000000";

//--Loading DOM--//
$(document).ready(function () {
    try {
        debugger;
        BindOrReloadApproverTable('Init');
        
    }
    catch (e) {
        console.log(e.message);
    }
    $("#DocumentType_Code").select2({
    });
    
});


//--function bind the Approver list checking search and filter--//
function BindOrReloadApproverTable(action) {
    try {
        debugger;
        //creating advancesearch object
        ApproverAdvanceSearchViewModel = new Object();
        DataTablePagingViewModel = new Object();
        DocumentTypeViewModel = new Object();
        ApproverViewModel = new Object();
        DataTablePagingViewModel.Length = 0;
        //switch case to check the operation
        switch (action) {
            case 'Reset':
                $('#SearchTerm').val('');
                $('#DocumentType_Code').val('').select2();
                break;
            case 'Init':
                break;
            case 'Search':
                break;
            case 'Apply':
                    break;
            case 'Export':
                    DataTablePagingViewModel.Length = -1;
                break;
            default:
                break;
        }
        ApproverAdvanceSearchViewModel.DataTablePaging = DataTablePagingViewModel;
        DocumentTypeViewModel.Code = $('#DocumentType_Code').val();
        ApproverAdvanceSearchViewModel.DocumentType = DocumentTypeViewModel;
       // ApproverViewModel.Level = $('#Approver_Level').val();
       // ApproverAdvanceSearchViewModel.Approver = ApproverViewModel();
        ApproverAdvanceSearchViewModel.SearchTerm = $('#SearchTerm').val();

        //apply datatable plugin on Approver table
        DataTables.approverList = $('#tblApprover').DataTable(
        {
            dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
            buttons: [{
                extend: 'excel',
                exportOptions:
                             {
                                 columns: [1, 2, 4, 5, 6]
                             }
            }],
            order: false,
            searching: false,
            paging: true,
            lengthChange: false,
            processing: true,
            language: {

                "processing": "<div class='spinner'><div class='bounce1'></div><div class='bounce2'></div><div class='bounce3'></div></div>"
            },
            serverSide: true,
            ajax: {
                url: "Approver/GetAllApprover/",
                data: { "approverAdvanceSearchVM": ApproverAdvanceSearchViewModel },
                type: 'POST'
            },
            pageLength: 10,
            columns: [
            { "data": "ID", "defaultContent": "<i>-</i>" },
            { "data": "DocumentType.Description", "defaultContent": "<i>-</i>" },
            { "data": "Level", "defaultContent": "<i>-</i>" },
            { "data": "UserID", "defaultContent": "<i>-<i>" },
            { "data": "User.LoginName", "defaultContent": "<i>-<i>" },
            { "data": "IsDefault", "defaultContent": "<i>-<i>" },
            { "data": "IsActive", "defaultContent": "<i>-<i>" },
            { "data": null, "orderable": false, "defaultContent": '<a href="#" onclick="DeleteApproverMaster(this)"<i class="glyphicon glyphicon-trash" aria-hidden="true"></i></a>  <a href="#" onclick="EditApproverMaster(this)"<i class="glyphicon glyphicon-share-alt" aria-hidden="true"></i></a>',"width":"4%" }
            ],
            columnDefs: [{ "targets": [0,3], "visible": false, "searchable": false },
                { className: "text-right", "targets": [ 7] },
                { className: "text-left", "targets": [1, 2, 4, 5, 6] },
                { className: "text-center", "targets": [] }],
            destroy: true,
            //for performing the import operation after the data loaded
            initComplete: function (settings, json) {
                if (action === 'Export') {
                    if (json.data.length > 0) {
                        if (json.data[0].TotalCount > 10000) {
                            MasterAlert("info", 'We are able to download maximum 10000 rows of data, There exist more than 10000 rows of data please filter and download')
                        }
                    }
                    $(".buttons-excel").trigger('click');
                    BindOrReloadApproverTable('Search');
                }
            }
        });
        $(".buttons-excel").hide();
    }
    catch (e) {
        console.log(e.message);
    }
}

//--function reset the list to initial--//
function ResetApproverList() {
    BindOrReloadApproverTable('Reset');
}

//--function export data to excel--//
function ImportApproverData() {
    BindOrReloadApproverTable('Export');
}

//-- add Approver--//
function AddApproverMaster(flag) {
    GetMasterPartial("Approver", "");
    $('#h3ModelMasterContextLabel').text('Add Approver')
    $('#divModelMasterPopUp').modal('show');
    $('#hdnMasterCall').val(flag);
}

//--edit Approver--//
function EditApproverMaster(this_obj) {
    debugger;
    rowData = DataTables.approverList.row($(this_obj).parents('tr')).data();
    GetMasterPartial("Approver", rowData.ID);
    $('#h3ModelMasterContextLabel').text('Edit Approver')
    $('#divModelMasterPopUp').modal('show');
    $('#hdnMasterCall').val('MSTR');
    if ($('#IsDefault').is(":checked")) {
        $('#IsDefault').prop("disabled",true);
    }
    else {
        $('#IsDefault').prop("disabled", false);
    }
}

//--Function To Confirm Approver Deletion 
function DeleteApproverMaster(this_obj) {
    debugger;
    approverVM = DataTables.approverList.row($(this_obj).parents('tr')).data();
    if (approverVM.IsDefault==true) {
        notyAlert('error', 'Cannot Delete Default Approver');
    } else {
        notyConfirm('Are you sure to delete?', 'DeleteApprover("' + approverVM.ID + '")');
    }
}

//--Function To Delete Approver
function DeleteApprover(id) {
    debugger;
    try {
        if (id) {
            var data = { "id": id};
            var jsonData = {};
            var message = "";
            var status = "";
            var result = "";
            jsonData = GetDataFromServer("Approver/DeleteApprover/", data);
            if (jsonData != '') {
                jsonData = JSON.parse(jsonData);
                message = jsonData.Message;
                status = jsonData.Status;
                result = jsonData.Record;
            }
            switch (status) {
                case "OK":
                    notyAlert('success', result.Message);
                    BindOrReloadApproverTable();
                    break;
                case "ERROR":
                    notyAlert('error', message);
                    break;
                default:
                    break;
            }
        }
    }
    catch (e) {
        //this will show the error msg in the browser console(F12) 
        console.log(e.message);
    }
}
