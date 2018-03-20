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
    public class PaymentTermRepository: IPaymentTermRepository
    {
        private IDatabaseFactory _databaseFactory;
        public PaymentTermRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }
        public List<PaymentTerm> GetAllPaymentTerm()
        {
            List<PaymentTerm> paymentTermList = null;
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
                        cmd.CommandText = "[AMC].[GetAllPaymentTerm]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                paymentTermList = new List<PaymentTerm>();
                                while (sdr.Read())
                                {
                                    PaymentTerm paymentTerm = new PaymentTerm();
                                    {
                                        paymentTerm.Code = (sdr["Code"].ToString() != "" ? (sdr["Code"].ToString()) : paymentTerm.Code);
                                        paymentTerm.Description = (sdr["Description"].ToString() != "" ? sdr["Description"].ToString() : paymentTerm.Description);
                                        paymentTerm.NoOfDays = (sdr["NoOfDays"].ToString() != "" ? int.Parse(sdr["NoOfDays"].ToString()) : paymentTerm.NoOfDays);

                                    }
                                    paymentTermList.Add(paymentTerm);
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

            return paymentTermList;
        }

        public PaymentTerm GetPaymentTermDetails(string Code)
        {
            PaymentTerm paymentTerm = new PaymentTerm();
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
                        cmd.CommandText = "[AMC].[GetPaymentTerm]";
                        cmd.Parameters.Add("@Code", SqlDbType.NVarChar, 10).Value = Code;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                if (sdr.Read())
                                {
                                    paymentTerm.Code = (sdr["Code"].ToString() != "" ? (sdr["Code"].ToString()) : paymentTerm.Code);
                                    paymentTerm.Description = (sdr["Description"].ToString() != "" ? sdr["Description"].ToString() : paymentTerm.Description);
                                    paymentTerm.NoOfDays = (sdr["NoOfDays"].ToString() != "" ? int.Parse(sdr["NoOfDays"].ToString()) : paymentTerm.NoOfDays);

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

            return paymentTerm;
        }
    }
}
