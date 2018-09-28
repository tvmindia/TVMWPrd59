//*****************************************************************************
//*****************************************************************************
//Author:Sruthi
//CreatedDate: 03-May-2018 
//LastModified: 27-Sep-2018 
//FileName: RequisitionDetailReport.js
//Description: Client side coding for Requisition Detail Report
//******************************************************************************
//******************************************************************************

//Global Declaration
var DataTables = {};
var EmptyGuid = "00000000-0000-0000-0000-000000000000";
$(document).ready(function () {
    debugger;
    try {
        $("#RequisitionBy,#ReqStatus").select2({
        });
        $('#SearchTerm').focus();
        BindOrReloadRequisitionDetailTable('Init');

    }
    catch (e) {
        console.log(e.message);
    }
    $("#Material").select2({

    });
    $("#DelStatus").select2({
    });
});

//Click function for search
function RedirectSearchClick(e, this_obj) {
    debugger;
    var unicode = e.charCode ? e.charCode : e.keyCode
    if (unicode == 13) {
        $(this_obj).closest('.input-group').find('button').trigger('click')
    }
}

//bind requisition detail report
function 
    BindOrReloadRequisitionDetailTable(action) {
    try {
        //creating advancesearch object       
        RequisitionDetailReportViewModel = new Object();
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
                $('#ReqStatus').val('').select2();
                $("#RequisitionBy").val('').select2();
                $('#DateFilter').val('');
                $('#DelStatus').val('').select2();
                $('#Material').val('').select2();
                $('.datepicker').datepicker('setDate', null);
                break;
            case 'Init':
                break;
            case 'Search':
                if (SearchTerm == SearchValue) {
                    return false;
                }
                break;
            case 'Apply':
                RequisitionDetailReportViewModel.FromDate = $('#FromDate').val();
                RequisitionDetailReportViewModel.ToDate = $('#ToDate').val();
                RequisitionDetailReportViewModel.ReqStatus = $('#ReqStatus').val();
                RequisitionDetailReportViewModel.EmployeeID = $('#RequisitionBy').val();
                RequisitionDetailReportViewModel.DateFilter = $('#DateFilter').val();
                RequisitionDetailReportViewModel.MaterialID = $('#Material').val();
                RequisitionDetailReportViewModel.DeliveryStatus = $('#DelStatus').val();
                break;
            case 'Export':
                DataTablePagingViewModel.Length = -1;
                RequisitionDetailReportViewModel.DataTablePaging = DataTablePagingViewModel;
                RequisitionDetailReportViewModel.SearchTerm = $('#SearchTerm').val() == "" ? null : $('#SearchTerm').val();
                RequisitionDetailReportViewModel.FromDate = $('#FromDate').val() == "" ? null : $('#FromDate').val();
                RequisitionDetailReportViewModel.ToDate = $('#ToDate').val() == "" ? null : $('#ToDate').val();
                RequisitionDetailReportViewModel.ReqStatus = $('#ReqStatus').val() == "" ? null : $('#ReqStatus').val();
                RequisitionDetailReportViewModel.EmployeeID = $('#RequisitionBy').val() == "" ? EmptyGuid : $('#RequisitionBy').val();
                RequisitionDetailReportViewModel.DateFilter = $('#DateFilter').val() == "" ? null : $('#DateFilter').val();
                RequisitionDetailReportViewModel.MaterialID = $('#Material').val() == "" ? EmptyGuid : $('#Material').val();
                RequisitionDetailReportViewModel.DeliveryStatus = $('#DelStatus').val() == "" ? null : $('#DelStatus').val();
                $('#AdvanceSearch').val(JSON.stringify(RequisitionDetailReportViewModel));
                $('#FormExcelExport').submit();
                return true;
                break;
            default:
                break;
        }
        RequisitionDetailReportViewModel.DataTablePaging = DataTablePagingViewModel;
        RequisitionDetailReportViewModel.SearchTerm = $('#SearchTerm').val();
        RequisitionDetailReportViewModel.FromDate = $('#FromDate').val();
        RequisitionDetailReportViewModel.ToDate = $('#ToDate').val();
        RequisitionDetailReportViewModel.ReqStatus = $('#ReqStatus').val();
        RequisitionDetailReportViewModel.EmployeeID = $('#RequisitionBy').val();
        RequisitionDetailReportViewModel.DateFilter = $('#DateFilter').val();
        RequisitionDetailReportViewModel.MaterialID = $('#Material').val();
        RequisitionDetailReportViewModel.DeliveryStatus = $('#DelStatus').val();
        DataTables.RequisitionList = $('#tblRequisitionDetailReport').DataTable(

            {

                dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',               
                order: false,
                ordering: false,
                searching: false,
                paging: true,
                lengthChange: false,
                proccessing: true,
                serverSide: true,
                ajax: {
                    url: "GetRequisitionDetailReport/",
                    data: { "requisitionDetailVM": RequisitionDetailReportViewModel },
                    type: 'POST'
                },
                pageLength: 15,
                columns: [                   
                    { "data": "ReqNo", "defaultContent": "<i>-</i>" },
                    { "data": "Title", "defaultContent": "<i>-</i>" },
                    { "data": "ReqDateFormatted", "defaultContent": "<i>-</i>" },
                    { "data": "ReqStatus", "defaultContent": "<i>-</i>" },                    
                    { "data": "Material.Description", "defaultContent": "<i>-</i>" },
                    { "data": "Material.UnitCode", "defaultContent": "<i>-</i>" },
                    { "data": "RequestedQty", "defaultContent": "<i>-</i>" },
                    { "data": "OrderedQty", "defaultContent": "<i>-</i>" },
                    { "data": "ReceivedQty", "defaultContent": "<i>-</i>" },
                    { "data": "DeliveryStatus", "defaultContent": "<i>-</i>" },
                    { "data": "RequisitionBy", "defaultContent": "<i>-</i>" }
                ],
                columnDefs: [{ "targets": [1,2,3,10], "visible": false, "searchable": false },
                    { className: "text-left", "targets": [0,4, 5,9] },
                    { className: "text-center", "targets": [6,7,8] },
                   // { className: "text-right", "targets": [10] }
                   { width: "12%", "targets": [0] },
                    { width: "15%", "targets": [5] },
                    { width: "15%", "targets": [6] },
                    { width: "15%", "targets": [7] },
                    { width: "15%", "targets": [8] },
                    { width: "15%", "targets": [9] },
                  
                ],
                
                destroy: true,
                //for performing the import operation after the data loaded
                initComplete: function (settings, json) {
                    debugger;
                    $('.dataTables_wrapper div.bottom div').addClass('col-md-6');
                    $('#tblRequisitionDetailReport').fadeIn('slow');
                    if (action == undefined) {
                        $('.excelExport').hide();
                        OnServerCallComplete();
                    }

                },
                // grouping with multiple columns
                drawCallback: function (settings) {
                    var api = this.api();
                    var rows = api.rows({ page: 'current' }).nodes();
                    var last = null;

                    api.column(0, { page: 'current' }).data().each(function (group, i) {
                        if (last !== group) {
                            debugger;
                            var rowData = api.row(i).data();
                            $(rows).eq(i).before('<tr class="group "><td colspan="7" class="rptGrp">' + '<b>Req No</b> : ' + group + "&nbsp;&nbsp;&nbsp;" + ' <b>Title</b> : ' + rowData.Title + "&nbsp;&nbsp;&nbsp;" + '<b>Req Date</b> : ' + rowData.ReqDateFormatted + "&nbsp;&nbsp;&nbsp;" + '<b>Req Status</b> : ' + rowData.ReqStatus + "&nbsp;&nbsp;&nbsp;" + ' <b>Req By</b> : ' + rowData.RequisitionBy + '</td></tr>');
                            last = group;
                        }
                    });
                }
            });
       
    }
    catch (e) {
        console.log(e.message);
    }
}

//function reset the list to initial
function ResetReportList() {
    BindOrReloadRequisitionDetailTable('Reset');
}
//function export data to excel
function ExportReportData() {
    BindOrReloadRequisitionDetailTable('Export');
}
//function to filter data based on date
function DateFilterOnchange() {
    BindOrReloadRequisitionDetailTable('Apply');
}