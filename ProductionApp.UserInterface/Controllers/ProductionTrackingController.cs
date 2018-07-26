using ProductionApp.BusinessService.Contracts;
using ProductionApp.DataAccessObject.DTO;
using ProductionApp.UserInterface.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Newtonsoft.Json;
using ProductionApp.UserInterface.SecurityFilter;
using System.Globalization;

namespace ProductionApp.UserInterface.Controllers
{
    public class ProductionTrackingController : Controller
    {

        #region Constructor Injection
        Settings settings = new Settings();
        Common _common = new Common();
        AppConst _appConst = new AppConst();
        private IProductionTrackingBusiness _productionTrackingBusiness;
        private IProductBusiness _productBusiness;
        private IEmployeeBusiness _employeeBusiness;
        private IStageBusiness _StageBusiness;
        public ProductionTrackingController(IProductionTrackingBusiness productionTrackingBusiness, IEmployeeBusiness employeeBusiness, IProductBusiness productBusiness, IStageBusiness StageBusiness)
        {
            _productionTrackingBusiness = productionTrackingBusiness;
            _productBusiness = productBusiness;
            _employeeBusiness = employeeBusiness;
            _StageBusiness = StageBusiness;
        }
        #endregion Constructor Injection

        // GET: ProductionTracking
        #region NewProductionTracking
        [AuthSecurityFilter(ProjectObject = "ProductionTracking", Mode = "R")]
        public ActionResult NewProductionTracking(string code, string id)
        {
            ViewBag.SysModuleCode = code;

            ProductionTrackingViewModel productionTrackingVM = new ProductionTrackingViewModel
            {
                ID = id == null ? Guid.Empty : Guid.Parse(id),
                IsUpdate = id == null ? false : true,
                EntryDateFormatted = _common.GetCurrentDateTime().ToString(settings.DateFormat),
                SubComponent = new SubComponentViewModel(),
                ForemanID = Guid.Empty,
                AcceptedQty = 0,
                AcceptedWt = 0,
                DamagedQty = 0,
                DamagedWt = 0
            };
            return View(productionTrackingVM);
        }
        #endregion NewProductionTracking

        #region ViewProductionTracking
        [AuthSecurityFilter(ProjectObject = "ProductionTracking", Mode = "R")]
        public ActionResult ViewProductionTracking(string code)
        {
            List<string> dateList = _productionTrackingBusiness.GetAllAvailableProductionTrackingEntryDate();
            ViewBag.SysModuleCode = code;
            List<StageViewModel> stageList = Mapper.Map<List<Stage>, List<StageViewModel>>(_StageBusiness.GetStageForSelectList());
            List<SelectListItem> selectList = new List<SelectListItem>();
            foreach(StageViewModel stage in stageList)
            {
                selectList.Add(new SelectListItem
                {
                    Text = stage.Description,
                    Value = stage.ID.ToString(),
                    Selected = false
                });
            }
            ProductionTrackingAdvanceSearchViewModel productionTrackingAdvanceSearchVM = new ProductionTrackingAdvanceSearchViewModel()
            {
                Stage = new StageViewModel()
                {
                    SelectList = selectList
                },
                Employee = new EmployeeViewModel()
                {
                    SelectList = _employeeBusiness.GetEmployeeSelectList()
                },
                Product = new ProductViewModel()
                {
                    ProductSelectList = _productBusiness.GetProductForSelectList("PRO")
                },
                PostDate = dateList!=null? DateTime.ParseExact(dateList[dateList.Count - 1], "dd-MM-yyyy", CultureInfo.InvariantCulture).ToString(settings.DateFormat):_common.GetCurrentDateTime().ToString(settings.DateFormat)
        };
            return View(productionTrackingAdvanceSearchVM);
        }
        #endregion ViewProductionTracking

