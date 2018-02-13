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
        public BankController(IBankBusiness bankBusiness)
        {
            _bankBusiness = bankBusiness;
        }
        public ActionResult Index(string code)
        {
            ViewBag.SysModuleCode = code;
            BankAdvanceSearchViewModel bankAdvanceSearchVM = new BankAdvanceSearchViewModel();
            return View(bankAdvanceSearchVM);
        }        
        [HttpPost]
        public JsonResult GetAllBank(DataTableAjaxPostModel model,BankAdvanceSearchViewModel bankAdvanceSearchVM)
        {
            //setting options to our model
            bankAdvanceSearchVM.DataTablePaging.Start = model.start;
            bankAdvanceSearchVM.DataTablePaging.Length = (bankAdvanceSearchVM.DataTablePaging.Length==0)?model.length: bankAdvanceSearchVM.DataTablePaging.Length;
            
            //bankAdvanceSearchVM.OrderColumn = model.order[0].column;
            //bankAdvanceSearchVM.OrderDir = model.order[0].dir;

            // action inside a standard controller
            List<BankViewModel> bankObjList = Mapper.Map<List<Bank>, List<BankViewModel>>(_bankBusiness.GetAllBank(Mapper.Map<BankAdvanceSearchViewModel, BankAdvanceSearch>(bankAdvanceSearchVM)));
            
            var settings = new JsonSerializerSettings
            {
                //ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Formatting = Formatting.None
            };
            return Json(new
            {
                // this is what datatables wants sending back
                draw = model.draw,
                recordsTotal = bankObjList.Count != 0 ? bankObjList[0].TotalCount : 0,
                recordsFiltered = bankObjList.Count != 0 ? bankObjList[0].FilteredCount : 0,
                data = bankObjList
            });
        }
       [HttpPost]
       public object InsertUpdateBank(BankViewModel bankVM)
        {
            var result = _bankBusiness.InsertUpdateBank(Mapper.Map<BankViewModel,Bank > (bankVM));
            return result;
        }
        #region MasterPartial
        [HttpGet]
        //[AuthSecurityFilter(ProjectObject = "Bank", Mode = "R")]
        public ActionResult AddMasterPartial(string masterCode)
        {
            BankViewModel bankVM = string.IsNullOrEmpty(masterCode)?new BankViewModel(): Mapper.Map <Bank, BankViewModel>(_bankBusiness.GetBank(masterCode));            
            return PartialView("_AddBankPartial", bankVM);
        }

        #endregion
        #region ButtonStyling
        [HttpGet]
        //[AuthSecurityFilter(ProjectObject = "Bank", Mode = "R")]
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