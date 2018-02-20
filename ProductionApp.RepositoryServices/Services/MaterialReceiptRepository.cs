using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using ProductionApp.DataAccessObject.DTO;
using ProductionApp.RepositoryServices.Contracts;

namespace ProductionApp.RepositoryServices.Services
{
    public class MaterialReceiptRepository : IMaterialReceiptRepository
    {
        #region Constructor Injection
        private IDatabaseFactory _databaseFactory;
        Settings settings = new Settings();
        public MaterialReceiptRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }
        #endregion Constructor Injection

        #region GetAllMaterialReceipt
        public List<MaterialReceipt> GetAllMaterialReceipt(MaterialReceiptAdvanceSearch materialReceiptAdvanceSearch)
        {
            try
            {
                List<MaterialReceipt> materialReceiptList = null;
                MaterialReceipt materialReceipt = null;
                using (SqlConnection con = _databaseFactory.GetDBConnection())
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        if (con.State == ConnectionState.Closed)
                        {
                            con.Open();
                        }
                        cmd.Connection = con;
                        cmd.CommandText = "[AMC].[GetAllMaterialReceipt]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = materialReceiptAdvanceSearch.FromDate;
                        cmd.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = materialReceiptAdvanceSearch.ToDate;
                        if (materialReceiptAdvanceSearch.Supplier.ID == Guid.Empty)
                        {
                            cmd.Parameters.AddWithValue("@SupplierID", DBNull.Value);
                        }
                        else
                        {
                            cmd.Parameters.Add("@SupplierID", SqlDbType.UniqueIdentifier).Value = materialReceiptAdvanceSearch.Supplier.ID;
                        }
                        if (materialReceiptAdvanceSearch.PurchaseOrder.ID == Guid.Empty)
                        {
                            cmd.Parameters.AddWithValue("@PurchaseOrderID", DBNull.Value);
                        }
                        else
                        {
                            cmd.Parameters.Add("@PurchaseOrderID", SqlDbType.UniqueIdentifier).Value = materialReceiptAdvanceSearch.PurchaseOrder.ID;
                        }
                        cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, -1).Value = string.IsNullOrEmpty(materialReceiptAdvanceSearch.SearchTerm) ? "" : materialReceiptAdvanceSearch.SearchTerm;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                materialReceiptList = new List<MaterialReceipt>();
                                while (sdr.Read())
                                {
                                    materialReceipt = new MaterialReceipt();
                                    materialReceipt.Supplier = new Supplier();
                                    materialReceipt.Common = new Common();
                                    materialReceipt.PurchaseOrderID = (sdr["PurchaseOrderID"].ToString() != "" ? Guid.Parse(sdr["PurchaseOrderID"].ToString()) : materialReceipt.PurchaseOrderID);
                                    materialReceipt.PurchaseOrderNo = (sdr["PurchaseOrderNo"].ToString() != "" ? sdr["PurchaseOrderNo"].ToString() : materialReceipt.PurchaseOrderNo);
                                    materialReceipt.Supplier.ID = materialReceipt.SupplierID = (sdr["SupplierID"].ToString() != "" ? Guid.Parse(sdr["SupplierID"].ToString()) : materialReceipt.SupplierID);
                                    materialReceipt.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : materialReceipt.SupplierID);
                                    materialReceipt.ReceiptNo = (sdr["ReceiptNo"].ToString() != "" ? sdr["ReceiptNo"].ToString() : materialReceipt.ReceiptNo);
                                    materialReceipt.ReceiptDate = (sdr["ReceiptDate"].ToString() != "" ? DateTime.Parse(sdr["ReceiptDate"].ToString()) : materialReceipt.ReceiptDate);
                                    materialReceipt.ReceiptDateFormatted = (sdr["ReceiptDate"].ToString() != "" ? DateTime.Parse(sdr["ReceiptDate"].ToString()).ToString(settings.DateFormat) : materialReceipt.ReceiptDateFormatted);
                                    materialReceipt.Supplier.CompanyName = (sdr["SupplierName"].ToString() != "" ? sdr["SupplierName"].ToString() : materialReceipt.Supplier.CompanyName);
                                    materialReceipt.GeneralNotes = (sdr["GeneralNotes"].ToString() != "" ? sdr["GeneralNotes"].ToString() : materialReceipt.GeneralNotes);
                                    materialReceipt.Common.CreatedBy = (sdr["CreatedBy"].ToString() != "" ? sdr["CreatedBy"].ToString() : materialReceipt.Common.CreatedBy);
                                    materialReceipt.Common.UpdatedBy = (sdr["UpdatedBy"].ToString() != "" ? sdr["UpdatedBy"].ToString() : materialReceipt.Common.UpdatedBy);
                                    materialReceipt.Common.CreatedDateString = (sdr["CreatedDate"].ToString() != "" ? DateTime.Parse(sdr["CreatedDate"].ToString()).ToString(settings.DateFormat) : materialReceipt.Common.CreatedDateString);
                                    materialReceipt.Common.UpdatedDateString = (sdr["UpdatedDate"].ToString() != "" ? DateTime.Parse(sdr["UpdatedDate"].ToString()).ToString(settings.DateFormat) : materialReceipt.Common.UpdatedDateString);
                                    materialReceipt.FilteredCount = (sdr["FilteredCount"].ToString() != "" ? int.Parse(sdr["FilteredCount"].ToString()) : materialReceipt.FilteredCount);
                                    materialReceipt.TotalCount= (sdr["TotalCount"].ToString() != "" ? int.Parse(sdr["TotalCount"].ToString()) : materialReceipt.TotalCount);
                                    materialReceiptList.Add(materialReceipt);
                                }
                            }
                        }
                        con.Close();
                    }
                }
                return materialReceiptList;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        #endregion GetAllMaterialReceipt    

        #region MaterialReceipt DropDown
        public List<MaterialReceipt> MaterialReceiptForSelectList()
        {
            List<MaterialReceipt> materialReceiptList = null;
            MaterialReceipt materialReceipt = null;
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
                        cmd.CommandText = "[AMC].[GetAllMaterialReceipt]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                materialReceiptList = new List<MaterialReceipt>();
                                while (sdr.Read())
                                {
                                    materialReceipt = new MaterialReceipt();
                                    materialReceipt.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : materialReceipt.SupplierID);
                                    materialReceipt.ReceiptNo = (sdr["ReceiptNo"].ToString() != "" ? sdr["ReceiptNo"].ToString() : materialReceipt.ReceiptNo);
                                    materialReceiptList.Add(materialReceipt);
                                }
                            }
                        }
                    }
                }
            }
            catch(Exception Ex)
            {
                throw Ex;
            }
            return materialReceiptList;
        }
        #endregion MaterialReceipt DropDown
    }
}
