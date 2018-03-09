//*****************************************************************************
//*****************************************************************************
//Author: Gibin Jacob
//CreatedDate: 12-Feb-2018 
//LastModified: 05-Mar-2018 
//FileName: NewRequisition.js
//Description: Client side coding for Add/Edit Sales Order
//******************************************************************************
//******************************************************************************

//--Global Declaration--//
var DataTables = {};
var EmptyGuid = "00000000-0000-0000-0000-000000000000";
var _SalesOrderDetail = [];
var _SalesOrderDetailList = [];

$(document).ready(function () {
    debugger;
    try {
        //$("#MaterialID").select2({
        //    dropdownParent: $("#RequisitionDetailsModal")
        //});
        $("#EmployeeID").select2({
        });

        $('#btnUpload').click(function () {
            debugger;
            //Pass the controller name
            var FileObject = new Object;
            if ($('#hdnFileDupID').val() != EmptyGuid) {
                FileObject.ParentID = (($('#ID').val()) != EmptyGuid ? ($('#ID').val()) : $('#hdnFileDupID').val());
            }
            else {
                FileObject.ParentID = ($('#ID').val() == EmptyGuid) ? "" : $('#ID').val();
            }
            FileObject.ParentType = "SalesOrder";
            FileObject.Controller = "FileUpload";
            UploadFile(FileObject);
        });

        DataTables.RequisitionDetailTable = $('#tblSalesOrderDetail').DataTable(
      {
          dom: '<"pull-right"f>rt<"bottom"ip><"clear">',
          ordering: false,
          searching: false,
          paging: false,
          data: null,
          autoWidth: false,
          columns: [
          { "data": "", "defaultContent": "<i></i>" },
          { "data": "", "defaultContent": "<i></i>" },
          { "data": "", "defaultContent": "<i></i>" },
          { "data": "", "defaultContent": "<i></i>" },
          { "data": "", "defaultContent": "<i></i>" },
          { "data": "", "defaultContent": "<i></i>" },
          { "data": "", "defaultContent": "<i></i>" },
          { "data": "", "defaultContent": "<i></i>" },
          { "data": "", render: function (data, type, row) { return data }, "defaultContent": "<i></i>", "width": "10%" },
          { "data": "", render: function (data, type, row) { return data }, "defaultContent": "<i></i>", "width": "10%" },
          { "data": "", render: function (data, type, row) { return data }, "defaultContent": "<i></i>", "width": "10%" },
          { "data": "", render: function (data, type, row) { return data }, "defaultContent": "<i></i>", "width": "10%" },
          { "data": "", render: function (data, type, row) { return data }, "defaultContent": "<i></i>", "width": "10%" },
          { "data": null, "orderable": false, "defaultContent": '<a href="#" class="actionLink"  onclick="MaterialEdit(this)" ><i class="glyphicon glyphicon-pencil" aria-hidden="true"></i></a>  |  <a href="#" class="DeleteLink"  onclick="Delete(this)" ><i class="glyphicon glyphicon-trash" aria-hidden="true"></i></a>' },
          ],
          columnDefs: [{ "targets": [0, 1], "visible": false, searchable: false },
              { className: "text-center", "targets": [13], "width": "5%" },
              { className: "text-right", "targets": [4, 5, 6] },
              { className: "text-left", "targets": [2, 3] }
          ]
      });

        $("#MaterialID").change(function () {
            BindMaterialDetails(this.value)
        });
        debugger;
        if ($('#IsUpdate').val() == 'True') {
            BindRequisitionByID()
        }
        else {
            $('#lblSalesOrderNo').text('Sales Order# : New');
        }

    }
    catch (e) {
        console.log(e.message);
    }
});

function Save() {
    debugger;
    $("#DetailJSON").val('');
    //_RequistionDetailList = [];
    //AddRequistionDetailList();
    //if (_RequistionDetailList.length > 0) {
    //    var result = JSON.stringify(_RequistionDetailList);
    //    $("#DetailJSON").val(result);
        $('#btnSave').trigger('click');
    //}
    //else {
    //    notyAlert('warning', 'Please Add Requistion Details!');
    //}

}