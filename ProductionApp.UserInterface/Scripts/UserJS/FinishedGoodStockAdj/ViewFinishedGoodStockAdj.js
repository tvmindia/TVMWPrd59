var DataTables = {};
var EmptyGuid = "00000000-0000-0000-0000-000000000000";
$(document).ready(function () {
    debugger;
    try {
        $("#AdjustedBy").select2({
        });
        BindOrReloadFGStockAdjustmentTable('init');
        $('#tblFinishedGoodStockAdjustment tbody').on('dblclick', 'td', function () {
            Edit(this);
        });
    }
    catch (e) {
        console.log(e.message);
    }
});

function Edit(curObj)
{
    try{
        debugger;
        var FinishedGoodStockAdjViewModel = DataTables.FinishedGoodStockAdjustmentList.row($(curObj).parents('tr')).data();
        window.location.replace('NewFinishedGoodStockAdj?code=STR&ID=' + FinishedGoodStockAdjViewModel.ID);
    }
    catch (e) {
        console.log(e.message);
    }
}

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
                $('#AdjustedBy').val('').select2();
                break;
            case 'init':
                break;
            case 'Search':
                break;
            case 'Apply':
                FinishedGoodStockAdjAdvanceSearchViewModel.FromDate = $('#FromDate').val();
                FinishedGoodStockAdjAdvanceSearchViewModel.ToDate = $('#ToDate').val();
                FinishedGoodStockAdjAdvanceSearchViewModel.AdjustedBy = $('#AdjustedBy').val();
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
        FinishedGoodStockAdjAdvanceSearchViewModel.AdjustedBy = $('#AdjustedBy').val();
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
                    { "data": "AdjustmentNo", "defaultContent": "<i>-</i>" },
                    { "data": "AdjustmentDateFormatted", "defaultContent": "<i>-</i>" },
                    { "data": "AdjustedByEmployeeName", "defaultContent": "<i>-</i>" },
                    { "data": "Remarks", "defaultContent": "<i>-</i>","width":"50%" },
                    { "data": "ApprovalStatus", "defaultContent": "<i>-</i>" },
                    {
                        "data": "ID", "orderable": false, render: function (data, type, row) {
                            return '<a href="/FinishedGoodStockAdj/NewFinishedGoodStockAdj?code=STR&ID=' + data + '" class="actionLink" ><i class="glyphicon glyphicon-edit" aria-hidden="true"></i></a>'
                        }, "defaultContent": "<i>-</i>", "width": "3%"
                    }
                ],
                columnDefs: [{ "targets": [0], "visible": false, "searchable": false },
              { className: "text-center", "targets": [2] }],           
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
    try{
        debugger;
        BindOrReloadFGStockAdjustmentTable('Reset');
    }
    catch (e) {
        console.log(e.message);
    }
}
//function export data to excel
function ImportFGStockAdjustmentData() {
    try{
        debugger
        BindOrReloadFGStockAdjustmentTable('Export');
    }
    catch (e) {
        console.log(e.message);
    }
}