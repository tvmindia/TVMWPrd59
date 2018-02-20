//add bank 
function AddBankMaster() {
    GetMasterPartial("Bank", "");
    $('#h3ModelMasterContextLabel').text('Add Bank')
    $('#divModelMasterPopUp').modal('show');
}
//-- add Raw material--//
function AddRawMaterialMaster(flag) {
    GetMasterPartial("RawMaterial", "");
    $('#h3ModelMasterContextLabel').text('Add Raw Material')
    $('#divModelMasterPopUp').modal('show');
}
//-- Function After Save rawmaterial--//
function SaveSuccessRawMaterial(data, status) {
    debugger;
    var JsonResult = JSON.parse(data)
    switch (JsonResult.Result) {
        case "OK":
            $('#IsUpdate').val('True');
            $('#ID').val(JsonResult.Records.ID);           
            MasterAlert("success", JsonResult.Records.Message)
            BindOrReloadRawMaterialTable('Reset');
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