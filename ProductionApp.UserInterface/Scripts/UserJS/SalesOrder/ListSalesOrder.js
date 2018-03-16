//*****************************************************************************
//*****************************************************************************
//Author: Gibin Jacob
//CreatedDate: 07-Mar-2018 
//LastModified: 
//FileName: ListSalesOrderr.js
//Description: Client side coding for Display list and filter Sales order
//******************************************************************************
//******************************************************************************
var DataTables = {};
var EmptyGuid = "00000000-0000-0000-0000-000000000000";

$(document).ready(function () {
    debugger;
    try {
        
        BindOrReloadSalesOrderTable('Init');
        BindOrReloadSalesOrderDetailTable('Init');
        //$('#tblSalesOrderSummaryView tbody').on('dblclick', 'td', function () {
        //    Edit(this);
        //});
    }
    catch (e) {
        console.log(e.message);
    }
});

function Edit(curObj) {
    debugger;
    var rowData = DataTables.SalesOrderList.row($(curObj).parents('tr')).data();
    window.location.replace("AddSalesOrder?code=SALE&ID=" + rowData.ID);

}
//bind sales order list
function BindOrReloadSalesOrderTable(action) {
    try {
        //creating advancesearch object
        debugger;
        SalesOrderAdvanceSearchViewModel = new Object();
        DataTablePagingViewModel = new Object();
        DataTablePagingViewModel.Length = 0;
        //switch case to check the operation
        switch (action) {
            case 'Reset':
                $('#SearchTerm').val('');
                $('#FromDate').val('');
                $('#ToDate').val('');

                break;
            case 'Init':
                break;
            case 'Search':
                break;
            case 'Apply':
                SalesOrderAdvanceSearchViewModel.FromDate = $('#FromDate').val();
                SalesOrderAdvanceSearchViewModel.ToDate = $('#ToDate').val();
                break;
            case 'Export':
                DataTablePagingViewModel.Length = -1;
                break;
            default:
                break;
        }
        SalesOrderAdvanceSearchViewModel.DataTablePaging = DataTablePagingViewModel;
        SalesOrderAdvanceSearchViewModel.SearchTerm = $('#SearchTerm').val();
        DataTables.SalesOrderSummaryView = $('#tblSalesOrderSummaryView').DataTable(
            {
                dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
                buttons: [{
                    extend: 'excel',
                    exportOptions:
                                 {
                                     columns: [1, 2, 3, 4, 5, 6]
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
                    url: "GetAllSalesOrder/",
                    data: { "salesOrderAdvanceSearchVM": SalesOrderAdvanceSearchViewModel },
                    type: 'POST'
                },
                pageLength: 10,
                columns: [
                    { "data": "ID", "defaultContent": "<i>-</i>" },
                    { "data": "CustomerName", "defaultContent": "<i>-</i>" },
                    { "data": "OrderNo", "defaultContent": "<i>-</i>" },
                    { "data": "OrderDateFormatted", "defaultContent": "<i>-</i>" },
                    { "data": "", "defaultContent": "<i>-</i>" },
                    { "data": "", "defaultContent": "<i>-</i>" },
                    { "data": "ExpectedDeliveryDateFormatted", "defaultContent": "<i>-</i>" },
                    //{ "data": "", "defaultContent": "<i>-</i>" },
                    {
                         "data": "ID", "orderable": false, render: function (data, type, row) {
                             return '<a href="/SalesOrder/AddSalesOrder?code=SALE&ID=' + data + '" class="actionLink" ><i class="glyphicon glyphicon-share-alt" aria-hidden="true"></i></a>'
                         }, "defaultContent": "<i>-</i>", "width": "3%"
                    }
                ],
                columnDefs: [{ "targets": [0], "visible": false, "searchable": false },
                    { className: "text-left", "targets": [1, 2, 4, 5, 6] },
                    { className: "text-center", "targets": [3, 7] }],
                destroy: true,
                //for performing the import operation after the data loaded
                initComplete: function (settings, json) {
                    if (action === 'Export') {
                        $(".buttons-excel").trigger('click');
                        ResetSalesOrderList();
                    }
                }
            });
        $(".buttons-excel").hide();
    }
    catch (e) {
        console.log(e.message);
    }
}



function BindOrReloadSalesOrderDetailTable(action) {
    try {
        //creating advancesearch object
        debugger;
        SalesOrderAdvanceSearchViewModel = new Object();
        DataTablePagingViewModel = new Object();
        DataTablePagingViewModel.Length = 0;
        //switch case to check the operation
        switch (action) {
            case 'Reset':
                $('#SearchTerm').val('');
                $('#FromDate').val('');
                $('#ToDate').val('');

                break;
            case 'Init':
                break;
            case 'Search':
                break;
            case 'Apply':
                SalesOrderAdvanceSearchViewModel.FromDate = $('#FromDate').val();
                SalesOrderAdvanceSearchViewModel.ToDate = $('#ToDate').val();
                break;
            case 'Export':
                DataTablePagingViewModel.Length = -1;
                break;
            default:
                break;
        }
        SalesOrderAdvanceSearchViewModel.DataTablePaging = DataTablePagingViewModel;
        SalesOrderAdvanceSearchViewModel.SearchTerm = $('#SearchTerm').val();
        DataTables.SalesOrderDetailView = $('#tblSalesOrderDetailView').DataTable(
            {
                dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
                buttons: [{
                    extend: 'excel',
                    exportOptions:
                                 {
                                     columns: [1, 2, 3, 4, 5, 6]
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
                    url: "GetAllSalesOrderDetail/",
                    data: { "salesOrderAdvanceSearchVM": SalesOrderAdvanceSearchViewModel },
                    type: 'POST'
                },
                pageLength: 10,
                columns: [
                    { "data": "ID", "defaultContent": "<i>-</i>" },
                    { "data": "CustomerName", "defaultContent": "<i>-</i>" },
                    { "data": "OrderNo", "defaultContent": "<i>-</i>" },
                    { "data": "", "defaultContent": "<i>-</i>" },
                    { "data": "", "defaultContent": "<i>-</i>" },
                    { "data": "", "defaultContent": "<i>-</i>" },
                    { "data": "", "defaultContent": "<i>-</i>" },
                    { "data": "", "defaultContent": "<i>-</i>" },
  { "data": "", "defaultContent": "<i>-</i>" },
                    { "data": "", "defaultContent": "<i>-</i>" },
                    {
                        "data": "ID", "orderable": false, render: function (data, type, row) {
                            return '<a href="/SalesOrder/AddSalesOrder?code=SALE&ID=' + data + '" class="actionLink" ><i class="glyphicon glyphicon-share-alt" aria-hidden="true"></i></a>'
                        }, "defaultContent": "<i>-</i>", "width": "3%"
                    }
                ],
                columnDefs: [{ "targets": [0], "visible": false, "searchable": false },
                    { className: "text-left", "targets": [1, 2, 4, 5, 6] },
                    { className: "text-center", "targets": [3, 7] }],
                destroy: true,
                //for performing the import operation after the data loaded
                initComplete: function (settings, json) {
                    if (action === 'Export') {
                        $(".buttons-excel").trigger('click');
                        ResetSalesOrderList();
                    }
                }
            });
        $(".buttons-excel").hide();
    }
    catch (e) {
        console.log(e.message);
    }
}



//function reset the list to initial
function ResetSalesOrderList() {
    BindOrReloadSalesOrderTable('Reset');
}
//function export data to excel
function ImportSalesOrderData() {
    BindOrReloadSalesOrderTable('Export');
}

function ShowHideDataTables()
{
    debugger;
    if ($('#divtogglevalue').val() == 0)
    {
        $('#divDetailTable').show();
        $('#divSummaryTable').hide();
        $('#divtogglevalue').val(1);
        $('#btnShowHide').html('<i class="glyphicon glyphicon-eye-open"></i> Show Summary');
    }
    else
    {
        $('#divSummaryTable').show();
        $('#divDetailTable').hide();
        $('#divtogglevalue').val(0);
        $('#btnShowHide').html('<i class="glyphicon glyphicon-eye-open"></i> Show Detail');
    }
}