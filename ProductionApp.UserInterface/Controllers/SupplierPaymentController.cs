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
    public class SupplierPaymentController : Controller
    {
        private ISupplierPaymentBusiness _supplierPaymentBusiness;
        private ISupplierBusiness _supplierBusiness;
        private ISupplierCreditNoteBusiness _supplierCreditNoteBusiness;
        //  private IChartOfAccountBusiness _chartOfAccountBusiness;
        //  private IPaymentTermBusiness _paymentTermBusiness;
        //  private IPurchaseOrderBusiness _purchaseOrderBusiness;

        Common _common = new Common();
        AppConst _appConst = new AppConst();

        public SupplierPaymentController(ISupplierPaymentBusiness supplierPaymentBusiness,
            ISupplierBusiness supplierBusiness,ISupplierCreditNoteBusiness supplierCreditNoteBusiness)
        {
            _supplierPaymentBusiness = supplierPaymentBusiness;
            _supplierBusiness = supplierBusiness;
            _supplierCreditNoteBusiness = supplierCreditNoteBusiness;
        }
        // GET: SupplierPayment
        [AuthSecurityFilter(ProjectObject = "SupplierPayments", Mode = "R")]
        public ActionResult ViewSupplierPayment(string code)
        {
            ViewBag.SysModuleCode = code;
            SupplierPaymentAdvanceSearchViewModel supplierPaymentAdvanceSearchVM = new SupplierPaymentAdvanceSearchViewModel();
            supplierPaymentAdvanceSearchVM.Supplier = new SupplierViewModel();
            List<SelectListItem> selectListItem = new List<SelectListItem>();
            supplierPaymentAdvanceSearchVM.Supplier.SelectList = new List<SelectListItem>();
            List<SupplierViewModel> supplierList = Mapper.Map<List<Supplier>, List<SupplierViewModel>>(_supplierBusiness.GetSupplierForSelectList());
            if (supplierList != null)
                foreach (SupplierViewModel supplier in supplierList)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Text = supplier.CompanyName,
                        Value = supplier.ID.ToString(),
                        Selected = false
                    });
                }
            supplierPaymentAdvanceSearchVM.Supplier.SelectList = selectListItem;
            return View(supplierPaymentAdvanceSearchVM);
        }
        [AuthSecurityFilter(ProjectObject = "SupplierPayments", Mode = "R")]
        public ActionResult NewSupplierPayment(string code, Guid? id)
        {
            ViewBag.SysModuleCode = code;
            SupplierPaymentViewModel supplierPaymentVM = new SupplierPaymentViewModel
            {
                ID = id == null ? Guid.Empty : (Guid)id,
                IsUpdate = id == null ? false : true,
            };
            supplierPaymentVM.Supplier = new SupplierViewModel();
            List<SelectListItem> selectListItem = new List<SelectListItem>();
            supplierPaymentVM.Supplier.SelectList = new List<SelectListItem>();
            List<SupplierViewModel> supplierList = Mapper.Map<List<Supplier>, List<SupplierViewModel>>(_supplierBusiness.GetSupplierForSelectList());
            if (supplierList != null)
                foreach (SupplierViewModel supplier in supplierList)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Text = supplier.CompanyName,
                        Value = supplier.ID.ToString(),
                        Selected = false
                    });
                }
            supplierPaymentVM.Supplier.SelectList = selectListItem;
            return View(supplierPaymentVM);
        }

       
        #region GetAllSupplierPayment
        [HttpPost]
        [AuthSecurityFilter(ProjectObject = "SupplierPayments", Mode = "R")]
        public JsonResult GetAllSupplierPayment(DataTableAjaxPostModel model, SupplierPaymentAdvanceSearchViewModel supplierPaymentAdvanceSearchVM)
        {
            supplierPaymentAdvanceSearchVM.DataTablePaging.Start = model.start;
            supplierPaymentAdvanceSearchVM.DataTablePaging.Length = (supplierPaymentAdvanceSearchVM.DataTablePaging.Length == 0 ? model.length : supplierPaymentAdvanceSearchVM.DataTablePaging.Length);
            List<SupplierPaymentViewModel> SupplierPaymentList = Mapper.Map<List<SupplierPayment>, List<SupplierPaymentViewModel>>(_supplierPaymentBusiness.GetAllSupplierPayment(Mapper.Map<SupplierPaymentAdvanceSearchViewModel, SupplierPaymentAdvanceSearch>(supplierPaymentAdvanceSearchVM)));
            if (supplierPaymentAdvanceSearchVM.DataTablePaging.Length == -1)
            {
                int totalResult = SupplierPaymentList.Count != 0 ? SupplierPaymentList[0].TotalCount : 0;
                int filteredResult = SupplierPaymentList.Count != 0 ? SupplierPaymentList[0].FilteredCount : 0;
                SupplierPaymentList = SupplierPaymentList.Skip(0).Take(filteredResult > 10000 ? 10000 : filteredResult).ToList();
            }

            return Json(new
            {
                draw = model.draw,
                recordsTotal = SupplierPaymentList.Count != 0 ? SupplierPaymentList[0].TotalCount : 0,
                recordsFiltered = SupplierPaymentList.Count != 0 ? SupplierPaymentList[0].FilteredCount : 0,
                data = SupplierPaymentList
            });
        }
        #endregion

        #region InsertUpdateSupplierPayment
        [AuthSecurityFilter(ProjectObject = "SupplierPayments", Mode = "W")]
        [HttpPost]
        public string InsertUpdateSupplierPayment(SupplierPaymentViewModel SupplierPaymentVM)
        {
           

   

            try
            {
                if ((SupplierPaymentVM.TotalPaidAmt == 0 || SupplierPaymentVM.TotalPaidAmt == null) && SupplierPaymentVM.Type == "C" || SupplierPaymentVM.hdfType == "C")
                {
                    SupplierPaymentVM.TotalPaidAmt = Decimal.Parse(SupplierPaymentVM.hdfCreditAmount);
                    SupplierPaymentVM.AdvanceAmount = 0;
                    if (SupplierPaymentVM.TotalPaidAmt == 0)
                    {
                        throw new Exception("Please Check Credit Notes");
                    }
                }
                else if (SupplierPaymentVM.TotalPaidAmt == 0)
                {
                    throw new Exception("Please Enter Amount");
                }

                AppUA appUA = Session["AppUA"] as AppUA;
                SupplierPaymentVM.Common = new CommonViewModel
                {
                    CreatedBy = appUA.UserName,
                    CreatedDate = _common.GetCurrentDateTime(),
                    UpdatedBy = appUA.UserName,
                    UpdatedDate = _common.GetCurrentDateTime(),
                };
                //Deserialize items
                object ResultFromJS = JsonConvert.DeserializeObject(SupplierPaymentVM.DetailJSON);
                string ReadableFormat = JsonConvert.SerializeObject(ResultFromJS);
                SupplierPaymentVM.SupplierPaymentDetailList = JsonConvert.DeserializeObject<List<SupplierPaymentDetailViewModel>>(ReadableFormat);
                var result = _supplierPaymentBusiness.InsertUpdateSupplierPayment(Mapper.Map<SupplierPaymentViewModel, SupplierPayment>(SupplierPaymentVM));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = result, Message = "Success" });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConst.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Records = "", Message = cm.Message });
            }

        }
        #endregion InsertUpdateSupplierPayment

        #region ValidateSupplierPayment
        public string ValidateSupplierPayment(string id, string paymentRefNo)
        {
            object result = null;
            try
            {
                result = _supplierPaymentBusiness.ValidateSupplierPayment(Guid.Parse(id), paymentRefNo);
                return JsonConvert.SerializeObject(new { Result = "OK", Record = result, Message = "" });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConst.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Record = "", Message = cm.Message });
            }
        }
        #endregion ValidateSupplierPayment

        #region GetOutStandingSupplierInvoices
        [AuthSecurityFilter(ProjectObject = "SupplierPayments", Mode = "R")]
        public string GetOutStandingSupplierInvoices(string paymentId, string supplierId)
        {
            try
            {
                List<SupplierInvoiceViewModel> SupplierInvoiceVM = new List<SupplierInvoiceViewModel>();
                SupplierInvoiceVM = Mapper.Map<List<SupplierInvoice>, List<SupplierInvoiceViewModel>>(_supplierPaymentBusiness.GetOutStandingSupplierInvoices(Guid.Parse(paymentId), Guid.Parse(supplierId)));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = SupplierInvoiceVM, Message = "Success" });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Records = "", Message = ex });
            }
        }
        #endregion

        #region GetOutStandingAmount
        [AuthSecurityFilter(ProjectObject = "SupplierPayments", Mode = "R")]
        public string GetOutStandingAmount(string Id)
        {
            try
            {
                SupplierInvoiceViewModel SupplierInvoiceVM = new SupplierInvoiceViewModel();
                SupplierInvoiceVM = Mapper.Map<SupplierInvoice, SupplierInvoiceViewModel>(_supplierPaymentBusiness.GetOutstandingAmount(Guid.Parse(Id)));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = SupplierInvoiceVM, Message = "Success" });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Records = "", Message = ex });
            }
        }
        #endregion

        #region GetSupplierPayment
        [AuthSecurityFilter(ProjectObject = "SupplierPayments", Mode = "R")]
        public string GetSupplierPayment(string Id)
        {
            try
            {
                SupplierPaymentViewModel SupplierPaymentVM = new SupplierPaymentViewModel();
                SupplierPaymentVM = Mapper.Map<SupplierPayment, SupplierPaymentViewModel>(_supplierPaymentBusiness.GetSupplierPayment(Id));
                return JsonConvert.SerializeObject(new { Result = "OK", Record = SupplierPaymentVM, Message = "Success" });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Record = "", Message = ex });
            }
        }
        #endregion GetSupplierPayment

        #region DeleteSupplierPayment
        [AuthSecurityFilter(ProjectObject = "SupplierPayments", Mode = "D")]
        public string DeleteSupplierPayment(string id)
        {
            object result = null;
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    throw new Exception("ID Missing");
                }

                AppUA appUA = Session["AppUA"] as AppUA;
                string userName = appUA.UserName;

                result = _supplierPaymentBusiness.DeleteSupplierPayment(Guid.Parse(id), userName);
                return JsonConvert.SerializeObject(new { Result = "OK", Record = result, Message = _appConst.DeleteSuccess });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConst.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Record = "", Message = cm.Message });
            }
        }
        #endregion DeleteSupplierPayment

        
        #region GetSupplierCreditNote
        [AuthSecurityFilter(ProjectObject = "SupplierPayments", Mode = "R")]
        [HttpGet]
        public string GetSupplierCreditNote(string ID)
        {
            List<SupplierCreditNoteViewModel> CreditList = Mapper.Map<List<SupplierCreditNote>, List<SupplierCreditNoteViewModel>>(_supplierCreditNoteBusiness.GetCreditNoteBySupplier(Guid.Parse(ID)));

            return JsonConvert.SerializeObject(new { Result = "OK", Records = CreditList });
        }
        #endregion GetSupplierCreditNote


        #region GetCreditNoteByPaymentID
        [AuthSecurityFilter(ProjectObject = "SupplierPayments", Mode = "R")]
        [HttpGet]
        public string GetCreditNoteByPaymentID(string ID, string PaymentID)
        {
            List<SupplierCreditNoteViewModel> CreditList = Mapper.Map<List<SupplierCreditNote>, List<SupplierCreditNoteViewModel>>(_supplierCreditNoteBusiness.GetCreditNoteByPaymentID(Guid.Parse(ID), Guid.Parse(PaymentID)));

            return JsonConvert.SerializeObject(new { Result = "OK", Records = CreditList });
        }
        #endregion GetCreditNoteByPaymentID

        #region GetCreditNoteAmount
        [AuthSecurityFilter(ProjectObject = "SupplierPayments", Mode = "R")]
        [HttpGet]
        public string GetCreditNoteAmount(string CreditID, string SupplierID)
        {
            SupplierCreditNoteViewModel CreditNote = Mapper.Map<SupplierCreditNote, SupplierCreditNoteViewModel>(_supplierCreditNoteBusiness.GetCreditNoteAmount(Guid.Parse(CreditID), Guid.Parse(SupplierID)));

            return JsonConvert.SerializeObject(new { Result = "OK", Records = CreditNote });

        }
        #endregion GetCreditNoteAmount

        //#region GetOutstandingAmountBySupplier
        //[AuthSecurityFilter(ProjectObject = "SupplierPayments", Mode = "R")]
        //[HttpGet]
        //public string GetOutstandingAmountBySupplier(string CreditID, string SupplierID)
        //{
        //    SupplierPaymentViewModel Cus_pay = Mapper.Map<SupplierPayment, SupplierPaymentViewModel>(_supplierPaymentBusiness.GetOutstandingAmountBySupplier(SupplierID));
        //    return JsonConvert.SerializeObject(new { Result = "OK", Records = Cus_pay });
        //}
        //#endregion GetOutstandingAmountBySupplier

        #region ButtonStyling
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "SupplierPayments", Mode = "R")]
        public ActionResult ChangeButtonStyle(string actionType)
        {
            ToolboxViewModel toolboxVM = new ToolboxViewModel();
            switch (actionType)
            {
                case "List":
                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "Add";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Href = Url.Action("NewSupplierPayment", "SupplierPayment", new { code = "ACC" });
                    //----added for reset button---------------
                    toolboxVM.resetbtn.Visible = true;
                    toolboxVM.resetbtn.Text = "Reset";
                    toolboxVM.resetbtn.Title = "Reset All";
                    toolboxVM.resetbtn.Event = "BindOrReloadSupplierPaymentTable('Reset');";
                    //----added for export button--------------
                    toolboxVM.PrintBtn.Visible = true;
                    toolboxVM.PrintBtn.Text = "Export";
                    toolboxVM.PrintBtn.Title = "Export";
                    toolboxVM.PrintBtn.Event = "BindOrReloadSupplierPaymentTable('Export');";
                    //---------------------------------------

                    break;
                case "Edit":

                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "New";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Href = Url.Action("NewSupplierPayment", "SupplierPayment", new { code = "ACC" });

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
                    toolboxVM.ListBtn.Href = Url.Action("ViewSupplierPayment", "SupplierPayment", new { code = "ACC" });

                    toolboxVM.SendForApprovalBtn.Visible = true;
                    toolboxVM.SendForApprovalBtn.Text = "Send";
                    toolboxVM.SendForApprovalBtn.Title = "Send For Approval";
                    toolboxVM.SendForApprovalBtn.Event = "ShowSendForApproval('SPAY');";


                    //Always To be placed Last
                    toolboxVM.AboutBtn.Visible = true;
                    toolboxVM.AboutBtn.Text = "History";
                    toolboxVM.AboutBtn.Title = "About Approval History";
                    toolboxVM.AboutBtn.Event = "ShowApprovalHistory()";

                    break;

                case "Disable":

                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "New";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Href = Url.Action("ViewSupplierPayment", "SupplierPayment", new { code = "ACC" });

                    toolboxVM.ListBtn.Visible = true;
                    toolboxVM.ListBtn.Text = "List";
                    toolboxVM.ListBtn.Title = "List";
                    toolboxVM.ListBtn.Href = Url.Action("ViewSupplierPayment", "SupplierPayment", new { code = "ACC" });


                    //Always To be placed Last
                    toolboxVM.AboutBtn.Visible = true;
                    toolboxVM.AboutBtn.Text = "History";
                    toolboxVM.AboutBtn.Title = "About Approval History";
                    toolboxVM.AboutBtn.Event = "ShowApprovalHistory()";

                    break;

                case "Add":

                    toolboxVM.savebtn.Visible = true;
                    toolboxVM.savebtn.Text = "Save";
                    toolboxVM.savebtn.Title = "Save";
                    toolboxVM.savebtn.Event = "Save();";

                    toolboxVM.ListBtn.Visible = true;
                    toolboxVM.ListBtn.Text = "List";
                    toolboxVM.ListBtn.Title = "List";
                    toolboxVM.ListBtn.Href = Url.Action("ViewSupplierPayment", "SupplierPayment", new { code = "ACC" });
                    break;
                default:
                    return Content("Nochange");
            }
            return PartialView("ToolboxView", toolboxVM);
        }

        #endregion

    }
}