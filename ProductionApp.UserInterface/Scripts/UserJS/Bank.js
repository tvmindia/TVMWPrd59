//*****************************************************************************
//*****************************************************************************
//Author: Thomson
//CreatedDate: 12-Feb-2018 
//LastModified: 14-Feb-2018 
//FileName: Bank.js
//Description: Client side coding for Bank
//******************************************************************************
//******************************************************************************

//--Global Declaration--//
var DataTables = {};
var EmptyGuid = "00000000-0000-0000-0000-000000000000";
//-------------------------------------------------------------
$(document).ready(function () {
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
            ordering:false,
            searching: false,
            paging: true,
            lengthChange: false,
            processing: true,
            language: {

                "processing": "<div class='spinner'><div class='bounce1'></div><div class='bounce2'></div><div class='bounce3'></div></div>"
            },
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
                        return formattToCurrency(roundoff(data, 1),"₹");
                },
                "defaultContent": "<i>-</i>"
            },
            {
                "data": "DisplayODLimit", render: function (data, type, row) {
                    if (data == 0)
                        return '-'
                    else
                        return formattToCurrency(roundoff(data, 1),"₹");
                }, "defaultContent": "<i>-</i>"
            },
            { "data": null, "orderable": false, "defaultContent": '<a href="#" onclick="DeleteBankMaster(this)"<i class="glyphicon glyphicon-trash" aria-hidden="true"></i></a>  <a href="#" onclick="EditBankMaster(this)"<i class="glyphicon glyphicon-edit" aria-hidden="true"></i></a>' }
            ],
            columnDefs: [{ "targets": [], "visible": false, "searchable": false },
                { className: "text-right", "targets": [2, 3] },
                { className: "text-left", "targets": [1] },
                { className: "text-center", "targets": [0,4] }],           
            destroy: true,
            //for performing the import operation after the data loaded
            initComplete: function (settings, json) {
                debugger;
                if (action === 'Export')
                {
                    if (json.data.length > 0)
                    {
                        if (json.data[0].TotalCount > 10000) {
                            MasterAlert("info", 'We are able to download maximum 10000 rows of data, There exist more than 10000 rows of data please filter and download')
                        }
                    }                    
                    $(".buttons-excel").trigger('click');
                    BindOrReloadBankTable('Search');
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
//edit bank 
function EditBankMaster(this_obj) {
    debugger;
    bankVM = DataTables.BankList.row($(this_obj).parents('tr')).data();
    GetMasterPartial("Bank", bankVM.Code);
    $('#h3ModelMasterContextLabel').text('Edit Bank')
    $('#divModelMasterPopUp').modal('show');
    $('#hdnMasterCall').val('MSTR');
}
function DeleteBankMaster(this_obj)
{
    bankVM = DataTables.BankList.row($(this_obj).parents('tr')).data();
    notyConfirm('Are you sure to delete?', 'DeleteBank("' + bankVM.Code + '")');
}
function DeleteBank(code)
{
    try {
        if (code) {
            var data = { "code": code };
            var jsonData = {};
            var message = "";
            var status = "";
            var result = "";
            jsonData = GetDataFromServer("Bank/DeleteBank/", data);
            if (jsonData != '') {
                jsonData = JSON.parse(jsonData);
                message = jsonData.Message;
                status = jsonData.Status;
                result = jsonData.Record;
            }
            switch (status ) {
                case "OK":
                    notyAlert('success', result.Message);
                    BindOrReloadBankTable('Reset');
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

