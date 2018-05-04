//*****************************************************************************
//*****************************************************************************
//Author: Arul
//CreatedDate: 30-Apr-2018 
//LastModified: 1-May-2018
//FileName: NewOtherIncome.js
//Description: Client side coding for Adding / Updating Other Income
//******************************************************************************
//******************************************************************************

/*

*/

//var DataTables = {};
var _emptyGuid = "00000000-0000-0000-0000-000000000000";
var _otherIncome = {};

$(document).ready(function () {
    try {
        debugger;
        $('#AccountSubHead').select2({
            tags: true,
            width: "100%"
        });

        PaymentModeOnChanged();
        try {
            $('#ChartOfAccountCode,#PaymentMode').select2({
                width: "100%"
            });
        } catch (ex) {
            console.log(ex);
        }

        try {
            $('#BankCode').select2({
                width: "100%"
            });
        } catch (ex) {
            console.log(ex);
        }

        $('#ChartOfAccountCode').change(function () {
            AccountCodeOnChange();
        });

        if ($('#IsUpdate').val()==='True') {
            BindOtherIncome();//GetOtherIncome
        }

    } catch (ex) {
        console.log(ex);
    }
});

//On Change of PaymentMode 
function PaymentModeOnChanged(curObj) {
    try{
        debugger;
        $('#msgPaymentRef').hide();
        $('#divBankDropdown .input-group-addon').each(function () {
            $(this).parent().css("width", "100%");
            $(this).parent().children().each(function () { $(this).prop("disabled", true); });
            $(this).hide();
        });
        $('#ReferenceBank').prop("disabled", true);
        $('#ChequeDate').prop("disabled", true);
        if (curObj !== undefined) {
            switch (curObj.value) {
                case "CHEQUE":
                    $('#ReferenceBank').prop("disabled", false);
                    $('#ChequeDate').prop("disabled", false);
                    $('#BankCode').val("").trigger('change');
                    break;
                case "ONLINE":
                    $('#divBankDropdown .input-group-addon').each(function () {
                        $(this).parent().children().each(function () { $(this).prop("disabled", false); });
                        $(this).show();
                    });
                    $('#ReferenceBank').val("");
                    $('#ChequeDate').val("");
                    break;
                default:
                    $('#BankCode').val("").trigger('change');
                    $('#ReferenceBank').val("");
                    $('#ChequeDate').val("");
                    break;
            }
        }
        
    } catch (ex) {
        console.log(ex);
    }
}

//Save button click
function Save() {
    try {
        debugger;
        var isInput = false;

        if ($('#hdnChartOfAccountCode').val() !== "") {
            isInput = true;
        }
        else {
            $('#msgAccountCode').show();
        }

        if ($('#PaymentMode').val() === "ONLINE") {
            if ($('#PaymentRef').val() !== "" && $('#PaymentRef').val() !== null) {
                isInput = true;
            }
            else {
                isInput = false;
                $('#msgPaymentRef').show();
                $('#PaymentRef').change(function () { $('#msgPaymentRef').hide(); });
            }
        }

        if ($('#Amount').val()<=0) {
            $('#Amount').val("");
        }

        if (isInput) {
            $('#hdnChartOfAccountCode').val($('#ChartOfAccountCode').val().split('|')[0]);
            $('#btnSave').click();
        }
    } catch (ex) {
        console.log(ex);
    }
}

//SaveSuccess
function SaveSuccess(data, status) {
    try {
        debugger;
        var JsonResult = JSON.parse(data)
        var result = JsonResult.Result;
        var message = JsonResult.Message;
        var otherIncomeVM = new Object();
        otherIncomeVM = JsonResult.Records;
        switch (result) {
            case "OK":
                $('#IsUpdate').val('True');
                $('#ID').val(otherIncomeVM.ID)
                $('#DepositWithdrawalID').val(otherIncomeVM.DepositWithdrawalID)
                $('#EntryNo').val(otherIncomeVM.EntryNo)
                message = otherIncomeVM.Message;
                notyAlert("success", message);
                ChangeButtonPatchView('OtherIncome', 'divButtonPatch', 'Edit');
                //BindMaterialReceiptDetailTable($('#ID').val());
                //PurchaseOrderOnChange()

                break;
            case "ERROR":
                notyAlert("error", message);
                break;
            default:
                notyAlert("error", message);
                break;
        }
    } catch (ex) {
        notyAlert("error", ex.message);
    }
}

