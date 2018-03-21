//add bank 
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
    $('#h3ModelMasterContextLabel').text('Add Material')
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
    $('#h3ModelMasterContextLabel').text('Add Material Type')
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
                if (JsonResult.Records.IsDefault == true) {
                    $("#IsDefault").prop("checked", true);
                    $('#IsDefault').prop("disabled", true);
                }
                else {
                    $("#IsDefault").prop("checked", false);
                    $('#IsDefault').prop("disabled", false);
                }
                BindOrReloadApproverTable('Reset');
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

//-- add  Stage--//
function AddStageMaster(flag) {
    GetMasterPartial("Stage", "");
    $('#h3ModelMasterContextLabel').text('Add Production Stage')
    $('#divModelMasterPopUp').modal('show');
    $('#hdnMasterCall').val(flag);
}
//-- Function After Save Stage--//
function SaveSuccessStage(data, status) {
    debugger;
    var JsonResult = JSON.parse(data)
    switch (JsonResult.Result) {
        case "OK":
            if ($('#hdnMasterCall').val() == "MSTR") {
                $('#IsUpdate').val('True');
                $('#ID').val(JsonResult.Records.ID);
                BindOrReloadStageTable('Reset');
            }
            else if ($('#hdnMasterCall').val() == "OTR") {
                $('#divMaterialDropdown').load('/Stage/StageDropdown');
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

//-- add  SubComponent--//
function AddSubComponentMaster(flag) {
    GetMasterPartial("SubComponent", "");
    $('#h3ModelMasterContextLabel').text('Add Sub Component')
    $('#divModelMasterPopUp').modal('show');
    $('#hdnMasterCall').val(flag);
}
//-- Function After Save SubComponent--//
function SaveSuccessSubComponent(data, status) {
    debugger;
    var JsonResult = JSON.parse(data)
    switch (JsonResult.Result) {
        case "OK":
            if ($('#hdnMasterCall').val() == "MSTR") {
                $('#IsUpdate').val('True');
                $('#ID').val(JsonResult.Records.ID);
                BindOrReloadSubComponentTable('Reset');
            }
            else if ($('#hdnMasterCall').val() == "OTR") {
                $('#divSubComponentDropdown').load('/SubComponent/SubComponentDropdown');
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

//-- Add  Customer--//
function AddCustomerMaster(flag) {
    GetMasterPartial("Customer", "");
    $('#h3ModelMasterContextLabel').text('Add Customer')
    $('#divModelMasterPopUp').modal('show');
    $('#hdnMasterCall').val(flag);
}