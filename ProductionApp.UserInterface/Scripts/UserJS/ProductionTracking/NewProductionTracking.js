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

$(document).ready(function () {
    try{
        debugger;
        LoadProductionTrackingSearchTable();
        ProductionTrackingInit();
        $('#EmployeeID').select2();
    } catch (ex) {
        console.log(ex.message);
    }
});

function ProductionTrackingInit() {
    try{
        debugger;
        $('.input-group-addon').each(function () {
            $(this).parent().css("width", "100%");
            $(this).remove();
        });

        $('#ProductionTrackingSearch').keypress(function (keyEvent) {
            debugger;
            if (keyEvent.which === 13) {
                debugger;
                keyEevent.preventDefault();
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
        $('#ForemanID').val($('#EmployeeID').val());

        $('#btnSave').trigger('click');

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
            $('#IsUpdate').val('True');
            $('#ID').val(productionTrackingVM.ID)
            message = productionTrackingVM.Message;
            notyAlert("success", message)
            ChangeButtonPatchView('ProductionTracking', 'divButtonPatch', 'Edit');
            break;
        case "ERROR":
            notyAlert("danger", message)
            break;
        default:
            notyAlert("danger", message)
            break;
    }
}
