//*****************************************************************************
//*****************************************************************************
//Author: Arul
//CreatedDate: 26-Apr-2018
//LastModified: 
//FileName: ViewApprovalHistory.js
//Description: Client side coding for Viewing Document Approval History
//******************************************************************************
//******************************************************************************

var DataTables = {};
var EmptyGuid = "00000000-0000-0000-0000-000000000000";

$(document).ready(function () {
    debugger;
    try {

        BindOrReloadApprovalHistory('Init');
        $('#tblApprovalHistory tbody').on('dblclick', 'td', function () {
            Edit(this);
        });
    }
    catch (e) {
        console.log(e.message);
    }
});

function Edit(curObj) {
    debugger;
    var rowData = DataTables.PurchaseOrderList.row($(curObj).parents('tr')).data();
    window.location.replace("ApproveDocument?code=APR&ID=" + rowData.ApprovalLogID + '&DocType=' + rowData.DocumentTypeCode + '&DocID=' + rowData.DocumentID);
}

//bind Pending list
function BindOrReloadApprovalHistory(action) {
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
                $('#ApprovalStatus').val('--- Show All ---');
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
        DocumentApprovalAdvanceSearchViewModel.ApprovalStatus = $('#ApprovalStatus').val();

        DocumentApprovalAdvanceSearchViewModel.DataTablePaging = DataTablePagingViewModel;
        DocumentApprovalAdvanceSearchViewModel.DocumentType = DocumentTypeViewModel;

        DataTables.PurchaseOrderList = $('#tblApprovalHistory').DataTable(
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
                    url: "GetAllApprovalHistory/",
                    data: { "documentApprovalAdvanceSearchVM": DocumentApprovalAdvanceSearchViewModel },
                    type: 'POST'
                },
                pageLength: 10,
                columns: [
                    { "data": "ApprovalLogID", "defaultContent": "<i>-</i>" },
                    { "data": "DocumentTypeCode", "defaultContent": "<i>-</i>" },
                    { "data": "DocumentDateFormatted", "defaultContent": "<i>-</i>" },
                    { "data": "DocumentType", "defaultContent": "<i>-</i>" },
                    { "data": "DocumentNo", "defaultContent": "<i>-</i>" },
                    { "data": "ApproverLevel", "defaultContent": "<i>-</i>" },
                    { "data": "DocumentCreatedBy", "defaultContent": "<i>-</i>" },
                    { "data": "DocumentStatus", "defaultContent": "<i>-</i>" },
                    {
                        "data": "ApprovalLogID", "orderable": false, render: function (data, type, row) {
                            debugger;
                            return '<a href="/DocumentApproval/ApproveDocument?code=APR&ID=' + data + '&DocType=' + row.DocumentTypeCode + '&DocID=' + row.DocumentID + '" class="actionLink" ><i class="glyphicon glyphicon-share-alt" aria-hidden="true"></i></a>'
                        }, "defaultContent": "<i>-</i>"
                    }
                ],
                columnDefs: [{ "targets": [0, 1], "visible": false, "searchable": false },
                    { className: "text-left", "targets": [3, 4, 6, 7] },
                    { className: "text-center", "targets": [2, 5], "width": "10%" }],
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
    BindOrReloadApprovalHistory('Reset');
}
//function export data to excel
function ExportPendingDocs() {
    BindOrReloadApprovalHistory('Export');
}
