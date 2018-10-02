//*****************************************************************************
//*****************************************************************************
//Author: Arul
//CreatedDate: 12-Apr-2018 
//FileName: ViewProductionTracking.js
//Description: Client side coding for Listing ProductionTracking
//******************************************************************************
//******************************************************************************

//--Global Declaration--//
var _dataTables = {};
var _emptyGuid = "00000000-0000-0000-0000-000000000000";
var _dateList = [];
var _productionTrackingList = [];
var _isTrue = true;

$(document).ready(function () {
    try {
        debugger;
        BindOrReloadProductionTrackingTable('Init');
        $('#ProductID,#EmployeeID,#StageID').select2({});
        $('#SearchTerm').keypress(function (event) {
            if (event.which === 13) {
                event.preventDefault();
                BindOrReloadProductionTrackingTable('Apply');
            }
        });
        BindPendingProductionTrackingDetailTable();
        BindPendingProductionTrackingTable();
        debugger;
        var _dateList = GetAllAvailableProductionTrackingEntryDate()
        if (_dateList === null) {
            _dateList = [$('#PostDate').val()]
        }
        $('#PostDate').datepicker({
            startDate: _dateList[0],
            endDate: _dateList[_dateList.length - 1],
            defaultViewDate: _dateList[_dateList.length-1],
            format: "dd-M-yyyy",
            autoclose: true,
            todayBtn: true,
            clearBtn: true,
            //datesDisabled: _dateList,
            //beforeShowDay:
            //  function (date) {
            //      return {"enabled": FetchAvailableDates(date)};
            //  },
        });
        $('#PostDate').addClass('datepicker');
        
    }
    catch (ex) {
        console.log(ex.message);
    }
});

function BindOrReloadProductionTrackingTable(action) {
    try {
        debugger;
        ProductionTrackingAdvanceSearchViewModel = new Object();
        DataTablePagingViewModel = new Object();
        DataTablePagingViewModel.Length = 0;
        //switch case to check the operation
        switch (action) {
            case 'Reset':
                $('#SearchTerm').val('');
                $('#FromDate').val('');
                $('#ToDate').val('');
                $('#ProductID').val('').trigger('change');
                $('#EmployeeID').val('').trigger('change');
                $('#StageID').val('').trigger('change');
                $('#Status').val('');
                $('#IsDamaged').val('');
                break;
            case 'Init':
                break;
            case 'Apply':
                break;
            case 'Export':
                DataTablePagingViewModel.Length = -1;
                break;
            default:
                break;
        }
        ProductionTrackingAdvanceSearchViewModel.DataTablePaging = DataTablePagingViewModel;
        ProductionTrackingAdvanceSearchViewModel.SearchTerm = $('#SearchTerm').val();
        ProductionTrackingAdvanceSearchViewModel.FromDate = $('#FromDate').val();
        ProductionTrackingAdvanceSearchViewModel.ToDate = $('#ToDate').val();
        ProductionTrackingAdvanceSearchViewModel.ProductID = $('#ProductID').val();
        ProductionTrackingAdvanceSearchViewModel.EmployeeID = $('#EmployeeID').val();
        ProductionTrackingAdvanceSearchViewModel.StageID = $('#StageID').val();
        ProductionTrackingAdvanceSearchViewModel.Status = $('#Status').val();
        ProductionTrackingAdvanceSearchViewModel.IsDamaged = $('#IsDamaged').val();

        //apply datatable plugin on ProductionTracking table
        _dataTables.ProductionTrackingList = $('#tblProductionTracking').DataTable(
        {
            dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
            buttons: [{
                extend: 'excel',
                exportOptions:
                             {
                                 columns: [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11/*, 12*/]
                             }
            }],
            ordering: false,
            searching: false,
            paging: true,
            lengthChange: false,
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
            { "data": "EntryDateFormatted", "defaultContent": "<i>-</i>", "width": '7.5%' },//1
            { "data": "Product.Name", "defaultContent": "<i>-</i>" },
            { "data": "Component.Name", "defaultContent": "<i>-</i>" },//3
            { "data": "Stage.Description", "defaultContent": "<i>-</i>" },
            {
                "data": null, render: function (data, type, row) {
                    if (row.SubComponent.Description !== null) {
                        return row.SubComponent.Description
                    } else {
                        return row.OutputComponent.Name
                    }
                }, "defaultContent": "<i>-</i>"
            },//5
            { "data": "Employee.Name", "defaultContent": "<i>-</i>" },
            { "data": "AcceptedQty", "defaultContent": "<i>-</i>" },//7
            { "data": "AcceptedWt", "defaultContent": "<i>-</i>" },
            { "data": "DamagedQty", "defaultContent": "<i>-</i>" },//9
            { "data": "DamagedWt", "defaultContent": "<i>-</i>" },
            //{ "data": "ProductionRefNo", "defaultContent": "<i>-</i>", "width": '6%' },//11
            //{ "data": "Remarks", "defaultContent": "<i>-</i>", "width": '8%' },
            {
                "data": "PostedBy", "defaultContent": "<i>-</i>",
                render: function (data, type, row) {
                    if (data !== null) {
                        return "Posted"
                    } else {
                        return "Unposted"
                    }
                }
            },

            { "data": null, "orderable": false, "defaultContent": '<a href="#" onclick="Edit(this)"><i class="glyphicon glyphicon-edit" aria-hidden="true"></i></a>', "width": '3%' }
            ],
            columnDefs: [{ "targets": [0], "visible": false, "searchable": false },
                { className: "text-left", "targets": [7, 8, 9, 10], "width": '5%' },
                { className: "text-right", "targets": [2, 3, 4, 5, 6, 11/*, 12*/] },
                { className: "text-center", "targets": [1, 12] }],
            destroy: true,
            //for performing the import operation after the data loaded
            initComplete: function (settings, json) {
                debugger;
                if (action === 'Export') {
                    if (json.data.length > 0) {
                        if (json.data[0].TotalCount > 10000) {
                            MasterAlert("info", 'We are able to download maximum 10000 rows of data, There exist more than 10000 rows of data please filter and download')
                        }
                    }
                    $(".buttons-excel").trigger('click');
                    BindOrReloadProductionTrackingTable('Search');
                }
            }
        });
        $(".buttons-excel").hide();

        $('#tblProductionTracking tbody').on('dblclick', 'td', function () {
            Edit(this);
        });

    } catch (ex) {
        console.log(e.message);
    }
}

