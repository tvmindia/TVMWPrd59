using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UserInterface.Models
{
    public class PDFGeneratorViewModel
    {
        public ElementStructure table;
        public ElementStructure trodd;
        public ElementStructure treven;
        public ElementStructure th;
        public ElementStructure td;
    }
    public struct ElementStructure
    {
        public string style { get; set; }
        public string text { get; set; }
    }
    public class PDFTools
    {
        [AllowHtml]
        public string Content { get; set; }
        [AllowHtml]
        public string Headcontent { get; set; }
        public string HeaderText { get; set; }
        [AllowHtml]
        public string ContentFileName { get; set; }
        [AllowHtml]
        public string CustomerName { get; set; }
    }
}