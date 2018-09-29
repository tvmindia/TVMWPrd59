//*****************************************************************************
//*****************************************************************************
//Author: Gibin Jacob
//CreatedDate: 27-AUG-2018 
//LastModified:  
//FileName: NewSupplierCreditNote.js
//Description: Client side coding for New/Edit Supplier CreditNote
//******************************************************************************
// ##1--Global Declaration
// ##2--Document Ready function
// ##3--Save  Supplier CreditNote 
// ##4--Save Success Supplier CreditNote
// ##5--Bind Supplier CreditNote By ID
// ##6--Reset Button Click
// ##7--DELETE Supplier CreditNote
//******************************************************************************

//##1--Global Declaration---------------------------------------------##1 
var _dataTables = {};
var EmptyGuid = "00000000-0000-0000-0000-000000000000";
var _SlNo = 1;
var _result = "";
var _message = "";
var _jsonData = {};


//##2--Document Ready function-----------------------------------------##2 
$(document).ready(function () {
    debugger;
    try {
        //------select2 fields-------//
        $("#SupplierID").select2({});

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
            FileObject.ParentType = "SupplierCreditNote";
            FileObject.Controller = "FileUpload";
            UploadFile(FileObject);
        });


        $("#SupplierID").change(function () {

        });


        if ($('#IsUpdate').val() == 'True') {
            BindSupplierCreditNoteByID()
        }
        else {
            $('#lblSupplierCreditNoteNo').text('Credit Note# : New');
        }
    }
    catch (e) {
        console.log(e.message);
    }
});


//##3--Save  Supplier CreditNote----------------------------##3
function Save() {
    debugger;
    _SlNo = 1;
    $('#btnSave').trigger('click');
}

//##4--Save Success Supplier CreditNote----------------------------##4
function SaveSuccessSupplierCreditNote(data, status) {
    _jsonData = JSON.parse(data)
    switch (_jsonData.Result) {
        case "OK":
            notyAlert("success", _jsonData.Records.Message)
            $('#IsUpdate').val('True');
            $('#ID').val(_jsonData.Records.ID)
            BindSupplierCreditNoteByID();
            break;
        case "ERROR":
            notyAlert("danger", _jsonData.Message)
            break;
        default:
            notyAlert("danger", _jsonData.Message)
            break;
    }
}

//##5--Bind Supplier CreditNote By ID----------------------------##5
function BindSupplierCreditNoteByID() {
    debugger;
    ChangeButtonPatchView('SupplierCreditNote', 'divbuttonPatchAddSupplierCreditNote', 'Edit');
    var ID = $('#ID').val();
    _SlNo = 1;
    var supplierCreditNoteVM = GetSupplierCreditNoteByID(ID);
    $('#lblSupplierCreditNoteNo').text('Supplier CreditNote# :' + supplierCreditNoteVM.CreditNoteNo);
    $('#CreditNoteNo').val(supplierCreditNoteVM.CreditNoteNo);
    $('#CreditNoteDateFormatted').val(supplierCreditNoteVM.CreditNoteDateFormatted);
    $('#SupplierID').val(supplierCreditNoteVM.SupplierID).select2();
    $('#SupplierID').prop('disabled', true);
    $('#CreditAmount').val(roundoff(supplierCreditNoteVM.CreditAmount));

    $('#GeneralNotes').val(supplierCreditNoteVM.GeneralNotes);


    $('#lblCreditAmount').text(roundoff(supplierCreditNoteVM.CreditAmount));
    $('#lblAvailableCredit').text(roundoff(supplierCreditNoteVM.AvailableCredit));
    $('#lblAdjustedAmount').text(roundoff(supplierCreditNoteVM.adjustedAmount));


    PaintImages(ID);//bind attachments written in custom js
}
function GetSupplierCreditNoteByID(ID) {
    try {
        var data = { "ID": ID };
        _jsonData = GetDataFromServer("SupplierCreditNote/GetSupplierCreditNote/", data);
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

//##6--Reset Button Click-----------------------------------------------------##6
function Reset() {
    BindSupplierCreditNoteByID();
}

//##7--DELETE Supplier CreditNote -----------------------------------------------------##16
function DeleteClick() {
    notyConfirm('Are you sure to delete?', 'DeleteSupplierCreditNote()');
}
function DeleteSupplierCreditNote() {
    try {
        debugger;
        var id = $('#ID').val();
        if (id != '' && id != null) {
            var data = { "id": id };
            _jsonData = GetDataFromServer("SupplierCreditNote/DeleteSupplierCreditNote/", data);
            if (_jsonData != '') {
                _jsonData = JSON.parse(_jsonData);
                _result = _jsonData.Result;
                _message = _jsonData.Message.Message;
            }
            if (_result == "OK") {
                notyAlert('success', _message);
                window.location.replace("NewSupplierCreditNote?code=ACC");
            }
            if (_result == "ERROR") {
                notyAlert('error', _message);
            }
            if (_result == "UNAUTH") {
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
