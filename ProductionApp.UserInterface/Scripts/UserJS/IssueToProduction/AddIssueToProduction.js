var DataTables = {};
var _MaterialIssueDetail = [];
var _MaterialIssueDetailList = [];
var EmptyGuid = "00000000-0000-0000-0000-000000000000";
$(document).ready(function () {

    try {
        $("#IssueTo").select2({
            placeholder: "Select Employee..",

        });
        $("#IssuedBy").select2({
            placeholder: "Select Employee..",

        });
        //$("#materialSearch").select2({
        //    dropdownParent: $("#AddIssueToProductionItemModal")
        //});

        DataTables.MaterialIssueDetailTable = $('#tblIssueToProductionDetail').DataTable(
     {
         dom: '<"pull-right"f>rt<"bottom"ip><"clear">',
         ordering:false,
         searching: false,
         paging: false,
         data: null,
         autoWidth: false,
         columns: [
         { "data": "ID", "defaultContent": "<i></i>" },
         { "data": "MaterialID", "defaultContent": "<i></i>" },
         { "data": "MaterialCode", render: function (data, type, row) { return data }, "defaultContent": "<i></i>" },
         { "data": "MaterialDesc", render: function (data, type, row) { return data }, "defaultContent": "<i></i>" },
         { "data": "UnitCode", render: function (data, type, row) { return data }, "defaultContent": "<i></i>" },
         { "data": "Qty", render: function (data, type, row) { return data }, "defaultContent": "<i></i>" },        
         { "data": null, "orderable": false, "defaultContent": '<a href="#" class="DeleteLink"  onclick="Delete(this)" ><i class="glyphicon glyphicon-trash" aria-hidden="true"></i></a> | <a href="#" class="actionLink"  onclick="ProductEdit(this)" ><i class="glyphicon glyphicon-share-alt" aria-hidden="true"></i></a>' },
         ],
         columnDefs: [{ "targets": [0,1], "visible": false, searchable: false },                              
             {className:"text-left","targets":[2,3,4,5]}
         ]
     });

        $("#MaterialID").change(function () {
            debugger;
            BindRawMaterialDetails(this.value)
        });

}
    catch (x) {
        //this will show the error msg in the browser console(F12) 
    console.log(x.message);
}
    
});
function ShowIssueToProductionDetailsModal()
{
    debugger;
    PopupClearFields();
    $('#RawMaterial_ID').val();
    $('#MaterialIssueDetail_MaterialCode').val();
    $('#MaterialIssueDetail_MaterialDesc').val();
    $('#MaterialIssueDetail_UnitCode').val();
    $('#MaterialIssueDetail_Qty').val();
    $('#AddIssueToProductionItemModal').modal('show');
}

function PopupClearFields()
{
    _MaterialIssueDetail = [];
   // $("#materialSearch").select2({ dropdownParent: $("#AddIssueToProductionItemModal") });
   // $('#materialSearch').val('').trigger('change');
    $('#MaterialID').val('');
    $('#MaterialIssueDetail_MaterialCode').val('');
    $('#MaterialIssueDetail_MaterialDesc').val('');
    $('#MaterialIssueDetail_UnitCode').val('');
    $('#MaterialIssueDetail_Qty').val('');
}

function BindRawMaterialDetails(ID)
{
    var result = GetRawMaterial(ID);
    $('#MaterialIssueDetail_MaterialCode').val(result.MaterialCode);
    $('#MaterialIssueDetail_MaterialDesc').val(result.Description);
    $('#MaterialIssueDetail_UnitCode').val(result.UnitCode);
    $('#MaterialIssueDetail_Qty').val(result.Qty);
}

function GetRawMaterial(ID) {
    try {
        debugger;
        var data = { "ID": ID };
        var ds = {};
        ds = GetDataFromServer("IssueToProduction/GetRawMaterial/", data);
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

function AddIssueToProductItem()
{
    debugger;
    if ($('#RawMaterial_ID').val() != "")
    {
        _MaterialIssueDetail = [];
        AddMaterialIssue = new Object();
        AddMaterialIssue.MaterialID = $('#RawMaterial_ID').val();
        AddMaterialIssue.MaterialCode = $('#MaterialIssueDetail_MaterialCode').val();
        AddMaterialIssue.MaterialDesc = $('#MaterialIssueDetail_MaterialDesc').val();
        AddMaterialIssue.UnitCode = $('#MaterialIssueDetail_UnitCode').val();
        AddMaterialIssue.Qty = $('#MaterialIssueDetail_Qty').val();
        _MaterialIssueDetail.push(AddMaterialIssue);

        if(_MaterialIssueDetail!=null)
        {
            var allData = DataTables.MaterialIssueDetailTable.rows().data();
            if(allData.length>0)
            {
                var checkPoint = 0;
                for(var i=0;i<=allData.length;i++)
                {
                    if(allData[i].MaterialID==$('#RawMaterial_ID').val())
                    {
                        allData[i].MaterialCode = $('#MaterialIssueDetail_MaterialCode').val();
                        allData[i].MaterialDesc = $('#MaterialIssueDetail_MaterialDesc').val();
                        allData[i].UnitCode = $('#MaterialIssueDetail_UnitCode').val();
                        allData[i].Qty = $('#MaterialIssueDetail_Qty').val();
                        checkPoint = 1;
                        break;
                    }
                }
                if(!checkPoint)
                {
                  
                    DataTables.MaterialIssueDetailTable.rows.add(_MaterialIssueDetail).draw(false);
                }
                else
                {
                    DataTables.MaterialIssueDetailTable.clear().rows.add(addData).draw(false);
                }
            }
            else
            {
                
                DataTables.MaterialIssueDetailTable.rows.add(_MaterialIssueDetail).draw(false);
            }
        }
    }
    $('#AddIssueToProductionItemModal').modal('hide');

}

//function Save()
//{
//    $("#DetailJSON").val('');
//    _MaterialIssueDetailList = [];
//    AddMaterialIssueDetailList();
//    if(AddMaterialIssueDetailList.length>0)
//    {
//        var result = JSON.stringify(_MaterialIssueDetailList);
//        $("#DetailJSON").val(result);
//        $('#btnSave').trigger('click');
//    }
//    else
//    {
//        notyAlert('warning', 'Please Add Requistion Details!');
//    }
//}
//function AddMaterialIssueDetailList()
//{
//    var data = DataTables.MaterialIssueDetailTable.rows().data();
//    for(var r=0;r<data.length;r++)
//    {
//        MaterialIssueDetail = new Object();
//        MaterialIssueDetail.MaterialID = data[r].MaterialID;
//        MaterialIssueDetail.MaterialDesc = data[r].MaterialDesc;
//        MaterialIssueDetail.UnitCode = data[r].UnitCode;
//        MaterialIssueDetail.Qty = data[r].Qty;
//        _MaterialIssueDetailList.push(MaterialIssueDetail);
//    }
//}