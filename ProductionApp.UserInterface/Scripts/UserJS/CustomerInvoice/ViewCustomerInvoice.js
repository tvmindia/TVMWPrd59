//*****************************************************************************
//*****************************************************************************
//Author: Gibin Jacob
//CreatedDate: 20-Mar-2018 
//LastModified:  
//FileName: ViewCustomerInvoice.js
//Description: Client side coding for View Customer Invoice
//******************************************************************************
//******************************************************************************

//--Global Declaration--//
var _DataTables = {};
var _EmptyGuid = "00000000-0000-0000-0000-000000000000";



$(document).ready(function () {
    debugger;
    try {

        BindOrReloadCustomerInvoiceTable('Init');
       
        $('#tblCustomerInvoiceView tbody').on('dblclick', 'td', function () {
            Edit(this);
        });
       
    }
    catch (e) {
        console.log(e.message);
    }
});


function Edit(curObj) {
    debugger;
    var rowData = _DataTables.CustomerInvoiceTable.row($(curObj).parents('tr')).data();
    window.location.replace("NewCustomerInvoice?code=ACC&ID=" + rowData.ID);
}



//bind sales order list
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
                //$('#CustomerID').val('').select2();
                //$('#EmployeeID').val('').select2();
                break;
            case 'Init':
                break;
            case 'Search': 
                CustomerInvoiceAdvanceSearchViewModel.FromDate = $('#FromDate').val();
                CustomerInvoiceAdvanceSearchViewModel.ToDate = $('#ToDate').val();
              //  CustomerInvoiceAdvanceSearchViewModel.CustomerID = $('#CustomerID').val();
              //  CustomerInvoiceAdvanceSearchViewModel.EmployeeID = $('#EmployeeID').val();
                break;
            case 'Export':
                DataTablePagingViewModel.Length = -1;
                break;
            default:
                break;
        }
        CustomerInvoiceAdvanceSearchViewModel.DataTablePaging = DataTablePagingViewModel;
        CustomerInvoiceAdvanceSearchViewModel.SearchTerm = $('#SearchTerm').val();
        _DataTables.CustomerInvoiceTable = $('#tblCustomerInvoiceView').DataTable(
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
                    url: "GetAllCustomerInvoice/",
                    data: { "customerInvoiceAdvanceSearchVM": CustomerInvoiceAdvanceSearchViewModel },
                    type: 'POST'
                },
                pageLength: 10,
                columns: [
                    { "data": "ID", "defaultContent": "<i>-</i>" },
                    { "data": "InvoiceNo", "defaultContent": "<i>-</i>" },
                    { "data": "Customer.CompanyName", "defaultContent": "<i>-</i>" },
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
                            return '<a href="/CustomerInvoice/NewCustomerInvoice?code=SALE&ID=' + data + '" class="actionLink" ><i class="glyphicon glyphicon-share-alt" aria-hidden="true"></i></a>'
                        }, "defaultContent": "<i>-</i>", "width": "3%"
                    }
                ],
                columnDefs: [{ "targets": [0], "visible": false, "searchable": false },
                    { className: "text-left", "targets": [1, 2,] },
                    { className: "text-right", "targets": [5] },
                    { className: "text-center", "targets": [3,4,6] }],
                destroy: true,
                //for performing the import operation after the data loaded
                initComplete: function (settings, json) {
                    if (action === 'Export') {
                        $(".buttons-excel").trigger('click');
                        ResetCustomerInvoiceList();
                    }
                }
            });
        $(".buttons-excel").hide();
    }
    catch (e) {
        console.log(e.message);
    }
}


function ResetCustomerInvoiceList()
{

}