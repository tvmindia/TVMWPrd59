using AutoMapper;
using ProductionApp.BusinessService.Contracts;
using ProductionApp.DataAccessObject.DTO;
using ProductionApp.UserInterface.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProductionApp.UserInterface.Controllers
{
    public class SupplierController : Controller
    {
        // GET: Supplier
        private ISupplierBusiness _supplierBusiness;
        public SupplierController(ISupplierBusiness supplierBusiness)
        {
            _supplierBusiness = supplierBusiness;
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult SupplierDropdown()
        {
            SupplierViewModel supplierVM = new SupplierViewModel();
            List<SelectListItem> selectListItem = new List<SelectListItem>();
            supplierVM.SelectList= new List<SelectListItem>();
            List<SupplierViewModel> supplierList = Mapper.Map<List<Supplier>, List<SupplierViewModel>>(_supplierBusiness.GetAllSupplier());
            if(supplierList!=null)
            foreach (SupplierViewModel supplier in supplierList)
            {
                selectListItem.Add(new SelectListItem
                {
                    Text = supplier.CompanyName,
                    Value = supplier.ID.ToString(),
                    Selected = false
                });
            }
            supplierVM.SelectList = selectListItem;
            return PartialView("_SupplierDropdown",supplierVM);
                 
        }
           
    }
}