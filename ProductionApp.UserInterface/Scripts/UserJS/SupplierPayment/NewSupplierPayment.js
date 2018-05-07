//*****************************************************************************
//*****************************************************************************
//Author: Gibin Jacob
//CreatedDate: 18-Apr-2018 
//LastModified:  18-Apr-2018 
//FileName: ViewSupplierPayment.js
//Description: Client side coding for View Supplier Payment
//******************************************************************************
// ##1--Global Declaration
// ##2--Document Ready function
// ##3-- On Change: Payment Type,Payment Mode
// ##4-- On Change: Supplier,Bind Outstanding Invocies and Amount
// ##5-- Paid Amount Entry and filling Invocies
// ##6-- Save Supplier Payment
// ##7-- Bind Supplier Payment Header and Details
// ##8-- Bind CreditDropDown
// ##9-- Delete Supplier Payments
// ##10-- Send For Approval
// ##11- 
// 
//******************************************************************************

//##1--Global Declaration---------------------------------------------##1 
var _dataTable = {};
var _emptyGuid = "00000000-0000-0000-0000-000000000000";
var _SlNo = 1;
var _result = "";
var _message = "";
var _jsonData = {};

//##2--Document Ready function-----------------------------------------##2  
$(document).ready(function () {
    try {
        
        $('#btnUpload').click(function () {
            //Pass the controller name
            var FileObject = new Object;
            if ($('#hdnFileDupID').val() != _emptyGuid) {
                FileObject.ParentID = (($('#ID').val()) != _emptyGuid ? ($('#ID').val()) : $('#hdnFileDupID').val());
            }
            else {
                FileObject.ParentID = ($('#ID').val() == _emptyGuid) ? "" : $('#ID').val();
            }


            FileObject.ParentType = "SupplierPayment";
            FileObject.Controller = "FileUpload";
            UploadFile(FileObject);
        });

        //Supplier InvoiceTbl
        _dataTable.OutStandingInvoices = $('#tblOutStandingDetails').DataTable({
            dom: '<"pull-left"f>rt<"bottom"ip><"clear">',
            order: [],
            searching: true,
            "bInfo": false,
            paging: false,
            data: null,
            columns: [
                 { "data": "ID", "defaultContent": "<i>-</i>" },
                 { "data": "Checkbox", "defaultContent": "" },
                 {
                     "data": "Description", 'render': function (data, type, row) {
                         return 'Inv# :<b>' + row.InvoiceNo + '</b>  Date :<b>' + row.InvoiceDateFormatted + '</b><br/>'
                     }, "width": "30%"
                 },
                 { "data": "PaymentDueDateFormatted", "defaultContent": "<i>-</i>", "width": "10%" },
                 {
                     "data": "InvoiceAmount", "defaultContent": "<i>-</i>", "width": "15%",
                     'render': function (data, type, row) {
                         return roundoff(row.InvoiceAmount)
                     }
                 },
                 {
                     "data": "PaymentReceived", "defaultContent": "<i>-</i>", "width": "15%",
                     'render': function (data, type, row) {
                         return roundoff(row.PaymentReceived)
                     }
                 },
                 {
                     "data": "Balance", "defaultContent": "<i>-</i>", "width": "10%",
                     'render': function (data, type, row) {
                         return roundoff(row.Balance)
                     }
                 },
                 {
                     "data": "SupplierPayment.SupplierPaymentDetail.PaidAmount", 'render': function (data, type, row) {
                         return '<input class="form-control text-right paymentAmount Amount" name="Markup" value="' + roundoff(data) + '" onfocus="this.select();" onchange="PaymentAmountChanged(this);" onkeypress = "return isNumber(event)" type="text">';
                     }, "width": "15%"
                 },
                 {
                     "data": "SupplierPayment.SupplierPaymentDetail.ID", "defaultContent": "<i>-</i>", "width": "10%",
                     'render': function (data, type, row) {
                         return roundoff(row.BalanceDue)
                     }
                 }
            ],
            columnDefs: [{ orderable: false, className: 'select-checkbox', targets: 1 }
                , { className: "text-right", "targets": [4, 5, 6] },
                  { className: "text-left", "targets": [2] }
                , { "targets": [0, 8], "visible": false, "searchable": false }
                , { "targets": [2, 3, 4, 5, 6, 7], "bSortable": false }],

            select: { style: 'multi', selector: 'td:first-child' }
        });

        $("#SupplierID").change(function () {
            SupplierOnChange(this.value)
        });
        if ($('#IsUpdate').val() == 'True') {
            BindSupplierPayment()
        }
        else {
            $('#lblReturnSlipNo').text('Supplier Payment# : New');
        }
    }
    catch (e) {
        console.log(e.message);
    }
});


