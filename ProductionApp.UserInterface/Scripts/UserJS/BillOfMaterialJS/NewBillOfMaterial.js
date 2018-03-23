//*****************************************************************************
//*****************************************************************************
//Author: Arul
//CreatedDate: 09-Mar-2018 
//FileName: ViewBillOfMaterial.js
//Description: Client side coding for Listing BillOfMaterials
//******************************************************************************
//******************************************************************************

//--Global Declaration--//
var DataTables = {};
var EmptyGuid = "00000000-0000-0000-0000-000000000000";
var _IsInput = false;
var _Product = {};
var _BillOfMaterialDetailList = [];
var _BillOfMaterialDetail = {};
var _BOMComponentLineList = [];
var _BOMComponentLine = {};
//-------------------------------------------------------------

$(document).ready(function () {
    try {
        debugger;
        $('#ProductID').select2({});
        //DataTable for List of BillOfMaterialDetail
        try {
            DataTables.ComponentList = $('#tblBOMComponentDetail').DataTable({
                dom: '<"pull-right"f>rt<"bottom"ip><"clear">',
                order: [],
                "scrollY": "150px",
                "scrollCollapse": true,
                searching: false,
                paging: false,
                data: null,
                language: {
                    search: "_INPUT_",
                    searchPlaceholder: "Search"
                },
                columns: [
                  { "data": "ID" },
                  { "data": "ComponentID", "defaultContent": "<i>-</i>" },
                  { "data": "Product.Name", "defaultContent": "<i>-</i>" },
                  { "data": "Product.UnitCode", "defaultContent": "<i>-</i>" },
                  {
                      "data": "Qty", render: function (data, type, row) {
                          return '<input class="form-control text-left " name="Markup" value="' + row.Qty + '" type="text" onclick="SelectAllValue(this);" onkeypress = "return isNumber(event)", onchange="TextBoxValueChanged(this);">';
                      }, "defaultContent": "<i>-</i>"
                  },
                  { "data": null, "orderable": false, "defaultContent": '<a href="#" class="DeleteLink"  onclick="Delete(this)" ><i class="glyphicon glyphicon-trash" aria-hidden="true"></i></a>  |  <a href="#" onclick="LoadPartialAddProductionLine(this)"<i class="glyphicon glyphicon-share-alt" aria-hidden="true"></i></a>' },
                ],
                columnDefs: [{ orderable: false, className: 'select-checkbox', "targets": 1 },
                    { "targets": [0, 1], "visible": false, "searchable": false },
                    { className: "text-left", "targets": [4], "width": "15%" },
                    { className: "text-left", "targets": [2, 3], "width": "40%" },
                    { className: "text-center", "targets": [5], "width": "5%" },
                    { "targets": [3, 4, 5], "bSortable": false }
                ],
                select: { style: 'multi', selector: 'td:first-child' },
                destroy: true
            });
        } catch (ex) {
            console.log(ex.message);
        }

        //In case of Update
        if ($('#IsUpdateBOM').val() === 'True' && $('#IDBillOfMaterial') !== EmptyGuid) {
            BindBillOfMaterial();
            ChangeButtonPatchView('BillOfMaterial', 'divButtonPatch', 'Edit');
        }
        DescriptionOnChange();//On change for description property

        $('#hdnMasterCall').val("OTR");//for dynamic add in #ProductID dropdown
        $('.close').click(AddProduct);// on click on Add new Component Pop up for DataTable add

    }
    catch (ex) {
        console.log(ex.message);
    }
});
//-----------------Onchange for description for Dynamic ability------------------//
function DescriptionOnChange(currObj) {
    if ($('#DescriptionBOM').val() !== "") {
        $('#lblDescription').text($('#DescriptionBOM').val());
    }
    else {
        $('#lblDescription').text('BOM: New');
    }
}
//-----------------------Product List Pop Up---------------------------//
function LoadComponents() {
    $('#ProductListModal').modal('show');
    BindProductList();
}
//Bind Values into Product List DataTable for Pop Up
function BindProductList() {
    try {
        debugger;

        ProductAdvanceSearchViewModel = new Object();
        DataTablePagingViewModel = new Object();
        UnitViewModel = new Object();
        DataTablePagingViewModel.Length = 10;
        UnitViewModel.Code = null;

        ProductAdvanceSearchViewModel.DataTablePaging = DataTablePagingViewModel;
        ProductAdvanceSearchViewModel.Unit = UnitViewModel;

        DataTables.ProductList = $('#tblProductList').DataTable({
            dom: '<"pull-right"f>rt<"bottom"ip><"clear">',
            ordering: false,
            searching: true,
            paging: true,
            lengthChange: true,
            processing: true,
            language: {
                "processing": "<div class='spinner'><div class='bounce1'></div><div class='bounce2'></div><div class='bounce3'></div></div>"
            },
            serverSide: true,
            ajax: {
                url: "GetAllComponent/",
                data: { "productAdvanceSearchVM": ProductAdvanceSearchViewModel },
                type: 'POST'
            },
            pageLength: 10,
            columns: [
            { "data": "ID", "defaultContent": "<i>-</i>" },
            { "data": "Checkbox", "defaultContent": "" },
            { "data": "Name", "defaultContent": "<i>-</i>" },
            { "data": "Description", "defaultContent": "<i>-<i>" },
            { "data": "Unit.Description", "defaultContent": "<i>-<i>" }
            ],
            columnDefs: [{ "targets": [0], "visible": false, "searchable": false },
                { orderable: false, className: 'select-checkbox', "targets": 1 },
                { className: "text-right", "targets": [] },
                { className: "text-left", "targets": [1, 2, 3] },
                { className: "text-center", "targets": [] }],
            select: { style: 'multi', selector: 'td:first-child' },
            destroy: true
        });
    }
    catch (ex) {
        console.log(ex.message);
    }

}
//------------------------Add Product and BOM Component as Detail---------------------//
//Add Product
function AddComponent() {
    try {
        debugger;
        _IsInput = true;
        $('#hdnMasterCall').val("MSTR");
        AddProductMaster('MSTR');
    } catch (ex) {
        console.log(ex.message);
    }
}
//Add Product as Component in Details Table
function AddProduct() {
    try {
        debugger;
        if (_IsInput === true) {
            AddNewComponent();
        }
        _IsInput = false;
        $('#hdnMasterCall').val("OTR");
    }
    catch (ex) {
        console.log(ex.message);
    }
}
//Load Details properties as Component Object into Data table
function AddNewComponent() {
    try {
        debugger;
        _BillOfMaterialDetailList = [];
        _BillOfMaterialDetail = new Object();
        _BillOfMaterialDetail.ID = EmptyGuid;
        _BillOfMaterialDetail.ComponentID = $('#ID').val();
        _BillOfMaterialDetail.Product = new Object();
        _BillOfMaterialDetail.Product.Name = $('#Name').val();
        _BillOfMaterialDetail.Product.UnitCode = $('#UnitCode').val();
        _BillOfMaterialDetail.Qty = 1;
        _BillOfMaterialDetailList.push(_BillOfMaterialDetail);
        if ($('#ID').val() !== EmptyGuid && $('#ID').val() !== '') {
            DataTables.ComponentList.rows.add(_BillOfMaterialDetailList).draw(true);
        }
    } catch (ex) {
        console.log(ex.message);
    }
}
//---------------Load selected Product from List DataTable into Deatails DataTable--------------//
function BindComponentToTable() {
    try {
        debugger;
        var componentList = DataTables.ProductList.rows(".selected").data();
        _BillOfMaterialDetailList = [];
        for (var i = 0; i < componentList.length; i++) {
            _BillOfMaterialDetail = new Object();
            _BillOfMaterialDetail.ID = EmptyGuid;
            _BillOfMaterialDetail.ComponentID = componentList[i].ID;
            _BillOfMaterialDetail.Product = new Object();
            _BillOfMaterialDetail.Product.Name = componentList[i].Name;
            _BillOfMaterialDetail.Product.UnitCode = componentList[i].UnitCode;
            _BillOfMaterialDetail.Qty = 1;
            _BillOfMaterialDetailList.push(_BillOfMaterialDetail);
        }
        RebindComponentList(_BillOfMaterialDetailList);

    } catch (ex) {
        console.log(ex.message);
    }
}
//Add vales into Data Table
function RebindComponentList(_BillOfMaterialDetailList) {
    try {
        debugger;
        if (_BillOfMaterialDetailList != null) {
            DataTables.ComponentList.rows.add(_BillOfMaterialDetailList).draw(true);
        }
    } catch (ex) {
        console.log(ex.message);
    }
}
//--------------------------------Save BillOfMaterials and BOMDetails---------------------//
function SaveComponentDetail() {
    try {
        debugger;
        $("#DetailJSON").val('');
        _BillOfMaterialDetailList = [];
        GetDetailsFromTable();
        if (_BillOfMaterialDetailList.length > 0) {
            var result = JSON.stringify(_BillOfMaterialDetailList);
            $("#DetailJSON").val(result);
            $('#btnSave').trigger('click');
        }
        else {
            notyAlert('warning', 'Please Add BillOfMaterial Details!');
        }
    }
    catch (ex) {
        console.log(ex.message);
    }
}
//After Saved Successfully
function SaveSuccess(data, status) {
    debugger;
    var JsonResult = JSON.parse(data)
    var result = JsonResult.Result;
    var message = JsonResult.Message;
    var billOfMaterialVM = new Object();
    billOfMaterialVM = JsonResult.Records;
    switch (result) {
        case "OK":
            //$('#IsUpdateBOM').val('True');
            $('#IDBillOfMaterial').val(billOfMaterialVM.ID)
            message = billOfMaterialVM.Message;
            //notyAlert("success", message)
            LoadPartialAddProductionLine();
            //BindBillOfMaterial();
            break;
        case "ERROR":
            notyAlert("danger", message)
            break;
        default:
            notyAlert("danger", message)
            break;
    }
}
//Fetch Details from Table
function GetDetailsFromTable() {
    try {
        debugger;
        var billOfMaterialDetailList = DataTables.ComponentList.rows().data();
        for (var r = 0; r < billOfMaterialDetailList.length; r++) {
            var billOfMaterialDetailVM = new Object();
            billOfMaterialDetailVM.ID = billOfMaterialDetailList[r].ID;
            billOfMaterialDetailVM.ComponentID = billOfMaterialDetailList[r].ComponentID;
            billOfMaterialDetailVM.Qty = billOfMaterialDetailList[r].Qty;
            _BillOfMaterialDetailList.push(billOfMaterialDetailVM);
        }
    }
    catch (ex) {
        console.log(ex.message);
    }
}
//----------------------------Bind Details values into property boxes--------------------//
function BindBillOfMaterial() {
    try {
        debugger;
        var id = $('#IDBillOfMaterial').val();
        _BillOfMaterialDetail = GetBillOfMaterial(id);
        $('#DescriptionBOM').val(_BillOfMaterialDetail.Description);
        $('#ProductID').val(_BillOfMaterialDetail.ProductID).select2();
        $('#Product_Name').val(_BillOfMaterialDetail.Product.Name);
        DataTables.ComponentList.clear().rows.add(GetBillOfMaterialDetail(id)).draw(false)
    } catch (ex) {
        console.log(ex.message);
    }
}
//-------------------------Get Details of BillOfMaterial and BOMDetail------------------------//
function GetBillOfMaterial(id) {
    try {
        debugger;
        var data = { "id": id };
        var result = "";
        var message = "";
        var billOfMaterialVM = new Object();
        var jsonData = GetDataFromServer("BillOfMaterial/GetBillOfMaterial/", data);
        if (jsonData != '') {
            jsonData = JSON.parse(jsonData);
            result = jsonData.Result;
            message = jsonData.Message;
            billOfMaterialVM = jsonData.Records;
        }
        switch (result) {
            case "OK":
                break;
            case "ERROR":
                notyAlert('error', message);
                break;
            default:
                break;
        }
        return billOfMaterialVM;

    } catch (ex) {
        console.log(ex.message);
    }
}

