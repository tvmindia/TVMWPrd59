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
// 
// 
//******************************************************************************

//##1--Global Declaration---------------------------------------------##1 
var DataTables = {};
var EmptyGuid = "00000000-0000-0000-0000-000000000000";
var _SlNo = 1;
var _result = "";
var _message = "";
var _jsonData = {};
var _taxDropdownScript = '';
var _CustomerInvoiceDetail = []; 

//##2--Document Ready function-----------------------------------------##2 
$(document).ready(function () {
    debugger;
    try {
        //------select2 fields-------//
        $("#PackingSlipID").select2({ dropdownParent: $("#CustomerInvoiceDetailsModal")  });
        $("#CustomerID").select2({ });
        $("#ReferenceCustomer").select2({});

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

        DataTables.CustomerInvoiceDetailTable = $('#tblCustomerInvoiceDetail').DataTable(
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
            { "data": "ProductName", "defaultContent": "<i>-</i>", "width": "" },
            { "data": "Quantity", "defaultContent": "<i>-</i>", "width": "" },
            { "data": "Weight", "defaultContent": "<i>-</i>", "width": "" },
            { "data": "Rate", "defaultContent": "<i>-</i>", "width": "" },
            { "data": "TradeDiscountAmount", "defaultContent": "<i>-</i>", "width": "" },
            { "data": "TaxableAmount", "defaultContent": "<i>-</i>", "width": "" },
            { "data": "TaxTypeCode", "defaultContent": "<i>-</i>", "width": "9%" },
            { "data": "Total", "defaultContent": "<i>-</i>", "width": "9%" },
            { "data": null, "orderable": false, "defaultContent": '<a href="#" class="actionLink"  onclick="ItemDetailsEdit(this)" ><i class="glyphicon glyphicon-pencil" aria-hidden="true"></i></a>  |  <a href="#" class="DeleteLink"  onclick="Delete(this)" ><i class="glyphicon glyphicon-trash" aria-hidden="true"></i></a>', "width": "7%" }

          ],
          columnDefs: [{ "targets": [], "visible": false, searchable: false },
              { className: "text-center", "targets": [9] },
              { className: "text-right", "targets": [6, 7, 8] },
              { className: "text-left", "targets": [3, 5] }
          ]
      }); 
     
        DataTables.PackingSlipListTable = $('#tblPackingSlipList').DataTable({
            dom: '<"pull-right"f>rt<"bottom"ip><"clear">',
            ordering: false,
            searching: false,
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
        DataTables.PackingSlipListDetailTable = $('#tblPackingSlipListDetail').DataTable({
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
                            return '<input class="form-control text-right" name="Markup" value="' + data + '" type="text" onkeypress = "return isNumber(event)"  onchange="EdittextBoxValue(this,1);"style="width:100%">';
                        }
                    },
                    {
                        "data": "Weight", "defaultContent": "<i>-</i>", "width": "11%",
                        'render': function (data, type, row) {
                            return '<input class="form-control text-right" name="Markup" value="' + data + '" type="text" onkeypress = "return isNumber(event)"  onchange="EdittextBoxValue(this,2);"style="width:100%">';
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


//------------------------------------------------------------------------------------------------//
        //$("#PackingSlipID").change(function () {
        //    BindPackingSlipDetails(this.value);
        //});
        $("#CustomerID").change(function () {
            BindCustomerDetails(this.value);
        }); 
        $("#PaymentTermCode").change(function () {
            GetDueDate(this.value);
        });
        
        if ($('#IsUpdate').val() == 'True')  {
            BindSalesOrderByID()
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
        $('#BillingAddress').val(customerVM.BillingAddress);
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
    DataTables.PackingSlipListTable.clear().rows.add(packingSlipList).draw(false);
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
            DataTables.PackingSlipListDetailTable.rows().select();
            $('#btnForward').hide();
            $('#btnBackward').show();
            $('#btnAdd').show();
        }
        else {
           $('#tabDetail').attr('data-toggle', '');
            DataTables.PackingSlipListDetailTable.clear().draw(false);
            notyAlert('warning', "Please Select Packing Slip");
        }
    }
}
function GetSelectedRowPackingSlipIds() {
    var SelectedRows = DataTables.PackingSlipListTable.rows(".selected").data();
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
        DataTables.PackingSlipListDetailTable.clear().rows.add(GetPackingSlipListDetail(packingSlipIDs)).draw(false);
    }
}

function GetPackingSlipListDetail(packingSlipIDs) {
    try {
        var data = { "packingSlipIDs": packingSlipIDs };
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
    var customerInvoiceDetailVM = DataTables.PackingSlipListDetailTable.rows().data();
    var rowtable = DataTables.PackingSlipListDetailTable.row($(thisObj).parents('tr')).data();
    for (var i = 0; i < customerInvoiceDetailVM.length; i++)
    {
        if (customerInvoiceDetailVM[i].ProductID == rowtable.ProductID)
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
                customerInvoiceDetailVM[i].TradeDiscountAmount = thisObj.value;
            if (textBoxCode == 5)
                customerInvoiceDetailVM[i].TaxTypeCode = thisObj.value;
        }
    }
    DataTables.PackingSlipListDetailTable.clear().rows.add(customerInvoiceDetailVM).draw(false);
    selectCheckbox(IDs); //Selecting the checked rows with their ids taken 
}
function selectedRowIDs() {
    var allData = DataTables.PackingSlipListDetailTable.rows(".selected").data();
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
    var customerInvoiceDetailVM = DataTables.PackingSlipListDetailTable.rows().data()
    for (var i = 0; i < customerInvoiceDetailVM.length; i++) {
        if (IDs.includes(customerInvoiceDetailVM[i].ProductID)) {
            DataTables.PackingSlipListDetailTable.rows(i).select();
        }
        else {
            DataTables.PackingSlipListDetailTable.rows(i).deselect();
        }
    }
}

//##9--Add Customer Invoice Details ---------------------------------------##9

function AddCustomerInvoiceDetails()
{
    var customerInvoiceDetailVM = DataTables.PackingSlipListDetailTable.rows(".selected").data();
    if (customerInvoiceDetailVM.length > 0)
    {
        AddCustomerInvoiceDetailList(customerInvoiceDetailVM)
        var result = JSON.stringify(_CustomerInvoiceDetail);
        $("#DetailJSON").val(result);
        _SlNo = 1;
        $('#btnSave').trigger('click');
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

//##11--Save Success Customer Invoice----------------------------##11
function SaveSuccessCustomerInvoice(data, status)
{
    _jsonData = JSON.parse(data)
    switch (_jsonData.Result) {
        case "OK":
            $('#IsUpdate').val('True');
            $('#ID').val(_jsonData.Records.ID)
            BindCustomerInvoiceByID();
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

//##11--Bind Customer Invoice By ID----------------------------##11
function BindCustomerInvoiceByID()
{


}