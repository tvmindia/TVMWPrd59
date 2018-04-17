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
    public class AssemblyRepository : IAssemblyRepository
    {
        private IDatabaseFactory _databaseFactory;
        AppConst _appConst = new AppConst();

        #region Constructor Injection
        /// <summary>
        /// Constructor Injection:-Getting IDatabaseFactory implementing object
        /// </summary>
        /// <param name="databaseFactory"></param>
        public AssemblyRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }
        #endregion Constructor Injection

        #region GetAllAssembly
        public List<Assembly> GetAllAssembly(AssemblyAdvanceSearch assemblyAdvanceSearch)
        {
            List<Assembly> assemblyList = null;
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
                        cmd.CommandText = "[AMC].[GetAllAssembly]";
                        cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, -1).Value = string.IsNullOrEmpty(assemblyAdvanceSearch.SearchTerm) ? "" : assemblyAdvanceSearch.SearchTerm;
                        cmd.Parameters.Add("@RowStart", SqlDbType.Int).Value = assemblyAdvanceSearch.DataTablePaging.Start;
                        if (assemblyAdvanceSearch.DataTablePaging.Length == -1)
                            cmd.Parameters.AddWithValue("@Length", DBNull.Value);
                        else
                            cmd.Parameters.Add("@Length", SqlDbType.Int).Value = assemblyAdvanceSearch.DataTablePaging.Length;
                        cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = assemblyAdvanceSearch.FromDate;
                        cmd.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = assemblyAdvanceSearch.ToDate;
                        if (assemblyAdvanceSearch.ProductID != Guid.Empty)
                            cmd.Parameters.Add("@ProductID", SqlDbType.UniqueIdentifier).Value = assemblyAdvanceSearch.ProductID;
                        if (assemblyAdvanceSearch.AssembleBy != Guid.Empty)
                            cmd.Parameters.Add("@AssemblyBy", SqlDbType.UniqueIdentifier).Value = assemblyAdvanceSearch.AssembleBy;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                assemblyList = new List<Assembly>();
                                while (sdr.Read())
                                {
                                    Assembly assembly = new Assembly();
                                    {
                                        assembly.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : assembly.ID);
                                        assembly.AssemblyDateFormatted = (sdr["AssemblyDate"].ToString() != "" ? DateTime.Parse(sdr["AssemblyDate"].ToString()).ToString("dd-MMM-yyyy").ToString() : assembly.AssemblyDateFormatted);
                                        assembly.AssemblyDate = (sdr["AssemblyDate"].ToString() != "" ? DateTime.Parse(sdr["AssemblyDate"].ToString()) : assembly.AssemblyDate);
                                        assembly.Product = new Product();
                                        assembly.Product.Name = (sdr["Product"].ToString() != "" ? sdr["Product"].ToString() : assembly.Product.Name);
                                        assembly.Employee = new Employee();
                                        assembly.Employee.Name = (sdr["AssembledBY"].ToString() != "" ? sdr["AssembledBY"].ToString() : assembly.Employee.Name);
                                        assembly.Qty = (sdr["Qty"].ToString() != "" ? Decimal.Parse(sdr["Qty"].ToString()) : assembly.Qty);
                                        assembly.TotalCount = (sdr["TotalCount"].ToString() != "" ? int.Parse(sdr["TotalCount"].ToString()) : assembly.TotalCount);
                                        assembly.FilteredCount = (sdr["FilteredCount"].ToString() != "" ? int.Parse(sdr["FilteredCount"].ToString()) : assembly.FilteredCount);
                                    }
                                    assemblyList.Add(assembly);
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
            return assemblyList;
        }
        #endregion GetAllAssembly
    }
}
