//*****************************************************************************
//*****************************************************************************
//Author:Sruthi
//CreatedDate: 03-May-2018 
//LastModified: 05-May-2018 
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
        $("#RequisitionBy").select2({
        });
        BindOrReloadRequisitionDetailTable('Init');

    }
    catch (e) {
        console.log(e.message);
    }
    $("#Material").select2({
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


        //switch case to check the operation
        switch (action) {
            case 'Reset':
                $('#SearchTerm').val('');
                $('#FromDate').val('');
                $('#ToDate').val('');
                $('#ReqStatus').val('');
                $("#RequisitionBy").val('').select2();
                $('#DateFilter').val('');
                $('#DelStatus').val('');
                $('#Material').val('');
                break;
            case 'Init':
                break;
            case 'Search':
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
                buttons: [{
                    extend: 'excel',
                    exportOptions:
                                 {
                                     columns: [1, 2, 3, 4, 5,6,7,8,9,10]
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
                    url: "GetRequisitionDetailReport/",
                    data: { "requisitionDetailVM": RequisitionDetailReportViewModel },
                    type: 'POST'
                },
                pageLength: 15,
                columns: [
                    { "data": "ID", "defaultContent": "<i>-</i>" },
                    { "data": "ReqNo", "defaultContent": "<i>-</i>" },
                    { "data": "Title", "defaultContent": "<i>-</i>" },
                    { "data": "ReqDateFormatted", "defaultContent": "<i>-</i>" },
                    { "data": "ReqStatus", "defaultContent": "<i>-</i>" },                    
                    { "data": "Material.Description", "defaultContent": "<i>-</i>" },
                    { "data": "Material.UnitCode", "defaultContent": "<i>-</i>" },
                    { "data": "RequestedQty", "defaultContent": "<i>-</i>" },
                    { "data": "OrderedQty", "defaultContent": "<i>-</i>" },
                    { "data": "ReceivedQty", "defaultContent": "<i>-</i>" },
                    { "data": "DeliveryStatus", "defaultContent": "<i>-</i>" }
                ],
                columnDefs: [{ "targets": [0,2,3,4], "visible": false, "searchable": false },
                    { className: "text-left", "targets": [1, 2, 5,6,10] },
                    { className: "text-center", "targets": [3, 4, 7, 8, 9] },
                   // { className: "text-right", "targets": [10] }
                   { width: "12%", "targets": [1] },
                    { width: "15%", "targets": [5] },
                    { width: "15%", "targets": [6] },
                    { width: "15%", "targets": [7] },
                    { width: "15%", "targets": [8] },
                    { width: "15%", "targets": [9] },
                    { width: "15%", "targets": [10] }
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
                        BindOrReloadRequisitionDetailTable('Search');
                    }                   
 
                },
                // grouping with multiple columns
                drawCallback: function (settings) {
                    var api = this.api();
                    var rows = api.rows({ page: 'current' }).nodes();
                    var last = null;

                    api.column(1, { page: 'current' }).data().each(function (group, i) {
                        if (last !== group) {
                            debugger;
                            var rowData = api.row(i).data();
                            $(rows).eq(i).before('<tr class="group "><td colspan="7" class="rptGrp">' + '<b>ReqNo</b> : ' + group + "&nbsp;&nbsp;&nbsp;" + ' <b>Title</b> : ' + rowData.Title + "&nbsp;&nbsp;&nbsp;" + '<b>Req Date</b> : ' + rowData.ReqDateFormatted + "&nbsp;&nbsp;&nbsp;" + '<b>Req Status</b> : ' + rowData.ReqStatus + '</td></tr>');
                            last = group;
                        }
                    });
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