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
        public ActionResult ViewBillOfMaterial(string code)
        {
            ViewBag.SysModuleCode = code;
            return View();
        }

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

        #region GetAllComponents
        public ActionResult GetAllComponent(DataTableAjaxPostModel model, ProductAdvanceSearchViewModel productAdvanceSearchVM)
        {
            productAdvanceSearchVM.DataTablePaging.Start = model.start;
            productAdvanceSearchVM.DataTablePaging.Length = (productAdvanceSearchVM.DataTablePaging.Length == 0) ? model.length : productAdvanceSearchVM.DataTablePaging.Length;
            
            List<ProductViewModel> productVMList = Mapper.Map<List<Product>, List<ProductViewModel>>(_productBusiness.GetAllProduct(Mapper.Map<ProductAdvanceSearchViewModel, ProductAdvanceSearch>(productAdvanceSearchVM)));
            
            var settings = new JsonSerializerSettings
            {
                Formatting = Formatting.None
            };
            return Json(new
            {
                // this is what datatables wants sending back
                draw = model.draw,
                recordsTotal = productVMList.Count != 0 ? productVMList[0].TotalCount : 0,
                recordsFiltered = productVMList.Count != 0 ? productVMList[0].FilteredCount : 0,
                data = productVMList
            });
        }
        #endregion GetAllComponents
        
        #region GetAllBillOfMaterial
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

        #region InsertUpdateBillOfMaterial
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

        public ActionResult AddProductionLine(BillOfMaterialViewModel billOfMaterialVM)
        {
            billOfMaterialVM.BillOfMaterialDetailList = Mapper.Map<List<BillOfMaterialDetail>, List<BillOfMaterialDetailViewModel>>(_billOfMaterialBusiness.GetBillOfMaterialDetail(billOfMaterialVM.ID));
            return PartialView("_AddProductionLine", billOfMaterialVM);
        }

        public ActionResult AddStageDetail(BillOfMaterialViewModel billOfMaterialVM)
        {
            billOfMaterialVM.BOMComponentLineList = Mapper.Map<List<BOMComponentLine>, List<BOMComponentLineViewModel>>(_billOfMaterialBusiness.GetBOMComponentLineByComponentID(billOfMaterialVM.BOMComponentLine.ComponentID));
            foreach (BOMComponentLineViewModel bOMComponentLine in billOfMaterialVM.BOMComponentLineList)
            {
                bOMComponentLine.BOMComponentLineStageList = Mapper.Map<List<BOMComponentLineStage>, List<BOMComponentLineStageViewModel>>(_billOfMaterialBusiness.GetBOMComponentLineStage(bOMComponentLine.ID));
            }
            billOfMaterialVM.BOMComponentLineStageDetail = new BOMComponentLineStageDetailViewModel()
            {
                Stage = new StageViewModel(),
                Material = new MaterialViewModel(),
                SubComponent = new SubComponentViewModel(),
                Product = new ProductViewModel()
            };
            return PartialView("_AddStageDetail", billOfMaterialVM);
        }

        public ActionResult ListAllStage(BOMComponentLineViewModel bOMComponentLineVM)
        {
            bOMComponentLineVM.StageList = Mapper.Map<List<Stage>, List<StageViewModel>>(_stageBusiness.GetStageForSelectList());
            return PartialView("_ListAllStage", bOMComponentLineVM);
        }

        public ActionResult StageDropdownForLine(BOMComponentLineViewModel bOMComponentLineVM)
        {
            bOMComponentLineVM.BOMComponentLineStageList = Mapper.Map<List<BOMComponentLineStage>, List<BOMComponentLineStageViewModel>>(_billOfMaterialBusiness.GetBOMComponentLineStage(bOMComponentLineVM.ID));
            return PartialView("_StageDropdownForLine", bOMComponentLineVM);
        }
        #endregion PartialViews

        #region InsertUpdateBOMComponentLine
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
        //[AuthSecurityFilter(ProjectObject = "MaterialReceipt", Mode = "R")]
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
    }
}