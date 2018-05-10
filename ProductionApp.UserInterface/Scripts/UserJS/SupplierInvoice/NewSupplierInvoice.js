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
// ##3--On Change Supplier : Bind Supplier Details & PurchaseOrder Dropdown By Supplier
// ##4--Bind Payment due date, based on Payment date
// ##5--From Purchase Order Changed 
// ##6--Show Supplier Invoice Details Modal
// ##7-- Add button click :Supplier Invoice Detail Modal 
// ##8-- Show Load PO Detail Modal,Add button click 
// ##9-- popup DataTable: Dropdown,TextBoxes,CheckBox Binding
// ##10--Load Purchase Order Dropdown By Supplier
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
var _dataTables = {};
var _emptyGuid = "00000000-0000-0000-0000-000000000000";
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
        $("#divRawMaterialDropdown").load('/Material/MaterialDropdown')

        $('#btnUpload').click(function () {
            debugger;
            //Pass the controller name
            var FileObject = new Object;
            if ($('#hdnFileDupID').val() != _emptyGuid) {
                FileObject.ParentID = (($('#ID').val()) != _emptyGuid ? ($('#ID').val()) : $('#hdnFileDupID').val());
            }
            else {
                FileObject.ParentID = ($('#ID').val() == _emptyGuid) ? "" : $('#ID').val();
            }
            FileObject.ParentType = "SupplierInvoice";
            FileObject.Controller = "FileUpload";
            UploadFile(FileObject);
        });

        _dataTables.SupplierInvoiceDetailTable = $('#tblSupplierInvoiceDetail').DataTable({
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
                  "data": "MaterialDesc", "defaultContent": "<i>-</i>", "width": "25%",
                  'render': function (data, type, row) {
                      return data + '</br><b>Code :</b>' + row.MaterialCode + '</br><b>Type :</b>' + row.MaterialTypeDesc
                  }
              },
              { "data": "Quantity", "defaultContent": "<i>-</i>", "width": "9%" },
              { "data": "Rate", "defaultContent": "<i>-</i>", "width": "9%" },
              { "data": "TradeDiscountAmount", "defaultContent": "<i>-</i>", "width": "9%" },
              { "data": "TaxableAmount", "defaultContent": "<i>-</i>", "width": "9%" },
              { "data": "TaxTypeDescription", "defaultContent": "<i>-</i>", "width": "9%" },
              { "data": "Total", "defaultContent": "<i>-</i>", "width": "9%" },
              { "data": null, "orderable": false, "defaultContent": '<a href="#" class="actionLink"  onclick="ItemDetailsEdit(this)" ><i class="glyphicon glyphicon-edit" aria-hidden="true"></i></a>     <a href="#" class="DeleteLink"  onclick="DeleteDetail(this)" ><i class="glyphicon glyphicon-trash" aria-hidden="true"></i></a>', "width": "7%" }

            ],
            columnDefs: [{ "targets": [], "visible": false, searchable: false },
                { className: "text-center", "targets": [8] },
                { className: "text-right", "targets": [2,3,4,5,7] },
                { className: "text-left", "targets": [1,6] }
            ]
        });
      
        _dataTables.PurchaseOrderDetailTable = $('#tblPurchaseOrderDetail').DataTable({
                dom: '<"pull-right"f>rt<"bottom"ip><"clear">', 
                ordering: false,
                searching: true,
                paging: true,
                data: null,
                pageLength: 5,
                autoWidth: false,
                language: {
                    search: "_INPUT_",
                    searchPlaceholder: "Search"
                },
                columns: [
                  { "data": "Checkbox", "defaultContent": "", "width": "5%" },
                  {
                      "data": "MaterialDesc", "defaultContent": "<i>-</i>", "width": "20%",
                       'render': function (data, type, row) {
                           return data + '</br><b>Code :</b>' + row.MaterialCode + '</br><b>Type :</b>' + row.MaterialTypeDesc
                       }
                  },
                  {
                      "data": "POQty", "defaultContent": "<i>-</i>", "width": "10%",
                      'render': function (data, type, row) {
                          // return '<input class="form-control text-right" style="width: 10%;" name="Markup" value="' + data + '" type="text" onkeypress = "return isNumber(event)"  onchange="EdittextBoxValue(this,1);">';
                          return data;
                      }
                  },
                   {
                       "data": "PrevInvQty", "defaultContent": "<i>-</i>", "width": "10%",
                       'render': function (data, type, row) {
                           // return '<input class="form-control text-right" style="width: 10%;" name="Markup" value="' + data + '" type="text" onkeypress = "return isNumber(event)"  onchange="EdittextBoxValue(this,1);">';
                          return data;
                       }
                   },
                   {
                       "data": "Qty", "defaultContent": "<i>-</i>", "width": "15%",
                       'render': function (data, type, row) { 
                           return '<input class="form-control text-right" style="width: 100%;"  name="Markup" value="' + data + '" type="text" onkeypress = "return isNumber(event)"  onchange="EdittextBoxValue(this,1);">';
                          // return data;
                       }
                   },
                    {
                        "data": "Rate", "defaultContent": "<i>-</i>", "width": "15%",
                        'render': function (data, type, row) {
                            return '<input class="form-control text-right" style="width: 100%;" name="Markup" value="' + data + '" type="text" onkeypress = "return isNumber(event)"  onchange="EdittextBoxValue(this,2);">';
                        }
                    },
                    {
                        "data": "Discount", "defaultContent": "<i>-</i>", "width": "15%",
                        'render': function (data, type, row) {
                            return '<input class="form-control text-right" style="width: 100%;" name="Markup" value="' + data + '" type="text" onkeypress = "return isNumber(event)"  onchange="EdittextBoxValue(this,3);">';
                        }
                    },
                    {
                        "data": "TaxTypeCode", "defaultContent": "<i>-</i>", "width": "10%",
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
                    { className: "text-right", "targets": [] },
                    { className: "text-left", "targets": [] },
                    { className: "text-center", "targets": [0] }, 
                ],
                select: { style: 'multi', selector: 'td:first-child' },
                destroy: true
            });
      
        //------------------------------------------------------------------------------------------------//
  
        $("#SupplierID").change(function () {
            BindSupplierDetails(this.value);
            LoadPurchaseOrderDropdownBySupplier();
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

        $(".Calculation").change(function () {
            ValueCalculation();
        });
        $("#TaxTypeCode").change(function () {
            ValueCalculation();
        });
    }
    catch (e) {
        console.log(e.message);
    }
});


