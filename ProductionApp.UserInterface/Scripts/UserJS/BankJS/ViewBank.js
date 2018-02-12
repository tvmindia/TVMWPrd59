var DataTables = {};
var EmptyGuid = "00000000-0000-0000-0000-000000000000";
//-------------------------------------------------------------
$(document).ready(function () {
    debugger;
    BindOrReloadBankTable('Init');
});
//function bind the bank list checking search and filter
function BindOrReloadBankTable(action) {
    try
    {
        //switch case to check the operation
        switch (action) {
            case 'Reset':
                $('SearchTerm').val('');
                break;
            case 'Init':
                break;
            case 'Search':
                break;
            default:
                break;
        }
        //creating advancesearch object
        BankAdvanceSearchViewModel = new Object();
        BankAdvanceSearchViewModel.SearchTerm = $('#SearchTerm').val();
        //apply datatable plugin on bank table
        DataTables.BankList = $('#tblBank').DataTable(
        {
            dom: '<"pull-right"f>rt<"bottom"ip><"clear">',
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
