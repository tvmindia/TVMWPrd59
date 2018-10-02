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
    public class PurchaseOrderRepository: IPurchaseOrderRepository
    {
        private IDatabaseFactory _databaseFactory;
        AppConst _appConst = new AppConst();
        Settings settings = new Settings();
        public PurchaseOrderRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }
        public List<PurchaseOrder>GetAllPurchaseOrder(PurchaseOrderAdvanceSearch purchaseOrderAdvanceSearch)
        {
            List<PurchaseOrder> purchaseOrderList = null;
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
                        cmd.CommandText = "[AMC].[GetAllPurchaseOrder]";
                        cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, -1).Value = string.IsNullOrEmpty(purchaseOrderAdvanceSearch.SearchTerm) ? "" : purchaseOrderAdvanceSearch.SearchTerm;
                        cmd.Parameters.Add("@RowStart", SqlDbType.Int).Value = purchaseOrderAdvanceSearch.DataTablePaging.Start;
                        cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = purchaseOrderAdvanceSearch.FromDate;
                        cmd.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = purchaseOrderAdvanceSearch.ToDate;
                        cmd.Parameters.Add("@Status", SqlDbType.VarChar).Value = purchaseOrderAdvanceSearch.Status;
                        if(purchaseOrderAdvanceSearch.Supplier.ID == Guid.Empty)
                            cmd.Parameters.AddWithValue("@SupplierID", DBNull.Value);
                        else
                        cmd.Parameters.Add("@SupplierID", SqlDbType.UniqueIdentifier).Value = purchaseOrderAdvanceSearch.Supplier.ID;
                        if (purchaseOrderAdvanceSearch.DataTablePaging.Length == -1)
                            cmd.Parameters.AddWithValue("@Length", DBNull.Value);
                        else
                            cmd.Parameters.Add("@Length", SqlDbType.Int).Value = purchaseOrderAdvanceSearch.DataTablePaging.Length;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                purchaseOrderList = new List<PurchaseOrder>();
                                while (sdr.Read())
                                {
                                    PurchaseOrder purchaseOrder = new PurchaseOrder();
                                    {
                                        purchaseOrder.ID= (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : purchaseOrder.ID);
                                        purchaseOrder.PurchaseOrderNo = (sdr["PurchaseOrderNo"].ToString() != "" ? sdr["PurchaseOrderNo"].ToString() : purchaseOrder.PurchaseOrderNo);
                                        purchaseOrder.PurchaseOrderDate = (sdr["PurchaseOrderDate"].ToString() != "" ? DateTime.Parse(sdr["PurchaseOrderDate"].ToString()): purchaseOrder.PurchaseOrderDate);
                                        purchaseOrder.PurchaseOrderDateFormatted = (sdr["PurchaseOrderDate"].ToString() != "" ? DateTime.Parse(sdr["PurchaseOrderDate"].ToString()).ToString(settings.DateFormat) : purchaseOrder.PurchaseOrderDateFormatted);
                                        purchaseOrder.PurchaseOrderIssuedDate = (sdr["PurchaseOrderIssuedDate"].ToString() != "" ? DateTime.Parse(sdr["PurchaseOrderIssuedDate"].ToString()) : purchaseOrder.PurchaseOrderIssuedDate);
                                        purchaseOrder.PurchaseOrderIssuedDateFormatted = (sdr["PurchaseOrderIssuedDate"].ToString() != "" ? DateTime.Parse(sdr["PurchaseOrderIssuedDate"].ToString()).ToString(settings.DateFormat) : purchaseOrder.PurchaseOrderIssuedDateFormatted);
                                        purchaseOrder.Supplier = (sdr["Supplier"].ToString() != "" ? sdr["Supplier"].ToString() : purchaseOrder.Supplier);
                                        purchaseOrder.SupplierID = (sdr["SupplierID"].ToString() != "" ? Guid.Parse(sdr["SupplierID"].ToString()) : purchaseOrder.SupplierID);
                                        purchaseOrder.PurchaseOrderStatus = (sdr["PurchaseOrderStatus"].ToString() != "" ? sdr["PurchaseOrderStatus"].ToString() : purchaseOrder.PurchaseOrderStatus);
                                        purchaseOrder.PurchaseOrderTitle = (sdr["PurchaseOrderTitle"].ToString() != "" ? sdr["PurchaseOrderTitle"].ToString() : purchaseOrder.PurchaseOrderTitle);
                                        purchaseOrder.ApprovalStatus = (sdr["ApprovalStatus"].ToString() != "" ? sdr["ApprovalStatus"].ToString() : purchaseOrder.ApprovalStatus);
                                        purchaseOrder.TotalCount= (sdr["TotalCount"].ToString() != "" ? int.Parse(sdr["TotalCount"].ToString()) : purchaseOrder.TotalCount);
                                        purchaseOrder.FilteredCount = (sdr["FilteredCount"].ToString() != "" ? int.Parse(sdr["FilteredCount"].ToString()) : purchaseOrder.FilteredCount);
                                    }
                                    purchaseOrderList.Add(purchaseOrder);
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

            return purchaseOrderList;
        }

        #region PurchaseOrder Dropdown
        public List<PurchaseOrder> PurchaseOrderDropdownList(Guid supplierID)
        {
            List<PurchaseOrder> purchaseOrderList = null;
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
                        cmd.CommandText = "[AMC].[GetPurchaseOrderForSelectList]";
                        if (supplierID != Guid.Empty)
                            cmd.Parameters.Add("@SupplierID", SqlDbType.UniqueIdentifier).Value = supplierID;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                purchaseOrderList = new List<PurchaseOrder>();
                                while (sdr.Read())
                                {
                                    PurchaseOrder purchaseOrder = new PurchaseOrder();
                                    {
                                        purchaseOrder.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : purchaseOrder.ID);
                                        purchaseOrder.PurchaseOrderNo = (sdr["PurchaseOrderNo"].ToString() != "" ? sdr["PurchaseOrderNo"].ToString() : purchaseOrder.PurchaseOrderNo);
                                    }
                                    purchaseOrderList.Add(purchaseOrder);
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

            return purchaseOrderList;
        }
        #endregion PurchaseOrder Dropdown

        #region InsertPurchaseOrder
        public object InsertPurchaseOrder(PurchaseOrder purchaseOrder)
        {
            SqlParameter outputStatus, outputID;
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
                        cmd.CommandText = "[AMC].[InsertPurchaseOrder]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        //cmd.Parameters.Add("@PONo", SqlDbType.VarChar, 20).Value = purchaseOrder.PurchaseOrderNo;
                        cmd.Parameters.Add("@PODate", SqlDbType.DateTime).Value = purchaseOrder.PurchaseOrderDate;
                        cmd.Parameters.Add("@POIssuedDate", SqlDbType.DateTime).Value = purchaseOrder.PurchaseOrderIssuedDate;
                        cmd.Parameters.Add("@SupplierID", SqlDbType.UniqueIdentifier).Value = purchaseOrder.SupplierID;
                        cmd.Parameters.Add("@POTitle", SqlDbType.VarChar, 500).Value = purchaseOrder.PurchaseOrderTitle;
                        cmd.Parameters.Add("@MailingAddress", SqlDbType.NVarChar, -1).Value = purchaseOrder.MailingAddress;
                        cmd.Parameters.Add("@ShippingAddress", SqlDbType.NVarChar, -1).Value = purchaseOrder.ShippingAddress;
                        cmd.Parameters.Add("@MailBodyHeader", SqlDbType.VarChar, 500).Value = purchaseOrder.MailBodyHeader;
                        cmd.Parameters.Add("@MailBodyFooter", SqlDbType.NVarChar, -1).Value = purchaseOrder.MailBodyFooter;
                        cmd.Parameters.Add("@GeneralNotes", SqlDbType.NVarChar, -1).Value = purchaseOrder.GeneralNotes;
                        cmd.Parameters.Add("@POStatus", SqlDbType.VarChar,10).Value = purchaseOrder.PurchaseOrderStatus;
                        cmd.Parameters.Add("@Discount", SqlDbType.Decimal).Value = purchaseOrder.Discount;
                        cmd.Parameters.Add("@PODDetailLinkXML", SqlDbType.NVarChar, -1).Value = purchaseOrder.PODDetailLinkXML;
                        cmd.Parameters.Add("@PODDetailXML", SqlDbType.NVarChar, -1).Value = purchaseOrder.PODDetailXML;
                        cmd.Parameters.Add("@CreatedBy", SqlDbType.NVarChar, 250).Value = purchaseOrder.Common.CreatedBy;
                        cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = purchaseOrder.Common.CreatedDate;
                        outputStatus = cmd.Parameters.Add("@Status", SqlDbType.SmallInt);
                        outputStatus.Direction = ParameterDirection.Output;
                        outputID = cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier);
                        outputID.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();
                    }
                }
                //AppConst Cobj = new AppConst();
                switch (outputStatus.Value.ToString())
                {
                    case "0":
                        throw new Exception(_appConst.InsertFailure);
                    case "1":
                        //success
                        return new
                        {
                            ID = outputID.Value.ToString(),
                            Status = outputStatus.Value.ToString(),
                            Message = _appConst.InsertSuccess
                        };
                    case "2":
                        //Exceed the limit
                        return new
                        {
                            Status = outputStatus.Value.ToString(),
                            Message = _appConst.InsertFailure
                        };

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
                ID = outputID.Value.ToString(),
                Status = outputStatus.Value.ToString(),
                Message = _appConst.InsertSuccess
            };
        }
        #endregion InsertPurcgaseOrder

        #region UpdatePurchaseOrder
        public object UpdatePurchaseOrder(PurchaseOrder purchaseOrder)
        {
            SqlParameter outputStatus = null, outputID;
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
                        cmd.CommandText = "[AMC].[UpdatePurchaseOrder]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = purchaseOrder.ID;
                        cmd.Parameters.Add("@PONo", SqlDbType.VarChar, 20).Value = purchaseOrder.PurchaseOrderNo;
                        cmd.Parameters.Add("@PODate", SqlDbType.DateTime).Value = purchaseOrder.PurchaseOrderDate;
                        cmd.Parameters.Add("@POIssuedDate", SqlDbType.DateTime).Value = purchaseOrder.PurchaseOrderIssuedDate;
                        cmd.Parameters.Add("@SupplierID", SqlDbType.UniqueIdentifier).Value = purchaseOrder.SupplierID;
                        cmd.Parameters.Add("@POTitle", SqlDbType.VarChar, 500).Value = purchaseOrder.PurchaseOrderTitle;
                        cmd.Parameters.Add("@MailingAddress", SqlDbType.NVarChar, -1).Value = purchaseOrder.MailingAddress;
                        cmd.Parameters.Add("@ShippingAddress", SqlDbType.NVarChar, -1).Value = purchaseOrder.ShippingAddress;
                        cmd.Parameters.Add("@MailBodyHeader", SqlDbType.VarChar, 500).Value = purchaseOrder.MailBodyHeader;
                        cmd.Parameters.Add("@MailBodyFooter", SqlDbType.NVarChar, -1).Value = purchaseOrder.MailBodyFooter;
                        cmd.Parameters.Add("@GeneralNotes", SqlDbType.NVarChar, -1).Value = purchaseOrder.GeneralNotes;
                        cmd.Parameters.Add("@POStatus", SqlDbType.VarChar, 10).Value = purchaseOrder.PurchaseOrderStatus;
                        cmd.Parameters.Add("@Discount", SqlDbType.Decimal).Value = purchaseOrder.Discount;
                        cmd.Parameters.Add("@reqDetailLinkObjXML", SqlDbType.NVarChar, -1).Value = purchaseOrder.PODDetailLinkXML;
                        cmd.Parameters.Add("@reqDetailObjXML", SqlDbType.NVarChar, -1).Value = purchaseOrder.PODDetailXML;
                        cmd.Parameters.Add("@UpdatedBy", SqlDbType.NVarChar, 250).Value = purchaseOrder.Common.UpdatedBy;
                        cmd.Parameters.Add("@UpdatedDate", SqlDbType.DateTime).Value = purchaseOrder.Common.UpdatedDate;
                        outputStatus = cmd.Parameters.Add("@Status", SqlDbType.SmallInt);
                        outputStatus.Direction = ParameterDirection.Output;
                        outputID = cmd.Parameters.Add("@ID1", SqlDbType.UniqueIdentifier);
                        outputID.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();

                    }
                }
                //AppConst Cobj = new AppConst();
                switch (outputStatus.Value.ToString())
                {
                    case "0":
                        throw new Exception(_appConst.UpdateFailure);

                    case "1":
                        return new
                        {
                            ID = outputID.Value.ToString(),
                            Status = outputStatus.Value.ToString(),
                            Message = _appConst.UpdateSuccess
                        };
                    case "2":
                        //Exceed the limit
                        return new
                        {
                            Status = outputStatus.Value.ToString(),
                            Message = _appConst.UpdateFailure
                        };
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
                ID = outputID.Value.ToString(),
                Status = outputStatus.Value.ToString(),
                Message = _appConst.UpdateSuccess
            };
        }
        #endregion UpdatePurchaseOrder

        #region UpdatePOMailDetails
        public object UpdatePOMailDetails(PurchaseOrder purchaseOrder)
        {
            SqlParameter outputStatus = null, outputID;
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
                        cmd.CommandText = "[AMC].[UpdatePurchaseOrderMail]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = purchaseOrder.ID;
                        cmd.Parameters.Add("@MailBodyHeader", SqlDbType.VarChar, 500).Value = purchaseOrder.MailBodyHeader;
                        cmd.Parameters.Add("@MailBodyFooter", SqlDbType.NVarChar, -1).Value = purchaseOrder.MailBodyFooter;
                        cmd.Parameters.Add("@UpdatedBy", SqlDbType.NVarChar, 250).Value = purchaseOrder.Common.UpdatedBy;
                        cmd.Parameters.Add("@UpdatedDate", SqlDbType.DateTime).Value = purchaseOrder.Common.UpdatedDate;
                        outputStatus = cmd.Parameters.Add("@Status", SqlDbType.SmallInt);
                        outputStatus.Direction = ParameterDirection.Output;
                        outputID = cmd.Parameters.Add("@ID1", SqlDbType.UniqueIdentifier);
                        outputID.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();
                    }
                }
                switch (outputStatus.Value.ToString())
                {
                    case "0":
                        throw new Exception(_appConst.UpdateFailure);

                    case "1":
                        return new
                        {
                            ID = outputID.Value.ToString(),
                            Status = outputStatus.Value.ToString(),
                            Message = _appConst.UpdateSuccess
                        };
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
                ID = outputID.Value.ToString(),
                Status = outputStatus.Value.ToString(),
                Message = _appConst.InsertSuccess
            };
        }
        #endregion UpdatePOMailDetails

        #region UpdatePurchaseOrderDetailLink
        public object UpdatePurchaseOrderDetailLink(PurchaseOrder purchaseOrder)
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
                        cmd.CommandText = "[AMC].[UpdatePurchaseOrderDetailLink]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@POID", SqlDbType.UniqueIdentifier).Value = purchaseOrder.ID;
                        cmd.Parameters.Add("@reqDetailLinkObjXML", SqlDbType.NVarChar, -1).Value = purchaseOrder.PODDetailLinkXML;
                        cmd.Parameters.Add("@PODDetailXML", SqlDbType.NVarChar, -1).Value = purchaseOrder.PODDetailXML;
                        cmd.Parameters.Add("@UpdatedBy", SqlDbType.NVarChar, 250).Value = purchaseOrder.Common.UpdatedBy;
                        cmd.Parameters.Add("@UpdatedDate", SqlDbType.DateTime).Value = purchaseOrder.Common.UpdatedDate;
                        outputStatus = cmd.Parameters.Add("@Status", SqlDbType.SmallInt);
                        outputStatus.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();
                    }
                }
                //AppConst Cobj = new AppConst();
                switch (outputStatus.Value.ToString())
                {
                    case "0":
                        throw new Exception(_appConst.UpdateFailure);

                    case "1":
                        return new
                        {
                            Status = outputStatus.Value.ToString(),
                            Message = _appConst.UpdateSuccess
                        };
                    case "2":
                        //Exceed the limit
                        return new
                        {
                            Status = outputStatus.Value.ToString(),
                            Message = _appConst.UpdateFailure
                        };
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
                Message = _appConst.UpdateSuccess
            };
        }
        #endregion UpdatePurchaseOrderDetailLink

        #region UpdatePurchaseOrderMailStatus
        public object UpdatePurchaseOrderMailStatus(PurchaseOrder purchaseOrder)
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
                        cmd.CommandText = "[AMC].[UpdatePurchaseOrderMailStatus]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = purchaseOrder.ID;
                        cmd.Parameters.Add("@SentToEmails", SqlDbType.NVarChar, -1).Value = purchaseOrder.PurchaseOrderMailPreview.SentToEmails;
                        cmd.Parameters.Add("@EmailSentYN", SqlDbType.Bit).Value = purchaseOrder.EmailSentYN;
                        cmd.Parameters.Add("@UpdatedBy", SqlDbType.NVarChar, 250).Value = purchaseOrder.Common.UpdatedBy;
                        cmd.Parameters.Add("@UpdatedDate", SqlDbType.DateTime).Value = purchaseOrder.Common.UpdatedDate;
                        outputStatus = cmd.Parameters.Add("@Status", SqlDbType.SmallInt);
                        outputStatus.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();
                    }
                }
                AppConst Cobj = new AppConst();
                switch (outputStatus.Value.ToString())
                {
                    case "0":

                        throw new Exception(Cobj.UpdateFailure);

                    case "1":

                        return new
                        {
                            Status = outputStatus.Value.ToString(),
                            Message = Cobj.UpdateSuccess
                        };
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
            };
        }
        #endregion UpdatePurchaseOrderMailStatus

        #region GetPurchaseOrderByID
        public PurchaseOrder GetPurchaseOrder(Guid ID)
        {
            PurchaseOrder purchaseOrder = new PurchaseOrder();
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
                        cmd.CommandText = "[AMC].[GetPurchaseOrder]";
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = ID;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                while (sdr.Read())
                                {
                                    purchaseOrder.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : purchaseOrder.ID);
                                    purchaseOrder.PurchaseOrderNo = (sdr["PurchaseOrderNo"].ToString() != "" ? sdr["PurchaseOrderNo"].ToString() : purchaseOrder.PurchaseOrderNo);
                                    purchaseOrder.Supplier = (sdr["Supplier"].ToString() != "" ? sdr["Supplier"].ToString() : purchaseOrder.Supplier);
                                    purchaseOrder.PurchaseOrderDate = (sdr["PurchaseOrderDate"].ToString() != "" ? DateTime.Parse(sdr["PurchaseOrderDate"].ToString()) : purchaseOrder.PurchaseOrderDate);
                                    purchaseOrder.PurchaseOrderDateFormatted = (sdr["PurchaseOrderDate"].ToString() != "" ? DateTime.Parse(sdr["PurchaseOrderDate"].ToString()).ToString(settings.DateFormat) : purchaseOrder.PurchaseOrderDateFormatted);
                                    purchaseOrder.PurchaseOrderIssuedDate = (sdr["PurchaseOrderIssuedDate"].ToString() != "" ? DateTime.Parse(sdr["PurchaseOrderIssuedDate"].ToString()) : purchaseOrder.PurchaseOrderIssuedDate);
                                    purchaseOrder.PurchaseOrderIssuedDateFormatted = (sdr["PurchaseOrderIssuedDate"].ToString() != "" ? DateTime.Parse(sdr["PurchaseOrderIssuedDate"].ToString()).ToString(settings.DateFormat) : purchaseOrder.PurchaseOrderIssuedDateFormatted);
                                    purchaseOrder.SupplierID = (sdr["SupplierID"].ToString() != "" ? Guid.Parse(sdr["SupplierID"].ToString()) : purchaseOrder.SupplierID);
                                    purchaseOrder.PurchaseOrderStatus = (sdr["PurchaseOrderStatus"].ToString() != "" ? sdr["PurchaseOrderStatus"].ToString() : purchaseOrder.PurchaseOrderStatus);
                                    purchaseOrder.PurchaseOrderTitle = (sdr["PurchaseOrderTitle"].ToString() != "" ? sdr["PurchaseOrderTitle"].ToString() : purchaseOrder.PurchaseOrderTitle);
                                    purchaseOrder.MailingAddress = (sdr["MailingAddress"].ToString() != "" ? sdr["MailingAddress"].ToString() : purchaseOrder.MailingAddress);
                                    purchaseOrder.ShippingAddress = (sdr["ShippingAddress"].ToString() != "" ? sdr["ShippingAddress"].ToString() : purchaseOrder.ShippingAddress);
                                    purchaseOrder.GeneralNotes = (sdr["GeneralNotes"].ToString() != "" ? sdr["GeneralNotes"].ToString() : purchaseOrder.GeneralNotes);
                                    purchaseOrder.MailBodyHeader = (sdr["MailBodyHeader"].ToString() != "" ? sdr["MailBodyHeader"].ToString() : purchaseOrder.MailBodyHeader);
                                    purchaseOrder.MailBodyFooter = (sdr["MailBodyFooter"].ToString() != "" ? sdr["MailBodyFooter"].ToString() : purchaseOrder.MailBodyFooter);
                                    purchaseOrder.Discount = (sdr["Discount"].ToString() != "" ? decimal.Parse(sdr["Discount"].ToString()) : purchaseOrder.Discount);
                                    purchaseOrder.EmailSentYN= (sdr["EmailSentYN"].ToString() != "" ? sdr["EmailSentYN"].ToString() : purchaseOrder.EmailSentYN);
                                    purchaseOrder.SubscriberEmail= (sdr["SubscriberEmail"].ToString() != "" ? sdr["SubscriberEmail"].ToString() : purchaseOrder.SubscriberEmail);
                                    purchaseOrder.ApprovalStatus= (sdr["ApprovalStatus"].ToString() != "" ? sdr["ApprovalStatus"].ToString() : purchaseOrder.ApprovalStatus);
                                    purchaseOrder.LatestApprovalStatus= (sdr["LatestApprovalStatus"].ToString() != "" ? Int16.Parse(sdr["LatestApprovalStatus"].ToString()) : purchaseOrder.LatestApprovalStatus);
                                    purchaseOrder.LatestApprovalID = (sdr["LatestApprovalID"].ToString() != "" ? Guid.Parse(sdr["LatestApprovalID"].ToString()) : purchaseOrder.LatestApprovalID);
                                    purchaseOrder.PurchaseOrderMailPreview = new PurchaseOrderMailPreview();
                                    purchaseOrder.PurchaseOrderMailPreview.SentToEmails= (sdr["SubscriberEmail"].ToString() != "" ? sdr["SubscriberEmail"].ToString() : purchaseOrder.SubscriberEmail);
                                    purchaseOrder.CompanyAddress = (sdr["EmailFrom"].ToString() != "" ? sdr["EmailFrom"].ToString() : purchaseOrder.CompanyAddress);
                                    purchaseOrder.CompanyName = (sdr["Company"].ToString() != "" ? sdr["Company"].ToString() : purchaseOrder.CompanyName);
                                    purchaseOrder.LogoURL = (sdr["Logo"].ToString() != "" ? sdr["Logo"].ToString() : purchaseOrder.LogoURL);
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

            return purchaseOrder;
        }
        #endregion GetPurchaseOrderByID

        #region GetPurchaseOrderDetailsBYID
        public List<PurchaseOrderDetail> GetPurchaseOrderDetails(Guid ID)
        {
            List<PurchaseOrderDetail> PODList = null;
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
                        cmd.CommandText = "[AMC].[GetPurchaseOrderDetails]";
                        cmd.Parameters.Add("@POID", SqlDbType.UniqueIdentifier).Value = ID;
                        cmd.CommandType = CommandType.StoredProcedure;

                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                PODList = new List<PurchaseOrderDetail>();
                                while (sdr.Read())
                                {
                                    PurchaseOrderDetail purchaseOrderDetail = new PurchaseOrderDetail();
                                    {

                                        purchaseOrderDetail.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : purchaseOrderDetail.ID);
                                        purchaseOrderDetail.MaterialID = (sdr["MaterialID"].ToString() != "" ? Guid.Parse(sdr["MaterialID"].ToString()) : purchaseOrderDetail.MaterialID);
                                        purchaseOrderDetail.MaterialDesc = (sdr["MaterialDesc"].ToString() != "" ? sdr["MaterialDesc"].ToString() : purchaseOrderDetail.MaterialDesc);
                                        purchaseOrderDetail.MaterialCode = (sdr["MaterialCode"].ToString() != "" ? sdr["MaterialCode"].ToString() : purchaseOrderDetail.MaterialCode);
                                        purchaseOrderDetail.MaterialTypeDesc = (sdr["MaterialTypeDesc"].ToString() != "" ? sdr["MaterialTypeDesc"].ToString() : purchaseOrderDetail.MaterialTypeDesc);
                                        purchaseOrderDetail.UnitCode = (sdr["UnitCode"].ToString() != "" ? sdr["UnitCode"].ToString() : purchaseOrderDetail.UnitCode);
                                        purchaseOrderDetail.POQty = (sdr["Qty"].ToString() != "" ? decimal.Parse(sdr["Qty"].ToString()) : purchaseOrderDetail.POQty);
                                        purchaseOrderDetail.Rate = (sdr["Rate"].ToString() != "" ? decimal.Parse(sdr["Rate"].ToString()) : purchaseOrderDetail.Rate);
                                        purchaseOrderDetail.CGSTAmt = (sdr["CGSTAmt"].ToString() != "" ? decimal.Parse(sdr["CGSTAmt"].ToString()) : purchaseOrderDetail.CGSTAmt);
                                        purchaseOrderDetail.SGSTAmt = (sdr["SGSTAmt"].ToString() != "" ? decimal.Parse(sdr["SGSTAmt"].ToString()) : purchaseOrderDetail.SGSTAmt);
                                        purchaseOrderDetail.IGSTAmt = (sdr["IGSTAmt"].ToString() != "" ? decimal.Parse(sdr["IGSTAmt"].ToString()) : purchaseOrderDetail.IGSTAmt);
                                        purchaseOrderDetail.CGSTPerc= (sdr["CGSTPerc"].ToString() != "" ? decimal.Parse(sdr["CGSTPerc"].ToString()) : purchaseOrderDetail.CGSTPerc);
                                        purchaseOrderDetail.SGSTPerc = (sdr["SGSTPerc"].ToString() != "" ? decimal.Parse(sdr["SGSTPerc"].ToString()) : purchaseOrderDetail.SGSTPerc);
                                        purchaseOrderDetail.IGSTPerc = (sdr["IGSTPerc"].ToString() != "" ? decimal.Parse(sdr["IGSTPerc"].ToString()) : purchaseOrderDetail.IGSTPerc);
                                        purchaseOrderDetail.TaxTypeCode= (sdr["TaxTypeCode"].ToString() != "" ? sdr["TaxTypeCode"].ToString() : purchaseOrderDetail.TaxTypeCode);
                                        purchaseOrderDetail.Discount = (sdr["Discount"].ToString() != "" ? decimal.Parse(sdr["Discount"].ToString()) : purchaseOrderDetail.Discount);
                                        purchaseOrderDetail.Amount = (sdr["Amount"].ToString() != "" ? decimal.Parse(sdr["Amount"].ToString()) : purchaseOrderDetail.Amount);
                                        purchaseOrderDetail.PrevRcvQty = (sdr["PrevRcvQty"].ToString() != "" ? decimal.Parse(sdr["PrevRcvQty"].ToString()) : purchaseOrderDetail.PrevRcvQty);
                                        purchaseOrderDetail.PrevRcvQtyInKG = (sdr["PrevRcvQtyInKG"].ToString() != "" ? decimal.Parse(sdr["PrevRcvQtyInKG"].ToString()) : purchaseOrderDetail.PrevRcvQtyInKG);
                                        purchaseOrderDetail.PrevInvQty = (sdr["PrevInvQty"].ToString() != "" ? decimal.Parse(sdr["PrevInvQty"].ToString()) : purchaseOrderDetail.PrevInvQty);
                                        //calculation
                                        //To solve TaxableAmt calculation in PO
                                        purchaseOrderDetail.Qty = purchaseOrderDetail.POQty;// - purchaseOrderDetail.PrevInvQty > 0 ? purchaseOrderDetail.POQty - purchaseOrderDetail.PrevInvQty : 0;
                                    }
                                    PODList.Add(purchaseOrderDetail);
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

            return PODList;

        }
        #endregion GetPurchaseOrderDetailsBYID

        #region PurchaseOrderDetailForEdit
        public List<PurchaseOrderDetail> GetPurchaseOrderDetailByPODetailID(Guid ID)
        {
            List<PurchaseOrderDetail> PODList = null;
           
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
                        cmd.CommandText = "[AMC].[GetPurchaseOrderDetailByPODetailID]";
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = ID;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                PODList = new List<PurchaseOrderDetail>();
                                while (sdr.Read())
                                {
                                    PurchaseOrderDetail purchaseOrderDetail = new PurchaseOrderDetail();
                                    {
                                        purchaseOrderDetail.PurchaseOrderID = (sdr["PurchaseOrderID"].ToString() != "" ? Guid.Parse(sdr["PurchaseOrderID"].ToString()) : purchaseOrderDetail.PurchaseOrderID);
                                        purchaseOrderDetail.MaterialCode = (sdr["MaterialCode"].ToString() != "" ? sdr["MaterialCode"].ToString() : purchaseOrderDetail.MaterialCode);
                                        purchaseOrderDetail.Qty = (sdr["PurchaseOrderQty"].ToString() != "" ? decimal.Parse(sdr["PurchaseOrderQty"].ToString()) : purchaseOrderDetail.Qty);
                                        purchaseOrderDetail.Discount = (sdr["Discount"].ToString() != "" ? decimal.Parse(sdr["Discount"].ToString()) : purchaseOrderDetail.Discount);
                                        purchaseOrderDetail.Rate = (sdr["Rate"].ToString() != "" ? decimal.Parse(sdr["Rate"].ToString()) : purchaseOrderDetail.Rate);
                                        purchaseOrderDetail.TaxTypeCode = (sdr["TaxTypeCode"].ToString() != "" ? sdr["TaxTypeCode"].ToString() : purchaseOrderDetail.TaxTypeCode);
                                        purchaseOrderDetail.UnitCode = (sdr["UnitCode"].ToString() != "" ? sdr["UnitCode"].ToString() : purchaseOrderDetail.UnitCode);
                                        purchaseOrderDetail.MaterialID = (sdr["MaterialID"].ToString() != "" ? Guid.Parse(sdr["MaterialID"].ToString()) : purchaseOrderDetail.MaterialID);
                                        purchaseOrderDetail.MaterialDesc = (sdr["MaterialDesc"].ToString() != "" ? sdr["MaterialDesc"].ToString() : purchaseOrderDetail.MaterialDesc);
                                        purchaseOrderDetail.RequisitionDetail = new RequisitionDetail();
                                        purchaseOrderDetail.RequisitionDetail.ID= (sdr["LinkID"].ToString() != "" ? Guid.Parse(sdr["LinkID"].ToString()) : purchaseOrderDetail.RequisitionDetail.ID);
                                        purchaseOrderDetail.RequisitionDetail.ReqID = (sdr["ReqID"].ToString() != "" ? Guid.Parse(sdr["ReqID"].ToString()) : purchaseOrderDetail.RequisitionDetail.ReqID);
                                        purchaseOrderDetail.RequisitionDetail.ReqNo = (sdr["ReqNo"].ToString() != "" ? sdr["ReqNo"].ToString() : purchaseOrderDetail.RequisitionDetail.ReqNo);
                                        purchaseOrderDetail.RequisitionDetail.RequestedQty = (sdr["RequestedQty"].ToString() != "" ? sdr["RequestedQty"].ToString() : purchaseOrderDetail.RequisitionDetail.RequestedQty);
                                        purchaseOrderDetail.RequisitionDetail.OrderedQty = (sdr["OrderedQty"].ToString() != "" ? sdr["OrderedQty"].ToString() : purchaseOrderDetail.RequisitionDetail.OrderedQty);
                                    }
                                    PODList.Add(purchaseOrderDetail);
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

            return PODList;
        }
        #endregion PurchaseOrderDetailForEdit

        #region DeletePurchaseOrder
        public object DeletePurchaseOrder(Guid ID)
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
                        cmd.CommandText = "[AMC].[DeletePurchaseOrder]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = ID;
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
        #endregion DeletePurchaseOrder

        #region DeletePurchaseOrderDetail
        public object DeletePurchaseOrderDetail(Guid ID)
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
                        cmd.CommandText = "[AMC].[DeletePurchaseOrderDetail]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = ID;
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
        #endregion DeletePurchaseOrderDetail

        #region RecentPurchaseOrder
        public List<PurchaseOrder> RecentPurchaseOrder()
        {
            List<PurchaseOrder> purchaseOrderList = new List<PurchaseOrder>();
            PurchaseOrder purchaseOrder = null;
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
                        cmd.CommandText = "[AMC].[GetPurchaseOrderSummary]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                while (sdr.Read())
                                {
                                    purchaseOrder = new PurchaseOrder();
                                    purchaseOrder.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : Guid.Empty);
                                    purchaseOrder.PurchaseOrderNo = (sdr["PurchaseOrderNo"].ToString() != "" ? sdr["PurchaseOrderNo"].ToString() : purchaseOrder.PurchaseOrderNo);
                                    purchaseOrder.PurchaseOrderDateFormatted = (sdr["PurchaseOrderDate"].ToString() != "" ? DateTime.Parse(sdr["PurchaseOrderDate"].ToString()).ToString(settings.DateFormat) : purchaseOrder.PurchaseOrderDateFormatted);
                                    purchaseOrder.PurchaseOrderStatus = (sdr["PurchaseOrderStatus"].ToString() != "" ? sdr["PurchaseOrderStatus"].ToString() : purchaseOrder.PurchaseOrderStatus);
                                    purchaseOrder.PurchaseOrderIssuedDateFormatted = (sdr["PurchaseOrderIssuedDate"].ToString() != "" ? DateTime.Parse(sdr["PurchaseOrderDate"].ToString()).ToString(settings.DateFormat) : purchaseOrder.PurchaseOrderIssuedDateFormatted);
                                    purchaseOrder.ApprovalStatus = (sdr["ApprovalStatus"].ToString() != "" ? sdr["ApprovalStatus"].ToString() : purchaseOrder.ApprovalStatus);
                                    purchaseOrderList.Add(purchaseOrder);
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
            return purchaseOrderList;
        }

        #endregion RecentPurchaseOrder
             
    }
}
