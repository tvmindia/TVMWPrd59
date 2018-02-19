var DataTables = {};
$(document).ready(function () {

    try {
        $("#employeeCode").select2({
            placeholder: "Select Employee..",

        });
    
     //   DataTables.ItemDetailTable = $('#tblIssueToProductionDetail').DataTable(
     //{
     //    dom: '<"pull-right"f>rt<"bottom"ip><"clear">',
     //    ordering:false,
     //    searching: false,
     //    paging: false,
     //    data: null,
     //    autoWidth: false,
     //    columns: [
     //    { "data": "ID", "defaultContent": "<i></i>" },
     //    { "data": "MaterialID", "defaultContent": "<i></i>" },
     //    { "data": "MaterialCode", render: function (data, type, row) { return data }, "defaultContent": "<i></i>" },
     //    { "data": "MaterialDesc", render: function (data, type, row) { return data }, "defaultContent": "<i></i>" },
     //    { "data": "UnitCode", render: function (data, type, row) { return data }, "defaultContent": "<i></i>" },
     //    { "data": "Qty", render: function (data, type, row) { return data }, "defaultContent": "<i></i>" },        
     //    { "data": null, "orderable": false, "defaultContent": '<a href="#" class="DeleteLink"  onclick="Delete(this)" ><i class="glyphicon glyphicon-trash" aria-hidden="true"></i></a> | <a href="#" class="actionLink"  onclick="ProductEdit(this)" ><i class="glyphicon glyphicon-share-alt" aria-hidden="true"></i></a>' },
     //    ],
     //    columnDefs: [{ "targets": [0,1], "visible": false, searchable: false },
     //        { "targets": [3], "width": "10%" },                          
     //        {className:"text-left","targets":[2,3,4,5]}
     //    ]
     //});

}
    catch (x) {
        //this will show the error msg in the browser console(F12) 
    console.log(x.message);
}
    
});
function ShowIssueToProductionDetailsModal()
{
    debugger;
    $('#btnAddIssueToProductionItems').modal('show');
}
function AddIssueToProductItem()
{
    debugger;
    $('#btnAddIssueToProductionItems').modal('hide');
}
