//*****************************************************************************
//*****************************************************************************
//Author: Angel
//CreatedDate: 12-Feb-2018 
//LastModified: 5-Mar-2018 
//FileName: PurchaseOrder.js
//Description: Client side coding for PurchaseOrder
//******************************************************************************
//******************************************************************************

//--Global Declaration--//
var emptyGuid = "00000000-0000-0000-0000-000000000000";
var DataTables = {};
var PODDetail = [];
var PODetailViewModel = new Object();
var PODDetailLink = [];
var PurchaseOrderDetailList = [];
var RequisitionDetailLink = new Object();
var PurchaseOrderViewModel = new Object();
var EditPOdetailID;
var ECGST, ESGST, ETotal, flag = 1;
var _SlNo = 1;
var _taxDropdownScript = '';

$(document).ready(function () {
    debugger;
    try {
        $("#SupplierID").select2({
        });
        $('#btnUpload').click(function () {
            debugger;
            //Pass the controller name
            var FileObject = new Object;
            if ($('#hdnFileDupID').val() != emptyGuid) {
                FileObject.ParentID = (($('#ID').val()) != emptyGuid ? ($('#ID').val()) : $('#hdnFileDupID').val());
            }
            else {
                FileObject.ParentID = ($('#ID').val() == emptyGuid) ? "" : $('#ID').val();
            }


            FileObject.ParentType = "PurchaseOrder";
            FileObject.Controller = "FileUpload";
            UploadFile(FileObject);
        });
        //RequisitionDetails
        DataTables.RequisitionDetailsTable = $('#tblRequisitionDetails').DataTable({
            dom: '<"pull-left"f>rt<"bottom"ip><"clear">',
            ordering: false,
            searching: true,
            paging: true,
            "bInfo": false,
            pageLength: 7,
            data: null,
            columns: [
                 { "data": "ID", "defaultContent": "<i>-</i>" },
                 { "data": "ReqID", "defaultContent": "<i>-</i>" },
                 { "data": "MaterialID", "defaultContent": "<i>-</i>" },
                 { "data": null, "defaultContent": "", "width": "5%" },
                 { "data": "ReqNo", "defaultContent": "<i>-</i>", "width": "5%" },
                 { "data": "Material.MaterialCode", "defaultContent": "<i>-</i>", "width": "5%" },
                 {
                     "data": "Description", "defaultContent": "<i>-</i>", "width": "10%",
                     'render': function (data, type, row) {
                         if (row.Description)
                             Desc = data;
                         else
                             Desc = row.Description;
                         return '<input class="form-control description" name="Markup" value="' + Desc + '" type="text" onchange="textBoxValueChanged(this,1);"style="width:100%">';
                     }
                 },
                 {
                     "data": "ApproximateRate", "defaultContent": "<i>-</i>", "width": "20%", 'render': function (data, type, row) {
                         return '<input class="form-control text-right " name="Markup" value="' + roundoff(row.ApproximateRate) + '" type="text" onclick="SelectAllValue(this);" onkeypress = "return isNumber(event)", onchange="textBoxValueChanged(this,2);"style="width:100%">';
                     }
                 },
                 {
                     "data": "TaxTypeCode", "defaultContent": "<i>-</i>", "width": "5%",
                     'render': function (data, type, row) {
                         if (data != null) {
                             var first = _taxDropdownScript.slice(0, _taxDropdownScript.indexOf('value="' + data + '"'));
                             var second = _taxDropdownScript.slice(_taxDropdownScript.indexOf('value="' + data + '"'), _taxDropdownScript.length);
                             return '<select class="form-control" onchange="textBoxValueChanged(this,5);" >' + first + ' selected="selected" ' + second + '</select>';
                         }
                         else {
                             return '<select class="form-control" onchange="textBoxValueChanged(this,5);" >' + _taxDropdownScript + '</select>';
                         }
                     }
                 },
                  {
                      "data": "Discount", "defaultContent": "<i>-</i>", "width": "20%",
                      'render': function (data, type, row) {
                          return '<input class="form-control text-right" name="Markup" value="' + roundoff(data) + '" type="text" onchange="textBoxValueChanged(this,3);"style="width:100%">';
                      }
                  },
                { "data": "RequestedQty", "defaultContent": "<i>-</i>", "width": "5%" },
                 { "data": "OrderedQty", "defaultContent": "<i>-</i>", "width": "5%" },
                 {
                     "data": "POQty", "defaultContent": "<i>-</i>", "width": "20%",
                     'render': function (data, type, row) {
                         return '<input class="form-control text-right " name="Markup" type="text"  value="' + roundoff(data) + '"  onclick="SelectAllValue(this);" onkeypress = "return isNumber(event)", onchange="textBoxValueChanged(this,4);"style="width:100%">';
                     }
                 },
                 { "data": "RawMaterial.UnitCode", "defaultContent": "<i>-</i>" }

            ],
            columnDefs: [{ orderable: false, className: 'select-checkbox', "targets": 3 }
                , { className: "text-left", "targets": [5, 6] }
                , { className: "text-right", "targets": [7, 8, 9, 10, 12] }
                , { className: "text-center", "targets": [1, 4,11] }
                , { "targets": [0,1,2,13], "visible": false, "searchable": false }
               ],

            select: { style: 'multi', selector: 'td:first-child' }
        });
        //PurchaseOrder
          DataTables.PurchaseOrderDetailTable = $('#tblPurchaseOrderDetail').DataTable(
            {
                dom: '<"pull-right"f>rt<"bottom"ip><"clear">',
                ordering: false,
                searching: false,
                "bInfo": false,
                paging: false,
                data: null,
                pageLength: 15,
                language: {
                    search: "_INPUT_",
                    searchPlaceholder: "Search"
                },
                columns: [
                  { "data": "ID" },
                  {
                      "data": "", render: function (data, type, row) {
                          debugger;
                          return _SlNo++
                      }, "defaultContent": "<i></i>", "width": "3%"
                  },
                  { "data": "MaterialCode", "defaultContent": "<i>-</i>", "width": "10%" },
                  { "data": "MaterialDesc", "defaultContent": "<i>-</i>" },
                  { "data": "UnitCode", "defaultContent": "<i>-</i>", "width": "5%" },
                  { "data": "POQty", "defaultContent": "<i>-</i>", "width": "4%" },
                  {
                      "data": "Rate", "defaultContent": "<i>-</i>",
                      'render': function (data, type, row) {
                          return roundoff(data);
                      }, "width": "6%"
                  },
                  {
                      "data": "Discount", "defaultContent": "<i>-</i>",
                      'render': function (data, type, row) {
                              return roundoff(data);
                      }, "width": "6%"
                  },
                  {
                      "data": "TaxableAmount", "defaultContent": "<i>-</i>",
                      'render': function (data, type, row) {
                          Desc = parseFloat(row.Qty) * parseFloat(row.Rate) - parseFloat(row.Discount);
                          return roundoff(Desc);
                      }, "width": "10%"
                  },
                  {
                      "data": "CGSTAmt", "defaultContent": "<i>-</i>",
                      'render': function (data, type, row) {
                          return roundoff(data);
                      }, "width": "6%"
                  },
                  {
                      "data": "SGSTAmt", "defaultContent": "<i>-</i>",
                      'render': function (data, type, row) {
                          return roundoff(data);
                      }, "width": "6%"
                  },
                  {
                      "data": "Total", "defaultContent": "<i>-</i>",
                      'render': function (data, type, row) {
                          Desc = (parseFloat(row.Qty) * parseFloat(row.Rate) - parseFloat(row.Discount)) + parseFloat(row.CGSTAmt) + parseFloat(row.SGSTAmt);
                          return roundoff(Desc);
                      }, "width": "10%"
                  },
                { "data": null, "orderable": false, "width": "7%", "defaultContent": '<a href="#" class="ItemEditlink" onclick="EditPurchaseOrderDetailTable(this)"><i class="glyphicon glyphicon-edit" aria-hidden="true"></i></a>  |  <a href="#" class="DeleteLink" onclick="Delete(this)"><i class="glyphicon glyphicon-trash" aria-hidden="true"></i></a>' }
                ],
                columnDefs: [{ "targets": [0], "visible": false, "searchable": false },
                    { "targets": [1], "width": "2%", },
                     { className: "text-right", "targets": [5,6,7, 8,9,10,11] },
                      { className: "text-left", "targets": [3,4] },
                { className: "text-center", "targets": [9,12] }

                ]
            });
        
        //EditPurchaseOrderDetail
            
          DataTables.EditPurchaseDetailsTable = $('#tblPurchaseOrderDetailsEdit').DataTable({

                  dom: '<"pull-left"f>rt<"bottom"ip><"clear">',
                  ordering: false,
                  searching: true,
                  "bInfo": false,
                  paging: false,
                  pageLength: 7,
                  data: null,
                      columns: [
                        { "data": "PurchaseOrderID", "defaultContent": "<i>-</i>"},
                        { "data": "RequisitionDetail.ReqID", "defaultContent": "<i>-</i>"},
                        {"data": "RequisitionDetail.ID", "defaultContent": "<i>-</i>"},
                        { "data": "MaterialID", "defaultContent": "<i>-</i>"},
                        { "data": "RequisitionDetail.ReqNo", "defaultContent": "<i>-</i>", "width": "10%" },
                        {"data": "MaterialCode", "defaultContent": "<i>-</i>"},
                        {
                         "data": "MaterialDesc", "defaultContent": "<i>-</i>",
                         'render': function (data, type, row) {
                           var Desc = "";
                           if (row.MaterialDesc)
                               Desc = data;
                           else
                               Desc = row.MaterialDesc;
                           return '<input class="form-control description" name="Markup" value="' + Desc + '" type="text" onchange="EdittextBoxValue(this,1);"style="width:100%">';
                        }
                        },
                        {
                          "data": "Rate", "defaultContent": "<i>-</i>", "width": "10%", 'render': function (data, type, row) {
                              return '<input class="form-control text-right " name="Markup" value="' + roundoff(data) + '" type="text" onclick="SelectAllValue(this);" onkeypress = "return isNumber(event)", onchange="EdittextBoxValue(this,2);"style="width:100%">';
                        }
                        },
                        {
                          "data": "TaxTypeCode", "defaultContent": "<i>-</i>", "width": "10%",
                           'render': function (data, type, row) {
                               if (data != null) {
                                   var first = _taxDropdownScript.slice(0, _taxDropdownScript.indexOf('value="' + data + '"'));
                                   var second = _taxDropdownScript.slice(_taxDropdownScript.indexOf('value="' + data + '"'), _taxDropdownScript.length);
                                   return '<select class="form-control" onchange="EdittextBoxValue(this,5);" >' + first + ' selected="selected" ' + second + '</select>';
                               }
                               else {
                                   return '<select class="form-control" onchange="EdittextBoxValue(this,5);" >' + _taxDropdownScript + '</select>';
                               }
                           }
                           },
                         {
                             "data": "Discount", "defaultContent": "<i>-</i>",
                             'render': function (data, type, row) {
                                 return '<input class="form-control description text-right" name="Markup" value="' + roundoff(data) + '" onclick="SelectAllValue(this);" onkeypress = "return isNumber(event)", type="text" onchange="EdittextBoxValue(this,3);"style="width:100%">';
                          }
                          },
                          { "data": "RequisitionDetail.RequestedQty", "defaultContent": "<i>-</i>", "width": "10%"},
                          {"data": "RequisitionDetail.OrderedQty", "defaultContent" : "<i>-</i>", "width": "10%" },
                          {
                            "data": "Qty", "defaultContent": "<i>-</i>", "width": "10px",
                            'render': function (data, type, row) {
                                return '<input class="form-control text-right " name="Markup" type="text"  value="' + roundoff(data) + '"  onclick="SelectAllValue(this);" onkeypress = "return isNumber(event)", onchange="EdittextBoxValue(this,6);"style="width:100%">';
                           }
                           }
                               
                       ],
                     columnDefs: [
                        { className: "text-left", "targets": [5, 6]}
                       , { className: "text-right", "targets": [7, 8, 9, 11]}
                       , {className: "text-center", "targets": [1, 4]}
                       , {"targets": [0, 1, 2,3], "visible": false, "searchable": false}]     
                         
                 
          });

        //RequisitionListTable
          DataTables.RequisitionListTable = $('#tblRequisitionList').DataTable({
              dom: '<"pull-right"f>rt<"bottom"ip><"clear">',
              ordering: false,
              searching: false,
              "bInfo": false,
              paging: true,
              data: null,
              pageLength: 15,
              language: {
                  search: "_INPUT_",
                  searchPlaceholder: "Search"
              },
              columns: [
                   { "data": "ID", "defaultContent": "<i>-</i>" },
                   { "data": "Checkbox", "defaultContent": "", "width": "5%" },
                   { "data": "ReqNo", "defaultContent": "<i>-</i>" },
                   { "data": "Title", "defaultContent": "<i>-</i>" },
                   { "data": "ReqDateFormatted", "defaultContent": "<i>-</i>" },
                   { "data": "ReqStatus", "defaultContent": "<i>-</i>" },
                   { "data": "ApprovalDateFormatted", "defaultContent": "<i>-</i>" },
                   { "data": "RequisitionBy", "defaultContent": "<i>-</i>" }
              ],
              columnDefs: [{ orderable: false, className: 'select-checkbox', "targets": 1 }
                  , { className: "text-left", "targets": [2, 3, 7, 6] }
                  , { className: "text-center", "targets": [4, 5] }
                  , { "targets": [0], "visible": false, "searchable": false }
                  , { "targets": [2, 3, 4, 5, 6, 7], "bSortable": false }],
              select: { style: 'multi', selector: 'td:first-child' },
              destroy: true
          });

        //EditPurchaseOrder
          if ($('#ID').val() != emptyGuid) {
              BindPurchaseOrder($('#ID').val());
          }
          $('#btnSendDownload').hide();
          $("#SupplierID").change(function () {
              SupplierDetails();
          });
          $("#Discount").change(function () {
              debugger;
              CalculateGrossAmount();
          });
        }
        
    
    catch (e) {
        console.log(e.message);
    }
});
function AddPurchaseOrderDetail() {
    debugger;
    var $form = $('#PurchaseOrderForm');
    if ($("#LatestApprovalStatus").val() == 3 || $("#LatestApprovalStatus").val() == 0) {
        if ($form.valid()) {
            $('#RequisitionDetailsModal').modal('show');
            ViewRequisitionList(1);
            BindRequisitionListTable();
        }
        else {
            notyAlert('warning', "Please fill required fields,to add items ");
        }
    }
}
function ViewRequisitionList(value) {
    $('#tabDetail').attr('data-toggle', 'tab');
    $('#btnForward').show();
    $('#btnBackward').hide();
    $('#btnAddPODetails').hide();
    if (value)
        $('#tabList').trigger('click');
}
//bind supplier address 
function SupplierDetails()
{
    debugger;
    var supplierid = $('#SupplierID').val();
    supplierVM = GetSupplierDetails(supplierid);
    $('#MailingAddress').val(supplierVM.BillingAddress);
    $('#ShippingAddress').val(supplierVM.ShippingAddress);
}
function GetSupplierDetails(supplierid)
{
    try{
        debugger;
        var data = { "supplierid": supplierid };
        var jsonData = {};
        var result = "";
        var message = "";
        var supplierVM = new Object();

        jsonData = GetDataFromServer("PurchaseOrder/GetSupplierDetails/", data);
        if (jsonData != '') {
            jsonData = JSON.parse(jsonData);
            result = jsonData.Result;
            message = jsonData.Message;
            supplierVM = jsonData.Records;
        }
        if (result == "OK") {

            return supplierVM;
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
//bind requistition List
function BindRequisitionListTable() {
    var requisitionList = GetRequisitionList();
    DataTables.RequisitionListTable.clear().rows.add(requisitionList).draw(false);
}
function GetRequisitionList() {
    try {
        debugger;
        var data = {};
        var jsonData = {};
        var result = "";
        var message = "";
        var requisitionListVM = new Object();

        jsonData = GetDataFromServer("PurchaseOrder/GetAllRequisitionForPurchaseOrder/", data);
        if (jsonData != '') {
            jsonData = JSON.parse(jsonData);
            result = jsonData.Result;
            message = jsonData.Message;
            requisitionListVM = jsonData.Records;
        }
        if (result == "OK") {

            return requisitionListVM;
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
function ViewRequisitionDetails(value) {
    debugger;
    $('#tabDetail').attr('data-toggle', 'tab');
    if (value)
        $('#tabDetail').trigger('click');
    else {
        //selecting Checked IDs for  bind the detail Table
        var ids = GetSelectedRowIDs();
        if (ids) {
            BindRequisitionDetailsTable(ids);
            DataTables.RequisitionDetailsTable.rows().select();
            $('#btnForward').hide();
            $('#btnBackward').show();
            $('#btnAddPODetails').show();
        }
        else {
            $('#tabDetail').attr('data-toggle', '');
            DataTables.RequisitionDetailsTable.clear().draw(false);
            notyAlert('warning', "Please select Requisition");
        }
    }
}
function GetSelectedRowIDs() {
    var SelectedRows = DataTables.RequisitionListTable.rows(".selected").data();
    if ((SelectedRows) && (SelectedRows.length > 0)) {
        var arrIDs = "";
        for (var r = 0; r < SelectedRows.length; r++) {
            if (r == 0)
                arrIDs = SelectedRows[r].ID;
            else
                arrIDs = arrIDs + ',' + SelectedRows[r].ID;
        }
        return arrIDs;
    }
}
function BindRequisitionDetailsTable(ids) {
    try {
        debugger;
        TaxtypeDropdown();
        var requisitionDetailsVM = GetRequisitionDetailsByIDs(ids);
        DataTables.RequisitionDetailsTable.clear().rows.add(requisitionDetailsVM).draw(false);
    }
    catch (e) {
        //this will show the error msg in the browser console(F12) 
        console.log(e.message);
    }
}
//RequisitionBinding
function GetRequisitionDetailsByIDs(ids) {
    try {
        debugger;
        var POID = $('#ID').val();
        var data = { "IDs": ids, "POID": POID };
        var result = "";
        var message = "";
        var jsonData = {};
        var requisitionDetailsVM = new Object();
        jsonData = GetDataFromServer("PurchaseOrder/GetRequisitionDetailsByIDs/", data);
        if (jsonData != '') {
            jsonData = JSON.parse(jsonData);
            result = jsonData.Result;
            message = jsonData.Message;
            requisitionDetailsVM = jsonData.Records;
        }
        if (result == "OK") {
            return requisitionDetailsVM;
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
function TaxtypeDropdown() {
    var taxTypeVM = GetTaxtypeDropdown()
    _taxDropdownScript = "<option value=" + '' + ">-Select-</option>";
    for (i = 0; i < taxTypeVM.length; i++) {
        _taxDropdownScript = _taxDropdownScript + '<option value="' + taxTypeVM[i].Code + '">' + taxTypeVM[i].Description + '</option>'
    }
}
function GetTaxtypeDropdown() {
    try {
        var data = {};
        var taxTypeVM = new Object();
        jsonData = GetDataFromServer("CustomerInvoice/GetTaxTypeForSelectList/", data);
        if (jsonData != '') {
            jsonData = JSON.parse(jsonData);
            result = jsonData.Result;
            message = jsonData.Message;
            taxTypeVM = jsonData.Records;
        }
        if (result == "OK") {
            return taxTypeVM;
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
//TextBox value change in datatable
function textBoxValueChanged(thisObj, textBoxCode) {
    debugger;
    var IDs = selectedRowIDs();//identify the selected rows 
    var requestionDetailsVM = DataTables.RequisitionDetailsTable.rows().data();
    var requestionDetailstable = DataTables.RequisitionDetailsTable;
    var rowtable = requestionDetailstable.row($(thisObj).parents('tr')).data();
    for (var i = 0; i < requestionDetailsVM.length; i++) {
        if (requestionDetailsVM[i].ID == rowtable.ID) {
            if (textBoxCode == 1)//textBoxCode is the code to know, which textbox changed is triggered
            {
                requestionDetailsVM[i].Description = thisObj.value;
            }
            if (textBoxCode == 2) {
                if ((thisObj.value != "") && (thisObj.value != 0))
                requestionDetailsVM[i].ApproximateRate = parseFloat(thisObj.value);
            }
            if (textBoxCode == 3) {
                if (thisObj.value != "")
                    requestionDetailsVM[i].Discount = parseFloat(thisObj.value);
                else
                    requestionDetailsVM[i].Discount = 0;
            }
            if (textBoxCode == 4) {
                if ((thisObj.value != "") && (thisObj.value != 0))
                requestionDetailsVM[i].POQty = parseFloat(thisObj.value);
            }
            if (textBoxCode == 5)
            {
                var taxTypeVM = GetTaxtypeDropdown();
                requestionDetailsVM[i].TaxTypeCode = thisObj.value;
                for (j = 0; j < taxTypeVM.length; j++) {
                    if (taxTypeVM[j].Code == thisObj.value) {
                        requestionDetailsVM[i].IGSTPerc = taxTypeVM[j].IGSTPercentage;
                        requestionDetailsVM[i].SGSTPerc = taxTypeVM[j].SGSTPercentage;
                        requestionDetailsVM[i].CGSTPerc = taxTypeVM[j].CGSTPercentage;
                    }
                }
            }
        }
    }
    DataTables.RequisitionDetailsTable.clear().rows.add(requestionDetailsVM).draw(false);
    selectCheckbox(IDs); //Selecting the checked rows with their ids taken 
}
//Selected rows
function GetSelectedRowIDs() {
    debugger;
    var SelectedRows = DataTables.RequisitionListTable.rows(".selected").data();
    if ((SelectedRows) && (SelectedRows.length > 0)) {
        var arrIDs = "";
        for (var r = 0; r < SelectedRows.length; r++) {
            if (r == 0)
                arrIDs = SelectedRows[r].ID;
            else
                arrIDs = arrIDs + ',' + SelectedRows[r].ID;
        }
        return arrIDs;
    }
}
//selected Checkbox
function selectCheckbox(IDs) {
    var requistionDetailsVM = DataTables.RequisitionDetailsTable.rows().data()
    for (var i = 0; i < requistionDetailsVM.length; i++) {
        if (IDs.includes(requistionDetailsVM[i].ID)) {
            DataTables.RequisitionDetailsTable.rows(i).select();
        }
        else {
            DataTables.RequisitionDetailsTable.rows(i).deselect();
        }
    }
}
//add details in btnAdd click
function AddPODetails()
{
    debugger;
    _SlNo = 1;
    //Merging  the rows with same MaterialID
    var requistionDetailsVM = DataTables.RequisitionDetailsTable.rows(".selected").data();
    var mergedRows = []; //to store rows after merging
    var currentMaterial, QuantitySum;
    AddRequsitionDetailLink(requistionDetailsVM)// adding values to reqDetailLink array function call

    for (var r = 0; r < requistionDetailsVM.length; r++) {
        var Particulars="";
        Particulars = requistionDetailsVM[r].ReqNo;
        currentMaterial = requistionDetailsVM[r].MaterialID
        for (var j = r + 1; j < requistionDetailsVM.length; j++) {
            if (requistionDetailsVM[j].MaterialID == currentMaterial)
            {
                Particulars = Particulars + "," + requistionDetailsVM[j].ReqNo;
                requistionDetailsVM[r].POQty = parseFloat(requistionDetailsVM[r].POQty) + parseFloat(requistionDetailsVM[j].POQty);
                requistionDetailsVM[r].Discount = parseFloat(requistionDetailsVM[r].Discount) + parseFloat(requistionDetailsVM[j].Discount);
                requistionDetailsVM[r].TaxTypeCode = requistionDetailsVM[r].TaxTypeCode;
                requistionDetailsVM.splice(j, 1);//removing duplicate after adding value 
                j = j - 1;// for avoiding skipping row while checking
            }
        }
        requistionDetailsVM[r].Particulars = Particulars
        mergedRows.push(requistionDetailsVM[r])// adding rows to merge array
    }

    var res=AddRequsitionDetail(mergedRows)// adding to reqDetail array function call

    if (res) {
        debugger;
        DataTables.PurchaseOrderDetailTable.rows.add(PODDetail).draw(false); //binding Detail table with new values added(not existing)
        CalculateGrossAmount();//Calculating GrossAmount after adding new rows 
        $('#RequisitionDetailsModal').modal('hide');
        Save();
    }
}
function AddRequsitionDetailLink(data) {
    debugger;
    ECGST = 0;
    ESGST = 0;
    ETotal = 0;
    for (var r = 0; r < data.length; r++) {
        PurchaseOrderDetailLink = new Object();
        PurchaseOrderDetailLink.MaterialID = data[r].MaterialID;
        PurchaseOrderDetailLink.ID = emptyGuid; //[PODetailID]
        PurchaseOrderDetailLink.ReqDetailID = data[r].ID;//[ReqDetailID]
        PurchaseOrderDetailLink.ReqID = data[r].ReqID;
        PurchaseOrderDetailLink.PurchaseOrderQty = data[r].POQty;
        //------------------------------------------------------//
        PurchaseOrderDetailLink.Discount = data[r].Discount;
        PurchaseOrderDetailLink.TaxTypeCode = data[r].TaxTypeCode;
        PurchaseOrderDetailLink.Amount = parseFloat(data[r].ApproximateRate) * parseFloat(data[r].POQty);
        if (PurchaseOrderDetailLink.Discount != undefined)
            PurchaseOrderDetailLink.Tax = parseFloat(PurchaseOrderDetailLink.Amount) - parseFloat(PurchaseOrderDetailLink.Discount);
        else
            PurchaseOrderDetailLink.Tax = parseFloat(PurchaseOrderDetailLink.Amount);
        if (PurchaseOrderDetailLink.TaxTypeCode != undefined && PurchaseOrderDetailLink.TaxTypeCode != "") {
            var taxTypeVM = GetTaxTypeByCode(PurchaseOrderDetailLink.TaxTypeCode);
            PurchaseOrderDetailLink.CGSTAmt = parseFloat(PurchaseOrderDetailLink.Tax) * parseFloat(parseFloat(taxTypeVM.CGSTPercentage) / 100);
            PurchaseOrderDetailLink.SGSTAmt = parseFloat(PurchaseOrderDetailLink.Tax) * parseFloat(parseFloat(taxTypeVM.SGSTPercentage) / 100);

        }
        else {
            PurchaseOrderDetailLink.CGSTAmt = 0;
            PurchaseOrderDetailLink.SGSTAmt = 0;
        }
        PurchaseOrderDetailLink.Total = parseFloat(PurchaseOrderDetailLink.Tax) + parseFloat(PurchaseOrderDetailLink.CGSTAmt) + parseFloat(PurchaseOrderDetailLink.SGSTAmt);
        ECGST = ECGST + parseFloat(PurchaseOrderDetailLink.CGSTAmt);
        PurchaseOrderDetailLink.ECGST = ECGST;
        ESGST = ESGST + parseFloat(PurchaseOrderDetailLink.SGSTAmt);
        PurchaseOrderDetailLink.ESGST = ESGST;
        ETotal = ETotal + parseFloat(PurchaseOrderDetailLink.Total);
        PODDetailLink.push(PurchaseOrderDetailLink);
    }
}
function AddRequsitionDetail(mergedRows) {
    debugger;
    if ((mergedRows) && (mergedRows.length > 0)) {
        for (var r = 0; r < mergedRows.length; r++) {
            PODetailViewModel = new Object();
            PODetailViewModel.MaterialID = mergedRows[r].MaterialID;
            PODetailViewModel.ID = emptyGuid; //newly added items has emptyguid
            PODetailViewModel.ReqDetailId = mergedRows[r].ID;
            PODetailViewModel.ReqID = mergedRows[r].ReqID;
            PODetailViewModel.MaterialCode = mergedRows[r].Material.MaterialCode;
            PODetailViewModel.MaterialDesc = mergedRows[r].Description;
            PODetailViewModel.Qty = mergedRows[r].POQty;
            PODetailViewModel.Rate = mergedRows[r].ApproximateRate;
            PODetailViewModel.UnitCode = mergedRows[r].Material.UnitCode;
            PODetailViewModel.Discount = mergedRows[r].Discount;
            PODetailViewModel.Particulars = mergedRows[r].Particulars;
            PODetailViewModel.TaxTypeCode = mergedRows[r].TaxTypeCode;
            PODetailViewModel.Amount = parseFloat(mergedRows[r].ApproximateRate) * parseFloat(mergedRows[r].POQty);
            if (mergedRows[r].Discount != undefined)
                PODetailViewModel.Tax = parseFloat(PODetailViewModel.Amount) - parseFloat(PODetailViewModel.Discount);
            else
                PODetailViewModel.Tax = PODetailViewModel.Amount;
            if (PODetailViewModel.TaxTypeCode != undefined && PODetailViewModel.TaxTypeCode != "") {
                var taxTypeVM = GetTaxTypeByCode(PODetailViewModel.TaxTypeCode);
                PODetailViewModel.CGSTAmt = parseFloat(PODetailViewModel.Tax) * parseFloat(parseFloat(taxTypeVM.CGSTPercentage) / 100);
                PODetailViewModel.SGSTAmt = parseFloat(PODetailViewModel.Tax) * parseFloat(parseFloat(taxTypeVM.SGSTPercentage) / 100);
                PODetailViewModel.CGSTPerc = parseFloat(taxTypeVM.CGSTPercentage);
                PODetailViewModel.SGSTPerc = parseFloat(taxTypeVM.SGSTPercentage);
            }
            else {
                PODetailViewModel.CGSTAmt = 0;
                PODetailViewModel.SGSTAmt = 0;
                PODetailViewModel.CGSTPerc = 0;
                PODetailViewModel.SGSTPerc = 0;
            }
            PODetailViewModel.Total = parseFloat(PODetailViewModel.Tax) + parseFloat(PODetailViewModel.CGSTAmt) + parseFloat(PODetailViewModel.SGSTAmt);
            PODDetail.push(PODetailViewModel);
        }
        return true;
    }
    else {
        notyAlert('warning', "Please select Requisition");
        return false;
    }
}

function selectedRowIDs() {
    var allData = DataTables.RequisitionDetailsTable.rows(".selected").data();
    var arrIDs = "";
    for (var r = 0; r < allData.length; r++) {
        if (r == 0)
            arrIDs = allData[r].ID;
        else
            arrIDs = arrIDs + ',' + allData[r].ID;
    }
    return arrIDs;
}

function CalculateGrossAmount() {
    debugger;
    var purchaseOrderVM = DataTables.PurchaseOrderDetailTable.rows().data();
    var GrossAmount = 0;
    var ItemTotal = 0;
    var CGSTTotal = 0;
    var SGSTTotal = 0;
    var TotalTax = 0;
    for (var i = 0; i < purchaseOrderVM.length; i++) {
        ItemTotal = ItemTotal + (parseFloat(purchaseOrderVM[i].Qty) * parseFloat(purchaseOrderVM[i].Rate) - parseFloat(purchaseOrderVM[i].Discount))
        CGSTTotal = CGSTTotal + parseFloat(purchaseOrderVM[i].CGSTAmt)
        SGSTTotal = SGSTTotal + parseFloat(purchaseOrderVM[i].SGSTAmt)
        GrossAmount = GrossAmount + ((parseFloat(purchaseOrderVM[i].Qty) * parseFloat(purchaseOrderVM[i].Rate) - parseFloat(purchaseOrderVM[i].Discount)) + parseFloat(purchaseOrderVM[i].CGSTAmt) + parseFloat(purchaseOrderVM[i].SGSTAmt))
    }
    TotalTax = CGSTTotal + SGSTTotal
    $('#GrossAmount').text(roundoff(GrossAmount));
    $('#ItemTotal').text(roundoff(ItemTotal));
    $('#CGSTTotal').text(roundoff(CGSTTotal));
    $('#SGSTTotal').text(roundoff(SGSTTotal));
    $('#TaxTotal').text(roundoff(TotalTax));
    $('#lblGrandTotal').text(roundoff(GrossAmount));
    if($('#Discount').val()!=0)
        $('#lblGrandTotal').text(roundoff(GrossAmount - $('#Discount').val()));
}
function OrderStatusChange() {
    if ($("#PurchaseOrderStatus").val() != "")
        $("#lblPOStatus").text($("#PurchaseOrderStatus option:selected").text());
    else
        $("#lblStatus").text('N/A');
}
function GetTaxTypeByCode(Code) {
    try {
        debugger;
        var data = { "Code": Code };
        var result = "";
        var message = "";
        var jsonData = {};
        var taxTypeVM = new Object();
        jsonData = GetDataFromServer("PurchaseOrder/GetTaxtype/", data);
        if (jsonData != '') {
            jsonData = JSON.parse(jsonData);
            result = jsonData.Result;
            message = jsonData.Message;
            taxTypeVM = jsonData.Records;
        }
        if (result == "OK") {
            return taxTypeVM;
        }
        if (result == "ERROR") {
            alert(Message);
        }
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}
function Save() {
    debugger;
    //validation main form 
    PurchaseOrderDetailList = [];
    AddPurchaseOrderDetailList();
    if (PurchaseOrderDetailList.length > 0) {
        PurchaseOrderViewModel.ID = $('#ID').val();
        PurchaseOrderViewModel.PurchaseOrderNo = $('#PurchaseOrderNo').val();
        PurchaseOrderViewModel.PurchaseOrderDate = $('#PurchaseOrderDateFormatted').val();
        PurchaseOrderViewModel.PurchaseOrderIssuedDate = $('#PurchaseOrderIssuedDateFormatted').val();
        PurchaseOrderViewModel.SupplierID = $('#SupplierID').val();
        PurchaseOrderViewModel.GeneralNotes = $('#GeneralNotes').val();
        PurchaseOrderViewModel.PurchaseOrderStatus = $('#PurchaseOrderStatus').val();
        PurchaseOrderViewModel.MailingAddress = $('#MailingAddress').val();
        PurchaseOrderViewModel.ShippingAddress = $('#ShippingAddress').val();
        PurchaseOrderViewModel.PurchaseOrderTitle = $('#PurchaseOrderTitle').val();
        PurchaseOrderViewModel.Discount = $('#Discount').val();
        PurchaseOrderViewModel.PODDetail = PODDetail;
        PurchaseOrderViewModel.PODDetailLink = PODDetailLink;
        _SlNo = 1;
        var data = "{'purchaseOrderVM':" + JSON.stringify(PurchaseOrderViewModel) + "}";

        PostDataToServer("PurchaseOrder/InsertPurchaseOrder/", data, function (JsonResult) {
            debugger;
            switch (JsonResult.Result) {
                case "OK":
                        notyAlert('success', JsonResult.Records.Message);
                        ChangeButtonPatchView('PurchaseOrder', 'divbuttonPatchAddPurchaseOrder', 'Edit');
                        if (JsonResult.Records.ID) {
                            $("#ID").val(JsonResult.Records.ID);
                            BindPurchaseOrder($("#ID").val());
                        } else {
                            Reset();
                    }
                        PODDetail = [];
                        PODDetailLink = [];
                    break;
                case "Error":
                    notyAlert('error', JsonResult.Message);
                    break;
                case "ERROR":
                    notyAlert('error', JsonResult.Message);
                    break;
                default:
                    break;
            }
        })
    }
else {
        notyAlert('warning', 'Please add item details!');
    }
}
//POD data checking
function AddPurchaseOrderDetailList() {
    debugger;
    var purchaseOrderDetail = DataTables.PurchaseOrderDetailTable.rows().data();
    for (var r = 0; r < purchaseOrderDetail.length; r++) {
        purchaseDetail = new Object();
        purchaseDetail.ID = purchaseOrderDetail[r].ID;
        purchaseDetail.MaterialCode = purchaseOrderDetail[r].MaterialCode;
        PurchaseOrderDetailList.push(purchaseDetail);
    }
}
//Bind PurchaseOrder
function BindPurchaseOrder(ID) {
    try {
        debugger;
        _SlNo = 1;
        var result = GetPurchaseOrderDetailsByID(ID)
        if (result) {

            $('#PurchaseOrderNo').val(result.PurchaseOrderNo);
            $('#PurchaseOrderDateFormatted').val(result.PurchaseOrderDateFormatted);
            $('#PurchaseOrderIssuedDateFormatted').val(result.PurchaseOrderIssuedDateFormatted);
            $('#SupplierID').val(result.SupplierID).select2();
            $('#GeneralNotes').val(result.GeneralNotes);
            $('#PurchaseOrderStatus').val(result.PurchaseOrderStatus);
            $('#MailingAddress').val(result.MailingAddress);
            $('#ShippingAddress').val(result.ShippingAddress);
            $('#PurchaseOrderTitle').val(result.PurchaseOrderTitle);
            $('#Discount').val(roundoff(result.Discount));
            $('#lblReqNo').text("PO Number# :" + result.PurchaseOrderNo);
            $("#lblEmailStatus").text(result.EmailSentYN == "True" ? 'YES' : 'NO');
            $("#PurchaseOrderMailPreview_SentToEmails").val(result.SubscriberEmail);
            $('#lblApprovalStatus').text(result.ApprovalStatus);
            $('#LatestApprovalStatus').val(result.LatestApprovalStatus);
            $('#LatestApprovalID').val(result.LatestApprovalID);
            $('#lblPOStatus').text(result.PurchaseOrderStatus);
            debugger;
            if (result.LatestApprovalStatus == 3 || result.LatestApprovalStatus == 0) {
                ChangeButtonPatchView('PurchaseOrder', 'divbuttonPatchAddPurchaseOrder', 'Edit');
                EnableDisableFields(false)
                $("#fileUploadControlDiv").show();

            }
            else {
                ChangeButtonPatchView('PurchaseOrder', 'divbuttonPatchAddPurchaseOrder', 'Disable');
                EnableDisableFields(true)
                $("#fileUploadControlDiv").hide();
            }
            PurchaseOrderDetailBindTable() //------binding Details table
            CalculateGrossAmount();
            PaintImages(ID);//bind attachments
            }
            $('#GrossAmount').prop('disabled', true);
            $('#ItemTotal').prop('disabled', true);
            $('#CGSTTotal').prop('disabled', true);
            $('#SGSTTotal').prop('disabled', true);
            $('#TaxTotal').prop('disabled', true);
    }
    catch (e) {
        notyAlert('error', e.Message);
    }
}

function EnableDisableFields(value) {
    $("#btnAddPOItems").attr("disabled", value);
    $('#PurchaseOrderTitle').attr("disabled", value);
    $('#PurchaseOrderDateFormatted').attr("disabled", value);
    $('#PurchaseOrderIssuedDateFormatted').attr("disabled", value);
    $('#MailingAddress').attr("disabled", value);
    $('#ShippingAddress').attr("disabled", value);
    $('#SupplierID').attr("disabled", value);
    //$('#PurchaseOrderStatus').attr("disabled", value);
    DataTables.PurchaseOrderDetailTable.column(12).visible(!value);
    $('#Discount').attr("disabled", value);
    $('#GeneralNotes').attr("disabled", value);
}

function GetPurchaseOrderDetailsByID(ID) {
    try {
        debugger;
        var data = { "ID": ID };
        var result = "";
        var message = "";
        var jsonData = {};
        var purchaseOrderVM = new Object();
        jsonData = GetDataFromServer("PurchaseOrder/GetPurchaseOrder/", data);
        if (jsonData != '') {
            jsonData = JSON.parse(jsonData);
            purchaseOrderVM = jsonData.Records;
            result = jsonData.Result;
            message = jsonData.Message;
        }
        if (result == "OK") {

            return purchaseOrderVM;
        }
        if (result == "ERROR") {
            alert(Message);

        }
    }
    catch (e) {
        //this will show the error msg in the browser console(F12) 
        console.log(e.message);
    }
}
function PurchaseOrderDetailBindTable() {
    try {
        debugger;
        _SlNo = 1;
        DataTables.PurchaseOrderDetailTable.clear().rows.add(GetPurchaseOrderDetailTable()).draw(false);
    }
    catch (e) {
        //this will show the error msg in the browser console(F12) 
        console.log(e.message);
    }
}
function GetPurchaseOrderDetailTable() {
    try {
        debugger;
        var id = $('#ID').val();
        var data = { "ID": id };
        var result = "";
        var message = "";
        var jsonData = {};
        var purchaseOrderDetailVM = new Object();
        jsonData = GetDataFromServer("PurchaseOrder/GetPurchaseOrderDetail/", data);
        if (jsonData != '') {
            jsonData = JSON.parse(jsonData);
            result = jsonData.Result;
            purchaseOrderDetailVM = jsonData.Records;
            message = jsonData.Message;
        }
        if (result == "OK") {
            return purchaseOrderDetailVM;

        }
        if (result == "ERROR") {
            alert(Message);
        }
    }
    catch (e) {
        //this will show the error msg in the browser console(F12) 
        console.log(e.Message);
    }
}
function Reset() {
        BindPurchaseOrder($("#ID").val());
}

//Edit PurchaseOrderDetail
function EditPurchaseOrderDetailTable(curObj) {
    debugger;
    $('#EditRequisitionDetailsModal').modal('show');
    var rowData = DataTables.PurchaseOrderDetailTable.row($(curObj).parents('tr')).data();
    _SlNo = 1;
    EditPurchaseOrderDetailByID(rowData.ID)
    EditPOdetailID = rowData.ID// to set PODetailID
    
}

function EditPurchaseOrderDetailByID(ID) {
    try {
        debugger;
        _SlNo = 1;
        TaxtypeDropdown();
        DataTables.EditPurchaseDetailsTable.clear().rows.add(EditPurchaseOrderDetail(ID)).draw(false);
    }
    catch (e) {
        //this will show the error msg in the browser console(F12) 
        console.log(e.message);
    }
}

function EditPurchaseOrderDetail(ID) {
    try {
        debugger;
        var data = { ID };
        var result = "";
        var message = "";
        var jsonData = {};
        var purchaseOrderDetailVM = new Object();
        jsonData = GetDataFromServer("PurchaseOrder/GetPurchaseOrderDetailByPODetailID/", data);
        if (jsonData != '') {
            jsonData = JSON.parse(jsonData);
            purchaseOrderDetailVM = jsonData.Records;
            result = jsonData.Result;
            message = jsonData.Message;
        }
        if (result == "OK") {
            return purchaseOrderDetailVM;
        }
        if (result == "ERROR") {
            alert(Message);
        }
    }
    catch (e) {
        //this will show the error msg in the browser console(F12) 
        console.log(e.message);
    }
}
function EditPODetails() {
    debugger;
    var purchaseOrderVM = DataTables.EditPurchaseDetailsTable.rows().data();

    var mergedRows = []; //to store rows after merging

    EditRequsitionDetailLink(purchaseOrderVM)// adding to object function call

    for (var r = 0; r < purchaseOrderVM.length; r++) {
        for (var j = r + 1; j < purchaseOrderVM.length; j++) {
            purchaseOrderVM[r].Qty = parseFloat(purchaseOrderVM[r].Qty) + parseFloat(purchaseOrderVM[j].Qty);
            purchaseOrderVM[r].Discount = parseFloat(purchaseOrderVM[r].Discount) + parseFloat(purchaseOrderVM[j].Discount);
            purchaseOrderVM[r].TaxTypeCode = purchaseOrderVM[r].TaxTypeCode;
            purchaseOrderVM.splice(j, 1);//removing duplicate after adding value 
            j = j - 1;// for avoiding skipping row while checking
        }
        mergedRows.push(purchaseOrderVM[r])// adding rows to merge array
    }
    debugger;
    if ((mergedRows) && (mergedRows.length > 0)) {
        for (var r = 0; r < mergedRows.length; r++) {
            PODetailViewModel = new Object();
            PODetailViewModel.MaterialID = mergedRows[r].MaterialID;
            PODetailViewModel.ID = EditPOdetailID;
            PODetailViewModel.ReqDetailId = mergedRows[r].ID;
            PODetailViewModel.ReqID = mergedRows[r].RequisitionDetail.ReqID;
            PODetailViewModel.MaterialCode = mergedRows[r].MaterialCode;
            PODetailViewModel.MaterialDesc = mergedRows[r].MaterialDesc;
            PODetailViewModel.Qty = mergedRows[r].Qty;
            PODetailViewModel.Rate = mergedRows[r].Rate;
            PODetailViewModel.Particulars = mergedRows[r].Particulars;
            PODetailViewModel.Discount = mergedRows[r].Discount;
            PODetailViewModel.TaxTypeCode = mergedRows[r].TaxTypeCode;
            PODetailViewModel.Amount = parseFloat(mergedRows[r].Rate) * parseFloat(mergedRows[r].Qty);
            if (mergedRows[r].Discount != null)
                PODetailViewModel.Tax = parseFloat(PODetailViewModel.Amount) - parseFloat(PODetailViewModel.Discount);
            else
                PODetailViewModel.Tax = parseFloat(PODetailViewModel.Amount);

            var taxTypeVM = GetTaxTypeByCode(PODetailViewModel.TaxTypeCode);
            if (PODetailViewModel.TaxTypeCode != undefined || PODetailViewModel.TaxTypeCode != "") {
                var taxTypeVM = GetTaxTypeByCode(PODetailViewModel.TaxTypeCode);
                PODetailViewModel.CGSTAmt = parseFloat(PODetailViewModel.Tax) * parseFloat(parseFloat(taxTypeVM.CGSTPercentage) / 100);
                PODetailViewModel.SGSTAmt = parseFloat(PODetailViewModel.Tax) * parseFloat(parseFloat(taxTypeVM.SGSTPercentage) / 100);
                PODetailViewModel.CGSTPerc = parseFloat(taxTypeVM.CGSTPercentage);
                PODetailViewModel.SGSTPerc = parseFloat(taxTypeVM.SGSTPercentage);
            }
            else {
                PODetailViewModel.CGSTAmt = 0;
                PODetailViewModel.SGSTAmt = 0;
                PODetailViewModel.CGSTPerc = 0;
                PODetailViewModel.SGSTPerc = 0;
            }
            PODetailViewModel.Total = parseFloat(PODetailViewModel.Tax) + parseFloat(PODetailViewModel.CGSTAmt) + parseFloat(PODetailViewModel.SGSTAmt);
            PODDetail.push(PODetailViewModel);
        }
        debugger;
        UpdateDetailLinkSave();
        $('#EditRequisitionDetailsModal').modal('hide');
    }
}
function EditRequsitionDetailLink(data) {
    debugger;
    ECGST = 0;
    ESGST = 0;
    ETotal = 0;
    for (var r = 0; r < data.length; r++) {
        PurchaseOrderDetailLink = new Object();
        PurchaseOrderDetailLink.MaterialID = data[r].MaterialID;
        PurchaseOrderDetailLink.ID = data[r].RequisitionDetail.ID;//LinkId
        PurchaseOrderDetailLink.ReqDetailID = data[r].ReqDetailID;//[ReqDetailID]
        PurchaseOrderDetailLink.ReqID = data[r].RequisitionDetail.ReqID;
        PurchaseOrderDetailLink.PurchaseOrderQty = data[r].Qty;
        PurchaseOrderDetailLink.Discount = data[r].Discount;
        PurchaseOrderDetailLink.TaxTypeCode = data[r].TaxTypeCode;
        //---------------------
        PurchaseOrderDetailLink.Amount = parseFloat(data[r].Rate) * parseFloat(data[r].Qty);
        if (PurchaseOrderDetailLink.Discount != undefined)
            PurchaseOrderDetailLink.Tax = parseFloat(PurchaseOrderDetailLink.Amount) - parseFloat(PurchaseOrderDetailLink.Discount);
        else
            PurchaseOrderDetailLink.Tax = parseFloat(PurchaseOrderDetailLink.Amount);
        //Particulars after adding same material(item)
        if (PurchaseOrderDetailLink.TaxTypeCode != undefined && PurchaseOrderDetailLink.TaxTypeCode != "") {
            var taxTypeVM = GetTaxTypeByCode(PurchaseOrderDetailLink.TaxTypeCode);
            PurchaseOrderDetailLink.CGSTAmt = parseFloat(PurchaseOrderDetailLink.Tax) * parseFloat(parseFloat(taxTypeVM.CGSTPercentage) / 100);
            PurchaseOrderDetailLink.SGSTAmt = parseFloat(PurchaseOrderDetailLink.Tax) * parseFloat(parseFloat(taxTypeVM.SGSTPercentage) / 100);

        }
        else {
            PurchaseOrderDetailLink.CGSTAmt = 0;
            PurchaseOrderDetailLink.SGSTAmt = 0;
        }
        PurchaseOrderDetailLink.Total = parseFloat(PurchaseOrderDetailLink.Tax) + parseFloat(PurchaseOrderDetailLink.CGSTAmt) + parseFloat(PurchaseOrderDetailLink.SGSTAmt);
        ECGST = ECGST + parseFloat(PurchaseOrderDetailLink.CGSTAmt);
        PurchaseOrderDetailLink.ECGST = ECGST;
        ESGST = ESGST + parseFloat(PurchaseOrderDetailLink.SGSTAmt);
        PurchaseOrderDetailLink.ESGST = ESGST;
        ETotal = ETotal + parseFloat(PurchaseOrderDetailLink.Total);
        PODDetailLink.push(PurchaseOrderDetailLink);
    }
}
function UpdateDetailLinkSave() {
    debugger;
    var $form = $('#PurchaseOrderForm');
    if ($form.valid()) {
        _SlNo = 1;
        PurchaseOrderViewModel.ID = $('#ID').val();
        PurchaseOrderViewModel.PODDetail = PODDetail;
        PurchaseOrderViewModel.PODDetailLink = PODDetailLink;
        var data = "{'purchaseOrderVM':" + JSON.stringify(PurchaseOrderViewModel) + "}";

        PostDataToServer("PurchaseOrder/UpdatePurchaseOrderDetailLink/", data, function (JsonResult) {

            debugger;
            switch (JsonResult.Result) {
                case "OK":
                   
                        notyAlert('success', JsonResult.Records.Message);
                        ChangeButtonPatchView('PurchaseOrder', 'divbuttonPatchAddPurchaseOrder', 'Edit');
                        BindPurchaseOrder($("#ID").val());
                        PODDetail = [];
                        PODDetailLink = [];
                  
                         break;
                case "Error":
                    notyAlert('error', JsonResult.Message);
                    break;
                case "ERROR":
                    notyAlert('error', JsonResult.Message);
                    break;
                default:
                    break;
            }
        })
    }
}
//Edit TextBox value changed in Editdatatable
function EdittextBoxValue(thisObj, textBoxCode) {
    debugger;
    var IDs = selectedRowIDs();//identify the selected rows 
    var purchaseOrderVM = DataTables.EditPurchaseDetailsTable.rows().data();
    var purchaseOrdertable = DataTables.EditPurchaseDetailsTable;
    var rowtable = purchaseOrdertable.row($(thisObj).parents('tr')).data();
    for (var i = 0; i < purchaseOrderVM.length; i++) {
        if (purchaseOrderVM[i].RequisitionDetail.ID == rowtable.RequisitionDetail.ID) {
            if (textBoxCode == 1)//textBoxCode is the code to know, which textbox changed is triggered
            {
                purchaseOrderVM[i].MaterialDesc = thisObj.value;
            }
            if (textBoxCode == 2) {
                if ((thisObj.value != "") && (thisObj.value != 0))
                purchaseOrderVM[i].Rate = parseFloat(thisObj.value);
            }
            if (textBoxCode == 3) {
                if (thisObj.value != "")
                    purchaseOrderVM[i].Discount = parseFloat(thisObj.value);
                else
                    purchaseOrderVM[i].Discount = 0;
            }
            if (textBoxCode == 5)
            {
                var taxTypeVM = GetTaxtypeDropdown();
                purchaseOrderVM[i].TaxTypeCode = thisObj.value;
                for (j = 0; j < taxTypeVM.length; j++) {
                    if (taxTypeVM[j].Code == thisObj.value) {
                        purchaseOrderVM[i].IGSTPerc = taxTypeVM[j].IGSTPercentage;
                        purchaseOrderVM[i].SGSTPerc = taxTypeVM[j].SGSTPercentage;
                        purchaseOrderVM[i].CGSTPerc = taxTypeVM[j].CGSTPercentage;
                    }
                }
            }
            if (textBoxCode == 6) {
                if ((thisObj.value != "") && (thisObj.value != 0))
                    purchaseOrderVM[i].Qty = parseFloat(thisObj.value);
            }
        }
    }
    DataTables.EditPurchaseDetailsTable.clear().rows.add(purchaseOrderVM).draw(false);
}
//Delete PurchaseOrder
function DeleteClick() {
    debugger;
    notyConfirm('Are you sure to delete?', 'DeletePurchaseOrder()');
}
function DeletePurchaseOrder() {
    try {
        debugger;
        var id = $('#ID').val();
        if (id != '' && id != null) {
            var data = { "ID": id };
            var result = "";
            var message = "";
            var jsonData = {};
            jsonData = GetDataFromServer("PurchaseOrder/DeletePurchaseOrder/", data);
            if (jsonData != '') {
                jsonData = JSON.parse(jsonData);
                result = jsonData.Result;
                message = jsonData.Message;
            }
            if (result == "OK") {
                notyAlert('success', message);
                window.location.replace("NewPurchaseOrder?code=PURCH");
            }
            if (result == "ERROR") {
                alert(message);
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
//Delete PurchaseOrderDetail
function Delete(curobj) {
    debugger;
    var rowData = DataTables.PurchaseOrderDetailTable.row($(curobj).parents('tr')).data();
    var Rowindex = DataTables.PurchaseOrderDetailTable.row($(curobj).parents('tr')).index();
    _SlNo = 1;
    if ((rowData != null) && (rowData.ID != null)) {
        notyConfirm('Are you sure to delete?', 'DeleteItem("' + rowData.ID + '")');
    }
    else {
        var res = notyConfirm('Are you sure to delete?', 'DeleteTempItem("' + Rowindex + '")');

    }
}

function DeleteTempItem(Rowindex) {
    debugger;
    _SlNo = 1;
    DataTables.PurchaseOrderDetailTable.row(Rowindex).remove().draw(false);
    notyAlert('success', 'Deleted successfully');
}
function DeleteItem(ID) {

    try {
        debugger;
        var data = { "ID": ID };
        var ds = {};
        ds = GetDataFromServer("PurchaseOrder/DeletePurchaseOrderDetail/", data);
        if (ds != '') {
            ds = JSON.parse(ds);
        }
        switch (ds.Result) {
            case "OK":
                notyAlert('success', ds.Message);
                BindPurchaseOrder($('#ID').val());
                break;
            case "ERROR":
                notyAlert('error', ds.Message);
                break;
            default:
                break;
        }
        return ds.Record;
    }
    catch (e) {

        notyAlert('error', e.message);
    }
}
//Email Sending
function EmailPreview(flag) {
    try {
        debugger;
        var QHID = $("#ID").val();
        if (QHID) {
            //Bind mail html into model
            GetMailPreview(QHID, flag);

            $("#MailPreviewModel").modal('show');
            $('#btnMail').show(); 
            $('#btnMailSend').hide();
            $('#btnSend').hide();
            $('#btnMailBackward').hide();
        }
    }
    catch (e) {
        notyAlert('error', e.Message);
    }

}

function GetMailPreview(ID,flag) {
    debugger;
    var data = { "ID": ID, "flag": flag };
    var jsonData = {};
    jsonData = GetDataFromServer("PurchaseOrder/GetMailPreview/", data);
    if (jsonData == "Nochange") {
        return; 0
    }
    debugger;
    $("#mailmodelcontent").empty();
    $("#mailmodelcontent").html(jsonData);
    $("#mailBodyText").val(jsonData);

}

function SendMailPreview() {
    debugger;
    PurchaseOrderViewModel.ID = $('#ID').val();
    PurchaseOrderViewModel.MailBodyHeader = $('#PurchaseOrder_MailBodyHeader').val();
    PurchaseOrderViewModel.MailBodyFooter = $('#PurchaseOrder_MailBodyFooter').val();
    if(PurchaseOrderViewModel.MailBodyHeader || PurchaseOrderViewModel.MailBodyFooter )
        SaveHeaderDetail()
    EmailPreview(0);
    if ($('#LatestApprovalStatus').val() == 4) {
        $('#btnMailSend').show();
        $('#btnSend').show();
        $('#btnMailBackward').show();
        $('#btnMail').hide();
    }
    else {
        $('#btnMailSend').show();
        $('#btnMailBackward').show();
        $('#btnSend').show();
        $('#btnMail').hide();
        $("#btnMailSend").attr("disabled", true);
        $("#btnSend").attr("disabled", true);
        $("#btnMailSend").attr('title', "Document not approved");
        $("#btnSend").attr('title', "Document not approved");
        
    }
}

function SaveHeaderDetail()
{
var data = "{'purchaseOrderVM':" + JSON.stringify(PurchaseOrderViewModel) + "}";

PostDataToServer("PurchaseOrder/UpdatePOMailDetails/", data, function (JsonResult) {
    debugger;
    switch (JsonResult.Result) {
        case "OK":
            if (JsonResult.Records.ID) {
                $("#ID").val(JsonResult.Records.ID);
            }
            break;
        case "Error":
            notyAlert('error', JsonResult.Message);
            break;
        case "ERROR":
            notyAlert('error', JsonResult.Message);
            break;
        default:
            break;
    }
})
}

function SendMailClick() {
    debugger;
    if ($('#LatestApprovalStatus').val() == 4) {
        $('#btnFormSendMail').trigger('click');
        $('#btnMail').hide();
    }
}

function ValidateEmail() {
    debugger;
    var ste = $('#PurchaseOrderMailPreview_SentToEmails').val();
    if (ste) {
        var atpos = ste.indexOf("@");
        var dotpos = ste.lastIndexOf(".");
        if (atpos < 1 || dotpos < atpos + 2 || dotpos + 2 >= ste.length) {
            notyAlert('error', 'Invalid email');
            return false;
        }
            //not valid

        else {
            $("#MailPreviewModel").modal('hide');
            OnServerCallBegin();
            return true;
        }

    }

    else
        notyAlert('error', 'Enter email address');
    return false;
}

function MailSuccess(data, status) {
    debugger;
    hideLoader();
    var JsonResult = JSON.parse(data)
    switch (JsonResult.Result) {
        case "OK":
            notyAlert('success', JsonResult.Message);
            OnServerCallComplete();
            //if (JsonResult.Record.Status == "1") {
                $("#lblEmailStatus").text('Yes');
            //}
            //else {
               
            //}
           
            Reset();
            break;
        case "ERROR":
            notyAlert('error', JsonResult.Message);
            $("#lblEmailStatus").text('No');
            break;
        default:
            notyAlert('error', JsonResult.Message);
            break;
    }
}
//To trigger PDF download button
function DownloadPDF() {
    debugger;
    if ($('#LatestApprovalStatus').val() == 4) {
        GetHtmlData();
        $('#btnSendDownload').trigger('click');
    }
}

//To download file in PDF
function GetHtmlData() {
    debugger;
    var bodyContent = $('#mailmodelcontent').html();
    var headerContent = $('#hdnHeadContent').html();
    $('#hdnContent').val(bodyContent);
    $('#hdnHeadContent').val(headerContent);
    var customerName = $("#SupplierID option:selected").text();
    $('#hdnCustomerName').val(customerName);

}

//For approval
function ShowSendForApproval(documentTypeCode) {
    debugger;
    if ($('#LatestApprovalStatus').val() == 3) {
        var documentID = $('#ID').val();
        var latestApprovalID = $('#LatestApprovalID').val();
        ReSendDocForApproval(documentID, documentTypeCode, latestApprovalID);
        BindPurchaseOrder($('#ID').val());
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
    BindPurchaseOrder($('#ID').val());
}