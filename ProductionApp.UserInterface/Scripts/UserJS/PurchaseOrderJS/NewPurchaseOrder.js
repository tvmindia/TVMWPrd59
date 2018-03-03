var emptyGuid = "00000000-0000-0000-0000-000000000000";
var DataTables = {};
var PODDetail = [];
var PODetailViewModel = new Object();
var PODDetailLink = [];
var RequisitionDetailLink = new Object();
var PurchaseOrderViewModel = new Object();
var EditPOdetailID;
var ECGST, ESGST, ETotal;
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
                { "data": "RequestedQty", "defaultContent": "<i>-</i>", "width": "10%" },
                 { "data": "OrderedQty", "defaultContent": "<i>-</i>", "width": "10%" },
                 {
                     "data": "POQty", "defaultContent": "<i>-</i>", "width": "10px",
                     'render': function (data, type, row) {
                         return '<input class="form-control text-right " name="Markup" type="text"  value="' + data + '"  onclick="SelectAllValue(this);" onkeypress = "return isNumber(event)", onchange="textBoxValueChanged(this,4);">';
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
                paging: false,
                data: null,
                pageLength: 15,
                language: {
                    search: "_INPUT_",
                    searchPlaceholder: "Search"
                },
                columns: [
                  { "data": "ID" },
                  { "data": "MaterialCode", "defaultContent": "<i>-</i>", "width": "5%" },
                  { "data": "MaterialDesc", "defaultContent": "<i>-</i>" },
                  { "data": "UnitCode", "defaultContent": "<i>-</i>", "width": "3%" },
                  { "data": "Qty", "defaultContent": "<i>-</i>", "width": "4%" },
                  {
                      "data": "Rate", "defaultContent": "<i>-</i>",
                      'render': function (data, type, row) {
                          return roundoff(data, 1);
                      }, "width": "8%"
                  },
                  {
                      "data": "Discount", "defaultContent": "<i>-</i>",
                      'render': function (data, type, row) {
                          if (data == 0)
                              {
                              data = 0.0;
                              return data;
                          }
                          else
                              {
                              data = row.Discount;
                              return roundoff(data, 1);
                          }
                      }, "width": "6%"
                  },
                  {
                      "data": "TaxableAmount", "defaultContent": "<i>-</i>",
                      'render': function (data, type, row) {
                          Desc = parseFloat(row.Qty) * parseFloat(row.Rate) - parseFloat(row.Discount);
                          return roundoff(Desc, 1);
                      }, "width": "10%"
                  },
                  {
                      "data": "CGSTAmt", "defaultContent": "<i>-</i>",
                      'render': function (data, type, row) {
                          return roundoff(data, 1);
                      }, "width": "6%"
                  },
                  {
                      "data": "SGSTAmt", "defaultContent": "<i>-</i>",
                      'render': function (data, type, row) {
                          return roundoff(data, 1);
                      }, "width": "6%"
                  },
                  {
                      "data": "Total", "defaultContent": "<i>-</i>",
                      'render': function (data, type, row) {
                          Desc = (parseFloat(row.Qty) * parseFloat(row.Rate) - parseFloat(row.Discount)) + parseFloat(row.CGSTAmt) + parseFloat(row.SGSTAmt);
                          return roundoff(Desc,1);
                      }, "width": "10%"
                  },
                { "data": null, "orderable": false, "width": "6%", "defaultContent": '<a href="#" class="ItemEditlink" onclick="EditPurchaseOrderDetailTable(this)"><i class="glyphicon glyphicon-pencil" aria-hidden="true"></i></a>  |  <a href="#" class="DeleteLink" onclick="Delete(this)"><i class="glyphicon glyphicon-trash" aria-hidden="true"></i></a>' }
                ],
                columnDefs: [{ "targets": [0], "visible": false, "searchable": false },
                     { className: "text-right", "targets": [5, 6, 7,8,9,10] },
                      { className: "text-left", "targets": [2, 3, 4] },
                { className: "text-center", "targets": [8] }

                ]
            });
        
        //EditPurchaseOrderDetail
            
          DataTables.EditPurchaseDetailsTable = $('#tblPurchaseOrderDetailsEdit').DataTable({

                  dom: '<"pull-left"f>rt<"bottom"ip><"clear">',
                  ordering: false,
                  searching: true,
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
                           return '<input class="form-control description" name="Markup" value="' + Desc + '" type="text" onchange="EdittextBoxValue(this,1);">';
                       }
                   },
                           {
                          "data": "Rate", "defaultContent": "<i>-</i>", "width": "10%", 'render': function (data, type, row) {
                              return '<input class="form-control text-right " name="Markup" value="' + roundoff(data,1) + '" type="text" onclick="SelectAllValue(this);" onkeypress = "return isNumber(event)", onchange="EdittextBoxValue(this,2);">';
                       }
                           },
                           {
                               "data": "TaxTypeCode", "defaultContent": "<i>-</i>", "width": "10%",
                               'render': function (data, type, row) {
                                   return '<select id="dddl' + row.RequisitionDetail.ID + '" onchange="EdittextBoxValue(this,5);" ><option value="GST18">GST18%</option><option value="GST28">GST28%</option><option value="GST12">GST12%</option></select>';
                               }
                           },
                       {
                           "data": "Discount", "defaultContent": "<i>-</i>",
                           'render': function (data, type, row) {
                               return '<input class="form-control description" name="Markup" value="' + roundoff(data,1) + '" type="text" onchange="EdittextBoxValue(this,3);">';
                           }
                       },
                      { "data": "RequisitionDetail.RequestedQty", "defaultContent": "<i>-</i>", "width": "10%"},
                    {"data": "RequisitionDetail.OrderedQty", "defaultContent" : "<i>-</i>", "width": "10%" },
                     {
                        "data": "Qty", "defaultContent": "<i>-</i>", "width": "10px",
                         'render': function (data, type, row) {
                             return '<input class="form-control text-right " name="Markup" type="text"  value="' + data + '"  onclick="SelectAllValue(this);" onkeypress = "return isNumber(event)", onchange="EdittextBoxValue(this,6);">';
                           }
                           }
                               
                       ],
                  columnDefs: [
                    { className: "text-left", "targets": [5, 6]}
                  , { className: "text-right", "targets": [7, 8, 9, 11]}
                  , {className: "text-center", "targets": [1, 4]}
                  , {"targets": [0, 1, 2,3], "visible": false, "searchable": false}],     
                  rowCallback: function (row, data, index) {
                      setTimeout(function () {
                          //your code to be executed after 1 second
                          $('#dddl' + data.RequisitionDetail.ID).val(data.TaxTypeCode);
                      }, 1000);
                      
                  },
                 
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
    var $form = $('#PurchaseOrderForm');
   if ($form.valid()) {
        $('#RequisitionDetailsModal').modal('show');
        ViewRequisitionList(1);
        BindRequisitionListTable();
    }
    else {
        notyAlert('warning', "Please Fill Required Fields,To Add Items ");
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
//bind requistition List
function BindRequisitionListTable() {
    var result = GetBindRequisitionListTable();
    DataTables.RequisitionListTable.clear().rows.add(result).draw(false);
}
function GetBindRequisitionListTable() {
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
            BindRequisitionDetailsTable(IDs);
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
function BindRequisitionDetailsTable(IDs) {
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
                allData[i].Taxtype = $("dddl'" + thisObj.ID + "'").text();
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
                allData[r].Discount = parseFloat(allData[r].Discount) + parseFloat(allData[j].Discount);
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
        PurchaseOrderDetailLink.TaxTypeCode = $("#dddl" + data[r].ID).val();
        PurchaseOrderDetailLink.Amount = parseFloat(data[r].ApproximateRate) * parseFloat(data[r].POQty);
        if (PurchaseOrderDetailLink.Discount != null)
            PurchaseOrderDetailLink.Tax = parseFloat(PurchaseOrderDetailLink.Amount) - parseFloat(PurchaseOrderDetailLink.Discount);
        else
            PurchaseOrderDetailLink.Tax = parseFloat(PurchaseOrderDetailLink.Amount);
        //Particulars after adding same material(item)
        var result = GetTaxTypeByCode(PurchaseOrderDetailLink.TaxTypeCode);
        PurchaseOrderDetailLink.CGSTAmt = parseFloat(PurchaseOrderDetailLink.Tax) * parseFloat(parseFloat(result.CGSTPercentage) / 100);
        PurchaseOrderDetailLink.SGSTAmt = parseFloat(PurchaseOrderDetailLink.Tax) * parseFloat(parseFloat(result.SGSTPercentage) / 100);
        PurchaseOrderDetailLink.Total = parseFloat(PurchaseOrderDetailLink.Tax) + parseFloat(PurchaseOrderDetailLink.CGSTAmt) + parseFloat(PurchaseOrderDetailLink.SGSTAmt);
        ECGST = ECGST + parseFloat(PurchaseOrderDetailLink.CGSTAmt);
        ESGST = ESGST + parseFloat(PurchaseOrderDetailLink.SGSTAmt);
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
            PODetailViewModel.TaxTypeCode = $("#dddl" + mergedRows[r].ID).val();
            PODetailViewModel.CGSTAmt = ECGST;
            PODetailViewModel.SGSTAmt = ESGST;
            PODetailViewModel.Total = ETotal;
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
    if ($form.valid()) {
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
    $('#EditRequisitionDetailsModal').modal('show');
    var rowData = DataTables.PurchaseOrderDetailTable.row($(curObj).parents('tr')).data();
    EditPurchaseOrderDetailByID(rowData.ID)
    EditPOdetailID = rowData.ID// to set PODetailID
    
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
            allData[r].Qty = parseFloat(allData[r].Qty) + parseFloat(allData[j].Qty);
            allData[r].Discount = parseFloat(allData[r].Discount) + parseFloat(allData[j].Discount);
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
            PODetailViewModel.ID = EditPOdetailID;
            PODetailViewModel.ReqDetailId = mergedRows[r].ID;
            PODetailViewModel.ReqID = mergedRows[r].RequisitionDetail.ReqID;
            PODetailViewModel.MaterialCode = mergedRows[r].MaterialCode;
            PODetailViewModel.MaterialDesc = mergedRows[r].MaterialDesc;
            PODetailViewModel.Qty = mergedRows[r].Qty;
            PODetailViewModel.Rate = mergedRows[r].Rate;
            PODetailViewModel.Particulars = mergedRows[r].Particulars;
            PODetailViewModel.Discount = mergedRows[r].Discount;
            PODetailViewModel.TaxTypeCode = $("#dddl" + mergedRows[r].RequisitionDetail.ID).val();
            PODetailViewModel.CGSTAmt = ECGST;
            PODetailViewModel.SGSTAmt = ESGST;
            PODetailViewModel.Total = ETotal;
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
        PurchaseOrderDetailLink.TaxTypeCode = $("#dddl" + data[r].RequisitionDetail.ID).val();
        //---------------------
        PurchaseOrderDetailLink.Amount = parseFloat(data[r].Rate) * parseFloat(data[r].Qty);
        if (PurchaseOrderDetailLink.Discount != null)
            PurchaseOrderDetailLink.Tax = parseFloat(PurchaseOrderDetailLink.Amount) - parseFloat(PurchaseOrderDetailLink.Discount);
        else
            PurchaseOrderDetailLink.Tax = parseFloat(PurchaseOrderDetailLink.Amount);
        //Particulars after adding same material(item)
        var result = GetTaxTypeByCode(PurchaseOrderDetailLink.TaxTypeCode);
        PurchaseOrderDetailLink.CGSTAmt = parseFloat(PurchaseOrderDetailLink.Tax) * parseFloat(parseFloat(result.CGSTPercentage) / 100);
        PurchaseOrderDetailLink.SGSTAmt = parseFloat(PurchaseOrderDetailLink.Tax) * parseFloat(parseFloat(result.SGSTPercentage) / 100);
        PurchaseOrderDetailLink.Total = parseFloat(PurchaseOrderDetailLink.Tax) + parseFloat(PurchaseOrderDetailLink.CGSTAmt) + parseFloat(PurchaseOrderDetailLink.SGSTAmt);
        ECGST = ECGST +parseFloat( PurchaseOrderDetailLink.CGSTAmt);
        ESGST = ESGST +parseFloat( PurchaseOrderDetailLink.SGSTAmt);
        ETotal = ETotal +parseFloat( PurchaseOrderDetailLink.Total);
        PODDetailLink.push(PurchaseOrderDetailLink);
    }
}
function UpdateDetailLinkSave() {
    debugger;
    var $form = $('#PurchaseOrderForm');
    if ($form.valid()) {
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
    var allData = DataTables.EditPurchaseDetailsTable.rows().data();
    var table = DataTables.EditPurchaseDetailsTable;
    var rowtable = table.row($(thisObj).parents('tr')).data();
    for (var i = 0; i < allData.length; i++) {
        if (allData[i].RequisitionDetail.ID == rowtable.RequisitionDetail.ID) {
            if (textBoxCode == 1)//textBoxCode is the code to know, which textbox changed is triggered
                allData[i].MaterialDesc = thisObj.value;
            if (textBoxCode == 2)
                allData[i].Rate = parseFloat(thisObj.value);
            if (textBoxCode == 3)
                allData[i].Discount = parseFloat(thisObj.value);
            if (textBoxCode == 5)
                allData[i].Taxtype = $("dddl'" + thisObj.ID + "'").text();
            if (textBoxCode == 6)
                allData[i].Qty = parseFloat(thisObj.value);
        }
    }
    DataTables.EditPurchaseDetailsTable.clear().rows.add(allData).draw(false);
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
//Email Sending
function EmailPreview() {
    try {
        debugger;

        var QHID = $("#ID").val();
        if (QHID) {
            //Bind mail html into model
            GetMailPreview(QHID);

            $("#MailPreviewModel").modal('show');
        }
    }
    catch (e) {
        notyAlert('error', e.Message);
    }

}

function GetMailPreview(ID) {
    debugger;
    var data = { "ID": ID };
    var ds = {};
    ds = GetDataFromServer("PurchaseOrder/GetMailPreview/", data);
    if (ds == "Nochange") {
        return; 0
    }
    debugger;
    $("#mailmodelcontent").empty();
    $("#mailmodelcontent").html(ds);
    $("#mailBodyText").val(ds);

}

function SendMailClick() {
    debugger;
    $('#btnFormSendMail').trigger('click');
}

function ValidateEmail() {
    debugger;
    var ste = $('#SentToEmails').val();
    if (ste) {
        var atpos = ste.indexOf("@");
        var dotpos = ste.lastIndexOf(".");
        if (atpos < 1 || dotpos < atpos + 2 || dotpos + 2 >= ste.length) {
            notyAlert('error', 'Invalid Email');
            return false;
        }
            //not valid

        else {
            $("#MailPreviewModel").modal('hide');
            showLoader();
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
            if (JsonResult.Record.Status == "1") {
                $("#lblEmailSent").text('Yes');
            }
            else {
                $("#lblEmailSent").text('No');
            }
            Reset();
            break;
        case "ERROR":
            notyAlert('error', JsonResult.Message);
            break;
        default:
            notyAlert('error', JsonResult.Message);
            break;
    }
}
