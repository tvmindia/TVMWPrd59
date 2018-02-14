using AutoMapper;
using Newtonsoft.Json;
using ProductionApp.BusinessService.Contracts;
using ProductionApp.DataAccessObject.DTO;
using ProductionApp.UserInterface.Models;
using ProductionApp.UserInterface.SecurityFilter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProductionApp.UserInterface.Controllers
{
    public class BankController : Controller
    {
        private IBankBusiness _bankBusiness;
        private Common _common = new Common();
        public BankController(IBankBusiness bankBusiness)
        {
            _bankBusiness = bankBusiness;
        }
        [AuthSecurityFilter(ProjectObject = "Bank", Mode = "")]
        public ActionResult Index(string code)
        {
            ViewBag.SysModuleCode = code;
            BankAdvanceSearchViewModel bankAdvanceSearchVM = new BankAdvanceSearchViewModel();
            return View(bankAdvanceSearchVM);
        }

        [AcceptVerbs("Get", "Post")]
        public ActionResult CheckCodeExist(BankViewModel bankVM)
        {
            bool exists = bankVM.IsUpdate ? false : _bankBusiness.CheckCodeExist(bankVM.Code);
            if (exists)
            {
                return Json("<p><span style='vertical-align: 2px'>Bank code already in use </span> <i class='fa fa-close' style='font-size:19px; color: red'></i></p>", JsonRequestBehavior.AllowGet);
            }
            //var result = new { success = true, message = "Success" };
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AuthSecurityFilter(ProjectObject = "Bank", Mode = "")]
        public JsonResult GetAllBank(DataTableAjaxPostModel model,BankAdvanceSearchViewModel bankAdvanceSearchVM)
        {
            //setting options to our model
            bankAdvanceSearchVM.DataTablePaging.Start = model.start;
            bankAdvanceSearchVM.DataTablePaging.Length = (bankAdvanceSearchVM.DataTablePaging.Length==0)?model.length: bankAdvanceSearchVM.DataTablePaging.Length;
            
            //bankAdvanceSearchVM.OrderColumn = model.order[0].column;
            //bankAdvanceSearchVM.OrderDir = model.order[0].dir;

            // action inside a standard controller
            List<BankViewModel> bankVMList = Mapper.Map<List<Bank>, List<BankViewModel>>(_bankBusiness.GetAllBank(Mapper.Map<BankAdvanceSearchViewModel, BankAdvanceSearch>(bankAdvanceSearchVM)));
            
            var settings = new JsonSerializerSettings
            {
                //ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Formatting = Formatting.None
            };
            return Json(new
            {
                // this is what datatables wants sending back
                draw = model.draw,
                recordsTotal = bankVMList.Count != 0 ? bankVMList[0].TotalCount : 0,
                recordsFiltered = bankVMList.Count != 0 ? bankVMList[0].FilteredCount : 0,
                data = bankVMList
            });
        }
        [HttpPost]
        [AuthSecurityFilter(ProjectObject = "Bank", Mode = "")]
        public string InsertUpdateBank(BankViewModel bankVM)
        {
            AppUA appUA = Session["AppUA"] as AppUA;
            bankVM.Common = new CommonViewModel
            {
               CreatedBy = appUA.UserName,
               CreatedDate = _common.GetCurrentDateTime(),
               UpdatedBy = appUA.UserName,
               UpdatedDate = _common.GetCurrentDateTime(),
           };            
            var result = _bankBusiness.InsertUpdateBank(Mapper.Map<BankViewModel,Bank > (bankVM));
            return JsonConvert.SerializeObject(new { Result = "OK", Records = result });
        }
        #region MasterPartial
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "Bank", Mode = "")]
        public ActionResult MasterPartial(string masterCode)
        {
            BankViewModel bankVM = string.IsNullOrEmpty(masterCode)?new BankViewModel(): Mapper.Map <Bank, BankViewModel>(_bankBusiness.GetBank(masterCode));
            bankVM.IsUpdate = string.IsNullOrEmpty(masterCode) ? false : true;
            return PartialView("_AddBankPartial", bankVM);
        }

        #endregion
        #region ButtonStyling
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "Bank", Mode = "")]
        public ActionResult ChangeButtonStyle(string actionType)
        {
            ToolboxViewModel toolboxVM = new ToolboxViewModel();
            switch (actionType)
            {
                case "List":
                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "Add";
                    toolboxVM.addbtn.Title = "Add New";
                    //toolboxVM.addbtn.Href = Url.Action("AddBank", "Bank", new { code = "SETT" });
                    toolboxVM.addbtn.Event = "AddBankMaster()";
                    //----added for reset button---------------
                    toolboxVM.resetbtn.Visible = true;
                    toolboxVM.resetbtn.Text = "Reset";
                    toolboxVM.resetbtn.Title = "Reset All";
                    toolboxVM.resetbtn.Event = "ResetBankList();";
                    //----added for export button--------------
                    toolboxVM.PrintBtn.Visible = true;
                    toolboxVM.PrintBtn.Text = "Export";
                    toolboxVM.PrintBtn.Title = "Export";
                    toolboxVM.PrintBtn.Event = "ImportBankData();";
                    //---------------------------------------

                    break;
                case "Edit":

                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "New";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Event = "openNav();";

                    toolboxVM.savebtn.Visible = true;
                    toolboxVM.savebtn.Text = "Save";
                    toolboxVM.savebtn.Title = "Save Bank";
                    toolboxVM.savebtn.Event = "Save();";

                    toolboxVM.deletebtn.Visible = true;
                    toolboxVM.deletebtn.Text = "Delete";
                    toolboxVM.deletebtn.Title = "Delete Bank";
                    toolboxVM.deletebtn.Event = "Delete()";

                    toolboxVM.resetbtn.Visible = true;
                    toolboxVM.resetbtn.Text = "Reset";
                    toolboxVM.resetbtn.Title = "Reset";
                    toolboxVM.resetbtn.Event = "Reset();";

                    toolboxVM.CloseBtn.Visible = true;
                    toolboxVM.CloseBtn.Text = "Close";
                    toolboxVM.CloseBtn.Title = "Close";
                    toolboxVM.CloseBtn.Event = "closeNav();";

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
                    toolboxVM.ListBtn.Href = Url.Action("ViewBank", "Bank", new { Code = "SETT" });


                    break;
                default:
                    return Content("Nochange");
            }
            return PartialView("ToolboxView", toolboxVM);
        }

        #endregion
    }
}