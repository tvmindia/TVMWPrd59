﻿
//*****************************************************************************
//*****************************************************************************
//Author: Jais
//CreatedDate: 13-Feb-2018 
//LastModified: 16-Feb-2018 
//FileName: Material.js
//Description: Client side coding for Material
//******************************************************************************
//******************************************************************************

//--Global Declaration--//
var DataTables = {};
var EmptyGuid = "00000000-0000-0000-0000-000000000000";

//--Loading DOM--//
$(document).ready(function () {
    try {
        debugger;
        BindOrReloadMaterialTable('Init');
    }
    catch (e) {
        console.log(e.message);
    }
});

//--function bind the Raw Material list checking search and filter--//
function BindOrReloadMaterialTable(action) {
    try {
        //creating advancesearch object
        MaterialAdvanceSearchViewModel = new Object();
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
                if ($('#SearchTerm').val() == "")
                DataTablePagingViewModel.Length = -1;
                break;
            default:
                break;
        }
        MaterialAdvanceSearchViewModel.DataTablePaging = DataTablePagingViewModel;
        MaterialAdvanceSearchViewModel.SearchTerm = $('#SearchTerm').val();

        //apply datatable plugin on Raw Material table
        DataTables.materialList = $('#tblMaterial').DataTable(
        {
            dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
            buttons: [{
                extend: 'excel',
                exportOptions:
                             {
                                 columns: [ 1,2,3,4,5,6]
                             }
            }],
            order: false,
            searching: false,
            paging: true,
            lengthChange: false,
            processing: true,
            language: {

                "processing": "<div class='spinner'><div class='bounce1'></div><div class='bounce2'></div><div class='bounce3'></div></div>"
            },
            serverSide: true,
            ajax: {
                url: "Material/GetAllMaterial/",
                data: { "materialAdvanceSearchVM": MaterialAdvanceSearchViewModel },
                type: 'POST'
            },
            pageLength: 10,
            columns: [
            { "data": "ID", "defaultContent": "<i>-</i>" },
            { "data": "MaterialCode", "defaultContent": "<i>-</i>" },
            { "data": "Rate", "defaultContent": "<i>-</i>" },
            { "data": "MaterialType.Description", "defaultContent": "<i>-<i>" },
            { "data": "Description", "defaultContent": "<i>-<i>" },
            { "data": "Unit.Description", "defaultContent": "<i>-<i>" },
            { "data": "ReorderQty", "defaultContent": "<i>-<i>" },
            { "data": null, "orderable": false, "defaultContent": '<a href="#" onclick="EditMaterialMaster(this)"<i class="glyphicon glyphicon-share-alt" aria-hidden="true"></i></a>' }
            ],
            columnDefs: [{ "targets": [0], "visible": false, "searchable": false },
                { className: "text-right", "targets": [2,6] },
                { className: "text-left", "targets": [1, 3,4,5,6] },
                { className: "text-center", "targets": [] }],
            destroy: true,
            //for performing the import operation after the data loaded
            initComplete: function (settings, json) {
                if (action === 'Export') {
                    if (json.data.length > 0) {
                        if (json.data[0].TotalCount > 10000) {
                            MasterAlert("info", 'We are able to download maximum 10000 rows of data, There exist more than 10000 rows of data please filter and download')
                        }
                    }
                    $(".buttons-excel").trigger('click');
                    ResetMaterialList();
                }
            }
        });
        $(".buttons-excel").hide();
    }
    catch (e) {
        console.log(e.message);
    }
}

//--function reset the list to initial--//
function ResetMaterialList()
{
    BindOrReloadMaterialTable('Reset');
}

//--function export data to excel--//
function ImportMaterialData()
{
    BindOrReloadMaterialTable('Export');
}
//--edit Raw material--//
function EditMaterialMaster(this_obj) {
    rowData = DataTables.materialList.row($(this_obj).parents('tr')).data();
    GetMasterPartial("Material", rowData.ID);
    $('#h3ModelMasterContextLabel').text('Edit Material')
    $('#divModelMasterPopUp').modal('show');
    $('#hdnMasterCall').val('MSTR');
}
