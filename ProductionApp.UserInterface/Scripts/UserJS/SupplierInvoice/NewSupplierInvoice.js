//*****************************************************************************
//*****************************************************************************
//Author: Gibin Jacob
//CreatedDate: 20-Mar-2018 
//LastModified:  
//FileName: NewSupplierInvoice.js
//Description: Client side coding for New/Edit Supplier Invoice
//******************************************************************************
// ##1--Global Declaration
// ##2--Document Ready function
// ##3--On Change Supplier : Bind Supplier Details
// ##4--Bind Payment due date, based on Payment date
// ##5--From Purchase Order Changed 
// ##6--Show Supplier Invoice Details Modal
// ##7--Show Load PO Detail Modal
// ##8-- 
// ##9-- 
// ##10-- 
// ##11--Save  Supplier Invoice 
// ##12--Save Success Supplier Invoice
// ##13--Bind Supplier Invoice By ID
// ##14--Reset Button Click
// ##15-- 
// ##16--DELETE Supplier Invoice 
// ##17--DELETE Supplier Invoice Details 
// 
//******************************************************************************

//##1--Global Declaration---------------------------------------------##1 
var _DataTables = {};
var EmptyGuid = "00000000-0000-0000-0000-000000000000";
var _SlNo = 1;
var _result = "";
var _message = "";
var _jsonData = {};
var _taxDropdownScript = '';
var _SupplierInvoiceDetail = []; 

//##2--Document Ready function-----------------------------------------##2 
$(document).ready(function () {
    debugger;
    try {
        //------select2 fields-------// 
        $("#SupplierID").select2({});
        $("#AccountCode").select2({});
        $("#PurchaseOrderID").select2({});

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
            FileObject.ParentType = "SupplierInvoice";
            FileObject.Controller = "FileUpload";
            UploadFile(FileObject);
        });

        _DataTables.SupplierInvoiceDetailTable = $('#tblSupplierInvoiceDetail').DataTable({
            dom: '<"pull-right"f>rt<"bottom"ip><"clear">',
            ordering: false,
            searching: false,
            paging: false,
            data: null,
            "bInfo": false,
            autoWidth: false,
            columns: [

              { "data": "ID", "defaultContent": "<i>-</i>", },
              {
                  "data": "", render: function (data, type, row) {
                      return _SlNo++
                  }, "width": "5%"
              },
              {
                  "data": "ProductName", "defaultContent": "<i>-</i>", "width": "25%",
                  'render': function (data, type, row) {
                      if (row.IsInvoiceInKG)
                          return data + '</br>(<b>Invoice in Kg </b>)'
                      else
                          return data
                  }
              },
              { "data": "Quantity", "defaultContent": "<i>-</i>", "width": "9%" },
              { "data": "Weight", "defaultContent": "<i>-</i>", "width": "9%" },
              { "data": "Rate", "defaultContent": "<i>-</i>", "width": "9%" },
              { "data": "TradeDiscountAmount", "defaultContent": "<i>-</i>", "width": "9%" },
              { "data": "TaxableAmount", "defaultContent": "<i>-</i>", "width": "9%" },
              { "data": "TaxTypeDescription", "defaultContent": "<i>-</i>", "width": "9%" },
              { "data": "Total", "defaultContent": "<i>-</i>", "width": "9%" },
              { "data": null, "orderable": false, "defaultContent": '<a href="#" class="actionLink"  onclick="ItemDetailsEdit(this)" ><i class="glyphicon glyphicon-edit" aria-hidden="true"></i></a>     <a href="#" class="DeleteLink"  onclick="DeleteDetail(this)" ><i class="glyphicon glyphicon-trash" aria-hidden="true"></i></a>', "width": "7%" }

            ],
            columnDefs: [{ "targets": [0], "visible": false, searchable: false },
                { className: "text-center", "targets": [10] },
                { className: "text-right", "targets": [7, 8, 9] },
                { className: "text-left", "targets": [4, 6] }
            ]
        });
      
        _DataTables.PurchaseOrderDetailTable = $('#tblPurchaseOrderDetail').DataTable({
                dom: '<"pull-right"f>rt<"bottom"ip><"clear">',
                order: [],
                searching: true,
                paging: true,
                data: null,
                pageLength: 5,
                language: {
                    search: "_INPUT_",
                    searchPlaceholder: "Search"
                },
                columns: [
                  { "data": "Checkbox", "defaultContent": "" },
                  { "data": "MaterialID", "defaultContent": "<i>-</i>" },
                  { "data": "MaterialCode", "defaultContent": "<i>-</i>" },
                  { "data": "MaterialDesc", "defaultContent": "<i>-</i>" },
                  { "data": "Qty", "defaultContent": "<i>-</i>" },
                  { "data": "UnitCode", "defaultContent": "<i>-</i>" },
                ],
                columnDefs: [{ orderable: false, className: 'select-checkbox', "targets": 0 },
                    { "targets": [], "visible": false, "searchable": false },
                    { className: "text-right", "targets": [4] },
                    { className: "text-left", "targets": [2,3, 5] },
                    { className: "text-center", "targets": [0] },
                    { "targets": [1,3,2,4,5], "bSortable": false }
                ],
                select: { style: 'multi', selector: 'td:first-child' },
                destroy: true
            });
      
        //------------------------------------------------------------------------------------------------//
  
        $("#SupplierID").change(function () {
            BindSupplierDetails(this.value);
        });
        $("#PaymentTermCode").change(function () {
            GetDueDate(this.value);
        });

        if ($('#IsUpdate').val() == 'True') {
            BindSupplierInvoiceByID()
        }
        else {
            $('#lblSupplierInvoiceNo').text('Supplier Invoice# : New');
        }
        IsFromPurchaseOrderChanged();
    }
    catch (e) {
        console.log(e.message);
    }
});


