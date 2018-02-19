var DataTables = {};
var EmptyGuid = "00000000-0000-0000-0000-000000000000";
var _RequistionDetail = [];
var _RequistionDetailList = [];

$(document).ready(function () {
    debugger;
    try {

        DataTables.RequisitionDetailTable = $('#tblRequisitionDetail').DataTable(
      {
          dom: '<"pull-right"f>rt<"bottom"ip><"clear">',
          ordering: false,
          searching: false,
          paging: false,
          data: null,
          autoWidth: false,
          columns: [
          { "data": "ID", "defaultContent": "<i></i>" },
          { "data": "MaterialID", "defaultContent": "<i></i>" },
          { "data": "MaterialCode", render: function (data, type, row) { return data }, "defaultContent": "<i></i>" },
          { "data": "Description", render: function (data, type, row) { return data }, "defaultContent": "<i></i>" },
          { "data": "CurrentStock", render: function (data, type, row) { return data }, "defaultContent": "<i></i>" },
          { "data": "RequestedQty", render: function (data, type, row) { return data }, "defaultContent": "<i></i>" },
          { "data": "ApproximateRate", render: function (data, type, row) { return roundoff(data) }, "defaultContent": "<i></i>" },
          { "data": null, "orderable": false, "defaultContent": '<a href="#" class="DeleteLink"  onclick="Delete(this)" ><i class="glyphicon glyphicon-trash" aria-hidden="true"></i></a> | <a href="#" class="actionLink"  onclick="ProductEdit(this)" ><i class="glyphicon glyphicon-share-alt" aria-hidden="true"></i></a>' },
          ],
          columnDefs: [{ "targets": [], "visible": false, searchable: false },
              { className: "text-center", "targets": [6],"width": "8%" },
              { className: "text-right", "targets": [4,5] },
              { className: "text-left", "targets": [1,2,3] }
          ]
      });

        $("#MaterialID").change(function () {
           BindRawMaterialDetails(this.value)
        });
    }
    catch (e) {
        console.log(e.message);
    }
});

function ShowRequisitionDetailsModal()
{
    debugger;
    $("#MaterialID").val('')
    $('#RequisitionDetail_MaterialCode').val('');
    $('#RequisitionDetail_CurrentStock').val('');
    $('#RequisitionDetail_Description').val('');
    $('#RequisitionDetail_ApproximateRate').val('');
    $('#RequisitionDetail_RequestedQty').val('');
    $('#RequisitionDetailsModal').modal('show');

}

function BindRawMaterialDetails(ID)
{
    debugger;
    var result = GetRawMaterial(ID);
    $('#RequisitionDetail_MaterialCode').val(result.MaterialCode);
    $('#RequisitionDetail_CurrentStock').val(result.CurrentStock);
    $('#RequisitionDetail_Description').val(result.Description);
    $('#RequisitionDetail_ApproximateRate').val(result.Rate);
}

function GetRawMaterial(ID) {
    try {
        debugger;
        var data = { "ID": ID };
        var ds = {};
        ds = GetDataFromServer("Requisition/GetRawMaterial/", data);
        if (ds != '') {
            ds = JSON.parse(ds);
        }
        if (ds.Result == "OK") {
            return ds.Records;
        }
        if (ds.Result == "ERROR") {
            alert(ds.Message);
        }
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}

function AddRequisitionDetails()
{
    debugger;
    if ($("#MaterialID").val() != "")
    {
        _RequistionDetail = [];
        RequisitionMaterial = new Object();
        RequisitionMaterial.MaterialID = $("#MaterialID").val();
        RequisitionMaterial.MaterialCode=$('#RequisitionDetail_MaterialCode').val();
        RequisitionMaterial.Description = $('#RequisitionDetail_Description').val();
        RequisitionMaterial.RequestedQty = $('#RequisitionDetail_RequestedQty').val();
        RequisitionMaterial.CurrentStock = $('#RequisitionDetail_CurrentStock').val();
        RequisitionMaterial.ApproximateRate = $('#RequisitionDetail_ApproximateRate').val();
        _RequistionDetail.push(RequisitionMaterial);

        if (_RequistionDetail != null)
        {
            //check product existing or not if soo update the new
            var allData = DataTables.RequisitionDetailTable.rows().data();
            if (allData.length > 0) {
                var checkPoint = 0;
                for (var i = 0; i < allData.length; i++) {
                    if (allData[i].MaterialID == $("#MaterialID").val()) {
                        allData[i].Description = $('#RequisitionDetail_Description').val();
                        allData[i].CurrentStock = $('#RequisitionDetail_CurrentStock').val();
                        allData[i].RequestedQty = $('#RequisitionDetail_RequestedQty').val();
                        allData[i].ApproximateRate = $('#RequisitionDetail_ApproximateRate').val();
                        checkPoint = 1;
                        break;
                    }
                }
                if (!checkPoint) {
                    DataTables.RequisitionDetailTable.rows.add(_RequistionDetail).draw(false);
                }
                else {
                    DataTables.RequisitionDetailTable.clear().rows.add(allData).draw(false);
                }
            }
            else {
                DataTables.RequisitionDetailTable.rows.add(_RequistionDetail).draw(false);
            }
        }      
        $('#RequisitionDetailsModal').modal('hide');
    }
    else
    {
        notyAlert('warning', "Material is Empty");
    }
}

function Save()
{
    debugger;
    $("#DetailJSON").val('');
    _RequistionDetailList = [];
    AddRequistionDetailList();
    if (_RequistionDetailList.length > 0) {
        var result = JSON.stringify(_RequistionDetailList);
        $("#DetailJSON").val(result);
        $('#btnSave').trigger('click');
    }
    else {
        notyAlert('warning', 'Please Add Requistion Details!');
    }

}
function AddRequistionDetailList() {
    debugger;
    var data = DataTables.RequisitionDetailTable.rows().data();
    for (var r = 0; r < data.length; r++) {
        RequisitionDetail = new Object();
        RequisitionDetail.MaterialID = data[r].MaterialID;
        RequisitionDetail.Description = data[r].Description;
        RequisitionDetail.RequestedQty = data[r].RequestedQty;
        RequisitionDetail.ApproximateRate = data[r].ApproximateRate;
        _RequistionDetailList.push(RequisitionDetail);
    }
}