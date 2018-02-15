//*****************************************************************************
//*****************************************************************************
//Author: Thomson
//CreatedDate: 12-Feb-2018 
//LastModified: 14-Feb-2018 
//FileName: Bank.js
//Description: Client side coding for Bank
//******************************************************************************
//******************************************************************************

//--Global Declaration--//
var DataTables = {};
var EmptyGuid = "00000000-0000-0000-0000-000000000000";
//-------------------------------------------------------------
$(document).ready(function () {
    try
    {
        BindOrReloadBankTable('Init');        
    }
    catch(e)
    {
        console.log(e.message);
    }    
});
//function bind the bank list checking search and filter
function BindOrReloadBankTable(action) {
    try
    {
        //creating advancesearch object
        BankAdvanceSearchViewModel = new Object();
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
                DataTablePagingViewModel.Length = -1;
                break;
            default:
                break;
        }
        BankAdvanceSearchViewModel.DataTablePaging = DataTablePagingViewModel;
        BankAdvanceSearchViewModel.SearchTerm = $('#SearchTerm').val();
        
        //apply datatable plugin on bank table
        DataTables.BankList = $('#tblBank').DataTable(
        {
            dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
            buttons: [{
                extend: 'excel',
                exportOptions:
                             {
                                 columns: [0, 1]
                             }
            }],
            order:false,
            searching: false,
            paging: true,
            lengthChange: false,
            processing: true,
            serverSide: true,
            ajax: {
                url: "Bank/GetAllBank/",
                data: { "bankAdvanceSearchVM": BankAdvanceSearchViewModel },
                type: 'POST'
            },
            pageLength: 10,
            columns: [
            { "data": "Code", "defaultContent": "<i>-</i>" },
            { "data": "Name", "defaultContent": "<i>-</i>" },
            {
                "data": "ActualODLimit", render: function (data, type, row) {
                    if (data == 0)
                        return '-'
                    else
                        return roundoff(data, 1);
                },
                "defaultContent": "<i>-</i>"
            },
            {
                "data": "DisplayODLimit", render: function (data, type, row) {
                    if (data == 0)
                        return '-'
                    else
                        return roundoff(data, 1);
                }, "defaultContent": "<i>-</i>"
            },
            { "data": null, "orderable": false, "defaultContent": '<a href="#" onclick="EditBankMaster(this)"<i class="glyphicon glyphicon-share-alt" aria-hidden="true"></i></a>' }
            ],
            columnDefs: [{ "targets": [], "visible": false, "searchable": false },
                { className: "text-right", "targets": [3, 4] },
                { className: "text-left", "targets": [1, 2, 3] },
                { className: "text-center", "targets": [0] }],           
            destroy: true,
            //for performing the import operation after the data loaded
            initComplete: function (settings, json) {
                if (action === 'Export')
                {
                    $(".buttons-excel").trigger('click');
                    ResetBankList();
                }
            }
        });
        $(".buttons-excel").hide();
    }
    catch(e)
    {
        console.log(e.message);
    }    
}
//function reset the list to initial
function ResetBankList()
{
    BindOrReloadBankTable('Reset');
}
//function export data to excel
function ExportBankData()
{
    BindOrReloadBankTable('Export');
}
//add bank 
function AddBankMaster()
{
    GetMasterPartial("Bank", "");
    $('#h3ModelMasterContextLabel').text('Add Bank')
    $('#divModelMasterPopUp').modal('show');
}
//edit bank 
function EditBankMaster(this_obj) {
    debugger;
    rowData = DataTables.BankList.row($(this_obj).parents('tr')).data();
    GetMasterPartial("Bank", rowData.Code);
    $('#h3ModelMasterContextLabel').text('Edit Bank')
    $('#divModelMasterPopUp').modal('show');
}
//onsuccess function for formsubmitt
function SaveSuccessBank(data, status)
{
    debugger;
    var JsonResult = JSON.parse(data)
    switch (JsonResult.Result) {
        case "OK":
            $('#IsUpdate').val('True');
            BindOrReloadBankTable('Reset');
            MasterAlert("success",JsonResult.Records.Message)
            break;
        case "ERROR":
            MasterAlert("danger", JsonResult.Message)
            break;
        default:
            MasterAlert("danger", JsonResult.Message)            
            break;
    }
}