//*****************************************************************************
//*****************************************************************************
//Author: Angel
//CreatedDate: 26-Mar-2018 
//LastModified: 26-Mar-2018 
//FileName: NewCustomerPayment.js
//Description: Client side coding for Add CustomerPayment.
//******************************************************************************
//******************************************************************************

//--Global Declaration--//
var DataTables = {};
var EmptyGuid = "00000000-0000-0000-0000-000000000000";
var _CustomerPaymentDetailList= [];
$(document).ready(function () {
    debugger;
    try {
        $("#PaymentMode").select2({
        });
        $("#CustomerID").select2({
        });
        $("#Type").select2({
        });

        //Customer InvoiceTbl
        DataTables.OutStandingInvoices = $('#tblOutStandingDetails').DataTable({
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
                     "data": "CustomerPayment.CustomerPaymentDetail.PaidAmount", 'render': function (data, type, row) {
                         index = index + 1
                         return '<input class="form-control text-right paymentAmount" name="Markup" value="' + roundoff(data) + '" onfocus="paymentAmountFocus(this);" onchange="PaymentAmountChanged(this);" id="PaymentValue_' + index + '" type="text">';
                     }, "width": "15%"
                 },
                 {
                     "data": "CustomerPayment.CustomerPaymentDetail.ID", "defaultContent": "<i>-</i>", "width": "10%",
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

        $("#CustomerID").change(function () {
            CustomerChange(this.value)
        });
        if ($('#IsUpdate').val() == 'True') {
            debugger;
            BindCustomerPayment()
            ChangeButtonPatchView('CustomerPayment', 'divbuttonPatchCustomerPayment', 'Edit');
        }
        else {
            $('#lblReturnSlipNo').text('Customer Payment# : New');
        }
    }
    catch (e) {
        console.log(e.message);
    }
});
function TypeOnChange() {
    if ($('#Type').val() == "C") {
        $("#ddlCreditDiv").css("visibility", "visible");
        $('#PaymentMode').val('');
        $('#BankCode').val('');
        $('#PaymentMode').prop('disabled', true);
        $('#TotalRecdAmt').prop('disabled', true);
        $('#BankCode').prop('disabled', true);
        $('#ChequeDate').prop('disabled', true);
        $('#CreditID').prop('disabled', false);
        CaptionChangeCredit()
    }
    else {
        $("#ddlCreditDiv").css("visibility", "hidden");
        $('#PaymentMode').prop('disabled', false);
        $('#BankCode').prop('disabled', true);
        $('#TotalRecdAmt').val(0);
        $('#TotalRecdAmt').prop('disabled', false);
        $('#CreditID').val(emptyGUID);
        CaptionChangePayment()
        AmountChanged();
    }
}
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
function CustomerChange() {
    debugger;
    if ($('#CustomerID').val() != "") {
    //    BindCreditDropDown();
        BindOutstandingAmount();
        $('#TotalRecievedAmt').val('');
    //    AmountChanged();
    }
    BindOutstanding();
}
function BindOutstanding() {
    index = 0;
    DataTables.OutStandingInvoices.clear().rows.add(GetOutStandingInvoices()).draw(false);
}
function GetOutStandingInvoices() {
    try {
        debugger;
        var custId = $('#CustomerID').val() == "" ? EmptyGuid : $('#CustomerID').val();
        var paymentId = $('#ID').val() == "" ? EmptyGuid : $('#ID').val();
        var data = { "CustomerId": custId, "paymentId": paymentId };
        var jsonData = {};
        var result = "";
        var message = "";
        var outStandingInvoiceVM = new Object();

        jsonData = GetDataFromServer("CustomerPayment/GetOutStandingInvoices/", data);
        if (jsonData != '') {
            jsonData = JSON.parse(jsonData);
            result = jsonData.Result;
            message = jsonData.Message;
            outStandingInvoiceVM = jsonData.Records;
        }
        if (result == "OK") {

            return outStandingInvoiceVM;
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
function paymentAmountFocus(event) {
    event.select();
}
function AmountChanged() {
    debugger;
    var sum = 0;
    DataTables.OutStandingInvoices.rows().deselect();
    if ($('#TotalRecievedAmt').val() < 0 || $('#TotalRecievedAmt').val() == "") {
        $('#TotalRecievedAmt').val(0);
    }

    AmountReceived = parseFloat($('#TotalRecievedAmt').val())
    if (!isNaN(AmountReceived)) {
        $('#TotalRecievedAmt').val(roundoff(AmountReceived));
        $('#lblTotalRecdAmt').text(roundoff(AmountReceived));
        $('#paidAmt').text(roundoff(AmountReceived));

        var table = $('#tblOutStandingDetails').DataTable();
        var allData = table.rows().data();
        var RemainingAmount = AmountReceived;

        for (var i = 0; i < allData.length; i++) {
            var CustPaymentObj = new Object;
            var CustPaymentDetailObj = new Object;
            CustPaymentObj.CustPaymentDetailObj = CustPaymentDetailObj;

            if (RemainingAmount != 0) {
                if (parseFloat(allData[i].Balance) < RemainingAmount) {
                    allData[i].CustomerPayment.CustomerPaymentDetail.PaidAmount = parseFloat(allData[i].Balance)
                    RemainingAmount = RemainingAmount - parseFloat(allData[i].Balance);
                    sum = sum + parseFloat(allData[i].Balance);
                }
                else {
                    allData[i].CustomerPayment.CustomerPaymentDetail.PaidAmount = RemainingAmount
                    sum = sum + RemainingAmount;
                    RemainingAmount = 0
                }
            }
            else {
                allData[i].CustomerPayment.CustomerPaymentDetail.PaidAmount = 0;
            }
        }
        DataTables.OutStandingInvoices.clear().rows.add(allData).draw(false);
        Selectcheckbox();
        $('#lblPaymentApplied').text(roundoff(sum));
        $('#lblCredit').text(roundoff(AmountReceived - sum));
    }
}

function PaymentAmountChanged(this_Obj) {
    debugger;
    AmountReceived = parseFloat($('#TotalRecievedAmt').val())
    sum = 0;
    var allData = DataTables.OutStandingInvoices.rows().data();
    var table = DataTables.OutStandingInvoices;
    var rowtable = table.row($(this_Obj).parents('tr')).data();

    for (var i = 0; i < allData.length; i++) {
        if (allData[i].ID == rowtable.ID) {

            var oldamount = parseFloat(allData[i].CustomerPayment.CustomerPaymentDetail.PaidAmount)
            var credit = parseFloat($("#lblCredit").text())
            if (credit > 0) {
                var currenttotal = AmountReceived + parseFloat(this_Obj.value) - (credit + oldamount)
            }
            else {
                var currenttotal = AmountReceived + parseFloat(this_Obj.value) - oldamount
            }

            if (parseFloat(allData[i].BalanceDue) < parseFloat(this_Obj.value)) {
                if (currenttotal < AmountReceived) {
                    allData[i].CustomerPayment.CustomerPaymentDetail.PaidAmount = parseFloat(allData[i].Balance)
                    sum = sum + parseFloat(allData[i].BalanceDue);
                }
                else {
                    allData[i].CustomerPayment.CustomerPaymentDetail.PaidAmount = oldamount
                    sum = sum + oldamount
                }
            }
            else {
                if (currenttotal > AmountReceived) {
                    allData[i].CustomerPayment.CustomerPaymentDetail.PaidAmount = oldamount
                    sum = sum + oldamount;
                }
                else {
                    allData[i].CustomerPayment.CustomerPaymentDetail.PaidAmount = this_Obj.value;
                    sum = sum + parseFloat(this_Obj.value);
                }
            }
        }
        else {
            sum = sum + parseFloat(allData[i].CustomerPayment.CustomerPaymentDetail.PaidAmount);
        }
    }
    DataTables.OutStandingInvoices.clear().rows.add(allData).draw(false);

    $('#lblPaymentApplied').text(roundoff(sum));
    $('#lblCredit').text(roundoff(AmountReceived - sum));
    Selectcheckbox();
}
function Selectcheckbox() {
    debugger;
    var table = $('#tblOutStandingDetails').DataTable();
    var allData = table.rows().data();
    for (var i = 0; i < allData.length; i++) {
        if (allData[i].CustomerPayment.CustomerPaymentDetail.PaidAmount == "" || allData[i].CustomerPayment.CustomerPaymentDetail.PaidAmount == roundoff(0)) {
            DataTables.OutStandingInvoices.rows(i).deselect();
        }
        else {
            DataTables.OutStandingInvoices.rows(i).select();
        }
    }
}
//Bind Outstanding Amount
function BindOutstandingAmount() {
    var thisitem = GetOutstandingAmountByCustomer();
    if (thisitem != null) {
        $('#invoicedAmt').text(thisitem.InvoiceAmount == null ? "₹ 0.00" : thisitem.InvoiceAmount);
    }
}
function GetOutstandingAmountByCustomer() {
    try {
        debugger;
        var custId = $('#CustomerID').val() == "" ? EmptyGuid : $('#CustomerID').val();
        var data = { "Id": custId };
        var jsonData = {};
        var result = "";
        var message = "";
        var outStandingAmountVM = new Object();

        jsonData = GetDataFromServer("CustomerPayment/GetOutStandingAmount/", data);
        if (jsonData != '') {
            jsonData = JSON.parse(jsonData);
            result = jsonData.Result;
            message = jsonData.Message;
            outStandingAmountVM = jsonData.Records;
        }
        if (result == "OK") {

            return outStandingAmountVM;
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
// Save CustomerPayment
function Save() {
    debugger;
    $("#DetailJSON").val('');
    _CustomerPaymentDetailList = [];
    AddCustomerPaymentDetailList();
    if (_CustomerPaymentDetailList.length > 0) {
        debugger;
        var result = JSON.stringify(_CustomerPaymentDetailList);
        $("#DetailJSON").val(result);
        $('#btnSave').trigger('click');
        _SlNo = 1;
    }
    else {
        notyAlert('warning', 'Please Add Payment Details!');
    }
}
function AddCustomerPaymentDetailList() {
    debugger;
    var PaymentInvoices = DataTables.OutStandingInvoices.rows(".selected").data();
        for (var r = 0; r < PaymentInvoices.length; r++) {
            paymentDetail = new Object();
            paymentDetail.InvoiceID = PaymentInvoices[r].ID;
            paymentDetail.ID = PaymentInvoices[r].CustomerPayment.CustomerPaymentDetail.ID;
            paymentDetail.PaymentID = $('#ID').val() == "" ? EmptyGuid : $('#ID').val();
            paymentDetail.PaidAmount = PaymentInvoices[r].CustomerPayment.CustomerPaymentDetail.PaidAmount;
            _CustomerPaymentDetailList.push(paymentDetail);
        }
}
function SaveSuccessCustomerPayment(data, status) {
    debugger;
    var JsonResult = JSON.parse(data)
    switch (JsonResult.Result) {
        case "OK":
            $('#IsUpdate').val('True');
            $('#ID').val(JsonResult.Records.ID)
            BindCustomerPayment();
            _SlNo = 1;
            notyAlert("success", JsonResult.Records.Message)
            ChangeButtonPatchView('CustomerPayment', 'divbuttonPatchCustomerPayment', 'Edit');
            break;
        case "ERROR":
            notyAlert("danger", JsonResult.Message)
            break;
        default:
            notyAlert("danger", JsonResult.Message)
            break;
    }
}
//Bind Details
function BindCustomerPayment() {
    var PaymentID = $('#ID').val();
    ChangeButtonPatchView('CustomerPayment', 'divbuttonPatchCustomerPayment', 'Edit');
    var thisitem = GetCustomerPayments();
    $('#lblReturnSlipNo').text('Customer Payment# : ' + thisitem.EntryNo); 
    $('#EntryNo').val(thisitem.EntryNo);
    $('#ID').val(PaymentID);
    $('#deleteId').val(PaymentID);
    $("#CustomerID").select2();
    $("#CustomerID").val(thisitem.CustomerID).trigger('change');
    $('#CustomerID').prop('disabled', true);
    $('#ReferenceBank').val(thisitem.ReferenceBank);
    $('#PaymentDateFormatted').val(thisitem.PaymentDateFormatted);
    $('#ChequeDate').val(thisitem.ChequeDateFormatted);
    $('#PaymentRef').val(thisitem.PaymentRef);
    $('#PaymentMode').val(thisitem.PaymentMode).select2();
    $('#BankCode').val(thisitem.BankCode);
    $('#DepositWithdrawalID').val(thisitem.DepositWithdrawalID);
    $('#GeneralNotes').val(thisitem.GeneralNotes);
    $('#TotalRecievedAmt').val(roundoff(thisitem.TotalRecievedAmt));
    $('#lblTotalRecdAmt').text(roundoff(thisitem.TotalRecievedAmt));
    $('#paidAmt').text(roundoff(thisitem.TotalRecievedAmt));
    $('#Type').val(thisitem.Type);
    $('#hdfType').val(thisitem.Type);
    $('#Type').prop('disabled', true);
    BindOutstandingAmount();

    if ($('#Type').val() == 'C') {
        $("#CreditID").html("");
        //Get Available Credit and Add with  TotalRecdAmt
        debugger;
        var thisObj = GetCreditNoteByPaymentID(thisitem.customerObj.ID, PaymentID)
        if (thisObj.length > 0)
            var CreditAmount = parseFloat(thisitem.TotalRecdAmt) + parseFloat(thisObj[0].AvailableCredit);
        else
            var CreditAmount = parseFloat(thisitem.TotalRecdAmt);

        $('#TotalRecievedAmt').val(roundoff(CreditAmount))
        $('#lblTotalRecdAmt').text(roundoff(CreditAmount))
        $('#paidAmt').text(roundoff(CreditAmount));
        $("#CreditID").append($('<option></option>').val(thisitem.CreditID).html(thisitem.CreditNo + ' ( Credit Amt: ₹' + CreditAmount + ')'));
        $('#CreditID').val(thisitem.CreditID)
        $('#CreditID').prop('disabled', true);
        $('#TotalRecievedAmt').prop('disabled', true);
        $('#hdfCreditID').val(thisitem.CreditID);
        $('#PaymentMode').prop('disabled', true);
        $("#ddlCreditDiv").css("visibility", "visible");
        CaptionChangeCredit();
    }
    else {
        $("#CreditID").html(""); // clear before appending new list 
        $("#CreditID").append($('<option></option>').val(EmptyGuid).html('--Select Credit Note--'));
        $('#hdfCreditID').val(EmptyGuid);
        $('#PaymentMode').prop('disabled', false);
        CaptionChangePayment();
    }

    PaymentModeChanged();
    //BIND OUTSTANDING INVOICE TABLE USING CUSTOMER ID AND PAYMENT HEADER 
    BindOutstanding();
    //edit outstanding table Payment text binding
    var table = $('#tblOutStandingDetails').DataTable();
    var allData = table.rows().data();
    var sum = 0;
    AmountReceived = roundoff($('#TotalRecievedAmt').val())
    for (var i = 0; i < allData.length; i++) {
        sum = sum + parseFloat(allData[i].CustomerPayment.CustomerPaymentDetail.PaidAmount);
    }
    $('#lblPaymentApplied').text(roundoff(sum));
    $('#lblCredit').text(roundoff(AmountReceived - sum));
    $('#lblPaymentApplied').text(roundoff(thisitem.TotalRecievedAmt));
    Selectcheckbox();
    clearUploadControl();
    PaintImages(PaymentID);
}
function CaptionChangePayment() {
    $("#lblTotalRecdAmtCptn").text('Total Amount Recevied');
    $("#lblPaymentAppliedCptn").text('Payment Applied');
    $("#lblCreditCptn").text('Credit Received');
    $("#lblTotalAmtRecdCptn").text('Amount Received');
    $("#lblpaidAmt").text('Amount Received');
}
function GetCustomerPayments() {
    try {
        debugger;
        var paymentId = $('#ID').val() == "" ? EmptyGuid : $('#ID').val();
        var data = {"Id": paymentId };
        var jsonData = {};
        var result = "";
        var message = "";
        var customerPaymentVM = new Object();

        jsonData = GetDataFromServer("CustomerPayment/GetCustomerPayment/", data);
        if (jsonData != '') {
            jsonData = JSON.parse(jsonData);
            result = jsonData.Result;
            message = jsonData.Message;
            customerPaymentVM = jsonData.Record;
        }
        if (result == "OK") {

            return customerPaymentVM;
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
//Delete
function DeletePayments() {
    notyConfirm('Are you sure to delete?', 'Delete()', '', "Yes, delete it!");
}
function Delete() {
    $('#btnFormDelete').trigger('click');
}