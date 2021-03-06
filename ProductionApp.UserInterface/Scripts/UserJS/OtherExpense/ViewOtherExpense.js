﻿//*****************************************************************************
//*****************************************************************************
//Author: Angel
//CreatedDate: 30-Apr-2018 
//LastModified: 30-Apr-2018
//FileName: ViewOtherExpense.js
//Description: Client side coding for ViewOtherExpense
//******************************************************************************
// ##1--Global Declaration
// ##2--Document Ready Function 
// ##3--OtherExpense Table Binding Function
//******************************************************************************
//##1--Global Declaration---------------------------------------------##1 
var _dataTable = {};
var _emptyGuid = "00000000-0000-0000-0000-000000000000";
//##2--Document Ready Function----------------------------------------##2
$(document).ready(function () {
    debugger;
    try {
        $('#AccountCode').select2();
        $('#Status').select2();
        $('#SearchTerm').focus();
        BindOrReloadOtherExpenseTable('Init');
        $('#SearchTerm').keypress(function (event) {
            if (event.which === 13) {
                event.preventDefault();
                BindOrReloadOtherExpenseTable('Apply');
            }
        });
        $('#tblOtherExpense tbody').on('dblclick', 'td', function () {
            Edit(this);
        });
        
    }
    catch (e) {
        console.log(e.message);
    }
});
//##3--OtherExpense Table Binding Function-------------------------------------##3
function BindOrReloadOtherExpenseTable(action) {
    try {
        debugger;
        otherExpenseAdvanceSearchVM = new Object();
        DataTablePagingViewModel = new Object();
        ProductViewModel = new Object();
        DataTablePagingViewModel.Length = 0;
        //switch case to check the operation
        switch (action) {
            case 'Reset':
                $('#SearchTerm').val('');
                $('#FromDate').val('');
                $('#AccountCode').val('').trigger('change');
                $('#ToDate').val('');
                $('#Status').val('').trigger('change');
                break;
            case 'Init':
                break;
            case 'Search':
                break;
            case 'Apply':
                otherExpenseAdvanceSearchVM.FromDate = $('#FromDate').val();
                otherExpenseAdvanceSearchVM.ToDate = $('#ToDate').val();
                break;
            case 'Export':
                DataTablePagingViewModel.Length = -1;
                break;
            default:
                break;
        }
        otherExpenseAdvanceSearchVM.DataTablePaging = DataTablePagingViewModel;
        otherExpenseAdvanceSearchVM.SearchTerm = $('#SearchTerm').val();
        otherExpenseAdvanceSearchVM.FromDate = $('#FromDate').val();
        otherExpenseAdvanceSearchVM.ToDate = $('#ToDate').val();
        otherExpenseAdvanceSearchVM.AccountCode = $('#AccountCode').val().split("|")[0];
        //To split value of #ChartOfAccountCode by '|' and to take only the AccountCode__________^
        if ($('#Status').val() != "")
            otherExpenseAdvanceSearchVM.Status = $('#Status').val();
        else
            otherExpenseAdvanceSearchVM.Status = -1;
        _dataTable.OtherExpense = $('#tblOtherExpense').DataTable(
            {
                dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
                buttons: [{
                    extend: 'excel',
                    exportOptions:
                                 {
                                     columns: [0, 1, 2, 3,4,5,6]
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
                    url: "GetAllOtherExpense/",
                    data: { "otherExpenseAdvanceSearchVM": otherExpenseAdvanceSearchVM },
                    type: 'POST'
                },
                pageLength: 10,
                columns: [
                    { "data": "EntryNo", "defaultContent": "<i>-</i>", "width": "10%" },
                    { "data": "ExpenseDateFormatted", "defaultContent": "<i>-</i>", "width": "15%" },
                    { "data": "Account", "defaultContent": "<i>-</i>", "width": "15%" },
                    { "data": "PaymentMode", "defaultContent": "<i>-</i>", "width": "10%" },
                    {
                        "data": "Description", render: function (data, type, row) {
                            if (row.ReversalRef != null && row.Description != null)
                                return row.Description + " ( Reversal Of  <label><i><b>Ref# " + row.ReversalRef + "</b></i></label>)"
                            else if (row.ReversalRef != null && row.Description == null)
                                return " ( Reversal Of  <label><i><b>Ref# " + row.ReversalRef + "</b></i></label>)"
                            else
                                return row.Description

                        }, "defaultContent": "<i>-</i>", "width": "20%"
                    },
                    { "data": "Amount", "defaultContent": "<i>-</i>", "width": "15%" },
                    { "data": "ApprovalStatus", "defaultContent": "<i>-</i>", "width": "22%" },
                    {
                        "data": "ID", "orderable": false, render: function (data, type, row) {
                            return '<a href="/OtherExpense/NewOtherExpense?code=ACC&ID=' + data + '" class="actionLink" ><i class="glyphicon glyphicon-edit" aria-hidden="true"></i></a>'
                        }, "defaultContent": "<i>-</i>", "width": "3%"
                    }
                ],
                columnDefs: [
                            { className: "text-left", "targets": [2,4, 3,6, 0] },
                            { className: "text-right", "targets": [5] },
                            { className: "text-center", "targets": [1] }],
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
                        BindOrReloadOtherExpenseTable('Search');
                    }
                }
            });
        $(".buttons-excel").hide();
    }

    catch (e) {
        console.log(e.message);
    }
}
//##4--Edit on table Click----------------------------------------##4
function Edit(curObj) {
    var rowData = _dataTable.OtherExpense.row($(curObj).parents('tr')).data();
    window.location.replace("NewOtherExpense?code=ACC&ID=" + rowData.ID);

}