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
var _BOMComponentLineStageDetailList = [];
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
    var BillOfMaterialViewModel = new Object();
    BillOfMaterialViewModel = JsonResult.Records;
    switch (result) {
        case "OK":
            //$('#IsUpdateBOM').val('True');
            $('#IDBillOfMaterial').val(BillOfMaterialViewModel.ID)
            message = BillOfMaterialViewModel.Message;
            //notyAlert("success", message)
            ChangeButtonPatchView('BillOfMaterial', 'divButtonPatch', 'Add');
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
        var BillOfMaterialViewModel = new Object();
        var jsonData = GetDataFromServer("BillOfMaterial/GetBillOfMaterial/", data);
        if (jsonData != '') {
            jsonData = JSON.parse(jsonData);
            result = jsonData.Result;
            message = jsonData.Message;
            BillOfMaterialViewModel = jsonData.Records;
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
        return BillOfMaterialViewModel;

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
        var BillOfMaterialViewModel = new Object();
        var jsonData = GetDataFromServer("BillOfMaterial/DeleteBillOfMaterial/", data);
        if (jsonData != '') {
            jsonData = JSON.parse(jsonData);
            result = jsonData.Result;
            message = jsonData.Message;
            BillOfMaterialViewModel = jsonData.Record;
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
        return BillOfMaterialViewModel;
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
    try {
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
                      //debugger;
                      var StageIDs = [];
                      for (var i = 0; i < row.BOMComponentLineStageList.length; i++) {
                          StageIDs.push(row.BOMComponentLineStageList[i].ID);
                      }
                      return StageIDs;
                  }, "defaultContent": "<i>-</i>"
              },
              { "data": null, "orderable": false, "defaultContent": '<a href="#" class="actionLink"  onclick="EditLine(this)" ><i class="glyphicon glyphicon-pencil" aria-hidden="true"></i></a>' },
              { "data": null, "orderable": false, "defaultContent": '<a href="#" class="DeleteLink"  onclick="DeleteLine(this)" ><i class="glyphicon glyphicon-trash" aria-hidden="true"></i></a>  |  <a href="#" onclick="BindComponentLineStageDetail(this)"<i class="glyphicon glyphicon-share-alt" aria-hidden="true"></i></a>' },
            ],
            columnDefs: [
                { "targets": [0, 2, 4], "visible": false, "searchable": false },
                { className: "text-left", "targets": [1, 3]},
                { className: "text-center", "targets": [5], "width": "3%" },
                { className: "text-center", "targets": [6], "width": "9%" },
                { "targets": [0, 2, 4], "bSortable": false }
            ],
            destroy: true
        });
        if ($("#BOMComponentLine_ComponentID").val() !== null && $("#BOMComponentLine_ComponentID").val() !== EmptyGuid) {
            ComponentIDOnChange();
        }
    }
    catch (ex) {
        console.log(ex.message);
    }
}
//------------------------------ComponentID on change---------------------------------//
function ComponentIDOnChange() {
    try {
        debugger;
        var id = $("#BOMComponentLine_ComponentID").val();
        DataTables.LineStageList.clear().rows.add(GetBOMComponentLine(id)).draw(true);
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
    try {
        debugger;
        //$('#BOMComponentLine_ComponentID').val("");
        $('#BOMComponentLine_ID').val(EmptyGuid);
        $('#BOMComponentLine_IsUpdate').val() = "false";
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
//-----------------------------------------Edit Line--------------------------------------------//
function EditLine(curobj) {
    try {
        debugger;
        BOMComponentLineViewModel = DataTables.LineStageList.row($(curobj).parents('tr')).data();
        $('#BOMComponentLine_ID').val(BOMComponentLineViewModel.ID);
        $('#BOMComponentLine_IsUpdate').val('true');
        $('#BOMComponentLine_StageJSON').val(JSON.stringify(BOMComponentLineViewModel.BOMComponentLineStageList));
        
        $("#selectable li.ui-selectee").each(function () {
            debugger;
            for (var i = 0; i < BOMComponentLineViewModel.BOMComponentLineStageList.length; i++) {
                if (BOMComponentLineViewModel.BOMComponentLineStageList[i].StageID === $(this).attr('value')) {
                    $(this).addClass("ui-selected");
                }
            }
        });
        OnStageSelect();
    }
    catch (ex) {
        console.log(ex.message);
    }
}
//----------------------Load ComponentLine to DataTable-----------------------------------//
function SaveLine() {
    try {
        debugger;
        //var BOMComponentLineList = [];
        //var BOMComponentLineVM = new Object();
        //BOMComponentLineVM.BOMComponentLineStageList = [];
        //var BOMComponentLineStageList = [];
        //var order = 1;
        $("#selected li.ui-selectee").each(function () {
            var BOMComponentLineStage = new Object();
            BOMComponentLineStage.Stage = new Object();
            BOMComponentLineStage.ID = EmptyGuid;
            BOMComponentLineStage.StageID = BOMComponentLineStage.Stage.ID = $(this).attr('value');
            BOMComponentLineStage.Stage.Description = $(this).text();
            BOMComponentLineStage.StageOrder = order++;
            BOMComponentLineStageList.push(BOMComponentLineStage);
        });
        if ($('#BOMComponentLine_IsUpdate').val() === "true")
        {
            //if ($('#BOMComponentLine_StageJSON').val() !== null && $('#BOMComponentLine_StageJSON').val() !== "")
            JSON.parse(jsonData)
        }
        //debugger;
        //BOMComponentLineVM.LineName = $('#BOMComponentLine_LineName').val();
        //BOMComponentLineVM.ComponentID = $('#BOMComponentLine_ComponentID').val();
        //BOMComponentLineVM.BOMComponentLineStageList = BOMComponentLineStageList;
        //BOMComponentLineList.push(BOMComponentLineVM);
        //DataTables.LineStageList.rows.add(BOMComponentLineList).draw(true);
        $('#BOMComponentLine_StageJSON').val(JSON.stringify(BOMComponentLineStageList));
        $("#btnSave").click();

    } catch (ex) {
        console.log(ex.message);
    }
}
//After Saved Successfully
function SaveLineSuccess(data, status) {
    try{
        debugger;
        var LineName = $('#BOMComponentLine_LineName').val();
        var JsonResult = JSON.parse(data)
        var result = JsonResult.Result;
        var message = JsonResult.Message;
        var BOMComponentLineViewModel = new Object();
        BOMComponentLineViewModel = JsonResult.Records;
        switch (result) {
            case "OK":
                //$('#IsUpdate').val('True');
                //RebindLineStageListTable(BOMComponentLineViewModel.ID)
                message = BOMComponentLineViewModel.Message;
                notyAlert("success", message)
                $('#BOMComponentLine_ComponentID').trigger('change');
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
        $('#BOMComponentLine_LineName').val(LineName)
    } catch (ex) {
        console.log(ex.message);
    }
    
}
////To add ID returned into the 
//function RebindLineStageListTable(id) {
//    try {
//        debugger;
//        _BOMComponentLineList = DataTables.LineStageList.rows().data();
//        _BOMComponentLineList[_BOMComponentLineList.length - 1].ID = id;
//        DataTables.LineStageList.clear().rows.add(_BOMComponentLineList).draw(false);
//    }
//    catch (ex) {
//        console.log(ex.message);
//    }
//}
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
//Delete detail by index from LineStage Datatable
function DeleteTempLine(rowindex) {
    debugger;
    DataTables.LineStageList.row(rowindex).remove().draw(true);
}
//DeleteBOMComponentLine
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
//-------------------Save the BOMComponentLine details and forwards to next Page----------------// 
function SaveAndProceed() {
    try {
        debugger;
        if ($('#BOMComponentLine_ComponentID').val() !== "") {
            $('#step2').removeClass('active').addClass('disabled');
            $('#step3').removeClass('disabled').addClass('active');
            var BillOfMaterialViewModel = new Object();
            BillOfMaterialViewModel.ID = $('#IDBillOfMaterial').val();
            BillOfMaterialViewModel.IsUpdate = $('#IsUpdateBOM').val();
            BillOfMaterialViewModel.Product = new Object();
            BillOfMaterialViewModel.Product.Name = $("#Product_Name").val();
            BillOfMaterialViewModel.BOMComponentLine = new Object();
            BillOfMaterialViewModel.BOMComponentLine.ID = $("#BOMComponentLine_ID").val();
            BillOfMaterialViewModel.BOMComponentLine.Product = new Object();
            BillOfMaterialViewModel.BOMComponentLine.Product.Name = $('#BOMComponentLine_ComponentID option:selected').text();
            BillOfMaterialViewModel.BOMComponentLine.ComponentID = $('#BOMComponentLine_ComponentID').val();
            BillOfMaterialViewModel.BOMComponentLine.LineName = $('#BOMComponentLine_LineName').val();
            BillOfMaterialViewModel.BOMComponentLineStageDetail = new Object();
            BillOfMaterialViewModel.BOMComponentLineStageDetail.ComponentLineID = ($("#BOMComponentLine_ID").val() !== EmptyGuid && $("#BOMComponentLine_ID").val() !== "") ? $("#BOMComponentLine_ID").val() : EmptyGuid;

            var data = { "billOfMaterialVM": BillOfMaterialViewModel }
            $('#divPartial').load("AddStageDetail", data);
        }
        else {
            notyAlert('warning', "Select a Component to Proceed");
        }
    }
    catch (ex) {
        console.log(ex.message);
    }
}
//on click of forward button on LineStage Table
function BindComponentLineStageDetail(curobj) {
    try {
        debugger;

        $('#step2').removeClass('active').addClass('disabled');
        $('#step3').removeClass('disabled').addClass('active');
        var BillOfMaterialViewModel = new Object();
        BillOfMaterialViewModel.ID = $('#IDBillOfMaterial').val();
        BillOfMaterialViewModel.IsUpdate = $('#IsUpdateBOM').val();
        BillOfMaterialViewModel.Product = new Object();
        BillOfMaterialViewModel.Product.Name = $("#Product_Name").val();
        BillOfMaterialViewModel.BOMComponentLine = new Object();
        BillOfMaterialViewModel.BOMComponentLineStageDetail = new Object();
        BillOfMaterialViewModel.BOMComponentLine = DataTables.LineStageList.row($(curobj).parents('tr')).data();
        BillOfMaterialViewModel.BOMComponentLine.Product.Name = $('#BOMComponentLine_ComponentID option:selected').text();
        BillOfMaterialViewModel.BOMComponentLineStageDetail.ComponentLineID = BillOfMaterialViewModel.BOMComponentLine.ID;

        var data = { "billOfMaterialVM": BillOfMaterialViewModel }
        $('#divPartial').load("AddStageDetail", data);
    }
    catch (ex) {
        console.log(ex.message);
    }
}
 //________________________________________________________________________________//
//-----------------------Return to Production Line page---------------------------//
function LoadStageDetailTable() {
    try {
        debugger;
        DataTables.StageDetailTable = $('#tblBOMComponentLineStageDetail').DataTable({
            dom: '<"pull-right"f>rt<"bottom"ip><"clear">',
            order: [],
            "scrollY": "130px",
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
              { "data": "ComponentLineID", "defaultContent": "<i>-</i>" },//1
              { "data": "Stage.Description", "defaultContent": "<i>-</i>" },
              { "data": "EntryType", "defaultContent": "<i>-</i>" },//3
              {
                  "data": "PartType", render: function (data, type, row) {
                      debugger;
                      switch (data) {
                          case "RAW":
                              return "Raw Material"
                          case "SUB":
                              return "Sub Component"
                          case "COM":
                              return "Component"
                          default:
                              return "<i>-</i>"
                      }
                  }, "defaultContent": "<i>-</i>"
              },
              {
                  "data": null, render: function (data, type, row) {
                      debugger;
                      switch (row.PartType) {
                          case "RAW":
                              return row.Material.Description
                          case "SUB":
                              return row.SubComponent.Description
                          case "COM":
                              return row.Product.Name
                          default:
                              return "<i>-</i>"
                      }
                  }, "defaultContent": "<i>-</i>"
              },//5
              { "data": "Qty", "defaultContent": "<i>-</i>" },
              { "data": null, "orderable": false, "defaultContent": '<a href="#" class="actionLink"  onclick="EditStageDetail(this)" ><i class="glyphicon glyphicon-pencil" aria-hidden="true"></i></a>' },
              { "data": null, "orderable": false, "defaultContent": '<a href="#" class="DeleteLink"  onclick="DeleteStageDetail(this)" ><i class="glyphicon glyphicon-trash" aria-hidden="true"></i></a>' },
            ],
            columnDefs: [
                { "targets": [0, 1], "visible": false, "searchable": false },
                { className: "text-left", "targets": [2, 3, 4, 5, 6], },
                { className: "text-center", "targets": [7, 8], "width": "3%" },
                { "targets": [0, 1], "bSortable": false }
            ],
            destroy: true
        });
    }
    catch (ex) {
        console.log(ex.message);
    }
}
//ComponentLine On Change
function ComponentLineOnChange(id) {
    try {
        DataTables.StageDetailTable.clear().rows.add(GetBOMComponentLineStageDetail(id)).draw(false);
    }
    catch (ex) {
        console.log(ex.message);
    }
}
//---------------------------Get BOMComponentLineSageDetail----------------------//
function GetBOMComponentLineStageDetail(id) {
    try {
        debugger;
        var data = { "componentLineID": id };
        var result = "";
        var message = "";
        _BOMComponentLineStageDetailList = [];
        var jsonData = GetDataFromServer("BillOfMaterial/GetBOMComponentLineStageDetail/", data);
        if (jsonData != '') {
            jsonData = JSON.parse(jsonData);
            result = jsonData.Result;
            message = jsonData.Message;
            _BOMComponentLineStageDetailList = jsonData.Records;
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
        return _BOMComponentLineStageDetailList;
    }
    catch (ex) {
        console.log(ex.message);
    }
}
//----------------------------Entry Type On Change-------------------------------//
function EntryTypeOnChange(value) {
    try {
        debugger;
        switch (value) {
            case "Input":
                $('#BOMComponentLineStageDetail_PartType').find('option').prop("disabled", false);
                $('#BOMComponentLineStageDetail_PartType').find('option[value="COM"]').prop("disabled", true);
                if ($('#BOMComponentLineStageDetail_PartType').val() === "COM") {
                    $('#BOMComponentLineStageDetail_PartType').val("SUB").trigger('change');
                }
                break;
            case "Output":
                $('#BOMComponentLineStageDetail_PartType').find('option').prop("disabled", false);
                $('#BOMComponentLineStageDetail_PartType').find('option[value="RAW"]').prop("disabled", true);
                if ($('#BOMComponentLineStageDetail_PartType').val() === "RAW") {
                    $('#BOMComponentLineStageDetail_PartType').val("SUB").trigger('change');
                }
                break;
            default:
                $('#BOMComponentLineStageDetail_PartType').find('option').prop("disabled", false);
                break;
        }
    }
    catch (ex) {
        console.log(ex.message);
    }
}
//-----------------------Part Type On Change--------------------------------------//
function PartTypeOnChange(value) {
    try {
        debugger;
        switch (value) {
            case "RAW":
                $("#divItemSelector").children().hide();
                $("#divMaterial").show();
                break;
            case "SUB":
                $("#divItemSelector").children().hide();
                $("#divSubComponent").show();
                break;
            case "COM":
                $("#divItemSelector").children().hide();
                $("#divComponent").show();
                break;
            default:
                $("#divItemSelector").children().hide();
                $("#divMaterial").show();
                break;
        }
    }
    catch (ex) {
        console.log(ex.message);
    }
}
//----------------------LoadPartialStageDropdownForLine---------------------//
function LoadPartialStageDropdownForLine(StageID) {
    try {
        debugger;
        var BOMComponentLineViewModel = new Object();
        BOMComponentLineViewModel.ID = $('#BOMComponentLineStageDetail_ComponentLineID').val();
        BOMComponentLineViewModel.BOMComponentLineStageDetail = new Object();
        BOMComponentLineViewModel.BOMComponentLineStageDetail.StageID = StageID;

        var data = { "bOMComponentLineVM": BOMComponentLineViewModel }
        $('#divStageList').load("StageDropdownForLine", data);
    }
    catch (ex) {
        console.log(ex.message);
    }
}
//---------------------------Save BOMComponentLineStageDetail---------------------------//
function SaveDetail() {
    try {
        $("#btnSave").click();
    }
    catch (ex) {
        console.log(ex.message);
    }
}
//After Save Success
function SaveDetailSuccess(data, status) {
    try {
        debugger;
        var JsonResult = JSON.parse(data)
        var result = JsonResult.Result;
        var message = JsonResult.Message;
        var BOMComponentLineStageDetailViewModel = new Object();
        BOMComponentLineStageDetailViewModel = JsonResult.Records;
        switch (result) {
            case "OK":
                message = BOMComponentLineStageDetailViewModel.Message;
                notyAlert("success", message)
                AddStageDetailToTable(BOMComponentLineStageDetailViewModel.ID)
                ClearStageDetail();
                break;
            case "ERROR":
                notyAlert("danger", message)
                break;
            default:
                notyAlert("danger", message)
                break;
        }
    }
    catch (ex) {
        console.log(ex.message);
    }
}
//Add to StageDetails Table
function AddStageDetailToTable(id) {
    try {
        debugger;
        var BOMComponentLineStageDetailViewModel = new Object();
        BOMComponentLineStageDetailViewModel.ID = id;
        BOMComponentLineStageDetailViewModel.ComponentLineID = $('#BOMComponentLineStageDetail_ComponentLineID').val();
        BOMComponentLineStageDetailViewModel.Stage = new Object();
        BOMComponentLineStageDetailViewModel.StageID = BOMComponentLineStageDetailViewModel.Stage.ID = $('#StageID').val();
        BOMComponentLineStageDetailViewModel.Stage.Description = $($('#StageID')).children("option[value='" + $('#StageID').val() + "']").first().html();

        BOMComponentLineStageDetailViewModel.EntryType = $('#BOMComponentLineStageDetail_EntryType').val();
        BOMComponentLineStageDetailViewModel.PartType = $('#BOMComponentLineStageDetail_PartType').val();
        switch (BOMComponentLineStageDetailViewModel.PartType) {
            case "RAW":
                BOMComponentLineStageDetailViewModel.Material = new Object();
                BOMComponentLineStageDetailViewModel.PartID = BOMComponentLineStageDetailViewModel.Material.ID = $('#MaterialID').val();
                BOMComponentLineStageDetailViewModel.Material.Description = $($('#MaterialID')).children("option[value='" + $('#MaterialID').val() + "']").first().html();

                break;
            case "SUB":
                BOMComponentLineStageDetailViewModel.SubComponent = new Object();
                BOMComponentLineStageDetailViewModel.PartID = BOMComponentLineStageDetailViewModel.SubComponent.ID = $('#SubComponentID').val();
                BOMComponentLineStageDetailViewModel.SubComponent.Description = $($('#SubComponentID')).children("option[value='" + $('#SubComponentID').val() + "']").first().html();

                break;
            case "COM":
                BOMComponentLineStageDetailViewModel.Product = new Object();
                BOMComponentLineStageDetailViewModel.PartID = BOMComponentLineStageDetailViewModel.Product.ID = $('#ProductID').val();
                BOMComponentLineStageDetailViewModel.Product.Name = $($('#ProductID')).children("option[value='" + $('#ProductID').val() + "']").first().html();

                break;
            default:

                break;
        }
        BOMComponentLineStageDetailViewModel.Qty = $('#BOMComponentLineStageDetail_Qty').val();
        BindStageDetailTable(BOMComponentLineStageDetailViewModel);
    }
    catch (ex) {
        console.log(ex.message);
    }
}
//Bind StageDetails Table
function BindStageDetailTable(BOMComponentLineStageDetailViewModel) {
    try {
        debugger;
        _BOMComponentLineStageDetailList = DataTables.StageDetailTable.rows().data();
        if (_BOMComponentLineStageDetailList.length > 0) {
            for (var i = 0; i < _BOMComponentLineStageDetailList.length; i++) {
                if (_BOMComponentLineStageDetailList[i].ID === BOMComponentLineStageDetailViewModel.ID) {

                    _BOMComponentLineStageDetailList[i].ComponentLineID = BOMComponentLineStageDetailViewModel.ComponentLineID;
                    _BOMComponentLineStageDetailList[i].Stage = new Object();
                    _BOMComponentLineStageDetailList[i].StageID = _BOMComponentLineStageDetailList[i].Stage.ID = BOMComponentLineStageDetailViewModel.StageID;
                    _BOMComponentLineStageDetailList[i].Stage.Description = BOMComponentLineStageDetailViewModel.Stage.Description;
                    _BOMComponentLineStageDetailList[i].EntryType = BOMComponentLineStageDetailViewModel.EntryType;
                    _BOMComponentLineStageDetailList[i].PartType = BOMComponentLineStageDetailViewModel.PartType;
                    _BOMComponentLineStageDetailList[i].PartID = BOMComponentLineStageDetailViewModel.PartID;
                    _BOMComponentLineStageDetailList[i].Qty = BOMComponentLineStageDetailViewModel.Qty;
                    switch (BOMComponentLineStageDetailViewModel.PartType) {
                        case "RAW":
                            _BOMComponentLineStageDetailList[i].Material = new Object();
                            _BOMComponentLineStageDetailList[i].Material.ID = BOMComponentLineStageDetailViewModel.Material.ID;
                            _BOMComponentLineStageDetailList[i].Material.Description
                            break;
                        case "SUB":
                            _BOMComponentLineStageDetailList[i].SubComponent = new Object();
                            _BOMComponentLineStageDetailList[i].SubComponent.ID = BOMComponentLineStageDetailViewModel.SubComponent.ID
                            _BOMComponentLineStageDetailList[i].SubComponent.Description
                            break;
                        case "COM":
                            _BOMComponentLineStageDetailList[i].Product = new Object();
                            _BOMComponentLineStageDetailList[i].Product.ID = BOMComponentLineStageDetailViewModel.Product.ID
                            _BOMComponentLineStageDetailList[i].Product.Name
                            break;
                        default:
                            break;
                    }
                }
            }
        }
        else
        {
            _BOMComponentLineStageDetailList.push(BOMComponentLineStageDetailViewModel);
        }
        DataTables.StageDetailTable.clear().rows.add(_BOMComponentLineStageDetailList).draw(false);
    }
    catch (ex) {
        console.log(ex.message);
    }
}
//Clear Details
function ClearStageDetail() {
    try {
        debugger;

        $('#BOMComponentLineStageDetail_ID').val(EmptyGuid);
        $('#BOMComponentLineStageDetail_IsUpdate').val('false');
        $('#StageID').val("").trigger('change');
        $('#BOMComponentLineStageDetail_EntryType').val("Input").trigger('change');
        $('#BOMComponentLineStageDetail_PartType').val("RAW").trigger('change');
        $('#MaterialID').val("").select2().trigger('change');
        $('#SubComponentID').val("").select2().trigger('change');
        $('#ProductID').val("").select2().trigger('change');
        $('#BOMComponentLineStageDetail_Qty').val(0.00);
    }
    catch (ex) {
        console.log(ex.message);
    }
}
//-------------------------Return to ProductionLine----------------------------//
function GoBack() {
    try {
        debugger;
        $('#step3').removeClass('active').addClass('disabled');
        $('#step2').removeClass('disabled').addClass('active');
        var BillOfMaterialViewModel = new Object();
        BillOfMaterialViewModel.ID = $('#IDBillOfMaterial').val();
        BillOfMaterialViewModel.IsUpdate = $('#IsUpdateBOM').val();
        BillOfMaterialViewModel.Product = new Object();
        BillOfMaterialViewModel.Product.Name = $('#Product_Name').val();
        var data = { "billOfMaterialVM": BillOfMaterialViewModel }
        $('#divPartial').load("AddProductionLine", data);
    }
    catch (ex) {
        console.log(ex.message);
    }
}
//-----------------------------Delete StageDetail--------------------------------//
function DeleteStageDetail(curobj) {
    try {
        debugger;

        var billOfMaterialLineStageDetailVM = DataTables.StageDetailTable.row($(curobj).parents('tr')).data();
        var rowindex = DataTables.StageDetailTable.row($(curobj).parents('tr')).index();

        if ((billOfMaterialLineStageDetailVM !== null) && (billOfMaterialLineStageDetailVM.ID !== EmptyGuid)) {
            var res = notyConfirm('Are you sure to delete?', 'DeleteBOMComponentLineStageDetail("' + billOfMaterialLineStageDetailVM.ID + '",' + rowindex + ')');
        }
        else {
            notyConfirm('Are you sure to delete?', 'DeleteTempDetail("' + rowindex + '")');
        }
    }
    catch (ex) {
        console.log(ex.message);
    }
}
//Delete Temp Detail from StageDetail DataTable
function DeleteTempDetail(rowindex) {
    debugger;
    DataTables.StageDetailTable.row(rowindex).remove().draw(true);
}
//Delete BOMComponentLineStageDetail from DB
function DeleteBOMComponentLineStageDetail(id, rowindex) {
    try {
        debugger;
        var data = { "id": id };
        var result = "";
        var message = "";
        var BOMComponentLineStageDetailViewModel = new Object();
        var jsonData = GetDataFromServer("BillOfMaterial/DeleteBOMComponentLineStageDetail/", data);
        if (jsonData != '') {
            jsonData = JSON.parse(jsonData);
            result = jsonData.Result;
            message = jsonData.Message;
            BOMComponentLineStageDetailViewModel = jsonData.Record;
        }
        switch (result) {
            case "OK":
                notyAlert('success', message);
                DeleteTempDetail(rowindex);
                break;
            case "ERROR":
                notyAlert('error', message);
                break;
            default:
                break;
        }
        return BOMComponentLineStageDetailViewModel;
    }
    catch (ex) {
        console.log(ex.message);
    }
}
//----------------------------Edit StageDetail--------------------------------//
function EditStageDetail(thisObj) {
    try {
        debugger;
        var BOMComponentLineStageDetail = DataTables.StageDetailTable.row($(thisObj).parents('tr')).data();
        $('#BOMComponentLineStageDetail_ID').val(BOMComponentLineStageDetail.ID);
        $('#BOMComponentLineStageDetail_IsUpdate').val('true');
        $('#BOMComponentLineStageDetail_ComponentLineID').val(BOMComponentLineStageDetail.ComponentLineID);//.trigger('change');
        LoadPartialStageDropdownForLine(BOMComponentLineStageDetail.StageID);
        $('#StageID').val(BOMComponentLineStageDetail.StageID).trigger('change');
        $('#BOMComponentLineStageDetail_EntryType').val(BOMComponentLineStageDetail.EntryType).trigger('change');
        $('#BOMComponentLineStageDetail_PartType').val(BOMComponentLineStageDetail.PartType).trigger('change');
        switch (BOMComponentLineStageDetail.PartType) {
            case "RAW":
                $('#MaterialID').val(BOMComponentLineStageDetail.PartID).select2().trigger('change');
                break;
            case "SUB":
                $('#SubComponentID').val(BOMComponentLineStageDetail.PartID).select2().trigger('change');
                break;
            case "COM":
                $('#ProductID').val(BOMComponentLineStageDetail.PartID).select2().trigger('change');
                break;
            default:
                break;
        }
        $('#BOMComponentLineStageDetail_Qty').val(BOMComponentLineStageDetail.Qty);

    }
    catch (ex) {
        console.log(ex.message);
    }
}
 //__________________________________________End_______________________________________________//
////////////////////////////////////////////////////////////////////////////////////////////////
