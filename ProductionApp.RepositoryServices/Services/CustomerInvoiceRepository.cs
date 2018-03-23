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
    public class CustomerInvoiceRepository: ICustomerInvoiceRepository
    {
        private IDatabaseFactory _databaseFactory;
        public CustomerInvoiceRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }

        public List<CustomerInvoiceDetail> GetPackingSlipDetailForCustomerInvoice(Guid packingSlipID)
        {
            List<CustomerInvoiceDetail> customerInvoiceDetailList = null;
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
                        cmd.CommandText = "[AMC].[GetPackingSlipDetailForCustomerInvoice]";
                        cmd.Parameters.Add("@PackingSlipID", SqlDbType.UniqueIdentifier).Value = packingSlipID;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                customerInvoiceDetailList = new List<CustomerInvoiceDetail>();
                                while (sdr.Read())
                                {
                                    CustomerInvoiceDetail customerInvoiceDetail = new CustomerInvoiceDetail();
                                    {
                                        customerInvoiceDetail.ProductID = (sdr["ProductID"].ToString() != "" ? Guid.Parse(sdr["ProductID"].ToString()) : customerInvoiceDetail.ProductID);
                                        customerInvoiceDetail.ProductName = (sdr["ProductName"].ToString() != "" ? sdr["ProductName"].ToString() : customerInvoiceDetail.ProductName);
                                        customerInvoiceDetail.Quantity = (sdr["Quantity"].ToString() != "" ? decimal.Parse(sdr["Quantity"].ToString()) : customerInvoiceDetail.Quantity);
                                        customerInvoiceDetail.Weight = (sdr["Weight"].ToString() != "" ? decimal.Parse(sdr["Weight"].ToString()) : customerInvoiceDetail.Weight);
                                        customerInvoiceDetail.IsInvoiceInKG = (sdr["IsInvoiceInKG"].ToString() != "" ? bool.Parse(sdr["IsInvoiceInKG"].ToString()) : customerInvoiceDetail.IsInvoiceInKG);
                                        customerInvoiceDetail.Rate = (sdr["Rate"].ToString() != "" ? decimal.Parse(sdr["Rate"].ToString()) : customerInvoiceDetail.Rate);
                                        customerInvoiceDetail.TradeDiscountAmount = (sdr["TradeDiscountAmount"].ToString() != "" ? decimal.Parse(sdr["TradeDiscountAmount"].ToString()) : customerInvoiceDetail.TradeDiscountAmount);
                                        customerInvoiceDetail.TradeDiscountPerc = (sdr["TradeDiscountPerc"].ToString() != "" ? decimal.Parse(sdr["TradeDiscountPerc"].ToString()) : customerInvoiceDetail.TradeDiscountPerc);
                                        customerInvoiceDetail.TaxTypeCode = (sdr["TaxTypeCode"].ToString() != "" ? sdr["TaxTypeCode"].ToString() : customerInvoiceDetail.TaxTypeCode);
                                        customerInvoiceDetail.SGSTPerc = (sdr["SGSTPerc"].ToString() != "" ? decimal.Parse(sdr["SGSTPerc"].ToString()) : customerInvoiceDetail.SGSTPerc);
                                        customerInvoiceDetail.CGSTPerc = (sdr["CGSTPerc"].ToString() != "" ? decimal.Parse(sdr["CGSTPerc"].ToString()) : customerInvoiceDetail.CGSTPerc);
                                        customerInvoiceDetail.IGSTPerc = (sdr["IGSTPerc"].ToString() != "" ? decimal.Parse(sdr["IGSTPerc"].ToString()) : customerInvoiceDetail.IGSTPerc);
                                        customerInvoiceDetail.Total = (sdr["Total"].ToString() != "" ? decimal.Parse(sdr["Total"].ToString()) : customerInvoiceDetail.Total);

                                    }
                                    customerInvoiceDetailList.Add(customerInvoiceDetail);
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
            return customerInvoiceDetailList;
        }

    }
}