function Edit(curObj) {
    try {
        var ProductionTrackingViewModel = _dataTables.ProductionTrackingList.row($(curObj).parents('tr')).data();
        window.location.replace("/ProductionTracking/NewProductionTracking?code=PROD&id=" + ProductionTrackingViewModel.ID);
    }
    catch (ex) {
        console.log(ex.message);
    }
}


function LoadPendingProductionTrackingPopUp() {
    try {
        $('#btnBackward').hide();
        $('#ProductionTrackingRecordsModal').modal('show');
        ViewProductionTrackingList(2);
    }
    catch (ex) {
        console.log(ex.message);
    }
}

function ReloadPendingProductionTrackingTable() {
    try {
        debugger;
        $('#btnSaveItems').hide();
        _isTrue = true;
        _dataTables.PendingTrackingList.clear().rows.add(GetPendingProductionTracking()).draw(true);
        if (_isTrue) {
            //To display add button in pop up
            $('#btnPostItems').show();
        }
    }
    catch (ex) {
        console.log(ex.message);
    }
}

function GetPendingProductionTracking() {
    try {
        debugger;
        var postDate = $('#PostDate').val();
        var data = { "postDate": postDate };

        var productionTrackingList = new Object();
        var result = "";
        var message = "";
        var jsonData = GetDataFromServer("ProductionTracking/GetPendingProductionTracking/", data);
        if (jsonData != '') {
            jsonData = JSON.parse(jsonData);
            result = jsonData.Result;
            message = jsonData.Message;
            productionTrackingList = jsonData.Records;
        }
        if (result == "OK") {
            return productionTrackingList;
        }
        if (result == "ERROR") {
            notyAlert('error', message);
        }
    }
    catch (ex) {
        console.log(ex.message);
    }
}

