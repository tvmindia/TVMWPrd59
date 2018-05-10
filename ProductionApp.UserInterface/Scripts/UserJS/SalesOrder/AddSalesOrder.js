﻿//*****************************************************************************
//*****************************************************************************
//Author: Gibin Jacob
//CreatedDate: 12-Feb-2018 
//LastModified: 05-Mar-2018 
//FileName: AddSalesOrder.js
//Description: Client side coding for Add/Edit Sales Order
//******************************************************************************
//******************************************************************************

//--Global Declaration--//
var DataTables = {};
var EmptyGuid = "00000000-0000-0000-0000-000000000000";
var _SalesOrderDetail = [];
var _SalesOrderDetailList = [];
var _SlNo = 1;

var _result = "";
var _message = "";
var _jsonData = {};

$(document).ready(function () {
    try {
        $("#divCustomerDropdown").load('/Customer/CustomerDropdown')
        $("#divEmployeeDropdown").load('/Employee/EmployeeDropdown')
        // $("#divProductDropdown").load('/Product/ProductDropdown')
        $("#ReferenceCustomer").select2({});

        $('#btnUpload').click(function () {
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
        });//

        DataTables.SalesOrderDetailTable = $('#tblSalesOrderDetail').DataTable(
      {
          dom: '<"pull-right"f>rt<"bottom"ip><"clear">',
          ordering: false,
          searching: false,
          paging: false,
          data: null,
          "bInfo": false,
          autoWidth: false,
          columns: [ 
           {
               "data": "", render: function (data, type, row) {
                   return _SlNo++
               }, "width": "5%"
           },
          {
              "data": "Product.Name", render: function (data, type, row) {
                  row.Product.HSNNo=row.Product.HSNNo == null ? "Nill" : row.Product.HSNNo
                  return data + '</br><b>HSNNo: </b>' + row.Product.HSNNo + '</br><b>Expected Delivery: </b>' + row.ExpectedDeliveryDateFormatted
              }, "defaultContent": "<i></i>", "width": "21%"
          },
          { "data": "Quantity", render: function (data, type, row) { return data + ' ' + row.UnitCode }, "defaultContent": "<i></i>", "width": "10%" },
          { "data": "Rate", render: function (data, type, row) { return roundoff(data) }, "defaultContent": "<i></i>", "width": "10%" },
          { "data": "TradeDiscountAmount", render: function (data, type, row) { return data }, "defaultContent": "<i></i>", "width": "10%" },
          { "data": "GrossAmount", render: function (data, type, row) { return roundoff(data) }, "defaultContent": "<i></i>", "width": "10%" },
          { "data": "TaxTypeDescription", "defaultContent": "<i></i>", "width": "7%" },
          { "data": "TaxAmount", render: function (data, type, row) { return roundoff(data) }, "defaultContent": "<i></i>", "width": "10%" },
          { "data": "NetAmount", render: function (data, type, row) { return roundoff(data) }, "defaultContent": "<i></i>", "width": "10%" },
          { "data": null, "orderable": false, "defaultContent": '<a href="#" class="actionLink"  onclick="ItemDetailsEdit(this)" ><i class="glyphicon glyphicon-edit" aria-hidden="true"></i></a>  |  <a href="#" class="DeleteLink"  onclick="Delete(this)" ><i class="glyphicon glyphicon-trash" aria-hidden="true"></i></a>', "width": "7%" }
          ],
          columnDefs: [{ "targets": [], "visible": false, searchable: false },
              { className: "text-center", "targets": [0,9] },
              { className: "text-right", "targets": [3,4,5,7,8] },
              { className: "text-left", "targets": [6,2,1] }
          ]
      });

        $("#ProductID").change(function () {
            BindProductDetails(this.value);
            ProductValueCalculation();
        });
        $("#CustomerID").change(function () {
            BindCustomerDetails(this.value);
            
        });
        $(".Calculation").change(function () {
            ProductValueCalculation();
        });

        $("#TaxTypeCode").change(function () {
            ProductValueCalculation();
        });
        
        if ($('#IsUpdate').val() == 'True') {
            BindSalesOrderByID()
        }
        else {
            $('#lblSalesOrderNo').text('Sales Order# : New');
        }
    }
    catch (e) {
        console.log(e.message);
    }
});



