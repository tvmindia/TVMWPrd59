var DataTables = {};
var EmptyGuid = "00000000-0000-0000-0000-000000000000";
$(document).ready(function () {
    debugger;
    try {
        $("#RequisitionBy").select2({
        });
        BindOrReloadRequisitionTable('Init');

        $('#tblRequisition tbody').on('dblclick', 'td', function () {
            Edit(this);
        });
    }
    catch (e) {
        console.log(e.message);
    }
});

function Edit (curObj)
{
    debugger;
    var rowData = DataTables.RequisitionList.row($(curObj).parents('tr')).data();
    window.location.replace("NewRequisition?code=PURCH&ID=" + rowData.ID);

}
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
                $('#FromDate').val('');
                $('#ToDate').val('');
                $('#ReqStatus').val('');
                $("#RequisitionBy").val('').select2();
                break;
            case 'Init':
                break;
            case 'Search':
                break;
            case 'Apply':
                RequisitionAdvanceSearchViewModel.FromDate = $('#FromDate').val();
                RequisitionAdvanceSearchViewModel.ToDate = $('#ToDate').val();
                RequisitionAdvanceSearchViewModel.ReqStatus = $('#ReqStatus').val();
                RequisitionAdvanceSearchViewModel.EmployeeID = $('#RequisitionBy').val();
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
                ordering: false,
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
                    { "data": "ReqNo", "defaultContent": "<i>-</i>", "width": "10%" },
                    { "data": "Title", "defaultContent": "<i>-</i>", "width": "45%" },
                    { "data": "ReqDateFormatted", "defaultContent": "<i>-</i>", "width": "5%" },
                    { "data": "ReqStatus", "defaultContent": "<i>-</i>", "width": "5%" },
                    { "data": "RequisitionBy", "defaultContent": "<i>-</i>", "width": "10%" },
                    { "data": "ApprovalStatus", "defaultContent": "<i>-</i>", "width": "15%" },
                     {
                         "data": "ID", "orderable": false, render: function (data, type, row) {
                             return '<a href="/Requisition/NewRequisition?code=PURCH&ID=' + data + '" class="actionLink" ><i class="glyphicon glyphicon-edit" aria-hidden="true"></i></a>'
                         }, "defaultContent": "<i>-</i>","width":"3%"
                     }
                ],
                columnDefs: [{ "targets": [0], "visible": false, "searchable": false },
                    { className: "text-left", "targets": [1,2,4,5,6] },
                    { className: "text-center", "targets": [3,7] }],
                destroy: true,
                //for performing the import operation after the data loaded
                initComplete: function (settings, json) {
                    if (action === 'Export') {
                        if (json.data.length > 0) {
                            if (json.data[0].TotalCount > 10000) {
                                notyAlert("info", 'We are able to download maximum 10000 rows of data, There exist more than 10000 rows of data please filter and download')
                            }
                        }
                        $(".buttons-excel").trigger('click');
                        BindOrReloadRequisitionTable('Search');
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