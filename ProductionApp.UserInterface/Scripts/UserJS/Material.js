
//*****************************************************************************
//*****************************************************************************
//Author: Jais
//CreatedDate: 13-Feb-2018 
//LastModified: 5-Mar-2018 
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
    $("#Unit_Code").select2({
    });
    $("#MaterialType_Code").select2({
    });
});

//--function bind the Raw Material list checking search and filter--//
function BindOrReloadMaterialTable(action) {
    try {
        debugger;
        //creating advancesearch object
        MaterialAdvanceSearchViewModel = new Object();
        DataTablePagingViewModel = new Object();
        MaterialTypeViewModel = new Object();
        UnitViewModel = new Object();
        DataTablePagingViewModel.Length = 0;
        //switch case to check the operation
        switch (action) {
            case 'Reset':
                $('#SearchTerm').val('');
                $('#MaterialType_Code').val('').select2();
                $('#Unit_Code').val('').select2();
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
        MaterialAdvanceSearchViewModel.DataTablePaging = DataTablePagingViewModel;
        MaterialTypeViewModel.Code = $('#MaterialType_Code').val();
        MaterialAdvanceSearchViewModel.MaterialType = MaterialTypeViewModel;
        UnitViewModel.Code = $('#Unit_Code').val();
        MaterialAdvanceSearchViewModel.Unit = UnitViewModel;
        MaterialAdvanceSearchViewModel.SearchTerm = $('#SearchTerm').val();

        //apply datatable plugin on Raw Material table
        DataTables.materialList = $('#tblMaterial').DataTable(
        {
            dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
            buttons: [{
                extend: 'excel',
                exportOptions:
                             {
                                 columns: [ 0,1,2,3,4,5,6,7,8,9,10]
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
                url: "Material/GetAllMaterial/",
                data: { "materialAdvanceSearchVM": MaterialAdvanceSearchViewModel },
                type: 'POST'
            },
            pageLength: 10,
            columns: [
            //{ "data": "ID", "defaultContent": "<i>-</i>" },
            { "data": "MaterialCode", "defaultContent": "<i>-</i>", "width": "5%" },
            { "data": "MaterialType.Description", "defaultContent": "<i>-<i>", "width": "10%" },
            { "data": "Description", "defaultContent": "<i>-<i>","width":"10%" },
            { "data": "HSNNo", "defaultContent": "<i>-<i>", "width": "10%" },
            { "data": "Unit.Description", "defaultContent": "<i>-<i>", "width": "5%" },
            {
                "data": "Rate", render: function (data, type, row) {
                    if (data == 0)
                        return '-'
                    else
                        return roundoff(data, 1);
                }, "defaultContent": "<i>-</i>", "width": "10%"
            },
            { "data": "ReorderQty", "defaultContent": "<i>-<i>", "width": "5%" },
            { "data": "OpeningStock", "defaultContent": "<i>-<i>", "width": "10%" },
            { "data": "CurrentStock", "defaultContent": "<i>-<i>", "width": "10%" },
            { "data": "WeightInKG", "defaultContent": "<i>-<i>", "width": "10%" },
            {
                "data": "CostPrice", render: function (data, type, row) {
                     if (data == 0)
                         return '-'
                     else
                         return roundoff(data, 1);
                }, "defaultContent": "<i>-</i>", "width": "10%"
            },
            //{ "data": "CostPrice", "defaultContent": "<i>-<i>" },

            { "data": null, "orderable": false, "defaultContent": '<a href="#" onclick="DeleteMaterialMaster(this)"<i class="glyphicon glyphicon-trash" aria-hidden="true"></i></a>  <a href="#" onclick="EditMaterialMaster(this)"<i class="glyphicon glyphicon-edit" aria-hidden="true"></i></a>', "width": "4%" }
            ],
            columnDefs: [{ "targets": [], "visible": false, "searchable": false },
                { className: "text-right", "targets": [5,6,7,8,9,10] },
                { className: "text-left", "targets": [0,1,2, 3,4] },
                { className: "text-center", "targets": [11] }],
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
                    BindOrReloadMaterialTable('Search');
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
    MaterialViewModel = DataTables.materialList.row($(this_obj).parents('tr')).data();
    GetMasterPartial("Material", MaterialViewModel.ID);
    $('#h3ModelMasterContextLabel').text('Edit Material')
    $('#divModelMasterPopUp').modal('show');
    $('#hdnMasterCall').val('MSTR');
}

//--Function To Confirm Material Deletion 
function DeleteMaterialMaster(this_obj) {
    debugger;
    materialVM = DataTables.materialList.row($(this_obj).parents('tr')).data();
    notyConfirm('Are you sure to delete?', 'DeleteMaterial("' + materialVM.ID + '")');
}

//--Function To Delete Material
function DeleteMaterial(id) {
    debugger;
    try {
        if (id) {
            var data = { "id": id };
            var jsonData = {};
            var message = "";
            var status = "";
            var result = "";
            jsonData = GetDataFromServer("Material/DeleteMaterial/", data);
            if (jsonData != '') {
                jsonData = JSON.parse(jsonData);
                message = jsonData.Message;
                status = jsonData.Status;
                result = jsonData.Record;
            }
            switch (status) {
                case "OK":
                    notyAlert('success', result.Message);
                    BindOrReloadMaterialTable('Reset');
                    break;
                case "ERROR":
                    notyAlert('error', message);
                    break;
                default:
                    break;
            }
        }
    }
    catch (e) {
        //this will show the error msg in the browser console(F12) 
        console.log(e.message);
    }
}