function GetBillOfMaterialDetail(id) {
    try {
        debugger;
        var data = { "id": id };
        var result = "";
        var message = "";
        var billOfMaterialDetailList = [];
        var jsonData = GetDataFromServer("BillOfMaterial/GetBillOfMaterialDetail/", data);
        if (jsonData != '') {
            jsonData = JSON.parse(jsonData);
            result = jsonData.Result;
            message = jsonData.Message;
            billOfMaterialDetailList = jsonData.Records;
        }
        switch (result) {
            case "OK":
                break;
            case "ERROR":
                notyAlert('error', message);
                break;
            default:
                break;
        }
        return billOfMaterialDetailList;

    } catch (ex) {
        console.log(ex.message);
    }
}
//---------------Load Qty value change into corresponding Object---------------------//
function TextBoxValueChanged(thisObj) {
    try {
        debugger;
        var billOfMaterialDetailVM = DataTables.ComponentList.row($(thisObj).parents('tr')).data();
        var billOfMaterialDetailList = DataTables.ComponentList.rows().data();
        for (var i = 0; i < billOfMaterialDetailList.length; i++) {
            if (billOfMaterialDetailList[i].ComponentID === billOfMaterialDetailVM.ComponentID) {
                billOfMaterialDetailList[i].Qty = parseFloat(thisObj.value);
            }
        }
        DataTables.ComponentList.clear().rows.add(billOfMaterialDetailList).draw(false);
    }
    catch (ex) {
        console.log(ex.message);
    }
}
//------------------------------Delete BillOfMaterial-------------------------//
function DeleteClick() {
    try {
        debugger;
        var id = $('#IDBillOfMaterial').val();
        if (id !== EmptyGuid)
            notyConfirm('Are you sure to delete?', 'DeleteBillOfMaterial("' + id + '")');
        else
            notyAlert('error', "Cannot Delete");
    }
    catch (ex) {
        console.log(ex.message);
    }
}
//Delete BOM
function DeleteBillOfMaterial(id) {
    try {
        debugger;
        var data = { "id": id };
        var result = "";
        var message = "";
        var billOfMaterialVM = new Object();
        var jsonData = GetDataFromServer("BillOfMaterial/DeleteBillOfMaterial/", data);
        if (jsonData != '') {
            jsonData = JSON.parse(jsonData);
            result = jsonData.Result;
            message = jsonData.Message;
            billOfMaterialVM = jsonData.Record;
        }
        switch (result) {
            case "OK":
                notyAlert('success', message);
                window.location.replace("NewBillOfMaterial?code=PROD");
                break;
            case "ERROR":
                notyAlert('error', message);
                break;
            default:
                break;
        }
        return billOfMaterialVM;
    }
    catch (ex) {
        console.log(ex.message);
    }
}
//------------------------------------Delete BOMDetail-----------------------------//
function Delete(curobj) {
    try {
        var billOfMaterialDetailVM = DataTables.ComponentList.row($(curobj).parents('tr')).data();
        var rowindex = DataTables.ComponentList.row($(curobj).parents('tr')).index();

        if ((billOfMaterialDetailVM !== null) && (billOfMaterialDetailVM.ID !== EmptyGuid)) {
            var res = notyConfirm('Are you sure to delete?', 'DeleteBillOfMaterialDetail("' + billOfMaterialDetailVM.ID + '",' + rowindex + ')');
        }
        else {
            notyConfirm('Are you sure to delete?', 'DeleteTempItem("' + rowindex + '")');
        }
    }
    catch (ex) {
        console.log(ex.message);
    }
}
//Delete BOMDetail
function DeleteBillOfMaterialDetail(id, rowindex) {
    try {
        debugger;
        var data = { "id": id };
        var result = "";
        var message = "";
        var BillOfMaterialDetailVM = new Object();
        var jsonData = GetDataFromServer("BillOfMaterial/DeleteBillOfMaterialDetail/", data);
        if (jsonData != '') {
            jsonData = JSON.parse(jsonData);
            result = jsonData.Result;
            message = jsonData.Message;
            BillOfMaterialDetailVM = jsonData.Record;
        }
        switch (result) {
            case "OK":
                notyAlert('success', message);
                Reset();
                DeleteTempItem(rowindex);
                break;
            case "ERROR":
                notyAlert('error', message);
                break;
            default:
                break;
        }
        return BillOfMaterialDetailVM;
    }
    catch (ex) {
        console.log(ex.message);
    }
}
//Delete Datatable Item
function DeleteTempItem(rowindex) {
    debugger;
    DataTables.ComponentList.row(rowindex).remove().draw(true);
}
//--------------------------Reset property values to default----------------------//
function Reset() {
    try{
        debugger;
        if ($('#IsUpdateBOM').val() === "True") {
            var id = $('#IDBillOfMaterial').val();
            window.location.replace("NewBillOfMaterial?code=STR&id=" + id + "");
        } else {
            window.location.replace("NewBillOfMaterial?code=STR");
        }
    } catch (ex) {
        console.log(ex.message);
    }
}
 //_________________________________________________________________________________//
