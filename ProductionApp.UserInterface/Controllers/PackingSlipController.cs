using AutoMapper;
using Newtonsoft.Json;
using ProductionApp.BusinessService.Contracts;
using ProductionApp.DataAccessObject.DTO;
using ProductionApp.UserInterface.Models;
using ProductionApp.UserInterface.SecurityFilter;
using SAMTool.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProductionApp.UserInterface.Controllers
{
    public class PackingSlipController : Controller
    {
        // GET: PackingSlip
        private IPackingSlipBusiness _packingSlipBusiness;
        private IEmployeeBusiness _employeeBusiness;
        private ISalesOrderBusiness _salesOrderBusiness;
        DataAccessObject.DTO.Common _common = new DataAccessObject.DTO.Common();
        AppConst _appConst = new AppConst();
        public PackingSlipController(IEmployeeBusiness employeeBusiness, IPackingSlipBusiness packingSlipBusiness, ISalesOrderBusiness salesOrderBusiness)
        {
            _employeeBusiness = employeeBusiness;
            _packingSlipBusiness = packingSlipBusiness;
            _salesOrderBusiness = salesOrderBusiness;
        }

        #region AddPackingSlip
        [AuthSecurityFilter(ProjectObject = "PackingSlip", Mode = "R")]
        public ActionResult AddPackingSlip(string code, Guid? id)
        {
            ViewBag.SysModuleCode = code;
            PackingSlipViewModel PackingSlipVM = new PackingSlipViewModel();
            PackingSlipVM.ID = id == null ? Guid.Empty : (Guid)id;
            PackingSlipVM.IsUpdate = id == null ? false : true;
            List<SelectListItem> selectListItem = new List<SelectListItem>();
            PackingSlipVM.Employee = new EmployeeViewModel();
            PackingSlipVM.Employee.SelectList = new List<SelectListItem>();
            List<EmployeeViewModel> employeeList = Mapper.Map<List<Employee>, List<EmployeeViewModel>>(_employeeBusiness.GetEmployeeForSelectList());
            if (employeeList != null)
            {
                foreach (EmployeeViewModel Emp in employeeList)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Text = Emp.Name,
                        Value = Emp.ID.ToString(),
                        Selected = false,
                    });

                }
            }
            PackingSlipVM.Employee.SelectList = selectListItem;
            selectListItem = new List<SelectListItem>();
            PackingSlipVM.SalesOrder = new SalesOrderViewModel();
            PackingSlipVM.SalesOrder.SelectList = new List<SelectListItem>();
            List< SalesOrderViewModel> salesOrderList= Mapper.Map<List<SalesOrder>, List<SalesOrderViewModel>>(_salesOrderBusiness.GetAllSalesOrderForSelectList());
            if (salesOrderList != null)
            {
                foreach (SalesOrderViewModel Emp in salesOrderList)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Text = Emp.OrderNo,
                        Value = Emp.ID.ToString(),
                        Selected = false,
                    });

                }
            }
            PackingSlipVM.SalesOrder.SelectList = selectListItem;
            Permission _permission = Session["UserRights"] as Permission;
            string pkgAccess = _permission.SubPermissionList.Where(li => li.Name == "Packer").First().AccessCode;
            string disAccess = _permission.SubPermissionList.Where(li => li.Name == "Dispatcher").First().AccessCode;
            if (pkgAccess.Contains("R") || pkgAccess.Contains("W"))
            {
                PackingSlipVM.ShowPkgSec = true;
                PackingSlipVM.ShowDispatcherSec = false;
            }
            if (disAccess.Contains("R") || disAccess.Contains("W"))
            {
                PackingSlipVM.ShowPkgSec = false;
                PackingSlipVM.ShowDispatcherSec = true;
            }
            if (pkgAccess.Contains("RWD") && disAccess.Contains("RWD"))
            {
                PackingSlipVM.ShowPkgSec = true;
                PackingSlipVM.ShowDispatcherSec = true;
            }
            return View(PackingSlipVM);
        }
        #endregion AddPackingSlip

        #region ListPackingSlips
        public ActionResult ListPackingSlips(string code)
        {
            ViewBag.SysModuleCode = code;
            PackingSlipAdvanceSearchViewModel PackingSlipAdvanceSearchVM = new PackingSlipAdvanceSearchViewModel();
            List<SelectListItem> selectListItem = new List<SelectListItem>();
            PackingSlipAdvanceSearchVM.Employee = new EmployeeViewModel();
            PackingSlipAdvanceSearchVM.Employee.SelectList = new List<SelectListItem>();
            List<EmployeeViewModel> employeeList = Mapper.Map<List<Employee>, List<EmployeeViewModel>>(_employeeBusiness.GetEmployeeForSelectList());
            if (employeeList != null)
            {
                foreach (EmployeeViewModel Emp in employeeList)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Text = Emp.Name,
                        Value = Emp.ID.ToString(),
                        Selected = false,
                    });

                }
            }
            PackingSlipAdvanceSearchVM.Employee.SelectList = selectListItem;
            return View(PackingSlipAdvanceSearchVM);
        }
        #endregion ListPackingSlips

        #region InsertUpdatePackingSlip
        [AuthSecurityFilter(ProjectObject = "PackingSlip", Mode = "R")]
        public string InsertUpdatePackingSlip(PackingSlipViewModel packingSlipVM)
        {

            try
            {
                AppUA appUA = Session["AppUA"] as AppUA;
                packingSlipVM.Common = new CommonViewModel
                {
                    CreatedBy = appUA.UserName,
                    CreatedDate = _common.GetCurrentDateTime(),
                    UpdatedBy = appUA.UserName,
                    UpdatedDate = _common.GetCurrentDateTime(),
                };
                //Deserialize items
                object ResultFromJS = JsonConvert.DeserializeObject(packingSlipVM.DetailJSON);
                string ReadableFormat = JsonConvert.SerializeObject(ResultFromJS);
                packingSlipVM.PackingSlipDetailList = JsonConvert.DeserializeObject<List<PackingSlipDetailViewModel>>(ReadableFormat);
                var result = _packingSlipBusiness.InsertUpdatePackingSlip(Mapper.Map<PackingSlipViewModel, PackingSlip>(packingSlipVM));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = result, Message = "Success" });


                //selectListItem = new List<SelectListItem>();

            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConst.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Records = "", Message = cm.Message });
            }

        }
        #endregion InsertUpdatePackingSlip

        #region GetAllPackingSlip
        [HttpPost]
        
        public JsonResult GetAllPackingSlip(DataTableAjaxPostModel model, PackingSlipAdvanceSearchViewModel packingSlipAdvanceSearchVM)
        {
            packingSlipAdvanceSearchVM.DataTablePaging.Start = model.start;
            packingSlipAdvanceSearchVM.DataTablePaging.Length = (packingSlipAdvanceSearchVM.DataTablePaging.Length == 0 ? model.length : packingSlipAdvanceSearchVM.DataTablePaging.Length);
            List<PackingSlipViewModel> packingSlipList = Mapper.Map<List<PackingSlip>, List<PackingSlipViewModel>>(_packingSlipBusiness.GetAllPackingSlip(Mapper.Map<PackingSlipAdvanceSearchViewModel, PackingSlipAdvanceSearch>(packingSlipAdvanceSearchVM)));
            if (packingSlipAdvanceSearchVM.DataTablePaging.Length == -1)
            {
                int totalResult = packingSlipList.Count != 0 ? packingSlipList[0].TotalCount : 0;
                int filteredResult = packingSlipList.Count != 0 ? packingSlipList[0].FilteredCount : 0;
                packingSlipList = packingSlipList.Skip(0).Take(filteredResult > 10000 ? 10000 : filteredResult).ToList();
            }

            return Json(new
            {
                draw = model.draw,
                recordsTotal = packingSlipList.Count != 0 ? packingSlipList[0].TotalCount : 0,
                recordsFiltered = packingSlipList.Count != 0 ? packingSlipList[0].FilteredCount : 0,
                data = packingSlipList
            });
        }
        #endregion GetAllPakingSlip

        #region GetPkgSlipByID
        [AuthSecurityFilter(ProjectObject = "PackingSlip", Mode = "R")]
        public string GetPkgSlipByID(string id)
        {
            try
            {
                PackingSlipViewModel packingSlipVM = new PackingSlipViewModel();
                packingSlipVM = Mapper.Map<PackingSlip, PackingSlipViewModel>(_packingSlipBusiness.GetPkgSlipByID(Guid.Parse(id)));
                return JsonConvert.SerializeObject(new { Result = "OK", Record = packingSlipVM, Message = "Success" });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Record = "", Message = ex });
            }
        }
        #endregion GetPkgSlipByID

        #region GetPkgSlipDetailByID
        [AuthSecurityFilter(ProjectObject = "PackingSlip", Mode = "R")]
        public string GetPkgSlipDetailByID(string id)
        {
            try
            {
                List<PackingSlipDetailViewModel> pkgSlipDetailVM = new List<PackingSlipDetailViewModel>();
                pkgSlipDetailVM = Mapper.Map<List<PackingSlipDetail>, List<PackingSlipDetailViewModel>>(_packingSlipBusiness.GetPkgSlipDetailByID(Guid.Parse(id)));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = pkgSlipDetailVM, Message = "Success" });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Records = "", Message = ex });
            }
        }
        #endregion GetPkgSlipDetailByID

        #region GetProductList
        public string GetProductList(string id)
        {
            try
            {

                List<SalesOrderDetailViewModel> salesOrderDetailVM = Mapper.Map<List<SalesOrderDetail>, List<SalesOrderDetailViewModel>>(_salesOrderBusiness.GetSalesOrderProductList(Guid.Parse(id)));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = salesOrderDetailVM, Message = "Success" });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Records = "", Message = ex });
            }
        }
        #endregion GetProductList

        #region ButtonStyling
        [HttpGet]
        public ActionResult ChangeButtonStyle(string actionType)
        {
            ToolboxViewModel toolboxVM = new ToolboxViewModel();
            switch (actionType)
            {
                case "List":
                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "Add";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Href = Url.Action("AddPackingSlip", "PackingSlip", new { code = "SALE" });
                    //----added for reset button---------------
                    toolboxVM.resetbtn.Visible = true;
                    toolboxVM.resetbtn.Text = "Reset";
                    toolboxVM.resetbtn.Title = "Reset All";
                    toolboxVM.resetbtn.Event = "ResetPackingSlipList();";
                    //----added for export button--------------
                    toolboxVM.PrintBtn.Visible = true;
                    toolboxVM.PrintBtn.Text = "Export";
                    toolboxVM.PrintBtn.Title = "Export";
                    toolboxVM.PrintBtn.Event = "ImportPackingSlipData();";
                    //---------------------------------------

                    break;
                case "Edit":

                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "New";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Href = Url.Action("AddPackingSlip", "PackingSlip", new { code = "SALE" });

                    toolboxVM.savebtn.Visible = true;
                    toolboxVM.savebtn.Text = "Save";
                    toolboxVM.savebtn.Title = "Save";
                    toolboxVM.savebtn.Event = "Save();";

                    toolboxVM.deletebtn.Visible = true;
                    toolboxVM.deletebtn.Text = "Delete";
                    toolboxVM.deletebtn.Title = "Delete";
                    toolboxVM.deletebtn.Event = "DeleteClick()";

                    toolboxVM.resetbtn.Visible = true;
                    toolboxVM.resetbtn.Text = "Reset";
                    toolboxVM.resetbtn.Title = "Reset";
                    toolboxVM.resetbtn.Event = "Reset();";

                    toolboxVM.ListBtn.Visible = true;
                    toolboxVM.ListBtn.Text = "List";
                    toolboxVM.ListBtn.Title = "List";
                    toolboxVM.ListBtn.Event = "";
                    toolboxVM.ListBtn.Href = Url.Action("ListPackingSlips", "PackingSlip", new { Code = "SALE" });


                    break;
                case "Add":

                    toolboxVM.savebtn.Visible = true;
                    toolboxVM.savebtn.Text = "Save";
                    toolboxVM.savebtn.Title = "Save";
                    toolboxVM.savebtn.Event = "Save();";

                    toolboxVM.ListBtn.Visible = true;
                    toolboxVM.ListBtn.Text = "List";
                    toolboxVM.ListBtn.Title = "List";
                    toolboxVM.ListBtn.Event = "";
                    toolboxVM.ListBtn.Href = Url.Action("ListPackingSlips", "PackingSlip", new { Code = "SALE" });


                    break;
                default:
                    return Content("Nochange");
            }
            return PartialView("ToolboxView", toolboxVM);
        }

        #endregion

    }
}