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
//-------------------------------------------------------------

$(document).ready(function () {
    try {
        debugger;
        $('#ProductID').select2({});

        try{
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
                          return '<input class="form-control text-right " name="Markup" value="' + row.Qty + '" type="text" onclick="SelectAllValue(this);" onkeypress = "return isNumber(event)", onchange="TextBoxValueChanged(this);">';
                      }, "defaultContent": "<i>-</i>"
                  },
                  { "data": null, "orderable": false, "defaultContent": '<a href="#" class="DeleteLink"  onclick="Delete(this)" ><i class="glyphicon glyphicon-trash" aria-hidden="true"></i></a>  |  <a href="#"></a>' },
                ],
                columnDefs: [{ orderable: false, className: 'select-checkbox', "targets": 1 },
                    { "targets": [0, 1], "visible": false, "searchable": false },
                    { className: "text-right", "targets": [4], "width": "15%" },
                    { className: "text-left", "targets": [2, 3], "width": "40%" },
                    { className: "text-center", "targets": [5], "width":"5%" },
                    { "targets": [3, 4, 5], "bSortable": false }
                ],
                select: { style: 'multi', selector: 'td:first-child' },
                destroy: true
            });
        } catch (ex) {
            console.log(ex.message);
        }
        
        if ($('#IsUpdateBOM').val() === 'True' && $('#IDBillOfMaterial') !== EmptyGuid) {
            BindBillOfMaterial();
            ChangeButtonPatchView('BillOfMaterial', 'divButtonPatch', 'Edit');
        }
        DescriptionOnChange();

        $('#hdnMasterCall').val("OTR");
        $('.close').click(AddProduct);
        //$('#ComponentSection').hide();
        $('#LineSection').hide();

        $("#selectable").selectable();
        $("#selected").selectable();
    }
    catch (ex) {
        console.log(ex.message);
    }
});

function DescriptionOnChange(currObj) {
    if ($('#DescriptionBOM').val() !== "") {
        $('#lblDescription').text($('#DescriptionBOM').val());
    }
    else {
        $('#lblDescription').text('BOM: New');
    }
}


function LoadComponents() {
    $('#ProductListModal').modal('show');
    BindProductList();
}

function BindProductList() {
    try{
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

function AddComponent() {
    try{
        debugger;
        _IsInput = true;
        $('#hdnMasterCall').val("MSTR");
        AddProductMaster('MSTR');
    } catch (ex) {
        console.log(ex.message);
    }
}

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

function RebindComponentList(_BillOfMaterialDetailList) {
    try{
        debugger;
        var checkPoint = 0;
        if (_BillOfMaterialDetailList != null) {
            DataTables.ComponentList.rows.add(_BillOfMaterialDetailList).draw(true);
        }
    } catch (ex) {
        console.log(ex.message);
    }
}

function SaveComponentDetail() {
    try {
        debugger;
        //$("#DetailJSON").val('');
        //_BillOfMaterialDetailList = [];
        //GetDetailsFromTable();
        //if (_BillOfMaterialDetailList.length > 0) {
        //    var result = JSON.stringify(_BillOfMaterialDetailList);
        //    $("#DetailJSON").val(result);
        //    $('#btnSave').trigger('click');
        LoadPartialAddProductionLine();
        //}
        //else {
        //    notyAlert('warning', 'Please Add BillOfMaterial Details!');
        //}
    }
    catch (ex) {
        console.log(ex.message);
    }
}

function SaveSuccess(data, status) {
    debugger;
    var JsonResult = JSON.parse(data)
    var result = JsonResult.Result;
    var message = JsonResult.Message;
    var billOfMaterialVM = new Object();
    billOfMaterialVM = JsonResult.Records;
    switch (result) {
        case "OK":
            $('#IsUpdateBOM').val('True');
            $('#IDBillOfMaterial').val(billOfMaterialVM.ID)
            message = billOfMaterialVM.Message;
            notyAlert("success", message)
            BindBillOfMaterial();
            break;
        case "ERROR":
            notyAlert("danger", message)
            break;
        default:
            notyAlert("danger", message)
            break;
    }
}

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

function GetBillOfMaterial(id) {
    try {
        debugger;
        var data = { "id": id };
        var result = "";
        var message = "";
        var billOfMaterialVM = new Object();
        var jsonData = GetDataFromServer("BillOfMaterial/GetBillOfMaterial/", data);
        if (jsonData != '') {
            jsonData    = JSON.parse(jsonData);
            result      = jsonData.Result;
            message     = jsonData.Message;
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

function Delete(curobj) {
    try {
        var billOfMaterialDetailVM = DataTables.ComponentList.row($(curobj).parents('tr')).data();
        var rowindex = DataTables.ComponentList.row($(curobj).parents('tr')).index();

        if ((billOfMaterialDetailVM !== null) && (billOfMaterialDetailVM.ID !== EmptyGuid)) {
            notyConfirm('Are you sure to delete?', 'DeleteBillOfMaterialDetail("' + billOfMaterialDetailVM.ID + '",' + rowindex + ')');
        }
        else {
            var res = notyConfirm('Are you sure to delete?', 'DeleteTempItem("' + rowindex + '")');
        }
    }
    catch (ex) {
        console.log(ex.message);
    }
}

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
    DeleteTempItem(rowindex);
}

function DeleteTempItem(rowindex) {
    debugger;
    DataTables.ComponentList.row(rowindex).remove().draw(true);
}

function LoadPartialAddProductionLine() {
    try{
        debugger;
        $('#step1').removeClass('active').addClass('disabled');
        $('#step2').removeClass('disabled').addClass('active');
        var BillOfMaterialVM = new Object();
        BillOfMaterialVM.ID = $('#IDBillOfMaterial').val();
        BillOfMaterialVM.IsUpdate = $('#IsUpdateBOM').val();
        BillOfMaterialVM.Product = new Object();
        BillOfMaterialVM.ProductID = BillOfMaterialVM.Product.ID = $('#ProductID').val();
        BillOfMaterialVM.Product.Name = $($('#ProductID')).children("option[value='" + $('#ProductID').val() + "']").first().html();
        var data = { "billOfMaterialVM": BillOfMaterialVM }
        $('#divPartial').load("AddProductionLine", data);
    }
    catch (ex) {
        console.log(ex.message);
    }
}

function OnStageSelect() {
    var txt = [];
    $("#selectable li.ui-selected").each(function () {
        txt.push($(this).text());
        $("#selected").append('<li class="ui-widget-content ui-selectee">' + $(this).text() + '</li>');
        $(this).remove();
    });
}

function OnStageDeselect() {
    var txt = [];
    $("#selected li.ui-selected").each(function () {
        txt.push($(this).text());
        $("#selectable").append('<li class="ui-widget-content ui-selectee">' + $(this).text() + '</li>');
        $(this).remove();
    });
}
