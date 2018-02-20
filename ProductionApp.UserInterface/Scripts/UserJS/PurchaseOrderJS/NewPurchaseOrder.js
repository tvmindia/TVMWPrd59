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
        //RequisitionDetails
        DataTables.RequisitionDetailsTable = $('#tblRequisitionDetails').DataTable({
            dom: '<"pull-left"f>rt<"bottom"ip><"clear">',
            order: [],
            searching: true,
            paging: true,
            pageLength: 7,
            data: null,
            columns: [
                 { "data": "ID", "defaultContent": "<i>-</i>" },
                 { "data": "ReqID", "defaultContent": "<i>-</i>" },
                 { "data": "MaterialID", "defaultContent": "<i>-</i>" },
                 { "data": null, "defaultContent": "", "width": "5%" },
                 { "data": "ReqNo", "defaultContent": "<i>-</i>", "width": "10%" },
                 { "data": "RawMaterial.MaterialCode", "defaultContent": "<i>-</i>" },
                 {
                     "data": "Description", "defaultContent": "<i>-</i>",
                     'render': function (data, type, row) {
                         if (row.Description)
                             Desc = data;
                         else
                             Desc = row.Description;
                         return '<input class="form-control description" name="Markup" value="' + Desc + '" type="text" onchange="textBoxValueChanged(this,1);">';
                     }
                 },
                 {
                     "data": "ApproximateRate", "defaultContent": "<i>-</i>", "width": "10%", 'render': function (data, type, row) {
                         return '<input class="form-control text-right " name="Markup" value="' + row.ApproximateRate + '" type="text" onclick="SelectAllValue(this);" onkeypress = "return isNumber(event)", onchange="textBoxValueChanged(this,2);">';
                     }
                 },
                 { "data": "RequestedQty", "defaultContent": "<i>-</i>", "width": "10%" },
                 { "data": "OrderedQty", "defaultContent": "<i>-</i>", "width": "10%" },
                 {
                     "data": "POQty", "defaultContent": "<i>-</i>", "width": "10px",
                     'render': function (data, type, row) {
                         return '<input class="form-control text-right " name="Markup" type="text"  value="' + data + '"  onclick="SelectAllValue(this);" onkeypress = "return isNumber(event)", onchange="textBoxValueChanged(this,3);">';
                     }
                 }
                 , { "data": "RawMaterial.UnitCode", "defaultContent": "<i>-</i>" }

            ],
            columnDefs: [{ orderable: false, className: 'select-checkbox', "targets": 3 }
                , { className: "text-left", "targets": [5, 6] }
                , { className: "text-right", "targets": [7, 8, 9, 10] }
                , { className: "text-center", "targets": [1, 4] }
                , { "targets": [0,1,2,11], "visible": false, "searchable": false }
                , { "targets": [2, 3, 4, 5, 6, 7, 8, 9, 10], "bSortable": false }],

            select: { style: 'multi', selector: 'td:first-child' }
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

        DataTables.RequisitionListTable = $('#tblRequisitionList').DataTable({
            dom: '<"pull-left"f>rt<"bottom"ip><"clear">',
            order: [],
            searching: true,
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
                 { "data": "Checkbox", "defaultContent": "", "width": "5%" },
                 { "data": "ReqNo", "defaultContent": "<i>-</i>" },
                 { "data": "Title", "defaultContent": "<i>-</i>" },
                 { "data": "ReqDateFormatted", "defaultContent": "<i>-</i>" },
                 { "data": "ReqStatus", "defaultContent": "<i>-</i>" },
                 { "data": "ApprovalDateFormatted", "defaultContent": "<i>-</i>" },
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
function ViewRequisitionDetails(value) {
    debugger;
    $('#tabDetail').attr('data-toggle', 'tab');
    if (value)
        $('#tabDetail').trigger('click');
    else {
        //selecting Checked IDs for  bind the detail Table
        var IDs = GetSelectedRowIDs();
        if (IDs) {
            BindGetRequisitionDetailsTable(IDs);
            DataTables.RequisitionDetailsTable.rows().select();
            $('#btnForward').hide();
            $('#btnBackward').show();
            $('#btnAddSPODetails').show();
        }
        else {
            $('#tabDetail').attr('data-toggle', '');
            DataTables.RequisitionDetailsTable.clear().draw(false);
            notyAlert('warning', "Please Select Requisition");
        }
    }
}
function GetSelectedRowIDs() {
    var SelectedRows = DataTables.RequisitionListTable.rows(".selected").data();
    if ((SelectedRows) && (SelectedRows.length > 0)) {
        var arrIDs = "";
        for (var r = 0; r < SelectedRows.length; r++) {
            if (r == 0)
                arrIDs = SelectedRows[r].ID;
            else
                arrIDs = arrIDs + ',' + SelectedRows[r].ID;
        }
        return arrIDs;
    }
}
function BindGetRequisitionDetailsTable(IDs) {
    try {
        debugger;
        var test = GetRequisitionDetailsByIDs(IDs);
        DataTables.RequisitionDetailsTable.clear().rows.add(test).draw(false);
    }
    catch (e) {
        //this will show the error msg in the browser console(F12) 
        console.log(e.message);
    }
}
//RequisitionBinding
function GetRequisitionDetailsByIDs(IDs) {
    try {
        debugger;
        var POID = $('#ID').val();
        var data = { "IDs": IDs, "POID": POID };

        var ds = {};
        ds = GetDataFromServer("PurchaseOrder/GetRequisitionDetailsByIDs/", data);
        if (ds != '') {
            ds = JSON.parse(ds);
        }
        if (ds.Result == "OK") {
            return ds.Records;
        }
        if (ds.Result == "ERROR") {
            notyAlert('error', ds.message);
        }
    }
    catch (e) {
        //this will show the error msg in the browser console(F12) 
        console.log(e.message);
    }
}