var DataTables = {};
var EmptyGuid = "00000000-0000-0000-0000-000000000000";
$(document).ready(function () {
    debugger;
    try {
        $("#IssueTo").select2({
        });        
        $("#IssuedBy").select2({
        });
        BindOrReloadIssueToProductionTable('Init');
    }
    catch (e) {
        console.log(e.message);
    }
});

function BindOrReloadIssueToProductionTable(action)
{
    try
    {
        debugger;
        MaterialIssueAdvanceSearchViewModel=new Object();
        DataTablePagingViewModel = new Object();
        DataTablePagingViewModel.Length = 0;
        //switch case to check the operation
        switch (action) {
            case 'Reset':
                $('#SearchTerm').val('');
                $('#FromDate').val('');
                $('#ToDate').val('');
                $('#IssueTo').val('').select2();
                $('#IssuedBy').val('').select2();
                break;
            case 'Init':
                break;
            case 'Search':
                break;
            case 'Apply':
                MaterialIssueAdvanceSearchViewModel.FromDate=$('#FromDate').val();
                MaterialIssueAdvanceSearchViewModel.ToDate=$('#ToDate').val();
                MaterialIssueAdvanceSearchViewModel.IssueTo=$('#IssueTo').val();
                MaterialIssueAdvanceSearchViewModel.IssuedBy=$('#IssuedBy').val();
                break;
            case 'Export':
                DataTablePagingViewModel.Length = -1;
                break;
            default:    
                break;
        }
                MaterialIssueAdvanceSearchViewModel.DataTablePaging=DataTablePagingViewModel;
                MaterialIssueAdvanceSearchViewModel.SearchTerm=$('#SearchTerm').val();
                DataTables.IssueToProductionList=$('#tblIssueToProduction').DataTable(
                    {
                        dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
                        buttons: [{
                            extend: 'excel',
                            exportOptions:
                                         {
                                             columns: [1,2,3,4]
                                         }
                        }],
                        order: false,
                        searching: false,
                        paging: true,
                        lengthChange: false,
                        proccessing: true,
                        serverSide: true,
                        ajax: {
                            url: "GetAllIssueToProduction/",
                            data: { "materialIssueAdvanceSearchVM": MaterialIssueAdvanceSearchViewModel },
                            type: 'POST'
                        },
                        pageLength: 10,
                        columns: [                            
                            { "data": "ID", "defaultContent": "<i>-</i>" },
                            { "data": "IssueNo", "defaultContent": "<i>-</i>" },                            
                            { "data": "IssueDateFormatted", "defaultContent": "<i>-</i>" },
                            { "data": "IssueToEmployeeName", "defaultContent": "<i>-</i>" },
                            { "data": "IssuedByEmployeeName", "defaultContent": "<i>-</i>" },
                             {
                                 "data": "ID", "orderable": false, render: function (data, type, row) {
                                     return '<a href="/IssueToProduction/AddIssueToProduction?code=STR&ID=' + data + '" class="actionLink" ><i class="glyphicon glyphicon-share-alt" aria-hidden="true"></i></a>'
                                 }, "defaultContent": "<i>-</i>"
                             }
                        ],
                        columnDefs: [{ "targets": [0], "visible": false, "searchable": false },
                    { className: "text-left", "targets": [1,2,3,4]}],                    
                        destroy: true,
                        //for performing the import operation after the data loaded
                        initComplete: function (settings, json) {
                            if (action === 'Export') {
                                $(".buttons-excel").trigger('click');
                                ResetIssueToProductionList();
                            }
                        }
                    });
                $(".buttons-excel").hide();
    }
        
     catch (e) {
            console.log(e.message);
        }
}


function ResetIssueToProductionList() {
    BindOrReloadIssueToProductionTable('Reset');
}
//function export data to excel
function ImportIssueToProductionData() {
    BindOrReloadIssueToProductionTable('Export');
}