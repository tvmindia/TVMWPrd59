﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProductionApp.UserInterface.Models
{
    public class DocumentTypeViewModel
    {
        public string Type { get; set; }
        public string Description { get; set; }
        public List<SelectListItem> SelectList { get; set; }
    }
}