using ProductionApp.BusinessService.Contracts;
using ProductionApp.DataAccessObject.DTO;
using ProductionApp.UserInterface.Models;
using ProductionApp.UserInterface.SecurityFilter;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace ProductionApp.UserInterface.Controllers
{
    public class BillOfMaterialController : Controller
    {
        #region Constructor Injection
        private IBillOfMaterialBusiness _billOfMaterialBusiness;
        private IProductBusiness _productBusiness;
        private IStageBusiness _stageBusiness;
        AppConst _appConst = new AppConst();
        Common _common = new Common();
        public BillOfMaterialController(IBillOfMaterialBusiness billOfMaterialBusiness, IProductBusiness productBusiness, IStageBusiness stageBusiness)
        {
            _billOfMaterialBusiness = billOfMaterialBusiness;
            _productBusiness = productBusiness;
            _stageBusiness = stageBusiness;
        }
        #endregion Constructor Injection

        // GET: BillOfMaterial
        #region ViewBillOfMaterial
        [AuthSecurityFilter(ProjectObject = "BillOfMaterial", Mode = "R")]
        public ActionResult ViewBillOfMaterial(string code)
        {
            ViewBag.SysModuleCode = code;
            return View();
        }
        #endregion ViewBillOfMaterial

        #region NewBillOfMaterial
        [AuthSecurityFilter(ProjectObject = "BillOfMaterial", Mode = "R")]
        public ActionResult NewBillOfMaterial(string code,Guid? id)
        {
            ViewBag.SysModuleCode = code;
            BillOfMaterialViewModel billOfMaterialVM = new BillOfMaterialViewModel
            {
                ID = id == null ? Guid.Empty : (Guid)id,
                IsUpdate = id == null ? false : true,
                Product = new ProductViewModel()
            };
            return View(billOfMaterialVM);
        }
        #endregion NewBillOfMaterial

        #region GetAllBillOfMaterial
        [AuthSecurityFilter(ProjectObject = "BillOfMaterial", Mode = "R")]
        public ActionResult GetAllBillOfMaterial(DataTableAjaxPostModel model, BillOfMaterialAdvanceSearchViewModel billOfMaterialAdvanceSearchVM)
        {
            try
            {
                billOfMaterialAdvanceSearchVM.DataTablePaging.Start = model.start;
                billOfMaterialAdvanceSearchVM.DataTablePaging.Length = (billOfMaterialAdvanceSearchVM.DataTablePaging.Length==0)?model.length: billOfMaterialAdvanceSearchVM.DataTablePaging.Length;
                List<BillOfMaterialViewModel> billOfMaterialList = Mapper.Map<List<BillOfMaterial>, List<BillOfMaterialViewModel>>(_billOfMaterialBusiness.GetAllBillOfMaterial(Mapper.Map<BillOfMaterialAdvanceSearchViewModel, BillOfMaterialAdvanceSearch>(billOfMaterialAdvanceSearchVM)));
                if (billOfMaterialAdvanceSearchVM.DataTablePaging.Length == -1)
                {
                    int totalResult = billOfMaterialList.Count != 0 ? billOfMaterialList[0].TotalCount : 0;
                    int filteredResult = billOfMaterialList.Count != 0 ? billOfMaterialList[0].FilteredCount : 0;
                    billOfMaterialList = billOfMaterialList.Skip(0).Take(filteredResult > 10000 ? 10000 : filteredResult).ToList();
                }
                var settings = new JsonSerializerSettings
                {
                    //ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    Formatting = Formatting.None
                };
                foreach(BillOfMaterialViewModel billOfMaterial in billOfMaterialList)
                {
                    billOfMaterial.BillOfMaterialDetailList= Mapper.Map<List<BillOfMaterialDetail>, List<BillOfMaterialDetailViewModel>>(_billOfMaterialBusiness.GetBillOfMaterialDetail(billOfMaterial.ID));
                }
                return Json(new
                {
                    draw = model.draw,
                    recordsTotal = billOfMaterialList.Count != 0 ? billOfMaterialList[0].TotalCount : 0,
                    recordsFiltered = billOfMaterialList.Count != 0 ? billOfMaterialList[0].FilteredCount : 0,
                    data = billOfMaterialList
                });
            }
            catch(Exception ex)
            {
                return Json(new
                {
                    message=ex.Message
                });
            }
        }
        #endregion GetAllBillOfMaterial

        #region GetProductListForBillOfMaterial
        [AuthSecurityFilter(ProjectObject = "BillOfMaterial", Mode = "R")]
        public string GetProductListForBillOfMaterial(string componentIDs)
        {
            try
            {
                componentIDs = (componentIDs != "" ? componentIDs : null);
                List<ProductViewModel> productList = Mapper.Map<List<Product>, List<ProductViewModel>>(_productBusiness.GetProductListForBillOfMaterial(componentIDs));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = productList, Message ="Success" });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConst.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Records = "", Message = cm.Message });
            }
        }
        #endregion GetProductListForBillOfMaterial

        #region CheckBillOfMaterialExist
        public string CheckBillOfMaterialExist(string productID)
        {
            try
            {
                bool result = _billOfMaterialBusiness.CheckBillOfMaterialExist(Guid.Parse(productID));
                if (!result)
                {
                    return JsonConvert.SerializeObject(new { Result = "OK", Message = "BOM Does not exists" });
                }
                else
                {
                    return JsonConvert.SerializeObject(new { Result = "WARNING", Message = "BOM Already Exists for the Product" });
                }
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConst.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }
        #endregion CheckBillOfMaterialExist

        #region InsertUpdateBillOfMaterial
        [AuthSecurityFilter(ProjectObject = "BillOfMaterial", Mode = "W")]
        public string InsertUpdateBillOfMaterial(BillOfMaterialViewModel billOfMaterialVM)
        {
            try
            {
                AppUA appUA = Session["AppUA"] as AppUA;
                billOfMaterialVM.Common = new CommonViewModel
                {
                    CreatedBy = appUA.UserName,
                    CreatedDate = _common.GetCurrentDateTime(),
                    UpdatedBy = appUA.UserName,
                    UpdatedDate = _common.GetCurrentDateTime(),
                };
                //Deserialize items
                object resultFromJS = JsonConvert.DeserializeObject(billOfMaterialVM.DetailJSON);
                string readableFormat = JsonConvert.SerializeObject(resultFromJS);
                billOfMaterialVM.BillOfMaterialDetailList = JsonConvert.DeserializeObject<List<BillOfMaterialDetailViewModel>>(readableFormat);
                object result = _billOfMaterialBusiness.InsertUpdateBillOfMaterial(Mapper.Map<BillOfMaterialViewModel, BillOfMaterial>(billOfMaterialVM));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = result, Message = _appConst.InsertSuccess });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConst.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Records = "", Message = cm.Message });
            }
        }
        #endregion InsertUpdateBillOfMaterial

        #region GetBillOfMaterial
        [AuthSecurityFilter(ProjectObject = "BillOfMaterial", Mode = "R")]
        public string GetBillOfMaterial(string id)
        {
            try
            {
                BillOfMaterialViewModel billOfMaterialVM = Mapper.Map<BillOfMaterial, BillOfMaterialViewModel>(_billOfMaterialBusiness.GetBillOfMaterial(Guid.Parse(id)));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = billOfMaterialVM, Message = "Success" });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConst.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Record = "", Message = cm.Message });
            }
        }
        #endregion GetBillOfMaterial

        #region GetBillOfMaterialDetail
        [AuthSecurityFilter(ProjectObject = "BillOfMaterial", Mode = "R")]
        public string GetBillOfMaterialDetail(string id)
        {
            try
            {
                List<BillOfMaterialDetailViewModel> billOfMaterialDetailList = Mapper.Map<List<BillOfMaterialDetail>, List<BillOfMaterialDetailViewModel>>(_billOfMaterialBusiness.GetBillOfMaterialDetail(Guid.Parse(id)));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = billOfMaterialDetailList, Message = "Success" });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConst.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Record = "", Message = cm.Message });
            }
        }
        #endregion GetBillOfMaterialDetail

        #region DeleteBillOfMaterial
        [AuthSecurityFilter(ProjectObject = "BillOfMaterial", Mode = "D")]
        public string DeleteBillOfMaterial(string id)
        {
            try
            {
                object result = _billOfMaterialBusiness.DeleteBillOfMaterial(Guid.Parse(id));
                return JsonConvert.SerializeObject(new { Result = "OK", Record = result, Message = _appConst.DeleteSuccess });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Record = "", Message = ex.Message });
            }
        }
        #endregion DeleteBillOfMaterial

        #region DeleteBillOfMaterialDetail
        [AuthSecurityFilter(ProjectObject = "BillOfMaterial", Mode = "D")]
        public object DeleteBillOfMaterialDetail(string id)
        {
            try
            {
                object result =_billOfMaterialBusiness.DeleteBillOfMaterialDetail(Guid.Parse(id));
                return JsonConvert.SerializeObject(new { Result = "OK", Record = result, Message = _appConst.DeleteSuccess });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Record = "", Message = ex.Message });
            }
        }
        #endregion DeleteBillOfMaterialDetail

        #region PartialViews
        [AuthSecurityFilter(ProjectObject = "BillOfMaterial", Mode = "W")]
        public ActionResult AddComponent(Guid? id)
        {
            BillOfMaterialViewModel billOfMaterialVM = new BillOfMaterialViewModel
            {
                ID = id == null ? Guid.Empty : (Guid)id,
                IsUpdate = id == null ? false : true,
                Product = new ProductViewModel()
            };
            return PartialView("_AddComponent", billOfMaterialVM);
        }

        [AuthSecurityFilter(ProjectObject = "BillOfMaterial", Mode = "W")]
        public ActionResult AddProductionLine(BillOfMaterialViewModel billOfMaterialVM)
        {
            billOfMaterialVM.BillOfMaterialDetailList = Mapper.Map<List<BillOfMaterialDetail>, List<BillOfMaterialDetailViewModel>>(_billOfMaterialBusiness.GetBillOfMaterialDetail(billOfMaterialVM.ID));
            return PartialView("_AddProductionLine", billOfMaterialVM);
        }

        [AuthSecurityFilter(ProjectObject = "BillOfMaterial", Mode = "W")]
        public ActionResult AddStageDetail(BillOfMaterialViewModel billOfMaterialVM)
        {
            billOfMaterialVM.BOMComponentLineList = Mapper.Map<List<BOMComponentLine>, List<BOMComponentLineViewModel>>(_billOfMaterialBusiness.GetBOMComponentLineByComponentID(billOfMaterialVM.BOMComponentLine.ComponentID));
            foreach (BOMComponentLineViewModel bOMComponentLine in billOfMaterialVM.BOMComponentLineList)
            {
                bOMComponentLine.BOMComponentLineStageList = Mapper.Map<List<BOMComponentLineStage>, List<BOMComponentLineStageViewModel>>(_billOfMaterialBusiness.GetBOMComponentLineStage(bOMComponentLine.ID));
            }
            billOfMaterialVM.BOMComponentLineStageDetail.Stage = new StageViewModel();
            billOfMaterialVM.BOMComponentLineStageDetail.Material = new MaterialViewModel();
            billOfMaterialVM.BOMComponentLineStageDetail.SubComponent = new SubComponentViewModel();
            billOfMaterialVM.BOMComponentLineStageDetail.Product = new ProductViewModel()
            {
                ProductSelectList=_productBusiness.GetProductForSelectList()
            };

            return PartialView("_AddStageDetail", billOfMaterialVM);
        }

        [AuthSecurityFilter(ProjectObject = "BillOfMaterial", Mode = "W")]
        public ActionResult ListAllStage(BOMComponentLineViewModel bOMComponentLineVM)
        {
            bOMComponentLineVM.StageList = Mapper.Map<List<Stage>, List<StageViewModel>>(_stageBusiness.GetStageForSelectList());
            return PartialView("_ListAllStage", bOMComponentLineVM);
        }

        [AuthSecurityFilter(ProjectObject = "BillOfMaterial", Mode = "W")]
        public ActionResult StageDropdownForLine(BOMComponentLineViewModel bOMComponentLineVM)
        {
            bOMComponentLineVM.BOMComponentLineStageList = Mapper.Map<List<BOMComponentLineStage>, List<BOMComponentLineStageViewModel>>(_billOfMaterialBusiness.GetBOMComponentLineStage(bOMComponentLineVM.ID));
            return PartialView("_StageDropdownForLine", bOMComponentLineVM);
        }
        #endregion PartialViews

        #region CheckLineNameExist
        public string CheckLineNameExist(string lineName)
        {
            try
            {
                bool result = _billOfMaterialBusiness.CheckLineNameExist(lineName);
                if (!result)
                {
                    return JsonConvert.SerializeObject(new { Result = "OK", Message = "Line Name Does not exists" });
                }
                else
                {
                    return JsonConvert.SerializeObject(new { Result = "WARNING", Message = "Line Name Exists" });
                }
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConst.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }
        #endregion CheckLineNameExist

        #region InsertUpdateBOMComponentLine
        [AuthSecurityFilter(ProjectObject = "BillOfMaterial", Mode = "W")]
        public string InsertUpdateBOMComponentLine(BillOfMaterialViewModel billOfMaterialVM)
        {
            try//billOfMaterialVM.BOMComponentLine
            {
                AppUA appUA = Session["AppUA"] as AppUA;
                billOfMaterialVM.BOMComponentLine.Common = new CommonViewModel
                {
                    CreatedBy = appUA.UserName,
                    CreatedDate = _common.GetCurrentDateTime(),
                    UpdatedBy = appUA.UserName,
                    UpdatedDate = _common.GetCurrentDateTime()
                };
                //Deserialize items
                object resultFromJS = JsonConvert.DeserializeObject(billOfMaterialVM.BOMComponentLine.StageJSON);
                string readableFormat = JsonConvert.SerializeObject(resultFromJS);
                billOfMaterialVM.BOMComponentLine.BOMComponentLineStageList = JsonConvert.DeserializeObject<List<BOMComponentLineStageViewModel>>(readableFormat);
                object result = _billOfMaterialBusiness.InsertUpdateBOMComponentLine(Mapper.Map<BOMComponentLineViewModel, BOMComponentLine>(billOfMaterialVM.BOMComponentLine));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = result, Message = _appConst.InsertSuccess });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConst.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Records = "", Message = cm.Message });
            }
        }
        #endregion InsertUpdateBOMComponentLine

        #region DeleteBOMComponentLine
        [AuthSecurityFilter(ProjectObject = "BillOfMaterial", Mode = "D")]
        public object DeleteBOMComponentLine(string id)
        {
            try
            {
                object result = _billOfMaterialBusiness.DeleteBOMComponentLine(Guid.Parse(id));
                return JsonConvert.SerializeObject(new { Result = "OK", Record = result, Message = _appConst.DeleteSuccess });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Record = "", Message = ex.Message });
            }
        }
        #endregion DeleteBOMComponentLine

        #region GetBOMComponentLine
        [AuthSecurityFilter(ProjectObject = "BillOfMaterial", Mode = "R")]
        public string GetBOMComponentLine(string componentID)
        {
            try
            {
            List<BOMComponentLineViewModel> bOMComponentLineList = new List<BOMComponentLineViewModel>();
            if (componentID != "")
            {
                bOMComponentLineList = Mapper.Map<List<BOMComponentLine>, List<BOMComponentLineViewModel>>(_billOfMaterialBusiness.GetBOMComponentLineByComponentID(Guid.Parse(componentID)));
                foreach (BOMComponentLineViewModel bOMComponentLine in bOMComponentLineList)
                {
                    bOMComponentLine.BOMComponentLineStageList = Mapper.Map<List<BOMComponentLineStage>, List<BOMComponentLineStageViewModel>>(_billOfMaterialBusiness.GetBOMComponentLineStage(bOMComponentLine.ID));
                }
            }
            return JsonConvert.SerializeObject(new { Result = "OK", Records = bOMComponentLineList, Message = "Success" });

            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Record = "", Message = ex.Message });
            }
        }
        #endregion

        #region InsertUpdateBOMComponentLineStageDetail
        [AuthSecurityFilter(ProjectObject = "BillOfMaterial", Mode = "W")]
        public string InsertUpdateBOMComponentLineStageDetail(BillOfMaterialViewModel billOfMaterialVM)
        {
            try
            {
                AppUA appUA = Session["AppUA"] as AppUA;
                billOfMaterialVM.BOMComponentLineStageDetail.Common = new CommonViewModel()
                {
                    CreatedBy = appUA.UserName,
                    CreatedDate = _common.GetCurrentDateTime(),
                    UpdatedBy = appUA.UserName,
                    UpdatedDate = _common.GetCurrentDateTime()
                };
                object result = _billOfMaterialBusiness.InsertUpdateBOMComponentLineStageDetail(Mapper.Map<BOMComponentLineStageDetailViewModel, BOMComponentLineStageDetail>(billOfMaterialVM.BOMComponentLineStageDetail));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = result, Message = _appConst.InsertSuccess });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConst.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Records = "", Message = cm.Message });
            }
        }
        #endregion InsertUpdateBOMComponentLineStageDetail

        #region DeleteBOMComponentLineStageDetail
        [AuthSecurityFilter(ProjectObject = "BillOfMaterial", Mode = "D")]
        public string DeleteBOMComponentLineStageDetail(string id)
        {
            try
            {
                object result = _billOfMaterialBusiness.DeleteBOMComponentLineStageDetail(Guid.Parse(id));
                return JsonConvert.SerializeObject(new { Result = "OK", Record = result, Message = _appConst.DeleteSuccess });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Record = "", Message = ex.Message });
            }
        }
        #endregion DeleteBOMComponentLineStageDetail

        #region GetBOMComponentLineStageDetail
        [AuthSecurityFilter(ProjectObject = "BillOfMaterial", Mode = "R")]
        public string GetBOMComponentLineStageDetail(string componentLineID)
        {
            try
            {
                List<BOMComponentLineStageDetailViewModel> bOMComponentLineStageDetailList = Mapper.Map<List<BOMComponentLineStageDetail>, List<BOMComponentLineStageDetailViewModel>>(_billOfMaterialBusiness.GetBOMComponentLineStageDetail(Guid.Parse(componentLineID)));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = bOMComponentLineStageDetailList, Message = "Success" });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Record = "", Message = ex.Message });
            }
        }
        #endregion GetBOMComponentLineStageDetail

        #region ButtonStyling
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "BillOfMaterial", Mode = "R")]
        public ActionResult ChangeButtonStyle(string actionType)
        {
            ToolboxViewModel toolboxVM = new ToolboxViewModel();
            switch (actionType)
            {
                case "List":
                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "Add";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Href = Url.Action("NewBillOfMaterial", "BillOfMaterial", new { code = "PROD" });
                    toolboxVM.addbtn.Event = "";

                    toolboxVM.resetbtn.Visible = true;
                    toolboxVM.resetbtn.Text = "Reset";
                    toolboxVM.resetbtn.Title = "Reset All";
                    toolboxVM.resetbtn.Event = "Reset();";

                    toolboxVM.PrintBtn.Visible = true;
                    toolboxVM.PrintBtn.Text = "Export";
                    toolboxVM.PrintBtn.Title = "Export";
                    toolboxVM.PrintBtn.Event = "Export();";

                    break;
                case "Edit":
                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "Add";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Href = Url.Action("NewBillOfMaterial", "BillOfMaterial", new { code = "PROD" });
                    toolboxVM.addbtn.Event = "";

                    toolboxVM.resetbtn.Visible = true;
                    toolboxVM.resetbtn.Text = "Reset";
                    toolboxVM.resetbtn.Title = "Reset All";
                    toolboxVM.resetbtn.Event = "Reset();";

                    toolboxVM.ListBtn.Visible = true;
                    toolboxVM.ListBtn.Text = "List";
                    toolboxVM.ListBtn.Title = "List";
                    toolboxVM.ListBtn.Href = Url.Action("ViewBillOfMaterial", "BillOfMaterial", new { code = "PROD" });
                    toolboxVM.ListBtn.Event = "";
                    
                    toolboxVM.deletebtn.Visible = true;
                    toolboxVM.deletebtn.Text = "Delete";
                    toolboxVM.deletebtn.Title = "Delete";
                    toolboxVM.deletebtn.Event = "DeleteClick()";
                    break;
                case "Add":
                    toolboxVM.ListBtn.Visible = true;
                    toolboxVM.ListBtn.Text = "List";
                    toolboxVM.ListBtn.Title = "List";
                    toolboxVM.ListBtn.Href = Url.Action("ViewBillOfMaterial", "BillOfMaterial", new { code = "PROD" });
                    toolboxVM.ListBtn.Event = "";
                    
                    break;
                default:
                    return Content("Nochange");
            }
            return PartialView("ToolboxView", toolboxVM);
        }
        #endregion ButtonStyling

        #region GetBOMTree
        public string GetBOMTree(string ProductID)
        {
           
            List<BOMTreeViewModel> bomTree = Mapper.Map<List<BOMTree>, List<BOMTreeViewModel>>(_billOfMaterialBusiness.GetBOMTree(Guid.Parse(ProductID)));
            object renamedResultForJSTree= from a in bomTree select new { id = a.ID.ToString(), parent =  a.ParentID==null?"#":a.ParentID.ToString() , text = a.Qty!="N/A"? a.Name + " -- Qty : "+ a.Qty +"": a.Name 
                                           ,icon= a.Icon };
            return JsonConvert.SerializeObject(new { Result = "OK", Records = renamedResultForJSTree, Message = "Success" });

        }
        #endregion GetBOMTree
    }
}