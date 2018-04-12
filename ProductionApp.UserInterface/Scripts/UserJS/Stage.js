
//*****************************************************************************
//*****************************************************************************
//Author: Jais
//CreatedDate: 9-Mar-2018 
//LastModified:  9-Mar-2018 
//FileName: Stage.js
//Description: Client side coding for Stage
//******************************************************************************
//******************************************************************************

//--Global Declaration--//
var DataTables = {};
var EmptyGuid = "00000000-0000-0000-0000-000000000000";

//--Loading DOM--//
$(document).ready(function () {
    try {
        debugger;
        BindOrReloadStageTable('Init');

    }
    catch (e) {
        console.log(e.message);
    }
});


//--function bind the Stage list checking search and filter--//
function BindOrReloadStageTable(action) {
    try {
        debugger;
        //creating advancesearch object
        StageAdvanceSearchViewModel = new Object();
        DataTablePagingViewModel = new Object();
        StageViewModel = new Object();
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
                    DataTablePagingViewModel.Length = -1;
                break;
            default:
                break;
        }
        StageAdvanceSearchViewModel.DataTablePaging = DataTablePagingViewModel;
        StageAdvanceSearchViewModel.SearchTerm = $('#SearchTerm').val();

        //apply datatable plugin on Stage table
        DataTables.stageList = $('#tblStage').DataTable(
        {
            dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
            buttons: [{
                extend: 'excel',
                exportOptions:
                             {
                                 columns: [0]
                             }
            }],
            ordering: false,
            searching: false,
            paging: true,
            lengthChange: false,
            processing: true,
            language: {

                "processing": "<div class='spinner'><div class='bounce1'></div><div class='bounce2'></div><div class='bounce3'></div></div>"
            },
            serverSide: true,
            ajax: {
                url: "Stage/GetAllStage/",
                data: { "stageAdvanceSearchVM": StageAdvanceSearchViewModel },
                type: 'POST'
            },
            pageLength: 10,
            columns: [
            //{ "data": "ID", "defaultContent": "<i>-</i>" },
            { "data": "Description", "defaultContent": "<i>-</i>", "width": "10%" },
            { "data": null, "orderable": false, "defaultContent": '<a href="#" onclick="DeleteStageMaster(this)"<i class="glyphicon glyphicon-trash" aria-hidden="true"></i></a>  <a href="#" onclick="EditStageMaster(this)"<i class="glyphicon glyphicon-share-alt" aria-hidden="true"></i></a>', "width": "4%" }
            ],
            columnDefs: [{ "targets": [], "visible": false, "searchable": false },
                { className: "text-right", "targets": [] },
                { className: "text-left", "targets": [0] },
                { className: "text-center", "targets": [1] }],
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
                    BindOrReloadStageTable('Search');
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
function ResetStageList() {
    BindOrReloadStageTable('Reset');
}

//--function export data to excel--//
function ImportStageData() {
    BindOrReloadStageTable('Export');
}

//--edit Stage--//
function EditStageMaster(this_obj) {
    StageViewModel = DataTables.stageList.row($(this_obj).parents('tr')).data();
    GetMasterPartial("Stage", StageViewModel.ID);
    $('#h3ModelMasterContextLabel').text('Edit Stage')
    $('#divModelMasterPopUp').modal('show');
}


//--Function To Confirm Stage Deletion 
function DeleteStageMaster(this_obj) {
    debugger;
    stageVM = DataTables.stageList.row($(this_obj).parents('tr')).data();
    notyConfirm('Are you sure to delete?', 'DeleteStage("' + stageVM.ID + '")');
}

//--Function To Delete Product
function DeleteStage(id) {
    debugger;
    try {
        if (id) {
            var data = { "id": id };
            var jsonData = {};
            var message = "";
            var status = "";
            var result = "";
            jsonData = GetDataFromServer("Stage/DeleteStage/", data);
            if (jsonData != '') {
                jsonData = JSON.parse(jsonData);
                message = jsonData.Message;
                status = jsonData.Status;
                result = jsonData.Record;
            }
            switch (status) {
                case "OK":
                    notyAlert('success', result.Message);
                    BindOrReloadStageTable('Reset');
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



