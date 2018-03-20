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
    public class TaxTypeController : Controller
    {
        // GET: TaxType
        private ITaxTypeBusiness _taxTypeBusiness;
        public TaxTypeController(ITaxTypeBusiness taxTypeBusiness)
        {
            _taxTypeBusiness = taxTypeBusiness;
        }

        public ActionResult TaxType()
        {
            return View();
        }

        public ActionResult TaxTypeDropdown()
        {
            TaxTypeViewModel taxTypeVM = new TaxTypeViewModel();
            taxTypeVM.SelectList = new List<SelectListItem>();
            List<TaxTypeViewModel> taxTypeList = Mapper.Map<List<TaxType>, List<TaxTypeViewModel>>(_taxTypeBusiness.GetTaxTypeForSelectList());
            if (taxTypeList != null)
                foreach (TaxTypeViewModel taxType in taxTypeList)
                {
                    taxTypeVM.SelectList.Add(new SelectListItem
                    {
                        Text = taxType.Description,
                        Value = taxType.Code,
                        Selected = false
                    });
                }
            return PartialView("_TaxTypeDropdown", taxTypeVM);

        }

        #region TaxTypeByDesc
        //[AuthSecurityFilter(ProjectObject = "TaxType", Mode = "R")]
        public string GetTaxtype(string Code)
        {
            try
            {
                TaxTypeViewModel taxTypeVM = new TaxTypeViewModel();
                taxTypeVM = Mapper.Map<TaxType, TaxTypeViewModel>(_taxTypeBusiness.GetTaxTypeDetailsByCode(Code));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = taxTypeVM, Message = "Success" });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Records = "", Message = ex });
            }
        }
        #endregion TaxTypeByDesc
    }
}