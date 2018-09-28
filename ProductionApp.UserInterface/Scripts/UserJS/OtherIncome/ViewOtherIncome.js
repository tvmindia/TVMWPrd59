//*****************************************************************************
//*****************************************************************************
//Author: Arul
//CreatedDate: 01-May-2018 
//LastModified: 02-May-2018
//FileName: ViewOtherIncome.js
//Description: Client side coding to List Other Income
//******************************************************************************
//******************************************************************************

var _dataTable = {};
var _emptyGuid = "00000000-0000-0000-0000-000000000000";


$(document).ready(function () {
    try
    {
        debugger;
        $('#ChartOfAccountCode,#PaymentMode').select2();
        $('#SearchTerm').focus();
        BindOrReloadOtherIncomeTable('Init');
        //$('#SearchTerm').keypress(function (event) {
        //    if (event.which === 13) {
        //        event.preventDefault();
        //        BindOrReloadOtherIncomeTable('Apply');
        //    }
        //});
    }
    catch(ex)
    {
        console.log(ex.message);
    }
})

function BindOrReloadOtherIncomeTable(action) {
    try {
        debugger;
        otherIncomeAdvanceSearchVM = new Object();
        DataTablePagingViewModel = new Object();
        ProductViewModel = new Object();
        DataTablePagingViewModel.Length = 0;
        //switch case to check the operation
        switch (action) {
            case 'Reset':
                $('#SearchTerm').val('');
                $('#FromDate').val('');
                $('#ToDate').val('');
                $('#ChartOfAccountCode').val('').trigger('change');
                $('#PaymentMode').val('').trigger('change');
                break;
            case 'Init':
                break;
            case 'Search':
                break;
            case 'Apply':
                break;
            case 'Export':
                DataTablePagingViewModel.Length = -1;
                break;
            default:
                break;
        }
        otherIncomeAdvanceSearchVM.DataTablePaging = DataTablePagingViewModel;
        otherIncomeAdvanceSearchVM.SearchTerm = $('#SearchTerm').val();
        otherIncomeAdvanceSearchVM.FromDate = $('#FromDate').val();
        otherIncomeAdvanceSearchVM.ToDate = $('#ToDate').val();
        otherIncomeAdvanceSearchVM.PaymentMode = $('#PaymentMode').val();
        otherIncomeAdvanceSearchVM.ChartOfAccount = new Object();
        otherIncomeAdvanceSearchVM.ChartOfAccount.Code = $('#ChartOfAccountCode').val().split("|")[0];
        //To split value of #ChartOfAccountCode by '|' and to take only the AccountCode__________^
        _dataTable.OtherIncome = $('#tblOtherIncome').DataTable(
            {
                dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
                buttons: [{
                    extend: 'excel',
                    exportOptions:
                                 {
                                     columns: [0, 1, 2, 5, 3, 4]
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
                    url: "GetAllOtherIncome/",
                    data: { "otherIncomeAdvanceSearchVM": otherIncomeAdvanceSearchVM },
                    type: 'POST'
                },
                pageLength: 10,
                columns: [
                    { "data": "EntryNo", "defaultContent": "<i>-</i>", "width": "10%" },
                    { "data": "IncomeDateFormatted", "defaultContent": "<i>-</i>", "width": "15%" },
                    { "data": "AccountCode", "defaultContent": "<i>-</i>", "width": "25%" },
                    { "data": "PaymentMode", "defaultContent": "<i>-</i>", "width": "20%" },
                    { "data": "Amount", render: function (data, type, row) { return formatCurrency(roundoff(data,2)); }, "defaultContent": "<i>-</i>", "width": "15%" },
                    { "data": "AccountSubHead", "defaultContent": "<i>-</i>", "width": "22%" },
                    {
                        "data": "ID", "orderable": false, render: function (data, type, row) {
                            return '<a href="NewOtherIncome?code=ACC&ID=' + data + '" class="actionLink" ><i class="glyphicon glyphicon-edit" aria-hidden="true"></i></a>'
                        }, "defaultContent": "<i>-</i>", "width": "3%"
                    }
                ],
                columnDefs: [
                            { className: "text-left", "targets": [2, 3, 5, 0] },
                            { className: "text-right", "targets": [4] },
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
                        BindOrReloadOtherIncomeTable('Search');
                    }
                }
            });
        $(".buttons-excel").hide();

        $('#tblOtherIncome tbody').on('dblclick', 'td', function () {
            Edit(this);
        });
    }
    catch (ex) {
        console.log(ex.message);
    }
}

function Edit(curobj) {
    debugger;
    var OtherIncomeViewModel = _dataTable.OtherIncome.row($(curobj).parents('tr')).data();
    window.location.replace("/OtherIncome/NewOtherIncome?code=ACC&id=" + OtherIncomeViewModel.ID);
}
