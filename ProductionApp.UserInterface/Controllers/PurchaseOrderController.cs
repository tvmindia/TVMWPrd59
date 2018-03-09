using AutoMapper;
using Newtonsoft.Json;
using ProductionApp.BusinessService.Contracts;
using ProductionApp.DataAccessObject.DTO;
using ProductionApp.UserInterface.Models;
using ProductionApp.UserInterface.SecurityFilter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ProductionApp.UserInterface.Controllers
{
    public class PurchaseOrderController : Controller
    {
        // GET: PurchaseOrder
        private IPurchaseOrderBusiness _purchaseOrderBusiness;
        private ITaxTypeBusiness _taxTypeBusiness;
        private ISupplierBusiness _supplierBusiness;
        private IRequisitionBusiness _requisitionBusiness;
        Common _common = new Common();
        AppConst _appConst = new AppConst();
        public PurchaseOrderController(IPurchaseOrderBusiness purchaseOrderBusiness, ISupplierBusiness supplierBusiness, IRequisitionBusiness requisitionBusiness, ITaxTypeBusiness taxTypeBusiness)
        {
            _purchaseOrderBusiness = purchaseOrderBusiness;
            _supplierBusiness = supplierBusiness;
            _requisitionBusiness = requisitionBusiness;
            _taxTypeBusiness = taxTypeBusiness;
        }

        [AuthSecurityFilter(ProjectObject = "PurchaseOrder", Mode = "R")]
        public ActionResult ViewPurchaseOrder(string code)
        {
            ViewBag.SysModuleCode = code;
            return View();
        }

        [AuthSecurityFilter(ProjectObject = "PurchaseOrder", Mode = "R")]
        public ActionResult NewPurchaseOrder(string code, Guid? id)
        {
            ViewBag.SysModuleCode = code;
            PurchaseOrderViewModel purchaseOrderVM = new PurchaseOrderViewModel();

            purchaseOrderVM.ID = id == null ? Guid.Empty : (Guid)id;
                
            return View(purchaseOrderVM);
        }

        #region InsertPurchaseOrder
        [HttpPost]
        public string InsertPurchaseOrder(PurchaseOrderViewModel purchaseOrderVM)
        {
            try
            {
                object result = null;
                AppUA appUA = Session["AppUA"] as AppUA;
                purchaseOrderVM.Common = new CommonViewModel();

                if (purchaseOrderVM.ID == Guid.Empty)
                {
                    purchaseOrderVM.Common.CreatedBy = appUA.UserName;
                    purchaseOrderVM.Common.CreatedDate = _common.GetCurrentDateTime();
                    result = _purchaseOrderBusiness.InsertPurchaseOrder(Mapper.Map<PurchaseOrderViewModel, PurchaseOrder>(purchaseOrderVM));
                }
                else
                {
                    purchaseOrderVM.Common.UpdatedBy = appUA.UserName;
                    purchaseOrderVM.Common.UpdatedDate = _common.GetCurrentDateTime();
                    result = _purchaseOrderBusiness.UpdatePurchaseOrder(Mapper.Map<PurchaseOrderViewModel, PurchaseOrder>(purchaseOrderVM));
                }
                return JsonConvert.SerializeObject(new { Result = "OK", Records = result, Message = "Success" });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConst.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Records = "", Message = cm.Message });
            }
        }
        #endregion InsertPurchaseOrder

        #region UpdatePurchaseOrderDetailLink
        public string UpdatePurchaseOrderDetailLink(PurchaseOrderViewModel purchaseOrderVM)
        {
            try
            {
                object result = null;
                AppUA appUA = Session["AppUA"] as AppUA;
                purchaseOrderVM.Common = new CommonViewModel
                {
                    UpdatedBy = appUA.UserName,
                    UpdatedDate = _common.GetCurrentDateTime(),
                };
                    result = _purchaseOrderBusiness.UpdatePurchaseOrderDetailLink(Mapper.Map<PurchaseOrderViewModel, PurchaseOrder>(purchaseOrderVM));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = result });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConst.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }
        #endregion UpdatePurchaseOrderDetailLink

        #region EditPurchaseOrderDetail
        [AuthSecurityFilter(ProjectObject = "PurchaseOrder", Mode = "R")]
        public string EditPurchaseOrderDetail(string ID)
        {
            try
            {
                List<PurchaseOrderDetailViewModel> purchaseOrderList = Mapper.Map<List<PurchaseOrderDetail>, List<PurchaseOrderDetailViewModel>>(_purchaseOrderBusiness.GetPurchaseOrderDetailByIDForEdit(Guid.Parse(ID)));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = purchaseOrderList, Message = "Success" });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Records="", Message = ex });
            }
        }
        #endregion EditPurchaseOrderDetail
        
        #region GetPurchaseOrderByID

        [AuthSecurityFilter(ProjectObject = "PurchaseOrder", Mode = "R")]
        public string GetPurchaseOrderByID(string ID)
        {
            try { 
            PurchaseOrderViewModel purchaseOrderVM = Mapper.Map<PurchaseOrder, PurchaseOrderViewModel>(_purchaseOrderBusiness.GetPurchaseOrderByID(Guid.Parse( ID)));
            return JsonConvert.SerializeObject(new { Result = "OK", Records = purchaseOrderVM, Message = "Success" });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Records = "", Message = ex });
            }
        }
        #endregion GetPurchaseOrderByID

        #region GetPurchaseOrderDetailTableByID
        [AuthSecurityFilter(ProjectObject = "PurchaseOrder", Mode = "R")]
        public string GetPurchaseOrderDetailByID(string ID)
        {
            try { 
            List<PurchaseOrderDetailViewModel> purchaseOrderList = Mapper.Map<List<PurchaseOrderDetail>, List<PurchaseOrderDetailViewModel>>(_purchaseOrderBusiness.GetPurchaseOrderDetailByID(Guid.Parse(ID)));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = purchaseOrderList, Message = "Success" });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Records="", Message = ex });
            }
        }
        #endregion GetPurchaseOrderDetailTableByID

        #region GetAllPurchaseOrder
        [HttpPost]
        [AuthSecurityFilter(ProjectObject = "PurchaseOrder", Mode = "R")]
        public JsonResult GetAllPurchaseOrder(DataTableAjaxPostModel model, PurchaseOrderAdvanceSearchViewModel purchaseOrderAdvanceSearchVM)
        {
            purchaseOrderAdvanceSearchVM.DataTablePaging.Start = model.start;
            purchaseOrderAdvanceSearchVM.DataTablePaging.Length = (purchaseOrderAdvanceSearchVM.DataTablePaging.Length == 0 ? model.length : purchaseOrderAdvanceSearchVM.DataTablePaging.Length);
            List<PurchaseOrderViewModel> purchaseOrderList = Mapper.Map<List<PurchaseOrder>, List<PurchaseOrderViewModel>>(_purchaseOrderBusiness.GetAllPurchaseOrder(Mapper.Map<PurchaseOrderAdvanceSearchViewModel, PurchaseOrderAdvanceSearch>(purchaseOrderAdvanceSearchVM)));
            var settings = new JsonSerializerSettings
            {
                Formatting = Formatting.None
            };
            return Json(new
            {
                draw = model.draw,
            recordsTotal = purchaseOrderList.Count != 0 ? purchaseOrderList[0].TotalCount : 0,
            recordsFiltered = purchaseOrderList.Count != 0 ? purchaseOrderList[0].FilteredCount : 0,
            data= purchaseOrderList
            
        });
        }
        #endregion GetAllPurchaseOrder

        #region GetAllRequisitionForPurchaseOrder
        public string GetAllRequisitionForPurchaseOrder()
        {
            try { 
            List<RequisitionViewModel> requisitionVMList = Mapper.Map<List<Requisition>, List<RequisitionViewModel>>(_requisitionBusiness.GetAllRequisitionForPurchaseOrder());
                return JsonConvert.SerializeObject(new { Result = "OK", Records = requisitionVMList, Message = "Success" });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Records="", Message = ex });
            }
        }
        #endregion GetAllRequisitionForPurchaseOrder

        #region GetRequisitionDetailsByIDs
        [AuthSecurityFilter(ProjectObject = "PurchaseOrder", Mode = "R")]
        public string GetRequisitionDetailsByIDs(string IDs, string POID)
        {
            try
            {
                List<RequisitionDetailViewModel> requisitionDetailVMList = Mapper.Map<List<RequisitionDetail>, List<RequisitionDetailViewModel>>(_requisitionBusiness.GetRequisitionDetailsByIDs(IDs, POID));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = requisitionDetailVMList, Message = "Success" });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Records = "", Message = ex });
            }
        }
        #endregion GetRequisitionDetailsByIDs
       
        #region DeletePurchaseOrder
        public string DeletePurchaseOrder(string ID)
        {
            object result = null;
            try
            {
                if (string.IsNullOrEmpty(ID))
                {
                    throw new Exception("ID Missing");
                }
                result = _purchaseOrderBusiness.DeletePurchaseOrder(Guid.Parse(ID));
                return JsonConvert.SerializeObject(new { Result = "OK", Record = result, Message = _appConst.DeleteSuccess });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConst.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Record="", Message = cm.Message });
            }
        }
        #endregion DeletePurchaseOrder

        #region DeletePurchaseOrderDetail
        public string DeletePurchaseOrderDetail(string ID)
        {
            object result = null;
            try
            {
                if (string.IsNullOrEmpty(ID))
                {
                    throw new Exception("ID Missing");
                }
                result = _purchaseOrderBusiness.DeletePurchaseOrderDetail(Guid.Parse(ID));
                return JsonConvert.SerializeObject(new { Result = "OK", Record = result, Message = _appConst.DeleteSuccess });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConst.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }
        #endregion DeletePurchaseOrderDetail


        #region TaxTypeByDesc
        //Need to be removed//refer TaxType  controller
        [AuthSecurityFilter(ProjectObject = "PurchaseOrder", Mode = "R")]
        public string GetTaxtype(string Code)
        {
            try
            {
                TaxTypeViewModel taxTypeVM = new TaxTypeViewModel();
                taxTypeVM = Mapper.Map<TaxType,TaxTypeViewModel>(_taxTypeBusiness.GetTaxTypeDetailsByCode(Code));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = taxTypeVM, Message = "Success" });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Records = "", Message = ex });
            }
        }
        #endregion TaxTypeByDesc

        #region GetMailPreview
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "PurchaseOrder", Mode = "")]
        public ActionResult GetMailPreview(string ID,int flag)
        {
            PurchaseOrderMailPreviewViewModel purchaseOrderMailPreviewVM = null;
            try
            {
                if (string.IsNullOrEmpty(ID))
                {
                    throw new Exception("ID is missing");
                }
                purchaseOrderMailPreviewVM = new PurchaseOrderMailPreviewViewModel();
                purchaseOrderMailPreviewVM.PurchaseOrder = Mapper.Map<PurchaseOrder, PurchaseOrderViewModel>(_purchaseOrderBusiness.GetMailPreview(Guid.Parse(ID)));
                if (flag == 0)
                {
                    purchaseOrderMailPreviewVM.Flag = false;
                    if (purchaseOrderMailPreviewVM.PurchaseOrder.MailBodyHeader != null)
                        purchaseOrderMailPreviewVM.PurchaseOrder.MailBodyHeader = purchaseOrderMailPreviewVM.PurchaseOrder.MailBodyHeader.Replace("\n", "<br/>");
                    if (purchaseOrderMailPreviewVM.PurchaseOrder.MailBodyFooter != null)
                        purchaseOrderMailPreviewVM.PurchaseOrder.MailBodyFooter = purchaseOrderMailPreviewVM.PurchaseOrder.MailBodyFooter.Replace("\n", "<br/>");
                }
                if (flag==1)
                {
                    purchaseOrderMailPreviewVM.Flag = true;
                    if (purchaseOrderMailPreviewVM.PurchaseOrder.MailBodyHeader != null)
                         purchaseOrderMailPreviewVM.PurchaseOrder.MailBodyHeader = purchaseOrderMailPreviewVM.PurchaseOrder.MailBodyHeader.Replace("<br/>",Environment.NewLine);
                    if (purchaseOrderMailPreviewVM.PurchaseOrder.MailBodyFooter != null)
                        purchaseOrderMailPreviewVM.PurchaseOrder.MailBodyFooter = purchaseOrderMailPreviewVM.PurchaseOrder.MailBodyFooter.Replace("<br/>",Environment.NewLine);
                }
                ViewBag.path = "http://" + HttpContext.Request.Url.Authority + purchaseOrderMailPreviewVM.PurchaseOrder.LogoURL;
            }
            catch (Exception ex)
            {
                throw ex;
            }
           

            return PartialView("_PurchaseOrderMailPreview", purchaseOrderMailPreviewVM);
        }
        #endregion GetMailPreview

        #region EmailSent
        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        [AuthSecurityFilter(ProjectObject = "PurchaseOrder", Mode = "")]
        public async Task <string> SendQuoteMail(PurchaseOrderViewModel purchaseOrderVM)
        {
            try
            {
                object result = null;
                if (!string.IsNullOrEmpty(purchaseOrderVM.ID.ToString()))
                {
                    AppUA appUA = Session["AppUA"] as AppUA;
                    purchaseOrderVM.Common = new CommonViewModel
                    {
                        UpdatedBy = appUA.UserName,
                        UpdatedDate = _common.GetCurrentDateTime(),
                    };

                    bool sendsuccess = await _purchaseOrderBusiness.QuoteEmailPush(Mapper.Map<PurchaseOrderViewModel, PurchaseOrder>(purchaseOrderVM));
                    if (sendsuccess)
                    {
                        //1 is meant for mail sent successfully
                        purchaseOrderVM.EmailSentYN = sendsuccess.ToString();
                        result = _purchaseOrderBusiness.UpdatePurchaseOrderMailStatus(Mapper.Map<PurchaseOrderViewModel, PurchaseOrder>(purchaseOrderVM));
                    }
                    return JsonConvert.SerializeObject(new { Result = "OK", Record = result, MailResult = sendsuccess, Message = _appConst.MailSuccess });
                }
                else
                {

                    return JsonConvert.SerializeObject(new { Result = "ERROR", Message = "ID is Missing" });
                }
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConst.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }
        #endregion EmailSent

        #region UpdatePOMailDetails
        [HttpPost]
        public string UpdatePOMailDetails(PurchaseOrderViewModel purchaseOrderVM)
        {
            try
            {
                object result = null;
                AppUA appUA = Session["AppUA"] as AppUA;
                purchaseOrderVM.Common = new CommonViewModel
                {
                    UpdatedBy = appUA.UserName,
                    UpdatedDate = _common.GetCurrentDateTime(),
                };
                result = _purchaseOrderBusiness.UpdatePOMailDetails(Mapper.Map<PurchaseOrderViewModel, PurchaseOrder>(purchaseOrderVM));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = result, Message = "Success" });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConst.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Records="", Message = cm.Message });
            }
        }
        #endregion UpdatePOMailDetails

        #region PurchaseOrder Dropdown
        public ActionResult PurchaseOrderDropdown()
        {
            PurchaseOrderViewModel purchaseOrderVM = new PurchaseOrderViewModel();
            List<SelectListItem> selectListItem = new List<SelectListItem>();
            purchaseOrderVM.SelectList=new List<SelectListItem>();
            List<PurchaseOrderViewModel> purchaseOrderList = Mapper.Map<List<PurchaseOrder>, List<PurchaseOrderViewModel>>(_purchaseOrderBusiness.GetAllPurchaseOrderForSelectList());
            if (purchaseOrderList != null)
                foreach (PurchaseOrderViewModel purchaseOrder in purchaseOrderList)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Text = purchaseOrder.PurchaseOrderNo,
                        Value = purchaseOrder.ID.ToString(),
                        Selected = false
                    });
                }
            purchaseOrderVM.SelectList = selectListItem;
            return PartialView("_PurchaseOrderDropdown", purchaseOrderVM);
        }
        #endregion

        #region GetSupplierDetails
        [AuthSecurityFilter(ProjectObject = "PurchaseOrder", Mode = "R")]
        public string GetSupplierDetails(string supplierid)
        {
            try
            {
                SupplierViewModel supplierVM = Mapper.Map<Supplier, SupplierViewModel>(_supplierBusiness.GetSupplier(Guid.Parse(supplierid)));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = supplierVM, Message = "Success" });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Records = "", Message = ex });
            }
        }
        #endregion GetSupplierDetails

        #region ButtonStyling
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "PurchaseOrder", Mode = "")]
        public ActionResult ChangeButtonStyle(string actionType)
        {
            ToolboxViewModel toolboxVM = new ToolboxViewModel();
            switch (actionType)
            {
                case "List":
                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "Add";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Href = Url.Action("NewPurchaseOrder", "PurchaseOrder", new { code = "PURCH" });
                    toolboxVM.addbtn.Event = "";
                    //----added for reset button---------------
                    toolboxVM.resetbtn.Visible = true;
                    toolboxVM.resetbtn.Text = "Reset";
                    toolboxVM.resetbtn.Title = "Reset All";
                    toolboxVM.resetbtn.Event = "ResetPurchaseOrderList();";
                    //----added for export button--------------
                    toolboxVM.PrintBtn.Visible = true;
                    toolboxVM.PrintBtn.Text = "Export";
                    toolboxVM.PrintBtn.Title = "Export";
                    toolboxVM.PrintBtn.Event = "ImportPurchaseOrderData();";
                    //---------------------------------------

                    break;
                case "Edit":

                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "New";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Href = Url.Action("NewPurchaseOrder", "PurchaseOrder", new { code = "PURCH" });
                    toolboxVM.addbtn.Event = "";

                    toolboxVM.savebtn.Visible = true;
                    toolboxVM.savebtn.Text = "Save";
                    toolboxVM.savebtn.Title = "Save PurchaseOrder";
                    toolboxVM.savebtn.Event = "Save();";

                    toolboxVM.deletebtn.Visible = true;
                    toolboxVM.deletebtn.Text = "Delete";
                    toolboxVM.deletebtn.Title = "Delete PurchaseOrder";
                    toolboxVM.deletebtn.Event = "DeleteClick()";

                    toolboxVM.resetbtn.Visible = true;
                    toolboxVM.resetbtn.Text = "Reset";
                    toolboxVM.resetbtn.Title = "Reset";
                    toolboxVM.resetbtn.Event = "Reset();";

                    toolboxVM.EmailBtn.Visible = true;
                    toolboxVM.EmailBtn.Text = "Email";
                    toolboxVM.EmailBtn.Title = "Email";
                    toolboxVM.EmailBtn.Event = "EmailPreview(1);";

                    toolboxVM.SendForApprovalBtn.Visible = true;
                    toolboxVM.SendForApprovalBtn.Text = "Send";
                    toolboxVM.SendForApprovalBtn.Title = "Send For Approval";
                    toolboxVM.SendForApprovalBtn.Event = "ShowSendForApproval('PO');";

                    toolboxVM.ListBtn.Visible = true;
                    toolboxVM.ListBtn.Text = "List";
                    toolboxVM.ListBtn.Title = "List";
                    toolboxVM.ListBtn.Event = "";
                    toolboxVM.ListBtn.Href = Url.Action("ViewPurchaseOrder", "PurchaseOrder", new { Code = "PURCH" });

                    break;

                case "Disable":

                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "New";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Href = Url.Action("NewPurchaseOrder", "PurchaseOrder", new { code = "PURCH" });
                    toolboxVM.addbtn.Event = "";

                    toolboxVM.EmailBtn.Visible = true;
                    toolboxVM.EmailBtn.Text = "Email";
                    toolboxVM.EmailBtn.Title = "Email";
                    toolboxVM.EmailBtn.Event = "EmailPreview(1);";

                    toolboxVM.ListBtn.Visible = true;
                    toolboxVM.ListBtn.Text = "List";
                    toolboxVM.ListBtn.Title = "List";
                    toolboxVM.ListBtn.Event = "";
                    toolboxVM.ListBtn.Href = Url.Action("ViewPurchaseOrder", "PurchaseOrder", new { Code = "PURCH" });

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
                    toolboxVM.ListBtn.Href = Url.Action("ViewPurchaseOrder", "PurchaseOrder", new { Code = "PURCH" });


                    break;
                default:
                    return Content("Nochange");
            }
            return PartialView("ToolboxView", toolboxVM);
        }

        #endregion
    }
}