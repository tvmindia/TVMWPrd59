//*****************************************************************************
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
var _groupInsert = 0;
var _result = "";
var _message = "";
var _jsonData = {};

$(document).ready(function () {
    try {
        $("#divCustomerDropdown").load('/Customer/CustomerDropdown')
        $("#divEmployeeDropdown").load('/Employee/EmployeeDropdown')
        // $("#divProductDropdown").load('/Product/ProductDropdown')
        $("#ReferenceCustomer").select2({});
        $("#ProductCategoryCode").select2({});
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
                  var HSNNo = row.Product.HSNNo == null ? "Nill" : row.Product.HSNNo;
                  return data + '</br><b>HSNNo: </b>' + HSNNo + '</br><b>Expected Delivery: </b>' + row.ExpectedDeliveryDateFormatted
              }, "defaultContent": "<i></i>", "width": "21%"
          },
          {
              "data": "Quantity", render: function (data, type, row) {
                  debugger; 
                  if (row.UnitCode == null)
                  {
                      return data + ' Sets</br>Wt. ' + row.PkgWt + ' Kg';
                  }
                  else
                  return data;
              }, "defaultContent": "<i></i>", "width": "10%"
          },
          {
              "data": "Rate", render: function (data, type, row) {
                  return roundoff(data);
              }, "defaultContent": "<i></i>", "width": "10%"
          },
          {
              "data": "TradeDiscountAmount", render: function (data, type, row) {
                  return roundoff(data);
              }, "defaultContent": "<i></i>", "width": "10%"
          },
          {
              "data": "GrossAmount", render: function (data, type, row) {
                  return roundoff(data);
              }, "defaultContent": "<i></i>", "width": "10%"
          },
          { "data": "TaxTypeDescription", "defaultContent": "<i></i>", "width": "7%" },
          {
              "data": "TaxAmount", render: function (data, type, row) {
                  return roundoff(data);
              }, "defaultContent": "<i></i>", "width": "10%"
          },
          {
              "data": "NetAmount", render: function (data, type, row) {
                  return roundoff(data);
              }, "defaultContent": "<i></i>", "width": "10%"
          },
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
        $(".GroupCalculation").change(function () {
            GroupValueCalculation();
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
        //------------GroupItemTbl-----------------------------------
        DataTables.GroupProductDetailTable = $('#tblGroupProductDetail').DataTable({
            dom: '<"pull-left"f>rt<"bottom"ip><"clear">',
            ordering: false,
            searching: false,
            paging: true,
            "bInfo": false,
            pageLength: 7, 
            data: null,
            columns: [ 
                 { "data": null, "defaultContent": "", "width": "5%" },
                 {
                     "data": "Name", render: function (data, type, row) { 
                         var IsInvoiceInKG=row.IsInvoiceInKG == 0 ? "" : '(Kg)';
                         return data + ' ' + IsInvoiceInKG;
                     }, "defaultContent": "<i>-</i>", "width": "20%"
                 },
                 { "data": "UnitCode", "defaultContent": "<i>-</i>", "width": "5%" },
                 { "data": "CurrentStock", "defaultContent": "<i>-</i>", "width": "10%" },
                 { "data": "OrderDue", "defaultContent": "<i>-</i>", "width": "10%" },
                 {
                     "data": "NetAvailableQty", "defaultContent": "<i>-</i>",
                     'render': function (data, type, row) {
                         if (row.OrderDue == 0)
                             return row.CurrentStock
                         else
                             return parseFloat(row.CurrentStock) - parseFloat(row.OrderDue)
                     },
                     "width": "10%"
                 },
                 {
                     "data": "Quantity", "defaultContent": "<i>-</i>",
                     'render': function (data, type, row) { 
                             return data 
                     },
                     "width": "10%"           
                 },
                 { "data": "WeightInKG", "defaultContent": "", "width": "10%" },
                 { "data": "CostPrice", "defaultContent": "<i>-</i>", "width": "10%" },
                 {
                     "data": "Amount",
                     'render': function (data, type, row) {
                         if (row.Quantity != "" && row.Quantity != undefined && row.Quantity != null) {
                             if (row.IsInvoiceInKG)
                                 return (row.Quantity * row.WeightInKG) * row.CostPrice;
                             else
                                 return row.Quantity * row.CostPrice;
                         }
                         else
                             return 0;
                     },
                     "defaultContent": "<i>-</i>", "width": "10%"
                 }
                 
            ],
            columnDefs: [{ orderable: false, className: 'select-checkbox', "targets": 0 }
                , { className: "text-left", "targets": [1, 2] }
                , { className: "text-right", "targets": [3,4,5, 6, 7, 8,9] }
                , { "targets": [], "visible": false, "searchable": false }
            ], 
            select: { style: 'multi', selector: 'td:first-child' } 
        });

        DataTables.GroupProductDetailTable.on('select', function (e, dt, type, indexes) {
            GroupValueCalculation();
        });
        DataTables.GroupProductDetailTable.on('deselect', function (e, dt, type, indexes) {
            GroupValueCalculation();
        });
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
    var rowData = DataTables.SalesOrderDetailTable.row($(curObj).parents('tr')).data();
    if (rowData.GroupID == null){
    $('#SalesOrderDetailsModal').modal('show');
    ClearSalesOrderDetailsModalFields();
    _SlNo = 1;
    $('#ProductID').attr("disabled", true);
    $('#modelContextLabel').text("Edit Sales Order Details");
    $('#SalesOrderDetail_Quantity').val(rowData.Quantity);
    $("#ProductID").val(rowData.ProductID).trigger('change');
    $('#SalesOrderDetail_Rate').val(rowData.Rate);
    $('#SalesOrderDetail_DiscountPercent').val(rowData.DiscountPercent);
    $('#SalesOrderDetail_TradeDiscountAmount').val(rowData.TradeDiscountAmount);
    $('#SalesOrderDetail_ExpectedDeliveryDateFormatted').val(rowData.ExpectedDeliveryDateFormatted);
    $('#TaxTypeCode').val(rowData.TaxTypeCode);
    ProductValueCalculation();
    }
    else {
        $('#SalesOrderGroupItemDetailsModal').modal('show');
        ClearSalesOrderGroupItemDetailsModalFields();
        productVM = GetGroupProductList(rowData.GroupID);
        DataTables.GroupProductDetailTable.clear().rows.add(productVM).draw(false);
        debugger;
        //DataTables.GroupProductDetailTable.rows(rowData.SalesOrderID != EmptyGuid).select();
        $('#SalesOrderDetail_GroupName').val(productVM[0].GroupName); 
        $('#SalesOrderDetail_NumOfSet').val(productVM[0].NumOfSet); 
        $('#ProductCategoryCode').val(productVM[0].Product.ProductCategoryCode).select2();
        $("#ProductCategoryCode").attr("disabled", true);
        $('#SalesOrderDetail_GroupID').val(productVM[0].GroupID);
        $('#groupModelContextLabel').text("Edit Sales Order Details"); 
        $('#SalesOrderDetail_GroupItemExpectedDeliveryDateFormatted').val(productVM[0].GroupItemExpectedDeliveryDateFormatted);
        $('#SalesOrderDetail_GroupTaxTypeCode').val(productVM[0].GroupTaxTypeCode);
        $('#SalesOrderDetail_GroupItemDiscountPercent').val(productVM[0].GroupItemDiscountPercent);
        $('#SalesOrderDetail_GroupItemTradeDiscountAmount').val(productVM[0].GroupItemTradeDiscountAmount);
        var GroupDetailList = DataTables.GroupProductDetailTable.rows().data();
        for (var r = 0; r < GroupDetailList.length; r++) {
            if(GroupDetailList[r].SalesOrderID !=EmptyGuid)
                DataTables.GroupProductDetailTable.row(r).select();
        }
    }
}
//-----Group ProductList---------------------
function GetGroupProductList(id) {
    try {
        debugger;
        var data = { "ID": id };
        var jsonData = {};
        var result = "";
        var message = "";
        var GroupProductVM = new Object();

        jsonData = GetDataFromServer("SalesOrder/GetSaleOrderDetaiByGroupId/", data);
        if (jsonData != '') {
            jsonData = JSON.parse(jsonData);
            result = jsonData.Result;
            message = jsonData.Message;
            GroupProductVM = jsonData.Records;
        }
        if (result == "OK") {

            return GroupProductVM;
        }
        if (result == "ERROR") {
            alert(message);
        }
    }
    catch (e) {
        //this will show the error msg in the browser console(F12) 
        console.log(e.message);
    }
}

function ShowSalesOrderDetailsModal()
{
    debugger;
    $('#groupModelContextLabel').text("Add Sales Order Details");
    var $form = $('#SalesOrderForm');
    if ($form.valid()) {
        $('#ProductID').attr("disabled", false);
        $('#modelContextLabel').text("Add Sales Order Details");

        $('#SalesOrderDetailsModal').modal('show');
        ClearSalesOrderDetailsModalFields();
        $('#SalesOrderDetail_ExpectedDeliveryDateFormatted').val($('#ExpectedDeliveryDateFormatted').val());
    }
    else {
        notyAlert('warning', "Please Fill Required Fields,To Add Items ");
    }
}

//-----GroupItemModal Popup---------------------------
function ShowSalesOrderGroupItemDetailsModal() {
    debugger;
    var $form = $('#SalesOrderForm');
    if ($form.valid()) {
        $('#ProductCategoryCode').attr("disabled", false);
        $('#modelContextLabel').text("Add Sales Order Details");

        $('#SalesOrderGroupItemDetailsModal').modal('show');
        ClearSalesOrderGroupItemDetailsModalFields();
        $('#SalesOrderDetail_GroupItemExpectedDeliveryDateFormatted').val($('#ExpectedDeliveryDateFormatted').val());
    }
    else {
        notyAlert('warning', "Please Fill Required Fields,To Add Group Items ");
    }
}


function ClearSalesOrderDetailsModalFields()
{
    $('#ProductID').val('').select2();
    $('#lblProductName').text('-');
    $('#lblHSN').text('-');
    $('#lblUnit').text('-');
    $('#SalesOrderDetail_Rate').val('');
    $('#SalesOrderDetail_Quantity').val('');
    $('#lblSalesOrderDetail_GrossAmount').text(roundoff(0));
    $('#SalesOrderDetail_TradeDiscountAmount').val(roundoff(0));
    $('#SalesOrderDetail_DiscountPercent').val(0);
    $('#lblSalesOrderDetail_TaxableAmount').text(roundoff(0));
    $('#TaxTypeCode').val('');
    $('#lblSalesOrderDetail_TaxAmount').text(roundoff(0));
    $('#lblSalesOrderDetail_NetAmount').text(roundoff(0));
    $('#SalesOrderDetail_ExpectedDeliveryDateFormatted').val('');
    $('#lblCurrentStock').text('0');
    $('#lblOrderDue').text('0');
    $('#lblNetAvailQty').text('0');
    $('#lbl_WeightInKG').text('0');
}

function BindProductDetails(ID)
{
    debugger;
    if (ID != "")
    {
        var result = GetProduct(ID);
        var quantity = $('#SalesOrderDetail_Quantity').val();
        var orderDue = result.OrderDue;
        $('#SalesOrderDetail_Product_Name').val(result.Name);
        $('#SalesOrderDetail_Product_HSNNo').val(result.HSNNo);
        $('#SalesOrderDetail_UnitCode').val(result.UnitCode);
        $('#SalesOrderDetail_Rate').val(result.Rate);
        $('#lblCurrentStock').text(result.CurrentStock);
        if (result.IsInvoiceInKG) {
            $('#tr_weightinkg').show();
            $('#lbl_WeightInKG').text(result.WeightInKG);
            $('#lblProductName').text(result.Name + ' (Invoice In KG)');
        }
        else {
            $('#tr_weightinkg').hide();
            $('#lbl_WeightInKG').text(''); 
            $('#lblProductName').text(result.Name);
        } 
        $('#lblHSN').text(result.HSNNo);
        $('#lblUnit').text(result.UnitCode);
        if (quantity != "")
            orderDue = parseFloat(orderDue) - parseFloat(quantity);
        $('#lblOrderDue').text(orderDue);
        var AvailQty = parseFloat(result.CurrentStock) - parseFloat(orderDue);
        $('#lblNetAvailQty').text(AvailQty);
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
    debugger;
    var product,rate, qty, discpercent, disc=0, taxTypeCode, taxableAmt=0,taxAmt=0, netAmt=0, GrossAmt=0
    product=$('#ProductID').val();
    rate = $('#SalesOrderDetail_Rate').val();

    var weight = 1;
    if($('#lbl_WeightInKG').text()!="")
    weight = parseFloat($('#lbl_WeightInKG').text()) == 0 ? 1 : parseFloat($('#lbl_WeightInKG').text());
    qty = $('#SalesOrderDetail_Quantity').val();

    if (rate != "" && qty != "" && product!="")
    {
        //--------------------Gross Amount-----------------------//
        GrossAmt = rate * qty *weight;
        $('#lblSalesOrderDetail_GrossAmount').text(roundoff(GrossAmt));

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
        $('#lblSalesOrderDetail_TaxableAmount').text(taxableAmt);

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
        $('#lblSalesOrderDetail_TaxAmount').text(roundoff(taxAmt));
        netAmt = parseFloat(taxableAmt) + parseFloat(taxAmt);
        $('#lblSalesOrderDetail_NetAmount').text(roundoff(netAmt));
    }

    if ($('#SalesOrderDetail_DiscountPercent').val() == "")
        $('#SalesOrderDetail_DiscountPercent').val(roundoff(0));
    if ($('#SalesOrderDetail_TradeDiscountAmount').val() == "")
        $('#SalesOrderDetail_TradeDiscountAmount').val(roundoff(0));
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
    var productId = $('#ProductID').val();
    _groupInsert = 0;
    if (rate != "" && qty != "" && date != "" && productId != "" && qty != 0)
    {
        _SalesOrderDetail = [];
        SalesOrderDetailVM = new Object();
       // SalesOrderDetailVM.ID = EmptyGuid;
        SalesOrderDetailVM.ProductID = $("#ProductID").val();
        SalesOrderDetailVM.Product = new Object();
        SalesOrderDetailVM.Product.Name = $('#lblProductName').text();
        SalesOrderDetailVM.Product.HSNNo = $('#lblHSN').text();
        SalesOrderDetailVM.UnitCode = $('#lblUnit').text();
        SalesOrderDetailVM.Quantity = $('#SalesOrderDetail_Quantity').val();
        SalesOrderDetailVM.Rate = $('#SalesOrderDetail_Rate').val();
        SalesOrderDetailVM.ExpectedDeliveryDateFormatted = $('#SalesOrderDetail_ExpectedDeliveryDateFormatted').val();
        SalesOrderDetailVM.GrossAmount = $('#lblSalesOrderDetail_GrossAmount').text();
        SalesOrderDetailVM.TradeDiscountAmount = $('#SalesOrderDetail_TradeDiscountAmount').val();
        SalesOrderDetailVM.DiscountPercent = $('#SalesOrderDetail_DiscountPercent').val();
        SalesOrderDetailVM.TaxAmount = $('#lblSalesOrderDetail_TaxAmount').text();
        SalesOrderDetailVM.NetAmount = $('#lblSalesOrderDetail_NetAmount').text();
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
                        SalesOrderDetailList[i].GrossAmount = $('#lblSalesOrderDetail_TaxableAmount').text();
                        SalesOrderDetailList[i].TradeDiscountAmount = $('#SalesOrderDetail_TradeDiscountAmount').val();
                        SalesOrderDetailList[i].DiscountPercent = $('#SalesOrderDetail_DiscountPercent').val();
                        SalesOrderDetailList[i].TaxAmount = $('#lblSalesOrderDetail_TaxAmount').text();
                        SalesOrderDetailList[i].NetAmount = $('#lblSalesOrderDetail_NetAmount').text();
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
        Save();
    }
    else
    {
        notyAlert('warning', "Please check the Required Fields");
    }
}

function Save() {
    $("#DetailJSON").val('');
    _SalesOrderDetailList = [];
    if (_groupInsert == 0)
        AddSalesOrderDetailList();
    else
        AddSalesOrderGroupDetailList();
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
        SalesOrderDetailVM.GroupID =EmptyGuid;
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
//------------------------GroupItemAdding Functions-----------------------------------------------------------------------
function ProductList() {
    debugger;
    var code = $('#ProductCategoryCode').val();
    $('#SalesOrderDetail_GroupName').val('');
    $('#SalesOrderDetail_NumOfSet').val('');
    $('#SalesOrderDetail_GroupTaxTypeCode').val('');
    $('#SalesOrderDetail_GroupID').val('');
    $('#SalesOrderDetail_GroupItemDiscountPercent').val(roundoff(0));
    $('#SalesOrderDetail_GroupItemTradeDiscountAmount').val(roundoff(0));
    $('#SalesOrderDetail_GroupGrossAmount').val(roundoff(0));
    productVM = GetProductList(code);
    DataTables.GroupProductDetailTable.clear().rows.add(productVM).draw(true);
}
function GetProductList(code) {
    try {
        debugger;
        var data = { "code": code };
        var jsonData = {};
        var result = "";
        var message = "";
        var supplierVM = new Object();

        jsonData = GetDataFromServer("SalesOrder/GetProductListByCategoryCode/", data);
        if (jsonData != '') {
            jsonData = JSON.parse(jsonData);
            result = jsonData.Result;
            message = jsonData.Message;
            supplierVM = jsonData.Records;
        }
        if (result == "OK") {

            return supplierVM;
        }
        if (result == "ERROR") {
            alert(message);
        }
    }
    catch (e) {
        //this will show the error msg in the browser console(F12) 
        console.log(e.message);
    }
}
//--------Set Quantity----------
function QuantityChanged()
{
    debugger;
    var Quantity = 0;
    if ($('#SalesOrderDetail_NumOfSet').val()!="" && $('#SalesOrderDetail_NumOfSet').val()!=0)
         Quantity = parseFloat($('#SalesOrderDetail_NumOfSet').val());

    var IDs = selectedRowIDs();//identify the selected rows 
    var productDetailsVM = DataTables.GroupProductDetailTable.rows().data();
    for (var r = 0; r < productDetailsVM.length; r++) {
        productDetailsVM[r].Quantity = Quantity;
      //  productDetailsVM[r].Amount = productDetailsVM[r].Quantity * productDetailsVM[r].CostPrice;
    }
    DataTables.GroupProductDetailTable.clear().rows.add(productDetailsVM).draw(false);
    debugger;
    selectCheckbox(IDs); //Selecting the checked rows with their ids taken 
    
}
function selectedRowIDs() {
    var allData = DataTables.GroupProductDetailTable.rows(".selected").data();
    var arrIDs = "";
    for (var r = 0; r < allData.length; r++) {
        if (r == 0)
            arrIDs = allData[r].ID;
        else
            arrIDs = arrIDs + ',' + allData[r].ID;
    }
    return arrIDs;
}
//selected Checkbox
function selectCheckbox(IDs) {
    debugger;
    var productDetailsVM = DataTables.GroupProductDetailTable.rows().data()
    for (var i = 0; i < productDetailsVM.length; i++) {
        if (IDs.includes(productDetailsVM[i].ID)) {
            DataTables.GroupProductDetailTable.rows(i).select();
        }
        else {
            DataTables.GroupProductDetailTable.rows(i).deselect();
        }
    }
   }
//SalesOrderDetailTbl Binding for GroupItems
function AddSalesOrderDetailsOfGrouping() {
    debugger;
    var qty = $('#SalesOrderDetail_NumOfSet').val();
    var group = $('#SalesOrderDetail_GroupName').val();
    var productCategoryCode = $('#ProductCategoryCode').val();
    var ProductGroupDetailList = DataTables.GroupProductDetailTable.rows(".selected").data();
    _groupInsert = 1;
    GroupingDetailVM = new Object();
    if (productCategoryCode != "" && qty != "" && group != "" && qty != 0) {
        if (ProductGroupDetailList != null) {
            if (ProductGroupDetailList.length > 0) {
                Save();
                $('#SalesOrderGroupItemDetailsModal').modal('hide');
            }
            else {
                notyAlert('warning', "Please select Items");
            }
        }
    }
    else {
        notyAlert('warning', "Please check the Required Fields");
    }
}
//Save ProductDetailList
function AddSalesOrderGroupDetailList() {
    debugger;
    var SalesOrderDetailList = DataTables.GroupProductDetailTable.rows(".selected").data();
    for (var r = 0; r < SalesOrderDetailList.length; r++) {
        SalesOrderDetailVM = new Object();
        if (SalesOrderDetailList[r].SalesOrderID != undefined)
            SalesOrderDetailVM.ID = SalesOrderDetailList[r].SalesOrderID;
        SalesOrderDetailVM.ProductID = SalesOrderDetailList[r].ID;
        SalesOrderDetailVM.Quantity = SalesOrderDetailList[r].Quantity
        SalesOrderDetailVM.Rate = SalesOrderDetailList[r].CostPrice
        SalesOrderDetailVM.ExpectedDeliveryDateFormatted = $('#SalesOrderDetail_GroupItemExpectedDeliveryDateFormatted').val();
        
        if (r == 0){
            SalesOrderDetailVM.TaxTypeCode = $('#SalesOrderDetail_GroupTaxTypeCode').val() == "" ? null : $('#SalesOrderDetail_GroupTaxTypeCode').val();
            SalesOrderDetailVM.TradeDiscountAmount = $('#SalesOrderDetail_GroupItemTradeDiscountAmount').val();
            SalesOrderDetailVM.DiscountPercent = $('#SalesOrderDetail_GroupItemDiscountPercent').val();
        }
        else{
            SalesOrderDetailVM.TaxTypeCode = null;
            SalesOrderDetailVM.TradeDiscountAmount = 0;
            SalesOrderDetailVM.DiscountPercent = 0;
        }
        SalesOrderDetailVM.UnitCode = SalesOrderDetailList[r].UnitCode;
        SalesOrderDetailVM.GroupName = $('#SalesOrderDetail_GroupName').val();
        if ($('#SalesOrderDetail_GroupID').val() != "")
            SalesOrderDetailVM.GroupID = $('#SalesOrderDetail_GroupID').val();
        else
        SalesOrderDetailVM.GroupID =EmptyGuid;

        _SalesOrderDetailList.push(SalesOrderDetailVM);
    }
}
//---Function To Clear SalesOrderGroupItemDetailsModal
function ClearSalesOrderGroupItemDetailsModalFields() {
    $('#ProductCategoryCode').val('').select2();
    $('#SalesOrderDetail_GroupName').val('');
    $('#SalesOrderDetail_NumOfSet').val('');
    $('#SalesOrderDetail_GroupItemExpectedDeliveryDateFormatted').val('');
    $('#SalesOrderDetail_GroupTaxTypeCode').val('');
    $('#SalesOrderDetail_GroupID').val('');
    $('#SalesOrderDetail_GroupItemDiscountPercent').val(roundoff(0));
    $('#SalesOrderDetail_GroupItemTradeDiscountAmount').val(roundoff(0));
    $('#SalesOrderDetail_GroupGrossAmount').val(roundoff(0));
    DataTables.GroupProductDetailTable.clear().draw();;
}
//---Function To Calculate GroupItemDiscount
function GroupValueCalculation() {
    debugger;
    var discpercent, disc = 0, GrossAmt = 0, taxableAmt=0;
    //--------------------Gross Amount--------------------//
    var SalesOrderDetailList = DataTables.GroupProductDetailTable.rows(".selected").data();
    for (var r = 0; r < SalesOrderDetailList.length; r++) {
        if (SalesOrderDetailList[r].IsInvoiceInKG)
            GrossAmt = parseFloat(GrossAmt) + (parseFloat(SalesOrderDetailList[r].Quantity) * parseFloat(SalesOrderDetailList[r].WeightInKG) * parseFloat(SalesOrderDetailList[r].CostPrice));
        else
            GrossAmt = parseFloat(GrossAmt) + (parseFloat(SalesOrderDetailList[r].Quantity) * parseFloat(SalesOrderDetailList[r].CostPrice));
    }
    $('#SalesOrderDetail_GroupGrossAmount').val(parseFloat(GrossAmt));
    //--------------------Discount Amount--------------------//
    discpercent = $('#SalesOrderDetail_GroupItemDiscountPercent').val();
    if (discpercent > 100)//if greater than 100% set percentage to 0%
    {
        $('#SalesOrderDetail_GroupItemDiscountPercent').val(0);
        discpercent = 0;
    }
    if (discpercent != "" && discpercent != 0) {
        disc = GrossAmt * (discpercent / 100);
        $('#SalesOrderDetail_GroupItemTradeDiscountAmount').val(roundoff(disc));
    }
    else {
        disc = $('#SalesOrderDetail_GroupItemTradeDiscountAmount').val() == "" ? 0 : $('#SalesOrderDetail_GroupItemTradeDiscountAmount').val();
        if (GrossAmt < disc) {
            $('#SalesOrderDetail_GroupItemTradeDiscountAmount').val(roundoff(0));
            disc = 0;
        }
    }
    if ($('#SalesOrderDetail_GroupItemDiscountPercent').val() == "")
        $('#SalesOrderDetail_GroupItemDiscountPercent').val(roundoff(0));
    if($('#SalesOrderDetail_GroupItemTradeDiscountAmount').val() == "")
        $('#SalesOrderDetail_GroupItemTradeDiscountAmount').val(roundoff(0));
}

function ClearGroupDiscountPercentage() {
    $('#SalesOrderDetail_GroupItemDiscountPercent').val(0);
}