//*****************************Production Line*************************************//

function LoadPartialAddProductionLine(curobj) {
    try {
        debugger;
        $('#step1').removeClass('active').addClass('disabled');
        $('#step2').removeClass('disabled').addClass('active');
        var BillOfMaterialViewModel = new Object();
        BillOfMaterialViewModel.ID = $('#IDBillOfMaterial').val();
        BillOfMaterialViewModel.IsUpdate = $('#IsUpdateBOM').val();
        BillOfMaterialViewModel.Product = new Object();
        BillOfMaterialViewModel.ProductID = BillOfMaterialViewModel.Product.ID = $('#ProductID').val();
        BillOfMaterialViewModel.Product.Name = $($('#ProductID')).children("option[value='" + $('#ProductID').val() + "']").first().html();
        BillOfMaterialViewModel.BOMComponentLine = new Object();
        if (curobj !== undefined) {
            var billOfMaterialDetailVM = DataTables.ComponentList.row($(curobj).parents('tr')).data();
            BillOfMaterialViewModel.BOMComponentLine.ComponentID = billOfMaterialDetailVM.ComponentID;
        }
        var data = { "billOfMaterialVM": BillOfMaterialViewModel }
        $('#divPartial').load("AddProductionLine", data);

    }
    catch (ex) {
        console.log(ex.message);
    }
}
//Reload ListAllStage PartialView
function LoadPartialListAllStage() {
    try {
        debugger;
        var BOMComponentLineVM = new Object();
        var data = { "bOMComponentLineVM": BOMComponentLineVM }
        $('#divStageList').load("ListAllStage", data);
    } catch (ex) {
        console.log(ex.message);
    }
}
//Insert stages into selected
function OnStageSelect() {
    debugger;
    //var txt = [];
    $("#selectable li.ui-selected").each(function () {
        //txt.push($(this).text());
        debugger;
        $("#selected").append('<li value="' + $(this).attr('value') + '" class="ui-widget-content ui-selectee">' + $(this).text() + '</li>');
        $(this).remove();
    });
}
//Revert stages from selected
function OnStageDeselect() {
    debugger;
    $("#selected li.ui-selected").each(function () {
        debugger;
        $("#selectable").append('<li value="' + $(this).attr('value') + '" class="ui-widget-content ui-selectee">' + $(this).text() + '</li>');
        $(this).remove();
    });
}
//--------------------Load LineStage table as DataTable-------------------//(called in the PartialView-script)
function LoadLineStageTable() {
    try {
        DataTables.LineStageList = $('#tblBOMLineStageList').DataTable({
            dom: '<"pull-right"f>rt<"bottom"ip><"clear">',
            order: [],
            "scrollY": "150px",
            "scrollCollapse": true,
            searching: false,
            paging: false,
            data: null,
            language: {
                search: "_INPUT_",
                searchPlaceholder: "Search"
            },
            columns: [
              { "data": "ID" },
              { "data": "LineName", "defaultContent": "<i>-</i>" },
              { "data": "ComponentID", "defaultContent": "<i>-</i>" },
              {
                  "data": "BOMComponentLineStageList", render: function (data, type, row) {
                      //debugger;
                      var Stages = [];
                      for (var i = 0; i < row.BOMComponentLineStageList.length; i++) {
                          Stages.push(" " + row.BOMComponentLineStageList[i].Stage.Description + "[" + (row.BOMComponentLineStageList[i].StageOrder) + "]");
                      }
                      return Stages;
                  }, "defaultContent": "<i>-</i>"
              },
              {
                  "data": null, render: function (data, type, row) {
                      debugger;
                      var StageIDs = [];
                      for (var i = 0; i < row.BOMComponentLineStageList.length; i++) {
                          StageIDs.push(row.BOMComponentLineStageList[i].ID);
                      }
                      return StageIDs;
                  }, "defaultContent": "<i>-</i>"
              },
              { "data": null, "orderable": false, "defaultContent": '<a href="#" class="actionLink"  onclick="EditLine(this)" ><i class="glyphicon glyphicon-pencil" aria-hidden="true"></i></a>' },
              { "data": null, "orderable": false, "defaultContent": '<a href="#" class="DeleteLink"  onclick="DeleteLine(this)" ><i class="glyphicon glyphicon-trash" aria-hidden="true"></i></a>  |  <a href="#"></a>' },
            ],
            columnDefs: [
                { "targets": [0, 2, 4], "visible": false, "searchable": false },
                { className: "text-left", "targets": [1, 3], "width": "40%" },
                { className: "text-center", "targets": [5], "width": "3%" },
                { className: "text-center", "targets": [6], "width": "7%" },
                { "targets": [0, 2, 4], "bSortable": false }
            ],
            destroy: true
        });
        //if ($("#BOMComponentLine_ComponentID").val() !== null && $("#BOMComponentLine_ComponentID").val() !== EmptyGuid) {
        //    GetBOMComponentLine($("#BOMComponentLine_ComponentID").val())
        //}
    }
    catch (ex) {
        console.log(ex.message);
    }
}
//--------------------------Get BOMComponentLine Details------------------------------//
function GetBOMComponentLine(id) {
    try {
        debugger;
        var data = { "componentID": id };
        var result = "";
        var message = "";
        var bOMComponentLineList = [];
        var jsonData = GetDataFromServer("BillOfMaterial/GetBOMComponentLine/", data);
        if (jsonData != '') {
            jsonData = JSON.parse(jsonData);
            result = jsonData.Result;
            message = jsonData.Message;
            bOMComponentLineList = jsonData.Records;
        }
        switch (result) {
            case "OK":
                break;
            case "ERROR":
                notyAlert('error', message);
                break;
            default:
                break;
        }
        return bOMComponentLineList;
    }
    catch (ex) {
        console.log(ex.message);
    }
}
//--------------------Refresh Properties for newLine for the component------------------//
function NewLine() {
    try{
        debugger;
        //$('#BOMComponentLine_ComponentID').val("");
        $('#BOMComponentLine_LineName').val("");
        $("#selected li.ui-selectee").each(function () {
            debugger;
            //$("#selectable").append('<li value="' + $(this).attr('value') + '" class="ui-widget-content ui-selectee">' + $(this).text() + '</li>');
            $(this).remove();
        });
        LoadPartialListAllStage();
    } catch (ex) {
        console.log(ex.message);
    }
}
//----------------------Load ComponentLine to DataTable-----------------------------------//
function SaveLine() {
    try{
        debugger;
        var BOMComponentLineList = [];
        var BOMComponentLineVM = new Object();
        BOMComponentLineVM.BOMComponentLineStageList = [];
        var BOMComponentLineStageList = [];
        var order = 1;
        $("#selected li.ui-selectee").each(function () {
            var BOMComponentLineStage = new Object();
            BOMComponentLineStage.Stage = new Object();
            BOMComponentLineStage.ID = EmptyGuid;
            BOMComponentLineStage.StageID = BOMComponentLineStage.Stage.ID = $(this).attr('value');
            BOMComponentLineStage.Stage.Description = $(this).text();
            BOMComponentLineStage.StageOrder = order++;
            BOMComponentLineStageList.push(BOMComponentLineStage);
        });
        debugger;
        BOMComponentLineVM.ID = EmptyGuid;
        BOMComponentLineVM.LineName = $('#BOMComponentLine_LineName').val();
        BOMComponentLineVM.ComponentID = $('#BOMComponentLine_ComponentID').val();
        BOMComponentLineVM.BOMComponentLineStageList = BOMComponentLineStageList;
        BOMComponentLineList.push(BOMComponentLineVM);
        DataTables.LineStageList.rows.add(BOMComponentLineList).draw(true);
        $('#BOMComponentLine_StageJSON').val(JSON.stringify(BOMComponentLineStageList));
        $("#btnSave").click();
    } catch (ex) {
        console.log(ex.message);
    }
}
//After Saved Successfully
function SaveLineSuccess(data, status) {
    debugger;
    var JsonResult = JSON.parse(data)
    var result = JsonResult.Result;
    var message = JsonResult.Message;
    var BOMComponentLineViewModel = new Object();
    BOMComponentLineViewModel = JsonResult.Records;
    switch (result) {
        case "OK":
            //$('#IsUpdate').val('True');
            RebindLineStageListTable(BOMComponentLineViewModel.ID)
            message = BOMComponentLineViewModel.Message;
            notyAlert("success", message)
            //LoadPartialAddProductionLine();
            //BindBillOfMaterial();
            break;
        case "ERROR":
            notyAlert("danger", message)
            break;
        default:
            notyAlert("danger", message)
            break;
    }
}
//To add ID returned into the 
function RebindLineStageListTable(id) {
    try {
        debugger;
        _BOMComponentLineList = DataTables.LineStageList.rows().data();
        _BOMComponentLineList[_BOMComponentLineList.length - 1].ID = id;
        DataTables.LineStageList.clear().rows.add(_BOMComponentLineList).draw(false);
    }
    catch (ex) {
        console.log(ex.message);
    }
}
//-------------------Save the BOMComponentLine details and forwards to next Page----------------// 
function SaveAndProceed() {
    try {
        debugger;
        $('#step2').removeClass('active').addClass('disabled');
        $('#step3').removeClass('disabled').addClass('active');
        var BillOfMaterialVM = new Object();
        BillOfMaterialVM.ID = $('#IDBillOfMaterial').val();
        BillOfMaterialVM.IsUpdate = $('#IsUpdateBOM').val();
        BillOfMaterialVM.Product = new Object();
        BillOfMaterialVM.Product.Name = $("#Product_Name").val();
        BillOfMaterialVM.BOMComponentLine = new Object();
        BillOfMaterialVM.BOMComponentLine.ID = $("#BOMComponentLine_ID").val();
        BillOfMaterialVM.BOMComponentLine.Product = new Object();
        BillOfMaterialVM.BOMComponentLine.Product.Name = $('#BOMComponentLine_ComponentID').text();
        BillOfMaterialVM.BOMComponentLine.LineName = $('#BOMComponentLine_LineName').val();
        BillOfMaterialVM.BOMComponentLineStageDetail = new Object();
        var data = { "billOfMaterialVM": BillOfMaterialVM }

        $('#divPartial').load("AddStageDetail", data);
    }
    catch (ex) {
        console.log(ex.message);
    }
}
//-------------------------Delete BOMComponentLine-------------------------------//
function DeleteLine(curobj) {
    try {
        debugger;
        var BOMComponentLineVM = DataTables.LineStageList.row($(curobj).parents('tr')).data();
        var rowindex = DataTables.ComponentList.row($(curobj).parents('tr')).index();

        if ((BOMComponentLineVM !== null) && (BOMComponentLineVM.ID !== EmptyGuid)) {
            var res = notyConfirm('Are you sure to delete?', 'DeleteBOMComponentLine("' + BOMComponentLineVM.ID + '",' + rowindex + ')');
        }
        else {
            notyConfirm('Are you sure to delete?', 'DeleteTempLine("' + rowindex + '")');
        }
    }
    catch (ex) {
        console.log(ex.message);
    }
}

