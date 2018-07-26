//*****************************************************************************
//*****************************************************************************
//Author:Sruthi
//CreatedDate: 14-May-2018 
//LastModified: 14-May-2018 
//FileName: InventoryReOrderStatusReport.js
//Description: Client side coding for Inventory ReOrder Status Report
//******************************************************************************
//******************************************************************************
//Global Declaration
var DataTables = {};
var EmptyGuid = "00000000-0000-0000-0000-000000000000";
$(document).ready(function () {
    debugger;
    try {
        $("#Material,#MaterialType").select2({
        });
        BindOrReloadInventoryReorderTable('Init');

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

//bind inventory reOrder Status report
function 
    BindOrReloadInventoryReorderTable(action) {
    try {
        debugger;
        //creating advancesearch object       
        InventoryReorderStatusReportViewModel = new Object();
        DataTablePagingViewModel = new Object();
        DataTablePagingViewModel.Length = 0;


        //switch case to check the operation
        switch (action) {
            case 'Reset':
                $('#SearchTerm').val('');                   
                $('#Status').val('');
                $('#Material').val('').select2();
                $('#MaterialType').val('').select2();
                break;
            case 'Init':
                break;
            case 'Search':
                break;
            case 'Apply':               
                InventoryReorderStatusReportViewModel.ItemStatus = $('#Status').val();
                InventoryReorderStatusReportViewModel.MaterialID = $('#Material').val();
                InventoryReorderStatusReportViewModel.Code = $('#MaterialType').val();
                break;
            case 'Export':
                DataTablePagingViewModel.Length = -1;
                break;
            default:
                break;
        }
        InventoryReorderStatusReportViewModel.DataTablePaging = DataTablePagingViewModel;
        InventoryReorderStatusReportViewModel.SearchTerm = $('#SearchTerm').val();      
        InventoryReorderStatusReportViewModel.ItemStatus = $('#Status').val();
        InventoryReorderStatusReportViewModel.MaterialID = $('#Material').val();
        InventoryReorderStatusReportViewModel.Code = $('#MaterialType').val();

        DataTables.InventoryReOrderList = $('#tblInventoryReOrderStatusReport').DataTable(

            {

                dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
                buttons: [{
                    extend: 'excel',
                    exportOptions:
                                 {
                                     columns: [0,1, 2, 3, 4, 5, 6]
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
                    url: "GetInventoryReorderStatusReport/",
                    data: { "inventoryReorderStatusVM": InventoryReorderStatusReportViewModel },
                    type: 'POST'
                },
                pageLength: 15,
                columns: [
                   // { "data": "ID", "defaultContent": "<i>-</i>" },
                    { "data": "Description", "defaultContent": "<i>-</i>"},
                    { "data": "CurrentStock", "defaultContent": "<i>-</i>"},
                    { "data": "PODueQty", "defaultContent": "<i>-</i>"},
                    { "data": "NetAvailableQty", "defaultContent": "<i>-</i>" },
                    { "data": "ReorderQty", "defaultContent": "<i>-</i>" },
                    { "data": "ShortFall", "defaultContent": "<i>-</i>" },
                    { "data": "ShortFall", "defaultContent": "<i>-</i>"}
                   

                ],
                columnDefs: [//{ "targets": [0], "visible": false, "searchable": false },
                    { className: "text-left", "targets": [0] },
                    { className: "text-center", "targets": [1,2,3,4,5,6] }

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
                        BindOrReloadInventoryReorderTable('Search');
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
    BindOrReloadInventoryReorderTable('Reset');
}

//function export data to excel
function ExportReportData() {
    BindOrReloadInventoryReorderTable('Export');
}

//function to filter data based on date
function DateFilterOnchange() {
    BindOrReloadInventoryReorderTable('Apply');
}