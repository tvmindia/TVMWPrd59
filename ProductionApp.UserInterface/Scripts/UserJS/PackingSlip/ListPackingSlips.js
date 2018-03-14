//*****************************************************************************
//*****************************************************************************
//Author: Angel
//CreatedDate: 13-Mar-2018 
//LastModified: 13-Mar-2018 
//FileName: ListPackingSlips.js
//Description: Client side coding for ListPackingSlips.
//******************************************************************************
//******************************************************************************

//--Global Declaration--//
var DataTables = {};
var EmptyGuid = "00000000-0000-0000-0000-000000000000";
$(document).ready(function () {
    debugger;
    try {
        $("#PackedBy").select2({
        });
        $("#DispatchedBy").select2({
        });
        BindOrReloadPackingSlipTable('Init');
    }
    catch (e) {
        console.log(e.message);
    }
});

function BindOrReloadPackingSlipTable(action)
{
    try {
        debugger;
        packingSlipAdvanceSearchVM = new Object();
        DataTablePagingViewModel = new Object();
        DataTablePagingViewModel.Length = 0;
        //switch case to check the operation
        switch (action) {
            case 'Reset':
                $('#SearchTerm').val('');
                $('#PackingFromDate').val('');
                $('#PackingToDate').val('');
                $('#DispatchedFromDate').val('');
                $('#DispatchedToDate').val('');
                $('#PackedBy').val('').select2();
                $('#DispatchedBy').val('').select2();
                break;
            case 'Init':
                break;
            case 'Search':
                break;
            case 'Apply':
                packingSlipAdvanceSearchVM.PackingFromDate = $('#PackingFromDate').val();
                packingSlipAdvanceSearchVM.PackingToDate = $('#PackingToDate').val();
                packingSlipAdvanceSearchVM.DispatchedFromDate = $('#DispatchedFromDate').val();
                packingSlipAdvanceSearchVM.DispatchedToDate = $('#DispatchedToDate').val();
                packingSlipAdvanceSearchVM.PackedBy = $('#PackedBy').val();
                packingSlipAdvanceSearchVM.DispatchedBy = $('#DispatchedBy').val();
                break;
            case 'Export':
                DataTablePagingViewModel.Length = -1;
                break;
            default:
                break;
        }
        packingSlipAdvanceSearchVM.DataTablePaging = DataTablePagingViewModel;
        packingSlipAdvanceSearchVM.SearchTerm = $('#SearchTerm').val();
        packingSlipAdvanceSearchVM.PackingFromDate = $('#PackingFromDate').val();
        packingSlipAdvanceSearchVM.PackingToDate = $('#PackingToDate').val();
        packingSlipAdvanceSearchVM.DispatchedFromDate = $('#DispatchedFromDate').val();
        packingSlipAdvanceSearchVM.DispatchedToDate = $('#DispatchedToDate').val();
        packingSlipAdvanceSearchVM.PackedBy = $('#PackedBy').val();
        packingSlipAdvanceSearchVM.DispatchedBy = $('#DispatchedBy').val();
        DataTables.PaySlipList = $('#tblPaySlip').DataTable(
            {
                dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
                buttons: [{
                    extend: 'excel',
                    exportOptions:
                                 {
                                     columns: [1, 2, 3, 4,5,6,7,8,9,10,11,12]
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
                    url: "GetAllPackingSlip/",
                    data: { "packingSlipAdvanceSearchVM": packingSlipAdvanceSearchVM },
                    type: 'POST'
                },
                pageLength: 10,
                columns: [
                    { "data": "ID", "defaultContent": "<i>-</i>" },
                    { "data": "SlipNo", "defaultContent": "<i>-</i>" },
                    { "data": "DateFormatted", "defaultContent": "<i>-</i>" },
                    { "data": "PackedByEmployeeName", "defaultContent": "<i>-</i>" },
                    { "data": "SalesOrderID", "defaultContent": "<i>-</i>" },
                    { "data": "CheckedPackageWeight", "defaultContent": "<i>-</i>" },
                    { "data": "DispatchedDateFormatted", "defaultContent": "<i>-</i>" },
                    { "data": "DispatchedByEmployeeName", "defaultContent": "<i>-</i>" },
                    { "data": "VehiclePlateNo", "defaultContent": "<i>-</i>" },
                    { "data": "DriverName", "defaultContent": "<i>-</i>" },
                    { "data": "ReceivedBy", "defaultContent": "<i>-</i>" },
                    { "data": "ReceivedDateFormatted", "defaultContent": "<i>-</i>" },
                    { "data": "DispatchRemarks", "defaultContent": "<i>-</i>" },
                    {
                        "data": "ID", "orderable": false, render: function (data, type, row) {
                            return '<a href="/PackingSlip/AddPackingSlip?code=SALE&ID=' + data + '" class="actionLink" ><i class="glyphicon glyphicon-share-alt" aria-hidden="true"></i></a>'
                        }, "defaultContent": "<i>-</i>", "width": "3%"
                    }
                ],
                columnDefs: [{ "targets": [0], "visible": false, "searchable": false },
                            { className: "text-left", "targets": [1, 3, 4,5,7,8,9,10,12] },
                            { className: "text-center", "targets": [2,6,11] }],
                destroy: true,
                //for performing the import operation after the data loaded
                //initComplete: function (settings, json) {
                //    if (action === 'Export') {
                //        debugger;
                //        if (json.data[0].length > 0) {
                //            if (json.data[0].TotalCount > 10000) {
                //                MasterAlert("info", 'We are able to download maximum 10000 rows of data, There exist more than 10000 rows of data please filter and download')
                //            }
                //        }
                //        $(".buttons-excel").trigger('click');
                //        BindOrReloadPaySlipTable('Search');
                //    }
                //}
            });
        $(".buttons-excel").hide();
    }

    catch (e) {
        console.log(e.message);
    }
}
//function reset the list to initial
function ResetPackingSlipList() {
    BindOrReloadPackingSlipTable('Reset');
}
//function export data to excel
function ImportPackingSlipData() {
    BindOrReloadPackingSlipTable('Export');
}
