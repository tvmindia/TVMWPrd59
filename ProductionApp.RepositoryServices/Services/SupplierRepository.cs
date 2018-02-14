using ProductionApp.DataAccessObject.DTO;
using ProductionApp.RepositoryServices.Contracts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionApp.RepositoryServices.Services
{
   public class SupplierRepository : ISupplierRepository
    {
        private IDatabaseFactory _databaseFactory;
        public SupplierRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }
        public List<Supplier> GetAllSupplier()
        {
            List<Supplier> supplierList = null;
            try
            {
                using (SqlConnection con = _databaseFactory.GetDBConnection())
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        if (con.State == ConnectionState.Closed)
                        {
                            con.Open();
                        }
                        cmd.Connection = con;
                        cmd.CommandText = "[AMC].[GetAllSupplier]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                supplierList = new List<Supplier>();
                                while (sdr.Read())
                                {
                                    Supplier supplier = new Supplier();
                                    {
                                        supplier.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : supplier.ID);
                                        supplier.CompanyName = (sdr["CompanyName"].ToString() != "" ? sdr["CompanyName"].ToString() : supplier.CompanyName);
                                        supplier.ContactPerson = (sdr["ContactPerson"].ToString() != "" ? sdr["ContactPerson"].ToString() : supplier.ContactPerson);
                                        supplier.ContactEmail = (sdr["ContactEmail"].ToString() != "" ? sdr["ContactEmail"].ToString() : supplier.ContactEmail);
                                        supplier.ContactTitle = (sdr["ContactTitle"].ToString() != "" ? sdr["ContactTitle"].ToString() : supplier.ContactTitle);
                                        supplier.Product = (sdr["Product"].ToString() != "" ? sdr["Product"].ToString() : supplier.Product);
                                        supplier.Website = (sdr["Website"].ToString() != "" ? sdr["Website"].ToString() : supplier.Website);
                                        supplier.Mobile = (sdr["Mobile"].ToString() != "" ? sdr["Mobile"].ToString() : supplier.Mobile);
                                        supplier.LandLine = (sdr["LandLine"].ToString() != "" ? sdr["LandLine"].ToString() : supplier.LandLine);
                                        supplier.OtherPhoneNos = (sdr["OtherPhoneNos"].ToString() != "" ? sdr["OtherPhoneNos"].ToString() : supplier.OtherPhoneNos);
                                        supplier.Fax = (sdr["Fax"].ToString() != "" ? sdr["Fax"].ToString() : supplier.Fax);
                                        supplier.BillingAddress = (sdr["BillingAddress"].ToString() != "" ? sdr["BillingAddress"].ToString() : supplier.BillingAddress);
                                        supplier.ShippingAddress = (sdr["ShippingAddress"].ToString() != "" ? sdr["ShippingAddress"].ToString() : supplier.ShippingAddress);
                                        supplier.PaymentTermCode = (sdr["PaymentTermCode"].ToString() != "" ? sdr["PaymentTermCode"].ToString() : supplier.PaymentTermCode);
                                        supplier.TaxRegNo = (sdr["TaxRegNo"].ToString() != "" ? sdr["TaxRegNo"].ToString() : supplier.TaxRegNo);
                                        supplier.PANNo = (sdr["PANNo"].ToString() != "" ? sdr["PANNo"].ToString() : supplier.PANNo);
                                        supplier.GeneralNotes = (sdr["GeneralNotes"].ToString() != "" ? sdr["GeneralNotes"].ToString() : supplier.GeneralNotes);
                                    }
                                    supplierList.Add(supplier);
                                }
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return supplierList;
        }
    }
}