// ##3-- On Change: Payment Type,Payment Mode--------------------------------##3

//function TypeOnChange() {
//    if ($('#Type').val() == "C") {
//        $("#ddlCreditDiv").css("visibility", "visible");
//        $('#PaymentMode').val('');
//        $('#BankCode').val('');
//        $('#PaymentMode').prop('disabled', true);
//        $('#TotalRecdAmt').prop('disabled', true);
//        $('#BankCode').prop('disabled', true);
//        $('#ChequeDate').prop('disabled', true);
//        $('#CreditID').prop('disabled', false);
//      //  CaptionChangeCredit()
//    }
//    else {
//        $("#ddlCreditDiv").css("visibility", "hidden");
//        $('#PaymentMode').prop('disabled', false);
//        $('#BankCode').prop('disabled', true);
//        $('#TotalRecdAmt').val(0);
//        $('#TotalRecdAmt').prop('disabled', false);
//        $('#CreditID').val(_emptyGuid);
//       // CaptionChangePayment()
//        AmountChanged();
//    }
//}

function PaymentModeChanged() {
    if ($('#PaymentMode').val() == "ONLINE") {
        $('#BankCode').prop('disabled', false);
    }
    else {
        $("#BankCode").val('');
        $('#BankCode').prop('disabled', true);
    }
    if ($('#PaymentMode').val() == "CHEQUE") {
        $('#ChequeDateFormatted').prop('disabled', false);
        $('#ReferenceBank').prop('disabled', false);
    }
    else {
        $("#ChequeDateFormatted").val('');
        $('#ChequeDateFormatted').prop('disabled', true);
        $('#ReferenceBank').prop('disabled', true);
    }


}

// ##4-- Onchange: Supplier,Bind Outstanding Invocies and Amount--------------------------------------------------##4
function SupplierOnChange() {
    if ($('#SupplierID').val() != "") {
        //    BindCreditDropDown();
        BindOutstandingAmount();
        $('#TotalPaidAmt').val('');
        //    AmountChanged();
    } 
    BindOutstanding();
}
function BindOutstanding() {
    _dataTable.OutStandingInvoices.clear().rows.add(GetOutStandingInvoices()).draw(false);
}
function GetOutStandingInvoices() {
    try {
        var custId = $('#SupplierID').val() == "" ? _emptyGuid : $('#SupplierID').val();
        var paymentId = $('#ID').val() == "" ? _emptyGuid : $('#ID').val();
        var data = { "SupplierId": custId, "paymentId": paymentId };
        var outStandingInvoiceVM = new Object();
        _jsonData = GetDataFromServer("SupplierPayment/GetOutStandingSupplierInvoices/", data);
        if (_jsonData != '') {
            _jsonData = JSON.parse(_jsonData);
            _result = _jsonData.Result;
            _message = _jsonData.Message;
            outStandingInvoiceVM = _jsonData.Records;
        }
        if (_result == "OK") {
            return outStandingInvoiceVM;
        }
        if (_result == "ERROR") {
            alert(message);
        }
    }
    catch (e) {
        //this will show the error msg in the browser console(F12) 
        console.log(e.message);
    }
}

