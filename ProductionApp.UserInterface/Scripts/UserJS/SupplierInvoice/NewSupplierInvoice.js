﻿//*****************************************************************************
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
                  { "data": "Checkbox", "defaultContent": "", "width": "" },
                  {
                      "data": "MaterialDesc", "defaultContent": "<i>-</i>", "width": "",
                       'render': function (data, type, row) {
                            return data;
                       }
                  },
                   {
                       "data": "Qty", "defaultContent": "<i>-</i>", "width": "",
                       'render': function (data, type, row) {
                             return '<input class="form-control text-right" name="Markup" value="' + data + '" type="text" onkeypress = "return isNumber(event)"  onchange="EdittextBoxValue(this,1);"style="width:100%">';
                           //return data;
                       }
                   },
                    {
                        "data": "Rate", "defaultContent": "<i>-</i>", "width": "",
                        'render': function (data, type, row) {
                            return '<input class="form-control text-right" name="Markup" value="' + data + '" type="text" onkeypress = "return isNumber(event)"  onchange="EdittextBoxValue(this,2);"style="width:100%">';
                        }
                    },
                    {
                        "data": "Discount", "defaultContent": "<i>-</i>", "width": "",
                        'render': function (data, type, row) {
                            return '<input class="form-control text-right" name="Markup" value="' + data + '" type="text" onkeypress = "return isNumber(event)"  onchange="EdittextBoxValue(this,3);" style="width:100%">';
                        }
                    },
                    {
                        "data": "TaxTypeCode", "defaultContent": "<i>-</i>", "width": "",
                        'render': function (data, type, row) {
                            if (data != null) {
                                var first = _taxDropdownScript.slice(0, _taxDropdownScript.indexOf('value="' + data + '"'));
                                var second = _taxDropdownScript.slice(_taxDropdownScript.indexOf('value="' + data + '"'), _taxDropdownScript.length);
                                return '<select class="form-control" onchange="EdittextBoxValue(this,4);" >' + first + ' selected="selected" ' + second + '</select>';
                            }
                            else {
                                return '<select class="form-control" onchange="EdittextBoxValue(this,4);" >' + _taxDropdownScript + '</select>';
                            }
                        }
                    }
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
        TaxtypeDropdown();
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
 
//##8-- popup DataTable: Dropdown,TextBoxes,CheckBox Binding-----------------##8
function TaxtypeDropdown() {
    var taxTypeVM = GetTaxtypeDropdown()
    _taxDropdownScript = "<option value=" + '' + ">-Select-</option>";
    for (i = 0; i < taxTypeVM.length; i++) {
        _taxDropdownScript = _taxDropdownScript + '<option value="' + taxTypeVM[i].Code + '">' + taxTypeVM[i].Description + '</option>'
    }
}
function GetTaxtypeDropdown() {
    try {
        var data = {};
        var taxTypeVM = new Object();
        jsonData = GetDataFromServer("CustomerInvoice/GetTaxTypeForSelectList/", data);
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
            alert(message);
        }
    }
    catch (e) {
        //this will show the error msg in the browser console(F12) 
        console.log(e.message);
    }

}
function EdittextBoxValue(thisObj, textBoxCode) {
    debugger;
    var IDs = selectedRowIDs();//identify the selected rows 
    var purchaseOrderDetailVM = _DataTables.PurchaseOrderDetailTable.rows().data();
    var rowtable = _DataTables.PurchaseOrderDetailTable.row($(thisObj).parents('tr')).data();
    for (var i = 0; i < purchaseOrderDetailVM.length; i++) {
        if (purchaseOrderDetailVM[i].MaterialID == rowtable.MaterialID) {
            if (textBoxCode == 1) 
                    purchaseOrderDetailVM[i].Quantity = thisObj.value;  
            if (textBoxCode == 2)
                purchaseOrderDetailVM[i].Rate = thisObj.value;
            if (textBoxCode == 3)
                purchaseOrderDetailVM[i].TradeDiscountAmount = thisObj.value;
            if (textBoxCode == 4)
            {
                var taxTypeVM = GetTaxtypeDropdown();
                purchaseOrderDetailVM[i].TaxTypeCode = thisObj.value;
                for (j = 0; j < taxTypeVM.length; j++) {
                    if (taxTypeVM[j].Code == thisObj.value) {
                        purchaseOrderDetailVM[i].IGSTPerc = taxTypeVM[j].IGSTPercentage;
                        purchaseOrderDetailVM[i].SGSTPerc = taxTypeVM[j].SGSTPercentage;
                        purchaseOrderDetailVM[i].CGSTPerc = taxTypeVM[j].CGSTPercentage;
                    }
                }
            }
        }
    }
    _DataTables.PurchaseOrderDetailTable.clear().rows.add(purchaseOrderDetailVM).draw(false);
    selectCheckbox(IDs); //Selecting the checked rows with their ids taken 
}
function selectedRowIDs() {
    var allData = _DataTables.PurchaseOrderDetailTable.rows(".selected").data();
    var arrIDs = "";
    for (var r = 0; r < allData.length; r++) {
        if (r == 0)
            arrIDs = allData[r].MaterialID;
        else
            arrIDs = arrIDs + ',' + allData[r].MaterialID;
    }
    return arrIDs;
}
function selectCheckbox(IDs) {
    var purchaseOrderDetailVM = _DataTables.PurchaseOrderDetailTable.rows().data()
    for (var i = 0; i < purchaseOrderDetailVM.length; i++) {
        if (IDs.includes(purchaseOrderDetailVM[i].MaterialID)) {
            _DataTables.PurchaseOrderDetailTable.rows(i).select();
        }
        else {
            _DataTables.PurchaseOrderDetailTable.rows(i).deselect();
        }
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