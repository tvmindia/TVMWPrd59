//*****************************************************************************
//*****************************************************************************
//Author: Gibin Jacob
//CreatedDate: 27-AUG-2018 
//LastModified:  
//FileName: NewCustomerCreditNote.js
//Description: Client side coding for New/Edit Customer CreditNote
//******************************************************************************
// ##1--Global Declaration
// ##2--Document Ready function
// ##3--Save  Customer CreditNote 
// ##4--Save Success Customer CreditNote
// ##5--Bind Customer CreditNote By ID
// ##6--Reset Button Click
// ##7--DELETE Customer CreditNote
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
        $("#CustomerID").select2({}); 

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
            FileObject.ParentType = "CustomerCreditNote";
            FileObject.Controller = "FileUpload";
            UploadFile(FileObject);
        });
 
        
        $("#CustomerID").change(function () {
           
        });
       

        if ($('#IsUpdate').val() == 'True') {
            BindCustomerCreditNoteByID()
        }
        else {
            $('#lblCustomerCreditNoteNo').text('Credit Note# : New');
        }
    }
    catch (e) {
        console.log(e.message);
    }
});
 

//##3--Save  Customer CreditNote----------------------------##3
function Save() {
    debugger;
    _SlNo = 1;
    $('#btnSave').trigger('click');
}

//##4--Save Success Customer CreditNote----------------------------##4
function SaveSuccessCustomerCreditNote(data, status) {
    _jsonData = JSON.parse(data)
    switch (_jsonData.Result) {
        case "OK":
            notyAlert("success", _jsonData.Records.Message)
            $('#IsUpdate').val('True');
            $('#ID').val(_jsonData.Records.ID) 
            BindCustomerCreditNoteByID();
            break;
        case "ERROR":
            notyAlert("danger", _jsonData.Message)
            break;
        default:
            notyAlert("danger", _jsonData.Message)
            break;
    }
}

//##5--Bind Customer CreditNote By ID----------------------------##5
function BindCustomerCreditNoteByID() {
    debugger;
    ChangeButtonPatchView('CustomerCreditNote', 'divbuttonPatchAddCustomerCreditNote', 'Edit');
    var ID = $('#ID').val();
    _SlNo = 1;
    var customerCreditNoteVM = GetCustomerCreditNoteByID(ID);
    $('#lblCustomerCreditNoteNo').text('Customer CreditNote# :' + customerCreditNoteVM.CreditNoteNo);
    $('#CreditNoteNo').val(customerCreditNoteVM.CreditNoteNo);
    $('#CreditNoteDateFormatted').val(customerCreditNoteVM.CreditNoteDateFormatted);
    $('#CustomerID').val(customerCreditNoteVM.CustomerID).select2();
    $('#CustomerID').prop('disabled', true);
    $('#CreditAmount').val(roundoff(customerCreditNoteVM.CreditAmount));

    $('#GeneralNotes').val(customerCreditNoteVM.GeneralNotes);
   

    $('#lblCreditAmount').text(roundoff(customerCreditNoteVM.CreditAmount));
    $('#lblAvailableCredit').text(roundoff(customerCreditNoteVM.CreditAmount -customerCreditNoteVM.adjustedAmount));
    $('#lblAdjustedAmount').text(roundoff(customerCreditNoteVM.adjustedAmount));
   
  
    PaintImages(ID);//bind attachments written in custom js
}
function GetCustomerCreditNoteByID(ID) {
    try {
        var data = { "ID": ID };
        _jsonData = GetDataFromServer("CustomerCreditNote/GetCustomerCreditNote/", data);
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
    BindCustomerCreditNoteByID();
}

//##7--DELETE Customer CreditNote -----------------------------------------------------##16
function DeleteClick() {
    notyConfirm('Are you sure to delete?', 'DeleteCustomerCreditNote()');
}
function DeleteCustomerCreditNote() {
    try {
        debugger;
        var id = $('#ID').val();
        if (id != '' && id != null) {
            var data = { "id": id };
            _jsonData = GetDataFromServer("CustomerCreditNote/DeleteCustomerCreditNote/", data);
            if (_jsonData != '') {
                _jsonData = JSON.parse(_jsonData);
                _result = _jsonData.Result;
                _message = _jsonData.Message;
            }
            if (_result == "OK") {
                notyAlert('success', _jsonData.Message.Message);
                window.location.replace("NewCustomerCreditNote?code=ACC");
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