//Bind Outstanding Amount
function BindOutstandingAmount() {
    var thisitem = GetOutstandingAmountBySupplier();
    if (thisitem != null) {
        $('#invoicedAmt').text(thisitem.InvoiceAmount == null ? "₹ 0.00" : "₹" + roundoff(thisitem.InvoiceAmount));
    }
}
function GetOutstandingAmountBySupplier() {
    try {
        var Id = $('#SupplierID').val() == "" ? _emptyGuid : $('#SupplierID').val();
        var data = { "Id": Id };
        var outStandingAmountVM = new Object();
        _jsonData = GetDataFromServer("SupplierPayment/GetOutStandingAmount/", data);
        if (_jsonData != '') {
            _jsonData = JSON.parse(_jsonData);
            _result = _jsonData.Result;
            _message = _jsonData.Message;
            outStandingAmountVM = _jsonData.Records;
        }
        if (_result == "OK") {
            return outStandingAmountVM;
        }
        if (_result == "ERROR") {
            alert(message);
        }
    }
    catch (e) {
        //this will show the error msg in the browser console(F12) 
        console.log(e.message);
    }
}

// ##5-- Paid Amount Entry and filling Invocies----------------------------------------- ##5
function AmountChanged() {
    var sum = 0;
    _dataTable.OutStandingInvoices.rows().deselect();
    if ($('#TotalPaidAmt').val() < 0 || $('#TotalPaidAmt').val() == "") {
        $('#TotalPaidAmt').val(roundoff(0));
    }

    AmountReceived = parseFloat($('#TotalPaidAmt').val())
    if (!isNaN(AmountReceived)) {
        $('#TotalPaidAmt').val(roundoff(AmountReceived));
        $('#lblTotalRecdAmt').text(roundoff(AmountReceived));
        $('#paidAmt').text("₹" + roundoff(AmountReceived));

        var table = $('#tblOutStandingDetails').DataTable();
        var allData = table.rows().data();
        var RemainingAmount = AmountReceived;

        for (var i = 0; i < allData.length; i++) {
            var CustPaymentObj = new Object;
            var CustPaymentDetailObj = new Object;
            CustPaymentObj.CustPaymentDetailObj = CustPaymentDetailObj;

            if (RemainingAmount != 0) {
                if (parseFloat(allData[i].Balance) < RemainingAmount) {
                    allData[i].SupplierPayment.SupplierPaymentDetail.PaidAmount = parseFloat(allData[i].Balance)
                    RemainingAmount = RemainingAmount - parseFloat(allData[i].Balance);
                    sum = sum + parseFloat(allData[i].Balance);
                }
                else {
                    allData[i].SupplierPayment.SupplierPaymentDetail.PaidAmount = RemainingAmount
                    sum = sum + RemainingAmount;
                    RemainingAmount = 0
                }
            }
            else {
                allData[i].SupplierPayment.SupplierPaymentDetail.PaidAmount = 0;
            }
        }
        _dataTable.OutStandingInvoices.clear().rows.add(allData).draw(false);
        Selectcheckbox();
        $('#lblPaymentApplied').text(roundoff(sum));
        $('#lblCredit').text(roundoff(AmountReceived - sum));
    }
}
function PaymentAmountChanged(this_Obj) {
    if (this_Obj.value == "")
        this_Obj.value = roundoff(0);
    AmountReceived = parseFloat($('#TotalPaidAmt').val())
    sum = 0;
    var allData = _dataTable.OutStandingInvoices.rows().data();
    var table = _dataTable.OutStandingInvoices;
    var rowtable = table.row($(this_Obj).parents('tr')).data();

    for (var i = 0; i < allData.length; i++) {
        if (allData[i].ID == rowtable.ID) {

            var oldamount = parseFloat(allData[i].SupplierPayment.SupplierPaymentDetail.PaidAmount)
            var credit = parseFloat($("#lblCredit").text())
            if (credit > 0) {
                var currenttotal = AmountReceived + parseFloat(this_Obj.value) - (credit + oldamount)
            }
            else {
                var currenttotal = AmountReceived + parseFloat(this_Obj.value) - oldamount
            }

            if (parseFloat(allData[i].Balance) < parseFloat(this_Obj.value)) {
                if (currenttotal < AmountReceived) {
                    allData[i].SupplierPayment.SupplierPaymentDetail.PaidAmount = parseFloat(allData[i].Balance)
                    sum = sum + parseFloat(allData[i].Balance);
                }
                else {
                    allData[i].SupplierPayment.SupplierPaymentDetail.PaidAmount = oldamount
                    sum = sum + oldamount
                }
            }
            else {
                if (currenttotal > AmountReceived) {
                    allData[i].SupplierPayment.SupplierPaymentDetail.PaidAmount = oldamount
                    sum = sum + oldamount;
                }
                else {
                    allData[i].SupplierPayment.SupplierPaymentDetail.PaidAmount = this_Obj.value;
                    sum = sum + parseFloat(this_Obj.value);
                }
            }
        }
        else {
            sum = sum + parseFloat(allData[i].SupplierPayment.SupplierPaymentDetail.PaidAmount);
        }
    }
    _dataTable.OutStandingInvoices.clear().rows.add(allData).draw(false);

    $('#lblPaymentApplied').text(roundoff(sum));
    $('#lblCredit').text(roundoff(AmountReceived - sum));
    Selectcheckbox();
}
function Selectcheckbox() {
    var table = $('#tblOutStandingDetails').DataTable();
    var allData = table.rows().data();
    for (var i = 0; i < allData.length; i++) {
        if (allData[i].SupplierPayment.SupplierPaymentDetail.PaidAmount == "" || allData[i].SupplierPayment.SupplierPaymentDetail.PaidAmount == roundoff(0)) {
            _dataTable.OutStandingInvoices.rows(i).deselect();
        }
        else {
            _dataTable.OutStandingInvoices.rows(i).select();
        }
    }
}







