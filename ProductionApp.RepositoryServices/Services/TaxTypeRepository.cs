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
    public class TaxTypeRepository: ITaxTypeRepository
    {
        private IDatabaseFactory _databaseFactory;
        public TaxTypeRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }

        public List<TaxType> GetTaxTypeForSelectList()
        {
            List<TaxType> taxTypeList = null;
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
                        cmd.CommandText = "[AMC].[GetTaxTypeForSelectList]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                taxTypeList = new List<TaxType>();
                                while (sdr.Read())
                                {
                                    TaxType taxType = new TaxType();
                                    {
                                        taxType.Code = (sdr["Code"].ToString() != "" ? sdr["Code"].ToString() : taxType.Code);
                                        taxType.Description = (sdr["Description"].ToString() != "" ? sdr["Description"].ToString() : taxType.Description);
                                    }
                                    taxTypeList.Add(taxType);
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
            return taxTypeList;
        }

        public TaxType GetTaxTypeDetailsByCode(string Code)
        {
            TaxType taxType = new TaxType();
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
                        cmd.CommandText = "[AMC].[GetTaxtypeByCode]";
                        cmd.Parameters.Add("@Code", SqlDbType.NVarChar).Value = Code;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                if (sdr.Read())
                                {
                                    
                                    taxType.Code = (sdr["Code"].ToString() != "" ? sdr["Code"].ToString() : taxType.Code);
                                    taxType.Description = (sdr["Description"].ToString() != "" ? sdr["Description"].ToString() : taxType.Description);
                                    taxType.CGSTPercentage= (sdr["CGSTPercentage"].ToString() != "" ? decimal.Parse(sdr["CGSTPercentage"].ToString()) : taxType.CGSTPercentage);
                                    taxType.SGSTPercentage = (sdr["SGSTPercentage"].ToString() != "" ? decimal.Parse(sdr["SGSTPercentage"].ToString()) : taxType.SGSTPercentage);
                                    taxType.IGSTPercentage = (sdr["IGSTPercentage"].ToString() != "" ? decimal.Parse(sdr["IGSTPercentage"].ToString()) : taxType.IGSTPercentage);
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
            return taxType;
        }
    }
}
