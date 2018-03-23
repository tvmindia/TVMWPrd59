//*****************************************************************************
//*****************************************************************************
//Author: Jais
//CreatedDate: 22-Mar-2018 
//LastModified: 22-Mar-2018 
//FileName: NewCustomer.js
//Description: Client side coding for New/Edit Customer
//******************************************************************************
//******************************************************************************

//--Global Declaration--//
var _dataTables = {};
var _emptyGuid = "00000000-0000-0000-0000-000000000000";

//--Loading DOM--//
$(document).ready(function () {
    debugger;
    try {

        debugger;
        if ($('#IsUpdate').val() == 'True') {
            BindCustomer()
        }
        else {
            $('#lblCustumerNo').text('Customer# : New');
        }

    }
    catch (e) {
        console.log(e.message);
    }
});

//--function bind the Customer --//
function BindCustomer() {
    var id = $('#ID').val();
    var customer = GetCustomer(id);
    debugger;
    $('#ID').val(customer.ID);
    $('#CompanyName').val(customer.CompanyName);
    $('#ContactPerson').val(customer.ContactPerson);
    $('#ContactEmail').val(customer.ContactEmail);
    $('#ContactTitle').val(customer.ContactTitle);
    $('#Website').val(customer.Website);
    $('#LandLine').val(customer.LandLine);
    $('#Mobile').val(customer.Mobile);
    $('#Fax').val(customer.Fax);
    $('#OtherPhoneNumbers').val(customer.OtherPhoneNumbers);
    $('#BillingAddress').val(customer.BillingAddress);
    $('#ShippingAddress').val(customer.ShippingAddress);
    $('#PaymentTermCode').val(customer.PaymentTermCode);
    $('#TaxRegNo').val(customer.TaxRegNo);
    $('#PANNo').val(customer.PANNo);
    $('#GeneralNotes').val(customer.GeneralNotes);
    ChangeButtonPatchView('Customer', 'divbuttonPatchAddCustomer', 'Edit');
}

//--Function To Get Customer Details By ID--//
function GetCustomer(id) {
    try {
        debugger;
        var data = { "id": id };
        var ds = {};
        ds = GetDataFromServer("Customer/GetCustomerDetails/", data);
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
        console.log(e.message);
    }
}

//--Function To Trigger Save button--//
function Save() {
    $('#btnSave').trigger('click');
}

//--Function on Save Success--//
function SaveSuccessCustomer(data, status) {
    try {
        debugger;
        var jsonData = JSON.parse(data)
        //message field will return error msg only
        message = jsonData.Message;
        status = jsonData.Status;
        result = jsonData.Record;
        switch (status) {
            case "OK":
                $('#IsUpdate').val('True');
                $('#ID').val(result.ID);
                ChangeButtonPatchView("Customer", "divbuttonPatchAddCustomer", "Edit");
                notyAlert('success', result.Message);
                break;
            case "ERROR":
                notyAlert('error', message);
                break;
            default:
                break;
        }
    }
    catch (e) {
        //this will show the error msg in the browser console(F12) 
        console.log(e.message);
    }
}

//--Function To Confirm Customer Deletion 
function DeleteClick() {
    debugger;
    notyConfirm('Are you sure to delete?', 'DeleteCustomer()');
}

//--Function To Delete Customer
function DeleteCustomer() {
    try {
        var id = $('#ID').val();
        if (id) {
            var data = { "id": id };
            var jsonData = {};
            var message = "";
            var status = "";
            var result = "";
            jsonData = GetDataFromServer("Customer/DeleteCustomer/", data);
            if (jsonData != '') {
                jsonData = JSON.parse(jsonData);
                message = jsonData.Message;
                status = jsonData.Status;
                result = jsonData.Record;
            }
            switch (status) {
                case "OK":
                    notyAlert('success', result.Message);
                    BindOrReloadCustomerTable('Init');
                    break;
                case "ERROR":
                    notyAlert('error', message);
                    break;
                default:
                    break;
            }
        }
    }
    catch (e) {
        //this will show the error msg in the browser console(F12) 
        console.log(e.message);
    }
}

