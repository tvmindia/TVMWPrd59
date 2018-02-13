﻿var DataTables = {};
var EmptyGuid = "00000000-0000-0000-0000-000000000000";
//-------------------------------------------------------------
$(document).ready(function () {
    debugger;
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
            proccessing: true,
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
            { "data": "Code", "orderable": false, render: function(data,type,row){
                return '<a href="/Bank/AddBank?code=SETT&ID=' + data + '" class="actionLink" ><i class="glyphicon glyphicon-share-alt" aria-hidden="true"></i></a>'
            },"defaultContent": "<i>-</i>" }
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
function ImportBankData()
{
    BindOrReloadBankTable('Export');
}
//add bank 
function AddBankMaster()
{
    GetMasterPartial("Bank", "");
    $('#h3ModelMasterContextLabel').text('Add Master')
    $('#divModelMasterPopUp').modal('show');
}
 