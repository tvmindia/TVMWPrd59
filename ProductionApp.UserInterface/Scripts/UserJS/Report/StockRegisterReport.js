//*****************************************************************************
//*****************************************************************************
//Author:Sruthi
//CreatedDate: 16-May-2018 
//LastModified: 16-May-2018 
//FileName: StockRegisterReport.js
//Description: Client side coding for Stock Register Report
//******************************************************************************
//******************************************************************************
//Global Declaration
var DataTables = {};
var EmptyGuid = "00000000-0000-0000-0000-000000000000";
$(document).ready(function () {
    debugger;
    try {
        $("#MaterialType,#Material").select2({
        });
        $('#SearchTerm').focus();
        BindOrReloadStockRegisterTable('Init');

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

//bind stock register report
function
    BindOrReloadStockRegisterTable(action) {
    try {
        debugger;
        //creating advancesearch object       
        StockRegisterReportViewModel = new Object();
        DataTablePagingViewModel = new Object();
        DataTablePagingViewModel.Length = 0;
        var SearchValue = $('#hdnSearchTerm').val();
        var SearchTerm = $('#SearchTerm').val();
        $('#hdnSearchTerm').val($('#SearchTerm').val());

        //switch case to check the operation
        switch (action) {
            case 'Reset':
                $('#SearchTerm').val('');               
                $('#MaterialType').val('').select2();
                $('#Material').val('').select2();
                $('#Location').val('Store');
                break;
            case 'Init':
                break;
            case 'Search':
                if (SearchTerm == SearchValue) {
                    return false;
                }
                break;
            case 'Apply':                           
                StockRegisterReportViewModel.MaterialTypeCode = $('#MaterialType').val();
                StockRegisterReportViewModel.MaterialID = $('#Material').val();
                StockRegisterReportViewModel.Location = $('#Location').val();

                break;
            case 'Export':
                DataTablePagingViewModel.Length = -1;
                StockRegisterReportViewModel.DataTablePaging = DataTablePagingViewModel;
                StockRegisterReportViewModel.SearchTerm = $('#SearchTerm').val() == "" ? null : $('#SearchTerm').val();
                StockRegisterReportViewModel.MaterialTypeCode = $('#MaterialType').val() == "" ? null : $('#MaterialType').val();
                StockRegisterReportViewModel.MaterialID = $('#Material').val() == "" ? EmptyGuid : $('#Material').val();
                StockRegisterReportViewModel.Location = $('#Location').val() == "" ? null : $('#Location').val();
                $('#AdvanceSearch').val(JSON.stringify(StockRegisterReportViewModel));
                $('#FormExcelExport').submit();
                return true;
                break;
            default:
                break;
        }
        StockRegisterReportViewModel.DataTablePaging = DataTablePagingViewModel;
        StockRegisterReportViewModel.SearchTerm = $('#SearchTerm').val();       
        StockRegisterReportViewModel.MaterialTypeCode = $('#MaterialType').val();
        StockRegisterReportViewModel.MaterialID = $('#Material').val();
        StockRegisterReportViewModel.Location = $('#Location').val();


        DataTables.StockRegisterList = $('#tblStockRegisterReport').DataTable(

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
                    url: "GetStockRegisterReport/",
                    data: { "stockRegisterReportVM": StockRegisterReportViewModel },
                    type: 'POST'
                },
                pageLength: 15,
                columns: [                   
                    { "data": "TypeDescription", "defaultContent": "<i>-</i>" },
                    { "data": "Description", "defaultContent": "<i>-</i>" },
                    { "data": "UnitCode", "defaultContent": "<i>-</i>" },
                    { "data": "CurrentStock", "defaultContent": "<i>-</i>" },
                    { "data": "CostPrice", render: function (data, type, row) { return roundoff(data) }, "defaultContent": "<i>-</i>" },
                    { "data": "StockValue",render: function (data, type, row) { return roundoff(data) }, "defaultContent": "<i>-</i>" }
                   


                ],
                columnDefs: [{ "targets": [0], "visible": false, "searchable": false },
                    { className: "text-left", "targets": [1] },
                    { className: "text-center", "targets": [2 ] },
                    { className: "text-right", "targets": [3,4,5] }

                ],

                destroy: true,
                //for performing the import operation after the data loaded
                initComplete: function (settings, json) {
                    debugger;
                    $('.dataTables_wrapper div.bottom div').addClass('col-md-6');
                    $('#tblStockRegisterReport').fadeIn('slow');
                    if (action == undefined) {
                        $('.excelExport').hide();
                        OnServerCallComplete();
                    }

                },
            
                // grouping with  columns
                drawCallback: function (settings) {
                    var api = this.api();
                    var rows = api.rows({ page: 'current' }).nodes();
                    var last = null;

                    api.column(0, { page: 'current' }).data().each(function (group, i) {
                        if (last !== group) {
                            debugger;
                            var rowData = api.row(i).data();
                            $(rows).eq(i).before('<tr class="group "><td colspan="7" class="rptGrp">' + '<b>Material Type</b> : ' + group +  '</td></tr>');
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
    BindOrReloadStockRegisterTable('Reset');
}

//function export data to excel
function ExportReportData() {
    BindOrReloadStockRegisterTable('Export');
}

