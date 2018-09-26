//*****************************************************************************
//*****************************************************************************
//Author: Angel
//CreatedDate: 11-Apr-2018 
//LastModified: 11-Apr-2018
//FileName: Employee.js
//Description: Client side coding for Employee
//******************************************************************************
// ##1--Global Declaration
// ##2--Document Ready function
// ##3--Product List Binding Function
// ##4--Reset Function
// ##5--Export Function
// ##6-- Edit Function
// ##7--Delete Function
//******************************************************************************

//##1--Global Declaration---------------------------------------------##1 
var _dataTable = {};
var _emptyGuid = "00000000-0000-0000-0000-000000000000";
//##2--Document Ready function---------------------------------------##2
$(document).ready(function () {
    try {
        debugger;
        BindOrReloadEmployeeTable('Init');
    }
    catch (e) {
        console.log(e.message);
    }
    $("#Department_Code").select2({
    });
    $("#EmployeeCategory_Code").select2({
    });
});

//##3--Product List Binding Function-----------------------------------##3
function BindOrReloadEmployeeTable(action) {
    try {
        debugger;
        //creating advancesearch object
        EmployeeAdvanceSearchViewModel = new Object();
        DataTablePagingViewModel = new Object();
        DepartmentViewModel = new Object();
        EmployeeCategoryViewModel = new Object();
        DataTablePagingViewModel.Length = 0;
        //switch case to check the operation
        switch (action) {
            case 'Reset':
                $('#SearchTerm').val('');
                $('#Department_Code').val('').select2();
                $('#EmployeeCategory_Code').val('').select2();
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
        EmployeeAdvanceSearchViewModel.DataTablePaging = DataTablePagingViewModel;
        DepartmentViewModel.Code = $("#Department_Code").val();
        EmployeeAdvanceSearchViewModel.Department = DepartmentViewModel;
        EmployeeCategoryViewModel.Code = $("#EmployeeCategory_Code").val();
        EmployeeAdvanceSearchViewModel.EmployeeCategory = EmployeeCategoryViewModel;
        EmployeeAdvanceSearchViewModel.SearchTerm = $('#SearchTerm').val();
        debugger;
        //apply datatable plugin on Raw Material table
        _dataTable.employeeList = $('#tblEmployee').DataTable(
        {

            dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
            buttons: [{
                extend: 'excel',
                exportOptions:
                             {
                                 columns: [0, 1, 2, 3, 4, 5, 6]
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
                url: "Employee/GetAllEmployee/",
                data: {
                    "employeeAdvanceSearchVM": EmployeeAdvanceSearchViewModel
            },
                type: 'POST'
            },
            pageLength: 10,
            columns: [
            { "data": "Code", "defaultContent": "<i>-</i>", "width": "5%" },
            { "data": "Name", "defaultContent": "<i>-<i>", "width": "18%" },
            { "data": "Department.Name", "defaultContent": "<i>-<i>", "width": "18%" },
            { "data": "EmployeeCategory.Name", "defaultContent": "<i>-<i>", "width": "14%" },
            { "data": "MobileNo", "defaultContent": "<i>-<i>", "width": "15%" },
            { "data": "Address", "defaultContent": "<i>-<i>", "width": "18%" },
            { "data": "IsActive", "defaultContent": "<i>-<i>", "width": "7%" },
            { "data": null, "orderable": false, "defaultContent": '<a href="#" onclick="EditEmployeeMaster(this)"<i class="glyphicon glyphicon-edit" aria-hidden="true"></i></a> <a href="#" onclick="DeleteEmployeeMaster(this)"<i class="glyphicon glyphicon-trash" aria-hidden="true"></i></a>', "width": "5%" }
            ],
            columnDefs: [{ "targets": [], "visible": false, "searchable": false },
                { className: "text-right", "targets": [] },
                { className: "text-left", "targets": [0, 1, 2, 3, 4, 5,6] },
                { className: "text-center", "targets": [7] }],
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
                    BindOrReloadEmployeeTable('Search');
                }
            }
        });
        $(".buttons-excel").hide();
    }
    catch (e) {
        console.log(e.message);
    }
}

//##4--Reset Function--------------------------------------------------##4
function ResetEmployeeList() {
    BindOrReloadEmployeeTable('Reset');
}

//##5--Export Function-------------------------------------------------##5
function ImportEmployeeData() {
    BindOrReloadEmployeeTable('Export');
}
//##6-- Edit Function-------------------------------------------------##6
function EditEmployeeMaster(this_obj) {
    EmployeeViewModel = _dataTable.employeeList.row($(this_obj).parents('tr')).data();
    GetMasterPartial("Employee", EmployeeViewModel.ID);
    $('#h3ModelMasterContextLabel').text('Edit Employee')
    $('#divModelMasterPopUp').modal('show');
    $('#hdnMasterCall').val('MSTR');
}
//##7--Delete Function-------------------------------------------------##7
function DeleteEmployeeMaster(this_obj) {
    debugger;
    productVM = _dataTable.employeeList.row($(this_obj).parents('tr')).data();
    notyConfirm('Are you sure to delete?', 'DeleteEmployee("' + productVM.ID + '")');
}
function DeleteEmployee(id) {
    debugger;
    try {
        if (id) {
            var data = { "id": id };
            var jsonData = {};
            var message = "";
            var status = "";
            var result = "";
            jsonData = GetDataFromServer("Employee/DeleteEmployee/", data);
            if (jsonData != '') {
                jsonData = JSON.parse(jsonData);
                message = jsonData.Message;
                status = jsonData.Status;
                result = jsonData.Record;
            }
            switch (status) {
                case "OK":
                    notyAlert('success', result.Message);
                    BindOrReloadEmployeeTable('Reset');
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