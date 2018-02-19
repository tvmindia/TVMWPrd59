﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionApp.DataAccessObject.DTO
{
    public class Product
    {
        public Guid ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string UnitCode { get; set; }
        public string Category { get; set; }
        public decimal Rate { get; set; }
        //additional fields 
        public bool IsUpdate { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public Common Common { get; set; }
    }
    public class ProductAdvanceSearch
    {
        public string SearchTerm { get; set; }
        public DataTablePaging DataTablePaging { get; set; }
    }
}