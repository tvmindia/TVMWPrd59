var DataTables = {};
var EmptyGuid = "00000000-0000-0000-0000-000000000000";
//-------------------------------------------------------------
$(document).ready(function () {
    debugger;
    try {
        debugger;
        BindOrReloadRawMaterialTable('Init');
    }
    catch (e) {
        console.log(e.message);
    }
});
//function bind the Raw Material list checking search and filter
function BindOrReloadRawMaterialTable(action) {
    try {
        debugger;
        //creating advancesearch object
        RawMaterialAdvanceSearchViewModel = new Object();
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
        RawMaterialAdvanceSearchViewModel.DataTablePaging = DataTablePagingViewModel;
        RawMaterialAdvanceSearchViewModel.SearchTerm = $('#SearchTerm').val();

        //apply datatable plugin on Raw Material table
        DataTables.rawMaterialList = $('#tblRawMaterial').DataTable(
        {
            dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
            buttons: [{
                extend: 'excel',
                exportOptions:
                             {
                                 columns: [0, 1,2,3,4]
                             }
            }],
            order: false,
            searching: false,
            paging: true,
            lengthChange: false,
            proccessing: true,
            serverSide: true,
            ajax: {
                url: "RawMaterial/GetAllRawMaterial/",
                data: { "rawMaterialAdvanceSearchVM": RawMaterialAdvanceSearchViewModel },
                type: 'POST'
            },
            pageLength: 10,
            columns: [
            { "data": "MaterialCode", "defaultContent": "<i>-</i>" },
            { "data": "Rate", "defaultContent": "<i>-</i>" },
            { "data": "Type", "defaultContent": "<i>-<i>" },
            { "data": "Description", "defaultContent": "<i>-<i>" },
            { "data": "UnitCode", "defaultContent": "<i>-<i>" },
            {
                "data": "Code", "orderable": false, render: function (data, type, row) {
                    return '<a href="/RawMaterial/AddRawMaterial?code=MSTR&ID=' + data + '" class="actionLink" ><i class="glyphicon glyphicon-share-alt" aria-hidden="true"></i></a>'
                }, "defaultContent": "<i>-</i>"
            }
            ],
            columnDefs: [{ "targets": [], "visible": false, "searchable": false },
                { className: "text-right", "targets": [1] },
                { className: "text-left", "targets": [0, 2, 3,4,5] },
                { className: "text-center", "targets": [] }],
            destroy: true,
            //for performing the import operation after the data loaded
            initComplete: function (settings, json) {
                if (action === 'Export') {
                    $(".buttons-excel").trigger('click');
                    ResetRawMaterialList();
                }
            }
        });
        $(".buttons-excel").hide();
    }
    catch (e) {
        console.log(e.message);
    }
}

//function reset the list to initial
function ResetRawMaterialList()
{
    BindOrReloadRawMaterialTable('Reset');
}

//function export data to excel
function ImportRawMaterialData()
{
    BindOrReloadRawMaterialTable('Export');
}
