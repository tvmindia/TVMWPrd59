var DataTables = {};
var EmptyGuid = "00000000-0000-0000-0000-000000000000";

$(document).ready(function () {
    debugger;
    try {
       
        BindOrReloadDocumetApprovals('Init');
    }
    catch (e) {
        console.log(e.message);
    }
});



//bind Pending list
function BindOrReloadDocumetApprovals(action) {
    try {
        //creating advancesearch object
        debugger;
        DocumentApprovalAdvanceSearchViewModel = new Object();
        DataTablePagingViewModel = new Object();
        DocumentTypeViewModel = new Object();

        DataTablePagingViewModel.Length = 0;
        //switch case to check the operation
        switch (action) {
            case 'Reset':
                $('#SearchTerm').val('');
                $('#FromDate').val('');
                $('#ToDate').val('');
                $('#DocumentTypeCode').val('');
                $('#ShowAll').val('false');
                break;
            case 'Init':
                break;
            case 'Apply':
                break;
            case 'Export':
                DataTablePagingViewModel.Length = -1;
                break;
            default:
                break;
        }
       
        DocumentApprovalAdvanceSearchViewModel.SearchTerm = $('#SearchTerm').val();
        DocumentApprovalAdvanceSearchViewModel.FromDate = $('#FromDate').val();
        DocumentApprovalAdvanceSearchViewModel.ToDate = $('#ToDate').val();
        DocumentTypeViewModel.Code = $('#DocumentTypeCode').val();      
        DocumentApprovalAdvanceSearchViewModel.ShowAll = $('#ShowAll').val();

        DocumentApprovalAdvanceSearchViewModel.DataTablePaging = DataTablePagingViewModel;
        DocumentApprovalAdvanceSearchViewModel.DocumentType = DocumentTypeViewModel;

        DataTables.PurchaseOrderList = $('#tblPendingDocuments').DataTable(
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
                    url: "GetAllDocumentApproval/",
                    data: { "documentApprovalAdvanceSearchVM": DocumentApprovalAdvanceSearchViewModel },
                    type: 'POST'
                },
                pageLength: 10,
                columns: [
                    { "data": "ApprovalLogID", "defaultContent": "<i>-</i>" },
                    { "data": "DocumentTypeCode", "defaultContent": "<i>-</i>" },
                    { "data": "DocumentType", "defaultContent": "<i>-</i>" },
                    { "data": "DocumentNo", "defaultContent": "<i>-</i>" },
                    { "data": "DocumentDateFormatted", "defaultContent": "<i>-</i>" },
                    { "data": "ApproverLevel", "defaultContent": "<i>-</i>" },
                    { "data": "DocumentCreatedBy", "defaultContent": "<i>-</i>" },
                    //{ "data": "DocumentCreatedDate", "defaultContent": "<i>-</i>" },
                    {
                        "data": "ApprovalLogID", "orderable": false, render: function (data, type, row) {
                            debugger;
                            return '<a href="/DocumentApproval/ApproveDocument?code=APR&ID=' + data + '&DocType=' + row.DocumentTypeCode + '&DocID=' + row.DocumentID +  '" class="actionLink" ><i class="glyphicon glyphicon-share-alt" aria-hidden="true"></i></a>'
                        }, "defaultContent": "<i>-</i>"
                    }
                ],
                columnDefs: [{ "targets": [0,1], "visible": false, "searchable": false },
                    { className: "text-left", "targets": [2,3,6] },
                    { className: "text-center", "targets": [4,5] }],
                destroy: true,
                //for performing the import operation after the data loaded
                initComplete: function (settings, json) {
                    if (action === 'Export') {
                        $(".buttons-excel").trigger('click');
                        ResetPurchaseOrderList();
                    }
                }
            });
        $(".buttons-excel").hide();
    }
    catch (e) {
        console.log(e.message);
    }
}


function ResetPendingDocList() {
    BindOrReloadDocumetApprovals('Reset');
}
//function export data to excel
function ExportPendingDocs() {
    BindOrReloadDocumetApprovals('Export');
}
