
//*****************************************************************************
//*****************************************************************************
//Author: Jais
//CreatedDate:21-Mar-2018 
//LastModified: 21-Mar-2018 
//FileName: Customer.js
//Description: Client side coding for Customer
//******************************************************************************
//******************************************************************************

//--Global Declaration--//
var DataTables = {};
var EmptyGuid = "00000000-0000-0000-0000-000000000000";

//--Loading DOM--//
$(document).ready(function () {
    try {
        debugger;
        BindOrReloadCustomerTable('Init');
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
        DataTablePagingViewModel = new Object();
        CustomerAdvanceSearchViewModel = new Object();
        DataTablePagingViewModel.Length = 0;
        //switch case to check the operation
        switch (action) {
            case 'Reset':
                $('#CustomerAdvanceSearch_SearchTerm').val('');
                //$('#MaterialTypeCode').val('');
                //$('#UnitCode').val('');
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
        //MaterialTypeViewModel.Code = $('#MaterialTypeCode').val();
        //MaterialAdvanceSearchViewModel.MaterialType = MaterialTypeViewModel;
        //UnitViewModel.Code = $('#UnitCode').val();
        //MaterialAdvanceSearchViewModel.Unit = UnitViewModel;
        CustomerAdvanceSearchViewModel.SearchTerm = $('#CustomerAdvanceSearch_SearchTerm').val();

        //apply datatable plugin on Raw Material table
        DataTables.customerList = $('#tblCustomer').DataTable(
        {
            dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
            buttons: [{
                extend: 'excel',
                exportOptions:
                             {
                                 columns: [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11]
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
                url: "Customer/GetAllCustomer/",
                data: { "customerAdvanceSearchVM": CustomerAdvanceSearchViewModel },
                type: 'POST'
            },
            pageLength: 10,
            columns: [
            { "data": "ID", "defaultContent": "<i>-</i>" },
            { "data": "CompanyName", "defaultContent": "<i>-</i>" },
            { "data": "ContactPerson", "defaultContent": "<i>-<i>" },
            { "data": "ContactEmail", "defaultContent": "<i>-<i>" },
            { "data": "ContactTitle", "defaultContent": "<i>-<i>" },
            { "data": "Website", "defaultContent": "<i>-<i>" },
            { "data": "LandLine", "defaultContent": "<i>-<i>" },
            { "data": "Mobile", "defaultContent": "<i>-<i>" },
            { "data": "Fax", "defaultContent": "<i>-<i>" },
            { "data": "BillingAddress", "defaultContent": "<i>-<i>" },
            { "data": "ShippingAddress", "defaultContent": "<i>-<i>" },
            { "data": "PANNo", "defaultContent": "<i>-<i>" },
            { "data": null, "orderable": false, "defaultContent": '<a href="#" onclick="DeleteCustomerMaster(this)"<i class="glyphicon glyphicon-trash" aria-hidden="true"></i></a>  <a href="#" onclick="EditCustomerMaster(this)"<i class="glyphicon glyphicon-edit" aria-hidden="true"></i></a>', "width": "4%" }
            ],
            columnDefs: [{ "targets": [0], "visible": false, "searchable": false },
                { className: "text-right", "targets": [] },
                { className: "text-left", "targets": [1, 2, 3, 4, 5,6, 7, 8, 9, 10,11] },
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

//--function reset the list to initial--//
function ResetCustomerList() {
    BindOrReloadCustomerTable('Reset');
}

//--function export data to excel--//
function ImportCustomerData() {
    BindOrReloadCustomerTable('Export');
}
//--edit Customer--//
function EditCustomerMaster(this_obj) {
    CustomerViewModel = DataTables.customerList.row($(this_obj).parents('tr')).data();
    GetMasterPartial("Customer", CustomerViewModel.ID);
    $('#h3ModelMasterContextLabel').text('Edit Customer')
    $('#divModelMasterPopUp').modal('show');
    $('#hdnMasterCall').val('MSTR');
}

//--Function To Confirm Customer Deletion 
function DeleteCustomerMaster(this_obj) {
    debugger;
    customerVM = DataTables.customerList.row($(this_obj).parents('tr')).data();
    notyConfirm('Are you sure to delete?', 'DeleteCustomer("' + customerVM.ID + '")');
}

//--Function To Delete Customer
function DeleteCustomer(id) {
    debugger;
    try {
        if (id) {
            var data = { "id": id };
            var jsonData = {};
            var message = "";
            var status = "";
            var result = "";
            jsonData = GetDataFromServer("Customer/DeleteCustomer/", data);
            if (jsonData != '') {
                jsonData = JSON.parse(jsonData);
                message = jsonData.Message;
                status = jsonData.Status;
                result = jsonData.Record;
            }
            switch (status) {
                case "OK":
                    notyAlert('success', result.Message);
                    BindOrReloadCustomerTable('Reset');
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
