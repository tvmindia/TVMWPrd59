//*****************************************************************************
//*****************************************************************************
//Author: Angel
//CreatedDate: 26-Mar-2018 
//LastModified: 26-Mar-2018 
//FileName: ViewCustomerPayment.js
//Description: Client side coding for view CustomerPayment.
//******************************************************************************
//******************************************************************************

//--Global Declaration--//
var DataTables = {};
var EmptyGuid = "00000000-0000-0000-0000-000000000000";
$(document).ready(function () {
    debugger;
    try {
        $("#PaymentMode").select2({
        });
        $("#CustomerID").select2({
        });
        BindOrReloadCustomerPaymentTable('Init');
        $('#tblCustomerPayment tbody').on('dblclick', 'td', function () {
            Edit(this);
        });
    }
    catch (e) {
        console.log(e.message);
    }
});
//edit on table click
function Edit(curObj) {
    debugger;
    var rowData = DataTables.CustomerPayment.row($(curObj).parents('tr')).data();
    window.location.replace("NewCustomerPayment?code=ACC&ID=" + rowData.ID);

}
function BindOrReloadCustomerPaymentTable(action)
{
    try {
        debugger;
        customerPaymentAdvanceSearchVM = new Object();
        DataTablePagingViewModel = new Object();
        DataTablePagingViewModel.Length = 0;
        //switch case to check the operation
        switch (action) {
            case 'Reset':
                $('#SearchTerm').val('');
                $('#FromDate').val('');
                $('#ToDate').val('');
                $('#CustomerID').val('').select2();
                $('#PaymentMode').val('').select2();
                break;
            case 'Init':
                break;
            case 'Search':
                break;
            case 'Apply':
                customerPaymentAdvanceSearchVM.FromDate = $('#FromDate').val();
                customerPaymentAdvanceSearchVM.ToDate = $('#ToDate').val();
                customerPaymentAdvanceSearchVM.CustomerID = $('#CustomerID').val();
                customerPaymentAdvanceSearchVM.PaymentMode = $('#PaymentMode').val();
                break;
            case 'Export':
                DataTablePagingViewModel.Length = -1;
                break;
            default:
                break;
        }
        customerPaymentAdvanceSearchVM.DataTablePaging = DataTablePagingViewModel;
        customerPaymentAdvanceSearchVM.SearchTerm = $('#SearchTerm').val();
        customerPaymentAdvanceSearchVM.FromDate = $('#FromDate').val();
        customerPaymentAdvanceSearchVM.ToDate = $('#ToDate').val();
        customerPaymentAdvanceSearchVM.CustomerID = $('#CustomerID').val();
        customerPaymentAdvanceSearchVM.PaymentMode = $('#PaymentMode').val();
        DataTables.CustomerPayment = $('#tblCustomerPayment').DataTable(
            {
                dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
                buttons: [{
                    extend: 'excel',
                    exportOptions:
                                 {
                                     columns: [1, 2, 3, 4, 5, 6, 7, 8, 9]
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
                    url: "GetAllCustomerPayment/",
                    data: { "customerPaymentAdvanceSearchVM": customerPaymentAdvanceSearchVM },
                    type: 'POST'
                },
                pageLength: 10,
                columns: [
                    { "data": "ID", "defaultContent": "<i>-</i>" },
                    { "data": "EntryNo", "defaultContent": "<i>-</i>", "width": "10%" },
                    { "data": "PaymentDateFormatted", "defaultContent": "<i>-</i>", "width": "10%" },
                    { "data": "PaymentRef", "defaultContent": "<i>-</i>", "width": "15%" },
                    { "data": "CustomerName", "defaultContent": "<i>-</i>", "width": "15%" },
                    { "data": "PaymentMode", "defaultContent": "<i>-</i>", "width": "10%" },
                    { "data": "Type", "defaultContent": "<i>-</i>", "width": "7%" },
                    { "data": "CreditNo", "defaultContent": "<i>-</i>", "width": "10%" },
                    {
                        "data": "TotalRecievedAmt", "defaultContent": "<i>-</i>",
                        'render': function (data, type, row) {
                            return roundoff(data, 1);
                        }, "width": "11%"
                    },
                    {
                        "data": "AdvanceAmount", "defaultContent": "<i>-</i>", 'render': function (data, type, row) {
                            if (data == 0)
                                return '0.00'
                            else
                                return roundoff(data, 1);
                        }, "width": "12%"
                    },
                    {
                        "data": "ID", "orderable": false, render: function (data, type, row) {
                            return '<a href="/CustomerPayment/NewCustomerPayment?code=ACC&ID=' + data + '" class="actionLink" ><i class="glyphicon glyphicon-edit" aria-hidden="true"></i></a>'
                        }, "defaultContent": "<i>-</i>", "width": "3%"
                    }
                ],
                columnDefs: [{ "targets": [0], "visible": false, "searchable": false },
                            { className: "text-left", "targets": [1, 3, 4,5,6,7] },
                            { className: "text-right", "targets": [8,9] },
                            { className: "text-center", "targets": [2,10] }],
                destroy: true,
                //for performing the import operation after the data loaded
                initComplete: function (settings, json) {
                    if (action === 'Export') {
                        debugger;
                        if (json.data[0].length > 0) {
                            if (json.data[0].TotalCount > 10000) {
                                MasterAlert("info", 'We are able to download maximum 10000 rows of data, There exist more than 10000 rows of data please filter and download')
                            }
                        }
                        $(".buttons-excel").trigger('click');
                        BindOrReloadCustomerPaymentTable('Search');
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
function ResettblCustomerPayment() {
    BindOrReloadCustomerPaymentTable('Reset');
}
//function export data to excel
function ImportCustomerPaymentData() {
    BindOrReloadCustomerPaymentTable('Export');
}