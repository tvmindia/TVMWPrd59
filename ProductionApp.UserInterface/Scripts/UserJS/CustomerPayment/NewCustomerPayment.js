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
var _dataTable = {};
var _emptyGuid = "00000000-0000-0000-0000-000000000000";
var _CustomerPaymentDetailList= [];
$(document).ready(function () {
    
    try {
        //$("#PaymentMode").select2({
        //});
        $("#CustomerID").select2({
        });
        //$("#Type").select2({
        //});
        $("#BankCode").select2({
        });
        $('#advAmt').hide();
        $('#lblAdvAmt').hide();
        $('#btnUpload').click(function () {
            
            //Pass the controller name
            var FileObject = new Object;
            if ($('#hdnFileDupID').val() != _emptyGuid) {
                FileObject.ParentID = (($('#ID').val()) != _emptyGuid ? ($('#ID').val()) : $('#hdnFileDupID').val());
            }
            else {
                FileObject.ParentID = ($('#ID').val() == _emptyGuid) ? "" : $('#ID').val();
            }


            FileObject.ParentType = "CustomerPayment";
            FileObject.Controller = "FileUpload";
            UploadFile(FileObject);
        });

        //Customer InvoiceTbl
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
                     "data": "CustomerPayment.CustomerPaymentDetail.PaidAmount", 'render': function (data, type, row) {
                         index = index + 1
                         return '<input class="form-control text-right paymentAmount" name="Markup" value="' + roundoff(data) + '" onfocus="paymentAmountFocus(this);" onchange="PaymentAmountChanged(this);" onkeypress = "return isNumber(event)" id="PaymentValue_' + index + '" type="text">';
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
            
            BindCustomerPayment()
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
        $('#TotalRecievedAmt').prop('disabled', true);
        $('#BankCode').prop('disabled', true);
        $('#ChequeDate').prop('disabled', true);
        $('#CreditID').prop('disabled', false);
        $('#ChequeDateFormatted').prop('disabled', true);
        $('#ReferenceBank').prop('disabled', true);
        CaptionChangeCredit();
    }
    else {
        $("#ddlCreditDiv").css("visibility", "hidden");
        $('#PaymentMode').prop('disabled', false);
        $('#BankCode').prop('disabled', true);
        $('#TotalRecievedAmt').val(0);
        $('#TotalRecievedAmt').prop('disabled', false);
        $('#CreditID').val(_emptyGuid); 
        $('#ChequeDateFormatted').prop('disabled', false);
        $('#ReferenceBank').prop('disabled', false);
        CaptionChangePayment();
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
    
    if ($('#CustomerID').val() != "") {
        BindCreditDropDown();
        BindOutstandingAmount();
        $('#TotalRecievedAmt').val('');
        AmountChanged();
    }
    BindOutstanding();
}
function BindOutstanding() {
    index = 0;
    _dataTable.OutStandingInvoices.search('').draw(); //required after
    _dataTable.OutStandingInvoices.clear().rows.add(GetOutStandingInvoices()).draw(false);
}
function GetOutStandingInvoices() {
    try {
        var custId = $('#CustomerID').val() == "" ? _emptyGuid : $('#CustomerID').val();
        var paymentId = $('#ID').val() == "" ? _emptyGuid : $('#ID').val();
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
    
    var sum = 0;
    _dataTable.OutStandingInvoices.rows().deselect();
    if ($('#TotalRecievedAmt').val() < 0 || $('#TotalRecievedAmt').val() == "") {
        $('#TotalRecievedAmt').val(0);
    }

    AmountReceived = parseFloat($('#TotalRecievedAmt').val())
    if (!isNaN(AmountReceived)) {
        $('#TotalRecievedAmt').val(roundoff(AmountReceived));
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
        _dataTable.OutStandingInvoices.clear().rows.add(allData).draw(false);
        Selectcheckbox();
        $('#lblPaymentApplied').text(roundoff(sum));
        $('#lblCredit').text(roundoff(AmountReceived - sum));
    }
}

function PaymentAmountChanged(this_Obj) {
    
    AmountReceived = parseFloat($('#TotalRecievedAmt').val())
    sum = 0;
    var allData = _dataTable.OutStandingInvoices.rows().data();
    var table = _dataTable.OutStandingInvoices;
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

            if (parseFloat(allData[i].Balance) < parseFloat(this_Obj.value)) {
                if (currenttotal < AmountReceived) {
                    allData[i].CustomerPayment.CustomerPaymentDetail.PaidAmount = parseFloat(allData[i].Balance)
                    sum = sum + parseFloat(allData[i].Balance);
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
    _dataTable.OutStandingInvoices.clear().rows.add(allData).draw(false);

    $('#lblPaymentApplied').text(roundoff(sum));
    $('#lblCredit').text(roundoff(AmountReceived - sum));
    Selectcheckbox();
}
function Selectcheckbox() {
    
    var table = $('#tblOutStandingDetails').DataTable();
    var allData = table.rows().data();
    for (var i = 0; i < allData.length; i++) {
        if (allData[i].CustomerPayment.CustomerPaymentDetail.PaidAmount == "" || allData[i].CustomerPayment.CustomerPaymentDetail.PaidAmount == roundoff(0)) {
            _dataTable.OutStandingInvoices.rows(i).deselect();
        }
        else {
            _dataTable.OutStandingInvoices.rows(i).select();
        }
    }
}
//Bind Outstanding Amount
function BindOutstandingAmount() {
    var thisitem = GetOutstandingAmountByCustomer();
    if (thisitem != null) {
        $('#invoicedAmt').text(thisitem.InvoiceAmount == null ? "₹ 0.00" : "₹" + roundoff(thisitem.InvoiceAmount));
    }
}
function GetOutstandingAmountByCustomer() {
    try {
        
        var custId = $('#CustomerID').val() == "" ? _emptyGuid : $('#CustomerID').val();
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
     
    var $form = $('#CustomerPaymentForm');
    if ($form.valid())
    {
        ValidatePaymentRefNo();
    }
    else {
        notyAlert('warning', "Please fill required fields");
    }
}
function ValidatePaymentRefNo() {
    try {
        
        var PaymentID = $('#ID').val();
        var paymentRefNo=$("#PaymentRef").val();
        var data = { "id": PaymentID, "paymentRefNo": paymentRefNo };
        var jsonData = {};
        var result = "";
        var message = "";
        
        jsonData = GetDataFromServer("CustomerPayment/ValidateCustomerPayment/", data);
        if (jsonData != '') {
            jsonData = JSON.parse(jsonData);
            result = jsonData.Result;
            message = jsonData.Message;
        }
        if (result == "OK") {
            if (jsonData.Record.Status == 1)
                notyConfirm(jsonData.Record.Message, 'SaveValidatedData();', '', "Yes,Proceed!", 1);
            else
                SaveValidatedData();
        }
        if (result == "ERROR") {
            alert(message);
        }
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}
function SaveValidatedData()
{
    $(".cancel").click();
    $("#DetailJSON").val('');
    _CustomerPaymentDetailList = [];
    AddCustomerPaymentDetailList();
    var result = JSON.stringify(_CustomerPaymentDetailList);
    $("#DetailJSON").val(result);
    //$('#hdfCreditAmount').val($('#lblPaymentApplied').text());
    //$('#AdvanceAmount').val($('#lblCredit').text());
    setTimeout(function () {
        $('#btnSave').trigger('click');
    }, 1000);
    _SlNo = 1;
}
function AddCustomerPaymentDetailList() {
    var PaymentInvoices = _dataTable.OutStandingInvoices.rows(".selected").data();
    var appliedAmountSum = 0;
    var totalAmtReceived = parseFloat($('#TotalRecievedAmt').val())
        for (var r = 0; r < PaymentInvoices.length; r++) {
            paymentDetail = new Object();
            paymentDetail.InvoiceID = PaymentInvoices[r].ID;
            paymentDetail.ID = PaymentInvoices[r].CustomerPayment.CustomerPaymentDetail.ID;
            paymentDetail.PaymentID = $('#ID').val() == "" ? _emptyGuid : $('#ID').val();
            paymentDetail.PaidAmount = PaymentInvoices[r].CustomerPayment.CustomerPaymentDetail.PaidAmount;
            _CustomerPaymentDetailList.push(paymentDetail);
            appliedAmountSum = parseFloat(appliedAmountSum) + parseFloat(PaymentInvoices[r].CustomerPayment.CustomerPaymentDetail.PaidAmount);
        }
    $('#hdfCreditAmount').val(appliedAmountSum);
    $('#AdvanceAmount').val(parseFloat(totalAmtReceived) - parseFloat(appliedAmountSum));
}
function SaveSuccessCustomerPayment(data, status) {
    
    var JsonResult = JSON.parse(data)
    switch (JsonResult.Result) {
        case "OK":
            $('#IsUpdate').val('True');
            $('#ID').val(JsonResult.Records.ID)
            notyAlert("success", JsonResult.Records.Message)
            BindCustomerPayment();
            _SlNo = 1;
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
    var customerPaymentVM = GetCustomerPayments();
    $('#lblReturnSlipNo').text('Customer Payment# : ' + customerPaymentVM.EntryNo); 
    $('#EntryNo').val(customerPaymentVM.EntryNo);
    $('#ID').val(PaymentID);
    $('#deleteId').val(PaymentID);
    $("#CustomerID").select2();
    $("#CustomerID").val(customerPaymentVM.CustomerID).trigger('change');
    $('#CustomerID').prop('disabled', true);
    $('#ReferenceBank').val(customerPaymentVM.ReferenceBank);
    $('#PaymentDateFormatted').val(customerPaymentVM.PaymentDateFormatted);
    $('#ChequeDateFormatted').val(customerPaymentVM.ChequeDateFormatted);
    $('#PaymentRef').val(customerPaymentVM.PaymentRef);
    $('#PaymentMode').val(customerPaymentVM.PaymentMode);
    $('#BankCode').val(customerPaymentVM.BankCode).select2();
    $('#DepositWithdrawalID').val(customerPaymentVM.DepositWithdrawalID);
    $('#GeneralNotes').val(customerPaymentVM.GeneralNotes);
    $('#TotalRecievedAmt').val(roundoff(customerPaymentVM.TotalRecievedAmt));
    $('#lblTotalRecdAmt').text(roundoff(customerPaymentVM.TotalRecievedAmt));
    $('#paidAmt').text("₹" + roundoff(customerPaymentVM.TotalRecievedAmt));
    $('#Type').val(customerPaymentVM.Type);
    $('#hdfType').val(customerPaymentVM.Type);
    $('#Type').prop('disabled', true);
    if (customerPaymentVM.AdvanceAmount == 0) {
        $('#advAmt').hide();
        $('#lblAdvAmt').hide();
    }
    else {
        $('#advAmt').show();
        $('#lblAdvAmt').show();
        $('#advAmt').text( "₹" + roundoff(customerPaymentVM.AdvanceAmount));
    }
    BindOutstandingAmount();

    if ($('#Type').val() == 'C') {
        $("#CreditID").html("");
        //Get Available Credit and Add with  TotalRecdAmt
        debugger;
        var thisObj = GetCreditNoteByPaymentID(customerPaymentVM.CustomerID, PaymentID)
        if (thisObj.length > 0)
            var CreditAmount = parseFloat(customerPaymentVM.TotalRecievedAmt) + parseFloat(thisObj[0].AvailableCredit);
        else
            var CreditAmount = parseFloat(customerPaymentVM.TotalRecievedAmt);

        $('#TotalRecievedAmt').val(roundoff(CreditAmount))
        $('#lblTotalRecdAmt').text(roundoff(CreditAmount))
        $('#paidAmt').text("₹" + roundoff(CreditAmount));
        $("#CreditID").append($('<option></option>').val(customerPaymentVM.CreditID).html(customerPaymentVM.CreditNo + ' ( Credit Amt: ₹' + CreditAmount + ')'));
        $('#CreditID').val(customerPaymentVM.CreditID)
        $('#CreditID').prop('disabled', true);
        $('#TotalRecievedAmt').prop('disabled', true);
        $('#hdfCreditID').val(customerPaymentVM.CreditID);
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
    $('#lblCredit').text(roundoff(AmountReceived - sum));
    $('#lblPaymentApplied').text(roundoff(sum));
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
function CaptionChangeCredit() {
    $("#lblTotalRecdAmtCptn").text('Credit Amount');
    $("#lblPaymentAppliedCptn").text('Total Credit Used');
    $("#lblCreditCptn").text('Credit Remaining');
    $("#lblTotalAmtRecdCptn").text('Credit Amount');
    $("#lblpaidAmt").text('Credit Amount');
}

function GetCustomerPayments() {
    try {
        
        var paymentId = $('#ID').val() == "" ? _emptyGuid : $('#ID').val();
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
//Bind CreditDropDown
function BindCreditDropDown() {
    
    var ID = $("#CustomerID").val() == "" ? null : $("#CustomerID").val();
    if (ID != null) {
        var ds = GetCreditNoteByCustomer(ID);
        if (ds.length > 0) {
            $("#CreditID").html(""); // clear before appending new list 
            $("#CreditID").append($('<option></option>').val(_emptyGuid).html('--Select Credit Note--'));
            $.each(ds, function (i, credit) {
                $("#CreditID").append(
                    $('<option></option>').val(credit.ID).html(credit.CreditNoteNo + ' ( Credit Amt: ₹' + credit.AvailableCredit + ')'));
            });
        }
        else {
            $("#CreditID").html("");
            $("#CreditID").append($('<option></option>').val(_emptyGuid).html('No Credit Notes Available'));
        }
    }
}
function GetCreditNoteByCustomer(ID) {
    try {
        
        var data = { "Id": ID };
        var jsonData = {};
        var result = "";
        var message = "";
        var customerCreditNoteVM = new Object();

        jsonData = GetDataFromServer("CustomerPayment/GetCustomerCreditNote/", data);
        if (jsonData != '') {
            jsonData = JSON.parse(jsonData);
            result = jsonData.Result;
            message = jsonData.Message;
            customerCreditNoteVM = jsonData.Records;
        }
        if (result == "OK") {

            return customerCreditNoteVM;
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
function DeleteClick() {
    notyConfirm('Are you sure to delete?', 'DeleteCustomerPayment()');
}
function DeleteCustomerPayment() {
    try {
        
        var id = $('#ID').val();
        if (id != '' && id != null) {
            var data = { "id": id };
            var jsonData = {};
            var result = "";
            var message = "";
            var customerPaymentVM = new Object();
            jsonData = GetDataFromServer("CustomerPayment/DeleteCustomerPayment/", data);
            if (jsonData != '') {
                jsonData = JSON.parse(jsonData);
                result = jsonData.Result;
                customerPaymentVM = jsonData.Record;
                message = jsonData.Message;
            }
            if (result == "OK") {
                notyAlert('success', customerPaymentVM.message);
                window.location.replace("NewCustomerPayment?code=ACC");
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
function Reset() {
    BindCustomerPayment();
}

//
function ddlCreditOnChange(event) {
    
    var creditID = $("#CreditID").val();
    var CustomerID = $("#CustomerID").val();
    if (creditID != _emptyGuid) {
        var ds = GetCreditNoteAmount(creditID, CustomerID);
        $('#TotalRecievedAmt').val(ds.AvailableCredit);
        $('#TotalRecievedAmt').prop('disabled', true);
        AmountChanged();
    }

}

function GetCreditNoteAmount(ID, CustomerID) {
    try {
        var data = { "CreditID": ID, "CustomerID": CustomerID };
        var ds = {};
        ds = GetDataFromServer("CustomerPayment/GetCreditNoteAmount/", data);
        if (ds != '') {
            ds = JSON.parse(ds);
        }
        if (ds.Result == "OK") {
            return ds.Records;
        }
        if (ds.Result == "ERROR") {
            notyAlert('error', ds.Message);
            var emptyarr = [];
            return emptyarr;
        }
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}
function GetCreditNoteByPaymentID(ID, PaymentID) {
    try {
        var data = { "ID": ID, "PaymentID": PaymentID };
        var ds = {};
        ds = GetDataFromServer("CustomerPayment/GetCreditNoteByPaymentID/", data);
        if (ds != '') {
            ds = JSON.parse(ds);
        }
        if (ds.Result == "OK") {
            return ds.Records;
        }
        if (ds.Result == "ERROR") {
            notyAlert('error', ds.Message);
            var emptyarr = [];
            return emptyarr;
        }
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}