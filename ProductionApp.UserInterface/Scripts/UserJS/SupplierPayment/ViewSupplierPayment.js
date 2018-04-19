//*****************************************************************************
//*****************************************************************************
//Author: Gibin Jacob
//CreatedDate: 18-Apr-2018 
//LastModified:  18-Apr-2018 
//FileName: NewSupplierPayment.js
//Description: Client side coding for New/Edit Supplier Payment
//******************************************************************************
// ##1--Global Declaration
// ##2--Document Ready function
// ##3-- 
// ##4-- 
// ##5-- 
// ##6-- 
// ##7-- 
// ##8-- 
// ##9-- 
// ##10-- 
// ##11--Save  Supplier Payment 
// ##12--Save Success Supplier Payment
// ##13--Bind Supplier Payment By ID
// ##14--Reset Button Click
// ##15-- 
// ##16--DELETE Supplier Payment 
// ##17--DELETE Supplier Payment Details 
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


//##4--Bind Supplier Invoice Table List-------------------------------------------##4
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
                    { "data": "ID", "defaultContent": "<i>-</i>" },
                    { "data": "InvoiceNo", "defaultContent": "<i>-</i>" },
                    { "data": "Supplier.CompanyName", "defaultContent": "<i>-</i>" },
                    { "data": "InvoiceDateFormatted", "defaultContent": "<i>-</i>" },
                    { "data": "PaymentDueDateFormatted", "defaultContent": "<i>-</i>" },
                    {
                        "data": "InvoiceAmount", "defaultContent": "<i>-</i>",
                        'render': function (data, type, row) {
                            return roundoff(data)
                        }
                    },
                    {
                        "data": "ID", "orderable": false, render: function (data, type, row) {
                            return '<a href="/SupplierPayment/NewSupplierPayment?code=ACC&ID=' + data + '" class="actionLink" ><i class="glyphicon glyphicon-edit" aria-hidden="true"></i></a>'
                        }, "defaultContent": "<i>-</i>", "width": "3%"
                    }
                ],
                columnDefs: [{ "targets": [0], "visible": false, "searchable": false },
                    { className: "text-left", "targets": [1, 2, ] },
                    { className: "text-right", "targets": [5] },
                    { className: "text-center", "targets": [3, 4, 6] }],
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