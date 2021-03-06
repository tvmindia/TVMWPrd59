﻿var DataTables = {};
var EmptyGuid = "00000000-0000-0000-0000-000000000000";
$(document).ready(function () {
    debugger;
    try {
        $("#AdjustedBy").select2({
        });
        BindOrReloadStockAdjustmentTable('Init');
        $('#tblMaterialStockAdjustment tbody').on('dblclick', 'td', function () {
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
        var MaterialStockAjViewModel = DataTables.MaterialStockAdjustmentList.row($(curObj).parents('tr')).data();
        window.location.replace('NewStockAdjustment?code=STR&ID=' + MaterialStockAjViewModel.ID);
    }
    catch (e) {
        console.log(e.message);
    }
}

function BindOrReloadStockAdjustmentTable(action) {
    try {
        debugger;
        MaterialStockAdjAdvanceSearchViewModel = new Object();
        DataTablePagingViewModel = new Object();
        DataTablePagingViewModel.Length = 0;
        //switch case to check the operation
        switch (action) {
            case 'Reset':
                $('#SearchTerm').val('');
                $('#FromDate').val('');
                $('#ToDate').val('');
                $('#AdjustedBy').val('').select2();
                break;
            case 'Init':
                break;
            case 'Search':
                break;
            case 'Apply':
                MaterialStockAdjAdvanceSearchViewModel.FromDate = $('#FromDate').val();
                MaterialStockAdjAdvanceSearchViewModel.ToDate = $('#ToDate').val();
                MaterialStockAdjAdvanceSearchViewModel.AdjustedBy = $('#AdjustedBy').val();
                break;
            case 'Export':
                DataTablePagingViewModel.Length = -1;
                break;
            default:
                break;
        }
        MaterialStockAdjAdvanceSearchViewModel.DataTablePaging = DataTablePagingViewModel;
        MaterialStockAdjAdvanceSearchViewModel.SearchTerm = $('#SearchTerm').val();
        MaterialStockAdjAdvanceSearchViewModel.FromDate = $('#FromDate').val();
        MaterialStockAdjAdvanceSearchViewModel.ToDate = $('#ToDate').val();
        MaterialStockAdjAdvanceSearchViewModel.AdjustedBy = $('#AdjustedBy').val();
        DataTables.MaterialStockAdjustmentList = $('#tblMaterialStockAdjustment').DataTable(
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
                    url: "GetAllMaterialStockAdjustment/",
                    data: { "materialStockAdjAdvanceSearchVM": MaterialStockAdjAdvanceSearchViewModel },
                    type: 'POST'
                },
                pageLength: 10,
                columns: [
                    { "data": "ID", "defaultContent": "<i>-</i>" },
                    { "data": "AdjustmentNo", "defaultContent": "<i>-</i>" },
                    { "data": "AdjustmentDateFormatted", "defaultContent": "<i>-</i>" },
                    { "data": "AdjustedByEmployeeName", "defaultContent": "<i>-</i>" },
                    { "data": "Remarks", "defaultContent": "<i>-</i>" },
                    { "data": "ApprovalStatus", "defaultContent": "<i>-</i>" },
                    {
                        "data": "ID", "orderable": false, render: function (data, type, row) {
                            return '<a href="/MaterialStockAdj/NewStockAdjustment?code=STR&ID=' + data + '" class="actionLink" ><i class="glyphicon glyphicon-edit" aria-hidden="true"></i></a>'
                        }, "defaultContent": "<i>-</i>", "width": "3%"
                    }
                ],
                columnDefs: [{ "targets": [0], "visible": false, "searchable": false },
                { className: "text-center", "targets": [2] },
                {"targets":[4],"width":"30%"}],
                destroy: true,
                initComplete: function (settings, json) {
                    if (action === 'Export') {
                        if (json.data[0].length > 0) {
                            if (json.data[0].TotalCount > 10000) {
                                MasterAlert("info", 'We are able to download maximum 10000 rows of data, There exist more than 10000 rows of data please filter and download')
                            }
                        }
                        $(".buttons-excel").trigger('click');
                        BindOrReloadStockAdjustmentTable('Search');
                    }
                }
            });
        $(".buttons-excel").hide();
    }
    catch (e) {
        console.log(e.message);
    }
}

function ResetStockAdjustmentList() {
    try{
        BindOrReloadStockAdjustmentTable('Reset');
    }
    catch (e) {
        console.log(e.message);
    }
}
//function export data to excel
function ImportStockAdjustmentData() {
    try{
        BindOrReloadStockAdjustmentTable('Export');
    }
    catch (e) {
        console.log(e.message);
    }
}