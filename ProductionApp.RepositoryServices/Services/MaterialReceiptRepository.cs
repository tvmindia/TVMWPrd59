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
        public List<MaterialReceipt> GetAllMaterialReceipt()
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
                                materialReceipt.ReceiptDateFormatted = (sdr["ReceiptDate"].ToString() != "" ? sdr["ReceiptDate"].ToString() : materialReceipt.ReceiptDateFormatted);
                                materialReceipt.Supplier.CompanyName = (sdr["SupplierName"].ToString() != "" ? sdr["SupplierName"].ToString() : materialReceipt.Supplier.CompanyName);
                                materialReceipt.GeneralNotes = (sdr["GeneralNotes"].ToString() != "" ? sdr["GeneralNotes"].ToString() : materialReceipt.GeneralNotes);
                                materialReceipt.Common.CreatedBy = (sdr["CreatedBy"].ToString() != "" ? sdr["CreatedBy"].ToString() : materialReceipt.Common.CreatedBy);
                                materialReceipt.Common.UpdatedBy = (sdr["UpdatedBy"].ToString() != "" ? sdr["UpdatedBy"].ToString() : materialReceipt.Common.UpdatedBy);
                                materialReceipt.Common.CreatedDateString = (sdr["CreatedDate"].ToString() != "" ? sdr["CreatedDate"].ToString() : materialReceipt.Common.CreatedDateString);
                                materialReceipt.Common.UpdatedDateString = (sdr["UpdatedDate"].ToString() != "" ? sdr["UpdatedDate"].ToString() : materialReceipt.Common.UpdatedDateString);
                                materialReceiptList.Add(materialReceipt);
                            }
                        }
                    }
                }
            }
            return materialReceiptList;
        }
        #endregion GetAllMaterialReceipt    
    }
}
