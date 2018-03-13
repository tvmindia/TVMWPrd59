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

var result = "";
var message = "";
var jsonData = {};

$(document).ready(function () {
    debugger;
    try {
        $("#ProductID").select2({
            dropdownParent: $("#SalesOrderDetailsModal")
        });
        $("#EmployeeID").select2({
        });
        $("#CustomerID").select2({
        });

        $('#btnUpload').click(function () {
            debugger;
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
        });

        DataTables.SalesOrderDetailTable = $('#tblSalesOrderDetail').DataTable(
      {
          dom: '<"pull-right"f>rt<"bottom"ip><"clear">',
          ordering: false,
          searching: false,
          paging: false,
          data: null,
          autoWidth: false,
          columns: [
          { "data": "ID", "defaultContent": "<i></i>" },
          { "data": "ProductID", "defaultContent": "<i></i>" },
          {
              "data": "Product.Name", render: function (data, type, row) {
                  return data + '</br><b>HSNNo: </b>' + row.Product.HSNNo + '</br><b>Expected Delivery: </b>' + row.ExpectedDeliveryDateFormatted
              }, "defaultContent": "<i></i>", "width": "25%"
          },
          { "data": "TaxTypeCode", "defaultContent": "<i></i>", "width": "8%" },
          { "data": "Quantity", render: function (data, type, row) { return data + ' ' + row.UnitCode }, "defaultContent": "<i></i>", "width": "10%" },
          { "data": "Rate", render: function (data, type, row) { return roundoff(data) }, "defaultContent": "<i></i>", "width": "10%" },
          { "data": "GrossAmount", render: function (data, type, row) { return roundoff(data) }, "defaultContent": "<i></i>", "width": "10%" },
          {
              "data": "DiscountAmount", render: function (data, type, row) {
                  if (row.DiscountPercent!='')
                      return data + '(' + row.DiscountPercent + '%)'
                  else
                      return data
              }, "defaultContent": "<i></i>", "width": "10%"
          },
          { "data": "TaxAmount", render: function (data, type, row) { return data }, "defaultContent": "<i></i>", "width": "10%" },
          { "data": "NetAmount", render: function (data, type, row) { return data }, "defaultContent": "<i></i>", "width": "10%" },
          { "data": null, "orderable": false, "defaultContent": '<a href="#" class="actionLink"  onclick="MaterialEdit(this)" ><i class="glyphicon glyphicon-pencil" aria-hidden="true"></i></a>  |  <a href="#" class="DeleteLink"  onclick="Delete(this)" ><i class="glyphicon glyphicon-trash" aria-hidden="true"></i></a>' },
          ],
          columnDefs: [{ "targets": [0, 1], "visible": false, searchable: false },
              { className: "text-center", "targets": [10], "width": "7%" },
              { className: "text-right", "targets": [5,6,7,8] },
              { className: "text-left", "targets": [2,3,4] }
          ]
      });

        $("#ProductID").change(function () {
            BindProductDetails(this.value);
            ProductValueCalculation();
        });
        $(".Calculation").change(function () {
            debugger;
            ProductValueCalculation();
        });

        $("#TaxTypeCode").change(function () {
            debugger;
            ProductValueCalculation();
        });
        
        debugger;
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

function Save() {
    debugger;
    $("#DetailJSON").val('');
    //_RequistionDetailList = [];
    //AddRequistionDetailList();
    //if (_RequistionDetailList.length > 0) {
    //    var result = JSON.stringify(_RequistionDetailList);
    //    $("#DetailJSON").val(result);
        $('#btnSave').trigger('click');
    //}
    //else {
    //    notyAlert('warning', 'Please Add Requistion Details!');
    //}

}

function ShowSalesOrderDetailsModal()
{
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
    $('#SalesOrderDetail_DiscountAmount').val(roundoff(0));
    $('#SalesOrderDetail_DiscountPercent').val('');
    $('#SalesOrderDetail_TaxableAmount').val(roundoff(0));
    $('#TaxTypeCode').val('');
    $('#SalesOrderDetail_TaxAmount').val(roundoff(0));
    $('#SalesOrderDetail_NetAmount').val(roundoff(0));
    $('#SalesOrderDetail_ExpectedDeliveryDateFormatted').val('');
}

function BindProductDetails(ID)
{
    var result = GetProduct(ID);
    $('#SalesOrderDetail_Product_Name').val(result.Name);
    $('#SalesOrderDetail_Product_HSNNo').val(result.HSNNo);
    $('#SalesOrderDetail_UnitCode').val(result.UnitCode);
    $('#SalesOrderDetail_Rate').val(result.Rate);
}

function GetProduct(ID) {
    try {
        var data = { "ID": ID };
         
        jsonData = GetDataFromServer("Product/GetProduct/", data);
        if (jsonData != '') {
            jsonData = JSON.parse(jsonData);
        }
        if (jsonData.Result == "OK") {
            return jsonData.Records;
        }
        if (jsonData.Result == "ERROR") {
            alert(jsonData.Message);
        }
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}

function ProductValueCalculation()
{
    var rate, qty, discpercent, disc=0, taxTypeCode, taxableAmt=0,taxAmt=0, netAmt=0, GrossAmt=0
   
    rate = $('#SalesOrderDetail_Rate').val();
    qty = $('#SalesOrderDetail_Quantity').val();

    if (rate != "" && qty != "")
    {
        //--------------------Gross Amount-----------------------//
        GrossAmt = rate * qty;
        $('#SalesOrderDetail_GrossAmount').val(roundoff(GrossAmt));

        //--------------------Discount Amount--------------------//
        discpercent = $('#SalesOrderDetail_DiscountPercent').val();
        if (discpercent != "")
        {
            disc = GrossAmt * (discpercent / 100);
            $('#SalesOrderDetail_DiscountAmount').val(roundoff(disc));
        }
        else {
            disc = $('#SalesOrderDetail_DiscountAmount').val();
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
            $('#SalesOrderDetail_TaxAmount').val(roundoff(taxAmt));
        }
        //----------------------Net Amount---------------------//
        netAmt = parseFloat(taxableAmt) + parseFloat(taxAmt);
        $('#SalesOrderDetail_NetAmount').val(roundoff(netAmt));
    }
}

function ClearDiscountPercentage()
{
    $('#SalesOrderDetail_DiscountPercent').val('');
}

function GetTaxTypeByCode(Code) {
    try {
        var data = { "Code": Code };

        var taxTypeVM = new Object();
        jsonData = GetDataFromServer("TaxType/GetTaxtype/", data);
        if (jsonData != '') {
            jsonData = JSON.parse(jsonData);
            result = jsonData.Result;
            message = jsonData.Message;
            taxTypeVM = jsonData.Records;
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

        SalesOrderDetailVM.ProductID = $("#ProductID").val();
        SalesOrderDetailVM.Product = new Object();
        SalesOrderDetailVM.Product.Name = $('#SalesOrderDetail_Product_Name').val();
        SalesOrderDetailVM.Product.HSNNo = $('#SalesOrderDetail_Product_HSNNo').val();
        SalesOrderDetailVM.UnitCode = $('#SalesOrderDetail_UnitCode').val();
        SalesOrderDetailVM.Quantity = $('#SalesOrderDetail_Quantity').val();
        SalesOrderDetailVM.Rate = $('#SalesOrderDetail_Rate').val();
        SalesOrderDetailVM.ExpectedDeliveryDateFormatted = $('#SalesOrderDetail_ExpectedDeliveryDateFormatted').val();
        SalesOrderDetailVM.GrossAmount = $('#SalesOrderDetail_GrossAmount').val();
        SalesOrderDetailVM.DiscountAmount = $('#SalesOrderDetail_DiscountAmount').val();
        SalesOrderDetailVM.DiscountPercent = $('#SalesOrderDetail_DiscountPercent').val();
        SalesOrderDetailVM.TaxAmount = $('#SalesOrderDetail_TaxAmount').val();
        SalesOrderDetailVM.NetAmount = $('#SalesOrderDetail_NetAmount').val();
        SalesOrderDetailVM.TaxTypeCode = $('#TaxTypeCode').val();
        
        _SalesOrderDetail.push(SalesOrderDetailVM);

        if (_SalesOrderDetail != null) {
            //check product existing or not if soo update the new
            var SalesOrderDetailList = DataTables.SalesOrderDetailTable.rows().data();
            if (SalesOrderDetailList.length > 0) {
                var checkPoint = 0;
                for (var i = 0; i < SalesOrderDetailList.length; i++) {
                    if (SalesOrderDetailList[i].ProductID == $("#ProductID").val()) {
                        SalesOrderDetailList[i].Quantity = $('#SalesOrderDetail_Quantity').val();
                        SalesOrderDetailList[i].Rate = $('#SalesOrderDetail_Rate').val();
                        SalesOrderDetailList[i].ExpectedDeliveryDateFormatted = $('#SalesOrderDetail_ExpectedDeliveryDateFormatted').val();
                        SalesOrderDetailList[i].GrossAmount = $('#SalesOrderDetail_GrossAmount').val();
                        SalesOrderDetailList[i].DiscountAmount = $('#SalesOrderDetail_DiscountAmount').val();
                        SalesOrderDetailList[i].DiscountPercent = $('#SalesOrderDetail_DiscountPercent').val();
                        SalesOrderDetailList[i].TaxAmount = $('#SalesOrderDetail_TaxAmount').val();
                        SalesOrderDetailList[i].NetAmount = $('#SalesOrderDetail_NetAmount').val();
                        SalesOrderDetailList[i].TaxTypeCode = $('#TaxTypeCode').val();
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
        $('#SalesOrderDetailsModal').modal('hide');
    }
    else
    {
        notyAlert('warning', "Please check the Required Fields");
    }
}