function DeleteTempLine(rowindex) {
    debugger;
    DataTables.LineStageList.row(rowindex).remove().draw(true);
}

function DeleteBOMComponentLine(id, rowindex) {
    try {
        debugger;
        var data = { "id": id };
        var result = "";
        var message = "";
        var BOMComponentLineViewModel = new Object();
        var jsonData = GetDataFromServer("BillOfMaterial/DeleteBOMComponentLine/", data);
        if (jsonData != '') {
            jsonData = JSON.parse(jsonData);
            result = jsonData.Result;
            message = jsonData.Message;
            BOMComponentLineViewModel = jsonData.Record;
        }
        switch (result) {
            case "OK":
                notyAlert('success', message);
                DeleteTempLine(rowindex);
                break;
            case "ERROR":
                notyAlert('error', message);
                break;
            default:
                break;
        }
        return BOMComponentLineViewModel;
    }
    catch (ex) {
        console.log(ex.message);
    }
}
 //________________________________________________________________________________//
//-----------------------Return to Production Line page---------------------------//
function GoBack() {
    try {
        debugger;
        $('#step3').removeClass('active').addClass('disabled');
        $('#step2').removeClass('disabled').addClass('active');
        var BillOfMaterialVM = new Object();
        BillOfMaterialVM.ID = $('#IDBillOfMaterial').val();
        BillOfMaterialVM.IsUpdate = $('#IsUpdateBOM').val();
        BillOfMaterialVM.Product = new Object();
        BillOfMaterialVM.Product.Name = $('#Product_Name').val();
        var data = { "billOfMaterialVM": BillOfMaterialVM }
        $('#divPartial').load("AddProductionLine", data);
    }
    catch (ex) {
        console.log(ex.message);
    }
}
 //__________________________________________End_______________________________________________//
////////////////////////////////////////////////////////////////////////////////////////////////
