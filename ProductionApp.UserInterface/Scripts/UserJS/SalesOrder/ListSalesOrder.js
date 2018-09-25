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
        
        $("#EmployeeID").select2({
        });
        $("#CustomerID").select2({
        });

        BindOrReloadSalesOrderTable('Init');
        BindOrReloadSalesOrderDetailTable('Init');
        $('#tblSalesOrderSummaryView tbody').on('dblclick', 'td', function () {
            Edit(this,1);
        });
        $('#tblSalesOrderDetailView tbody').on('dblclick', 'td', function () {
            Edit(this, 0);
        });
    }
    catch (e) {
        console.log(e.message);
    }
});

function Edit(curObj,flag) {
    debugger;
    if(flag)
        var rowData = DataTables.SalesOrderSummaryView.row($(curObj).parents('tr')).data();
    else
        var rowData = DataTables.SalesOrderDetailView.row($(curObj).parents('tr')).data();

    window.location.replace("AddSalesOrder?code=SALE&ID=" + rowData.ID);

}

//Advance Search Apply Button Click
function BindOrReloadTables(action)
{
    BindOrReloadSalesOrderTable(action);
    BindOrReloadSalesOrderDetailTable(action)
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
                $('#CustomerID').val('').select2();
                $('#EmployeeID').val('').select2();
                break;
            case 'Init':
                break;
            case 'Search': 
                SalesOrderAdvanceSearchViewModel.FromDate = $('#FromDate').val();
                SalesOrderAdvanceSearchViewModel.ToDate = $('#ToDate').val();
                SalesOrderAdvanceSearchViewModel.CustomerID = $('#CustomerID').val();
                SalesOrderAdvanceSearchViewModel.EmployeeID = $('#EmployeeID').val();
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
                    extend: 'excel', className: 'excelButtonSummary',
                    exportOptions:
                                 {
                                     columns: [0,1, 2, 3, 4, 5,6]
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
                    { "data": "CustomerName", "defaultContent": "<i>-</i>", "width": "25%" },
                    { "data": "OrderNo", "defaultContent": "<i>-</i>", "width": "8%" },
                    { "data": "OrderDateFormatted", "defaultContent": "<i>-</i>", "width": "10%" },
                    { "data": "SalesPersonName", "defaultContent": "<i>-</i>", "width": "10%" },
                    {
                        "data": "OrderAmount", render: function (data, type, row) {
                            return roundoff(data);
                        }, "defaultContent": "<i>-</i>", "width": "14%"
                    },
                    { "data": "OrderStatus", "defaultContent": "<i>-</i>", "width": "20%" },
                    { "data": "ExpectedDeliveryDateFormatted", "defaultContent": "<i>-</i>", "width": "10%" }, 
                    {
                         "data": "ID", "orderable": false, render: function (data, type, row) {
                             return '<a href="/SalesOrder/AddSalesOrder?code=SALE&ID=' + data + '" class="actionLink" ><i class="glyphicon glyphicon-edit" aria-hidden="true"></i></a>'
                         }, "defaultContent": "<i>-</i>", "width": "3%"
                    }
                ],
                columnDefs: [{ "targets": [ ], "visible": false, "searchable": false },
                    { className: "text-left", "targets": [0,1,3,5] },
                    { className: "text-right", "targets": [4] },
                    { className: "text-center", "targets": [2, 6,7] }],
                destroy: true,
                //for performing the import operation after the data loaded
                initComplete: function (settings, json) {
                    if (action === 'Export') {
                        $(".excelButtonSummary").trigger('click');
                    }
                }
            });
         $(".excelButtonSummary").hide();
    }
    catch (e) {
        console.log(e.message);
    }
}

//Bind Sales Order Detail List
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
                $('#CustomerID').val('').select2();
                $('#EmployeeID').val('').select2();

                break;
            case 'Init':
                break;
            case 'Search':
                SalesOrderAdvanceSearchViewModel.FromDate = $('#FromDate').val();
                SalesOrderAdvanceSearchViewModel.ToDate = $('#ToDate').val();
                SalesOrderAdvanceSearchViewModel.CustomerID = $('#CustomerID').val();
                SalesOrderAdvanceSearchViewModel.EmployeeID = $('#EmployeeID').val();
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
                    extend: 'excel', className: 'excelButtonDetail',
                    exportOptions:
                                 {
                                     columns: [0,1, 2, 3, 4, 5, 6,7,8]
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
                    { "data": "CustomerName", "defaultContent": "<i>-</i>" ,"width": "25%" },
                    { "data": "OrderNo", "defaultContent": "<i>-</i>", "width": "8%" },
                    { "data": "OrderDateFormatted", "defaultContent": "<i>-</i>", "width": "10%" },
                    { "data": "SalesOrderDetail.Product.Name", render: function (data, type, row) {

                        row.SalesOrderDetail.Product.HSNNo = row.SalesOrderDetail.Product.HSNNo == null ? "Nill" : row.SalesOrderDetail.Product.HSNNo
                        if (row.SalesOrderDetail.GroupName == null || row.SalesOrderDetail.GroupName == "")
                            return data + '</br><b>HSNNo: </b>' + row.SalesOrderDetail.Product.HSNNo
                        else
                            return '<b>' + row.SalesOrderDetail.GroupName + '</b></br>' + data + '</br><b>HSNNo: </b>' + row.SalesOrderDetail.Product.HSNNo
                    }
                        , "defaultContent": "<i>-</i>", "width": "10%"
                    },
                    { "data": "SalesOrderDetail.Quantity", "defaultContent": "<i>-</i>", "width": "7%" },
                    { "data": "DispatchedQty", "defaultContent": "<i>-</i>", "width": "7%" },
                    { "data": "DispatchedDates", "defaultContent": "<i>-</i>", "width": "10%" },
                    { "data": "OrderStatus", "defaultContent": "<i>-</i>", "width": "10%" },
                    { "data": "SalesOrderDetail.ExpectedDeliveryDateFormatted", "defaultContent": "<i>-</i>", "width": "10%" },
                    {
                        "data": "ID", "orderable": false, render: function (data, type, row) {
                            return '<a href="/SalesOrder/AddSalesOrder?code=SALE&ID=' + data + '" class="actionLink" ><i class="glyphicon glyphicon-edit" aria-hidden="true"></i></a>'
                        }, "defaultContent": "<i>-</i>", "width": "3%"
                    }
                ],
                columnDefs: [{ "targets": [], "visible": false, "searchable": false },
                    { className: "text-left", "targets": [0,1,3,6,7] },
                    { className: "text-center", "targets": [2,9,8] },
                    { className: "text-right", "targets": [4,5,] }],
                destroy: true,
                //for performing the import operation after the data loaded
                initComplete: function (settings, json) {
                    if (action === 'Export') {
                        $(".excelButtonDetail").trigger('click');
                        ResetSalesOrderList('Search'); 
                    }
                }
            });
          $(".excelButtonDetail").hide();
    }
    catch (e) {
        console.log(e.message);
    }
}



//function reset the list to initial
function ResetSalesOrderList(action) {
    BindOrReloadSalesOrderTable(action);
    BindOrReloadSalesOrderDetailTable(action);
}
//function export data to excel
function ImportSalesOrderData() {
    BindOrReloadSalesOrderTable('Export');
    BindOrReloadSalesOrderDetailTable('Export');
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