//##3--On Change Supplier : Bind Supplier Details----------------------##3
function BindSupplierDetails(SupplierId) {
    if (SupplierId != "") {
        var SupplierVM = GetSupplierDetails(SupplierId)
        $('#BillingAddress').val(SupplierVM.BillingAddress)
        $('#ShippingAddress').val(SupplierVM.ShippingAddress)
        $('#PaymentTermCode').val(SupplierVM.PaymentTermCode)
        GetDueDate($('#PaymentTermCode').val());
    }
    else {
        $('#BillingAddress').val('');
    }
}
function GetSupplierDetails(SupplierId) {
    try {
        var data = { "supplierId": SupplierId };
        _jsonData = GetDataFromServer("SupplierInvoice/GetSupplierDetails/", data);
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

//##4--Bind Payment due date, based on Payment date-------------##4
function GetDueDate(Code) {
    try {
        debugger;
        var PaymentTermViewModel = GetPaymentTermDetails(Code);
        $('#PaymentDueDateFormatted').val(PaymentTermViewModel);
    }
    catch (e) {

    }
}
function GetPaymentTermDetails(Code) {
    debugger;
    try {
        var data = { "Code": Code, "InvoiceDate": $('#InvoiceDateFormatted').val() };
        var ds = {};
        ds = GetDataFromServer("SupplierInvoice/GetDueDate/", data);
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

//##5--From Purchase Order Changed -----------------------------##5
function IsFromPurchaseOrderChanged() {
    debugger;
    if ($('#IsFromPurchaseOrder').val() == "True") {
        $('#divPONo').hide();
        $('#divPOID').show();
    } else {
        $('#divPONo').show();
        $('#PurchaseOrderID').val('').select2();
        $('#divPOID').hide();
    }
}

//##6--Show Supplier Invoice Detail Modal ---------------------------##6
function ShowSupplierInvoiceDetailModal()
{
    debugger;
    $('#SupplierInvoiceDetailModal').modal('show');
}




//##7--Show Load PO Detail Modal ---------------------------##7
function LoadPODetailModal() {
    debugger;
    if ($('#PurchaseOrderID').val() !== "") {
        $('#PurchaseOrderDetailModal').modal('show');
        var id = $('#PurchaseOrderID').val();
        BindPurchaseOrderDetailTable(id);
    } else {
        notyAlert('warning', 'Purchase Order Not selcted!');
    }
} 
function BindPurchaseOrderDetailTable(id) {
    debugger;
    _DataTables.PurchaseOrderDetailTable.clear().rows.add(GetPurchaseOrderItem(id)).select().draw(false);
}
function GetPurchaseOrderItem(id) {
    try {
        debugger;
        var data = { "id": id };
        var purchaseOrderDetailList = []; 
        _jsonData = GetDataFromServer("SupplierInvoice/GetAllPurchaseOrderItem/", data);
        if (_jsonData != '') {
            _jsonData = JSON.parse(_jsonData);
            _result = _jsonData.Result;
            _message = _jsonData.Message;
            purchaseOrderDetailList = _jsonData.Records;
        }
        if (_result == "OK") {
            return purchaseOrderDetailList;
        }
        if (_result == "ERROR") {
            notyAlert('error', _message);
        }
    }
    catch (e) {
        console.log(e.message);
    }
} 
 








//##11--Save  Supplier Invoice----------------------------##11
function Save() {
    debugger;
    _SlNo = 1;
    $('#btnSave').trigger('click');
}

//##12--Save Success Supplier Invoice----------------------------##12
function SaveSuccessSupplierInvoice(data, status) {
    _jsonData = JSON.parse(data)
    switch (_jsonData.Result) {
        case "OK":
            $('#IsUpdate').val('True');
            $('#ID').val(_jsonData.Records.ID)
            _SupplierInvoiceDetail = [];
            BindSupplierInvoiceByID();
            notyAlert("success", _jsonData.Records.Message)
            break;
        case "ERROR":
            notyAlert("danger", _jsonData.Message)
            break;
        default:
            notyAlert("danger", _jsonData.Message)
            break;
    }
}
//##13--Bind Supplier Invoice By ID----------------------------##13
function BindSupplierInvoiceByID() {
    debugger;
    ChangeButtonPatchView('SupplierInvoice', 'divbuttonPatchAddSupplierInvoice', 'Edit');
    var ID = $('#ID').val();
    _SlNo = 1;
    var SupplierInvoiceVM = GetSupplierInvoiceByID(ID);
    $('#lblSupplierInvoiceNo').text('Supplier Invoice# :' + SupplierInvoiceVM.InvoiceNo);
    $('#InvoiceNo').val(SupplierInvoiceVM.InvoiceNo);
    $('#InvoiceDateFormatted').val(SupplierInvoiceVM.InvoiceDateFormatted);
    $('#PaymentTermCode').val(SupplierInvoiceVM.PaymentTermCode);
    $('#PaymentDueDateFormatted').val(SupplierInvoiceVM.PaymentDueDateFormatted);
    $('#SupplierID').val(SupplierInvoiceVM.SupplierID).select2();
    $('#GeneralNotes').val(SupplierInvoiceVM.GeneralNotes);
    $('#BillingAddress').val(SupplierInvoiceVM.BillingAddress);
    $('#ShippingAddress').val(SupplierInvoiceVM.ShippingAddress);
    $('#Discount').val(roundoff(SupplierInvoiceVM.Discount));
    $('#lblTotalTaxableAmount').text(roundoff(SupplierInvoiceVM.TotalTaxableAmount));
    $('#lblTotalTaxAmount').text(roundoff(SupplierInvoiceVM.TotalTaxAmount));
    $('#lblInvoiceAmount').text(roundoff(SupplierInvoiceVM.InvoiceAmount));
    $('#lblStatusInvoiceAmount').text(roundoff(SupplierInvoiceVM.InvoiceAmount));

    //detail Table values binding with header id
   // BindSupplierInvoiceDetailTable(ID);
    PaintImages(ID);//bind attachments written in custom js
}
function GetSupplierInvoiceByID(ID) {
    try {
        var data = { "ID": ID };
        _jsonData = GetDataFromServer("SupplierInvoice/GetSupplierInvoice/", data);
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