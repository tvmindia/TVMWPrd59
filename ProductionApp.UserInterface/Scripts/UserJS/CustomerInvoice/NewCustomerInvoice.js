//*****************************************************************************
//*****************************************************************************
//Author: Gibin Jacob
//CreatedDate: 20-Mar-2018 
//LastModified:  
//FileName: NewCustomerInvoice.js
//Description: Client side coding for New/Edit Customer Invoice
//******************************************************************************
//******************************************************************************

//--Global Declaration--//
var DataTables = {};
var EmptyGuid = "00000000-0000-0000-0000-000000000000";
var _SlNo = 1;
var _result = "";
var _message = "";
var _jsonData = {};
var _taxDropdownScript = '';

$(document).ready(function () {
    debugger;
    try {
        //------select2 fields-------//
        $("#PackingSlipID").select2({ dropdownParent: $("#CustomerInvoiceDetailsModal")  });
        $("#CustomerID").select2({ });

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
     

        DataTables.PackingSlipDetailToInvocieTable = $('#tblPackingSlipDetailToInvocie').DataTable({
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
                        "data": "ProductName", "defaultContent": "<i>-</i>", "width": "23%",
                        'render': function (data, type, row) {
                            if (row.IsInvoiceInKG)
                                return data +'</br>(<b>Invoice in Kg </b>)'
                            else
                                return data
                        }
                    },
                    {
                        "data": "Quantity", "defaultContent": "<i>-</i>", "width": "9%",
                        'render': function (data, type, row) {
                            return '<input class="form-control text-right" name="Markup" value="' + data + '" type="text" onkeypress = "return isNumber(event)"  onchange="EdittextBoxValue(this,1);"style="width:100%">';
                        }
                    },
                    {
                        "data": "Weight", "defaultContent": "<i>-</i>", "width": "9%",
                        'render': function (data, type, row) {
                            return '<input class="form-control text-right" name="Markup" value="' + data + '" type="text" onkeypress = "return isNumber(event)"  onchange="EdittextBoxValue(this,2);"style="width:100%">';
                        }
                    },
                    {
                        "data": "Rate", "defaultContent": "<i>-</i>", "width": "9%",
                      'render': function (data, type, row) {
                          return '<input class="form-control text-right" name="Markup" value="' + data + '" type="text" onkeypress = "return isNumber(event)"  onchange="EdittextBoxValue(this,3);"style="width:100%">';
                      }
                    },
                    {
                        "data": "TradeDiscountAmount", "defaultContent": "<i>-</i>", "width": "9%",
                        'render': function (data, type, row) {
                            return '<input class="form-control text-right" name="Markup" value="' + data + '" type="text" onkeypress = "return isNumber(event)"  onchange="EdittextBoxValue(this,4);" style="width:100%">';
                        }
                    },
                    {
                        "data": "TaxTypeCode", "defaultContent": "<i>-</i>", "width": "9%",
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



        $("#PackingSlipID").change(function () {
            BindPackingSlipDetails(this.value);
        });

        $("#CustomerID").change(function () {
            BindCustomerDetails(this.value);
        });

        debugger;
        if ($('#IsUpdate').val() == 'True') {
            BindSalesOrderByID()
        }
        else {
            $('#lblCustomerInvoiceNo').text('Customer Invoice# : New');
        }


    }
    catch (e) {
        console.log(e.message);
    }
});

function BindCustomerDetails(customerId) {
    var customerVM = GetCustomerDetails(customerId)
    $('#BillingAddress').val(customerVM.BillingAddress);
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

function ShowCustomerInvoiceDetailsModal()
{
    $('#CustomerInvoiceDetailsModal').modal('show');
}


function BindPackingSlipDetails(packingSlipID)
{
    debugger;
    TaxtypeDropdown();
    if (packingSlipID != "")
    {
        var PackingSlipVM = GetPackingSlip(packingSlipID);
        $('#PackingSlip_SlipNo').val(PackingSlipVM.SlipNo);
        $('#PackingSlip_SalesOrder_CustomerName').val(PackingSlipVM.SalesOrder.CustomerName);
        $('#PackingSlip_SalesOrder_OrderNo').val(PackingSlipVM.SalesOrder.OrderNo);
        DataTables.PackingSlipDetailToInvocieTable.clear().rows.add(GetPackingSlipDetail(packingSlipID)).draw(false);
    }
    else
    {
        $('#PackingSlip_SlipNo').val('');
        $('#PackingSlip_SalesOrder_CustomerName').val('');
        $('#PackingSlip_SalesOrder_OrderNo').val('');
        DataTables.PackingSlipDetailToInvocieTable.clear().draw(false);
    }
}

function GetPackingSlip(packingSlipID)
{
    try {
        var data = { "packingSlipID": packingSlipID };
        var PackingSlipVM = new Object();

        jsonData = GetDataFromServer("CustomerInvoice/GetPackingSlip/", data);
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

function GetPackingSlipDetail(packingSlipId) {
    try {
        var data = {"packingSlipID": packingSlipId };
        var PackingSlipDetailVM = new Object();

        jsonData = GetDataFromServer("CustomerInvoice/GetPackingSlipDetail/", data);
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
    var customerInvoiceDetailVM = DataTables.PackingSlipDetailToInvocieTable.rows().data();
    var rowtable = DataTables.PackingSlipDetailToInvocieTable.row($(thisObj).parents('tr')).data();
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
    DataTables.PackingSlipDetailToInvocieTable.clear().rows.add(customerInvoiceDetailVM).draw(false);
    selectCheckbox(IDs); //Selecting the checked rows with their ids taken 
}

function selectedRowIDs() {
    var allData = DataTables.PackingSlipDetailToInvocieTable.rows(".selected").data();
    var arrIDs = "";
    for (var r = 0; r < allData.length; r++) {
        if (r == 0)
            arrIDs = allData[r].ProductID;
        else
            arrIDs = arrIDs + ',' + allData[r].ProductID;
    }
    return arrIDs;
}
//selected Checkbox
function selectCheckbox(IDs) {
    var customerInvoiceDetailVM = DataTables.PackingSlipDetailToInvocieTable.rows().data()
    for (var i = 0; i < customerInvoiceDetailVM.length; i++) {
        if (IDs.includes(customerInvoiceDetailVM[i].ProductID)) {
            DataTables.PackingSlipDetailToInvocieTable.rows(i).select();
        }
        else {
            DataTables.PackingSlipDetailToInvocieTable.rows(i).deselect();
        }
    }
}


function AddCustomerInvoiceDetails()
{
    var customerInvoiceDetailVM = DataTables.PackingSlipDetailToInvocieTable.rows(".selected").data();

    $('#CustomerInvoiceDetailsModal').modal('hide');
}

