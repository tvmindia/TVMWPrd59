
//*****************************************************************************
//*****************************************************************************
//Author: Jais
//CreatedDate: 20-Feb-2018 
//LastModified: 20-Feb-2018 
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
});


//--function bind the Approver list checking search and filter--//
function BindOrReloadApproverTable(action) {
    try {
        //creating advancesearch object
        ApproverAdvanceSearchViewModel = new Object();
        DataTablePagingViewModel = new Object();
        DataTablePagingViewModel.Length = 0;
        //switch case to check the operation
        switch (action) {
            case 'Reset':
                $('#SearchTerm').val('');
                break;
            case 'Init':
                break;
            case 'Search':
                break;
            case 'Export':
                if ($('#SearchTerm').val() == "")
                    DataTablePagingViewModel.Length = -1;
                break;
            default:
                break;
        }
        ApproverAdvanceSearchViewModel.DataTablePaging = DataTablePagingViewModel;
        ApproverAdvanceSearchViewModel.SearchTerm = $('#SearchTerm').val();

        //apply datatable plugin on Approver table
        DataTables.approverList = $('#tblApprover').DataTable(
        {
            dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
            buttons: [{
                extend: 'excel',
                exportOptions:
                             {
                                 columns: [1, 2, 3, 4, 5, 6]
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
            { "data": "DocType", "defaultContent": "<i>-</i>" },
            { "data": "Level", "defaultContent": "<i>-</i>" },
            { "data": "UserID", "defaultContent": "<i>-<i>" },
            { "data": "User.LoginName", "defaultContent": "<i>-<i>" },
            { "data": "IsDefault", "defaultContent": "<i>-<i>" },
            { "data": "IsActive", "defaultContent": "<i>-<i>" },
            { "data": null, "orderable": false, "defaultContent": '<a href="#" onclick="EditApproverMaster(this)"<i class="glyphicon glyphicon-share-alt" aria-hidden="true"></i></a>' }
            ],
            columnDefs: [{ "targets": [0,3], "visible": false, "searchable": false },
                { className: "text-right", "targets": [ 6] },
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
                    ResetApproverList();
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
function AddApproverMaster() {
    GetMasterPartial("Approver", "");
    $('#h3ModelMasterContextLabel').text('Add Approver')
    $('#divModelMasterPopUp').modal('show');
}
//-- Function After Save Approver--//
function SaveSuccessApprover(data, status) {
    debugger;
    var JsonResult = JSON.parse(data)
    switch (JsonResult.Result) {
        case "OK":
            $('#IsUpdate').val('True');
            $('#ID').val(JsonResult.Records.ID);
            MasterAlert("success", JsonResult.Records.Message)
            BindOrReloadApproverTable('Reset');
            break;
        case "ERROR":
            MasterAlert("danger", JsonResult.Message)
            break;
        default:
            MasterAlert("danger", JsonResult.Message)
            break;
    }
}
