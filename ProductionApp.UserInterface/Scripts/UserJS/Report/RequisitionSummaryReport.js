var DataTables = {};
var EmptyGuid = "00000000-0000-0000-0000-000000000000";
$(document).ready(function () {
    debugger;
    try {
        $("#RequisitionBy").select2({
        });
        BindOrReloadRequisitionTable('Init');
        
    }
    catch (e) {
        console.log(e.message);
    }
});


//bind requisition report
function
    BindOrReloadRequisitionTable(action) {
    try {
        //creating advancesearch object       
        RequisitionSummaryReportViewModel = new Object();        
        DataTablePagingViewModel = new Object();     
        DataTablePagingViewModel.Length = 0;


        //switch case to check the operation
        switch (action) {
            case 'Reset':
                $('#SearchTerm').val('');
                $('#FromDate').val('');
                $('#ToDate').val('');
                $('#ReqStatus').val('');
                $("#RequisitionBy").val('').select2();
                $('#DateFilter').val('');
                break;
            case 'Init':
                break;
            case 'Search':
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
                    url: "GetRequisitionSummaryReport/",
                    data: { "requisitionSummaryVM": RequisitionSummaryReportViewModel },
                    type: 'POST'
                },
                pageLength: 15,
                columns: [
                    { "data": "ID", "defaultContent": "<i>-</i>" },
                    { "data": "ReqNo", "defaultContent": "<i>-</i>" },
                    { "data": "Title", "defaultContent": "<i>-</i>" },
                    { "data": "ReqDateFormatted", "defaultContent": "<i>-</i>" },
                    { "data": "ReqStatus", "defaultContent": "<i>-</i>" },
                    { "data": "RequisitionBy", "defaultContent": "<i>-</i>" },
                    
                ],
                columnDefs: [{ "targets": [0], "visible": false, "searchable": false },
                    { className: "text-left", "targets": [1, 2, 5] },
                    { className: "text-center", "targets": [3,4] },
                    { width: "20%", "targets": [2] }                   
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
                        BindOrReloadRequisitionTable('Search');
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
    BindOrReloadRequisitionTable('Reset');
}
//function export data to excel
function ExportReportData() {  
    BindOrReloadRequisitionTable('Export');
}

function DateFilterOnchange()
{
    BindOrReloadRequisitionTable('Apply');
}