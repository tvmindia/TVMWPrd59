//*****************************************************************************
//*****************************************************************************
//Author: Gibin Jacob
//CreatedDate: 20-Mar-2018 
//LastModified:  
//FileName: NewCustomerInvoice.js
//Description: Client side coding for New/Edit Customer Invoice
//******************************************************************************
// ##1--Global Declaration
// ##2--Document Ready function
// ##3--On Change Customer : Bind Customer Details
// ##4--Customer Invoice Detail Modal Popup
// ##5--Bind Packing Slip List
// ##6--Popup Tab Clicks
// ##7--Bind Packing Slip Selected List Details
// ##8-- PackingSlipListDetail DataTable: Dropdown,TextBoxes,CheckBox Binding
// ##9--Add Customer Invoice Details
// ##10--Bind Payment due date, based on Payment date
// ##11--Save  Customer Invoice 
// ##12--Save Success Customer Invoice
// ##13--Bind Customer Invoice By ID
// ##14--Reset Button Click
// ##15--Edit Popup Modal Update Customer Invoice Details
// ##16--DELETE Customer Invoice 
// ##17--DELETE Customer Invoice Details 
// ##18--Discount Amount Changed
// ##19--Email and Print
//******************************************************************************

//##1--Global Declaration---------------------------------------------##1 
var _dataTables = {};
var EmptyGuid = "00000000-0000-0000-0000-000000000000";
var _SlNo = 1;
var _result = "";
var _message = "";
var _jsonData = {};
var _taxDropdownScript = '';
var _CustomerInvoiceDetail = [];
var _CustomerInvoiceDetailLink = [];

