﻿//add bank 
function AddBankMaster(flag) {
    GetMasterPartial("Bank", "");
    $('#h3ModelMasterContextLabel').text('Add Bank')
    $('#divModelMasterPopUp').modal('show');
    $('#hdnMasterCall').val(flag);
}
//onsuccess function for formsubmitt
function SaveSuccessBank(data, status) {
    debugger;
    var JsonResult = JSON.parse(data)
    switch (JsonResult.Result) {
        case "OK":
            if ($('#hdnMasterCall').val() == "MSTR") {
                $('#IsUpdate').val('True');
                BindOrReloadBankTable('Reset');
            }
            else if ($('#hdnMasterCall').val() == "OTR") {
                $('#divBankDropdown').load('/Bank/BankDropdown');
            }
            MasterAlert("success", JsonResult.Records.Message)
            break;
        case "ERROR":
            MasterAlert("danger", JsonResult.Message)
            break;
        default:
            MasterAlert("danger", JsonResult.Message)
            break;
    }
}
//-- add  material--//
function AddMaterialMaster(flag) {
    GetMasterPartial("Material", "");
    $('#h3ModelMasterContextLabel').text('Add Raw Material')
    $('#divModelMasterPopUp').modal('show');
    $('#hdnMasterCall').val(flag);
}
//-- Function After Save material--//
function SaveSuccessMaterial(data, status) {
    debugger;
    var JsonResult = JSON.parse(data)
    switch (JsonResult.Result) {
        case "OK":
            if ($('#hdnMasterCall').val() == "MSTR")
            {
                $('#IsUpdate').val('True');
                $('#ID').val(JsonResult.Records.ID);
                BindOrReloadMaterialTable('Reset');
            }
            else if ($('#hdnMasterCall').val() == "OTR")
            {
                $('#divMaterialDropdown').load('/Material/MaterialDropdown');
            }            
            MasterAlert("success", JsonResult.Records.Message)            
            break;
        case "ERROR":
            MasterAlert("danger", JsonResult.Message)
            break;
        default:
            MasterAlert("danger", JsonResult.Message)
            break;
    }
}
//-- add Product--//
function AddProductMaster() {
    GetMasterPartial("Product", "");
    $('#h3ModelMasterContextLabel').text('Add Product')
    $('#divModelMasterPopUp').modal('show');
}
//-- Function After Product Save --//
function SaveSuccessProduct(data, status) {
    debugger;
    var JsonResult = JSON.parse(data)
    switch (JsonResult.Result) {
        case "OK":
            if ($('#hdnMasterCall').val() == "MSTR") {
                $('#IsUpdate').val('True');
                $('#ID').val(JsonResult.Records.ID);
                BindOrReloadProductTable('Reset');
            }
            else if ($('#hdnMasterCall').val() == "OTR") {
                $('#divProductDropdown').load('/Product/ProductDropdown');
            }            
            MasterAlert("success", JsonResult.Records.Message)
            break;
        case "ERROR":
            MasterAlert("danger", JsonResult.Message)
            break;
        default:
            MasterAlert("danger", JsonResult.Message)
            break;
    }
}
//-- add MaterialType--//
function AddMaterialTypeMaster(flag) {
    GetMasterPartial("MaterialType", "");
    $('#h3ModelMasterContextLabel').text('Add MaterialType')
    $('#divModelMasterPopUp').modal('show');
    $('#hdnMasterCall').val(flag);
}
//-- Function After MaterialType Save --//
function SaveSuccessMaterialType(data, status) {
    debugger;
    var JsonResult = JSON.parse(data)
    switch (JsonResult.Result) {
        case "OK":
            if ($('#hdnMasterCall').val() == "MSTR") {
                $('#IsUpdate').val('True');
                //BindOrReloadMaterialTypeTable('Reset');
            }
            else if ($('#hdnMasterCall').val() == "OTR") {
                $('#divMaterialTypeDropdown').load('/MaterialType/MaterialTypeDropdown');
            }
            MasterAlert("success", JsonResult.Records.Message)
            break;
        case "ERROR":
            MasterAlert("danger", JsonResult.Message)
            break;
        default:
            MasterAlert("danger", JsonResult.Message)
            break;
    }
}

//-- Function After Save Approver--//
function SaveSuccessApprover(data, status) {
    debugger;
    var JsonResult = JSON.parse(data)
    switch (JsonResult.Result) {
        case "OK":
            if ($('#hdnMasterCall').val() == "MSTR") {
                $('#IsUpdate').val('True');
                BindOrReloadApproverTable('Reset');
                //$('#ID').val(JsonResult.Records.ID);
                //BindOrReloadApproverTable('Reset');
            }
            else if ($('#hdnMasterCall').val() == "OTR") {
                $('#divApproverDropdown').load('/Approver/ApproverDropdown');
            }
            MasterAlert("success", JsonResult.Records.Message)


            break;
        case "ERROR":
            MasterAlert("danger", JsonResult.Message)
            break;
        default:
            MasterAlert("danger", JsonResult.Message)
            break;
    }
}