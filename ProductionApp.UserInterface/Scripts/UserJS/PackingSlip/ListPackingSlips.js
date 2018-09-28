//*****************************************************************************
//*****************************************************************************
//Author: Angel
//CreatedDate: 13-Mar-2018 
//LastModified: 20-Mar-2018 
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
        $('#tblPaySlip tbody').on('dblclick', 'td', function () {
            Edit(this);
        });
    }
    catch (e) {
        console.log(e.message);
    }
});
//edit on table click
function Edit(curObj) {
    debugger;
    var rowData = DataTables.PaySlipList.row($(curObj).parents('tr')).data();
    window.location.replace("AddPackingSlip?code=SALE&ID=" + rowData.ID);

}
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
                                     columns: [1, 2, 3, 4,5,6,7]
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
                    { "data": "SlipNo", "defaultContent": "<i>-</i>","width":"5%" },
                    { "data": "DateFormatted", "defaultContent": "<i>-</i>", "width": "8%" },
                    {
                        "data": "SalesOrder.CustomerName",render: function (data, type, row) {
                            row.SalesOrder.OrderNo = row.SalesOrder.OrderNo == null ? " " : row.SalesOrder.OrderNo
                            return '<b>By: </b>' + row.PackedByEmployeeName + '</br><b>Customer: </b>' + row.SalesOrder.CustomerName + '</br><b>OrderNo: </b>' + row.SalesOrder.OrderNo
                        },
                        "defaultContent": "<i>-</i>","width":"15%",
                    },
                   
                    {
                        "data": "DispatchedDateFormatted", render: function (data, type, row) {
                            row.DispatchedDateFormatted = row.DispatchedDateFormatted == null ? " " : row.DispatchedDateFormatted
                            row.DispatchedByEmployeeName = row.DispatchedByEmployeeName == null ? " " : row.DispatchedByEmployeeName
                            row.DispatchRemarks = row.DispatchRemarks == null ? " " : row.DispatchRemarks
                        return '<b>Date: </b>' + row.DispatchedDateFormatted + '</br><b>By: </b>' + row.DispatchedByEmployeeName + '</br><b>Remarks: </b>' + row.DispatchRemarks
                        }, "defaultContent": "<i>-</i>", "width": "15%"
                    },
                     { "data": "CheckedPackageWeight", "defaultContent": "<i>-</i>", "width": "10%" },
                    {
                        "data": "VehiclePlateNo", render: function (data, type, row) {
                            row.VehiclePlateNo = row.VehiclePlateNo == null ? " " : row.VehiclePlateNo
                            row.DriverName = row.DriverName == null ? " " : row.DriverName
                            return '<b>PlateNo: </b>' + row.VehiclePlateNo + '</br><b>Driver: </b>' + row.DriverName
                        }, "defaultContent": "<i>-</i>", "width": "15%"
                    },
                    {
                        "data": "ReceivedBy", render: function (data, type, row) {
                            row.ReceivedBy = row.ReceivedBy == null ? " " : row.ReceivedBy
                            row.ReceivedDateFormatted = row.ReceivedDateFormatted == null ? " " : row.ReceivedDateFormatted
                            return '<b>By: </b>' + row.ReceivedBy + '</br><b>Date: </b>' + row.ReceivedDateFormatted
                        }, "defaultContent": "<i>-</i>", "width": "15%"
                    },
                    {
                        "data": "ID", "orderable": false, render: function (data, type, row) {
                            return '<a href="/PackingSlip/AddPackingSlip?code=SALE&ID=' + data + '" class="actionLink" ><i class="glyphicon glyphicon-edit" aria-hidden="true"></i></a>'
                        }, "defaultContent": "<i>-</i>", "width": "3%"
                    }
                ],
                columnDefs: [{ "targets": [0], "visible": false, "searchable": false },
                            { className: "text-left", "targets": [1, 3,4,7] },
                            { className: "text-right", "targets": [5] },
                            { className: "text-center", "targets": [2,8] }],
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
                        BindOrReloadPackingSlipTable('Search');
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
function ResetPackingSlipList() {
    BindOrReloadPackingSlipTable('Reset');
}
//function export data to excel
function ImportPackingSlipData() {
    BindOrReloadPackingSlipTable('Export');
}