function BindCustomerDetails(customerId)
{
    var customerVM = GetCustomerDetails(customerId)
    $('#BillingAddress').val(customerVM.BillingAddress);
    $('#ShippingAddress').val(customerVM.ShippingAddress);

}
function GetCustomerDetails(customerId) {
    try {
        var data = { "customerId": customerId };

        _jsonData = GetDataFromServer("SalesOrder/GetCustomerDetails/", data);
        if (_jsonData != '') {
            _jsonData = JSON.parse(_jsonData);
        }
        if (_jsonData.Result == "OK") {
            return _jsonData.Records;
        }
        if (_jsonData.Result == "ERROR") {
            alert(_jsonData.Message);
        }
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}


function ItemDetailsEdit(curObj) {
    debugger;
    $('#SalesOrderDetailsModal').modal('show');
    ClearSalesOrderDetailsModalFields();
    _SlNo = 1;
    var rowData = DataTables.SalesOrderDetailTable.row($(curObj).parents('tr')).data();
    $('#ProductID').attr("disabled", true);
    $('#modelContextLabel').text("Edit Sales Order Details");

    $("#ProductID").val(rowData.ProductID).trigger('change');
    $('#SalesOrderDetail_Rate').val(rowData.Rate);
    $('#SalesOrderDetail_Quantity').val(rowData.Quantity);
    $('#SalesOrderDetail_DiscountPercent').val(rowData.DiscountPercent);
    $('#SalesOrderDetail_TradeDiscountAmount').val(rowData.TradeDiscountAmount);
    $('#SalesOrderDetail_ExpectedDeliveryDateFormatted').val(rowData.ExpectedDeliveryDateFormatted);
    $('#TaxTypeCode').val(rowData.TaxTypeCode);
    ProductValueCalculation();
}

function ShowSalesOrderDetailsModal()
{
    $('#ProductID').attr("disabled", false);
    $('#modelContextLabel').text("Add Sales Order Details");

    $('#SalesOrderDetailsModal').modal('show');
    ClearSalesOrderDetailsModalFields();
    $('#SalesOrderDetail_ExpectedDeliveryDateFormatted').val($('#ExpectedDeliveryDateFormatted').val());

}

function ClearSalesOrderDetailsModalFields()
{
    $('#ProductID').val('').select2();
    $('#SalesOrderDetail_Product_Name').val('');
    $('#SalesOrderDetail_Product_HSNNo').val('');
    $('#SalesOrderDetail_UnitCode').val('');
    $('#SalesOrderDetail_Rate').val('');
    $('#SalesOrderDetail_Quantity').val('');
    $('#SalesOrderDetail_GrossAmount').val(roundoff(0));
    $('#SalesOrderDetail_TradeDiscountAmount').val(roundoff(0));
    $('#SalesOrderDetail_DiscountPercent').val(0);
    $('#SalesOrderDetail_TaxableAmount').val(roundoff(0));
    $('#TaxTypeCode').val('');
    $('#SalesOrderDetail_TaxAmount').val(roundoff(0));
    $('#SalesOrderDetail_NetAmount').val(roundoff(0));
    $('#SalesOrderDetail_ExpectedDeliveryDateFormatted').val('');
    $('#lblCurrentStock').text('0');
}

function BindProductDetails(ID)
{
    if (ID != "")
    {
        var result = GetProduct(ID);
        $('#SalesOrderDetail_Product_Name').val(result.Name);
        $('#SalesOrderDetail_Product_HSNNo').val(result.HSNNo);
        $('#SalesOrderDetail_UnitCode').val(result.UnitCode);
        $('#SalesOrderDetail_Rate').val(result.Rate);
        $('#lblCurrentStock').text(result.CurrentStock);
    }
    else
    {
        ClearSalesOrderDetailsModalFields();
    }
}
function GetProduct(ID) {
    try {
        var data = { "ID": ID };
         
        _jsonData = GetDataFromServer("Product/GetProduct/", data);
        if (_jsonData != '') {
            _jsonData = JSON.parse(_jsonData);
        }
        if (_jsonData.Result == "OK") {
            return _jsonData.Records;
        }
        if (_jsonData.Result == "ERROR") {
            alert(_jsonData.Message);
        }
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}

function ProductValueCalculation()
{
    var product,rate, qty, discpercent, disc=0, taxTypeCode, taxableAmt=0,taxAmt=0, netAmt=0, GrossAmt=0
    product=$('#ProductID').val();
    rate = $('#SalesOrderDetail_Rate').val();
    qty = $('#SalesOrderDetail_Quantity').val();

    if (rate != "" && qty != "" && product!="")
    {
        //--------------------Gross Amount-----------------------//
        GrossAmt = rate * qty;
        $('#SalesOrderDetail_GrossAmount').val(roundoff(GrossAmt));

        //--------------------Discount Amount--------------------//
        discpercent = $('#SalesOrderDetail_DiscountPercent').val();
        if (discpercent > 100)//if greater than 100% set percentage to 0%
        {
            $('#SalesOrderDetail_DiscountPercent').val(0);
            discpercent = 0;
        }
        if (discpercent != "" && discpercent!=0)
        {
            disc = GrossAmt * (discpercent / 100);
            $('#SalesOrderDetail_TradeDiscountAmount').val(roundoff(disc));
        }
        else {
            disc = $('#SalesOrderDetail_TradeDiscountAmount').val() == "" ? 0 : $('#SalesOrderDetail_TradeDiscountAmount').val();
            if(GrossAmt<disc)
            {
                $('#SalesOrderDetail_TradeDiscountAmount').val(roundoff(0));
                disc = 0;
            }
        }
        //--------------------Taxable Amount---------------------//
        taxableAmt = roundoff(parseFloat(GrossAmt) - parseFloat(disc));
        $('#SalesOrderDetail_TaxableAmount').val(taxableAmt);

        //--------------------Tax Amount------------------------//
        taxTypeCode = $('#TaxTypeCode').val();
        if (taxTypeCode != "")
        {
            var taxTypeVM = GetTaxTypeByCode(taxTypeCode);
            var CGSTAmt = parseFloat(taxableAmt) * parseFloat(parseFloat(taxTypeVM.CGSTPercentage) / 100);
            var SGSTAmt = parseFloat(taxableAmt) * parseFloat(parseFloat(taxTypeVM.SGSTPercentage) / 100);
            var IGSTAmt = parseFloat(taxableAmt) * parseFloat(parseFloat(taxTypeVM.IGSTPercentage) / 100);
            taxAmt = CGSTAmt + SGSTAmt + IGSTAmt;
        }
        //----------------------Net Amount---------------------//
        $('#SalesOrderDetail_TaxAmount').val(roundoff(taxAmt));
        netAmt = parseFloat(taxableAmt) + parseFloat(taxAmt);
        $('#SalesOrderDetail_NetAmount').val(roundoff(netAmt));
    }
}

function ClearDiscountPercentage()
{
    $('#SalesOrderDetail_DiscountPercent').val(0);
}

function GetTaxTypeByCode(Code) {
    try {
        var data = { "Code": Code };

        var taxTypeVM = new Object();
        _jsonData = GetDataFromServer("TaxType/GetTaxtype/", data);
        if (_jsonData != '') {
            _jsonData = JSON.parse(_jsonData);
            result = _jsonData.Result;
            message = _jsonData.Message;
            taxTypeVM = _jsonData.Records;
        }
        if (result == "OK") {
            return taxTypeVM;
        }
        if (result == "ERROR") {
            alert(Message);
        }
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}

function AddSalesOrderDetails()
{
    debugger;
    var rate=$('#SalesOrderDetail_Rate').val();
    var qty=$('#SalesOrderDetail_Quantity').val();
    var date=$('#SalesOrderDetail_ExpectedDeliveryDateFormatted').val();
    var productId=$('#ProductID').val();
    if(rate !="" && qty!="" && date!="" && productId !="" )
    {
        _SalesOrderDetail = [];
        SalesOrderDetailVM = new Object();
       // SalesOrderDetailVM.ID = EmptyGuid;
        SalesOrderDetailVM.ProductID = $("#ProductID").val();
        SalesOrderDetailVM.Product = new Object();
        SalesOrderDetailVM.Product.Name = $('#SalesOrderDetail_Product_Name').val();
        SalesOrderDetailVM.Product.HSNNo = $('#SalesOrderDetail_Product_HSNNo').val();
        SalesOrderDetailVM.UnitCode = $('#SalesOrderDetail_UnitCode').val();
        SalesOrderDetailVM.Quantity = $('#SalesOrderDetail_Quantity').val();
        SalesOrderDetailVM.Rate = $('#SalesOrderDetail_Rate').val();
        SalesOrderDetailVM.ExpectedDeliveryDateFormatted = $('#SalesOrderDetail_ExpectedDeliveryDateFormatted').val();
        SalesOrderDetailVM.GrossAmount = $('#SalesOrderDetail_GrossAmount').val();
        SalesOrderDetailVM.TradeDiscountAmount = $('#SalesOrderDetail_TradeDiscountAmount').val();
        SalesOrderDetailVM.DiscountPercent = $('#SalesOrderDetail_DiscountPercent').val();
        SalesOrderDetailVM.TaxAmount = $('#SalesOrderDetail_TaxAmount').val();
        SalesOrderDetailVM.NetAmount = $('#SalesOrderDetail_NetAmount').val();
        SalesOrderDetailVM.TaxTypeCode = $('#TaxTypeCode').val();
        if (SalesOrderDetailVM.TaxTypeCode != "")
            SalesOrderDetailVM.TaxTypeDescription = $('#TaxTypeCode option:selected').text();
        else
            SalesOrderDetailVM.TaxTypeDescription = null;
        _SalesOrderDetail.push(SalesOrderDetailVM);

        if (_SalesOrderDetail != null) {
            //check product existing or not if soo update the new
            var SalesOrderDetailList = DataTables.SalesOrderDetailTable.rows().data();
            if (SalesOrderDetailList.length > 0) {
                var checkPoint = 0;
                for (var i = 0; i < SalesOrderDetailList.length; i++) {
                    if (SalesOrderDetailList[i].ProductID == $("#ProductID").val())
                    {
                        SalesOrderDetailList[i].Quantity = $('#SalesOrderDetail_Quantity').val();
                        SalesOrderDetailList[i].Rate = $('#SalesOrderDetail_Rate').val();
                        SalesOrderDetailList[i].ExpectedDeliveryDateFormatted = $('#SalesOrderDetail_ExpectedDeliveryDateFormatted').val();
                        SalesOrderDetailList[i].GrossAmount = $('#SalesOrderDetail_TaxableAmount').val();
                        SalesOrderDetailList[i].TradeDiscountAmount = $('#SalesOrderDetail_TradeDiscountAmount').val();
                        SalesOrderDetailList[i].DiscountPercent = $('#SalesOrderDetail_DiscountPercent').val();
                        SalesOrderDetailList[i].TaxAmount = $('#SalesOrderDetail_TaxAmount').val();
                        SalesOrderDetailList[i].NetAmount = $('#SalesOrderDetail_NetAmount').val();
                        SalesOrderDetailList[i].TaxTypeCode = $('#TaxTypeCode').val();
                        if (SalesOrderDetailList[i].TaxTypeCode != "")
                            SalesOrderDetailList[i].TaxTypeDescription = $('#TaxTypeCode option:selected').text();
                        else
                            SalesOrderDetailList[i].TaxTypeDescription = null;
                        checkPoint = 1;
                        break;
                    }
                }
                if (!checkPoint) {
                    DataTables.SalesOrderDetailTable.rows.add(_SalesOrderDetail).draw(false);
                }
                else {
                    DataTables.SalesOrderDetailTable.clear().rows.add(SalesOrderDetailList).draw(false);
                }
            }
            else {
                DataTables.SalesOrderDetailTable.rows.add(_SalesOrderDetail).draw(false);
            }
        }
        CalculateDetailTableSummary();
        $('#SalesOrderDetailsModal').modal('hide');
    }
    else
    {
        notyAlert('warning', "Please check the Required Fields");
    }
}

function Save() {
    $("#DetailJSON").val('');
    _SalesOrderDetailList = [];
    AddSalesOrderDetailList();
    if (_SalesOrderDetailList.length > 0) {
        var result = JSON.stringify(_SalesOrderDetailList);
        $("#DetailJSON").val(result);
        $('#btnSave').trigger('click');
    }
    else {
        notyAlert('warning', 'Please Add Item Details!');
    }

}

function AddSalesOrderDetailList() {
    debugger;
    var SalesOrderDetailList = DataTables.SalesOrderDetailTable.rows().data();
    for (var r = 0; r < SalesOrderDetailList.length; r++) {
        SalesOrderDetailVM = new Object();
        SalesOrderDetailVM.ID = SalesOrderDetailList[r].ID;
        SalesOrderDetailVM.ProductID = SalesOrderDetailList[r].ProductID
        SalesOrderDetailVM.Quantity = SalesOrderDetailList[r].Quantity
        SalesOrderDetailVM.Rate = SalesOrderDetailList[r].Rate
        SalesOrderDetailVM.ExpectedDeliveryDateFormatted = SalesOrderDetailList[r].ExpectedDeliveryDateFormatted
        SalesOrderDetailVM.TradeDiscountAmount = SalesOrderDetailList[r].TradeDiscountAmount
        SalesOrderDetailVM.DiscountPercent = SalesOrderDetailList[r].DiscountPercent
        SalesOrderDetailVM.TaxTypeCode = SalesOrderDetailList[r].TaxTypeCode == "" ? null : SalesOrderDetailList[r].TaxTypeCode;
        SalesOrderDetailVM.UnitCode = SalesOrderDetailList[r].UnitCode

        _SalesOrderDetailList.push(SalesOrderDetailVM);
    }
}
function SaveSuccessSalesOrder(data, status) {
    _jsonData = JSON.parse(data)
    switch (_jsonData.Result) {
        case "OK":
            notyAlert("success", _jsonData.Records.Message)
            $('#IsUpdate').val('True');
            $('#ID').val(_jsonData.Records.ID)
            BindSalesOrderByID();
            break;
        case "ERROR":
            notyAlert("danger", _jsonData.Message)
            break;
        default:
            notyAlert("danger", _jsonData.Message)
            break;
    }
}

function BindSalesOrderByID()
{
    ChangeButtonPatchView('SalesOrder', 'divbuttonPatchAddSalesOrder', 'Edit');
    var ID = $('#ID').val();
    _SlNo = 1;
    var salesOrderVM = GetSalesOrderByID(ID);
    $('#lblSalesOrderNo').text('Sales Order# :' + salesOrderVM.OrderNo);
    $('#OrderNo').val(salesOrderVM.OrderNo);
    $('#OrderDateFormatted').val(salesOrderVM.OrderDateFormatted);
    $('#ExpectedDeliveryDateFormatted').val(salesOrderVM.ExpectedDeliveryDateFormatted);
    $('#hdnEmployeeID').val(salesOrderVM.SalesPerson);
    $('#hdnCustomerID').val(salesOrderVM.CustomerID);
    $('#ReferenceCustomer').val(salesOrderVM.ReferenceCustomer).select2();
    $('#Remarks').val(salesOrderVM.Remarks);
    $('#BillingAddress').val(salesOrderVM.BillingAddress);
    $('#ShippingAddress').val(salesOrderVM.ShippingAddress);
    //detail Table values binding with header id
    BindSalesOrderDetailTable(ID);
    PaintImages(ID);//bind attachments
    CalculateDetailTableSummary();
}

function GetSalesOrderByID(ID)
{
    try {
        var data = { "ID": ID };
        _jsonData = GetDataFromServer("SalesOrder/GetSalesOrder/", data);
        if (_jsonData != '') {
            _jsonData = JSON.parse(_jsonData);
        }
        if (_jsonData.Result == "OK") {
            return _jsonData.Records;
        }
        if (_jsonData.Result == "ERROR") {
            alert(_jsonData.Message);
        }
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}

function BindSalesOrderDetailTable(ID) {
    DataTables.SalesOrderDetailTable.clear().rows.add(GetSalesOrderDetail(ID)).draw(false);
}

function GetSalesOrderDetail(ID) {
    try {
        var data = { "ID": ID };
        _jsonData = GetDataFromServer("SalesOrder/GetSalesOrderDetail/", data);
        if (_jsonData != '') {
            _jsonData = JSON.parse(_jsonData);
        }
        if (_jsonData.Result == "OK") {
            return _jsonData.Records;
        }
        if (_jsonData.Result == "ERROR") {
            alert(_jsonData.Message);
        }
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}

function Delete(curobj) {
    var rowData = DataTables.SalesOrderDetailTable.row($(curobj).parents('tr')).data();
    var Rowindex = DataTables.SalesOrderDetailTable.row($(curobj).parents('tr')).index();

    if ((rowData != null) && (rowData.ID != null)) {
        notyConfirm('Are you sure to delete?', 'DeleteItem("' + rowData.ID + '")');
    }
    else {
        var res = notyConfirm('Are you sure to delete?', 'DeleteTempItem("' + Rowindex + '")');

    }
}

function DeleteTempItem(Rowindex) {
    var Itemtabledata = DataTables.SalesOrderDetailTable.rows().data();
    Itemtabledata.splice(Rowindex, 1);
    _SlNo = 1;
    DataTables.SalesOrderDetailTable.clear().rows.add(Itemtabledata).draw(false);
    notyAlert('success', 'Deleted Successfully');
    CalculateDetailTableSummary();
}

function DeleteItem(ID) {

    try {
        var data = { "ID": ID };
        _jsonData = GetDataFromServer("SalesOrder/DeleteSalesOrderDetail/", data);
        if (_jsonData != '') {
            _jsonData = JSON.parse(_jsonData);
        }
        switch (_jsonData.Result) {
            case "OK":
                notyAlert('success', _jsonData.Message);
                BindSalesOrderByID();
                break;
            case "ERROR":
                notyAlert('error', _jsonData.Message);
                break;
            default:
                break;
        }
        return _jsonData.Record;
    }
    catch (e) {

        notyAlert('error', e.message);
    }
}

function DeleteClick() {
    notyConfirm('Are you sure to delete?', 'DeleteSalesOrder()');
}

function DeleteSalesOrder() {
    try {
        var id = $('#ID').val();
        if (id != '' && id != null) {
            var data = { "ID": id };
            var ds = {};
            ds = GetDataFromServer("SalesOrder/DeleteSalesOrder/", data);
            if (ds != '') {
                ds = JSON.parse(ds);
            }
            if (ds.Result == "OK") {
                notyAlert('success', ds.Record.Message);
                window.location.replace("AddSalesOrder?code=SALE");
            }
            if (ds.Result == "ERROR") {
                notyAlert('error', ds.Message);
                return 0;
            }
            return 1;
        }
    }
    catch (e) {
        notyAlert('error', e.message);
        return 0;
    }
}

function Reset()
{
    BindSalesOrderByID();
}
//------------Table Summary Calculation to display below detail table---------
function CalculateDetailTableSummary()
{
    debugger;
    var taxableAmount = 0, totalGST = 0, grandTotal = 0;
    var SalesOrderDetailList = DataTables.SalesOrderDetailTable.rows().data();
    if (SalesOrderDetailList.length > 0) {
        for (var i = 0; i < SalesOrderDetailList.length; i++)
        {
            taxableAmount = taxableAmount + parseFloat(SalesOrderDetailList[i].GrossAmount);
            totalGST = totalGST + parseFloat(SalesOrderDetailList[i].TaxAmount);
            grandTotal = grandTotal + parseFloat(SalesOrderDetailList[i].NetAmount);
        }
    }
    $('#lblTaxableAmount').text(roundoff(taxableAmount));
    $('#lblTotalGST').text(roundoff(totalGST));
    $('#lblGrandTotal').text(roundoff(grandTotal));
}