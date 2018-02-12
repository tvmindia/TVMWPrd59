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
        // GET: View Bank
        public ActionResult ViewBank(string Code)
        {
            ViewBag.SysModuleCode = Code;
            BankViewModel bankVM = new BankViewModel();
            return View(bankVM);
        }
        // GET: Add Bank
        public ActionResult AddBank(string Code)
        {
            ViewBag.SysModuleCode = Code;
            BankViewModel bankVM = new BankViewModel();
            return View(bankVM);
        }
        [HttpPost]
        public JsonResult GetAllBank(DataTableAjaxPostModel model,BankAdvanceSearchViewModel bankAdvanceSearchVM)
        {
            //setting options to our model
            bankAdvanceSearchVM.Start = model.start;
            bankAdvanceSearchVM.Length = (bankAdvanceSearchVM.Length==0)?model.length: bankAdvanceSearchVM.Length;
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
       
        #region ButtonStyling
        [HttpGet]
        //[AuthSecurityFilter(ProjectObject = "Bank", Mode = "R")]
        public ActionResult ChangeButtonStyle(string actionType)
        {
            ToolboxViewModel toolboxVMObj = new ToolboxViewModel();
            switch (actionType)
            {
                case "List":
                    toolboxVMObj.addbtn.Visible = true;
                    toolboxVMObj.addbtn.Text = "Add";
                    toolboxVMObj.addbtn.Title = "Add New";
                    toolboxVMObj.addbtn.Href = Url.Action("AddBank", "Bank", new { Code = "SETT" });
                    toolboxVMObj.addbtn.Event = "";
                    //----added for reset button---------------
                    toolboxVMObj.resetbtn.Visible = true;
                    toolboxVMObj.resetbtn.Text = "Reset";
                    toolboxVMObj.resetbtn.Title = "Reset All";
                    toolboxVMObj.resetbtn.Event = "ResetBankList();";
                    //----added for export button--------------
                    toolboxVMObj.PrintBtn.Visible = true;
                    toolboxVMObj.PrintBtn.Text = "Export";
                    toolboxVMObj.PrintBtn.Title = "Export";
                    toolboxVMObj.PrintBtn.Event = "PrintReport();";
                    //---------------------------------------

                    break;
                case "Edit":

                    toolboxVMObj.addbtn.Visible = true;
                    toolboxVMObj.addbtn.Text = "New";
                    toolboxVMObj.addbtn.Title = "Add New";
                    toolboxVMObj.addbtn.Event = "openNav();";

                    toolboxVMObj.savebtn.Visible = true;
                    toolboxVMObj.savebtn.Text = "Save";
                    toolboxVMObj.savebtn.Title = "Save Bank";
                    toolboxVMObj.savebtn.Event = "Save();";

                    toolboxVMObj.deletebtn.Visible = true;
                    toolboxVMObj.deletebtn.Text = "Delete";
                    toolboxVMObj.deletebtn.Title = "Delete Bank";
                    toolboxVMObj.deletebtn.Event = "Delete()";

                    toolboxVMObj.resetbtn.Visible = true;
                    toolboxVMObj.resetbtn.Text = "Reset";
                    toolboxVMObj.resetbtn.Title = "Reset";
                    toolboxVMObj.resetbtn.Event = "Reset();";

                    toolboxVMObj.CloseBtn.Visible = true;
                    toolboxVMObj.CloseBtn.Text = "Close";
                    toolboxVMObj.CloseBtn.Title = "Close";
                    toolboxVMObj.CloseBtn.Event = "closeNav();";

                    break;
                case "Add":

                    toolboxVMObj.savebtn.Visible = true;
                    toolboxVMObj.savebtn.Text = "Save";
                    toolboxVMObj.savebtn.Title = "Save";
                    toolboxVMObj.savebtn.Event = "Save();";

                    toolboxVMObj.CloseBtn.Visible = true;
                    toolboxVMObj.CloseBtn.Text = "Close";
                    toolboxVMObj.CloseBtn.Title = "Close";
                    toolboxVMObj.CloseBtn.Event = "closeNav();";

                    toolboxVMObj.resetbtn.Visible = false;
                    toolboxVMObj.resetbtn.Text = "Reset";
                    toolboxVMObj.resetbtn.Title = "Reset";
                    toolboxVMObj.resetbtn.Event = "Reset();";

                    toolboxVMObj.deletebtn.Visible = false;
                    toolboxVMObj.deletebtn.Text = "Delete";
                    toolboxVMObj.deletebtn.Title = "Delete Bank";
                    toolboxVMObj.deletebtn.Event = "Delete()";

                    toolboxVMObj.addbtn.Visible = false;
                    toolboxVMObj.addbtn.Text = "New";
                    toolboxVMObj.addbtn.Title = "Add New";
                    toolboxVMObj.addbtn.Event = "openNav();";

                    break;
                case "AddSub":

                    break;
                case "tab1":

                    break;
                case "tab2":

                    break;
                default:
                    return Content("Nochange");
            }
            return PartialView("ToolboxView", toolboxVMObj);
        }

        #endregion
    }
}