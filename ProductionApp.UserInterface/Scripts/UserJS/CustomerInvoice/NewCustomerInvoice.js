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

$(document).ready(function () {
    debugger;
    try {
        $("#PackingSlipID").select2({
            dropdownParent: $("#CustomerInvoiceDetailsModal")
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
            FileObject.ParentType = "CustomerInvoice";
            FileObject.Controller = "FileUpload";
            UploadFile(FileObject);
        });

        DataTables.SalesOrderDetailTable = $('#tblCustomerInvoiceDetail').DataTable(
      {
          dom: '<"pull-right"f>rt<"bottom"ip><"clear">',
          ordering: false,
          searching: false,
          paging: false,
          data: null,
          "bInfo": false,
          autoWidth: false,
          columns: [
          { "data": "ID", "defaultContent": "<i></i>" },
          { "data": "", "defaultContent": "<i></i>" },
           {
               "data": "", render: function (data, type, row) {
                   return _SlNo++
               }, "width": "5%"
           }, 
          { "data": "ItemDescription", "defaultContent": "<i></i>", "width": "7%" },
          { "data": "TaxTypeDescription", "defaultContent": "<i></i>", "width": "7%" },
          { "data": "Quantity", render: function (data, type, row) { return data + ' ' + row.UnitCode }, "defaultContent": "<i></i>", "width": "8%" },
          { "data": "Rate", render: function (data, type, row) { return roundoff(data) }, "defaultContent": "<i></i>", "width": "8%" },
          { "data": "GrossAmount", render: function (data, type, row) { return roundoff(data) }, "defaultContent": "<i></i>", "width": "8%" },
          { "data": "TradeDiscountAmount", render: function (data, type, row) { return data }, "defaultContent": "<i></i>", "width": "8%" },
          { "data": "TaxAmount", render: function (data, type, row) { return roundoff(data) }, "defaultContent": "<i></i>", "width": "9%" },
          { "data": "NetAmount", render: function (data, type, row) { return roundoff(data) }, "defaultContent": "<i></i>", "width": "9%" },
          { "data": "ExpectedDeliveryDateFormatted", "defaultContent": "<i></i>", "width": "10%" },
          { "data": null, "orderable": false, "defaultContent": '<a href="#" class="actionLink"  onclick="ItemDetailsEdit(this)" ><i class="glyphicon glyphicon-pencil" aria-hidden="true"></i></a>  |  <a href="#" class="DeleteLink"  onclick="Delete(this)" ><i class="glyphicon glyphicon-trash" aria-hidden="true"></i></a>', "width": "7%" }

          ],
          columnDefs: [{ "targets": [0, 1], "visible": false, searchable: false },
              { className: "text-center", "targets": [12, 11, 4] },
              { className: "text-right", "targets": [6, 7, 8, 9, 10] },
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
            data: null,
            pageLength: 15,
            language: {
                search: "_INPUT_",
                searchPlaceholder: "Search"
            },
            columns: [
                 { "data": "", "defaultContent": "<i>-</i>" },
                 { "data": "","width": "5%" },
                 { "data": "Product", "defaultContent": "<i>-</i>" },
                 { "data": " ", "defaultContent": "<i>-</i>" },
                 { "data": " ", "defaultContent": "<i>-</i>" },
                 { "data": " ", "defaultContent": "<i>-</i>" },
                 { "data": " ", "defaultContent": "<i>-</i>" },
                 { "data": " ", "defaultContent": "<i>-</i>" } 
            ],
            columnDefs: [{ orderable: false, className: 'select-checkbox', "targets": 1 }
                , { className: "text-right", "targets": [] }
                , { className: "text-left", "targets": [] }
                , { "targets": [0], "visible": false, "searchable": false }
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
    var PackingSlipVM = GetPackingSlip(packingSlipID);
    $('#PackingSlip_SlipNo').val(PackingSlipVM.SlipNo);
    $('#PackingSlip_SalesOrder_CustomerName').val(PackingSlipVM.SalesOrder.CustomerName);
    $('#PackingSlip_SalesOrder_OrderNo').val(PackingSlipVM.SalesOrder.OrderNo);

    DataTables.PackingSlipDetailToInvocieTable.clear(GetPackingSlipDetail(packingSlipID)).rows.add().draw(false);

}

function GetPackingSlip(packingSlipID)
{
    try {
        debugger;
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
        debugger;
        var data = {"packingSlipID": packingSlipID };
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

function AddCustomerInvoiceDetails()
{
    $('#CustomerInvoiceDetailsModal').modal('hide');
}

