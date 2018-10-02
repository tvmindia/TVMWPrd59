//*****************************************************************************
//*****************************************************************************
//Author        : Arul
//CreatedDate   : 06-Apr-2018 
//LastModified  : 
//FileName      : NewProductionTracking.js
//Description   : Client side coding for Adding / Updating ProductionTracking
//******************************************************************************
//******************************************************************************

var DataTables = {};
var EmptyGuid = "00000000-0000-0000-0000-000000000000";
var _ProductionTracking = {};
var _Today = '';
$(document).ready(function () {
    try {
        debugger;
        LoadProductionTrackingSearchTable();
        $('#EmployeeID').select2();
        _Today = $('#EntryDate').val();
        ProductionTrackingInit();
        try {
            if ($('#IsUpdate').val() === "True") {
                BindProductionTracking();
                ChangeButtonPatchView('ProductionTracking', 'divButtonPatch', 'Edit');
            }
        } catch (ex) {
            console.log(ex.message);
        }
        BindOrReloadProductionTrackingTable();
    } catch (ex) {
        console.log(ex.message);
    }
});

function ProductionTrackingInit() {
    try{
        debugger;

        $('#ProductionTrackingSearch').keydown(function (event) {
            if (event.which === 13) {
                debugger;
                event.preventDefault();
                ProductionTrackingSearch();
            }
        });

        $("#TrackingDetailSearchDiv").hide();

    } catch (ex) {
        console.log(ex.message);
    }
}

function CancelSearch() {
    $("#TrackingDetailSearchDiv").hide();
}

function LoadProductionTrackingSearchTable() {
    try {
        DataTables.ProductionTrackingSearchTable = $('#TrackingDetailSearchTable').DataTable({
            dom: '<"pull-right"f>rt<"bottom"ip><"clear">',
            order: [],
            "scrollY": "140px",
            "scrollCollapse": true,
            searching: false,
            paging: false,
            data: null,
            //pageLength: 3,
            columns: [
              //{ "data": "Search", "defaultContent": "<i>-</i>" },
              { "data": "Product.ID", "defaultContent": "<i>-</i>" },
              { "data": "BOMComponentLineStageDetail.ID", "defaultContent": "<i>-</i>" },
              { "data": "SubComponent.Description", "defaultContent": "<i>-</i>", "width": "30%" },
              {
                  "data": "SearchDetail", render: function (data, type, row) {
                      return data
                  }, "defaultContent": "<i>-</i>", "width": "40%"
              },
              { "data": null, "orderable": false, "defaultContent": '<a  href="#" class="actionLink" style="background:lightgreen;border-radius:1em;padding: 0.25em .75em; aria-hidden="true"  onclick="SelectDetail(this)" ><i>Select</i></a>' }
            ],
            columnDefs: [
                 { "targets": [0, 1], "visible": false, "searchable": false },
                 { className: "text-left", "targets": [2, 3] },
                 { className: "text-center", "width": "10%", "targets": [4] },
                 { "bSortable": false, "aTargets": [0, 1, 2, 3, 4] }
            ],
        });
    } catch (ex) {
        console.log(ex.message);
    }
}

function ProductionTrackingSearch() {
    try{
        debugger;
        $("#TrackingDetailSearchDiv").show();
        var search = $('#ProductionTrackingSearch').val();
        DataTables.ProductionTrackingSearchTable.clear().rows.add(GetProductionTrackingSearchList(search)).draw(false);
    } catch (ex) {
        console.log(ex.message);
    }
}

