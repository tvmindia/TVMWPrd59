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
        BindOrReloadRequisitionListTable('Init');
    }
    catch (e) {
        console.log(e.message);
    }
});
function AddPurchaseOrderDetail() {
    debugger;
    $('#RequisitionDetailsModal').modal('show');
}
function BindOrReloadRequisitionListTable(action) {

}