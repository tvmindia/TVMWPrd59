
//*****************************************************************************
//*****************************************************************************
//Author: Jais
//CreatedDate:16-Apr-2018 
//LastModified: 16-Apr-2018 
//FileName: ChartOfAccount.js
//Description: Client side coding for Chart Of Account
//******************************************************************************
//******************************************************************************

//--Global Declaration--//
var _dataTables = {};
var _emptyGuid = "00000000-0000-0000-0000-000000000000";

//--Loading DOM--//
$(document).ready(function () {
    try {
        debugger;
        BindOrReloadChartOfAccountTable('Init');
    }
    catch (e) {
        console.log(e.message);
    }
});


//--function bind the ChartOfAccount list checking search and filter--//
function BindOrReloadChartOfAccountTable(action) {
    try {
        debugger;
        //creating advancesearch object
        DataTablePagingViewModel = new Object();
        ChartOfAccountAdvanceSearchViewModel = new Object();
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
        ChartOfAccountAdvanceSearchViewModel.DataTablePaging = DataTablePagingViewModel;
        ChartOfAccountAdvanceSearchViewModel.SearchTerm = $('#SearchTerm').val();

        //apply datatable plugin on ChartOfAccount table
        _dataTables.chartOfAccountList = $('#tblChartOfAccount').DataTable(
        {
            dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
            buttons: [{
                extend: 'excel',
                exportOptions:
                             {
                                 columns: [0,1, 2, 3]
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
                url: "ChartOfAccount/GetAllChartOfAccount/",
                data: { "chartOfAccountAdvanceSearchVM": ChartOfAccountAdvanceSearchViewModel },
                type: 'POST'
            },
            pageLength: 10,
            columns: [
            { "data": "Code", "defaultContent": "<i>-</i>", "width": "10%" },
            {
                "data": "Type", render: function (data, type, row) {
                    debugger;
                    if (data == 'INC')
                        return 'Income'
                    else
                        return 'Expense';
                }, "defaultContent": "<i>-</i>", "width": "10%"
            },
            { "data": "TypeDesc", "defaultContent": "<i>-<i>", "width": "10%" },
            {
                "data": "IsSubHeadApplicable", render: function (data, type, row) {
                    debugger;
                    if (data == true)
                        return 'Yes'
                    else
                        return 'No';
                }, "defaultContent": "<i>-<i>", "width": "10%" },
            { "data": null, "orderable": false, "defaultContent": '<a href="#" onclick="EditChartOfAccountMaster(this)"<i class="glyphicon glyphicon-edit" aria-hidden="true"></i></a> <a href="#" onclick="DeleteChartOfAccountMaster(this)"<i class="glyphicon glyphicon-trash" aria-hidden="true"></i></a>', "width": "4%" }
            ],
            columnDefs: [{ "targets": [], "visible": false, "searchable": false },
                { className: "text-right", "targets": [] },
                { className: "text-left", "targets": [0,1, 2,3] },
                { className: "text-center", "targets": [4] }],
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
                    BindOrReloadChartOfAccountTable('Search');
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
function ResetChartOfAccountList() {
    BindOrReloadChartOfAccountTable('Reset');
}

//--function export data to excel--//
function ImportChartOfAccountData() {
    BindOrReloadChartOfAccountTable('Export');
}
//--edit ChartOfAccount--//
function EditChartOfAccountMaster(this_obj) {
    debugger;
     var ChartOfAccountViewModel = _dataTables.chartOfAccountList.row($(this_obj).parents('tr')).data();
    GetMasterPartial("ChartOfAccount", ChartOfAccountViewModel.Code);
    $('#h3ModelMasterContextLabel').text('Edit Chart Of Account')
    $('#divModelMasterPopUp').modal('show');
    $('#hdnMasterCall').val('MSTR');
}

//--Function To Confirm ChartOfAccount Deletion 
function DeleteChartOfAccountMaster(this_obj) {
    debugger;
    chartOfAccountVM = _dataTables.chartOfAccountList.row($(this_obj).parents('tr')).data();
    notyConfirm('Are you sure to delete?', 'DeleteChartOfAccount("' + chartOfAccountVM.Code + '")');
}

//--Function To Delete ChartOfAccount
function DeleteChartOfAccount(code) {
    debugger;
    try {
        if (code) {
            var data = { "code": code };
            var jsonData = {};
            var message = "";
            var status = "";
            var result = "";
            jsonData = GetDataFromServer("ChartOfAccount/DeleteChartOfAccount/", data);
            if (jsonData != '') {
                jsonData = JSON.parse(jsonData);
                message = jsonData.Message;
                status = jsonData.Status;
                result = jsonData.Record;
            }
            switch (status) {
                case "OK":
                    notyAlert('success', result.Message);
                    BindOrReloadChartOfAccountTable('Reset');
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
