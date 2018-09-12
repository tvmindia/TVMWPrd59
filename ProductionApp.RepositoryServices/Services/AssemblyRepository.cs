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
        Settings settings = new Settings();
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
                                        assembly.EntryNo = (sdr["EntryNo"].ToString() != "" ? sdr["EntryNo"].ToString() : assembly.EntryNo);
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

        #region GetProductComponentList
        public List<Assembly> GetProductComponentList(Guid id,decimal qty,Guid assemblyId)
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
                        cmd.CommandText = "[AMC].[GetProductComponetDetailsForAssembly]";
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = id;
                        cmd.Parameters.Add("@Qty", SqlDbType.Decimal).Value = qty;
                        if(assemblyId !=Guid.Empty)
                            cmd.Parameters.Add("@AssembyID", SqlDbType.UniqueIdentifier).Value = assemblyId;
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
                                        //assembly.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : assembly.ID);
                                        assembly.Product = new Product();
                                        assembly.Material = new Material();
                                        assembly.Product.Name = (sdr["Name"].ToString() != "" ? sdr["Name"].ToString() : assembly.Product.Name);
                                        assembly.BOMQty = (sdr["Qty"].ToString() != "" ? Decimal.Parse(sdr["Qty"].ToString()) : assembly.BOMQty);
                                        assembly.Stock = (sdr["Stock"].ToString() != "" ? Decimal.Parse(sdr["Stock"].ToString()) : assembly.Stock);
                                        assembly.ReaquiredQty = (sdr["RequiredQty"].ToString() != "" ? Decimal.Parse(sdr["RequiredQty"].ToString()) : assembly.ReaquiredQty);
                                        assembly.Material.Description = (sdr["Description"].ToString() != "" ? sdr["Description"].ToString() : assembly.Material.Description);
                                        assembly.Balance = assembly.Stock - assembly.ReaquiredQty;
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
        #endregion GetProductComponentList

        #region InsertUpdateAssembly
        public object InsertUpdateAssembly(Assembly assembly)
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
                        cmd.CommandText = "[AMC].[InsertUpdateAssembly]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@IsUpdate", SqlDbType.Bit).Value = assembly.IsUpdate;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = assembly.ID;
                        cmd.Parameters.Add("@AssemblyDate", SqlDbType.DateTime).Value = assembly.AssemblyDateFormatted;
                        cmd.Parameters.Add("@AssembleBy", SqlDbType.UniqueIdentifier).Value = assembly.AssembleBy;
                        cmd.Parameters.Add("@ProductID", SqlDbType.UniqueIdentifier).Value = assembly.ProductID;
                        cmd.Parameters.Add("@Qty", SqlDbType.Decimal).Value = assembly.Qty;
                        cmd.Parameters.Add("@CreatedBy", SqlDbType.VarChar).Value = assembly.Common.CreatedBy;
                        cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = assembly.Common.CreatedDate;
                        cmd.Parameters.Add("@UpdatedBy", SqlDbType.VarChar).Value = assembly.Common.UpdatedBy;
                        cmd.Parameters.Add("@UpdatedDate", SqlDbType.DateTime).Value = assembly.Common.UpdatedDate;
                        outputStatus = cmd.Parameters.Add("@Status", SqlDbType.SmallInt);
                        outputStatus.Direction = ParameterDirection.Output;
                        OutputID = cmd.Parameters.Add("@IDOut", SqlDbType.UniqueIdentifier);
                        OutputID.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();
                    }
                }
                switch (outputStatus.Value.ToString())
                {
                    case "0":
                        throw new Exception(assembly.IsUpdate ? _appConst.UpdateFailure : _appConst.InsertFailure);
                    case "1":
                        assembly.ID = Guid.Parse(OutputID.Value.ToString());
                        return new
                        {
                            ID = Guid.Parse(OutputID.Value.ToString()),
                            Status = outputStatus.Value.ToString(),
                            Message = assembly.IsUpdate ? _appConst.UpdateSuccess : _appConst.InsertSuccess
                        };
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return new
            {
                ID = Guid.Parse(OutputID.Value.ToString()),
                Status = outputStatus.Value.ToString(),
                Message = assembly.IsUpdate ? _appConst.UpdateSuccess : _appConst.InsertSuccess
            };
        }
        #endregion InsertUpdateAssembly

        #region GetAssembly
        public Assembly GetAssembly(Guid id)
        {
            Assembly assembly = null;

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
                        cmd.CommandText = "[AMC].[GetAssembly]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = id;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                if (sdr.Read())
                                {
                                    assembly = new Assembly();
                                    assembly.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : assembly.ID);
                                    assembly.EntryNo = (sdr["EntryNo"].ToString() != "" ? sdr["EntryNo"].ToString() : assembly.EntryNo);
                                    assembly.AssemblyDateFormatted = (sdr["AssemblyDate"].ToString() != "" ? DateTime.Parse(sdr["AssemblyDate"].ToString()).ToString("dd-MMM-yyyy").ToString() : assembly.AssemblyDateFormatted);
                                    assembly.AssemblyDate = (sdr["AssemblyDate"].ToString() != "" ? DateTime.Parse(sdr["AssemblyDate"].ToString()) : assembly.AssemblyDate);
                                    assembly.Qty = (sdr["Qty"].ToString() != "" ? Decimal.Parse(sdr["Qty"].ToString()) : assembly.Qty);
                                    assembly.ProductID = (sdr["ProductID"].ToString() != "" ? Guid.Parse(sdr["ProductID"].ToString()) : assembly.ProductID);
                                    assembly.AssembleBy = (sdr["AssembleBy"].ToString() != "" ? Guid.Parse(sdr["AssembleBy"].ToString()) : assembly.AssembleBy);
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
            return assembly;
        }
        #endregion GetAssembly

        #region DeleteAssembly
        public object DeleteAssembly(Guid id,string createdBy)
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
                        cmd.CommandText = "[AMC].[DeleteAssembly]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = id;
                        cmd.Parameters.Add("@CreatedBy", SqlDbType.VarChar).Value = createdBy;
                        outputStatus = cmd.Parameters.Add("@Status", SqlDbType.SmallInt);
                        outputStatus.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return new
            {
                Status = outputStatus.Value.ToString(),
            };
        }
        #endregion DeleteAssembly

        #region GetRecentAssemblyProduct
        public List<Assembly> GetRecentAssemblyProduct()
        {
            List<Assembly> assemblyList = new List<Assembly>();
            Assembly assembly = null;
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
                        cmd.CommandText = "[AMC].[GetRecentAssemblyProduct]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                while (sdr.Read())
                                {
                                    assembly = new Assembly();
                                    assembly.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : Guid.Empty);
                                    assembly.EntryNo = (sdr["EntryNo"].ToString() != "" ? sdr["EntryNo"].ToString() : assembly.EntryNo);
                                    assembly.AssemblyDateFormatted = (sdr["AssemblyDate"].ToString() != "" ? DateTime.Parse(sdr["AssemblyDate"].ToString()).ToString(settings.DateFormat) : assembly.AssemblyDateFormatted);
                                    assembly.Product = new Product();
                                    assembly.Product.Name = (sdr["Product"].ToString() != "" ? sdr["Product"].ToString() : assembly.Product.Name);
                                    assembly.Employee = new Employee();
                                    assembly.Employee.Name = (sdr["EmployeeName"].ToString() != "" ? sdr["EmployeeName"].ToString() : assembly.Employee.Name);
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
        #endregion GetRecentAssemblyProduct

        #region GetPossibleItemQuantityForAssembly
        public List<Assembly> GetPossibleItemQuantityForAssembly(Guid id)
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
                        cmd.CommandText = "[AMC].[GetMaximumPossibleQuantityForAssembly]";
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = id;                      
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
                                        assembly.MaxAvailableQuantity = (sdr["MaxQtyAvailable"].ToString() != "" ? Decimal.Parse(sdr["MaxQtyAvailable"].ToString()) : assembly.MaxAvailableQuantity);
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
        #endregion GetPossibleItemQuantityForAssembly


    }
}
