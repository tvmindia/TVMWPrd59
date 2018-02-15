var DataTables = {};
var EmptyGuid = "00000000-0000-0000-0000-000000000000";
//-------------------------------------------------------------
$(document).ready(function () {
    debugger;
    try
    {
        BindOrReloadBankTable('Init');        
    }
    catch(e)
    {
        console.log(e.message);
    }    
});
//function bind the bank list checking search and filter
function BindOrReloadBankTable(action) {
    try
    {
        //creating advancesearch object
        BankAdvanceSearchViewModel = new Object();
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
        BankAdvanceSearchViewModel.DataTablePaging = DataTablePagingViewModel;
        BankAdvanceSearchViewModel.SearchTerm = $('#SearchTerm').val();
        
        //apply datatable plugin on bank table
        DataTables.BankList = $('#tblBank').DataTable(
        {
            dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
            buttons: [{
                extend: 'excel',
                exportOptions:
                             {
                                 columns: [0, 1]
                             }
            }],
            order:false,
            searching: false,
            paging: true,
            lengthChange: false,
            proccessing: true,
            serverSide: true,
            ajax: {
                url: "Bank/GetAllBank/",
                data: { "bankAdvanceSearchVM": BankAdvanceSearchViewModel },
                type: 'POST'
            },
            pageLength: 10,
            columns: [
            { "data": "Code", "defaultContent": "<i>-</i>" },
            { "data": "Name", "defaultContent": "<i>-</i>" },
            {
                "data": "ActualODLimit", render: function (data, type, row) {
                    if (data == 0)
                        return '-'
                    else
                        return roundoff(data, 1);
                },
                "defaultContent": "<i>-</i>"
            },
            {
                "data": "DisplayODLimit", render: function (data, type, row) {
                    if (data == 0)
                        return '-'
                    else
                        return roundoff(data, 1);
                }, "defaultContent": "<i>-</i>"
            },
            { "data": null, "orderable": false, "defaultContent": '<a href="#" onclick="EditBankMaster(this)"<i class="glyphicon glyphicon-share-alt" aria-hidden="true"></i></a>' }
            ],
            columnDefs: [{ "targets": [], "visible": false, "searchable": false },
                { className: "text-right", "targets": [3, 4] },
                { className: "text-left", "targets": [1, 2, 3] },
                { className: "text-center", "targets": [0] }],           
            destroy: true,
            //for performing the import operation after the data loaded
            initComplete: function (settings, json) {
                if (action === 'Export')
                {
                    $(".buttons-excel").trigger('click');
                    ResetBankList();
                }
            }
        });
        $(".buttons-excel").hide();
    }
    catch(e)
    {
        console.log(e.message);
    }    
}
//function reset the list to initial
function ResetBankList()
{
    BindOrReloadBankTable('Reset');
}
//function export data to excel
function ExportBankData()
{
    BindOrReloadBankTable('Export');
}
//add bank 
function AddBankMaster()
{
    GetMasterPartial("Bank", "");
    $('#h3ModelMasterContextLabel').text('Add Bank')
    $('#divModelMasterPopUp').modal('show');
}
//edit bank 
function EditBankMaster(this_obj) {
    debugger;
    rowData = DataTables.BankList.row($(this_obj).parents('tr')).data();
    GetMasterPartial("Bank", rowData.Code);
    $('#h3ModelMasterContextLabel').text('Edit Bank')
    $('#divModelMasterPopUp').modal('show');
}
function SaveSuccessBank(data, status)
{
    debugger;
    var JsonResult = JSON.parse(data)
    switch (JsonResult.Result) {
        case "OK":
            $('#IsUpdate').val('True');
            BindOrReloadBankTable('Reset');
            $.notify({
                title: 'Success',
                message: JsonResult.Records.Message
            }, {
                type: 'pastel-success',
                allow_dismiss: false,
                placement: {
                    from: 'top',
                    align: 'right'
                },
                z_index: 21031,
                delay: 5000,
                template: '<div data-notify="container" class="col-xs-11 col-sm-3 alert alert-{0}" role="alert">' +
                    '<span data-notify="title">{1}</span>' +
                    '<span data-notify="message">{2}</span>' +
                '</div>'
            });
            //$.notify(JsonResult.Records.Message, {                                    
            //    allow_dismiss: false,
            //    placement: {
            //        from: 'bottom',
            //        align: 'left'
            //    },
            //    z_index: 21031,
            //    delay: 5000,
            //    type: 'success',
            //    animate: {
            //                enter: 'animated fadeInDown',
            //                exit: 'animated fadeOutUp'
            //            },
            //});

            //notyAlert('success', JsonResult.Records.Message);           
            break;
        case "ERROR":
            $.notify(JsonResult.Message, {
                offset: {
                    x: 390,
                    y: 70
                },
                z_index: 21031,
                delay: 5000,
                type: 'error',
                animate: {
                    enter: 'animated fadeInDown',
                    exit: 'animated fadeOutUp'
                },
            });
            //notyAlert('error', JsonResult.Message);
            break;
        default:
            $.notify(JsonResult.Message, {               
                allow_dismiss: false,
                placement: {
                    from: 'bottom',
                    align: 'left'
                },
                z_index: 21031,
                delay: 5000,
                type: 'error',
                animate: {
                    enter: 'animated fadeInDown',
                    exit: 'animated fadeOutUp'
                },
            });
            //notyAlert('error', JsonResult.Message);
            break;
    }
}