//##2--Document Ready function-----------------------------------------##2 
$(document).ready(function () {
    debugger;
    try {
        //------select2 fields-------//
        $("#PackingSlipID").select2({ dropdownParent: $("#CustomerInvoiceDetailsModal")  });
        $("#CustomerID").select2({ });
        $("#ReferenceCustomer").select2({});
        $('#btnSendDownload').hide();
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
            FileObject.ParentType = "CustomerInvoice";
            FileObject.Controller = "FileUpload";
            UploadFile(FileObject);
        });

        _dataTables.CustomerInvoiceDetailTable = $('#tblCustomerInvoiceDetail').DataTable({
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
              { className: "text-right", "targets": [ 7, 8,9] },
              { className: "text-left", "targets": [4, 6] }
          ]
      }); 
        _dataTables.PackingSlipListTable = $('#tblPackingSlipList').DataTable({
            dom: '<"pull-right"f>rt<"bottom"ip><"clear">',
            ordering: false,
            searching: true,
            paging: true,
            "bInfo": false,
            "bSortable": false,
            autoWidth: false,
            data: null,
            pageLength: 5,
            language: {
                search: "_INPUT_",
                searchPlaceholder: "Search"
            },
            columns: [
                    { "data": "", "defaultContent": "<i></i>", "width": "5%" },
                    { "data": "SlipNo", "defaultContent": "<i></i>", "width": "10%" },
                    { "data": "SalesOrder.ReferenceCustomerName", "defaultContent": "<i></i>", "width": "25%" },
                    { "data": "DateFormatted", "defaultContent": "<i></i>", "width": "25%" },
                    { "data": "SalesOrder.SalesPersonName", "defaultContent": "<i></i>", "width": "25%" },
                    { "data": "SalesOrder.OrderNo", "defaultContent": "<i></i>", "width": "10%" }
            ],
            columnDefs: [{ orderable: false, className: 'select-checkbox', "targets": 0 }
                , { className: "text-center", "targets": [0,1,3,5] }
                , { className: "text-left", "targets": [2,3] }
            ],
            select: { style: 'multi', selector: 'td:first-child' },
            destroy: true
        });
        _dataTables.PackingSlipListDetailTable = $('#tblPackingSlipListDetail').DataTable({
            dom: '<"pull-right"f>rt<"bottom"ip><"clear">',
            ordering: false,
            searching: false,
            paging: true,
            "bInfo": false,
            "bSortable": false ,
            autoWidth: false,
            data: null,
            pageLength:5,
            language: {
                search: "_INPUT_",
                searchPlaceholder: "Search"
            },
            columns: [
                    { "data": "", "defaultContent": "<i></i>", "width": "5%" },
                    {
                        "data": "ProductName", "defaultContent": "<i>-</i>", "width": "30%",
                        'render': function (data, type, row) {
                            if (row.IsInvoiceInKG)
                                return data +'</br>(<b>Invoice in Kg </b>)'
                            else
                                return data
                        }
                    },
                    { "data": "SlipNo", "defaultContent": "<i></i>", "width": "10%" },
                    {
                        "data": "Quantity", "defaultContent": "<i>-</i>", "width": "11%",
                        'render': function (data, type, row) {
                          //  return '<input class="form-control text-right" name="Markup" value="' + data + '" type="text" onkeypress = "return isNumber(event)"  onchange="EdittextBoxValue(this,1);"style="width:100%">';
                            return data;
                        }
                    },
                    {
                        "data": "Weight", "defaultContent": "<i>-</i>", "width": "11%",
                        'render': function (data, type, row) {
                          //  return '<input class="form-control text-right" name="Markup" value="' + data + '" type="text" onkeypress = "return isNumber(event)"  onchange="EdittextBoxValue(this,2);"style="width:100%">';
                            return data;
                        }
                    },
                    {
                        "data": "Rate", "defaultContent": "<i>-</i>", "width": "11%",
                      'render': function (data, type, row) {
                          return '<input class="form-control text-right" name="Markup" value="' + data + '" type="text" onkeypress = "return isNumber(event)"  onchange="EdittextBoxValue(this,3);"style="width:100%">';
                      }
                    },
                    {
                        "data": "TradeDiscountAmount", "defaultContent": "<i>-</i>", "width": "11%",
                        'render': function (data, type, row) {
                            return '<input class="form-control text-right" name="Markup" value="' + data + '" type="text" onkeypress = "return isNumber(event)"  onchange="EdittextBoxValue(this,4);" style="width:100%">';
                        }
                    },
                    {
                        "data": "TaxTypeCode", "defaultContent": "<i>-</i>", "width": "11%",
                        'render': function (data, type, row) {
                            if (data != null)
                            {
                                var first = _taxDropdownScript.slice(0, _taxDropdownScript.indexOf('value="' + data + '"'));
                                var second = _taxDropdownScript.slice(_taxDropdownScript.indexOf('value="' + data + '"'), _taxDropdownScript.length);
                                return '<select class="form-control" onchange="EdittextBoxValue(this,5);" >' + first + ' selected="selected" ' + second + '</select>';
                            }
                            else
                            {
                                return '<select class="form-control" onchange="EdittextBoxValue(this,5);" >' + _taxDropdownScript + '</select>';
                            }                       
                        }
                    } 
            ],
            columnDefs: [{ orderable: false, className: 'select-checkbox', "targets":0 }
                , { className: "text-left", "targets": [1,6] }
                , { className: "text-right", "targets": [2, 3, 4] }
                ],
            select: { style: 'multi', selector: 'td:first-child' },
            destroy: true
        });
        _dataTables.EditPackingSlipListDetailTable = $('#tblPackingSlipListDetailEdit').DataTable( {
      dom: '<"pull-right"f>rt<"bottom"ip><"clear">',
      ordering: false,
      searching: false,
      paging: false,
      data: null,
      "bInfo": false,
      autoWidth: false,
      columns: [
                 
                  { "data": "ID", "defaultContent": "<i></i>"  },
                  { "data": "ProductName", "defaultContent": "<i>-</i>", "width": "35%",
                      'render': function (data, type, row) {
                          if (row.IsInvoiceInKG)
                              return data + '</br>(<b>Invoice in Kg </b>)'
                          else
                              return data
                      }
                  },
                  { "data": "SlipNo", "defaultContent": "<i></i>", "width": "10%" },
                  { "data": "Quantity", "defaultContent": "<i>-</i>", "width": "11%",
                      'render': function (data, type, row) {
                        //  return '<input class="form-control text-right" name="Markup" value="' + data + '" type="text" onkeypress = "return isNumber(event)"  onchange="EditLinkTableTextBoxValue(this,1);"style="width:100%">';
                          return data;
                      }
                  },
                  { "data": "Weight", "defaultContent": "<i>-</i>", "width": "11%",
                      'render': function (data, type, row) {
                        //  return '<input class="form-control text-right" name="Markup" value="' + data + '" type="text" onkeypress = "return isNumber(event)"  onchange="EditLinkTableTextBoxValue(this,2);"style="width:100%">';
                          return data;
                      }
                  },
                  { "data": "Rate", "defaultContent": "<i>-</i>", "width": "11%",
                      'render': function (data, type, row) {
                          return '<input class="form-control text-right" name="Markup" value="' + data + '" type="text" onkeypress = "return isNumber(event)"  onchange="EditLinkTableTextBoxValue(this,3);"style="width:100%">';
                      }
                  },
                  { "data": "TradeDiscountAmount", "defaultContent": "<i>-</i>", "width": "11%",
                      'render': function (data, type, row) {
                          return '<input class="form-control text-right" name="Markup" value="' + data + '" type="text" onkeypress = "return isNumber(event)"  onchange="EditLinkTableTextBoxValue(this,4);" style="width:100%">';
                      }
                  },
                  { "data": "TaxTypeCode", "defaultContent": "<i>-</i>", "width": "11%",
                  'render': function (data, type, row) {
                      debugger;
                          if (data != null) {
                              var first = _taxDropdownScript.slice(0, _taxDropdownScript.indexOf('value="' + data + '"'));
                              var second = _taxDropdownScript.slice(_taxDropdownScript.indexOf('value="' + data + '"'), _taxDropdownScript.length);
                              return '<select class="form-control" onchange="EditLinkTableTextBoxValue(this,5);" >' + first + ' selected="selected" ' + second + '</select>';
                          }
                          else {
                              return '<select class="form-control" onchange="EditLinkTableTextBoxValue(this,5);" >' + _taxDropdownScript + '</select>';
                          }
                      }
                  }
            ],
      columnDefs: [{ "targets": [0], "visible": false, searchable: false },
                    { className: "text-left", "targets": [1,] },
                    { className: "text-right", "targets": [3,4,5,6,7] }
            ]
  });

