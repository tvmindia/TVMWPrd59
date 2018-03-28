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

        Settings settings = new Settings();
        AppConst _appConst = new AppConst();

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
                                        customerInvoiceDetail.PackingSlipDetailID = (sdr["PackingSlipDetailID"].ToString() != "" ? Guid.Parse(sdr["PackingSlipDetailID"].ToString()) : customerInvoiceDetail.PackingSlipDetailID);
                                        customerInvoiceDetail.QuantityCheck = (sdr["Quantity"].ToString() != "" ? decimal.Parse(sdr["Quantity"].ToString()) : customerInvoiceDetail.QuantityCheck);
                                        customerInvoiceDetail.WeightCheck = (sdr["Weight"].ToString() != "" ? decimal.Parse(sdr["Weight"].ToString()) : customerInvoiceDetail.WeightCheck);
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

        public object InsertUpdateCustomerInvoice(CustomerInvoice customerInvoice )
        {
            SqlParameter outputStatus, IDOut = null;
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
                        cmd.CommandText = "[AMC].[InsertUpdateCustomerInvoice]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@IsUpdate", SqlDbType.Bit).Value = customerInvoice.IsUpdate;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = customerInvoice.ID;
                        cmd.Parameters.Add("@CustomerID", SqlDbType.UniqueIdentifier).Value = customerInvoice.CustomerID;
                        cmd.Parameters.Add("@FileDupID", SqlDbType.UniqueIdentifier).Value = customerInvoice.hdnFileID;
                        cmd.Parameters.Add("@PaymentTermCode", SqlDbType.VarChar,10 ).Value = customerInvoice.PaymentTerm;
                        cmd.Parameters.Add("@InvoiceDate", SqlDbType.DateTime).Value = customerInvoice.InvoiceDateFormatted;
                        cmd.Parameters.Add("@PaymentDueDate", SqlDbType.DateTime).Value = customerInvoice.PaymentDueDateFormatted;
                        cmd.Parameters.Add("@DetailXML", SqlDbType.VarChar, -1).Value = customerInvoice.DetailXML;
                        cmd.Parameters.Add("@Discount", SqlDbType.Decimal).Value = customerInvoice.Discount;
                        
                        cmd.Parameters.Add("@BillingAddress", SqlDbType.VarChar,-1 ).Value = customerInvoice.BillingAddress;
                        cmd.Parameters.Add("@GeneralNotes", SqlDbType.VarChar,-1 ).Value = customerInvoice.GeneralNotes;
                        cmd.Parameters.Add("@CreatedBy", SqlDbType.NVarChar, 250).Value = customerInvoice.Common.CreatedBy;
                        cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = customerInvoice.Common.CreatedDate;
                        cmd.Parameters.Add("@UpdatedBy", SqlDbType.NVarChar, 250).Value = customerInvoice.Common.UpdatedBy;
                        cmd.Parameters.Add("@UpdatedDate", SqlDbType.DateTime).Value = customerInvoice.Common.UpdatedDate;
                        outputStatus = cmd.Parameters.Add("@Status", SqlDbType.SmallInt);
                        outputStatus.Direction = ParameterDirection.Output;
                        IDOut = cmd.Parameters.Add("@IDOut", SqlDbType.UniqueIdentifier);
                        IDOut.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();
                    }
                }

                switch (outputStatus.Value.ToString())
                {
                    case "0":
                        throw new Exception(customerInvoice.IsUpdate ? _appConst.UpdateFailure : _appConst.InsertFailure);
                    case "1":
                        //  requisition.ID = Guid.Parse(IDOut.Value.ToString());
                        return new
                        {
                            ID = IDOut.Value.ToString(),
                            Status = outputStatus.Value.ToString(),
                            Message = customerInvoice.IsUpdate ? _appConst.UpdateSuccess : _appConst.InsertSuccess
                        };
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return new
            {
                Code = IDOut.Value.ToString(),
                Status = outputStatus.Value.ToString(),
                Message = customerInvoice.IsUpdate ? _appConst.UpdateSuccess : _appConst.InsertSuccess
            };
        }
    }
}