function BindPendingProductionTrackingTable() {
    try {
        debugger;
            //apply datatable plugin on ProductionTracking table
            _dataTables.PendingTrackingList = $('#tblPendingTrackingList').DataTable(
            {
                dom: '<"pull-right"f>rt<"bottom"ip><"clear">',
                ordering: false,
                searching: true,
                paging: true,
                lengthChange: false,
                processing: true,
                language: {
                    "processing": "<div class='spinner'><div class='bounce1'></div><div class='bounce2'></div><div class='bounce3'></div></div>"
                },
                data: null,
                pageLength: 8,
                columns: [
                { "data": "SlNo", "defaultContent": "<i>-</i>" },
                { "data": "Product.Name", "defaultContent": "<i>-</i>" },
                {
                    "data": "SubComponent.Description", render: function (data, type, row) {
                        if (data !== null) {
                            return data
                        } else {
                            return row.OutputComponent.Name
                        }
                    }, "defaultContent": "<i>-</i>"
                },//1
                { "data": "PreviousQty", "defaultContent": "<i>-</i>" },//2
                { "data": "AcceptedQty", "defaultContent": "<i>-</i>" },
                { "data": "DamagedQty", "defaultContent": "<i>-</i>" },
                { "data": "TotalQty", "defaultContent": "<i>-</i>" },//5
                {
                    "data": "IsValid", render: function (data, type, row) {
                        debugger;
                        if (data) {
                            return '<a href="#" onclick="ReloadPendingProductionTrackingDetailTable(this)" class="actionLink" style="background:lightgreen;border-radius:1em;padding: 0.25em 1.25em;" aria-hidden="true"> OK </a>'/* <-alt255*/
                        } else {
                            $('#btnPostItems').hide();
                            _isTrue = false;
                            return '<a href="#" onclick="ReloadPendingProductionTrackingDetailTable(this)"  class="actionLink" style="background:Salmon;border-radius:1em;padding: 0.25em .75em;" aria-hidden="true"> Error </a>'
                        }
                    }, "defaultContent": "<i>-</i>"
                },
                ],
                columnDefs: [{ "targets": [], "visible": false, "searchable": false },
                    { className: "text-left", "targets": [1, 2] },
                    { className: "text-right", "targets": [3, 4, 5, 6], "width": '10%' },
                    { className: "text-center", "targets": [0, 7] }],
                destroy: true
            });

    } catch (ex) {
        console.log(ex.message);
    }
}



function ViewProductionTrackingList(value) {
    try {
        $('#tabDetail').attr('data-toggle', 'tab');
        $('#btnBackward').hide();
        $('#btnSaveItems').hide();
        if (value)
            $('#tabList').trigger('click');
        if(value===2)
            ReloadPendingProductionTrackingTable();
        if (_isTrue) {
            //To display add button in pop up
            $('#btnPostItems').show();
        }
        _dataTables.PendingTrackingDetail.rows().clear();
    } catch (ex) {
        console.log(ex.message);
    }
}

function ReloadPendingProductionTrackingDetailTable(curObj) {
    try {
        debugger;
        $('#ErrorMsg').text('');
        var ProductionTrackingViewModel = _dataTables.PendingTrackingList.row($(curObj).parents('tr')).data();
        if (ProductionTrackingViewModel !== null)
            RebindPendingProductionTrackingDetailTable(ProductionTrackingViewModel.LineStageDetailID)
        $('#lblProduct').text(ProductionTrackingViewModel.Product.Name);
        $('#lblSubComponent').text(ProductionTrackingViewModel.SubComponent.Description !== null ? ProductionTrackingViewModel.SubComponent.Description : ProductionTrackingViewModel.OutputComponent.Name);
        $('#lblPreviousQty').text(ProductionTrackingViewModel.PreviousQty);
        $('#lblAcceptedQty').text(ProductionTrackingViewModel.AcceptedQty);
        $('#lblTotalQty').text(ProductionTrackingViewModel.TotalQty);
        $('#lblSlNo').text(ProductionTrackingViewModel.SlNo);
        $('#ErrorMsg').append(ProductionTrackingViewModel.ErrorMessage);
        ViewProductionTrackingDetails(1);
    }
    catch (ex) {
        console.log(ex.message);
    }
}

function ViewProductionTrackingDetails(value) {
    try {
        debugger;
        if (value !== undefined) {
            $('#tabDetail').attr('data-toggle', 'tab');
            $('#btnPostItems').hide();
            $('#btnSaveItems').show();
            $('#btnBackward').show();
            $('#tabDetail').trigger('click');
        } else {
            debugger;
            if (_dataTables.PendingTrackingDetail.rows().data().length < 1) {
                $('#tabList').trigger('click');
                $('#tabDetail').attr('data-toggle', '');
            }
        }
    } catch (ex) {

        console.log(ex.message);
    }
}