//------------------------------------------------------------------------------------------------//

        //$("#PackingSlipID").change(function () {
        //    BindPackingSlipDetails(this.value);
        //});
        debugger;
        $("#CustomerID").change(function () {
            BindCustomerDetails(this.value);
        }); 
        $("#PaymentTermCode").change(function () {
            GetDueDate(this.value);
        });
        
        if ($('#IsUpdate').val() == 'True')  {
            BindCustomerInvoiceByID()
        }
        else  {
            $('#lblCustomerInvoiceNo').text('Customer Invoice# : New');
        }
    }
    catch (e) {
        console.log(e.message);
    }
});

//##3--On Change Customer : Bind Customer Details----------------------##3
function BindCustomerDetails(customerId) {
    if (customerId != "") {
        var customerVM = GetCustomerDetails(customerId)
        $('#BillingAddress').val(customerVM.BillingAddress)
        $('#PaymentTermCode').val(customerVM.PaymentTermCode)
        GetDueDate( $('#PaymentTermCode').val());
    }
    else   {
        $('#BillingAddress').val('');
    }
}
function GetCustomerDetails(customerId) {
    try {
        var data = { "customerId": customerId };
        _jsonData = GetDataFromServer("CustomerInvoice/GetCustomerDetails/", data);
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

//##4--Customer Invoice Detail Modal Popup----------------------------##4
function ShowCustomerInvoiceDetailsModal()
{
    if ($('#CustomerInvoiceForm').valid())
    {
        ViewPackingSlipList(1);         //##6
        BindPackingSlipListTable();     //##5
        $('#CustomerInvoiceDetailsModal').modal('show');
    }
    else
    {
        notyAlert('warning', "Please Fill Required Fields");
    }
}

//##5--Bind Packing Slip List ----------------------------------------##5
function BindPackingSlipListTable() {
    var packingSlipList = GetPackingSlipList();
    _dataTables.PackingSlipListTable.clear().rows.add(packingSlipList).draw(false);
}
function GetPackingSlipList()
{
    try {
        var customerID=$("#CustomerID").val();
        var data = { "customerID": customerID };
        var PackingSlipDetailVM = new Object();
        jsonData = GetDataFromServer("CustomerInvoice/GetPackingSlipList/", data);
        if (jsonData != '') {
            jsonData = JSON.parse(jsonData);
            result = jsonData.Result;
            message = jsonData.Message;
            PackingSlipDetailVM = jsonData.Records;
        }
        if (result == "OK") {
            return PackingSlipDetailVM;
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

//##6--Popup Tab Clicks------------------------------------------------##6
function ViewPackingSlipList(value) {
    $('#tabDetail').attr('data-toggle', 'tab');
    $('#btnForward').show();
    $('#btnBackward').hide();
    $('#btnAdd').hide();
    if (value)
        $('#tabList').trigger('click');
}
function ViewPackingSlipListDetails(value) {
    $('#tabDetail').attr('data-toggle', 'tab');
    if (value)
        $('#tabDetail').trigger('click');
    else {
        //selecting Checked IDs for  bind the detail Table
        debugger;
        var packingSlipIds = GetSelectedRowPackingSlipIds();
        if (packingSlipIds) {
            BindPackingSlipListDetailTable(packingSlipIds);
            _dataTables.PackingSlipListDetailTable.rows().select();
            $('#btnForward').hide();
            $('#btnBackward').show();
            $('#btnAdd').show();
        }
        else {
           $('#tabDetail').attr('data-toggle', '');
            _dataTables.PackingSlipListDetailTable.clear().draw(false);
            notyAlert('warning', "Please Select Packing Slip");
        }
    }
}
function GetSelectedRowPackingSlipIds() {
    var SelectedRows = _dataTables.PackingSlipListTable.rows(".selected").data();
    if ((SelectedRows) && (SelectedRows.length > 0)) {
        var arrIDs = "";
        for (var r = 0; r < SelectedRows.length; r++) {
            if (r == 0)
                arrIDs = SelectedRows[r].ID;
            else
                arrIDs = arrIDs + ',' + SelectedRows[r].ID;
        }
        return arrIDs;
    }
}

//##7--Bind Packing Slip Selected List Details-------------------------##7
function BindPackingSlipListDetailTable(packingSlipIDs)
{
    debugger;
    TaxtypeDropdown(); //##8
    if (packingSlipIDs != "")
    {
        _dataTables.PackingSlipListDetailTable.clear().rows.add(GetPackingSlipListDetail(packingSlipIDs)).draw(false);
    }
}
function GetPackingSlipListDetail(packingSlipIDs) {
    try {
        var id = $('#ID').val();
        var data = { "packingSlipIDs": packingSlipIDs,"id":id };
        var PackingSlipDetailVM = new Object();
        jsonData = GetDataFromServer("CustomerInvoice/GetPackingSlipListDetail/", data);
        if (jsonData != '') {
            jsonData = JSON.parse(jsonData);
            result = jsonData.Result;
            message = jsonData.Message;
            PackingSlipDetailVM = jsonData.Records;
        }
        if (result == "OK") {
            return PackingSlipDetailVM;
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

//##8-- PackingSlipListDetail DataTable: Dropdown,TextBoxes,CheckBox Binding-----------------##8
function TaxtypeDropdown()
{
    var taxTypeVM = GetTaxtypeDropdown()
    _taxDropdownScript = "<option value="+''+">-Select-</option>";
    for (i = 0; i < taxTypeVM.length; i++)
    {
        _taxDropdownScript = _taxDropdownScript + '<option value="' + taxTypeVM[i].Code + '">' + taxTypeVM[i].Description + '</option>'
    }
}
function GetTaxtypeDropdown()
{
    try {
        var data = { };
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
function EdittextBoxValue(thisObj, textBoxCode)
{
    debugger;
    var IDs = selectedRowIDs();//identify the selected rows 
    var customerInvoiceDetailVM = _dataTables.PackingSlipListDetailTable.rows().data();
    var rowtable = _dataTables.PackingSlipListDetailTable.row($(thisObj).parents('tr')).data();
    for (var i = 0; i < customerInvoiceDetailVM.length; i++)
    {
        if (customerInvoiceDetailVM[i].ProductID == rowtable.ProductID && customerInvoiceDetailVM[i].SlipNo == rowtable.SlipNo)
        {
            if (textBoxCode == 1)
                if (thisObj.value <= customerInvoiceDetailVM[i].QuantityCheck)
                    customerInvoiceDetailVM[i].Quantity = thisObj.value;
                else
                    customerInvoiceDetailVM[i].Quantity = customerInvoiceDetailVM[i].QuantityCheck;
            if (textBoxCode == 2)
                if (thisObj.value <= customerInvoiceDetailVM[i].WeightCheck)
                    customerInvoiceDetailVM[i].Weight = thisObj.value;
                else
                    customerInvoiceDetailVM[i].Weight = customerInvoiceDetailVM[i].WeightCheck;
            if (textBoxCode == 3)
                customerInvoiceDetailVM[i].Rate = thisObj.value;
            if (textBoxCode == 4)
            {
                customerInvoiceDetailVM[i].TradeDiscountAmount = thisObj.value;
                customerInvoiceDetailVM[i].TradeDiscountPerc = 0;
            }               
            if (textBoxCode == 5)
            {
                var taxTypeVM = GetTaxtypeDropdown();
                customerInvoiceDetailVM[i].TaxTypeCode = thisObj.value;
                for (j = 0; j < taxTypeVM.length; j++) {
                    if (taxTypeVM[j].Code == thisObj.value) {
                        customerInvoiceDetailVM[i].IGSTPerc = taxTypeVM[j].IGSTPercentage;
                        customerInvoiceDetailVM[i].SGSTPerc = taxTypeVM[j].SGSTPercentage;
                        customerInvoiceDetailVM[i].CGSTPerc = taxTypeVM[j].CGSTPercentage;
                    }
                }
            }
        }
    }
    _dataTables.PackingSlipListDetailTable.clear().rows.add(customerInvoiceDetailVM).draw(false);
    selectCheckbox(IDs); //Selecting the checked rows with their ids taken 
}
function selectedRowIDs() {
    var allData = _dataTables.PackingSlipListDetailTable.rows(".selected").data();
    var arrIDs = "";
    for (var r = 0; r < allData.length; r++) {
        if (r == 0)
            arrIDs = allData[r].ProductID;
        else
            arrIDs = arrIDs + ',' + allData[r].ProductID;
    }
    return arrIDs;
}
function selectCheckbox(IDs) {
    var customerInvoiceDetailVM = _dataTables.PackingSlipListDetailTable.rows().data()
    for (var i = 0; i < customerInvoiceDetailVM.length; i++) {
        if (IDs.includes(customerInvoiceDetailVM[i].ProductID)) {
            _dataTables.PackingSlipListDetailTable.rows(i).select();
        }
        else {
            _dataTables.PackingSlipListDetailTable.rows(i).deselect();
        }
    }
}

//##9--Add Customer Invoice Details ---------------------------------------##9
function AddCustomerInvoiceDetails()
{
   
    var customerInvoiceDetailVM = _dataTables.PackingSlipListDetailTable.rows(".selected").data();
    if (customerInvoiceDetailVM.length > 0)
    {
        AddCustomerInvoiceDetailList(customerInvoiceDetailVM)
        var result = JSON.stringify(_CustomerInvoiceDetail);
        $("#DetailJSON").val(result);
        Save();
        $('#CustomerInvoiceDetailsModal').modal('hide');
    }
    else
    {
        notyAlert('warning', "No Rows Selected");
    }
}
function AddCustomerInvoiceDetailList(customerInvoiceDetailVM) {
    debugger;
    for (var r = 0; r < customerInvoiceDetailVM.length; r++) {
        CustomerInvoiceDetail = new Object();
        //   CustomerInvoiceDetail.ID = customerInvoiceDetailVM[r].ID;
        //   CustomerInvoiceDetail.CustomerInvoiceID = customerInvoiceDetailVM[r].CustomerInvoiceID;
        CustomerInvoiceDetail.PackingSlipDetailID = customerInvoiceDetailVM[r].PackingSlipDetailID;
        CustomerInvoiceDetail.ProductID = customerInvoiceDetailVM[r].ProductID;
        CustomerInvoiceDetail.Quantity = customerInvoiceDetailVM[r].Quantity;
        CustomerInvoiceDetail.Weight = customerInvoiceDetailVM[r].Weight;
        CustomerInvoiceDetail.Rate = customerInvoiceDetailVM[r].Rate;
        CustomerInvoiceDetail.TaxTypeCode = customerInvoiceDetailVM[r].TaxTypeCode == "" ? null : customerInvoiceDetailVM[r].TaxTypeCode;
        CustomerInvoiceDetail.IGSTPerc = customerInvoiceDetailVM[r].IGSTPerc;
        CustomerInvoiceDetail.SGSTPerc = customerInvoiceDetailVM[r].SGSTPerc;
        CustomerInvoiceDetail.CGSTPerc = customerInvoiceDetailVM[r].CGSTPerc;
        CustomerInvoiceDetail.TradeDiscountPerc = customerInvoiceDetailVM[r].TradeDiscountPerc;
        CustomerInvoiceDetail.TradeDiscountAmount = customerInvoiceDetailVM[r].TradeDiscountAmount;
        CustomerInvoiceDetail.IsInvoiceInKG = customerInvoiceDetailVM[r].IsInvoiceInKG;
        _CustomerInvoiceDetail.push(CustomerInvoiceDetail);
    }
}

//##10--Bind Payment due date, based on Payment date-------------##10
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
        ds = GetDataFromServer("CustomerInvoice/GetDueDate/", data);
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

//##11--Save  Customer Invoice----------------------------##11
function Save()
{
    debugger;
    _SlNo = 1;
    $('#btnSave').trigger('click');
}

//##12--Save Success Customer Invoice----------------------------##12
function SaveSuccessCustomerInvoice(data, status)
{
    _jsonData = JSON.parse(data)
    switch (_jsonData.Result) {
        case "OK":
            notyAlert("success", _jsonData.Records.Message)
            $('#IsUpdate').val('True');
            $('#ID').val(_jsonData.Records.ID)
            _CustomerInvoiceDetail = [];
            $("#DetailJSON").val('');
            BindCustomerInvoiceByID();
            break;
        case "ERROR":
            notyAlert("danger", _jsonData.Message)
            break;
        default:
            notyAlert("danger", _jsonData.Message)
            break;
    }
}

//##13--Bind Customer Invoice By ID----------------------------##13
function BindCustomerInvoiceByID()
{
    debugger;
    ChangeButtonPatchView('CustomerInvoice', 'divbuttonPatchAddCustomerInvoice', 'Edit');
    var ID = $('#ID').val();
    _SlNo = 1;
    var customerInvoiceVM = GetCustomerInvoiceByID(ID);
    $('#lblCustomerInvoiceNo').text('Customer Invoice# :' + customerInvoiceVM.InvoiceNo);
    $('#InvoiceNo').val(customerInvoiceVM.InvoiceNo);
    $('#InvoiceDateFormatted').val(customerInvoiceVM.InvoiceDateFormatted);
    $('#PaymentTermCode').val(customerInvoiceVM.PaymentTermCode);
    $('#PaymentDueDateFormatted').val(customerInvoiceVM.PaymentDueDateFormatted);
    $('#CustomerID').val(customerInvoiceVM.CustomerID).select2();
    $('#CustomerID').prop('disabled', true);
    $('#GeneralNotes').val(customerInvoiceVM.GeneralNotes);
    $('#BillingAddress').val(customerInvoiceVM.BillingAddress);
    $('#Discount').val(roundoff(customerInvoiceVM.Discount));
    $('#lblTotalTaxableAmount').text(roundoff(customerInvoiceVM.TotalTaxableAmount));
    $('#lblTotalTaxAmount').text(roundoff(customerInvoiceVM.TotalTaxAmount));
    $('#lblPaymentReceived').text(roundoff(customerInvoiceVM.PaymentReceived));
    $('#lblBalance').text(roundoff(roundoff(customerInvoiceVM.InvoiceAmount - customerInvoiceVM.Discount) - customerInvoiceVM.PaymentReceived));
    $('#lblInvoiceAmount').text(roundoff(customerInvoiceVM.InvoiceAmount - customerInvoiceVM.Discount));
    $('#lblStatusInvoiceAmount').text(roundoff(customerInvoiceVM.InvoiceAmount-customerInvoiceVM.Discount));
    $('#InvoiceAmount').val(roundoff(customerInvoiceVM.InvoiceAmount));
    $('#CustomerInvoiceMailPreview_SentToEmails').val(customerInvoiceVM.Customer.ContactEmail)
    //detail Table values binding with header id
    BindCustomerInvoiceDetailTable(ID);
    PaintImages(ID);//bind attachments written in custom js
}
function GetCustomerInvoiceByID(ID) {
    try {
        var data = { "ID": ID };
        _jsonData = GetDataFromServer("CustomerInvoice/GetCustomerInvoice/", data);
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
function BindCustomerInvoiceDetailTable(ID)
{
    _dataTables.CustomerInvoiceDetailTable.clear().rows.add(GetCustomerInvoiceDetail(ID)).draw(false);
}
function GetCustomerInvoiceDetail(ID)
{
   try {
        debugger;
        var data = { "ID": ID };
        _jsonData = GetDataFromServer("CustomerInvoice/GetCustomerInvoiceDetail/", data);
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
function Reset()
{
    BindCustomerInvoiceByID();
}

//##15--Edit Popup Modal Update Customer Invoice Details----------------------------##15
function ItemDetailsEdit(thisObj) {
    debugger;

    var rowData = _dataTables.CustomerInvoiceDetailTable.row($(thisObj).parents('tr')).data();
    TaxtypeDropdown(); //##8
    _dataTables.EditPackingSlipListDetailTable.clear().rows.add(GetCustomerInvoiceDetailLinkForEdit(rowData.ID)).draw(false);

    $('#EditCustomerInvoiceDetailModal').modal('show');

}
function GetCustomerInvoiceDetailLinkForEdit(id) {
    try {
        debugger;
        var data = { "id": id };
        _jsonData = GetDataFromServer("CustomerInvoice/GetCustomerInvoiceDetailLinkForEdit/", data);
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
function EditLinkTableTextBoxValue(thisObj, textBoxCode) {
    debugger;
    var customerInvoiceDetailVM = _dataTables.EditPackingSlipListDetailTable.rows().data();
    var rowtable = _dataTables.EditPackingSlipListDetailTable.row($(thisObj).parents('tr')).data();
    for (var i = 0; i < customerInvoiceDetailVM.length; i++) {
        if (customerInvoiceDetailVM[i].ProductID == rowtable.ProductID && customerInvoiceDetailVM[i].SlipNo == rowtable.SlipNo) {
            if (textBoxCode == 1)
                if (thisObj.value <= customerInvoiceDetailVM[i].QuantityCheck)
                    customerInvoiceDetailVM[i].Quantity = thisObj.value;
                else
                    customerInvoiceDetailVM[i].Quantity = customerInvoiceDetailVM[i].QuantityCheck;
            if (textBoxCode == 2)
                if (thisObj.value <= customerInvoiceDetailVM[i].WeightCheck)
                    customerInvoiceDetailVM[i].Weight = thisObj.value;
                else
                    customerInvoiceDetailVM[i].Weight = customerInvoiceDetailVM[i].WeightCheck;
            if (textBoxCode == 3)
                customerInvoiceDetailVM[i].Rate = thisObj.value;
            if (textBoxCode == 4)
            {
                customerInvoiceDetailVM[i].TradeDiscountAmount = thisObj.value;
                customerInvoiceDetailVM[i].TradeDiscountPerc = 0;
            }

            if (textBoxCode == 5) 
            {
                var taxTypeVM = GetTaxtypeDropdown();
                customerInvoiceDetailVM[i].TaxTypeCode = thisObj.value;
                for (j = 0; j < taxTypeVM.length; j++) {
                    if (taxTypeVM[j].Code == thisObj.value) {
                        customerInvoiceDetailVM[i].IGSTPerc = taxTypeVM[j].IGSTPercentage;
                        customerInvoiceDetailVM[i].SGSTPerc = taxTypeVM[j].SGSTPercentage;
                        customerInvoiceDetailVM[i].CGSTPerc = taxTypeVM[j].CGSTPercentage;
                    }
                }
            }
        }
    }
    _dataTables.EditPackingSlipListDetailTable.clear().rows.add(customerInvoiceDetailVM).draw(false);
}
function UpdateCustomerInvoiceDetails()
{
    debugger;
    var CustomerInvoiceDetailVM = _dataTables.EditPackingSlipListDetailTable.rows().data();
    _CustomerInvoiceDetailLink = [];
    UpdateCustomerInvoiceDetailLinkVM(CustomerInvoiceDetailVM)
    customerInvoiceVM = new Object();
    customerInvoiceVM.CustomerInvoiceDetailList = new Object();
    customerInvoiceVM.CustomerInvoiceDetailList = _CustomerInvoiceDetailLink;
    customerInvoiceVM.ID = $('#ID').val();
    customerInvoiceVM.InvoiceDateFormatted=$('#InvoiceDateFormatted').val();
    var data = "{'customerInvoiceVM':" + JSON.stringify(customerInvoiceVM) + "}";

    PostDataToServer("CustomerInvoice/UpdateCustomerInvoiceDetail/", data, function (JsonResult) {

        debugger;
        switch (JsonResult.Result) {
            case "OK":
                notyAlert('success', JsonResult.Records.Message);
                BindCustomerInvoiceByID() 
                break;
            case "Error":
                notyAlert('error', JsonResult.Message);
                break;
            case "ERROR":
                notyAlert('error', JsonResult.Message);
                break;
            default:
                break;
        }
    })
    $('#EditCustomerInvoiceDetailModal').modal('hide');
}
function UpdateCustomerInvoiceDetailLinkVM(CustomerInvoiceDetailLinkVM) {
    debugger;
    for (var r = 0; r < CustomerInvoiceDetailLinkVM.length; r++) {
        CustomerInvoiceDetail = new Object();
        CustomerInvoiceDetail.ID = CustomerInvoiceDetailLinkVM[r].ID;
        CustomerInvoiceDetail.CustomerInvoiceDetailLinkID = CustomerInvoiceDetailLinkVM[r].CustomerInvoiceDetailLinkID;
        CustomerInvoiceDetail.Quantity = CustomerInvoiceDetailLinkVM[r].Quantity;
        CustomerInvoiceDetail.Weight = CustomerInvoiceDetailLinkVM[r].Weight;
        CustomerInvoiceDetail.Rate = CustomerInvoiceDetailLinkVM[r].Rate;
        CustomerInvoiceDetail.IGSTPerc = CustomerInvoiceDetailLinkVM[r].IGSTPerc;
        CustomerInvoiceDetail.SGSTPerc = CustomerInvoiceDetailLinkVM[r].SGSTPerc;
        CustomerInvoiceDetail.CGSTPerc = CustomerInvoiceDetailLinkVM[r].CGSTPerc;
        CustomerInvoiceDetail.TaxTypeCode = CustomerInvoiceDetailLinkVM[r].TaxTypeCode == "" ? null : CustomerInvoiceDetailLinkVM[r].TaxTypeCode;
        CustomerInvoiceDetail.TradeDiscountPerc = CustomerInvoiceDetailLinkVM[r].TradeDiscountPerc;
        CustomerInvoiceDetail.TradeDiscountAmount = CustomerInvoiceDetailLinkVM[r].TradeDiscountAmount;
        _CustomerInvoiceDetailLink.push(CustomerInvoiceDetail);
    }
}


//##16--DELETE Customer Invoice -----------------------------------------------------##16
function DeleteClick()
{
    notyConfirm('Are you sure to delete?', 'DeleteCustomerInvoice()');
}
function DeleteCustomerInvoice() {
    try {
        debugger;
        var id = $('#ID').val();
        if (id != '' && id != null) {
            var data = { "id": id };
            _jsonData = GetDataFromServer("CustomerInvoice/DeleteCustomerInvoice/", data);
            if (_jsonData != '') {
                _jsonData = JSON.parse(_jsonData);
                _result = _jsonData.Result;
                _message = _jsonData.Message;
            }
            if (_result == "OK") {
                notyAlert('success', _message);
                window.location.replace("NewCustomerInvoice?code=ACC");
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

//##17--DELETE Customer Invoice Details------------------------------------------------##17
function DeleteDetail(curobj)
{
    debugger;
    if (_dataTables.CustomerInvoiceDetailTable.rows().count() > 1)
    {
        var rowData = _dataTables.CustomerInvoiceDetailTable.row($(curobj).parents('tr')).data();
        notyConfirm('Are you sure to delete?', 'DeleteCustomerInvoiceDetail("' + rowData.ID + '")');
    }
    else
    {
        notyAlert('warning', "Can't delete item detail, Minimum one item is required");
    }
 
}
function DeleteCustomerInvoiceDetail(id) {
    try {
      
        if (id != '' && id != null) {
            var data = { "id": id };
            _jsonData = GetDataFromServer("CustomerInvoice/DeleteCustomerInvoiceDetail/", data);
            if (_jsonData != '') {
                _jsonData = JSON.parse(_jsonData);
                _result = _jsonData.Result;
                _message = _jsonData.Message;
            }
            if (_result == "OK") {
                notyAlert('success', _message);
                BindCustomerInvoiceByID();
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
function DiscountAmountChanged(thisObj)
{
    debugger;
    if (thisObj.value != "" && thisObj.value != ".")
    {
            var InvoiceAmount = $('#InvoiceAmount').val();
            var calculatedAmount = parseFloat(InvoiceAmount) - parseFloat(thisObj.value);
            $('#lblInvoiceAmount').text(roundoff(calculatedAmount));
            $('#lblStatusInvoiceAmount').text(roundoff(calculatedAmount));
    }
    else
    {
        $('#Discount').val(roundoff(0));
        $('#Discount').select();
    }
   
}

//##19--Email and Print------------------------------------------------------------------##19
//Email Sending
function EmailPreview(flag) {
    try {
        debugger;
        var headerID = $("#ID").val();
        if (headerID) {
            //Bind mail html into model
            GetMailPreview(headerID, flag);
            $("#MailPreviewModel").modal('show'); 
        }
    }
    catch (e) {
        notyAlert('error', e.Message);
    }
}

function GetMailPreview(ID) {
    debugger;
    var data = { "ID": ID};
    var jsonData = {};
    jsonData = GetDataFromServer("CustomerInvoice/GetMailPreview/", data);
    if (jsonData == "Nochange") {
        return; 0
    }
    $("#mailmodelcontent").empty();
    $("#mailmodelcontent").html(jsonData);
    $("#mailBodyText").val(jsonData);
}
 
function SendMailClick() {
    debugger;
        $('#btnFormSendMail').trigger('click');
        $('#btnMail').hide();
}

function ValidateEmail() {
    debugger;
    var ste = $('#CustomerInvoiceMailPreview_SentToEmails').val();
    if (ste) {
        var atpos = ste.indexOf("@");
        var dotpos = ste.lastIndexOf(".");
        if (atpos < 1 || dotpos < atpos + 2 || dotpos + 2 >= ste.length) {
            notyAlert('error', 'Invalid Email');
            return false;
        }
            //not valid
        else {
            $("#MailPreviewModel").modal('hide');
            OnServerCallBegin();
            return true;
        }
    }
    else
        notyAlert('error', 'Enter email address');
    return false;
}

function MailSuccess(data, status) {
    debugger;
    var JsonResult = JSON.parse(data)
    switch (JsonResult.Result) {
        case "OK":
            notyAlert('success', JsonResult.Message);
            OnServerCallComplete(); 
            Reset();
            break;
        case "ERROR":
            notyAlert('error', JsonResult.Message); 
            break;
        default:
            notyAlert('error', JsonResult.Message);
            break;
    }
}
//To trigger PDF download button
function DownloadPDF() {
    debugger; 
        GetHtmlData();
        $('#btnSendDownload').trigger('click');
}

//To download file in PDF
function GetHtmlData() {
    debugger;
    var bodyContent = $('#mailmodelcontent').html();
    var headerContent = $('#hdnHeadContent').html();
    $('#hdnContent').val(bodyContent);
    $('#hdnHeadContent').val(headerContent);
    var customerName = $("#CustomerID option:selected").text();
    $('#hdnCustomerName').val(customerName);

} 