function GetProductionTrackingSearchList(search) {
    try{
        debugger;
        var data = { "searchTerm": search };
        var result = "";
        var message = "";
        var productionTrackingList = [];
        var jsonData = GetDataFromServer("ProductionTracking/GetProductionTrackingSearchList/", data);
        if (jsonData != '') {
            jsonData = JSON.parse(jsonData);
            result = jsonData.Result;
            message = jsonData.Message;
            productionTrackingList = jsonData.Records;
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
        return productionTrackingList;
    }
    catch (ex) {
        console.log(ex.message);
    }
}

function SelectDetail(curObj){
    try{
        debugger;
        $('#msgSearch').hide();
        _ProductionTracking = DataTables.ProductionTrackingSearchTable.row($(curObj).parents('tr')).data();

        $('#LineStageDetailID').val(_ProductionTracking.LineStageDetailID);
        $('#ProductID').val(_ProductionTracking.ProductID);
        $('#lblSubComponent').text(_ProductionTracking.SubComponent.Description);

        var Detail = _ProductionTracking.SearchDetail.split('<br/>');
        $('#lblProduct').text(Detail[0].replace("<b>Product</b>: ", ''));
        $('#lblStage').text(Detail[1].replace("<b>Stage</b>: ", ''));
        $('#lblComponent').text(Detail[2].replace("<b>Component</b>: ", ''));

        CancelSearch();
    } catch (ex) {
        console.log(ex.message);
    }
}

function Save() {
    try{
        debugger;
        var isInput = true;
        if ($('#EmployeeID').val() !== "") {
            $('#ForemanID').val($('#EmployeeID').val());
        }
        else {
            $('#ForemanID').val(EmptyGuid);
            isInput = false;
            $('#msgForemanID').show();
            $('#EmployeeID').change(function () {
                $('#msgForemanID').hide();
            });

        }
        if ($('#EntryDate').val() === "") {
            isInput = false;
            $('#msgEntryDate').show();
            $('#EntryDate').change(function () {
                $('#msgEntryDate').hide();
            });

        }
        if ($('#ProductID').val() === "" || $('#LineStageDetailID').val() === "" || $('#ProductID').val() === EmptyGuid || $('#LineStageDetailID').val() === EmptyGuid) {
            isInput = false;
            $('#msgSearch').show();

        }
        if (isInput) {
            $('#btnSave').trigger('click');
        }

    } catch (ex) {
        console.log(ex.message);
    }
}

function SaveSuccess(data, status) {
    debugger;
    var JsonResult = JSON.parse(data)
    var result = JsonResult.Result;
    var message = JsonResult.Message;
    var productionTrackingVM = new Object();
    productionTrackingVM = JsonResult.Records;
    switch (result) {
        case "OK":
            //$('#IsUpdate').val('True');
            $('#ID').val(productionTrackingVM.ID)
            message = productionTrackingVM.Message;
            //notyAlert("success", message);
            //Reset(1);
            NewTracking();
            BindOrReloadProductionTrackingTable();
            ChangeButtonPatchView('ProductionTracking', 'divButtonPatch', 'Add');
            break;
        case "ERROR":
            notyAlert("danger", message)
            break;
        default:
            notyAlert("danger", message)
            break;
    }
}

function DeleteClick() {
    try{
        debugger;
        var id = $('#ID').val();
        if (id !== EmptyGuid)
            notyConfirm('Are you sure to delete?', 'DeleteProductionTracking("' + id + '")');
        else
            notyAlert('error', "Cannot Delete");
    } catch (ex) {
        console.log(ex.message);
    }
}

function DeleteProductionTracking(id) {
    try{
        debugger;
        var LineStageID = $('#LineStageDetailID').val();
        var data = { "id": id, "lineStageID": LineStageID };
        var result = "";
        var message = "";
        var ProductionTrackingViewModel = new Object();
        var jsonData = GetDataFromServer("ProductionTracking/DeleteProductionTracking/", data);
        if (jsonData != '') {
            jsonData = JSON.parse(jsonData);
            result = jsonData.Result;
            message = jsonData.Message;
            ProductionTrackingViewModel = jsonData.Record;
        }
        switch (result) {
            case "OK":
                notyAlert('success', message);
                window.location.replace("NewProductionTracking?code=PROD");
                break;
            case "ERROR":
                notyAlert('error', message);
                break;
            default:
                break;
        }
        return ProductionTrackingViewModel;
    } catch (ex) {
        console.log(ex.message);
    }
}

function GetProductionTracking() {
    try{
        debugger;
        var id = $('#ID').val();
        var data = { "id": id };
        var result = "";
        var message = "";
        var ProductionTrackingViewModel = new Object();
        var jsonData = GetDataFromServer("ProductionTracking/GetProductionTracking/", data);
        if (jsonData != '') {
            jsonData = JSON.parse(jsonData);
            result = jsonData.Result;
            message = jsonData.Message;
            ProductionTrackingViewModel = jsonData.Record;
        }
        switch (result) {
            case "OK":
                //notyAlert('success', message);
                break;
            case "ERROR":
                notyAlert('error', message);
                break;
            default:
                break;
        }
        return ProductionTrackingViewModel;
    } catch (ex) {
        console.log(ex.message);
    }
}

function BindProductionTracking() {
    try {
        debugger;
        _ProductionTracking = new Object();
        _ProductionTracking = GetProductionTracking();
        $('#EntryDate').val(_ProductionTracking.EntryDateFormatted);
        $('#EmployeeID').val(_ProductionTracking.ForemanID).select2();
        $('#ProductionRefNo').val(_ProductionTracking.ProductionRefNo);
        $('#AcceptedQty').val(_ProductionTracking.AcceptedQty);
        $('#AcceptedWt').val(_ProductionTracking.AcceptedWt);
        $('#DamagedQty').val(_ProductionTracking.DamagedQty);
        $('#DamagedWt').val(_ProductionTracking.DamagedWt);
        $('#Remarks').val(_ProductionTracking.Remarks);
        $('#lblProduct').text(_ProductionTracking.Product.Name);
        $('#lblComponent').text(_ProductionTracking.Component.Name);
        $('#lblStage').text(_ProductionTracking.Stage.Description);
        $('#lblSubComponent').text(_ProductionTracking.SubComponent.Description === null ? _ProductionTracking.OutputComponent.Name : _ProductionTracking.SubComponent.Description);
        $('#ProductID').val(_ProductionTracking.ProductID)
        $('#LineStageDetailID').val(_ProductionTracking.LineStageDetailID)
    } catch (ex) {
        console.log(ex.message);
    }
}

function Reset() {
    try {
        debugger;
        if ($('#IsUpdate').val() === "True") {
            var id = $('#ID').val();
            window.location.replace("NewProductionTracking?code=PROD&id=" + id + "");
        } else {
            window.location.replace("NewProductionTracking?code=PROD");
        }
        
    } catch (ex) {
        console.log(ex.message);
    }
}

function BindOrReloadProductionTrackingTable() {
    try {
        debugger;
        ProductionTrackingAdvanceSearchViewModel = new Object();
        DataTablePagingViewModel = new Object();
        DataTablePagingViewModel.Length = 0;
        ProductionTrackingAdvanceSearchViewModel.DataTablePaging = DataTablePagingViewModel;
        ProductionTrackingAdvanceSearchViewModel.Product = new Object();
        ProductionTrackingAdvanceSearchViewModel.Product.ID = null;
        ProductionTrackingAdvanceSearchViewModel.Employee = new Object();
        ProductionTrackingAdvanceSearchViewModel.Employee.ID = null;
        ProductionTrackingAdvanceSearchViewModel.Stage = new Object();
        ProductionTrackingAdvanceSearchViewModel.Stage.ID = null;
        //apply datatable plugin on ProductionTracking table
        DataTables.ProductionTrackingList = $('#tblProductionTracking').DataTable(
        {
            dom: '<"pull-right"f>rt<"bottom"ip><"clear">',
            ordering: false,
            searching: false,
            paging: true,
            processing: true,
            language: {

                "processing": "<div class='spinner'><div class='bounce1'></div><div class='bounce2'></div><div class='bounce3'></div></div>"
            },
            serverSide: true,
            ajax: {
                url: "GetAllProductionTracking/",
                data: { "productionTrackingAdvanceSearchVM": ProductionTrackingAdvanceSearchViewModel },
                type: 'POST'
            },
            pageLength: 10,
            columns: [
            { "data": "ID", "defaultContent": "<i>-</i>" },
            { "data": "EntryDateFormatted", "defaultContent": "<i>-</i>", "width": '22%' },//1
            //{ "data": "Product.Name", "defaultContent": "<i>-</i>" },
            //{ "data": "Component.Name", "defaultContent": "<i>-</i>" },//3
            //{ "data": "Stage.Description", "defaultContent": "<i>-</i>" },
            {
                "data": null, render: function (data, type, row) {
                    if (row.SubComponent.Description !== null) {
                        return row.SubComponent.Description
                    } else {
                        return row.OutputComponent.Name
                    }
                }, "defaultContent": "<i>-</i>"
            },//5
            //{ "data": "Employee.Name", "defaultContent": "<i>-</i>" },
            { "data": "AcceptedQty", "defaultContent": "<i>-</i>" },
            { "data": "DamagedQty", "defaultContent": "<i>-</i>" },

            { "data": null, "orderable": false, "defaultContent": '<a href="#" onclick="Edit(this)"<i class="glyphicon glyphicon-share-alt" aria-hidden="true"></i></a>', "width": '3%' }
            ],
            columnDefs: [{ "targets": [0], "visible": false, "searchable": false },
                { className: "text-left", "targets": [2] },
                { className: "text-center", "targets": [1, 5] },
                { className: "text-right", "targets": [3, 4] }],
            destroy: true
        });

        $('#tblProductionTracking tbody').on('dblclick', 'td', function () {
            Edit(this);
        });

    } catch (ex) {
        console.log(e.message);
    }
}

function Edit(curObj) {
    try {
        debugger;
        OnServerCallBegin();
        var ProductionTrackingViewModel = DataTables.ProductionTrackingList.row($(curObj).parents('tr')).data();
        $('#ID').val(ProductionTrackingViewModel.ID);
        $('#IsUpdate').val('True');
        BindProductionTracking();
        ChangeButtonPatchView('ProductionTracking', 'divButtonPatch', 'Edit');
        BindOrReloadProductionTrackingTable();
        CancelSearch();
        //window.location.replace("/ProductionTracking/NewProductionTracking?code=PROD&id=" + ProductionTrackingViewModel.ID);
        OnServerCallComplete();
    }
    catch (e) {
        console.log(e.message);
    }
}

function NewTracking() {
    debugger;
    $('#EntryDate').val(_Today);
    $('#ID').val(EmptyGuid);
    $('#IsUpdate').val('False');
    $('#EmployeeID').val("").trigger('change');
    $('#ProductionRefNo').val("");
    $('#ProductionTrackingSearch').val("");
    $('#lblSubComponent').text("N/A");
    $('#lblProduct').text("N/A");
    $('#lblStage').text("N/A");
    $('#lblComponent').text("N/A");
    $('#AcceptedQty').val(0);
    $('#AcceptedWt').val(0);
    $('#DamagedQty').val(0);
    $('#DamagedWt').val(0);
    $('#Remarks').val("");
    $('#ProductID').val(EmptyGuid);
    $('#LineStageDetailID').val(EmptyGuid);
}