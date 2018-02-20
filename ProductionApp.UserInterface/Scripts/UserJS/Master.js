//add bank 
function AddBankMaster() {
    GetMasterPartial("Bank", "");
    $('#h3ModelMasterContextLabel').text('Add Bank')
    $('#divModelMasterPopUp').modal('show');
}
//-- add Raw material--//
function AddRawMaterialMaster() {
    GetMasterPartial("RawMaterial", "");
    $('#h3ModelMasterContextLabel').text('Add Raw Material')
    $('#divModelMasterPopUp').modal('show');
}
//-- add Product--//
function AddProductMaster() {
    GetMasterPartial("Product", "");
    $('#h3ModelMasterContextLabel').text('Add Product')
    $('#divModelMasterPopUp').modal('show');
}