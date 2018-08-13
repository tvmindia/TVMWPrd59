//*****************************************************************************
//*****************************************************************************
//Author: Arul
//CreatedDate: 08-Mar-2018 
//FileName: ViewBillOfMaterial.js
//Description: Client side coding for Listing BillOfMaterials
//******************************************************************************
//******************************************************************************

//--Global Declaration--//
var DataTables = {};
var EmptyGuid = "00000000-0000-0000-0000-000000000000";
//-------------------------------------------------------------
$(document).ready(function () {
    try {
        BindOrReloadBOMTable('Init');
         
    }
    catch (e) {
        console.log(e.message);
    }
});
//function bind the BillOfMaterial list checking search and filter
function BindOrReloadBOMTable(action) {
    try {
        var i = 0;
        //creating advancesearch object
        BillOfMaterialAdvanceSearchViewModel = new Object();
        DataTablePagingViewModel = new Object();
        DataTablePagingViewModel.Length = 0;
        //switch case to check the operation
        switch (action) {
            case 'Reset':
                $('#SearchTerm').val('');
                break;
            case 'Init':
                break;
            case 'Search':
                break;
            case 'Export':
                DataTablePagingViewModel.Length = -1;
                break;
            default:
                break;
        }
        BillOfMaterialAdvanceSearchViewModel.DataTablePaging = DataTablePagingViewModel;
        BillOfMaterialAdvanceSearchViewModel.SearchTerm = $('#SearchTerm').val();

        //apply datatable plugin on BillOfMaterial table
        DataTables.BillOfMaterialList = $('#tblBillOfMaterial').DataTable(
        {
            dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
            buttons: [{
                extend: 'excel',
                exportOptions:
                             {
                                 columns: [2, 3]
                             }
            }],
            ordering: false,
            searching: false,
            paging: true,
            lengthChange: false,
            processing: true,
            language: {

                "processing": "<div class='spinner'><div class='bounce1'></div><div class='bounce2'></div><div class='bounce3'></div></div>"
            },
            serverSide: true,
            ajax: {
                url: "GetAllBillOfMaterial/",
                data: { "billOfMaterialAdvanceSearchVM": BillOfMaterialAdvanceSearchViewModel },
                type: 'POST'
            },
            pageLength: 10,
            columns: [
            { "data": "ID", "defaultContent": "<i>-</i>" },
            { "data": "Product.Name", "defaultContent": "<i>-</i>" },
            { "data": "Description", "defaultContent": "<i>-</i>"},
            {
                "data": null, render: function (data, type, row) {
                    
                    var componentList = [];
                    for (var i = 0; i < row.BillOfMaterialDetailList.length; i++) {
                        componentList.push("<b><strong> * </strong></b>" +
                            (row.BillOfMaterialDetailList[i].Product.Name !== null ? row.BillOfMaterialDetailList[i].Product.Name : row.BillOfMaterialDetailList[i].Material.Description)
                       + " : " + row.BillOfMaterialDetailList[i].Qty + " Nos")
                    }
                    return componentList;
                },
                "defaultContent": "<i>-</i>"
            },
            {
                "data": null, render: function (data, type, row) {                   
                    debugger;
                    var name = row.Product.Name.replace(" ", "&nbsp;").replace(" ", "&nbsp;").replace(" ", "&nbsp;").replace(" ", "&nbsp;").replace(" ", "&nbsp;").replace(" ", "&nbsp;");
                    return '<span style="color:blue;cursor:pointer" onclick=ViewBOMTree("' + row.Product.ID + '","' + name + '")>View</span>';
                },
                "defaultContent": "<i>-</i>"

            },
            { "data": null, "orderable": false, "defaultContent": '<a href="#" onclick="EditBillOfMaterialMaster(this)"<i class="glyphicon glyphicon-share-alt" aria-hidden="true"></i></a>' ,"width":'3%'}
            ],
            columnDefs: [{ "targets": [0], "visible": false, "searchable": false },
                { className: "text-left", "targets": [3], "width": "55%" },
                { className: "text-left", "targets": [2, 1], "width": '15%' },
                { className: "text-center", "targets": [4] }],
            destroy: true,
            //for performing the import operation after the data loaded
            initComplete: function (settings, json) {
                debugger;
                if (action === 'Export') {
                    if (json.data.length > 0) {
                        if (json.data[0].TotalCount > 10000) {
                            MasterAlert("info", 'We are able to download maximum 10000 rows of data, There exist more than 10000 rows of data please filter and download')
                        }
                    }
                    $(".buttons-excel").trigger('click');
                    BindOrReloadBOMTable('Search');
                }
            }
        });
        $(".buttons-excel").hide();
        $('#tblBillOfMaterial tbody').on('dblclick', 'td', function () {
            EditBillOfMaterialMaster(this);
        });
    }
    catch (e) {
        console.log(e.message);
    }
}
//function reset the list to initial
function Reset() {
    BindOrReloadBOMTable('Reset');
}
//function export data to excel
function Export() {
    BindOrReloadBOMTable('Export');
}

function EditBillOfMaterialMaster(curobj) {
    try{
        var BillOfMaterialViewModel = DataTables.BillOfMaterialList.row($(curobj).parents('tr')).data();
        window.location.replace("/BillOfMaterial/NewBillOfMaterial?code=PROD&id=" + BillOfMaterialViewModel.ID);
    }
    catch (e) {
        console.log(e.message);
    }
}
//<span onclick="ViewBOMTree('ff075b34-684d-4f42-93eb-b4777bf40bdb')">Quick View</span>

function ViewBOMTree(id,name) {
    var data = { "ProductID": id };
    debugger;
    var result = "";
    var message = "";
    $('#bomName').text(name);

    var jsonData = GetDataFromServer("BillOfMaterial/GetBOMTree/", data);
    if (jsonData != '') {
        jsonData = JSON.parse(jsonData);
        result = jsonData.Result;
        message = jsonData.Message;
        
    }

    switch (result) {
        case "OK":
            debugger;

            $('#jstree').parent().html('<div id="jstree"></div>');        

            $('#jstree').jstree({
                'core': {
                    'data': jsonData.Records
                }
            });

            var $treeview = $("#jstree");
            $treeview               
              .on('loaded.jstree', function () {            
              
                  $treeview.jstree('open_all');
              });

          
           

            //$('#data').jstree({
            //    'core': {
            //        'data': jsonData.Result
            //    }
            //});

            $('#bomTreeModal').modal('show');

            break;
        case "ERROR":
            notyAlert('error', message);
            break;
        default:
            break;
    }


  

   


}