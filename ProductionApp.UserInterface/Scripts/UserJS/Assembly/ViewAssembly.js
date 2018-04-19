//*****************************************************************************
//*****************************************************************************
//Author: Angel
//CreatedDate: 16-Apr-2018 
//LastModified: 16-Apr-2018
//FileName: Employee.js
//Description: Client side coding for ViewAssembly
//******************************************************************************
// ##1--Global Declaration
// ##2--Document Ready Function
// ##3--Assembly Table Binding Function
//******************************************************************************
//##1--Global Declaration---------------------------------------------##1 
var _dataTable = {};
var _emptyGuid = "00000000-0000-0000-0000-000000000000";
//##2--Document Ready Function----------------------------------------##2
$(document).ready(function () {
    debugger;
    try {
        $("#ProductID").select2({
        });
        $("#AssembleBy").select2({
        });
        BindOrReloadAssembleTable('Init');
    }
    catch (e) {
        console.log(e.message);
    }
});
//##3--Assembly Table Binding Function-------------------------------------##3
function BindOrReloadAssembleTable(action) {
    try {
        debugger;
        assemblyAdvanceSearchVM = new Object();
        DataTablePagingViewModel = new Object();
        ProductViewModel = new Object();
        DataTablePagingViewModel.Length = 0;
        //switch case to check the operation
        switch (action) {
            case 'Reset':
                $('#SearchTerm').val('');
                $('#FromDate').val('');
                $('#ToDate').val('');
                $('#ProductID').val('').select2();
                $('#AssembleBy').val('').select2();
                break;
            case 'Init':
                break;
            case 'Search':
                break;
            case 'Apply':
                assemblyAdvanceSearchVM.FromDate = $('#FromDate').val();
                assemblyAdvanceSearchVM.ToDate = $('#ToDate').val();
                assemblyAdvanceSearchVM.ProductID = $('#ProductID').val();
                assemblyAdvanceSearchVM.AssembleBy = $('#AssembleBy').val();
                break;
            case 'Export':
                DataTablePagingViewModel.Length = -1;
                break;
            default:
                break;
        }
        assemblyAdvanceSearchVM.DataTablePaging = DataTablePagingViewModel;
        assemblyAdvanceSearchVM.SearchTerm = $('#SearchTerm').val();
        assemblyAdvanceSearchVM.FromDate = $('#FromDate').val();
        assemblyAdvanceSearchVM.ToDate = $('#ToDate').val();
        ProductViewModel.ID = $("#ProductID").val();
        assemblyAdvanceSearchVM.Product = ProductViewModel;
        assemblyAdvanceSearchVM.AssembleBy = $('#AssembleBy').val();
        _dataTable.Assemble = $('#tblAssemble').DataTable(
            {
                dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
                buttons: [{
                    extend: 'excel',
                    exportOptions:
                                 {
                                     columns: [0,1,2,3]
                                 }
                }],
                order: false,
                ordering: false,
                searching: false,
                paging: true,
                lengthChange: false,
                proccessing: true,
                serverSide: true,
                ajax: {
                    url: "GetAllAssembly/",
                    data: { "assemblyAdvanceSearchVM": assemblyAdvanceSearchVM },
                    type: 'POST'
                },
                pageLength: 10,
                columns: [
                    { "data": "EntryNo", "defaultContent": "<i>-</i>", "width": "7%" },
                    { "data": "AssemblyDateFormatted", "defaultContent": "<i>-</i>", "width": "13%" },
                    { "data": "Product.Name", "defaultContent": "<i>-</i>", "width": "37%" },
                    { "data": "Employee.Name", "defaultContent": "<i>-</i>", "width": "30%" },
                    { "data": "Qty", "defaultContent": "<i>-</i>", "width": "10%" },
                    {
                        "data": "ID", "orderable": false, render: function (data, type, row) {
                            return '<a href="/Assembly/NewAssembly?code=PROD&ID=' + data + '" class="actionLink" ><i class="glyphicon glyphicon-edit" aria-hidden="true"></i></a>'
                        }, "defaultContent": "<i>-</i>", "width": "3%"
                    }
                ],
                columnDefs: [
                            { className: "text-left", "targets": [2, 3,0 ] },
                            { className: "text-right", "targets": [4] },
                            { className: "text-center", "targets": [1,5] }],
                destroy: true,
                //for performing the import operation after the data loaded
                initComplete: function (settings, json) {
                    if (action === 'Export') {
                        debugger;
                        if (json.data[0].length > 0) {
                            if (json.data[0].TotalCount > 10000) {
                                MasterAlert("info", 'We are able to download maximum 10000 rows of data, There exist more than 10000 rows of data please filter and download')
                            }
                        }
                        $(".buttons-excel").trigger('click');
                        BindOrReloadAssembleTable('Search');
                    }
                }
            });
        $(".buttons-excel").hide();
    }

    catch (e) {
        console.log(e.message);
    }
}