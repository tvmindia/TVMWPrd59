//*****************************************************************************
//*****************************************************************************
//Author: Gibin Jacob
//CreatedDate: 20-Mar-2018 
//LastModified:  17-APR-2018 
//FileName: ViewCustomerInvoice.js
//Description: Client side coding for View Customer Invoice
//******************************************************************************
// ##1--Global Declaration
// ##2--Document Ready function
// ##3--Edit Click Redirection
// ##4--Bind Customer Invoice Table List
// 
//******************************************************************************


//##1--Global Declaration---------------------------------------------##1 
var _dataTables = {};
var _emptyGuid = "00000000-0000-0000-0000-000000000000";


//##2--Document Ready function-----------------------------------------##2  
$(document).ready(function () {
    debugger;
    try {
        $("#CustomerID").select2({});
        BindOrReloadCustomerInvoiceTable('Init');
        $('#tblCustomerInvoiceView tbody').on('dblclick', 'td', function () {
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
    var rowData = _dataTables.CustomerInvoiceTable.row($(curObj).parents('tr')).data();
    window.location.replace("NewCustomerInvoice?code=ACC&ID=" + rowData.ID);
}


//##4--Bind Customer Invoice Table List-------------------------------------------##4
function BindOrReloadCustomerInvoiceTable(action) {
    try {
        //creating advancesearch object
        debugger;
        CustomerInvoiceAdvanceSearchViewModel = new Object();
        DataTablePagingViewModel = new Object();
        DataTablePagingViewModel.Length = 0;
        //switch case to check the operation
        switch (action) {
            case 'Reset':
                $('#SearchTerm').val('');
                $('#FromDate').val('');
                $('#ToDate').val('');
                $('#CustomerID').val('').select2();
                break;
            case 'Init':
                break;
            case 'Search':
                CustomerInvoiceAdvanceSearchViewModel.FromDate = $('#FromDate').val();
                CustomerInvoiceAdvanceSearchViewModel.ToDate = $('#ToDate').val();
                CustomerInvoiceAdvanceSearchViewModel.CustomerID = $('#CustomerID').val();
                CustomerInvoiceAdvanceSearchViewModel.InvoiceType = $('#InvoiceType').val();
                break;
            case 'Export':
                DataTablePagingViewModel.Length = -1;
                break;
            default:
                break;
        }
        CustomerInvoiceAdvanceSearchViewModel.DataTablePaging = DataTablePagingViewModel;
        CustomerInvoiceAdvanceSearchViewModel.SearchTerm = $('#SearchTerm').val();
        _dataTables.CustomerInvoiceTable = $('#tblCustomerInvoiceView').DataTable(
            {
                dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
                buttons: [{
                    extend: 'excel',
                    exportOptions:
                                 {
                                     columns: [0, 1, 2, 3, 4, 5, 6, 7]
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
                    url: "GetAllCustomerInvoice/",
                    data: { "customerInvoiceAdvanceSearchVM": CustomerInvoiceAdvanceSearchViewModel },
                    type: 'POST'
                },
                pageLength: 10,
                columns: [
                    { "data": "InvoiceNo", "defaultContent": "<i>-</i>","width": "7%" },
                    {
                        "data": "InvoiceType", "defaultContent": "<i>-</i>",
                        'render': function (data, type, row) {
                            if (row.InvoiceType == 'RI')
                                return 'Regular'
                            else
                                return  'Service'
                        }, "width": "5%"
                    },
                    { "data": "Customer.CompanyName", "defaultContent": "<i>-</i>", "width": "30%" },
                    { "data": "InvoiceDateFormatted", "defaultContent": "<i>-</i>", "width": "10%" },
                    { "data": "PaymentDueDateFormatted", "defaultContent": "<i>-</i>", "width": "10%" },
                    {
                        "data": "InvoiceAmount", "defaultContent": "<i>-</i>",
                        'render': function (data, type, row) {
                            return roundoff(data)
                        }, "width": "9%"
                    },
                      {
                          "data": "BalanceDue", "defaultContent": "<i>-</i>",
                          'render': function (data, type, row) {
                              return roundoff(data)
                          }, "width": "9%"
                      },
                    { "data": "LastPaymentDateFormatted", "defaultContent": "<i>-</i>", "width": "10%" },
                    { "data": "Status", "defaultContent": "<i>-</i>", "width": "7%" },
                    {
                        "data": "ID", "orderable": false, render: function (data, type, row) {
                            return '<a href="/CustomerInvoice/NewCustomerInvoice?code=ACC&ID=' + data + '" class="actionLink" ><i class="glyphicon glyphicon-edit" aria-hidden="true"></i></a>'
                        }, "defaultContent": "<i>-</i>", "width": "3%"
                    }
                ],
                columnDefs: [{ "targets": [], "visible": false, "searchable": false },
                    { className: "text-left", "targets": [0, 1,2, 8] },
                    { className: "text-right", "targets": [5,6] },
                    { className: "text-center", "targets": [ 3,4, 7, 9] }],
                destroy: true,
                //for performing the import operation after the data loaded
                initComplete: function (settings, json) {
                    if (action === 'Export') {
                        $(".buttons-excel").trigger('click');
                        BindOrReloadCustomerInvoiceTable('Search');
                    }
                }
            });
        $(".buttons-excel").hide();
    }
    catch (e) {
        console.log(e.message);
    }
}