function GetPendingProductionTrackingDetail(lineStageDetailID) {
    try {
        debugger;
        var postDate = $('#PostDate').val();
        var data = { "postDate": postDate, "lineStageDetailID": lineStageDetailID };

        var ProductionTrackingViewModel = new Object();
        var result = "";
        var message = "";
        var jsonData = GetDataFromServer("ProductionTracking/GetPendingProductionTrackingDetail/", data);
        if (jsonData != '') {
            jsonData = JSON.parse(jsonData);
            result = jsonData.Result;
            message = jsonData.Message;
            ProductionTrackingViewModel = jsonData.Records;
        }
        if (result == "OK") {
            return ProductionTrackingViewModel;
        }
        if (result == "ERROR") {
            notyAlert('error', message);
        }
    } catch (ex) {
        console.log(ex.message);
    }
}

function RebindPendingProductionTrackingDetailTable(lineStageDetailID) {
    try {
        debugger;
        var productionTrackingList = GetPendingProductionTrackingDetail(lineStageDetailID);
        _dataTables.PendingTrackingDetail.clear().rows.add(productionTrackingList).draw(true);
    } catch (ex) {
        console.log(ex.message);
    }
}

function BindPendingProductionTrackingDetailTable() {
    try {
        debugger;
        //apply datatable plugin on ProductionTracking table
        _dataTables.PendingTrackingDetail = $('#tblProductionTrackingDetail').DataTable(
        {
            dom: '<"pull-right"f>rt<"bottom"ip><"clear">',
            ordering: false,
            searching: true,
            paging: true,
            processing: true,
            language: {

                "processing": "<div class='spinner'><div class='bounce1'></div><div class='bounce2'></div><div class='bounce3'></div></div>"
            },
            data: null,
            pageLength: 5,
            columns: [
            { "data": "ID", "defaultContent": "<i>-</i>" },
            { "data": "EntryDateFormatted", "defaultContent": "<i>-</i>" },
            { "data": "Employee.Name", "defaultContent": "<i>-</i>" },
            { "data": "Remarks", "defaultContent": "<i>-</i>" },
            {
                "data": "AcceptedQty", "defaultContent": "<i>-</i>",
                'render': function (data, type, row) {
                    return '<input class="form-control text-right " name="Markup" type="text"  value="' + Math.floor(data) + '"  onclick="SelectAllValue(this);" onkeypress = "return isNumber(event)", onchange="TextBoxValueChanged(this,1);"style="width:100%">';
                }
            },
            {
                "data": "AcceptedWt", "defaultContent": "<i>-</i>",
                'render': function (data, type, row) {
                    return '<input class="form-control text-right " name="Markup" type="text"  value="' + roundoff(data) + '"  onclick="SelectAllValue(this);" onkeypress = "return isNumber(event)", onchange="TextBoxValueChanged(this,2);"style="width:100%">';
                }
            },
            {
                "data": "DamagedQty", "defaultContent": "<i>-</i>",
                'render': function (data, type, row) {
                    return '<input class="form-control text-right " name="Markup" type="text"  value="' + Math.floor(data) + '"  onclick="SelectAllValue(this);" onkeypress = "return isNumber(event)", onchange="TextBoxValueChanged(this,3);"style="width:100%">';
                }
            },
            {
                "data": "DamagedWt", "defaultContent": "<i>-</i>",
                'render': function (data, type, row) {
                    return '<input class="form-control text-right " name="Markup" type="text"  value="' + roundoff(data) + '"  onclick="SelectAllValue(this);" onkeypress = "return isNumber(event)", onchange="TextBoxValueChanged(this,4);"style="width:100%">';
                }
            }//4
            ],
            columnDefs: [{ "targets": [0], "visible": false, "searchable": false },
                { className: "text-left", "targets": [3, 2] },
                { className: "text-center", "targets": [1] },
                { className: "text-right", "targets": [7, 4, 5, 6], "width": '10%' }],
            destroy: true//,
            //initComplete: function (settings, json) {
            //    debugger;
            //    //console.log(json.data.length);
            //    //ViewProductionTrackingDetails(1);
            //}
        });

    } catch (ex) {
        console.log(ex.message);
    }
}