        #region GetAllProductionTracking
        [AuthSecurityFilter(ProjectObject = "ProductionTracking", Mode = "R")]
        public ActionResult GetAllProductionTracking(DataTableAjaxPostModel model, ProductionTrackingAdvanceSearchViewModel productionTrackingAdvanceSearchVM)
        {
            try
            {
                productionTrackingAdvanceSearchVM.DataTablePaging.Start = model.start;
                productionTrackingAdvanceSearchVM.DataTablePaging.Length = (productionTrackingAdvanceSearchVM.DataTablePaging.Length == 0) ? model.length : productionTrackingAdvanceSearchVM.DataTablePaging.Length;
                List<ProductionTrackingViewModel> productionTrackingList = Mapper.Map<List<ProductionTracking>, List<ProductionTrackingViewModel>>(_productionTrackingBusiness.GetAllProductionTracking(Mapper.Map<ProductionTrackingAdvanceSearchViewModel, ProductionTrackingAdvanceSearch>(productionTrackingAdvanceSearchVM)));
                if (productionTrackingAdvanceSearchVM.DataTablePaging.Length == -1)
                {
                    int totalResult = productionTrackingList.Count != 0 ? productionTrackingList[0].TotalCount : 0;
                    int filteredResult = productionTrackingList.Count != 0 ? productionTrackingList[0].FilteredCount : 0;
                    productionTrackingList = productionTrackingList.Skip(0).Take(filteredResult > 10000 ? 10000 : filteredResult).ToList();
                }
                var settings = new JsonSerializerSettings
                {
                    //ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    Formatting = Formatting.None
                };

                return Json(new
                {
                    draw = model.draw,
                    recordsTotal = productionTrackingList.Count != 0 ? productionTrackingList[0].TotalCount : 0,
                    recordsFiltered = productionTrackingList.Count != 0 ? productionTrackingList[0].FilteredCount : 0,
                    data = productionTrackingList
                });
            }
            catch(Exception ex)
            {
                return Json(new
                {
                    message = ex.Message
                });
            }
        }
        #endregion GetAllProductionTracking

