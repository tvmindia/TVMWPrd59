var DataTables = {};
var EmptyGuid = "00000000-0000-0000-0000-000000000000";
//-------------------------------------------------------------
$(document).ready(function () {
    debugger;
    BindOrReloadBankTable('Init');
    $(".buttons-excel").hide();
});
//function bind the bank list checking search and filter
function BindOrReloadBankTable(action) {
    try
    {
        //creating advancesearch object
        BankAdvanceSearchViewModel = new Object();
        //switch case to check the operation
        switch (action) {
            case 'Reset':
                $('#SearchTerm').val('');
                break;
            case 'Init':
                break;
            case 'Search':
                break;
            case 'Import':
                BankAdvanceSearchViewModel.Length = -1;
                break;
            default:
                break;
        }
        
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
                url: "GetAllBank/",
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
            { "data": null, "orderable": false, "defaultContent": '<a href="#" class="actionLink"  onclick="Edit(this)" ><i class="glyphicon glyphicon-share-alt" aria-hidden="true"></i></a>' }
            ],
            columnDefs: [{ "targets": [], "visible": false, "searchable": false },
                { className: "text-right", "targets": [3, 4] },
                { className: "text-left", "targets": [1, 2, 3] },
                { className: "text-center", "targets": [0] }],           
            destroy: true,
            //for performing the import operation after the data loaded
            initComplete: function (settings, json) {
                if (action === 'Import')
                {
                    $(".buttons-excel").trigger('click');
                    ResetBankList();
                }
            }
        });
    }
    catch(e)
    {
        
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
    BindOrReloadBankTable('Import');   
}
