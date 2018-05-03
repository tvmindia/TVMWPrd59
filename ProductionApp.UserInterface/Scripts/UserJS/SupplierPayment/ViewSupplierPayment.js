//*****************************************************************************
//*****************************************************************************
//Author: Gibin Jacob
//CreatedDate: 18-Apr-2018 
//LastModified:  18-Apr-2018 
//FileName: NewSupplierPayment.js
//Description: Client side coding for New/Edit Supplier Payment
//******************************************************************************
// ##1-- Global Declaration
// ##2-- Document Ready function
// ##3-- Edit Click Redirection
// ##4-- Bind Supplier Payment Table List
// ##5-- 
// ##6-- 
// ##7-- 
// ##8-- 
// ##9-- 
// ##10-- 
// 
//******************************************************************************

//##1--Global Declaration---------------------------------------------##1 
var _dataTables = {};
var _emptyGuid = "00000000-0000-0000-0000-000000000000";
var _SlNo = 1;
var _result = "";
var _message = "";
var _jsonData = {};

//##2--Document Ready function-----------------------------------------##2  
$(document).ready(function () {
    debugger;
    try {
        $("#SupplierID").select2({});
        BindOrReloadSupplierPaymentTable('Init');
        $('#tblSupplierPaymentView tbody').on('dblclick', 'td', function () {
            Edit(this);
        });
    }
    catch (e) {
        console.log(e.message);
    }
});

//##3--Edit Click Redirection-----------------------------------------##3  
function Edit(curObj) {
    debugger;
    var rowData = _dataTables.SupplierPaymentTable.row($(curObj).parents('tr')).data();
    window.location.replace("NewSupplierPayment?code=ACC&ID=" + rowData.ID);
}


//##4--Bind Supplier Payment Table List-------------------------------------------##4
function BindOrReloadSupplierPaymentTable(action) {
    try {
        //creating advancesearch object
        debugger;
        SupplierPaymentAdvanceSearchViewModel = new Object();
        DataTablePagingViewModel = new Object();
        DataTablePagingViewModel.Length = 0;
        //switch case to check the operation
        switch (action) {
            case 'Reset':
                $('#SearchTerm').val('');
                $('#FromDate').val('');
                $('#ToDate').val('');
                $('#SupplierID').val('').select2();
                break;
            case 'Init':
                break;
            case 'Search':
                SupplierPaymentAdvanceSearchViewModel.FromDate = $('#FromDate').val();
                SupplierPaymentAdvanceSearchViewModel.ToDate = $('#ToDate').val();
                SupplierPaymentAdvanceSearchViewModel.SupplierID = $('#SupplierID').val();
                break;
            case 'Export':
                DataTablePagingViewModel.Length = -1;
                break;
            default:
                break;
        }
        SupplierPaymentAdvanceSearchViewModel.DataTablePaging = DataTablePagingViewModel;
        SupplierPaymentAdvanceSearchViewModel.SearchTerm = $('#SearchTerm').val();
        _dataTables.SupplierPaymentTable = $('#tblSupplierPaymentView').DataTable(
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
                    url: "GetAllSupplierPayment/",
                    data: { "supplierPaymentAdvanceSearchVM": SupplierPaymentAdvanceSearchViewModel },
                    type: 'POST'
                },
                pageLength: 10,
                columns: [
                 { "data": "EntryNo", "defaultContent": "<i>-</i>", "width": "10%" },
                 { "data": "PaymentDateFormatted", "defaultContent": "<i>-</i>", "width": "10%" },
                 { "data": "PaymentRef", "defaultContent": "<i>-</i>", "width": "15%" },
                 { "data": "SupplierName", "defaultContent": "<i>-</i>", "width": "15%" },
                 { "data": "PaymentMode", "defaultContent": "<i>-</i>", "width": "10%" },
                 { "data": "Type", "defaultContent": "<i>-</i>", "width": "7%" },
                 { "data": "CreditNo", "defaultContent": "<i>-</i>", "width": "10%" },
                 {
                     "data": "TotalPaidAmt", "defaultContent": "<i>-</i>",
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
                         return '<a href="/SupplierPayment/NewSupplierPayment?code=ACC&ID=' + data + '" class="actionLink" ><i class="glyphicon glyphicon-edit" aria-hidden="true"></i></a>'
                     }, "defaultContent": "<i>-</i>", "width": "3%"
                 }
                ],
                columnDefs: [{ "targets": [], "visible": false, "searchable": false },
                            { className: "text-left", "targets": [0,1,2,3, 4, 5, 6] },
                            { className: "text-right", "targets": [7, 8] },
                            { className: "text-center", "targets": [9] }],
                destroy: true,
                //for performing the import operation after the data loaded
                initComplete: function (settings, json) {
                    if (action === 'Export') {
                        $(".buttons-excel").trigger('click');
                        ResetSupplierPaymentList();
                    }
                }
            });
        $(".buttons-excel").hide();
    }
    catch (e) {
        console.log(e.message);
    }
}

function ResetSupplierPaymentList() {
    BindOrReloadSupplierPaymentTable('Reset');
}