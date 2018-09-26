
//*****************************************************************************
//*****************************************************************************
//Author: Jais
//CreatedDate: 12-Mar-2018 
//LastModified: 12-Mar-2018 
//FileName: ServiceItem.js
//Description: Client side coding for ServiceItem
//******************************************************************************
//******************************************************************************

//--Global Declaration--//
var DataTables = {};
var EmptyGuid = "00000000-0000-0000-0000-000000000000";

//--Loading DOM--//
$(document).ready(function () {
    try {
        debugger;
        BindOrReloadServiceItemTable('Init');
    }
    catch (e) {
        console.log(e.message);
    }
});

//--function bind the ServiceItem list checking search and filter--//
function BindOrReloadServiceItemTable(action) {
    try {
        debugger;
        //creating advancesearch object
        ServiceItemAdvanceSearchViewModel = new Object();
        DataTablePagingViewModel = new Object();
        UnitViewModel = new Object();
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
        ServiceItemAdvanceSearchViewModel.DataTablePaging = DataTablePagingViewModel;
        ServiceItemAdvanceSearchViewModel.SearchTerm = $('#SearchTerm').val();

        //apply datatable plugin on ServiceItem table
        DataTables.serviceItemList = $('#tblServiceItem').DataTable(
        {
            dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
            buttons: [{
                extend: 'excel',
                exportOptions:
                             {
                                 columns: [0, 1]
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
                url: "ServiceItem/GetAllServiceItem/",
                data: { "serviceItemAdvanceSearchVM": ServiceItemAdvanceSearchViewModel },
                type: 'POST'
            },
            pageLength: 10,
            columns: [
            //{ "data": "ID", "defaultContent": "<i>-</i>", "width": "10%" },
            { "data": "ServiceName", "defaultContent": "<i>-</i>", "width": "35%" },
            { "data": null, "defaultContent": "<i>-<i>", "width": "25%" },
            { "data": "Rate", "defaultContent": "<i>-<i>", "width": "30%" },
            { "data": null, "orderable": false, "defaultContent": '<a href="#" onclick="EditServiceItemMaster(this)"<i class="glyphicon glyphicon-edit" aria-hidden="true"></i></a> <a href="#" onclick="DeleteServiceItemMaster(this)"<i class="glyphicon glyphicon-trash" aria-hidden="true"></i></a>  ', "width": "10%" }
            ],
            columnDefs: [{ "targets": [], "visible": false, "searchable": false },
                { className: "text-right", "targets": [2] },
                { className: "text-left", "targets": [0,1] },
                { className: "text-center", "targets": [3] }],
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
                    BindOrReloadServiceItemTable('Search');
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
function ResetServiceItemList() {
    BindOrReloadServiceItemTable('Reset');
}

//--function export data to excel--//
function ImportServiceItemData() {
    BindOrReloadServiceItemTable('Export');
}
//--edit ServiceItem--//
function EditServiceItemMaster(this_obj) {
    ServiceItemViewModel = DataTables.serviceItemList.row($(this_obj).parents('tr')).data();
    GetMasterPartial("ServiceItem", ServiceItemViewModel.ID);
    $('#h3ModelMasterContextLabel').text('Edit ServiceItem')
    $('#divModelMasterPopUp').modal('show');
    $('#hdnMasterCall').val('MSTR');
}

//--Function To Confirm ServiceItem Deletion 
function DeleteServiceItemMaster(this_obj) {
    debugger;
    serviceItemVM = DataTables.serviceItemList.row($(this_obj).parents('tr')).data();
    notyConfirm('Are you sure to delete?', 'DeleteServiceItem("' + serviceItemVM.ID + '")');
}

//--Function To Delete ServiceItem
function DeleteServiceItem(id) {
    debugger;
    try {
        if (id) {
            var data = { "id": id };
            var jsonData = {};
            var message = "";
            var status = "";
            var result = "";
            jsonData = GetDataFromServer("ServiceItem/DeleteServiceItem/", data);
            if (jsonData != '') {
                jsonData = JSON.parse(jsonData);
                message = jsonData.Message;
                status = jsonData.Status;
                result = jsonData.Record;
            }
            switch (status) {
                case "OK":
                    notyAlert('success', result.Message);
                    BindOrReloadServiceItemTable('Reset');
                    break;
                case "ERROR":
                    notyAlert('error', message);
                    break;
                default:
                    notyAlert('error', message);
                    break;
            }
        }
    }
    catch (e) {
        //this will show the error msg in the browser console(F12) 
        console.log(e.message);
    }
}