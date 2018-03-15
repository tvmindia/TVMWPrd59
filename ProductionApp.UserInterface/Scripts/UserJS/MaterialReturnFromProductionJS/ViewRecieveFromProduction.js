//*****************************************************************************
//*****************************************************************************
//Author: Angel
//CreatedDate: 08-Mar-2018 
//LastModified: 09-Mar-2018 
//FileName: ViewRecieveFromProduction.js
//Description: Client side coding for RecieveFromProduction.
//******************************************************************************
//******************************************************************************

//--Global Declaration--//
var DataTables = {};
var EmptyGuid = "00000000-0000-0000-0000-000000000000";
$(document).ready(function () {
    debugger;
    try {
        $("#ReceivedBy").select2({
        });
        $("#ReturnBy").select2({
        });
        BindOrReloadReturnFromProductionTable('Init');
    }
    catch (e) {
        console.log(e.message);
    }
});

function BindOrReloadReturnFromProductionTable(action)
{
        try {
            debugger;
            materialReturnAdvanceSearchVM = new Object();
            DataTablePagingViewModel = new Object();
            DataTablePagingViewModel.Length = 0;
            //switch case to check the operation
            switch (action) {
                case 'Reset':
                    $('#SearchTerm').val('');
                    $('#FromDate').val('');
                    $('#ToDate').val('');
                    $('#ReceivedBy').val('').select2();
                    $('#ReturnBy').val('').select2();
                    break;
                case 'Init':
                    break;
                case 'Search':
                    break;
                case 'Apply':
                    materialReturnAdvanceSearchVM.FromDate = $('#FromDate').val();
                    materialReturnAdvanceSearchVM.ToDate = $('#ToDate').val();
                    materialReturnAdvanceSearchVM.ReceivedBy = $('#ReceivedBy').val();
                    materialReturnAdvanceSearchVM.ReturnBy = $('#ReturnBy').val();
                    break;
                case 'Export':
                    DataTablePagingViewModel.Length = -1;
                    break;
                default:
                    break;
            }
            materialReturnAdvanceSearchVM.DataTablePaging = DataTablePagingViewModel;
            materialReturnAdvanceSearchVM.SearchTerm = $('#SearchTerm').val();
            materialReturnAdvanceSearchVM.FromDate = $('#FromDate').val();
            materialReturnAdvanceSearchVM.ToDate = $('#ToDate').val();
            materialReturnAdvanceSearchVM.ReceivedBy = $('#ReceivedBy').val();
            materialReturnAdvanceSearchVM.ReturnBy = $('#ReturnBy').val();
            DataTables.ReturnFromProductionList = $('#tblRecieveFromProduction').DataTable(
                {
                    dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
                    buttons: [{
                        extend: 'excel',
                        exportOptions:
                                     {
                                         columns: [1, 2, 3, 4]
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
                        url: "GetAllReturnFromProduction/",
                        data: { "materialReturnAdvanceSearchVM": materialReturnAdvanceSearchVM },
                        type: 'POST'
                    },
                    pageLength: 10,
                    columns: [
                        { "data": "ID", "defaultContent": "<i>-</i>" },
                        { "data": "ReturnNo", "defaultContent": "<i>-</i>" },
                        { "data": "ReturnDateFormatted", "defaultContent": "<i>-</i>" },
                        { "data": "ReturnToEmployeeName", "defaultContent": "<i>-</i>" },
                        { "data": "RecievedByEmployeeName", "defaultContent": "<i>-</i>" },
                        {
                            "data": "ID", "orderable": false, render: function (data, type, row) {
                                return '<a href="/MaterialReturnFromProduction/NewRecieveFromProduction?code=STR&ID=' + data + '" class="actionLink" ><i class="glyphicon glyphicon-share-alt" aria-hidden="true"></i></a>'
                            }, "defaultContent": "<i>-</i>", "width": "3%"
                        }
                    ],
                    columnDefs: [{ "targets": [0], "visible": false, "searchable": false },
                                { className: "text-left", "targets": [1, 3, 4] },
                                { className: "text-center", "targets": [2] }],
                    destroy: true,
                    //for performing the import operation after the data loaded
                    initComplete: function (settings, json) {
                        if (action === 'Export') {
                            debugger;
                            if (json.data[0].length > 0) {
                                if (json.data[0].TotalCount > 10000) {
                                    MasterAlert("info", 'We are able to download maximum 10000 rows of data, There exist more than 10000 rows of data please filter and download')
                                }
                            }
                            $(".buttons-excel").trigger('click');
                            BindOrReloadReturnFromProductionTable('Search');
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
function ResetReturnFromProductionList() {
    BindOrReloadReturnFromProductionTable('Reset');
}
//function export data to excel
function ImportReturnFromProductionData() {
    BindOrReloadReturnFromProductionTable('Export');
}
