//*****************************************************************************
//*****************************************************************************
//Author:Sruthi
//CreatedDate: 01-May-2018 
//LastModified: 05-May-2018 
//FileName: RequisitionSummaryReport.js
//Description: Client side coding for Requisition Summary Report
//******************************************************************************
//******************************************************************************
//Global Declaration
var DataTables = {};
var EmptyGuid = "00000000-0000-0000-0000-000000000000";
$(document).ready(function () {
    debugger;
    try {
        $("#RequisitionBy").select2({
        });
        $('#SearchTerm').focus();
        BindOrReloadRequisitionTable('Init');
        
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

//bind requisition summary report
function
    BindOrReloadRequisitionTable(action) {
    try {
        debugger;
        //creating advancesearch object       
        RequisitionSummaryReportViewModel = new Object();        
        DataTablePagingViewModel = new Object();     
        DataTablePagingViewModel.Length = 0;
        var SearchValue = $('#hdnSearchTerm').val();
        var SearchTerm = $('#SearchTerm').val();
        $('#hdnSearchTerm').val($('#SearchTerm').val());
        //switch case to check the operation
        switch (action) {
            case 'Reset':
                $('#SearchTerm').val('');
                $('#FromDate').val('');
                $('#ToDate').val('');
                $('#ReqStatus').val('');
                $("#RequisitionBy").val('').select2();
                $('#DateFilter').val('');
                $('.datepicker').datepicker('setDate', null);
                break;
            case 'Init': $('#SearchTerm').val('');
                $('#FromDate').val('');
                $('#ToDate').val('');
                $('#ReqStatus').val('');
                $("#RequisitionBy").val('').select2();
                $('#DateFilter').val('');
                break;
            case 'Search':
                debugger;
               
                if (SearchTerm == SearchValue)
                {
                    return false;
                }
               
                break;
            case 'Apply':              
                RequisitionSummaryReportViewModel.FromDate = $('#FromDate').val();
                RequisitionSummaryReportViewModel.ToDate = $('#ToDate').val();
                RequisitionSummaryReportViewModel.ReqStatus = $('#ReqStatus').val();
                RequisitionSummaryReportViewModel.EmployeeID = $('#RequisitionBy').val();
                RequisitionSummaryReportViewModel.DateFilter = $('#DateFilter').val();
                break;
            case 'Export':
                DataTablePagingViewModel.Length = -1;
                RequisitionSummaryReportViewModel.DataTablePaging = DataTablePagingViewModel;
                RequisitionSummaryReportViewModel.SearchTerm = $('#SearchTerm').val() == "" ? null : $('#SearchTerm').val();
                RequisitionSummaryReportViewModel.FromDate = $('#FromDate').val() == "" ? null : $('#FromDate').val();
                RequisitionSummaryReportViewModel.ToDate = $('#ToDate').val() == "" ? null : $('#ToDate').val();
                RequisitionSummaryReportViewModel.ReqStatus = $('#ReqStatus').val() == "" ? null : $('#ReqStatus').val();
                RequisitionSummaryReportViewModel.EmployeeID = $('#RequisitionBy').val() == "" ? EmptyGuid : $('#RequisitionBy').val();
                RequisitionSummaryReportViewModel.DateFilter = $('#DateFilter').val() == "" ? null : $('#DateFilter').val();
                $('#AdvanceSearch').val(JSON.stringify(RequisitionSummaryReportViewModel));
                $('#FormExcelExport').submit();
                return true;
                break;
            default:
                break;
        }      
        RequisitionSummaryReportViewModel.DataTablePaging = DataTablePagingViewModel;
        RequisitionSummaryReportViewModel.SearchTerm = $('#SearchTerm').val();
        RequisitionSummaryReportViewModel.FromDate = $('#FromDate').val();
        RequisitionSummaryReportViewModel.ToDate = $('#ToDate').val();
        RequisitionSummaryReportViewModel.ReqStatus = $('#ReqStatus').val();
        RequisitionSummaryReportViewModel.EmployeeID = $('#RequisitionBy').val();
        RequisitionSummaryReportViewModel.DateFilter = $('#DateFilter').val();
        DataTables.RequisitionList = $('#tblRequisitionSummaryReport').DataTable(
       
            {
                
                dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
                //buttons: [{
                //    extend: 'excel',
                //    exportOptions:
                //                 {
                //                     columns: [1, 2, 3, 4, 5,6,7]
                //                 }
                //}],
               
                order: false,               
                ordering: false,
                searching: false,
                paging: true,                
                lengthChange: false,
                proccessing: true,
                serverSide: true,
                ajax: {
                    url: "GetRequisitionSummaryReport/",
                    data: { "requisitionSummaryVM": RequisitionSummaryReportViewModel },
                    type: 'POST'
                },
                pageLength: 15,
                columns: [                    
                    { "data": "ReqNo", "defaultContent": "<i>-</i>" },
                    { "data": "ReqDateFormatted", "defaultContent": "<i>-</i>" },
                    { "data": "RequisitionBy", "defaultContent": "<i>-</i>" },
                    { "data": "Title", "defaultContent": "<i>-</i>" },                    
                    { "data": "ReqStatus", "defaultContent": "<i>-</i>" },                   
                    { "data": "ReqAmount", "defaultContent": "<i>-</i>" },
                    { "data": "RequiredDateFormatted", "defaultContent": "<i>-</i>" }
                    
                ],
                columnDefs: [
                    { className: "text-left", "targets": [0,2,3,4] },
                    { className: "text-center", "targets": [1,6] },
                    { className: "text-right", "targets": [5] },
                    { width: "20%", "targets": [3] }                   
                ],
               
                destroy: true,
                //for performing the import operation after the data loaded
                //initComplete: function (settings, json) {
                //    if (action === 'Export') {
                //        if (json.data.length > 0) {
                //            if (json.data[0].TotalCount > 10000) {
                //                MasterAlert("info", 'We are able to download maximum 10000 rows of data, There exist more than 10000 rows of data please filter and download')
                //            }
                //        }                       
                //        $(".buttons-excel").trigger('click');                     
                //        BindOrReloadRequisitionTable('Search');
                //    }
                //}

                initComplete: function (settings, json) {
                    debugger;
                    $('.dataTables_wrapper div.bottom div').addClass('col-md-6');
                    $('#tblRequisitionSummaryReport').fadeIn('slow');
                    if (action == undefined) {
                        $('.excelExport').hide();
                        OnServerCallComplete();
                    }

                }

            });
        //$(".buttons-excel").hide();
    }
    catch (e) {
        console.log(e.message);
    }
}

//function reset the list to initial
function ResetReportList() {  
    BindOrReloadRequisitionTable('Reset');
}

//function export data to excel
function ExportReportData() {  
    BindOrReloadRequisitionTable('Export');
}


//function to filter data based on date
function DateFilterOnchange()
{
    BindOrReloadRequisitionTable('Apply');
}