// ##6-- Save Supplier Payment ----------------------------------------- ##6
function Save() {
    var $form = $('#SupplierPaymentForm');
    if ($form.valid()) {
        ValidatePaymentRefNo();
    }
    else {
        notyAlert('warning', "Please Fill Required Fields,To Add Items ");
    }
}
function ValidatePaymentRefNo() {
    try {
        var PaymentID = $('#ID').val();
        var paymentRefNo = $("#PaymentRef").val();
        var data = { "id": PaymentID, "paymentRefNo": paymentRefNo };
        _jsonData = GetDataFromServer("SupplierPayment/ValidateSupplierPayment/", data);
        if (_jsonData != '') {
            _jsonData = JSON.parse(_jsonData);
            _result = _jsonData.Result;
            _message = _jsonData.Message;
        }
        if (_result == "OK") {
            if (_jsonData.Record.Status == 1)
                notyConfirm(_jsonData.Record.Message, 'SaveValidatedData();', '', "Yes,Proceed!", 1);
            else
                SaveValidatedData();
        }
        if (_result == "ERROR") {
            alert(_message);
        }
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}
function SaveValidatedData() {
    $(".cancel").click();
    $("#DetailJSON").val('');
    _SupplierPaymentDetailList = [];
    AddSupplierPaymentDetailList();
    var result = JSON.stringify(_SupplierPaymentDetailList);
    $("#DetailJSON").val(result);
    $('#hdfCreditAmount').val($('#lblPaymentApplied').text());
    $('#AdvanceAmount').val($('#lblCredit').text());
    setTimeout(function () {
        $('#btnSave').trigger('click');
    }, 1000);
    _SlNo = 1;
}
function AddSupplierPaymentDetailList() {
    var PaymentInvoices = _dataTable.OutStandingInvoices.rows(".selected").data();
    for (var r = 0; r < PaymentInvoices.length; r++) {
        paymentDetail = new Object();
        paymentDetail.InvoiceID = PaymentInvoices[r].ID;
        paymentDetail.ID = PaymentInvoices[r].SupplierPayment.SupplierPaymentDetail.ID;
        paymentDetail.PaymentID = $('#ID').val() == "" ? _emptyGuid : $('#ID').val();
        paymentDetail.PaidAmount = PaymentInvoices[r].SupplierPayment.SupplierPaymentDetail.PaidAmount;
        _SupplierPaymentDetailList.push(paymentDetail);
    }
}
function SaveSuccessSupplierPayment(data, status) {
    _jsonData = JSON.parse(data)
    switch (_jsonData.Result) {
        case "OK":
            $('#IsUpdate').val('True');
            $('#ID').val(_jsonData.Records.ID)
            _message=_jsonData.Records.Message;
            notyAlert("success", _message);
            BindSupplierPayment();
            break;
        case "ERROR":
            notyAlert("danger", _jsonData.Message)
            break;
        default:
            notyAlert("danger", _jsonData.Message)
            break;
    }
}


// ##7-- Bind Supplier Payment Header and Details,reset ---------------------------------------------------------##7
function Reset() {
    BindSupplierPayment();
}

function BindSupplierPayment() {
    var PaymentID = $('#ID').val();
    var SupplierPaymentVM = GetSupplierPayments();
    $('#lblReturnSlipNo').text('Supplier Payment# : ' + SupplierPaymentVM.EntryNo);
    $('#EntryNo').val(SupplierPaymentVM.EntryNo);
    $('#ID').val(PaymentID);
    $('#deleteId').val(PaymentID);
    $("#SupplierID").select2();
    $("#SupplierID").val(SupplierPaymentVM.SupplierID).trigger('change');
    $('#SupplierID').prop('disabled', true);
    $('#ReferenceBank').val(SupplierPaymentVM.ReferenceBank);
    $('#PaymentDateFormatted').val(SupplierPaymentVM.PaymentDateFormatted);
    $('#ChequeDateFormatted').val(SupplierPaymentVM.ChequeDateFormatted);
    $('#PaymentRef').val(SupplierPaymentVM.PaymentRef);
    $('#PaymentMode').val(SupplierPaymentVM.PaymentMode);
    $('#BankCode').val(SupplierPaymentVM.BankCode).select2();
    $('#DepositWithdrawalID').val(SupplierPaymentVM.DepositWithdrawalID);
    $('#GeneralNotes').val(SupplierPaymentVM.GeneralNotes);
    $('#TotalPaidAmt').val(roundoff(SupplierPaymentVM.TotalPaidAmt));
    $('#lblTotalRecdAmt').text(roundoff(SupplierPaymentVM.TotalPaidAmt));
    $('#paidAmt').text("₹" + roundoff(SupplierPaymentVM.TotalPaidAmt));
    $('#Type').val(SupplierPaymentVM.Type);
    $('#hdfType').val(SupplierPaymentVM.Type);
    $('#lblApprovalStatus').text(SupplierPaymentVM.ApprovalStatus);
    $('#LatestApprovalStatus').val(SupplierPaymentVM.LatestApprovalStatus);
    $('#LatestApprovalID').val(SupplierPaymentVM.LatestApprovalID);
    $('#Type').prop('disabled', true);
    if (SupplierPaymentVM.AdvanceAmount == 0) {
        $('#advAmt').hide();
        $('#lblAdvAmt').hide();
    }
    else {
        $('#advAmt').show();
        $('#lblAdvAmt').show();
        $('#advAmt').text("₹" + roundoff(SupplierPaymentVM.AdvanceAmount));
    }
    BindOutstandingAmount();

    if ($('#Type').val() == 'C') {
        $("#CreditID").html("");
        //Get Available Credit and Add with  TotalRecdAmt
        var thisObj = GetCreditNoteByPaymentID(SupplierPaymentVM.SupplierObj.ID, PaymentID)
        if (thisObj.length > 0)
            var CreditAmount = parseFloat(SupplierPaymentVM.TotalRecdAmt) + parseFloat(thisObj[0].AvailableCredit);
        else
            var CreditAmount = parseFloat(SupplierPaymentVM.TotalRecdAmt);

        $('#TotalPaidAmt').val(roundoff(CreditAmount))
        $('#lblTotalRecdAmt').text(roundoff(CreditAmount))
        $('#paidAmt').text("₹" + roundoff(CreditAmount));
        $("#CreditID").append($('<option></option>').val(SupplierPaymentVM.CreditID).html(SupplierPaymentVM.CreditNo + ' ( Credit Amt: ₹' + CreditAmount + ')'));
        $('#CreditID').val(SupplierPaymentVM.CreditID)
        $('#CreditID').prop('disabled', true);
        $('#TotalPaidAmt').prop('disabled', true);
        $('#hdfCreditID').val(SupplierPaymentVM.CreditID);
        $('#PaymentMode').prop('disabled', true);
        $("#ddlCreditDiv").css("visibility", "visible");
        CaptionChangeCredit();
    }
    else {
        $("#CreditID").html(""); // clear before appending new list 
        $("#CreditID").append($('<option></option>').val(_emptyGuid).html('--Select Credit Note--'));
        $('#hdfCreditID').val(_emptyGuid);
        $('#PaymentMode').prop('disabled', false);
        CaptionChangePayment();
    }

    //BIND OUTSTANDING INVOICE TABLE USING Supplier ID AND PAYMENT HEADER 
    BindOutstanding();
    //edit outstanding table Payment text binding
    var table = $('#tblOutStandingDetails').DataTable();
    var allData = table.rows().data();
    var sum = 0;
    AmountReceived = roundoff($('#TotalPaidAmt').val())
    for (var i = 0; i < allData.length; i++) {
        sum = sum + parseFloat(allData[i].SupplierPayment.SupplierPaymentDetail.PaidAmount);
    }
    $('#lblCredit').text(roundoff(AmountReceived - sum));
    $('#lblPaymentApplied').text(roundoff(sum));
    Selectcheckbox();
    clearUploadControl();
    PaintImages(PaymentID);
    if (SupplierPaymentVM.LatestApprovalStatus == 3 || SupplierPaymentVM.LatestApprovalStatus == 0) {
        ChangeButtonPatchView('SupplierPayment', 'divbuttonPatchSupplierPayment', 'Edit');
        EnableDisableFields(false)
        $("#fileUploadControlDiv").show();
    }
    else {
        ChangeButtonPatchView('SupplierPayment', 'divbuttonPatchSupplierPayment', 'Disable');
        EnableDisableFields(true)
        $("#fileUploadControlDiv").hide();

    }
    PaymentModeChanged();
}
function EnableDisableFields(value) {
    $('#PaymentMode').attr("disabled", value);
    $('#TotalPaidAmt').attr("disabled", value);
    $('#PaymentDateFormatted').attr("disabled", value);
    $('#PaymentRef').attr("disabled", value);
    $('#ChequeDateFormatted').attr("disabled", value);
    $('#ReferenceBank').attr("disabled", value);
    $('#BankCode').attr("disabled", value);
    $('#GeneralNotes').attr("disabled", value);
}

function CaptionChangePayment() {
    $("#lblTotalRecdAmtCptn").text('Total Amount Recevied');
    $("#lblPaymentAppliedCptn").text('Payment Applied');
    $("#lblCreditCptn").text('Credit Received');
    $("#lblTotalAmtRecdCptn").text('Amount Received');
    $("#lblpaidAmt").text('Amount Received');
}
function GetSupplierPayments() {
    try {
        var paymentId = $('#ID').val() == "" ? _emptyGuid : $('#ID').val();
        var data = { "Id": paymentId };
        var _jsonData = {};
        var SupplierPaymentVM = new Object();

        _jsonData = GetDataFromServer("SupplierPayment/GetSupplierPayment/", data);
        if (_jsonData != '') {
            _jsonData = JSON.parse(_jsonData);
            _result = _jsonData.Result;
            _message = _jsonData.Message;
            SupplierPaymentVM = _jsonData.Record;
        }
        if (_result == "OK") {

            return SupplierPaymentVM;
        }
        if (_result == "ERROR") {
            alert(message);
        }
    }
    catch (e) {
        //this will show the error msg in the browser console(F12) 
        console.log(e.message);
    }
}

// CURRENTLY NOT USING, UNCOMMENT THESE LINE WHILE USING CREDIT NOTE
// ##8-- Bind CreditDropDown---------------------------------------------------------##8

//function BindCreditDropDown() {
//    var ID = $("#SupplierID").val() == "" ? null : $("#SupplierID").val();
//    if (ID != null) {
//        var ds = GetCreditNoteBySupplier(ID);
//        if (ds.length > 0) {
//            $("#CreditID").html(""); // clear before appending new list 
//            $("#CreditID").append($('<option></option>').val(_emptyGuid).html('--Select Credit Note--'));
//            $.each(ds, function (i, credit) {
//                $("#CreditID").append(
//                    $('<option></option>').val(credit.ID).html(credit.CreditNoteNo + ' ( Credit Amt: ₹' + credit.AvailableCredit + ')'));
//            });
//        }
//        else {
//            $("#CreditID").html("");
//            $("#CreditID").append($('<option></option>').val(_emptyGuid).html('No Credit Notes Available'));
//        }
//    }
//}
//function GetCreditNoteBySupplier(ID) {
//    try {
//        var data = { "Id": ID }; 
//        var SupplierCreditNoteVM = new Object();
//        _jsonData = GetDataFromServer("SupplierPayment/GetSupplierCreditNote/", data);
//        if (_jsonData != '') {
//            _jsonData = JSON.parse(_jsonData);
//            _result = _jsonData.Result;
//            _message = _jsonData.Message;
//            SupplierCreditNoteVM = _jsonData.Record;
//        }
//        if (_result == "OK") {
//            return SupplierCreditNoteVM;
//        }
//        if (_result == "ERROR") {
//            notyAlert('error', _message);
//        }
//    }
//    catch (e) {
//        //this will show the error msg in the browser console(F12) 
//        console.log(e.message);
//    }
//}

// ##9-- Delete Supplier Payments---------------------------------------------------##9
function DeleteClick() {
    notyConfirm('Are you sure to delete?', 'DeleteSupplierPayment()');
}
function DeleteSupplierPayment() {
    try {
        var id = $('#ID').val();
        if (id != '' && id != null) {
            var data = { "id": id }; 
            var SupplierPaymentVM = new Object();
            _jsonData = GetDataFromServer("SupplierPayment/DeleteSupplierPayment/", data);
            if (_jsonData != '') {
                _jsonData = JSON.parse(_jsonData);
                _result = _jsonData.Result;
                SupplierPaymentVM = _jsonData.Record;
                _message = _jsonData.Message;
            }
            if (_result == "OK") {
                notyAlert('success', SupplierPaymentVM.message);
                window.location.replace("NewSupplierPayment?code=ACC");
            }
            if (_result == "ERROR") {
                notyAlert('error', _message);
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

// ##10-- Send For Approval---------------------------------------------------##10
function ShowSendForApproval(documentTypeCode) {
    
    if ($('#LatestApprovalStatus').val() == 3) {
        var documentID = $('#ID').val();
        var latestApprovalID = $('#LatestApprovalID').val();
        ReSendDocForApproval(documentID, documentTypeCode, latestApprovalID);
    }
    else {
        $('#SendApprovalModal').modal('show');
    }
}
function SendForApproval(documentTypeCode) {
    
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
    BindRequisitionByID();
}