        #region GetProductionTrackingSearchList
        [AuthSecurityFilter(ProjectObject = "ProductionTracking", Mode = "R")]
        public string GetProductionTrackingSearchList(string searchTerm)
        {
            try
            {
                List<ProductionTrackingViewModel> productionTrackingList = Mapper.Map<List<ProductionTracking>, List<ProductionTrackingViewModel>>(_productionTrackingBusiness.GetProductionTrackingSearchList(searchTerm));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = productionTrackingList, Message = "Success" });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConst.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Record = "", Message = cm.Message });
            }
        }
        #endregion GetProductionTrackingSearchList

        #region InsertUpdateProductionTracking
        [AuthSecurityFilter(ProjectObject = "ProductionTracking", Mode = "R")]
        public string InsertUpdateProductionTracking(ProductionTrackingViewModel productionTrackingVM)
        {
            try
            {
                AppUA appUA = Session["AppUA"] as AppUA;
                productionTrackingVM.Common = new CommonViewModel
                {
                    CreatedBy = appUA.UserName,
                    CreatedDate = _common.GetCurrentDateTime(),
                    UpdatedBy = appUA.UserName,
                    UpdatedDate = _common.GetCurrentDateTime()
                };


                object result = _productionTrackingBusiness.InsertUpdateProductionTracking(Mapper.Map<ProductionTrackingViewModel, ProductionTracking>(productionTrackingVM));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = result, Message = _appConst.InsertSuccess });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConst.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Records = cm, Message = ex.Message });
            }
        }
        #endregion InsertUpdateProductionTracking

        #region DeleteProductionTracking
        [AuthSecurityFilter(ProjectObject = "ProductionTracking", Mode = "R")]
        public string DeleteProductionTracking(string id, string lineStageID)
        {
            try
            {
                AppUA appUA = Session["AppUA"] as AppUA;
                ProductionTrackingViewModel productionTrackingVM = new ProductionTrackingViewModel()
                {
                    ID = Guid.Parse(id),
                    LineStageDetailID = Guid.Parse(lineStageID)
                };
                productionTrackingVM.Common = new CommonViewModel
                {
                    CreatedBy = appUA.UserName,
                    CreatedDate = _common.GetCurrentDateTime(),
                    UpdatedBy = appUA.UserName,
                    UpdatedDate = _common.GetCurrentDateTime()
                };
                object result = _productionTrackingBusiness.DeleteProductionTracking(Mapper.Map<ProductionTrackingViewModel, ProductionTracking>(productionTrackingVM));
                return JsonConvert.SerializeObject(new { Result = "OK", Record = result, Message = _appConst.DeleteSuccess });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConst.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Record = "", Message = cm.Message });
            }
        }

        #endregion DeleteProductionTracking

        #region GetProductionTracking
        [AuthSecurityFilter(ProjectObject = "ProductionTracking", Mode = "R")]
        public string GetProductionTracking(string id)
        {
            try
            {
                ProductionTracking productionTracking = _productionTrackingBusiness.GetProductionTracking(Guid.Parse(id));
                return JsonConvert.SerializeObject(new { Result = "OK", Record = productionTracking, Message = _appConst.DeleteSuccess });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConst.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Record = "", Message = cm.Message });
            }
        }
        #endregion

        #region GetPendingProductionTracking
        [AuthSecurityFilter(ProjectObject = "ProductionTracking", Mode = "R")]
        public string GetPendingProductionTracking(string postDate)
        {
            try
            {
                List<ProductionTrackingViewModel> productionTrackingList = Mapper.Map<List<ProductionTracking>, List<ProductionTrackingViewModel>>(_productionTrackingBusiness.GetPendingProductionTracking(postDate));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = productionTrackingList, Message = "success" });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConst.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Records = "", Message = cm.Message });
            }
        }
        #endregion GetPendingProductionTracking

        #region GetPendingProductionTrackingDetail
        [AuthSecurityFilter(ProjectObject = "ProductionTracking", Mode = "R")]
        public string GetPendingProductionTrackingDetail(string postDate,string lineStageDetailID)
        {
            try
            {
                ProductionTrackingAdvanceSearchViewModel productionTrackingAdvanceSearchVM = new ProductionTrackingAdvanceSearchViewModel()
                {
                    PostDate = postDate,
                    LineStageDetailID= Guid.Parse(lineStageDetailID)
                };
                List<ProductionTrackingViewModel> productionTrackingList = Mapper.Map<List<ProductionTracking>, List<ProductionTrackingViewModel>>(_productionTrackingBusiness.GetPendingProductionTrackingDetail(Mapper.Map<ProductionTrackingAdvanceSearchViewModel, ProductionTrackingAdvanceSearch>(productionTrackingAdvanceSearchVM)));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = productionTrackingList, Message = "success" });

            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConst.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Records = "", Message = cm.Message });
            }
        }
        #endregion GetPendingProductionTrackingDetail

        #region GetAllAvailableProductionTrackingEntryDate
        public string GetAllAvailableProductionTrackingEntryDate()
        {
            try
            {
                List<string> dateList = _productionTrackingBusiness.GetAllAvailableProductionTrackingEntryDate();
                return JsonConvert.SerializeObject(new { Result = "OK", Records = dateList, Message = "success" });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConst.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Records = "", Message = cm.Message });
            }
        }
        #endregion GetAllAvailableProductionTrackingEntryDate

        #region UpdateProductionTrackingByXML
        public string UpdateProductionTrackingByXML(string productionTrackingJSONList)
        {
            try
            {
                AppUA appUA = Session["AppUA"] as AppUA;
                CommonViewModel common = new CommonViewModel
                {
                    UpdatedBy = appUA.UserName,
                    UpdatedDate = _common.GetCurrentDateTime(),
                };
                //Deserialize items
                object resultFromJS = JsonConvert.DeserializeObject(productionTrackingJSONList);
                string readableFormat = JsonConvert.SerializeObject(resultFromJS);
                List<ProductionTrackingViewModel> productionTrackingList = JsonConvert.DeserializeObject<List<ProductionTrackingViewModel>>(readableFormat);
                object result = _productionTrackingBusiness.UpdateProductionTrackingByXML(Mapper.Map<CommonViewModel, Common>(common), Mapper.Map<List<ProductionTrackingViewModel>, List<ProductionTracking>>(productionTrackingList));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = result, Message = _appConst.UpdateSuccess });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConst.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Records = "", Message = cm.Message });
            }
        }
        #endregion UpdateProductionTrackingByXML

        #region ProductionTrackingPosting
        public string ProductionTrackingPosting(string postDate)
        {
            try
            {
                AppUA appUA = Session["AppUA"] as AppUA;
                CommonViewModel common = new CommonViewModel
                {
                    UpdatedBy = appUA.UserName,
                    UpdatedDate = _common.GetCurrentDateTime(),
                };
                object result = _productionTrackingBusiness.ProductionTrackingPosting(Mapper.Map<CommonViewModel, Common>(common), postDate);
                return JsonConvert.SerializeObject(new { Result = "OK", Records = result, Message = "success" });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConst.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Records = "", Message = cm.Message });
            }
        }
        #endregion ProductionTrackingPosting

        #region ButtonStyling
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "ProductionTracking", Mode = "R")]
        public ActionResult ChangeButtonStyle(string actionType)
        {
            ToolboxViewModel toolboxVM = new ToolboxViewModel();
            switch (actionType)
            {
                case "List":
                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "Add";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Href = Url.Action("NewProductionTracking", "ProductionTracking", new { code = "PROD" });
                    toolboxVM.addbtn.Event = "";

                    toolboxVM.resetbtn.Visible = true;
                    toolboxVM.resetbtn.Text = "Reset";
                    toolboxVM.resetbtn.Title = "Reset All";
                    toolboxVM.resetbtn.Event = "BindOrReloadProductionTrackingTable('Reset');";

                    toolboxVM.PrintBtn.Visible = true;
                    toolboxVM.PrintBtn.Text = "Export";
                    toolboxVM.PrintBtn.Title = "Export";
                    toolboxVM.PrintBtn.Event = "BindOrReloadProductionTrackingTable('Export');";

                    toolboxVM.PostBtn.Visible = true;
                    toolboxVM.PostBtn.Text = "Post";
                    toolboxVM.PostBtn.Title = "Post to Ledger";
                    toolboxVM.PostBtn.Event = "LoadPendingProductionTrackingPopUp()";

                    break;
                case "Edit":
                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "Add";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Href = Url.Action("NewProductionTracking", "ProductionTracking", new { code = "PROD" });
                    toolboxVM.addbtn.Event = "";

                    toolboxVM.resetbtn.Visible = true;
                    toolboxVM.resetbtn.Text = "Reset";
                    toolboxVM.resetbtn.Title = "Reset All";
                    toolboxVM.resetbtn.Event = "Reset();";

                    toolboxVM.ListBtn.Visible = true;
                    toolboxVM.ListBtn.Text = "List";
                    toolboxVM.ListBtn.Title = "List";
                    toolboxVM.ListBtn.Href = Url.Action("ViewProductionTracking", "ProductionTracking", new { code = "PROD" });
                    toolboxVM.ListBtn.Event = "";

                    toolboxVM.savebtn.Visible = true;
                    toolboxVM.savebtn.Text = "Save";
                    toolboxVM.savebtn.Title = "Save";
                    toolboxVM.savebtn.Event = "Save()";

                    toolboxVM.deletebtn.Visible = true;
                    toolboxVM.deletebtn.Text = "Delete";
                    toolboxVM.deletebtn.Title = "Delete";
                    toolboxVM.deletebtn.Event = "DeleteClick()";
                    break;
                case "Add":
                    toolboxVM.ListBtn.Visible = true;
                    toolboxVM.ListBtn.Text = "List";
                    toolboxVM.ListBtn.Title = "List";
                    toolboxVM.ListBtn.Href = Url.Action("ViewProductionTracking", "ProductionTracking", new { code = "PROD" });
                    toolboxVM.ListBtn.Event = "";

                    toolboxVM.savebtn.Visible = true;
                    toolboxVM.savebtn.Text = "Save";
                    toolboxVM.savebtn.Title = "Save";
                    toolboxVM.savebtn.Event = "Save()";
                    break;
                default:
                    return Content("Nochange");
            }
            return PartialView("ToolboxView", toolboxVM);
        }
        #endregion ButtonStyling
    }
}