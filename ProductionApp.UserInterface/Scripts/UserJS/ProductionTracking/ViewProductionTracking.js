//*****************************************************************************
//*****************************************************************************
//Author: Arul
//CreatedDate: 12-Apr-2018 
//FileName: ViewProductionTracking.js
//Description: Client side coding for Listing ProductionTracking
//******************************************************************************
//******************************************************************************

//--Global Declaration--//
var DataTables = {};
var EmptyGuid = "00000000-0000-0000-0000-000000000000";

$(document).ready(function () {
    try {
        BindOrReloadProductionTrackingTable('Init');
        $('#ProductID,#EmployeeID,#StageID').select2({});
        AddOnRemove();
    }
    catch (ex) {
        console.log(ex.message);
    }
});

//Remove input addon for supplier insert
function AddOnRemove() {
    try {
        debugger;

        $('.input-group-addon').each(function () {
            $(this).parent().css("width", "100%");
            $(this).remove();
        });

    } catch (ex) {
        console.log(ex.message)
    }
}

function BindOrReloadProductionTrackingTable(action) {
    try {
        debugger;
        ProductionTrackingAdvanceSearchViewModel = new Object();
        DataTablePagingViewModel = new Object();
        DataTablePagingViewModel.Length = 0;
        //switch case to check the operation
        switch (action) {
            case 'Reset':
                $('#SearchTerm').val('');
                $('#FromDate').val('');
                $('#ToDate').val('');
                $('#ProductID').val('').trigger('change');
                $('#EmployeeID').val('').trigger('change');
                $('#StageID').val('').trigger('change');
                break;
            case 'Init':
                break;
            case 'Apply':
                break;
            case 'Export':
                DataTablePagingViewModel.Length = -1;
                break;
            default:
                break;
        }
        ProductionTrackingAdvanceSearchViewModel.DataTablePaging = DataTablePagingViewModel;
        ProductionTrackingAdvanceSearchViewModel.SearchTerm = $('#SearchTerm').val();
        ProductionTrackingAdvanceSearchViewModel.FromDate = $('#FromDate').val();
        ProductionTrackingAdvanceSearchViewModel.ToDate = $('#ToDate').val();
        ProductionTrackingAdvanceSearchViewModel.Product = new Object();
        ProductionTrackingAdvanceSearchViewModel.Product.ID = $('#ProductID').val();
        ProductionTrackingAdvanceSearchViewModel.Employee = new Object();
        ProductionTrackingAdvanceSearchViewModel.Employee.ID = $('#EmployeeID').val();
        ProductionTrackingAdvanceSearchViewModel.Stage = new Object();
        ProductionTrackingAdvanceSearchViewModel.Stage.ID = $('#StageID').val();

        //apply datatable plugin on ProductionTracking table
        DataTables.ProductionTrackingList = $('#tblProductionTracking').DataTable(
        {
            dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
            buttons: [{
                extend: 'excel',
                exportOptions:
                             {
                                 columns: [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12]
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
                url: "GetAllProductionTracking/",
                data: { "productionTrackingAdvanceSearchVM": ProductionTrackingAdvanceSearchViewModel },
                type: 'POST'
            },
            pageLength: 10,
            columns: [
            { "data": "ID", "defaultContent": "<i>-</i>" },
            { "data": "EntryDateFormatted", "defaultContent": "<i>-</i>", "width": '7.5%' },//1
            { "data": "Product.Name", "defaultContent": "<i>-</i>" },
            { "data": "Component.Name", "defaultContent": "<i>-</i>" },//3
            { "data": "Stage.Description", "defaultContent": "<i>-</i>" },
            {
                "data": null, render: function (data, type, row) {
                    if (row.SubComponent.Description !== null) {
                        return row.SubComponent.Description
                    } else {
                        return row.OutputComponent.Name
                    }
                }, "defaultContent": "<i>-</i>"
            },//5
            { "data": "Employee.Name", "defaultContent": "<i>-</i>" },
            { "data": "AcceptedQty", "defaultContent": "<i>-</i>" },//7
            { "data": "AcceptedWt", "defaultContent": "<i>-</i>" },
            { "data": "DamagedQty", "defaultContent": "<i>-</i>" },//9
            { "data": "DamagedWt", "defaultContent": "<i>-</i>" },
            { "data": "ProductionRefNo", "defaultContent": "<i>-</i>", "width": '6%' },//11
            { "data": "Remarks", "defaultContent": "<i>-</i>", "width": '8%' },
            
            { "data": null, "orderable": false, "defaultContent": '<a href="#" onclick="Edit(this)"<i class="glyphicon glyphicon-share-alt" aria-hidden="true"></i></a>' ,"width":'3%'}
            ],
            columnDefs: [{ "targets": [0], "visible": false, "searchable": false },
                { className: "text-left", "targets": [7, 8, 9, 10], "width": '5%' },
                { className: "text-right", "targets": [2, 3, 4, 5, 6, 11, 12]},
                { className: "text-center", "targets": [1, 13] }],
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
                    BindOrReloadProductionTrackingTable('Search');
                }
            }
        });
        $(".buttons-excel").hide();

        $('#tblProductionTracking tbody').on('dblclick', 'td', function () {
            Edit(this);
        });

    } catch (ex) {
        console.log(e.message);
    }
}

//function reset the list to initial
function Reset() {
    BindOrReloadProductionTrackingTable('Reset');
}

//function export data to excel
function Export() {
    BindOrReloadProductionTrackingTable('Export');
}

function Edit(curObj){
    try {
        var ProductionTrackingViewModel = DataTables.ProductionTrackingList.row($(curObj).parents('tr')).data();
        window.location.replace("/ProductionTracking/NewProductionTracking?code=PROD&id=" + ProductionTrackingViewModel.ID);
    }
    catch (e) {
        console.log(e.message);
    }
}