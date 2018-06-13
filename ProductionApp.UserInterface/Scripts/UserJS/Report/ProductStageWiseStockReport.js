//*****************************************************************************
//*****************************************************************************
//Author:Sruthi
//CreatedDate: 11-Jun-2018 
//LastModified: 11-Jun-2018 
//FileName: ProductStageWiseStockReport.js
//Description: Client side coding for Product StageWise Stock Report
//******************************************************************************
//******************************************************************************
//Global Declaration
var DataTables = {};
var EmptyGuid = "00000000-0000-0000-0000-000000000000";
$(document).ready(function () {
    debugger;
    try {
        $('#Product').select2({
        });
        
        if (($("#Product").val() == "") || ($("#Product").val() == "00000000-0000-0000-0000-000000000000"))
        {
            $('#lblMessage').visible = true;
        }
      else
        {
            $('#lblMessage').hide();
        }
        BindOrReloadProductstagewiseStockTable('Init');
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

//bind inventory reOrder Status FG report
function 
    BindOrReloadProductstagewiseStockTable(action) {
    try {
        debugger;
        //creating advancesearch object       
        ProductStageWiseStockReportViewModel= new Object();
        DataTablePagingViewModel = new Object();
        DataTablePagingViewModel.Length = 0;


        //switch case to check the operation
        switch (action) {
            case 'Reset':
                $('#SearchTerm').val('');
                $('#Product').val('').select2();
              
                break;
            case 'Init':
                break;
            case 'Search':
                break;
            case 'Apply':                
                ProductStageWiseStockReportViewModel.ProductID = $("#Product").val();
                $('#lblMessage').hide();
                break;
            case 'Export':
                DataTablePagingViewModel.Length = -1;
                break;
            default:
                break;
        }
        ProductStageWiseStockReportViewModel.DataTablePaging = DataTablePagingViewModel;
        ProductStageWiseStockReportViewModel.SearchTerm = $('#SearchTerm').val();
        ProductStageWiseStockReportViewModel.ProductID = $('#Product').val();
        //$('#lblMessage').hide();
        DataTables.ProductStagewiseList = $('#tblProductStagewiseStockReport').DataTable(

            {

                dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
                buttons: [{
                    extend: 'excel',
                    exportOptions:
                                 {
                                     columns: [1, 2, 3]
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
                    url: "GetProductStageWiseStockReport/",
                    data: { "productStageWiseStockReportVM": ProductStageWiseStockReportViewModel },
                    type: 'POST'
                },
                pageLength: 15,
                columns: [
                   { "data": "ID", "defaultContent": "<i>-</i>" },
                    { "data": "Description", "defaultContent": "<i>-</i>" },
                    { "data": "Stage", "defaultContent": "<i>-</i>" },
                    { "data": "CurrentStock", "defaultContent": "<i>-</i>" }               


                ],
                columnDefs: [
                    { "targets": [0], "visible": false, "searchable": false },
                    { className: "text-left", "targets": [1, 2] },
                    { className: "text-center", "targets": [3] },
                    { width: "40%", "targets": [1] },
                    { width: "30%", "targets": [2] },
                    { width: "30%", "targets": [3] },


                   // { className: "text-center", "targets": [3, 4, 5, 6, 7] },
                    //{ className: "text-right", "targets": [4, 5, 6, 7, 8, 9, 10, 11] }

                   
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
                        BindOrReloadProductstagewiseStockTable('Search');
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
    BindOrReloadProductstagewiseStockTable('Reset');
}

//function export data to excel
function ExportReportData() {
    BindOrReloadProductstagewiseStockTable('Export');
}

