
//*****************************************************************************
//*****************************************************************************
//Author: Jais
//CreatedDate: 12-Mar-2018 
//LastModified: 12-Mar-2018 
//FileName: SubComponent.js
//Description: Client side coding for SubComponent
//******************************************************************************
//******************************************************************************

//--Global Declaration--//
var DataTables = {};
var EmptyGuid = "00000000-0000-0000-0000-000000000000";

//--Loading DOM--//
$(document).ready(function () {
    try {
        debugger;
        BindOrReloadSubComponentTable('Init');
    }
    catch (e) {
        console.log(e.message);
    }
});

//--function bind the SubComponent list checking search and filter--//
function BindOrReloadSubComponentTable(action) {
    try {
        debugger;
        //creating advancesearch object
        SubComponentAdvanceSearchViewModel = new Object();
        DataTablePagingViewModel = new Object();
        UnitViewModel = new Object();
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
        SubComponentAdvanceSearchViewModel.DataTablePaging = DataTablePagingViewModel;
        SubComponentAdvanceSearchViewModel.SearchTerm = $('#SearchTerm').val();

        //apply datatable plugin on Sub Component table
        DataTables.subComponentList = $('#tblSubComponent').DataTable(
        {
            dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
            buttons: [{
                extend: 'excel',
                exportOptions:
                             {
                                 columns: [0,1, 2, 3, 4, 5]
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
                url: "SubComponent/GetAllSubComponent/",
                data: { "subComponentAdvanceSearchVM": SubComponentAdvanceSearchViewModel },
                type: 'POST'
            },
            pageLength: 10,
            columns: [
            //{ "data": "ID", "defaultContent": "<i>-</i>", "width": "10%" },
            { "data": "Code", "defaultContent": "<i>-</i>", "width": "10%" },
            { "data": "Description", "defaultContent": "<i>-<i>", "width": "10%" },
            { "data": "Unit.Description", "defaultContent": "<i>-<i>", "width": "5%" },
            { "data": "OpeningQty", "defaultContent": "<i>-<i>", "width": "10%" },
            { "data": "CurrentQty", "defaultContent": "<i>-<i>", "width": "10%" },
            { "data": "WeightInKG", "defaultContent": "<i>-<i>", "width": "10%" },
            { "data": null, "orderable": false, "defaultContent": '<a href="#" onclick="EditSubComponentMaster(this)"<i class="glyphicon glyphicon-edit" aria-hidden="true"></i></a> <a href="#" onclick="DeleteSubComponentMaster(this)"<i class="glyphicon glyphicon-trash" aria-hidden="true"></i></a>  ', "width": "4%" }
            ],
            columnDefs: [{ "targets": [], "visible": false, "searchable": false },
                { className: "text-right", "targets": [3,4,5] },
                { className: "text-left", "targets": [0,1, 2] },
                { className: "text-center", "targets": [6] }],
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
                    BindOrReloadSubComponentTable('Search');
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
function ResetSubComponentList() {
    BindOrReloadSubComponentTable('Reset');
}

//--function export data to excel--//
function ImportSubComponentData() {
    BindOrReloadSubComponentTable('Export');
}
//--edit SubComponent--//
function EditSubComponentMaster(this_obj) {
    SubComponentViewModel = DataTables.subComponentList.row($(this_obj).parents('tr')).data();
    GetMasterPartial("SubComponent", SubComponentViewModel.ID);
    $('#h3ModelMasterContextLabel').text('Edit Sub Component')
    $('#divModelMasterPopUp').modal('show');
    $('#hdnMasterCall').val('MSTR');
}

//--Function To Confirm SubComponent Deletion 
function DeleteSubComponentMaster(this_obj) {
    debugger;
    subComponentVM = DataTables.subComponentList.row($(this_obj).parents('tr')).data();
    notyConfirm('Are you sure to delete?', 'DeleteSubComponent("' + subComponentVM.ID + '")');
}

//--Function To Delete SubComponent
function DeleteSubComponent(id) {
    debugger;
    try {
        if (id) {
            var data = { "id": id };
            var jsonData = {};
            var message = "";
            var status = "";
            var result = "";
            jsonData = GetDataFromServer("SubComponent/DeleteSubComponent/", data);
            if (jsonData != '') {
                jsonData = JSON.parse(jsonData);
                message = jsonData.Message;
                status = jsonData.Status;
                result = jsonData.Record;
            }
            switch (status) {
                case "OK":
                    notyAlert('success', result.Message);
                    BindOrReloadSubComponentTable('Reset');
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