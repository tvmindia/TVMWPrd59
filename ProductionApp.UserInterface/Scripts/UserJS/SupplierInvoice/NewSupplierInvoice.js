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
// ##5-- 
// ##6-- 
// ##7-- 
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
