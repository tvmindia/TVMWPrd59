//*****************************************************************************
//*****************************************************************************
//Author:Sruthi
//CreatedDate: 22-May-2018 
//LastModified: 24-May-2018 
//FileName: StockRegisterFGReport.js
//Description: Client side coding for Stock Register FG Report
//******************************************************************************
//******************************************************************************
//Global Declaration
var DataTables = {};
var EmptyGuid = "00000000-0000-0000-0000-000000000000";
$(document).ready(function () {
    debugger;
    try
    {
        $('#SearchTerm').focus();
        BindOrReloadStockRegisterFGTable('Init');
      
    }
    catch (e)
    {
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

//bind stock register FG report
function 
    BindOrReloadStockRegisterFGTable(action) {
    try {
        debugger;
        //creating advancesearch object       
        StockRegisterFGReportViewModel = new Object();
        DataTablePagingViewModel = new Object();
        DataTablePagingViewModel.Length = 0;
        var SearchValue = $('#hdnSearchTerm').val();
        var SearchTerm = $('#SearchTerm').val();
        $('#hdnSearchTerm').val($('#SearchTerm').val());


        //switch case to check the operation
        switch (action) {
            case 'Reset':
                $('#SearchTerm').val('');
                $('#Type').val('');               
                break;
            case 'Init':
                break;
            case 'Search':
                if (SearchTerm == SearchValue) {
                    return false;
                }
                break;
            case 'Apply':
                StockRegisterFGReportViewModel.ProductType = $('#Type').val();               

                break;
            case 'Export':
                DataTablePagingViewModel.Length = -1;
                StockRegisterFGReportViewModel.DataTablePaging = DataTablePagingViewModel;
                StockRegisterFGReportViewModel.SearchTerm = $('#SearchTerm').val() == "" ? null : $('#SearchTerm').val();
                StockRegisterFGReportViewModel.ProductType = $('#Type').val() == "" ? null : $('#Type').val();
                $('#AdvanceSearch').val(JSON.stringify(StockRegisterFGReportViewModel));
                $('#FormExcelExport').submit();
                return true;
                break;
            default:
                break;
        }
        StockRegisterFGReportViewModel.DataTablePaging = DataTablePagingViewModel;
        StockRegisterFGReportViewModel.SearchTerm = $('#SearchTerm').val();
        StockRegisterFGReportViewModel.ProductType = $('#Type').val();
      
        DataTables.StockRegisterFGList = $('#tblStockRegisterFGReport').DataTable(

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
                    url: "GetStockRegisterFGReport/",
                    data: { "stockRegisterFGReportVM": StockRegisterFGReportViewModel },
                    type: 'POST'
                },
                pageLength: 15,
                columns: [                   
                    { "data": "ProductType", "defaultContent": "<i>-</i>" },
                    { "data": "Description", "defaultContent": "<i>-</i>" },
                    { "data": "UnitCode", "defaultContent": "<i>-</i>" },
                    { "data": "CurrentStock", "defaultContent": "<i>-</i>" },                  
                    
                    { "data": "CostPrice", "defaultContent": "<i>-</i>" },
                   { "data": "CostAmount", "defaultContent": "<i>-</i>" },
                    { "data": "SellingRate", "defaultContent": "<i>-</i>" },
                    { "data": "SellingAmount", "defaultContent": "<i>-</i>" }
                



                ],
                columnDefs: [{ "targets": [0], "visible": false, "searchable": false },
                    { className: "text-left", "targets": [2] },
                    { className: "text-center", "targets": [3] },
                    { className: "text-right", "targets": [4, 5, 6, 7] },

                    


                    {
                        targets: [0],
                        render: function (data, type, row) {
                            switch (data) {
                                case 'COM': return 'Component'; break;
                                case 'PRO': return 'Product'; break;
                                case 'SUB': return 'Sub Component'; break;
                                default: return '';
                            }
                        }
                    }

                   

                ],

                destroy: true,
                //for performing the import operation after the data loaded
                initComplete: function (settings, json) {
                    debugger;
                    $('.dataTables_wrapper div.bottom div').addClass('col-md-6');
                    $('#tblStockRegisterFGReport').fadeIn('slow');
                    if (action == undefined) {
                        $('.excelExport').hide();
                        OnServerCallComplete();
                    }

                    var totalCostAmount = json.data[0].StockCostAmount;
                    var totalSellingAmount = json.data[0].StockSellingAmount;
                    $('#stockcostamount').text("₹"+(totalCostAmount));
                    $('#stocksellingamount').text("₹" + (totalSellingAmount));                   
                },
            
               

                // grouping with  columns
                drawCallback: function (settings) {
                    var api = this.api();
                    var rows = api.rows({ page: 'current' }).nodes();
                    var last = null;
                 
                    api.column(0, { page: 'current' }).data().each(function (group, i) {
                        if (group == 'SUB') {
                            group = 'Sub Component';
                        }
                        else if (group == 'PRO')
                        {
                            group = 'Product';
                        }
                        else
                        {
                            group = 'Component';
                        }
                        if (last !== group) {
                          
                            var rowData = api.row(i).data();
                            $(rows).eq(i).before('<tr class="group "><td colspan="8" class="rptGrp">' + '<b>Stock Type</b> : ' + group + '</td></tr>');
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
    BindOrReloadStockRegisterFGTable('Reset');
}

//function export data to excel
function ExportReportData() {
    BindOrReloadStockRegisterFGTable('Export');
}

