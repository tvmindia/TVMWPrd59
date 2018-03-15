var DataTables = {};
var EmptyGuid = "00000000-0000-0000-0000-000000000000";
$(document).ready(function () {
    debugger;
    try {
        $("#EmployeeID").select2({
        });
        BindOrReloadFGStockAdjustmentTable('init');
    }
    catch (e) {
        console.log(e.message);
    }
});

function BindOrReloadFGStockAdjustmentTable(action) {
    try {
        debugger;
        FinishedGoodStockAdjAdvanceSearchViewModel=new Object();
        DataTablePagingViewModel = new Object();
        DataTablePagingViewModel.Length = 0;
        //switch case to check the operation
        switch (action) 
        {
            case 'Reset':
                $('#SearchTerm').val('');
                $('#FromDate').val('');
                $('#ToDate').val('');
                $('#EmployeeID').val('').select2();
                break;
            case 'init':
                break;
            case 'Search':
                break;
            case 'Apply':
                FinishedGoodStockAdjAdvanceSearchViewModel.FromDate = $('#FromDate').val();
                FinishedGoodStockAdjAdvanceSearchViewModel.ToDate = $('#ToDate').val();
                FinishedGoodStockAdjAdvanceSearchViewModel.AdjustedBy = $('#EmployeeID').val();
            case 'Export':
                DataTablePagingViewModel.Length = -1;
                break;
            default:
                break;
        }
        FinishedGoodStockAdjAdvanceSearchViewModel.DataTablePaging = DataTablePagingViewModel;
        FinishedGoodStockAdjAdvanceSearchViewModel.SearchTerm = $('#SearchTerm').val();
        FinishedGoodStockAdjAdvanceSearchViewModel.FromDate = $('#FromDate').val();
        FinishedGoodStockAdjAdvanceSearchViewModel.ToDate = $('#ToDate').val();
        FinishedGoodStockAdjAdvanceSearchViewModel.AdjustedBy = $('#EmployeeID').val();
        DataTables.FinishedGoodStockAdjustmentList = $('#tblFinishedGoodStockAdjustment').DataTable(
            {
                dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
                buttons: [{
                    extend: 'excel',
                    exportOptions:
                                 {
                                     columns: [1, 2, 3, 4, 5]
                                 }
                }],
                order: false,
                ordering: false,
                searching: false,
                paging: true,
                lengthChange: false,
                proccessing: true,
                serverSide: true,
                ajax: {
                    url: "GetAllFinishedGoodStockAdj/",
                    data: { "finishedGoodStockAdjAdvanceSearchVM": FinishedGoodStockAdjAdvanceSearchViewModel },
                    type: 'POST'
                },
                pageLength: 10,
                columns: [
                    { "data": "ID", "defaultContent": "<i>-</i>" },
                    { "data": "ReferenceNo", "defaultContent": "<i>-</i>" },
                    { "data": "AdjustmentDateFormatted", "defaultContent": "<i>-</i>" },
                    { "data": "AdjustedByEmployeeName", "defaultContent": "<i>-</i>" },
                    { "data": "Remarks", "defaultContent": "<i>-</i>" },
                    { "data": "ApprovalStatus", "defaultContent": "<i>-</i>" },
                    {
                        "data": "ID", "orderable": false, render: function (data, type, row) {
                            return '<a href="/FinishedGoodStockAdj/NewFinishedGoodStockAdj?code=STR&ID=' + data + '" class="actionLink" ><i class="glyphicon glyphicon-share-alt" aria-hidden="true"></i></a>'
                        }, "defaultContent": "<i>-</i>", "width": "3%"
                    }
                ],
                columnDefs: [{ "targets": [0], "visible": false, "searchable": false },
                { className: "text-center", "targets": [2] },
                { "targets": [4], "width": "30%" }],
                destroy: true,
                initComplete: function (settings, json) {
                    if (action === 'Export') {
                        if (json.data[0].length > 0) {
                            if (json.data[0].TotalCount > 10000) {
                                MasterAlert("info", 'We are able to download maximum 10000 rows of data, There exist more than 10000 rows of data please filter and download')
                            }
                        }
                        $(".buttons-excel").trigger('click');
                        BindOrReloadFGStockAdjustmentTable('Search');
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
function ResetFGStockAdjustmentList() {
    BindOrReloadFGStockAdjustmentTable('Reset');
}
//function export data to excel
function ImportFGStockAdjustmentData() {
    BindOrReloadFGStockAdjustmentTable('Export');
}