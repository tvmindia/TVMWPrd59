﻿//*****************************************************************************
//*****************************************************************************
//Author: Angel
//CreatedDate: 30-Apr-2018 
//LastModified: 30-Apr-2018
//FileName: NewOtherExpense.js
//Description: Client side coding for NewOtherExpense
//******************************************************************************
// ##1--Global Declaration
// ##2--Document Ready Function
// ##3--PaymentMode Changed Function
// ##4--Save Function
// ##5--Save Success Function
// ##6--Binding OtherExpense Functions
// ##7--Reversal setting Functions
// ##8--Delete OtherExpense Function        
// ##9--Approval Functions
//******************************************************************************
//##1--Global Declaration---------------------------------------------##1 
var _dataTable = {};
var _emptyGuid = "00000000-0000-0000-0000-000000000000";
//##2--Document Ready Function----------------------------------------##2
$(document).ready(function () {
    debugger;
    try {
        $("#ChartOfAccountCode").select2({
        });
        $("#AccountSubHead").select2({
            tags: true
        });
        $("#PaymentMode").select2({
        });
        $("#BankCode").select2({
        });
        $("#ChartOfAccountCode").select2({
        });
        if ($('#IsUpdate').val() == 'True') {
            BindOtherExpense()
        }
        else {
            $('#lblOtherExpenseEntryNo').text('Entry No. # : New');
        }

        _dataTable.RefSearchTable = $('#RefSearchTable').DataTable({
            dom: '<"pull-right"f>rt<"bottom"ip><"clear">',
            ordering: false,
            searching: true,
            "bInfo": false,
            paging: true,
            data: null,
            pageLength: 15,
            language: {
                search: "_INPUT_",
                searchPlaceholder: "Search"
            },
            columns: [
                 { "data": "EntryNo", "defaultContent": "<i>-</i>", "width": "15%" },
                 { "data": "Description", "defaultContent": "<i>-</i>", "width": "30%" },
                 { "data": "ExpenseDateFormatted", "defaultContent": "<i>-</i>", "width": "15%" },
                 { "data": "Amount", "defaultContent": "<i>-</i>", "width": "15%" },
                 { "data": "ReversableAmount", "defaultContent": "<i>-</i>", "width": "15%" },
                 { "data": null, "orderable": false, "defaultContent": '<a  href="#" class="actionLink" style="background:lightgreen;border-radius:1em;padding: 0.25em .75em; aria-hidden="true"  onclick="SelectRefNo(this)" ><i>Select</i></a>', "width": "10%" }
            ],
            columnDefs: [
                  { className: "text-left", "targets": [0, 1] }
                , { className: "text-right", "targets": [3,4] }
                , { className: "text-center", "targets": [2,5] }
                , { "targets": [2, 3, 4, 5], "bSortable": false }
                ]
           
        });

    }
    catch (e) {
        console.log(e.message);
    }
});
//##3--PaymentMode Changed Function----------------------------------------##3
function PaymentModeChanged() {
    debugger;
    if ($('#PaymentMode').val() == "ONLINE") {
        $('#BankCode').prop('disabled', false);
    }
    else {
        $("#BankCode").val('');
        $('#BankCode').prop('disabled', true);
    }
    if ($('#PaymentMode').val() == "CHEQUE") {
        $('#ChequeDateFormatted').prop('disabled', false);
        $('#ChequeClearDateFormatted').prop('disabled', false);
        $('#BankCode').prop('disabled', false);
    }
    else {
        $("#ChequeDateFormatted").val('');
        $('#ChequeDateFormatted').prop('disabled', true);
        $("#ChequeClearDateFormatted").val('');
        $('#ChequeClearDateFormatted').prop('disabled', true);
    }


}
//##4--Save Function-----------------------------------------------##4
function Save() {
    debugger;
    var $form = $('#OtherExpenseForm');
    if ($form.valid()) {
        $('#btnSave').trigger('click');
    }
    else {
        notyAlert('warning', "Please Fill Required Fields");
    }
} 
//##5--Save Success Function-------------------------------------------------##5.
function SaveSuccessOtherExpense(data, status) {
    debugger;
    var JsonResult = JSON.parse(data)
    switch (JsonResult.Status) {
        case "OK":
            $('#IsUpdate').val('True');
            $('#ID').val(JsonResult.Record.ID)
            notyAlert("success", JsonResult.Record.Message)
            BindOtherExpense();
            break;
        case "ERROR":
            notyAlert("danger", JsonResult.Message)
            break;
        default:
            notyAlert("danger", JsonResult.Message)
            break;
    }
}
//##6--BindOtherExpense Function---------------------------------------------------##6
function BindOtherExpense() {
    var ID = $('#ID').val();
    var otherExpenseVM = GetOtherExpense(ID);
    $('#ID').val(otherExpenseVM.ID);
    $('#DepositWithdrawalID').val(otherExpenseVM.DepositWithdrawalID);
    $('#EntryNo').val(otherExpenseVM.EntryNo);
    $('#ExpenseDateFormatted').val(otherExpenseVM.ExpenseDateFormatted);
    $('#ChartOfAccountCode').val(otherExpenseVM.AccountCode).select2();
    $('#AccountSubHead').val(otherExpenseVM.AccountSubHead).select2();
    $('#lblApprovalStatus').text(otherExpenseVM.ApprovalStatus);
    $('#lblOtherExpenseEntryNo').text('Entry No. # : ' + otherExpenseVM.EntryNo);
    $('#PaymentMode').val(otherExpenseVM.PaymentMode).select2();
    $('#BankCode').val(otherExpenseVM.BankCode).select2();
    $('#ChequeDateFormatted').val(otherExpenseVM.ChequeDateFormatted);
    $('#ChequeClearDateFormatted').val(otherExpenseVM.ChequeClearDateFormatted);
    $('#Amount').val(otherExpenseVM.Amount);
    $('#ExpneseRef').val(otherExpenseVM.ExpneseRef);
    $('#Description').val(otherExpenseVM.Description);
    $("#ReversalRef").val(otherExpenseVM.ReversalRef);
    if (otherExpenseVM.Amount < 0)
        $("#IsReverse").val('true');
    else
        $("#IsReverse").val('false');
    
    
    if (otherExpenseVM.ReversableAmount > 0)//Setting hidden field to limit Reversible amount
    {
        $("#hdnAmountReversal").val(otherExpenseVM.ReversableAmount);
        $("#ReFAmountMsg").show();
        $("#ReFAmountMsg").text('* Amount must be less than ' + otherExpenseVM.ReversableAmount);
    }
    else {
        GetMaximumReducibleAmount(otherExpenseVM.EntryNo);
    }
    if (otherExpenseVM.LatestApprovalStatus == 3 || otherExpenseVM.LatestApprovalStatus == 0) {
        ChangeButtonPatchView('OtherExpense', 'divbuttonPatchOtherExpense', 'Edit');
        EnableDisableFields(false)
    }
    else {
        ChangeButtonPatchView('OtherExpense', 'divbuttonPatchOtherExpense', 'Disable');
        EnableDisableFields(true)
    }
    IsReverseOnchange();
    PaymentModeChanged();
}
function GetOtherExpense(ID) {
    try {
        debugger;
        var data = { "id": ID };
        var jsonData = {};
        var result = "";
        var message = "";
        var otherExpenseVM = new Object();
        jsonData = GetDataFromServer("OtherExpense/GetOtherExpense/", data);
        if (jsonData != '') {
            jsonData = JSON.parse(jsonData);
            result = jsonData.Result;
            otherExpenseVM = jsonData.Records;
            message = jsonData.Message;
        }
        if (result == "OK") {
            return otherExpenseVM;
        }
        if (result == "ERROR") {
            alert(message);
        }
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}
//##7--Reversal setting Functions-----------------------------------------------##7
function IsReverseOnchange() {
    debugger;
    if ($("#IsReverse").val() == 'true') {
        $('#ReversalRefDiv').show();
        $("#hdnReducibleAmt").val('');
    }
    else {
        $('#ReversalRefDiv').hide();
        $('#ReversalRef').val('');
        $('#hdnAmountReversal').val('');
        $('#ReFAmountMsg').hide();
        $("#hdnReducibleAmt").val('');
    }
}
function SearchReference() {
    debugger;
    $("#RefSearchDiv").fadeIn();
    $("#ReFSearchMsg").hide();
    $("#hdnAmountReversal").val();
    $("#Amount").val('');
    try {
        debugger;
        var data = GetReversalReference();
        _dataTable.RefSearchTable.clear().rows.add(data).draw(false);
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}
function CancelSearch() {
    $("#RefSearchDiv").hide();
}
function SelectRefNo(currentObj) {
    debugger;
    var rowData = _dataTable.RefSearchTable.row($(currentObj).parents('tr')).data();
    $("#ReversalRef").val(rowData.EntryNo);
    $("#hdnAmountReversal").val(rowData.ReversableAmount);
    $("#ReFAmountMsg").show();
    $("#ReFAmountMsg").text('* Amount must be less than ' + rowData.ReversableAmount);
    $("#RefSearchDiv").hide();
}
function CheckReversableAmount() {
    debugger;
    if ($("#IsReverse").val()) {
        var reversableAmt = $("#hdnAmountReversal").val();
        var EnteredAmt = $("#Amount").val();
        if (parseInt(EnteredAmt) > parseInt(reversableAmt)) {
            $("#Amount").val('');
            $("#ReFAmountMsg").show();
            $("#ReFAmountMsg").text('* Amount must be less than ' + reversableAmt);
        }
        else {
            $("#ReFAmountMsg").hide();
            CheckReducibleAmount();
        }
    }
    else {
        CheckReducibleAmount();
    }
}
function CheckReducibleAmount() {
    try {
        debugger;
        var reducableAmt = $("#hdnReducibleAmt").val();
        var enteredAmt = $("#Amount").val();
        if (parseInt(enteredAmt) < parseInt(reducableAmt)) {
            $("#Amount").val('');
            $("#ReFAmountMsg").show();
            $("#ReFAmountMsg").text('* Amount cannot be less than ' + reducableAmt);
        }
        else {
            $("#ReFAmountMsg").hide();
        }
        if (parseInt(reducableAmt) === 0) {
            $("#ReFAmountMsg").hide();
        }
    }
    catch (ex) {
        console.log(ex.message);
    }
}
function ClearReversalRef() {
    $("#ReversalRef").val('');
    $("#hdnAmountReversal").val('');
    SearchReference();
}
function GetReversalReference()
{
    try {
        debugger;
        var ChartOfAccountCode = $('#ChartOfAccountCode').val();
        var data = { "ChartOfAccountCode": ChartOfAccountCode };
        if (ChartOfAccountCode == null)
            $("#ReFSearchMsg").show();
        var jsonData = {};
        var result = "";
        var message = "";
        var otherExpenseVM = new Object();
        jsonData = GetDataFromServer("OtherExpense/GetReversalReference/", data);
        if (jsonData != '') {
            jsonData = JSON.parse(jsonData);
            result = jsonData.Result;
            otherExpenseVM = jsonData.Records;
            message = jsonData.Message;
        }
        if (result == "OK") {
            return otherExpenseVM;
        }
        if (result == "ERROR") {
            alert(message);
        }
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}

function GetMaximumReducibleAmount(refNo) {
    try {
        debugger;
        var data = { "refNumber": refNo };
        var jsonData = {};
        var result = "";
        var message = "";
        var reducibleAmount = 0;
        jsonData = GetDataFromServer("OtherExpense/GetMaximumReducibleAmount/", data);
        if (jsonData != '') {
            jsonData = JSON.parse(jsonData);
            result = jsonData.Result;
            reducibleAmount = jsonData.Records;
            message = jsonData.Message;
        }
        if (result == "OK") {
            $("#hdnReducibleAmt").val('' + reducibleAmount);
            if (reducibleAmount > 0) {
                $("#ReFAmountMsg").show();
                $("#ReFAmountMsg").text('* Amount must be more than ' + reducibleAmount);
            }
        }
        if (result == "ERROR") {
            alert(message);
        }
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}
//#8--Delete OtherExpense Function------------------------------------------------##8
function DeleteClick() {
    notyConfirm('Are you sure to delete?', 'DeleteOtherExpense()');
}
function DeleteOtherExpense() {
    try {
        debugger;
        var id = $('#ID').val();
        if (id != '' && id != null) {
            var data = { "id": id };
            var jsonData = {};
            var result = "";
            var message = "";
            var otherExpenseVM = new Object();
            jsonData = GetDataFromServer("OtherExpense/DeleteOtherExpense/", data);
            if (jsonData != '') {
                jsonData = JSON.parse(jsonData);
                result = jsonData.Status;
                otherExpenseVM = jsonData.Record;
                message = jsonData.Message;
            }
            if (result == "OK") {
                notyAlert('success', otherExpenseVM.message);
                window.location.replace("NewOtherExpense?code=ACC");
            }
            if (result == "ERROR") {
                notyAlert('error', message);
                return 0;
            }
            return 1;
        }
    }
    catch (e) {
        notyAlert('error', e.message);
        return 0;
    }
}
//##9--Approval Functions-----------------------------------------------------------##9
function ShowSendForApproval(documentTypeCode) {
    debugger;
    if ($('#LatestApprovalStatus').val() == 3) {
        var documentID = $('#ID').val();
        var latestApprovalID = $('#LatestApprovalID').val();
        ReSendDocForApproval(documentID, documentTypeCode, latestApprovalID);
        BindOtherExpense();
    }
    else {
        $('#SendApprovalModal').modal('show');
    }
}

function SendForApproval(documentTypeCode) {
    debugger;
    var documentID = $('#ID').val();
    var approversCSV;
    var count = $('#ApproversCount').val();

    for (i = 0; i < count; i++) {
        if (i == 0)
            approversCSV = $('#ApproverLevel' + i).val();
        else
            approversCSV = approversCSV + ',' + $('#ApproverLevel' + i).val();
    }
    SendDocForApproval(documentID, documentTypeCode, approversCSV);
    $('#SendApprovalModal').modal('hide');
    BindOtherExpense();
}
function EnableDisableFields(value) {
    $('#ExpenseDateFormatted').attr("disabled", value);
    $('#ChartOfAccountCode').attr("disabled", value);
    $('#AccountSubHead').attr("disabled", value);
    $('#PaymentMode').attr("disabled", value);
    $('#BankCode').attr("disabled", value);
    $('#ChequeDateFormatted').attr("disabled", value);
    $('#ChequeClearDateFormatted').attr("disabled", value);
    $('#Amount').attr("disabled", value);
    $('#ExpneseRef').attr("disabled", value);
    $('#Description').attr("disabled", value);
    $('#ReversalRef').attr("disabled", value);
    $('#IsReverse').attr("disabled", value);
}
//##8-----OnChange of ChartOfAccountCode Dropdown---------------------------------------------// 

function AccountCodeOnChange() {

    try {
        debugger;
        if ($('#ChartOfAccountCode').val().split("|")[1] === "True") {

            $('#AccountSubHead').prop("disabled", false);

        } else {

            $('#AccountSubHead').prop("disabled", true);
        }

    } catch (ex) {

        console.log("error", ex.message);

    }

}