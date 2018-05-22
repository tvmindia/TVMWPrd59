//*****************************************************************************
//*****************************************************************************
//Author:Sruthi
//CreatedDate: 22-May-2018 
//LastModified: 22-May-2018 
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


        //switch case to check the operation
        switch (action) {
            case 'Reset':
                $('#SearchTerm').val('');
                $('#Type').val('');               
                break;
            case 'Init':
                break;
            case 'Search':
                break;
            case 'Apply':
                StockRegisterFGReportViewModel.ProductType = $('#Type').val();               

                break;
            case 'Export':
                DataTablePagingViewModel.Length = -1;
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
                buttons: [{
                    extend: 'excel',
                    exportOptions:
                                 {
                                     columns: [1, 2, 3, 4, 5, 6,7,8]
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
                    url: "GetStockRegisterFGReport/",
                    data: { "stockRegisterFGReportVM": StockRegisterFGReportViewModel },
                    type: 'POST'
                },
                pageLength: 15,
                columns: [
                    { "data": "ID", "defaultContent": "<i>-</i>" },
                    { "data": "ProductType", "defaultContent": "<i>-</i>" },
                    { "data": "Description", "defaultContent": "<i>-</i>" },
                    { "data": "UnitCode", "defaultContent": "<i>-</i>" },
                    { "data": "CurrentStock", "defaultContent": "<i>-</i>" },                  
                    
                    { "data": "CostPrice", "defaultContent": "<i>-</i>" },
                   { "data": "CostAmount", "defaultContent": "<i>-</i>" },
                    { "data": "SellingRate", "defaultContent": "<i>-</i>" },
                    { "data": "SellingAmount", "defaultContent": "<i>-</i>" }



                ],
                columnDefs: [{ "targets": [0,1], "visible": false, "searchable": false },
                    { className: "text-left", "targets": [2] },
                    { className: "text-center", "targets": [3] },
                    { className: "text-right", "targets": [4,5,6,7,8] },
                    {
                        targets: [1],
                        render: function (data, type, row) {
                            switch (data) {
                                case 'COM': return 'Component'; break;
                                case 'PRO': return 'Product'; break;
                                case 'SUB': return 'SubComponent'; break;
                                default: return '';
                            }
                        }
                    }

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
                        BindOrReloadStockRegisterFGTable('Search');
                    }
                },
                // grouping with  columns
                drawCallback: function (settings) {
                    var api = this.api();
                    var rows = api.rows({ page: 'current' }).nodes();
                    var last = null;
                 
                    api.column(1, { page: 'current' }).data().each(function (group, i) {
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
                            debugger;
                           
                            var rowData = api.row(i).data();
                            $(rows).eq(i).before('<tr class="group "><td colspan="8" class="rptGrp">' + '<b>Stock Type</b> : ' + group + '</td></tr>');
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
    BindOrReloadStockRegisterFGTable('Reset');
}

//function export data to excel
function ExportReportData() {
    BindOrReloadStockRegisterFGTable('Export');
}