//##3--On Change Supplier : Bind Supplier Details & PurchaseOrder Dropdown By Supplier-----##3
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
function LoadPurchaseOrderDropdownBySupplier() {
    try {
        debugger;
        if($('#SupplierID').val()!="")
        $("#divPOID").load('/PurchaseOrder/PurchaseOrderDropdown?SupplierID='+$('#SupplierID').val())
        else
        {
            $("#divPOID").empty();
            $("#divPOID").append('<input class="form-control HeaderBox text-box single-line" disabled="disabled" id="PurchaseOrderHDN" name="PurchaseOrderNo" type="text" value="">');
        }
    }
    catch (ex) {
        console.log(ex.message);
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
        $('#btnLoadPODetailModal').attr('onclick', 'LoadPODetailModal()');
    } else {
        $('#divPONo').show();
        $('#PurchaseOrderID').val('').select2();
        $('#divPOID').hide();
        $('#btnLoadPODetailModal').removeAttr('onclick');
    }
}

//##6--Show Supplier Invoice Detail Modal ---------------------------##6
function ShowSupplierInvoiceDetailModal()
{
    if ($('#SupplierInvoiceForm').valid()) {
        $("#supplierInvoiceDetailModalLabel").text('Add Supplier Invoice Detail');
        $("#MaterialID").prop("disabled", false);
        $("#MaterialID").val('').select2();
        $('#SupplierInvoiceDetail_ID').val('');
        $('#SupplierInvoiceDetail_UnitCode').val('');
        $('#SupplierInvoiceDetail_Quantity').val('');
        $('#SupplierInvoiceDetail_Rate').val('');
        $('#SupplierInvoiceDetail_TradeDiscountAmount').val(roundoff(0));
        $('#SupplierInvoiceDetail_TradeDiscountPerc').val('0');
        $('#TaxTypeCode').val('');
        $('#SupplierInvoiceDetail_MaterialCode').val('');
        $('#SupplierInvoiceDetail_MaterialTypeDesc').val('');
        $('#SupplierInvoiceDetail_UnitCode').val('');
        $('#SupplierInvoiceDetail_GrossAmount').val(roundoff(0));
        $('#SupplierInvoiceDetail_TaxableAmount').val(roundoff(0));
        $('#SupplierInvoiceDetail_NetAmount').val(roundoff(0));
        $('#SupplierInvoiceDetail_TaxAmount').val(roundoff(0));
        $('#SupplierInvoiceDetailModal').modal('show');
    }
} 
function BindRawMaterialDetails(id) {
    try {
        debugger;
        var result = GetMaterial(id);
        _SlNo = 1;
        $('#SupplierInvoiceDetail_MaterialCode').val(result.MaterialCode);
        $('#SupplierInvoiceDetail_MaterialTypeDesc').val(result.MaterialType.Description);
        $('#SupplierInvoiceDetail_UnitCode').val(result.UnitCode);
        $('#SupplierInvoiceDetail_Rate').val(result.Rate);
        ValueCalculation();
    }
    catch (e) {
        console.log(e.message);
    }
}
function GetMaterial(ID) {
    try {
        debugger;
        var data = { "id": ID };
        var jsonData = {};
        var result = "";
        var message = "";
        var materialViewModel = new Object();
        jsonData = GetDataFromServer("IssueToProduction/GetMaterial/", data);
        if (jsonData != '') {
            jsonData = JSON.parse(jsonData);
            result = jsonData.Result;
            materialViewModel = jsonData.Records;
            message = jsonData.Message;
        }
        if (result == "OK") {
            return materialViewModel;
        }
        if (result == "ERROR") {
            alert(message);
        }
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}
function ValueCalculation() {
    var material, rate, qty, discpercent, disc = 0, taxTypeCode, taxableAmt = 0, taxAmt = 0, netAmt = 0, GrossAmt = 0
    material = $('#MaterialID').val();
    rate = $('#SupplierInvoiceDetail_Rate').val();
    qty = $('#SupplierInvoiceDetail_Quantity').val();

    if (rate != "" && qty != "" && material != "") {
        //--------------------Gross Amount-----------------------//
        GrossAmt = rate * qty;
        $('#SupplierInvoiceDetail_GrossAmount').val(roundoff(GrossAmt));

        //--------------------Discount Amount--------------------//
        discpercent = $('#SupplierInvoiceDetail_TradeDiscountPerc').val();
        if (discpercent > 100)//if greater than 100% set percentage to 0%
        {
            $('#SupplierInvoiceDetail_TradeDiscountPerc').val(0);
            discpercent = 0;
        }
        if (discpercent != "" && discpercent != 0) {
            disc = GrossAmt * (discpercent / 100);
            $('#SupplierInvoiceDetail_TradeDiscountAmount').val(roundoff(disc));
        }
        else {
            disc = $('#SupplierInvoiceDetail_TradeDiscountAmount').val() == "" ? 0 : $('#SupplierInvoiceDetail_TradeDiscountAmount').val();
            if (GrossAmt < disc) {
                $('#SupplierInvoiceDetail_TradeDiscountAmount').val(roundoff(0));
                disc = 0;
            }
        }
        //--------------------Taxable Amount---------------------//
        taxableAmt = roundoff(parseFloat(GrossAmt) - parseFloat(disc));
        $('#SupplierInvoiceDetail_TaxableAmount').val(taxableAmt);

        //--------------------Tax Amount------------------------//
        taxTypeCode = $('#TaxTypeCode').val();
        if (taxTypeCode != "") {
            var taxTypeVM = GetTaxTypeByCode(taxTypeCode);
            var CGSTAmt = parseFloat(taxableAmt) * parseFloat(parseFloat(taxTypeVM.CGSTPercentage) / 100);
            var SGSTAmt = parseFloat(taxableAmt) * parseFloat(parseFloat(taxTypeVM.SGSTPercentage) / 100);
            var IGSTAmt = parseFloat(taxableAmt) * parseFloat(parseFloat(taxTypeVM.IGSTPercentage) / 100);
            taxAmt = CGSTAmt + SGSTAmt + IGSTAmt;
            $('#SupplierInvoiceDetail_TaxAmount').val(roundoff(taxAmt));
        }
        //----------------------Net Amount---------------------//
        netAmt = parseFloat(taxableAmt) + parseFloat(taxAmt);
        $('#SupplierInvoiceDetail_NetAmount').val(roundoff(netAmt));
    }
}
function ClearDiscountPercentage()
{
    $('#SupplierInvoiceDetail_TradeDiscountPerc').val(0);
    ValueCalculation();
}
function GetTaxTypeByCode(Code) {
    try {
        var data = {"Code": Code};
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
  

//##7--Add button click :Supplier Invoice Detail Modal ---------------------------##7
function AddSupplierInvoiceDetails() {
    debugger;
    var rate = $('#SupplierInvoiceDetail_Rate').val();
    var qty = $('#SupplierInvoiceDetail_Quantity').val();
    var productId = $('#MaterialID').val();

    if (rate != "" && qty != "" && productId != "") {
        _SupplierInvoiceDetail = [];
        SupplierInvoiceDetailVM = new Object();
        SupplierInvoiceDetailVM.ID = $('#SupplierInvoiceDetail_ID').val() == "" ? _emptyGuid : $('#SupplierInvoiceDetail_ID').val();
        SupplierInvoiceDetailVM.MaterialID = $("#MaterialID").val();
        SupplierInvoiceDetailVM.UnitCode = $('#SupplierInvoiceDetail_UnitCode').val();
        SupplierInvoiceDetailVM.Quantity = $('#SupplierInvoiceDetail_Quantity').val();
        SupplierInvoiceDetailVM.Rate = $('#SupplierInvoiceDetail_Rate').val();
        SupplierInvoiceDetailVM.TradeDiscountAmount = $('#SupplierInvoiceDetail_TradeDiscountAmount').val();
        SupplierInvoiceDetailVM.TradeDiscountPercent = $('#SupplierInvoiceDetail_TradeDiscountPerc').val();
        SupplierInvoiceDetailVM.TaxTypeCode = $('#TaxTypeCode').val();
        var taxTypeVM = GetTaxTypeByCode($('#TaxTypeCode').val());
        SupplierInvoiceDetailVM.IGSTPerc =taxTypeVM.IGSTPercentage
        SupplierInvoiceDetailVM.SGSTPerc = taxTypeVM.SGSTPercentage
        SupplierInvoiceDetailVM.CGSTPerc = taxTypeVM.CGSTPercentage
        _SupplierInvoiceDetail.push(SupplierInvoiceDetailVM); 
        if (_SupplierInvoiceDetail.length > 0)
        {
            var result = JSON.stringify(_SupplierInvoiceDetail);
            $("#DetailJSON").val(result);
            Save();
        }
        $('#SupplierInvoiceDetailModal').modal('hide');
    }
    else {
        notyAlert('warning', "Please check the Required Fields");
    }
}



//##8--Show Load PO Detail Modal ---------------------------##8
function LoadPODetailModal() {
    debugger;
    if ($('#SupplierInvoiceForm').valid()) {
        var POID = $('#PurchaseOrderID').val();
        if (POID !== "" && POID != undefined) {
            TaxtypeDropdown();
            $('#PurchaseOrderDetailModal').modal('show');
            BindPurchaseOrderDetailTable();
        } else {
            notyAlert('warning', 'Purchase Order Not selcted!');
        }
    }
    
} 
function BindPurchaseOrderDetailTable() {
    debugger;
    var purchaseOrderDetailList = GetPurchaseOrderItem();
    var supplierInvoiceDetailList = _dataTables.SupplierInvoiceDetailTable.rows().data();
    //To exclude the materials already in Details table
    for (j = 0; j < supplierInvoiceDetailList.length; j++) {//To remove the existing items in Details from PO items
        for (i = 0; i < purchaseOrderDetailList.length; i++) {
            if (purchaseOrderDetailList[i].MaterialID === supplierInvoiceDetailList[j].MaterialID) {
                purchaseOrderDetailList.splice(i, 1);//Removes the ith element
            }
        }
    }
    _dataTables.PurchaseOrderDetailTable.clear().rows.add(purchaseOrderDetailList).select().draw(false);
}
function GetPurchaseOrderItem() {
    try {
        debugger;
        var id = $('#PurchaseOrderID').val();
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

 
//##9-- popup DataTable: Dropdown,TextBoxes,CheckBox Binding-----------------##9
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
    var purchaseOrderDetailVM = _dataTables.PurchaseOrderDetailTable.rows().data();
    var rowtable = _dataTables.PurchaseOrderDetailTable.row($(thisObj).parents('tr')).data();
    for (var i = 0; i < purchaseOrderDetailVM.length; i++) {
        if (purchaseOrderDetailVM[i].MaterialID == rowtable.MaterialID) {
            if (textBoxCode == 1) 
                    purchaseOrderDetailVM[i].Qty = thisObj.value;  
            if (textBoxCode == 2)
                purchaseOrderDetailVM[i].Rate = thisObj.value;
            if (textBoxCode == 3)
                purchaseOrderDetailVM[i].Discount = thisObj.value;
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
    _dataTables.PurchaseOrderDetailTable.clear().rows.add(purchaseOrderDetailVM).draw(false);
    selectCheckbox(IDs); //Selecting the checked rows with their ids taken 
}
function selectedRowIDs() {
    var allData = _dataTables.PurchaseOrderDetailTable.rows(".selected").data();
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
    var purchaseOrderDetailVM = _dataTables.PurchaseOrderDetailTable.rows().data()
    for (var i = 0; i < purchaseOrderDetailVM.length; i++) {
        if (IDs.includes(purchaseOrderDetailVM[i].MaterialID)) {
            _dataTables.PurchaseOrderDetailTable.rows(i).select();
        }
        else {
            _dataTables.PurchaseOrderDetailTable.rows(i).deselect();
        }
    }
}


//##10--Button Click: Add PO Items 
function AddPOItems() {
    debugger;
    var purchaseOrderItemList = _dataTables.PurchaseOrderDetailTable.rows(".selected").data();
    _SupplierInvoiceDetail =[];
    for(i = 0; i < purchaseOrderItemList.length; i++)
    {
        SupplierInvoiceDetailVM = new Object();
        SupplierInvoiceDetailVM.MaterialID = purchaseOrderItemList[i].MaterialID;
        SupplierInvoiceDetailVM.Quantity = purchaseOrderItemList[i].Qty;
        SupplierInvoiceDetailVM.Rate = purchaseOrderItemList[i].Rate;
        SupplierInvoiceDetailVM.TradeDiscountAmount = purchaseOrderItemList[i].Discount;
        SupplierInvoiceDetailVM.TaxTypeCode = purchaseOrderItemList[i].TaxTypeCode;
        SupplierInvoiceDetailVM.UnitCode = purchaseOrderItemList[i].UnitCode
        SupplierInvoiceDetailVM.IGSTPerc = purchaseOrderItemList[i].IGSTPerc
        SupplierInvoiceDetailVM.SGSTPerc = purchaseOrderItemList[i].SGSTPerc
        SupplierInvoiceDetailVM.CGSTPerc = purchaseOrderItemList[i].CGSTPerc
        _SupplierInvoiceDetail.push(SupplierInvoiceDetailVM);
    }
    if (_SupplierInvoiceDetail.length > 0) {
        var result = JSON.stringify(_SupplierInvoiceDetail);
        $("#DetailJSON").val(result);
        Save();
         $('#PurchaseOrderDetailModal').modal('hide');
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
            notyAlert("success", _jsonData.Records.Message)
            $('#IsUpdate').val('True');
            $('#ID').val(_jsonData.Records.ID)
            _SupplierInvoiceDetail = [];
            $("#DetailJSON").val('');
            BindSupplierInvoiceByID();
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
    $('#InvoiceAmount').val(SupplierInvoiceVM.InvoiceAmount);
    $('#lblInvoiceAmount').text(roundoff(SupplierInvoiceVM.InvoiceAmount-SupplierInvoiceVM.Discount));
    $('#lblStatusInvoiceAmount').text(roundoff(SupplierInvoiceVM.InvoiceAmount-SupplierInvoiceVM.Discount));
    $('#AccountCode').val(SupplierInvoiceVM.AccountCode).select2();
    debugger;
    if (SupplierInvoiceVM.PurchaseOrderID!=_emptyGuid)
    {
        LoadPurchaseOrderDropdownBySupplier();
        $('#hdnPurchaseOrderID').val(SupplierInvoiceVM.PurchaseOrderID);
        $('#IsFromPurchaseOrder').val('True');
        $('#PurchaseOrderID').prop("disabled", true);
    }
    else
    {
        $('#PurchaseOrderNo').val(SupplierInvoiceVM.PurchaseOrderNo);
        $('#IsFromPurchaseOrder').val('False');
    }
    IsFromPurchaseOrderChanged();
    $('#IsFromPurchaseOrder').attr('disabled', 'true');
    $('#SupplierID').attr('disabled', 'true');
    $('#PurchaseOrderID').attr('disabled', 'true');


    //detail Table values binding with header id
    BindSupplierInvoiceDetailTable(ID);
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
function BindSupplierInvoiceDetailTable(id)
{
    _dataTables.SupplierInvoiceDetailTable.clear().rows.add(GetAllSupplierInvoiceDetail(id)).draw(false);
}
function GetAllSupplierInvoiceDetail(id) {
    try {
        debugger;
        var data = { "id": id };
        _jsonData = GetDataFromServer("SupplierInvoice/GetAllSupplierInvoiceDetail/", data);
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



//##14--Reset Button Click-----------------------------------------------------##14
function Reset() {
    BindSupplierInvoiceByID();
}

////##15--Edit Popup Modal Update Supplier Invoice Detail----------------------------##15
function ItemDetailsEdit(thisObj) {
    debugger;
    $("#supplierInvoiceDetailModalLabel").text('Edit Supplier Invoice Detail');
    var rowData = _dataTables.SupplierInvoiceDetailTable.row($(thisObj).parents('tr')).data(); 
    var supplierInvoiceDetailVM= GetSupplierInvoiceDetail(rowData.ID)
    $('#SupplierInvoiceDetail_ID').val(rowData.ID);
    $("#MaterialID").prop("disabled", true);
    $("#MaterialID").val(supplierInvoiceDetailVM.MaterialID).select2();
    $('#SupplierInvoiceDetail_UnitCode').val(supplierInvoiceDetailVM.UnitCode);
    $('#SupplierInvoiceDetail_Quantity').val(supplierInvoiceDetailVM.Quantity);
    $('#SupplierInvoiceDetail_Rate').val(supplierInvoiceDetailVM.Rate);
    $('#SupplierInvoiceDetail_TradeDiscountAmount').val(supplierInvoiceDetailVM.TradeDiscountAmount);
    $('#SupplierInvoiceDetail_TradeDiscountPerc').val(supplierInvoiceDetailVM.TradeDiscountPercent);
    $('#TaxTypeCode').val(supplierInvoiceDetailVM.TaxTypeCode);
    $('#SupplierInvoiceDetail_MaterialCode').val(supplierInvoiceDetailVM.MaterialCode);
    $('#SupplierInvoiceDetail_MaterialTypeDesc').val(supplierInvoiceDetailVM.MaterialTypeDesc);
    $('#SupplierInvoiceDetail_UnitCode').val(supplierInvoiceDetailVM.UnitCode);
    ValueCalculation();
    $('#SupplierInvoiceDetailModal').modal('show');
}
function GetSupplierInvoiceDetail(id) {
    try {
        debugger;
        var data = { "id": id };
        _jsonData = GetDataFromServer("SupplierInvoice/GetSupplierInvoiceDetail/", data);
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


//##16--DELETE Supplier Invoice -----------------------------------------------------##16
function DeleteClick() {
    notyConfirm('Are you sure to delete?', 'DeleteSupplierInvoice()');
}
function DeleteSupplierInvoice() {
    try {
        debugger;
        var id = $('#ID').val();
        if (id != '' && id != null) {
            var data = { "id": id };
            _jsonData = GetDataFromServer("SupplierInvoice/DeleteSupplierInvoice/", data);
            if (_jsonData != '') {
                _jsonData = JSON.parse(_jsonData);
                _result = _jsonData.Result;
                _message = _jsonData.Message;
            }
            if (_result == "OK") {
                notyAlert('success', _message);
                window.location.replace("NewSupplierInvoice?code=ACC");
            }
            if (_result == "ERROR") {
                notyAlert('error', _message);
            }
            return 1;
        }
    }
    catch (e) {
        notyAlert('error', e.message);
        return 0;
    }
}

//##17--DELETE Supplier Invoice Details------------------------------------------------##17
function DeleteDetail(curobj) {
    var rowData = _dataTables.SupplierInvoiceDetailTable.row($(curobj).parents('tr')).data();
    notyConfirm('Are you sure to delete?', 'DeleteSupplierInvoiceDetail("' + rowData.ID + '")');
}
function DeleteSupplierInvoiceDetail(id) {
    try {
        debugger;
        if (id != '' && id != null) {
            var data = { "id": id };
            _jsonData = GetDataFromServer("SupplierInvoice/DeleteSupplierInvoiceDetail/", data);
            if (_jsonData != '') {
                _jsonData = JSON.parse(_jsonData);
                _result = _jsonData.Result;
                _message = _jsonData.Message;
            }
            if (_result == "OK") {
                notyAlert('success', _message);
                BindSupplierInvoiceByID();
            }
            if (_result == "ERROR") {
                notyAlert('error', _message);
            }
            return 1;
        }
    }
    catch (e) {
        notyAlert('error', e.message);
        return 0;
    }
}

//##18--Discount Amount Changed -------------------------------------------------------##18
function DiscountAmountChanged(thisObj) {
    debugger;
    if (thisObj.value != "" && thisObj.value!=".") {
        var InvoiceAmount = $('#InvoiceAmount').val();
        var calculatedAmount = parseFloat(InvoiceAmount) - parseFloat(thisObj.value);
        $('#lblInvoiceAmount').text(roundoff(calculatedAmount));
        $('#lblStatusInvoiceAmount').text(roundoff(calculatedAmount));
    }
    else {
        $('#Discount').val(roundoff(0));
        $('#Discount').select();
    }

}
//calling from purchase order dropdown partial view
function CallForSupplierInvocie() {
    if ($("#PurchaseOrderID").val() != "" && ($("#PurchaseOrderNo").val() == "" || $("#PurchaseOrderNo").val() == undefined))
        $('#PurchaseOrderID').prop("disabled", true);
}
