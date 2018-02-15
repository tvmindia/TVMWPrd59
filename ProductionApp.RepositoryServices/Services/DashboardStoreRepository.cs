﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductionApp.RepositoryServices.Contracts;
using ProductionApp.DataAccessObject.DTO;
using System.Data.SqlClient;
using System.Data;

namespace ProductionApp.RepositoryServices.Services
{
    public class DashboardStoreRepository: IDashboardStoreRepository
    {
        Settings settings = new Settings();
        private IDatabaseFactory _databaseFactory;

        public DashboardStoreRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }

        #region GetRecentIssueSummary
        public List<MaterialIssue> GetRecentIssueSummary()
        {
            List<MaterialIssue> materialIssueList = new List<MaterialIssue>();
            MaterialIssue materialIssue = null;
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
                        cmd.CommandText = "[AMC].[GetRecentIssueSummary]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                while (sdr.Read())
                                {
                                    materialIssue = new MaterialIssue();
                                    materialIssue.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : Guid.Empty);
                                    materialIssue.IssueNo= (sdr["IssueNo"].ToString() != "" ? sdr["IssueNo"].ToString() : materialIssue.IssueNo);
                                    materialIssue.IssueTo= (sdr["IssueTo"].ToString() != "" ? Guid.Parse(sdr["IssueTo"].ToString()) : materialIssue.IssueTo);
                                    materialIssue.IssuedBy= (sdr["IssuedBy"].ToString() != "" ? Guid.Parse(sdr["IssuedBy"].ToString()) : materialIssue.IssuedBy);
                                    materialIssue.IssueDate = (sdr["IssueDate"].ToString() != "" ? DateTime.Parse(sdr["IssueDate"].ToString()).ToString(settings.DateFormat) : materialIssue.IssueDate);
                                    materialIssue.IssuedByEmployee = new Employee();
                                    materialIssue.IssueToEmployee = new Employee();
                                    materialIssue.IssuedByEmployee.Name= (sdr["IssuedByName"].ToString() != "" ? sdr["IssuedByName"].ToString() : materialIssue.IssuedByEmployee.Name);
                                    materialIssue.IssueToEmployee.Name = (sdr["IssueToName"].ToString() != "" ? sdr["IssueToName"].ToString() : materialIssue.IssueToEmployee.Name);
                                    materialIssueList.Add(materialIssue);
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
            return materialIssueList;
        }
        #endregion GetRecentIssueSummary

        #region GetRecentMaterialReceiptSummary
        public List<MaterialReceipt> GetRecentMaterialReceiptSummary()
        {
            List<MaterialReceipt> materialReceiptList = new List<MaterialReceipt>();
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
                        cmd.CommandText = "[AMC].[GetRecentMaterialReceiptSummary]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                while (sdr.Read())
                                {
                                    materialReceipt = new MaterialReceipt();
                                    materialReceipt.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : Guid.Empty);
                                    materialReceipt.MRNDate = (sdr["MRNDate"].ToString() != "" ? DateTime.Parse(sdr["MRNDate"].ToString()).ToString(settings.DateFormat) : materialReceipt.MRNDate);
                                    materialReceipt.MRNNo = (sdr["MRNNo"].ToString() != "" ? sdr["MRNNo"].ToString() : materialReceipt.MRNNo);
                                    materialReceipt.Supplier = new Supplier();
                                    materialReceipt.PurchaseOrder = new PurchaseOrder();
                                    materialReceipt.Supplier.CompanyName= (sdr["CompanyName"].ToString() != "" ? sdr["CompanyName"].ToString() : materialReceipt.Supplier.CompanyName);
                                    materialReceipt.PurchaseOrder.PurchaseOrderTitle= (sdr["Title"].ToString() != "" ? sdr["Title"].ToString() : materialReceipt.PurchaseOrder.PurchaseOrderTitle);
                                    materialReceiptList.Add(materialReceipt);
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
            return materialReceiptList;
        }
        #endregion GetRecentMaterialReceiptSummary
    }
}
