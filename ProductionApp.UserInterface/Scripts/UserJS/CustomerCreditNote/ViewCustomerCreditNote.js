//*****************************************************************************
//*****************************************************************************
//Author: Gibin Jacob Job
//CreatedDate: 27-AUG-2018 
//LastModified:  27-AUG-2018 
//FileName: ViewCustomerCreditNote.js
//Description: Client side coding for View Customer CreditNote
//******************************************************************************
// ##1--Global Declaration
// ##2--Document Ready function
// ##3--Edit Click Redirection
// ##4--Bind Customer CreditNote Table List
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
        BindOrReloadCustomerCreditNoteTable('Init');
        $('#tblCustomerCreditNoteView tbody').on('dblclick', 'td', function () {
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
    var rowData = _dataTables.CustomerCreditNoteTable.row($(curObj).parents('tr')).data();
    window.location.replace("NewCustomerCreditNote?code=ACC&ID=" + rowData.ID);
}


//##4--Bind Customer CreditNote Table List-------------------------------------------##4
function BindOrReloadCustomerCreditNoteTable(action) {
    try {
        //creating advancesearch object
        debugger;
        CustomerCreditNoteAdvanceSearchViewModel = new Object();
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
                CustomerCreditNoteAdvanceSearchViewModel.FromDate = $('#FromDate').val();
                CustomerCreditNoteAdvanceSearchViewModel.ToDate = $('#ToDate').val();
                CustomerCreditNoteAdvanceSearchViewModel.CustomerID = $('#CustomerID').val();
              //  CustomerCreditNoteAdvanceSearchViewModel.CreditNoteType = $('#CreditNoteType').val();
                break;
            case 'Export':
                DataTablePagingViewModel.Length = -1;
                break;
            default:
                break;
        }
        CustomerCreditNoteAdvanceSearchViewModel.DataTablePaging = DataTablePagingViewModel;
        CustomerCreditNoteAdvanceSearchViewModel.SearchTerm = $('#SearchTerm').val();
        _dataTables.CustomerCreditNoteTable = $('#tblCustomerCreditNoteView').DataTable(
            {
                dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
                buttons: [{
                    extend: 'excel',
                    exportOptions:
                                 {
                                     columns: [0, 1, 2, 3, 4]
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
                    url: "GetAllCustomerCreditNote/",
                    data: { "customerCreditNoteAdvanceSearchVM": CustomerCreditNoteAdvanceSearchViewModel },
                    type: 'POST'
                },
                pageLength: 10,
                columns: [
                    { "data": "CreditNoteNo", "defaultContent": "<i>-</i>", "width": "10%" }, 
                    { "data": "CustomerName", "defaultContent": "<i>-</i>", "width": "27%" },
                    { "data": "CreditNoteDateFormatted", "defaultContent": "<i>-</i>", "width": "20%" },
                    {
                        "data": "CreditAmount", "defaultContent": "<i>-</i>",
                        'render': function (data, type, row) {
                            return roundoff(data)
                        }, "width": "20%"
                    },
                      {
                          "data": "", "defaultContent": "<i>-</i>",
                          'render': function (data, type, row) {
                              return roundoff(row.CreditAmount - row.AvailableCredit)
                          }, "width": "20%"
                      },
                  //  { "data": "Status", "defaultContent": "<i>-</i>", "width": "7%" },
                    {
                        "data": "ID", "orderable": false, render: function (data, type, row) {
                            return '<a href="/CustomerCreditNote/NewCustomerCreditNote?code=ACC&ID=' + data + '" class="actionLink" ><i class="glyphicon glyphicon-edit" aria-hidden="true"></i></a>'
                        }, "defaultContent": "<i>-</i>", "width": "3%"
                    }
                ],
                columnDefs: [{ "targets": [], "visible": false, "searchable": false },
                    { className: "text-left", "targets": [0, 1] },
                    { className: "text-right", "targets": [3,4] },
                    { className: "text-center", "targets": [2,5] }],
                destroy: true,
                //for performing the import operation after the data loaded
                initComplete: function (settings, json) {
                    if (action === 'Export') {
                        $(".buttons-excel").trigger('click');
                        BindOrReloadCustomerCreditNoteTable('Search');
                    }
                }
            });
        $(".buttons-excel").hide();
    }
    catch (e) {
        console.log(e.message);
    }
}