//Bind Fields of the OtherIncome
function BindOtherIncome() {
    try{
        debugger;
        _otherIncome = new Object();
        _otherIncome = GetOtherIncome($('#ID').val());
        $('#EntryNo').val(_otherIncome.EntryNo);
        $('#IncomeDate').val(_otherIncome.IncomeDateFormatted);
        $('#hdnChartOfAccountCode').val(_otherIncome.AccountCode);
        $('#ChartOfAccountCode').val(_otherIncome.AccountCode).trigger('change');
        $('#AccountSubHead').val(_otherIncome.AccountSubHead).trigger('change');
        $('#PaymentMode').val(_otherIncome.PaymentMode).trigger('change');
        $('#BankCode').val(_otherIncome.BankCode).trigger('change');
        $('#ChequeDate').val(_otherIncome.ChequeDateFormatted);
        $('#ReferenceBank').val(_otherIncome.ReferenceBank);
        $('#PaymentRef').val(_otherIncome.PaymentRef);
        debugger;
        $('#Amount').val(_otherIncome.Amount);
        $('#Description').val(_otherIncome.Description);
        $('#DepositWithdrawalID').val(_otherIncome.DepositWithdrawalID)
        $('#lblRefNo').text(" Entry No#: "+_otherIncome.EntryNo);
        ChangeButtonPatchView('OtherIncome', 'divButtonPatch', 'Edit');
    } catch (ex) {
        notyAlert("error", ex.message);
    }
}

//Get OtherIncome By ID
function GetOtherIncome(id) {
    try{
        debugger;
        var data = { "id": id };
        var OtherIncomeViewModel = new Object();
        var result = "";
        var message = "";
        var jsonData = GetDataFromServer("OtherIncome/GetOtherIncome/", data);
        if (jsonData != '') {
            jsonData = JSON.parse(jsonData);
            result = jsonData.Result;
            message = jsonData.Message;
            OtherIncomeViewModel = jsonData.Records;
        }
        if (result == "OK") {
            return OtherIncomeViewModel;
        }
        if (result == "ERROR") {
            notyAlert('error', message);
        }
    } catch (ex) {
        console.log("error", ex.message);
    }
}

//Delete OtherIncome by ID
function DeleteOtherIncome() {
    try {
        debugger;
        var id = $('#ID').val();
        var data = { "id": id };
        var OtherIncomeViewModel = new Object();
        var result = "";
        var message = "";
        var jsonData = GetDataFromServer("OtherIncome/DeleteOtherIncome/", data);
        if (jsonData != '') {
            jsonData = JSON.parse(jsonData);
            result = jsonData.Result;
            message = jsonData.Message;
            OtherIncomeViewModel = jsonData.Records;
        }
        if (result == "OK") {
            ClearFields();
            notyAlert('success', message);
        }
        if (result == "ERROR") {
            notyAlert('error', message);
        }
    } catch (ex) {
        console.log("error", ex.message);
    }
}

//To clear all entry fields, hidden values
function ClearFields() {
    try {
        debugger;
        $('#ID').val(_emptyGuid);
        $('#IsUpdate').val('False');
        $('#EntryNo').val("");
        $('#IncomeDate').val("");
        $('#hdnChartOfAccountCode').val("");
        $('#ChartOfAccountCode').val("").trigger('change');
        $('#AccountSubHead').val("").trigger('change');
        $('#PaymentMode').val("").trigger('change');
        $('#PaymentRef').val("").trigger('change');
        $('#BankCode').val("").trigger('change');
        $('#lblRefNo').text(" Entry No#: New");
        $('#ReferenceBank').val("");
        $('#ChequeDate').val("");
        $('#Amount').val("");
        $('#Description').val("");
        $('#DepositWithdrawalID').val(_emptyGuid);
        ChangeButtonPatchView('OtherIncome', 'divButtonPatch', 'Add');
        $('#msgAccountCode').hide();
        $('#msgPaymentRef').hide();
    } catch (ex) {
        console.log("error", ex.message);
    }
}

//Reset on insert Clear Fields on update Bind OtherIncome
function Reset() {
    try{
        debugger;
        if ($('#IsUpdate').val() === 'True') {
            BindOtherIncome();//GetOtherIncome
        }
        else {
            ClearFields()
        }
    } catch (ex) {
        console.log("error", ex.message);
    }
}

//OnChange of ChartOfAccountCode Dropdown
function AccountCodeOnChange() {
    try {
        debugger;
        $('#msgAccountCode').hide();
        if ($('#ChartOfAccountCode').val().split("|")[1] === "True") {
            $('#AccountSubHead').prop("disabled", false);
        } else {
            $('#AccountSubHead').prop("disabled", true);
        }
    } catch (ex) {
        console.log("error", ex.message);
    }
}