//TextBox value change in datatable
function TextBoxValueChanged(thisObj, textBoxCode) {
    try{
        debugger;
        var productionTrackingList = _dataTables.PendingTrackingDetail.rows().data();
        var table = _dataTables.PendingTrackingDetail;
        var productionTrackingVM = table.row($(thisObj).parents('tr')).data();

        for (var i = 0; i < productionTrackingList.length; i++) {
            if (productionTrackingList[i].ID == productionTrackingVM.ID) {

                switch (textBoxCode) { //textBoxCode is the code to know, which textbox changed is triggered
                    case 1:
                        if ((thisObj.value != "") && (thisObj.value >= 0))
                            productionTrackingList[i].AcceptedQty = parseInt(thisObj.value);
                        break;
                    case 2:
                        if ((thisObj.value != "") && (thisObj.value >= 0))
                            productionTrackingList[i].AcceptedWt = parseFloat(thisObj.value);
                        break;
                    case 3:
                        if ((thisObj.value != "") && (thisObj.value >= 0))
                            productionTrackingList[i].DamagedQty = parseInt(thisObj.value);
                        break;
                    case 4:
                        if ((thisObj.value != "") && (thisObj.value >= 0))
                            productionTrackingList[i].DamagedWt = parseFloat(thisObj.value);
                        break;
                }
            }
        }

        _dataTables.PendingTrackingDetail.clear().rows.add(productionTrackingList).draw(true);
    } catch (ex) {
        console.log(ex.message);
    }
}

function FetchAvailableDates(date) {
    //debugger;
    dmy = ("0" + date.getDate()).slice(-2) + "-" + ("0" + date.getMonth() + 1).slice(-2) + "-" + date.getFullYear();
    if ($.inArray(dmy, _dateList) != -1) {
        return true;
    } else {
        return false;
    }
}

//To retrieve all available dates inserted so far into Production Tracking
function GetAllAvailableProductionTrackingEntryDate() {
    try {
        debugger;
        _dateList = [];
        var result = "";
        var message = "";
        var jsonData = GetDataFromServer("ProductionTracking/GetAllAvailableProductionTrackingEntryDate/");
        if (jsonData != '') {
            jsonData = JSON.parse(jsonData);
            result = jsonData.Result;
            message = jsonData.Message;
            _dateList = jsonData.Records;
        }
        if (result == "OK") {
            return _dateList;
        }
        if (result == "ERROR") {
            notyAlert('error', message);
        }
    } catch (ex) {
        console.log(ex.message);
    }
}

function Save() {
    try {
        debugger;
        ModifyDataTableList();
        var data = { "productionTrackingJSONList": JSON.stringify(_productionTrackingList) };
        var productionTrackingVM = new Object();
        var result = "";
        var message = "";
        var jsonData = GetDataFromServer("ProductionTracking/UpdateProductionTrackingByXML/", data);
        if (jsonData != '') {
            jsonData = JSON.parse(jsonData);
            result = jsonData.Result;
            message = jsonData.Message;
            productionTrackingVM = jsonData.Records;
        }
        if (result == "OK") {
            ViewProductionTrackingList(2);
            //return productionTrackingVM;
        }
        if (result == "ERROR") {
            notyAlert('error', message);
        }

    } catch (ex) {
        console.log(ex.message);
    }
}

function ModifyDataTableList(){
    try {
        _productionTrackingList = [];
        var productionTrackingList = _dataTables.PendingTrackingDetail.rows().data();

        for (var i = 0; i < productionTrackingList.length; i++) {
            var productionTrackingVM = new Object();
            productionTrackingVM.ID = productionTrackingList[i].ID;
            productionTrackingVM.AcceptedQty = productionTrackingList[i].AcceptedQty;
            productionTrackingVM.AcceptedWt = productionTrackingList[i].AcceptedWt;
            productionTrackingVM.DamagedQty = productionTrackingList[i].DamagedQty;
            productionTrackingVM.DamagedWt = productionTrackingList[i].DamagedWt;
            _productionTrackingList.push(productionTrackingVM);
        }
        
    } catch (ex) {
        console.log(ex.message);
    }
}
    
function ProductionTrackingPosting() {
    try {
        debugger;
        var postDate = $('#PostDate').val();
        var data = { "postDate": postDate };
        var result = "";
        var message = "";
        var jsonData = GetDataFromServer("ProductionTracking/ProductionTrackingPosting/", data);
        if (jsonData != '') {
            jsonData = JSON.parse(jsonData);
            result = jsonData.Result;
            message = jsonData.Message;
        }
        if (result == "OK") {
            $(".close").click();
            ReloadPendingProductionTrackingTable();
            notyAlert('success', message);
        }
        if (result == "ERROR") {
            notyAlert('error', message);
        }
    } catch (ex) {
        console.log(ex.message);
    }
}