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
    public class MaterialReturnRepository: IMaterialReturnRepository
    {
        private IDatabaseFactory _databaseFactory;
        AppConst _appConst = new AppConst();
        Settings settings = new Settings();
        public MaterialReturnRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }
        #region GetAllReturnToSupplier
        public List<MaterialReturn> GetAllReturnToSupplier(MaterialReturnAdvanceSearch materialReturnAdvanceSearch)
        {
            List<MaterialReturn> materialReturnList = null;
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
                        cmd.CommandText = "[AMC].[GetAllReturnToSupplier]";
                        cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, -1).Value = string.IsNullOrEmpty(materialReturnAdvanceSearch.SearchTerm) ? "" : materialReturnAdvanceSearch.SearchTerm;
                        cmd.Parameters.Add("@RowStart", SqlDbType.Int).Value = materialReturnAdvanceSearch.DataTablePaging.Start;
                        if (materialReturnAdvanceSearch.DataTablePaging.Length == -1)
                            cmd.Parameters.AddWithValue("@Length", DBNull.Value);
                        else
                            cmd.Parameters.Add("@Length", SqlDbType.Int).Value = materialReturnAdvanceSearch.DataTablePaging.Length;
                        cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = materialReturnAdvanceSearch.FromDate;
                        cmd.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = materialReturnAdvanceSearch.ToDate;
                        if (materialReturnAdvanceSearch.ReturnBy != Guid.Empty)
                            cmd.Parameters.Add("@ReturnBy", SqlDbType.UniqueIdentifier).Value = materialReturnAdvanceSearch.ReturnBy;
                        if (materialReturnAdvanceSearch.SupplierID != Guid.Empty)
                            cmd.Parameters.Add("@SupplierID", SqlDbType.UniqueIdentifier).Value = materialReturnAdvanceSearch.SupplierID;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                materialReturnList = new List<MaterialReturn>();
                                while (sdr.Read())
                                {
                                    MaterialReturn materiaReturn = new MaterialReturn();
                                    {
                                        materiaReturn.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : materiaReturn.ID);
                                        materiaReturn.ReturnSlipNo = (sdr["ReturnSlipNo"].ToString() != "" ? sdr["ReturnSlipNo"].ToString() : materiaReturn.ReturnSlipNo);
                                        materiaReturn.ReturnDate = (sdr["ReturnDate"].ToString() != "" ? DateTime.Parse(sdr["ReturnDate"].ToString()) : materiaReturn.ReturnDate);
                                        materiaReturn.ReturnDateFormatted = (sdr["ReturnDate"].ToString() != "" ? DateTime.Parse(sdr["ReturnDate"].ToString()).ToString(settings.DateFormat) : materiaReturn.ReturnDateFormatted);
                                        materiaReturn.ReturnByEmployeeName = (sdr["ReturnByEmployeeName"].ToString() != "" ? sdr["ReturnByEmployeeName"].ToString() : materiaReturn.ReturnByEmployeeName);
                                        materiaReturn.SupplierName = (sdr["SupplierName"].ToString() != "" ? sdr["SupplierName"].ToString() : materiaReturn.SupplierName);
                                        materiaReturn.TotalCount = (sdr["TotalCount"].ToString() != "" ? int.Parse(sdr["TotalCount"].ToString()) : materiaReturn.TotalCount);
                                        materiaReturn.FilteredCount = (sdr["FilteredCount"].ToString() != "" ? int.Parse(sdr["FilteredCount"].ToString()) : materiaReturn.FilteredCount);
                                    }
                                    materialReturnList.Add(materiaReturn);
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
            return materialReturnList;
        }

        #endregion GetAllReturnToSupplier
    }
}
