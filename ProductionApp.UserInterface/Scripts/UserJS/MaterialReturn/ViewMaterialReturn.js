//*****************************************************************************
//*****************************************************************************
//Author: Angel
//CreatedDate: 21-Mar-2018 
//LastModified: 21-Mar-2018 
//FileName: ViewMaterialReturn.js
//Description: Client side coding for ViewMaterialReturn.
//******************************************************************************
//******************************************************************************

//--Global Declaration--//
var _dataTable = {};
var _emptyGuid = "00000000-0000-0000-0000-000000000000";
$(document).ready(function () {
    debugger;
    try {
        $("#ReturnBy").select2({
        });
        $("#SupplierID").select2({
        });
        BindOrReloadReturnToSupplierTable('Init');
        $('#tblReturnToSupplier tbody').on('dblclick', 'td', function () {
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
    var rowData = _dataTable.ReturnToSupplier.row($(curObj).parents('tr')).data();
    window.location.replace("NewMaterialReturn?code=STR&ID=" + rowData.ID);

}
function BindOrReloadReturnToSupplierTable(action) {
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
                $('#SupplierID').val('').select2();
                $('#ReturnBy').val('').select2();
                break;
            case 'Init':
                break;
            case 'Search':
                break;
            case 'Apply':
                materialReturnAdvanceSearchVM.FromDate = $('#FromDate').val();
                materialReturnAdvanceSearchVM.ToDate = $('#ToDate').val();
                materialReturnAdvanceSearchVM.SupplierID = $('#SupplierID').val();
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
        materialReturnAdvanceSearchVM.SupplierID = $('#SupplierID').val();
        materialReturnAdvanceSearchVM.ReturnBy = $('#ReturnBy').val();
        _dataTable.ReturnToSupplier = $('#tblReturnToSupplier').DataTable(
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
                    url: "GetAllReturnToSupplier/",
                    data: { "materialReturnAdvanceSearchVM": materialReturnAdvanceSearchVM },
                    type: 'POST'
                },
                pageLength: 10,
                columns: [
                    { "data": "ID", "defaultContent": "<i>-</i>" },
                    { "data": "ReturnSlipNo", "defaultContent": "<i>-</i>", "width": "10%" },
                    { "data": "ReturnDateFormatted", "defaultContent": "<i>-</i>", "width": "10%" },
                    { "data": "ReturnByEmployeeName", "defaultContent": "<i>-</i>", "width": "38%" },
                    { "data": "SupplierName", "defaultContent": "<i>-</i>", "width": "39%" },
                    {
                        "data": "ID", "orderable": false, render: function (data, type, row) {
                            return '<a href="/MaterialReturn/NewMaterialReturn?code=STR&ID=' + data + '" class="actionLink" ><i class="glyphicon glyphicon-edit" aria-hidden="true"></i></a>'
                        }, "defaultContent": "<i>-</i>", "width": "3%"
                    }
                ],
                columnDefs: [{ "targets": [0], "visible": false, "searchable": false },
                            { className: "text-left", "targets": [1, 3, 4] },
                            { className: "text-center", "targets": [2,5] }],
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
                        BindOrReloadReturnToSupplierTable('Search');
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
function ResettblReturnToSupplier() {
    BindOrReloadReturnToSupplierTable('Reset');
}
//function export data to excel
function ImportReturnToSupplierData() {
    BindOrReloadReturnToSupplierTable('Export');
}