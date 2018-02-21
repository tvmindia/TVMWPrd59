//add bank 
function AddBankMaster() {
    GetMasterPartial("Bank", "");
    $('#h3ModelMasterContextLabel').text('Add Bank')
    $('#divModelMasterPopUp').modal('show');
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