var DataTables = {};
var EmptyGuid = "00000000-0000-0000-0000-000000000000";
$(document).ready(function () {
    debugger;
    try {
        BindOrReloadRequisitionTable('Init');
    }
    catch (e) {
        console.log(e.message);
    }
});
//bind purchae order list
function BindOrReloadRequisitionTable(action) {
    try {
        //creating advancesearch object
        debugger;
        RequisitionAdvanceSearchViewModel = new Object();
        DataTablePagingViewModel = new Object();
        DataTablePagingViewModel.Length = 0;
        //switch case to check the operation
        switch (action) {
            case 'Reset':
                $('#SearchTerm').val('');
                break;
            case 'Init':
                break;
            case 'Search':
                break;
            case 'Export':
                DataTablePagingViewModel.Length = -1;
                break;
            default:
                break;
        }
        RequisitionAdvanceSearchViewModel.DataTablePaging = DataTablePagingViewModel;
        RequisitionAdvanceSearchViewModel.SearchTerm = $('#SearchTerm').val();
        DataTables.RequisitionList = $('#tblRequisition').DataTable(
            {
                dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
                buttons: [{
                    extend: 'excel',
                    exportOptions:
                                 {
                                     columns: [1, 2, 3, 4, 5, 6]
                                 }
                }],
                order: false,
                searching: false,
                paging: true,
                lengthChange: false,
                proccessing: true,
                serverSide: true,
                ajax: {
                    url: "GetAllRequisition/",
                    data: { "requisitionAdvanceSearchVM": RequisitionAdvanceSearchViewModel },
                    type: 'POST'
                },
                pageLength: 10,
                columns: [
                    { "data": "ID", "defaultContent": "<i>-</i>" },
                    { "data": "ReqNo", "defaultContent": "<i>-</i>" },
                    { "data": "Title", "defaultContent": "<i>-</i>" },
                    { "data": "ReqDateFormatted", "defaultContent": "<i>-</i>" },
                    { "data": "ReqStatus", "defaultContent": "<i>-</i>" },
                    { "data": "RequisitionBy", "defaultContent": "<i>-</i>" },
                    { "data": "ApprovalStatus", "defaultContent": "<i>-</i>" },
                     {
                         "data": "ID", "orderable": false, render: function (data, type, row) {
                             return '<a href="/Requisition/NewRequisition?code=PURCH&ID=' + data + '" class="actionLink" ><i class="glyphicon glyphicon-share-alt" aria-hidden="true"></i></a>'
                         }, "defaultContent": "<i>-</i>"
                     }
                ],
                columnDefs: [{ "targets": [0], "visible": false, "searchable": false },
                    { className: "text-left", "targets": [1, 4, 5,6] },
                    { className: "text-center", "targets": [2, 3,7] }],
                destroy: true,
                //for performing the import operation after the data loaded
                initComplete: function (settings, json) {
                    if (action === 'Export') {
                        $(".buttons-excel").trigger('click');
                        ResetRequisitionList();
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
function ResetRequisitionList() {
    BindOrReloadRequisitionTable('Reset');
}
//function export data to excel
function ImportRequisitionData() {
    BindOrReloadRequisitionTable('Export');
}