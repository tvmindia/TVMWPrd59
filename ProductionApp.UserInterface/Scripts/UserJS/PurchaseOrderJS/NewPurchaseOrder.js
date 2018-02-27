﻿var emptyGuid = "00000000-0000-0000-0000-000000000000";
var DataTables = {};
var PODDetail = [];
var PODetailViewModel = new Object();
var PODDetailLink = [];
var RequisitionDetailLink = new Object();
var PurchaseOrderViewModel = new Object();
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
            pageLength: 7,
            data: null,
            columns: [
                 { "data": "ID", "defaultContent": "<i>-</i>" },
                 { "data": "ReqID", "defaultContent": "<i>-</i>" },
                 { "data": "MaterialID", "defaultContent": "<i>-</i>" },
                 { "data": null, "defaultContent": "", "width": "5%" },
                 { "data": "ReqNo", "defaultContent": "<i>-</i>", "width": "10%" },
                 { "data": "Material.MaterialCode", "defaultContent": "<i>-</i>" },
                 {
                     "data": "Description", "defaultContent": "<i>-</i>",
                     'render': function (data, type, row) {
                         if (row.Description)
                             Desc = data;
                         else
                             Desc = row.Description;
                         return '<input class="form-control description" name="Markup" value="' + Desc + '" type="text" onchange="textBoxValueChanged(this,1);">';
                     }
                 },
                 {
                     "data": "ApproximateRate", "defaultContent": "<i>-</i>", "width": "10%", 'render': function (data, type, row) {
                         return '<input class="form-control text-right " name="Markup" value="' + row.ApproximateRate + '" type="text" onclick="SelectAllValue(this);" onkeypress = "return isNumber(event)", onchange="textBoxValueChanged(this,2);">';
                     }
                 },
                { "data": "RequestedQty", "defaultContent": "<i>-</i>", "width": "10%" },
                 { "data": "OrderedQty", "defaultContent": "<i>-</i>", "width": "10%" },
                 {
                     "data": "POQty", "defaultContent": "<i>-</i>", "width": "10px",
                     'render': function (data, type, row) {
                         return '<input class="form-control text-right " name="Markup" type="text"  value="' + data + '"  onclick="SelectAllValue(this);" onkeypress = "return isNumber(event)", onchange="textBoxValueChanged(this,4);">';
                     }
                 },
                 {
                     "data": "Taxtype", "defaultContent": "<i>-</i>", "width": "10%",
                     'render': function (data, type, row) {
                         return '<select id="dddl' + row.ID + '" onchange="textBoxValueChanged(this,5);" ><option value="GST18">GST18%</option><option value="GST28">GST28%</option><option value="GST12">GST12%</option></select>';
                     }
                 },
                 {
                     "data": "Discount", "defaultContent": "<i>-</i>",
                     'render': function (data, type, row) {
                         if (data == undefined)
                             data = 0.0;
                         else
                             data = row.Discount;
                         return '<input class="form-control description" name="Markup" value="' + data + '" type="text" onchange="textBoxValueChanged(this,3);">';
                     }
                 },
                 
                 { "data": "RawMaterial.UnitCode", "defaultContent": "<i>-</i>" }

            ],
            columnDefs: [{ orderable: false, className: 'select-checkbox', "targets": 3 }
                , { className: "text-left", "targets": [5, 6] }
                , { className: "text-right", "targets": [7, 8, 9, 10] }
                , { className: "text-center", "targets": [1, 4] }
                , { "targets": [0,1,2,13], "visible": false, "searchable": false }
                , { "targets": [2, 3, 4, 5, 6, 7, 8, 9, 10], "bSortable": false }],

            select: { style: 'multi', selector: 'td:first-child' }
        });
        //PurchaseOrder
          DataTables.PurchaseOrderDetailTable = $('#tblPurchaseOrderDetail').DataTable(
            {
                dom: '<"pull-right"f>rt<"bottom"ip><"clear">',
                ordering: false,
                searching: false,
                paging: false,
                data: null,
                pageLength: 15,
                language: {
                    search: "_INPUT_",
                    searchPlaceholder: "Search"
                },
                columns: [
                  { "data": "ID" },
                  { "data": "MaterialCode", "defaultContent": "<i>-</i>" },
                  { "data": "MaterialDesc", "defaultContent": "<i>-</i>" },
                  { "data": "UnitCode", "defaultContent": "<i>-</i>" },
                  { "data": "Qty", "defaultContent": "<i>-</i>" },
                  { "data": "Rate", "defaultContent": "<i>-</i>" },
                  { "data": "Discount", "defaultContent": "<i>-</i>" },
                  {
                      "data": "TaxableAmount", "defaultContent": "<i>-</i>",
                      'render': function (data, type, row) {
                          Desc = parseFloat(row.Qty) * parseFloat(row.Rate) - parseFloat(row.Discount);
                          return Desc ;
                      }
                  },
                  { "data": "CGSTAmt", "defaultContent": "<i>-</i>" },
                  { "data": "SGSTAmt", "defaultContent": "<i>-</i>" },
                  {
                      "data": "Total", "defaultContent": "<i>-</i>",
                      'render': function (data, type, row) {
                          Desc = (parseFloat(row.Qty) * parseFloat(row.Rate) - parseFloat(row.Discount)) + parseFloat(row.CGSTAmt) + parseFloat(row.SGSTAmt);
                          return roundoff(Desc,1);
                      }
                  },
                { "data": null, "orderable": false, "width": "5%", "defaultContent": '<a href="#" class="ItemEditlink" onclick="EditPurchaseOrderDetailTable(this)"><i class="fa fa-pencil-square-o" aria-hidden="true"></i></a><span> | </span><a href="#" class="ItemEditlink" onclick="Delete(this)"><i class="fa fa-trash-o" aria-hidden="true"></i></a>' }
                ],
                columnDefs: [{ "targets": [0], "visible": false, "searchable": false },
                     { className: "text-right", "targets": [5, 6, 7] },
                      { className: "text-left", "targets": [2, 3, 4] },
                { className: "text-center", "targets": [8] }

                ]
            });
        
        //EditPurchaseOrderDetail
          DataTables.EditPurchaseDetailsTable = $('#tblPurchaseOrderDetailsEdit').DataTable({
              
              dom: '<"pull-left"f>rt<"bottom"ip><"clear">',
              ordering: false,
              searching: true,
              paging: true,
              pageLength: 7,
              data: null,
              columns: [
                   { "data": "PurchaseOrderID", "defaultContent": "<i>-</i>" },
                   { "data": "RequisitionDetail.ReqID", "defaultContent": "<i>-</i>" },
                   { "data": "MaterialID", "defaultContent": "<i>-</i>" },
                   //{ "data": null, "defaultContent": "", "width": "5%" },
                   { "data": "RequisitionDetail.ReqNo", "defaultContent": "<i>-</i>", "width": "10%" },
                   { "data": "MaterialCode", "defaultContent": "<i>-</i>" },
                   {
                       "data": "MaterialDesc", "defaultContent": "<i>-</i>",
                       'render': function (data, type, row) {
                           debugger;
                           if (row.Description)
                               Desc = data;
                           else
                               Desc = row.Description;
                           return '<input class="form-control description" name="Markup" value="' + Desc + '" type="text" onchange="textBoxValueChanged(this,1);">';
                       }
                   },
                   {
                       "data": "Rate", "defaultContent": "<i>-</i>", "width": "10%", 'render': function (data, type, row) {
                           debugger;
                           return '<input class="form-control text-right " name="Markup" value="' + data + '" type="text" onclick="SelectAllValue(this);" onkeypress = "return isNumber(event)", onchange="textBoxValueChanged(this,2);">';
                       }
                   },
                  { "data": "RequisitionDetail.RequestedQty", "defaultContent": "<i>-</i>", "width": "10%" },
                   { "data": "RequisitionDetail.OrderedQty", "defaultContent": "<i>-</i>", "width": "10%" },
                   {
                       "data": "Qty", "defaultContent": "<i>-</i>", "width": "10px",
                       'render': function (data, type, row) {
                           return '<input class="form-control text-right " name="Markup" type="text"  value="' + data + '"  onclick="SelectAllValue(this);" onkeypress = "return isNumber(event)", onchange="textBoxValueChanged(this,4);">';
                       }
                   },
                   {
                       "data": "TaxTypeCode", "defaultContent": "<i>-</i>", "width": "10%",
                       'render': function (data, type, row) {
                           return '<select id="dddl' + row.PurchaseOrderID + '" onchange="textBoxValueChanged(this,5);" ><option value="GST18">GST18%</option><option value="GST28">GST28%</option><option value="GST12">GST12%</option></select>';
                       }
                   },
                   {
                       "data": "Discount", "defaultContent": "<i>-</i>",
                       'render': function (data, type, row) {
                           return '<input class="form-control description" name="Markup" value="' + data + '" type="text" onchange="textBoxValueChanged(this,3);">';
                       }
                   },

                   { "data": "UnitCode", "defaultContent": "<i>-</i>" }

              ],
              columnDefs: [
                     { className: "text-left", "targets": [5, 6] }
                  , { className: "text-right", "targets": [7, 8, 9, 10] }
                  , { className: "text-center", "targets": [1, 4] }
                  , { "targets": [0, 1, 2, 12], "visible": false, "searchable": false }
                  , { "targets": [2, 3, 4, 5, 6, 7, 8, 9, 10], "bSortable": false }],

              select: { style: 'multi', selector: 'td:first-child' }
          });
        //RequisitionListTable
          DataTables.RequisitionListTable = $('#tblRequisitionList').DataTable({
              dom: '<"pull-right"f>rt<"bottom"ip><"clear">',
              ordering: false,
              searching: false,
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
              debugger;
              ChangeButtonPatchView('PurchaseOrder', 'divbuttonPatchAddPurchaseOrder', 'Edit');
          }
        }
        
    
    catch (e) {
        console.log(e.message);
    }
});
function AddPurchaseOrderDetail() {
    debugger;
    $('#RequisitionDetailsModal').modal('show');
    ViewRequisitionList(1);
    BindOrReloadRequisitionListTable();
}
function ViewRequisitionList(value) {
    $('#tabDetail').attr('data-toggle', 'tab');
    $('#btnForward').show();
    $('#btnBackward').hide();
    $('#btnAddPODetails').hide();
    if (value)
        $('#tabList').trigger('click');
}
//bind requistition List
function BindOrReloadRequisitionListTable() {
    var result = GetBindOrReloadRequisitionListTable();
    DataTables.RequisitionListTable.clear().rows.add(result).draw(false);
}
function GetBindOrReloadRequisitionListTable() {
    try {
        debugger;
        var data = {};
        var ds = {};
        ds = GetDataFromServer("PurchaseOrder/GetAllRequisitionForPurchaseOrder/", data);
        if (ds != '') {
            ds = JSON.parse(ds);
        }
        if (ds.Result == "OK") {

            return ds.Records;
        }
        if (ds.Result == "ERROR") {
            notyAlert('error', ds.Message);

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
        var IDs = GetSelectedRowIDs();
        if (IDs) {
            BindGetRequisitionDetailsTable(IDs);
            DataTables.RequisitionDetailsTable.rows().select();
            $('#btnForward').hide();
            $('#btnBackward').show();
            $('#btnAddPODetails').show();
        }
        else {
            $('#tabDetail').attr('data-toggle', '');
            DataTables.RequisitionDetailsTable.clear().draw(false);
            notyAlert('warning', "Please Select Requisition");
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
function BindGetRequisitionDetailsTable(IDs) {
    try {
        debugger;
        var test = GetRequisitionDetailsByIDs(IDs);
        DataTables.RequisitionDetailsTable.clear().rows.add(test).draw(false);
    }
    catch (e) {
        //this will show the error msg in the browser console(F12) 
        console.log(e.message);
    }
}
//RequisitionBinding
function GetRequisitionDetailsByIDs(IDs) {
    try {
        debugger;
        var POID = $('#ID').val();
        var data = { "IDs": IDs, "POID": POID };

        var ds = {};
        ds = GetDataFromServer("PurchaseOrder/GetRequisitionDetailsByIDs/", data);
        if (ds != '') {
            ds = JSON.parse(ds);
        }
        if (ds.Result == "OK") {
            return ds.Records;
        }
        if (ds.Result == "ERROR") {
            notyAlert('error', ds.message);
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
    var allData = DataTables.RequisitionDetailsTable.rows().data();
    var table = DataTables.RequisitionDetailsTable;
    var rowtable = table.row($(thisObj).parents('tr')).data();
    for (var i = 0; i < allData.length; i++) {
        if (allData[i].ID == rowtable.ID) {
            if (textBoxCode == 1)//textBoxCode is the code to know, which textbox changed is triggered
                allData[i].Description = thisObj.value;
            if (textBoxCode == 2)
                allData[i].ApproximateRate = parseFloat(thisObj.value);
            if (textBoxCode == 3)
                allData[i].Discount = parseFloat(thisObj.value);
            if (textBoxCode == 4)
                allData[i].POQty = parseFloat(thisObj.value);
            if (textBoxCode == 5)
                allData[i].Taxtype = $("dddl'"+thisObj.ID+"'").text();
        }
    }
    DataTables.RequisitionDetailsTable.clear().rows.add(allData).draw(false);
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
    var allData = DataTables.RequisitionDetailsTable.rows().data()
    for (var i = 0; i < allData.length; i++) {
        if (IDs.includes(allData[i].ID)) {
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
    //Merging  the rows with same MaterialID
    var allData = DataTables.RequisitionDetailsTable.rows(".selected").data();
    var mergedRows = []; //to store rows after merging
    var currentMaterial, QuantitySum;
    AddRequsitionDetailLink(allData)// adding values to reqDetailLink array function call

    for (var r = 0; r < allData.length; r++) {
        var Particulars="";
        Particulars = allData[r].ReqNo;
        currentMaterial = allData[r].MaterialID
        for (var j = r + 1; j < allData.length; j++) {
            if(allData[j].MaterialID==currentMaterial)
            {
                Particulars = Particulars + "," + allData[j].ReqNo;
                allData[r].POQty = parseFloat(allData[r].POQty) + parseFloat(allData[j].POQty);
                allData.splice(j, 1);//removing duplicate after adding value 
                j = j - 1;// for avoiding skipping row while checking
            }
        }
        allData[r].Particulars =  Particulars
        mergedRows.push(allData[r])// adding rows to merge array
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
    for (var r = 0; r < data.length; r++) {
        PurchaseOrderDetailLink = new Object();
        PurchaseOrderDetailLink.MaterialID = data[r].MaterialID;
        PurchaseOrderDetailLink.ID = emptyGuid; //[PODetailID]
        PurchaseOrderDetailLink.ReqDetailID = data[r].ID;//[ReqDetailID]
        PurchaseOrderDetailLink.ReqID = data[r].ReqID;
        PurchaseOrderDetailLink.PurchaseOrderQty = data[r].POQty;
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
            PODetailViewModel.TaxTypeCode = $("#dddl" + mergedRows[r].ID).val();
            PODetailViewModel.Amount = parseFloat(mergedRows[r].ApproximateRate) * parseFloat(mergedRows[r].POQty);
            if (PODetailViewModel.Discount != null)
                PODetailViewModel.Tax = parseFloat(PODetailViewModel.Amount) - parseFloat(PODetailViewModel.Discount);
            else
                PODetailViewModel.Tax = parseFloat(PODetailViewModel.Amount);
            //Particulars after adding same material(item)
            var result = GetTaxTypeByCode(PODetailViewModel.TaxTypeCode);
            PODetailViewModel.CGSTAmt = parseFloat(PODetailViewModel.Tax) * parseFloat(parseFloat(result.CGSTPercentage) / 100);
            PODetailViewModel.SGSTAmt = parseFloat(PODetailViewModel.Tax) * parseFloat(parseFloat(result.SGSTPercentage) / 100);
            PODetailViewModel.Total = parseFloat(PODetailViewModel.Tax) + parseFloat(PODetailViewModel.CGSTAmt) + parseFloat(PODetailViewModel.SGSTAmt);
            PODDetail.push(PODetailViewModel);
        }
        return true;
    }
    else {
        notyAlert('warning', "Please Select Requisition");
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
    var allData = DataTables.PurchaseOrderDetailTable.rows().data();
    var GrossAmount = 0;
    var ItemTotal = 0;
    var CGSTTotal = 0;
    var SGSTTotal = 0;
    var TotalTax = 0;
    for (var i = 0; i < allData.length; i++) {
        ItemTotal = ItemTotal + (parseFloat(allData[i].Qty)*parseFloat(allData[i].Rate)-parseFloat(allData[i].Discount))
        CGSTTotal = CGSTTotal + parseFloat(allData[i].CGSTAmt)
        SGSTTotal = SGSTTotal + parseFloat(allData[i].SGSTAmt)
        GrossAmount = GrossAmount + ((parseFloat(allData[i].Qty) * parseFloat(allData[i].Rate) - parseFloat(allData[i].Discount))+parseFloat(allData[i].CGSTAmt)+parseFloat(allData[i].SGSTAmt))
    }
    TotalTax = CGSTTotal + SGSTTotal
    $('#GrossAmount').val(roundoff(GrossAmount));
    $('#ItemTotal').val(roundoff(ItemTotal));
    $('#CGSTTotal').val(roundoff(CGSTTotal));
    $('#SGSTTotal').val(roundoff(SGSTTotal));
    $('#TaxTotal').val(roundoff(TotalTax));
    //AmountSummary();
}
function OrderStatusChange() {
    if ($("#PurchaseOrderStatus").val() != "")
        $("#lblStatus").text($("#PurchaseOrderStatus option:selected").text());
    else
        $("#lblStatus").text('N/A');
}
function GetTaxTypeByCode(Code) {
    try {
        debugger;
        var data = { "Code": Code };
        var ds = {};
        ds = GetDataFromServer("PurchaseOrder/GetTaxtype/", data);
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
        notyAlert('error', e.message);
    }
}
function Save() {
    debugger;
    //validation main form 
    var $form = $('#PurchaseOrderForm');
    //if ($form.valid()) {
        PurchaseOrderViewModel.ID = $('#ID').val();
        //PurchaseOrderViewModel.hdnFileID = $('#hdnFileID').val();
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
                            //CalculateGrossAmount();
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
    //}
}
//Bind PurchaseOrder
function BindPurchaseOrder(ID) {
    try {
        debugger;
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
            $('#Discount').val(result.Discount);
            $('#lblReqNo').text("PO# :" + result.PurchaseOrderNo);
            PurchaseOrderDetailBindTable() //------binding Details table
            CalculateGrossAmount();
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
function GetPurchaseOrderDetailsByID(ID) {
    try {
        debugger;
        var data = { "ID": ID };
        var ds = {};
        ds = GetDataFromServer("PurchaseOrder/GetPurchaseOrderByID/", data);
        if (ds != '') {
            ds = JSON.parse(ds);
        }
        if (ds.Result == "OK") {

            return ds.Records;
        }
        if (ds.Result == "ERROR") {
            notyAlert('error', ds.Message);

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
        var ds = {};
        ds = GetDataFromServer("PurchaseOrder/GetPurchaseOrderDetailByID/", data);
        if (ds != '') {
            ds = JSON.parse(ds);
        }
        if (ds.Result == "OK") {
            //$('#GrossTotal').val(roundoff(ds.GrossAmount));
            return ds.Records;

        }
        if (ds.Result == "ERROR") {
            notyAlert('error', ds.Message);
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
    var rowData = DataTables.PurchaseOrderDetailTable.row($(curObj).parents('tr')).data();
    $('#EditRequisitionDetailsModal').modal('show');
    EditPurchaseOrderDetailByID(rowData.ID)
    EditSPOdetailID = rowData.ID// to set PODetailID
    
}

function EditPurchaseOrderDetailByID(ID) {
    try {
        debugger;
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
        var ds = {};
        ds = GetDataFromServer("PurchaseOrder/EditPurchaseOrderDetail/", data);
        if (ds != '') {
            ds = JSON.parse(ds);
        }
        if (ds.Result == "OK") {
            return ds.Records;
        }
        if (ds.Result == "ERROR") {
            notyAlert('error', ds.message);
        }
    }
    catch (e) {
        //this will show the error msg in the browser console(F12) 
        console.log(e.message);
    }
}
function EditPODetails() {
    debugger;
    var allData = DataTables.EditPurchaseDetailsTable.rows().data();

    var mergedRows = []; //to store rows after merging

    EditRequsitionDetailLink(allData)// adding to object function call

    for (var r = 0; r < allData.length; r++) {
        for (var j = r + 1; j < allData.length; j++) {
            allData[r].POQty = parseFloat(allData[r].POQty) + parseFloat(allData[j].POQty);
            allData.splice(j, 1);//removing duplicate after adding value 
            j = j - 1;// for avoiding skipping row while checking
        }
        mergedRows.push(allData[r])// adding rows to merge array
    }
    debugger;
    if ((mergedRows) && (mergedRows.length > 0)) {
        for (var r = 0; r < mergedRows.length; r++) {
            PODetailViewModel = new Object();
            PODetailViewModel.MaterialID = mergedRows[r].MaterialID;
            PODetailViewModel.ID = EditSPOdetailID;
            PODetailViewModel.ReqDetailId = mergedRows[r].ID;
            PODetailViewModel.ReqID = mergedRows[r].ReqID;
            PODetailViewModel.MaterialCode = mergedRows[r].Material.MaterialCode;
            PODetailViewModel.MaterialDesc = mergedRows[r].Material.MaterialDesc;
            PODetailViewModel.Qty = mergedRows[r].POQty;
            PODetailViewModel.Rate = mergedRows[r].AppxRate;
            PODetailViewModel.UnitCode = mergedRows[r].UnitCode;
            PODetailViewModel.Particulars = mergedRows[r].Particulars;
            PODetailViewModel.Amount = parseFloat(mergedRows[r].AppxRate) * parseFloat(mergedRows[r].POQty);
            //Particulars after adding same material(item)
            PODDetail.push(PODetailViewModel);
        }
        debugger;
        UpdateDetailLinkSave();
        $('#EditRequisitionDetailsModal').modal('hide');
    }
}
function EditRequsitionDetailLink(data) {
    debugger;
    for (var r = 0; r < data.length; r++) {
        PurchaseOrderDetailLink = new Object();
        PurchaseOrderDetailLink.MaterialID = data[r].MaterialID;
        PurchaseOrderDetailLink.ID = data[r].LinkID;//LinkId
        PurchaseOrderDetailLink.ReqDetailID = data[r].ReqDetailID;//[ReqDetailID]
        PurchaseOrderDetailLink.ReqID = data[r].ReqID;
        PurchaseOrderDetailLink.Qty = data[r].POQty;
        PODDetailLink.push(PurchaseOrderDetailLink);
    }
}
function UpdateDetailLinkSave() {
    debugger;
    //validation main form 
    var $form = $('#SupplierPOForm');
    if ($form.valid()) {
        PurchaseOrderViewModel.ID = $('#ID').val();
        PurchaseOrderViewModel.PODDetail = PODDetail;
        PurchaseOrderViewModel.PODDetailLink = PODDetailLink;
        var data = "{'POViewModel':" + JSON.stringify(PurchaseOrderViewModel) + "}";

        PostDataToServer("PurchaseOrder/UpdatePurchaseOrderDetailLink/", data, function (JsonResult) {

            debugger;
            switch (JsonResult.Result) {
                case "OK":
                    if (JsonResult.Record.Status == "2") {
                        notyAlert('warning', JsonResult.Record.Message);
                        Reset();
                    }
                    else {
                        notyAlert('success', JsonResult.Record.Message);
                        ChangeButtonPatchView('PurchaseOrder', 'divbuttonPatchAddPurchaseOrder', 'Edit');
                        BindPurchaseOrder($("#ID").val());
                        PODDetail = [];
                        PODDetailLink = [];
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
            var ds = {};
            ds = GetDataFromServer("PurchaseOrder/DeletePurchaseOrder/", data);
            if (ds != '') {
                ds = JSON.parse(ds);
            }
            if (ds.Result == "OK") {
                notyAlert('success', ds.Record.Message);
                window.location.replace("NewPurchaseOrder?code=PURCH");
                // $('#').load(controllername/actionname)
            }
            if (ds.Result == "ERROR") {
                notyAlert('error', ds.Message);
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

    if ((rowData != null) && (rowData.ID != null)) {
        notyConfirm('Are you sure to delete?', 'DeleteItem("' + rowData.ID + '")');
    }
    else {
        var res = notyConfirm('Are you sure to delete?', 'DeleteTempItem("' + Rowindex + '")');

    }
}

function DeleteTempItem(Rowindex) {
    debugger;
    //var Itemtabledata = DataTables.RequisitionDetailList.rows().data();
    //Itemtabledata.splice(Rowindex, 1);
    //DataTables.RequisitionDetailList.clear().rows.add(Itemtabledata).draw(false);
    DataTables.PurchaseOrderDetailTable.row(Rowindex).remove().draw(false);
    notyAlert('success', 'Deleted Successfully');
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