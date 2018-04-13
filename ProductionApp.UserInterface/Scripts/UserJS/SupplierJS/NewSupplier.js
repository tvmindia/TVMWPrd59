//*****************************************************************************
//*****************************************************************************
//Author: Jais
//CreatedDate: 27-Mar-2018 
//LastModified: 27-Mar-2018 
//FileName: NewSupplier.js
//Description: Client side coding for New/Edit Supplier
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
            BindSupplier()
        }
        else {
            $('#lblSupplierNo').text('Supplier# : New');
        }

    }
    catch (e) {
        console.log(e.message);
    }
});

//--function bind the Supplier --//
function BindSupplier() {
    var id = $('#ID').val();
    var supplier = GetSupplier(id);
    debugger;
    $('#ID').val(supplier.ID);
    $('#CompanyName').val(supplier.CompanyName);
    $('#ContactPerson').val(supplier.ContactPerson);
    $('#ContactEmail').val(supplier.ContactEmail);
    $('#ContactTitle').val(supplier.ContactTitle);
    $('#Product').val(supplier.Product);
    $('#Website').val(supplier.Website);
    $('#LandLine').val(supplier.LandLine);
    $('#Mobile').val(supplier.Mobile);
    $('#Fax').val(supplier.Fax);
    $('#OtherPhoneNos').val(supplier.OtherPhoneNos);
    $('#BillingAddress').val(supplier.BillingAddress);
    $('#ShippingAddress').val(supplier.ShippingAddress);
    $('#PaymentTermCode').val(supplier.PaymentTermCode);
    $('#TaxRegNo').val(supplier.TaxRegNo);
    $('#PANNo').val(supplier.PANNo);
    $('#GeneralNotes').val(supplier.GeneralNotes);
    ChangeButtonPatchView('Supplier', 'divbuttonPatchAddSupplier', 'Edit');
}

//--Function To Get Supplier Details By ID--//
function GetSupplier(id) {
    try {
        debugger;
        var data = { "id": id };
        var ds = {};
        ds = GetDataFromServer("Supplier/GetSupplierDetails/", data);
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
    debugger;
    $('#btnSave').trigger('click');
}

//--Function To Reset --//
function Reset() {
    BindSupplier();
}

//--Function on Save Success--//
function SaveSuccessSupplier(data, status) {
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
                ChangeButtonPatchView("Supplier", "divbuttonPatchAddSupplier", "Edit");
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

//--Function To Confirm Supplier Deletion 
function DeleteClick() {
    debugger;
    notyConfirm('Are you sure to delete?', 'DeleteSupplier()');
}

//--Function To Delete Supplier
function DeleteSupplier() {
    try {
        debugger;
        var id = $('#ID').val();
        if (id) {
            var data = { "id": id };
            var jsonData = {};
            var message = "";
            var status = "";
            var result = "";
            jsonData = GetDataFromServer("Supplier/DeleteSupplier/", data);
            if (jsonData != '') {
                jsonData = JSON.parse(jsonData);
                message = jsonData.Message;
                status = jsonData.Status;
                result = jsonData.Record;
            }
            switch (status) {
                case "OK":
                    notyAlert('success', result.Message);
                    window.location.replace("NewSupplier?code=MSTR");
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

