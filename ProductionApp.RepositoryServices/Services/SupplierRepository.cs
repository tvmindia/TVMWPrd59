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
        AppConst _appConst = new AppConst();
        private IDatabaseFactory _databaseFactory;
        public SupplierRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }

        #region GetSupplierForSelectList
        /// <summary>
        /// To Get Supplier For SelectList
        /// </summary>
        /// <returns>List</returns>
        public List<Supplier> GetSupplierForSelectList()
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
                        cmd.CommandText = "[AMC].[GetSupplierForSelectList]";
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
        #endregion GetSupplierForSelectList

        #region GetAllSupplier
        /// <summary>
        /// To Get All Supplier
        /// </summary>
        /// <param name="supplierAdvanceSearch"></param>
        /// <returns>List</returns>
        public List<Supplier> GetAllSupplier(SupplierAdvanceSearch supplierAdvanceSearch)
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
                        cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, -1).Value = string.IsNullOrEmpty(supplierAdvanceSearch.SearchTerm) ? "" : supplierAdvanceSearch.SearchTerm.Trim();
                        cmd.Parameters.Add("@RowStart", SqlDbType.Int).Value = supplierAdvanceSearch.DataTablePaging.Start;
                        if (supplierAdvanceSearch.DataTablePaging.Length == -1)
                            cmd.Parameters.AddWithValue("@Length", DBNull.Value);
                        else
                            cmd.Parameters.Add("@Length", SqlDbType.Int).Value = supplierAdvanceSearch.DataTablePaging.Length;
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
                                        supplier.FilteredCount = (sdr["FilteredCount"].ToString() != "" ? int.Parse(sdr["FilteredCount"].ToString()) : supplier.FilteredCount);
                                        supplier.TotalCount = (sdr["TotalCount"].ToString() != "" ? int.Parse(sdr["TotalCount"].ToString()) : supplier.TotalCount);
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
        #endregion GetAllSupplier

        #region InsertUpdateSupplier
        /// <summary>
        /// To Insert and update Supplier
        /// </summary>
        /// <param name="supplier"></param>
        /// <returns>object</returns>
        public object InsertUpdateSupplier(Supplier supplier)
        {
            SqlParameter outputStatus, OutputID;
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
                        cmd.CommandText = "[AMC].[InsertUpdateSupplier]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@IsUpdate", SqlDbType.Bit).Value = supplier.IsUpdate;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = supplier.ID;
                        cmd.Parameters.Add("@CompanyName", SqlDbType.VarChar, 150).Value = supplier.CompanyName;
                        cmd.Parameters.Add("@ContactPerson", SqlDbType.VarChar, 100).Value = supplier.ContactPerson;
                        cmd.Parameters.Add("@ContactEmail", SqlDbType.VarChar, 150).Value = supplier.ContactEmail;
                        cmd.Parameters.Add("@ContactTitle", SqlDbType.VarChar, 10).Value = supplier.ContactTitle;
                        cmd.Parameters.Add("@Product", SqlDbType.VarChar, 500).Value = supplier.Product;
                        cmd.Parameters.Add("@Website", SqlDbType.NVarChar, 500).Value = supplier.Website;
                        cmd.Parameters.Add("@LandLine", SqlDbType.VarChar, 50).Value = supplier.LandLine;
                        cmd.Parameters.Add("@Mobile", SqlDbType.VarChar, 50).Value = supplier.Mobile;
                        cmd.Parameters.Add("@Fax", SqlDbType.VarChar, 50).Value = supplier.Fax;
                        cmd.Parameters.Add("@OtherPhoneNos", SqlDbType.VarChar, 250).Value = supplier.OtherPhoneNos;
                        cmd.Parameters.Add("@BillingAddress", SqlDbType.NVarChar, -1).Value = supplier.BillingAddress;
                        cmd.Parameters.Add("@ShippingAddress", SqlDbType.NVarChar, -1).Value = supplier.ShippingAddress;
                        cmd.Parameters.Add("@PaymentTermCode", SqlDbType.VarChar, 10).Value = supplier.PaymentTermCode;
                        cmd.Parameters.Add("@TaxRegNo", SqlDbType.VarChar, 50).Value = supplier.TaxRegNo;
                        cmd.Parameters.Add("@PANNo", SqlDbType.VarChar, 50).Value = supplier.PANNo;
                        cmd.Parameters.Add("@GeneralNotes", SqlDbType.NVarChar, -1).Value = supplier.GeneralNotes;
                        cmd.Parameters.Add("@UpdatedBy", SqlDbType.VarChar, 50).Value = supplier.Common.UpdatedBy;
                        cmd.Parameters.Add("@UpdatedDate", SqlDbType.DateTime).Value = supplier.Common.UpdatedDate;
                        outputStatus = cmd.Parameters.Add("@Status", SqlDbType.Int);
                        outputStatus.Direction = ParameterDirection.Output;
                        OutputID = cmd.Parameters.Add("@IDOut", SqlDbType.UniqueIdentifier);
                        OutputID.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();
                    }
                }
                switch (outputStatus.Value.ToString())
                {
                    case "0":
                        throw new Exception(supplier.IsUpdate ? _appConst.UpdateFailure : _appConst.InsertFailure);
                    case "1":
                        supplier.ID = Guid.Parse(OutputID.Value.ToString());
                        return new
                        {
                            ID = supplier.ID,
                            Status = outputStatus.Value.ToString(),
                            Message = supplier.IsUpdate ? _appConst.UpdateSuccess : _appConst.InsertSuccess
                        };
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return new
            {
                ID = supplier.ID,
                Status = outputStatus.Value.ToString(),
                Message = supplier.IsUpdate ? _appConst.UpdateSuccess : _appConst.InsertSuccess
            };
        }
        #endregion InsertUpdateSupplier

        public Supplier GetSupplier(Guid id)
        {
            Supplier supplier = new Supplier();
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
                        cmd.CommandText = "[AMC].[GetSupplier]";
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = id;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                while (sdr.Read())
                                {
                                    supplier.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : supplier.ID);
                                    supplier.CompanyName = (sdr["CompanyName"].ToString() != "" ? sdr["CompanyName"].ToString() : supplier.CompanyName);
                                    supplier.ContactPerson = (sdr["ContactPerson"].ToString() != "" ? sdr["ContactPerson"].ToString() : supplier.ContactPerson);
                                    supplier.ContactEmail = (sdr["ContactEmail"].ToString() != "" ? sdr["ContactEmail"].ToString() : supplier.ContactEmail);
                                    supplier.ContactTitle = (sdr["ContactTitle"].ToString() != "" ? sdr["ContactTitle"].ToString() : supplier.ContactTitle);
                                    supplier.Product = (sdr["Product"].ToString() != "" ? sdr["Product"].ToString() : supplier.Product);
                                    supplier.Website = (sdr["Website"].ToString() != "" ? sdr["Website"].ToString() : supplier.Website);
                                    supplier.LandLine = (sdr["LandLine"].ToString() != "" ? sdr["LandLine"].ToString() : supplier.LandLine);
                                    supplier.Mobile = (sdr["Mobile"].ToString() != "" ? sdr["Mobile"].ToString() : supplier.Mobile);
                                    supplier.Fax = (sdr["Fax"].ToString() != "" ? sdr["Fax"].ToString() : supplier.Fax);
                                    supplier.OtherPhoneNos = (sdr["OtherPhoneNos"].ToString() != "" ? sdr["OtherPhoneNos"].ToString() : supplier.OtherPhoneNos);
                                    supplier.BillingAddress = (sdr["BillingAddress"].ToString() != "" ? sdr["BillingAddress"].ToString() : supplier.BillingAddress);
                                    supplier.ShippingAddress = (sdr["ShippingAddress"].ToString() != "" ? sdr["ShippingAddress"].ToString() : supplier.ShippingAddress);
                                    supplier.PaymentTermCode = (sdr["PaymentTermCode"].ToString() != "" ? sdr["PaymentTermCode"].ToString() : supplier.PaymentTermCode);
                                    supplier.TaxRegNo = (sdr["TaxRegNo"].ToString() != "" ? sdr["TaxRegNo"].ToString() : supplier.TaxRegNo);
                                    supplier.PANNo = (sdr["PANNo"].ToString() != "" ? sdr["PANNo"].ToString() : supplier.PANNo);
                                    supplier.GeneralNotes = (sdr["GeneralNotes"].ToString() != "" ? sdr["GeneralNotes"].ToString() : supplier.GeneralNotes);
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
            return supplier;
        }

        #region DeleteSupplier
        /// <summary>
        /// To Delete Supplier
        /// </summary>
        /// <param name="id"></param>
        /// <returns>object</returns>
        public object DeleteSupplier(Guid id)
        {
            SqlParameter outputStatus = null;
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
                        cmd.CommandText = "[AMC].[DeleteSupplier]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = id;
                        outputStatus = cmd.Parameters.Add("@Status", SqlDbType.SmallInt);
                        outputStatus.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();
                    }
                }
                switch (outputStatus.Value.ToString())
                {
                    case "0":
                        throw new Exception(_appConst.DeleteFailure);

                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return new
            {
                Status = outputStatus.Value.ToString(),
                Message = _appConst.DeleteSuccess
            };
        }
        #endregion DeleteSupplier

    }
}
