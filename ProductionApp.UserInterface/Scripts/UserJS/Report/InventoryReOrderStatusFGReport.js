﻿//*****************************************************************************
//*****************************************************************************
//Author:Sruthi
//CreatedDate: 19-May-2018 
//LastModified: 22-May-2018 
//FileName: InventoryReOrderStatusFGReport.js
//Description: Client side coding for Inventory ReOrder Status FG Report
//******************************************************************************
//******************************************************************************
//Global Declaration
var DataTables = {};
var EmptyGuid = "00000000-0000-0000-0000-000000000000";
$(document).ready(function () {
    debugger;
    try
    {     
        //$('#Product').select2({
        //});
        BindOrReloadInventoryReorderFGTable('Init');
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

//bind inventory reOrder Status FG report
function
    BindOrReloadInventoryReorderFGTable(action)
{
    try
    {
        debugger;
        //creating advancesearch object       
        InventoryReOrderStatusFGReportViewModel = new Object();
        DataTablePagingViewModel = new Object();
        DataTablePagingViewModel.Length = 0;


        //switch case to check the operation
        switch (action) {
            case 'Reset':
                $('#SearchTerm').val('');
                $('#Status').val('');               
                $('#Type').val('PRO');
                break;
            case 'Init':
                break;
            case 'Search':
                break;
            case 'Apply':
                InventoryReOrderStatusFGReportViewModel.ItemStatus = $('#Status').val();             
                InventoryReOrderStatusFGReportViewModel.ProductType = $("#Type").val();
                break;
            case 'Export':
                DataTablePagingViewModel.Length = -1;
                break;
            default:
                break;
        }
        InventoryReOrderStatusFGReportViewModel.DataTablePaging = DataTablePagingViewModel;
        InventoryReOrderStatusFGReportViewModel.SearchTerm = $('#SearchTerm').val();
        InventoryReOrderStatusFGReportViewModel.ItemStatus = $('#Status').val();      
        InventoryReOrderStatusFGReportViewModel.ProductType = $("#Type").val();
        DataTables.InventoryReOrderFGList = $('#tblInventoryReOrderStatusFGReport').DataTable(

            {

                dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
                buttons: [{
                    extend: 'excel',
                    exportOptions:
                                 {
                                     columns: [0,1, 2, 3, 4, 5, 6]
                                 }
                }],
                //fixedHeader: {
                //header: true
                //},
                order: false,
                ordering: false,
                searching: false,
                paging: true,
                lengthChange: false,
                proccessing: true,
                serverSide: true,
                ajax: {
                    url: "GetInventoryReOrderStatusFGReport/",
                    data: { "inventoryReOrderStatusFGVM": InventoryReOrderStatusFGReportViewModel },
                    type: 'POST'
                },
                pageLength: 15,
                columns: [
                  //  { "data": "ID", "defaultContent": "<i>-</i>" },
                    { "data": "Description", "defaultContent": "<i>-</i>" },
                    { "data": "ProductType", "defaultContent": "<i>-</i>" },
                    { "data": "CurrentStock", "defaultContent": "<i>-</i>" },
                    { "data": "SalesOrderDueQty", "defaultContent": "<i>-</i>" },
                    { "data": "NetAvailableQty", "defaultContent": "<i>-</i>" },
                    { "data": "ReorderQty", "defaultContent": "<i>-</i>" },
                    { "data": "ShortFall", "defaultContent": "<i>-</i>" }


                ],
                columnDefs: [//{ "targets": [0], "visible": false, "searchable": false },
                    { className: "text-left", "targets": [0,1] },
                    { className: "text-center", "targets": [2,3, 4, 5, 6] },
                    //{ className: "text-right", "targets": [4, 5, 6, 7, 8, 9, 10, 11] }

                     {
                         targets: [1],
                         render: function (data, type, row) {
                             switch (data) {
                                 case 'COM': return 'Component'; break;
                                 case 'PRO': return 'Product'; break;
                                 case 'SUB': return 'SubComponent'; break;
                                 default: return '-';
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
                        BindOrReloadInventoryReorderFGTable('Search');
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
    BindOrReloadInventoryReorderFGTable('Reset');
}

//function export data to excel
function ExportReportData() {
    BindOrReloadInventoryReorderFGTable('Export');
}

