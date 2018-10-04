//*****************************************************************************
//*****************************************************************************
//Author:Sruthi
//CreatedDate: 09-08-2018
//LastModified: 09-08-2018
//FileName: SalesRegisterReport.js
//Description: Client side coding for Sales Register Report
//******************************************************************************
//******************************************************************************
//Global Declaration
var DataTables = {};
var EmptyGuid = "00000000-0000-0000-0000-000000000000";
$(document).ready(function () {
    debugger;
    try {
        $("#Customer").select2({
        });
        $('#SearchTerm').focus();
        BindOrReloadSalesRegisterTable('Init');

    }
    catch (e) {
        console.log(e.message);
    }
});

//Click function for search
debugger;
function RedirectSearchClick(e, this_obj) {
    debugger;
    var unicode = e.charCode ? e.charCode : e.keyCode
    if (unicode == 13) {
        $(this_obj).closest('.input-group').find('button').trigger('click')
    }
}

//bind purchase register report
function 
    BindOrReloadSalesRegisterTable(action) {
    try {
        debugger;
        //creating advancesearch object       
        SalesRegisterReportViewModel = new Object();
        DataTablePagingViewModel = new Object();
        DataTablePagingViewModel.Length = 0;
        var SearchValue = $('#hdnSearchTerm').val();
        var SearchTerm = $('#SearchTerm').val();
        $('#hdnSearchTerm').val($('#SearchTerm').val());


        //switch case to check the operation
        switch (action) {
            case 'Reset':
                $('#SearchTerm').val('');
                //$('#FromDate').val('');
                $('#FromDate').datepicker('setDate', null);
              //  $('#ToDate').val('');
                $('#ToDate').datepicker('setDate', null);
                $("#Customer").val('').select2();               
                break;
            case 'Init':
                break;
            case 'Search':
                if (SearchTerm == SearchValue) {
                    return false;
                }
                break;
            case 'Apply':
                SalesRegisterReportViewModel.FromDate = $('#FromDate').val();
                SalesRegisterReportViewModel.ToDate = $('#ToDate').val();               
                SalesRegisterReportViewModel.CustomerID = $('#Customer').val();             
                break;
            case 'Export':
                DataTablePagingViewModel.Length = -1;
                break;
            default:
                break;
        }
        SalesRegisterReportViewModel.DataTablePaging = DataTablePagingViewModel;
        SalesRegisterReportViewModel.SearchTerm = $('#SearchTerm').val();
        SalesRegisterReportViewModel.FromDate = $('#FromDate').val();
        SalesRegisterReportViewModel.ToDate = $('#ToDate').val();      
        SalesRegisterReportViewModel.CustomerID = $('#Customer').val();      
        DataTables.SalesRegisterList = $('#tblSalesRegisterReport').DataTable(

            {

                dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
                buttons: [{
                    extend: 'excel',
                    exportOptions:
                                 {
                                     columns: [0,1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11,12,13]
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
                    url: "GetSalesRegisterReport/",
                    data: { "salesRegisterReportVM": SalesRegisterReportViewModel },
                    type: 'POST'
                },
                pageLength: 8,
                columns: [
                    { "data": "DateFormatted", "defaultContent": "<i>-</i>" },
                    { "data": "Particulars", "defaultContent": "<i>-</i>" },
                    { "data": "Buyer", "defaultContent": "<i>-</i>" },
                    { "data": "VoucherType", "defaultContent": "<i>-</i>" },
                    { "data": "VoucherNo", "defaultContent": "<i>-</i>" },
                    { "data": "VoucherRef", "defaultContent": "<i>-</i>" },
                    { "data": "GSTIN", "defaultContent": "<i>-</i>" },
                    { "data": "Quantity", "defaultContent": "<i>-</i>" },
                    { "data": "Value", "defaultContent": "<i>-</i>" },
                    { "data": "GrossAmount", "defaultContent": "<i>-</i>" },

                    { "data": "SaleGST", "defaultContent": "<i>-</i>" },
                    { "data": "CGST", "defaultContent": "<i>-</i>" },
                    { "data": "SGST", "defaultContent": "<i>-</i>" },
                    { "data": "RoundOffAmount", "defaultContent": "<i>-</i>" }

                ],
                columnDefs: [{ "targets": [0], "visible": true, "searchable": true },
                    { className: "text-left", "targets": [1, 2,3,4,5,6,7] },
                    { className: "text-center", "targets": [] },
                    { className: "text-right", "targets": [ 8, 9, 10, 11,12] }

                ],

                destroy: true,
                //for performing the import operation after the data loaded
                initComplete: function (settings, json) {
                    if (action === 'Export') {
                        if (json.data.length > 0) {
                            if (json.data[0].TotalCount > 10000) {
                                MasterAlert("info", 'We are able to download maximum 10000 rows of data, There exist more than 10000 rows of data please filter and download')
                            }
                        }
                        $(".buttons-excel").trigger('click');
                        BindOrReloadSalesRegisterTable('Search');
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
function ResetReportList() {
    BindOrReloadSalesRegisterTable('Reset');
}

//function export data to excel
function ExportReportData() {
    BindOrReloadSalesRegisterTable('Export');
}

