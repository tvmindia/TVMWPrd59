
//*****************************************************************************
//*****************************************************************************
//Author: Jais
//CreatedDate:22-Mar-2018 
//LastModified: 22-Mar-2018 
//FileName: ViewCustomer.js
//Description: Client side coding for ViewCustomer
//******************************************************************************
//******************************************************************************

//--Global Declaration--//
var _dataTables = {};
var _emptyGuid = "00000000-0000-0000-0000-000000000000";

$(document).ready(function () {
    debugger;
    try {
      
        BindOrReloadCustomerTable('Init');

        $('#tblCustomer tbody').on('dblclick', 'td', function () {
            Edit(this);
        });
    }
    catch (e) {
        console.log(e.message);
    }
});


//--function bind the Customer list checking search and filter--//
function BindOrReloadCustomerTable(action) {
    try {
        debugger;
        //creating advancesearch object
        CustomerViewModel = new Object();
        DataTablePagingViewModel = new Object();
        CustomerAdvanceSearchViewModel = new Object();
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
        CustomerAdvanceSearchViewModel.DataTablePaging = DataTablePagingViewModel;
        CustomerAdvanceSearchViewModel.SearchTerm = $('#SearchTerm').val();

        //apply datatable plugin on Customer table
        _dataTables.customerList = $('#tblCustomer').DataTable(
        {
            dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
            buttons: [{
                extend: 'excel',
                exportOptions:
                             {
                                 columns: [0,1, 2, 3, 4]
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
                url: "GetAllCustomer/",
                data: { "customerAdvanceSearchVM": CustomerAdvanceSearchViewModel },
                type: 'POST'
            },
            pageLength: 10,
            columns: [
            { "data": "CompanyName", "defaultContent": "<i>-</i>", "width": "10%" },
            { "data": "ContactPerson", "defaultContent": "<i>-<i>", "width": "10%" },
            { "data": "Mobile", "defaultContent": "<i>-<i>", "width": "10%" },
            { "data": "TaxRegNo", "defaultContent": "<i>-<i>", "width": "10%" },
            { "data": "PANNo", "defaultContent": "<i>-<i>", "width": "10%" },
             {
                 "data": "ID", "orderable": false, render: function (data, type, row) {
                     return '<a href="/Customer/NewCustomer?code=MSTR&ID=' + data + '" class="actionLink" ><i class="glyphicon glyphicon-edit" aria-hidden="true"></i></a>'
                 }, "defaultContent": "<i>-</i>","width":"3%"
             }
            ],
            columnDefs: [{ "targets": [], "visible": false, "searchable": false },
                { className: "text-right", "targets": [] },
                { className: "text-left", "targets": [0,1, 2, 3, 4] },
                { className: "text-center", "targets": [5] }],
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
                    BindOrReloadCustomerTable('Search');
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
function ResetCustomerList() {
    BindOrReloadCustomerTable('Reset');
}
//function export data to excel
function ImportCustomerData() {
    BindOrReloadCustomerTable('Export');
}