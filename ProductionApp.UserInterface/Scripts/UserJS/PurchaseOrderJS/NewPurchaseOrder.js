var emptyGuid = "00000000-0000-0000-0000-000000000000";
var DataTables = {};
$(document).ready(function () {
    debugger;
    try {
        $("#SupplierID").select2({
        });
        $('#btnUpload').click(function () {
            debugger;
            //Pass the controller name
            var FileObject = new Object;
            if ($('#hdnFileDupID').val() != emptyGuid) {
                FileObject.ParentID = (($('#ID').val()) != emptyGuid ? ($('#ID').val()) : $('#hdnFileDupID').val());
            }
            else {
                FileObject.ParentID = ($('#ID').val() == emptyGuid) ? "" : $('#ID').val();
            }


            FileObject.ParentType = "PurchaseOrder";
            FileObject.Controller = "FileUpload";
            UploadFile(FileObject);
        });
        
    }
    catch (e) {
        console.log(e.message);
    }
});
function AddPurchaseOrderDetail() {
    debugger;
    $('#RequisitionDetailsModal').modal('show');
    BindOrReloadRequisitionListTable('Init');
}
//bind requistition List
function BindOrReloadRequisitionListTable(action) {
    try {
        //creating advancesearch object
        debugger;
        RequisitionAdvanceSearchViewModel = new Object();
        DataTablePagingViewModel = new Object();
        DataTablePagingViewModel.Length = 0;
        //switch case to check the operation
        switch (action) {
            case 'Reset':

                break;
            case 'Init':

                break;
            case 'Search':
                break;
            default:
                break;
        }
        RequisitionAdvanceSearchViewModel.DataTablePaging = DataTablePagingViewModel;

        DataTables.RequisitionList = $('#tblRequisitionList').DataTable({
            dom: '<"pull-left"f>rt<"bottom"ip><"clear">',
            order: [],
            searching: false,
            paging: true,
            pageLength: 10,

            lengthChange: false,
            proccessing: true,
            serverSide: true,
            ajax: {
                url: "GetAllRequisitionForPurchaseOrder/",
                data: { "requisitionAdvanceSearchVM": RequisitionAdvanceSearchViewModel },
                type: 'POST'
            },
            columns: [
                 { "data": "ID", "defaultContent": "<i>-</i>" },
                 { "data": null, "defaultContent": "", "width": "5%" },
                 { "data": "ReqNo", "defaultContent": "<i>-</i>" },
                 { "data": "Title", "defaultContent": "<i>-</i>" },
                 { "data": "ReqDateFormatted", "defaultContent": "<i>-</i>" },
                 { "data": "ReqStatus", "defaultContent": "<i>-</i>" },
                 { "data": "FinalApprovalDateFormatted", "defaultContent": "<i>-</i>" },
                 { "data": "RequisitionBy", "defaultContent": "<i>-</i>" }
            ],
            columnDefs: [{ orderable: false, className: 'select-checkbox', "targets": 1 }
                , { className: "text-left", "targets": [2, 3, 7, 6] }
                , { className: "text-center", "targets": [ 4, 5] }
                , { "targets": [0], "visible": false, "searchable": false }
                , { "targets": [2, 3, 4, 5, 6, 7], "bSortable": false }],
            select: { style: 'multi', selector: 'td:first-child' },
            destroy: true
        });       
    }
    catch (e) {
        console.log(e.message);
    }

}