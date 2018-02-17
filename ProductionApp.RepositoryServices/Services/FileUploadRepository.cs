using ProductionApp.DataAccessObject.DTO;
using ProductionApp.RepositoryServices.Contracts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ProductionApp.RepositoryServices.Services
{
    public class FileUploadRepository:IFileUploadRepository
    {
        private IDatabaseFactory _databaseFactory;
        public FileUploadRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }
        public FileUpload InsertAttachment(FileUpload fileUploadObj)
        {
            try
            {
                SqlParameter outputStatus, outputParentID, outputID = null;
                using (SqlConnection con = _databaseFactory.GetDBConnection())
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        if (con.State == ConnectionState.Closed)
                        {
                            con.Open();
                        }
                        cmd.Connection = con;
                        cmd.CommandText = "[AMC].[InsertAttachment]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ParentID", SqlDbType.UniqueIdentifier).Value = fileUploadObj.ParentID;
                        cmd.Parameters.Add("@ParentType", SqlDbType.VarChar, 20).Value = fileUploadObj.ParentType;
                        cmd.Parameters.Add("@FileName", SqlDbType.NVarChar, 255).Value = fileUploadObj.FileName;
                        cmd.Parameters.Add("@FileType", SqlDbType.VarChar, 5).Value = fileUploadObj.FileType;
                        cmd.Parameters.Add("@FileSize", SqlDbType.VarChar, 50).Value = fileUploadObj.FileSize;
                        cmd.Parameters.Add("@AttachmentURL", SqlDbType.NVarChar, -1).Value = fileUploadObj.AttachmentURL;
                        cmd.Parameters.Add("@CreatedBy", SqlDbType.NVarChar, 250).Value = fileUploadObj.CommonObj.CreatedBy;
                        cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = fileUploadObj.CommonObj.CreatedDate;
                        cmd.Parameters.Add("@UpdatedBy", SqlDbType.NVarChar, 250).Value = fileUploadObj.CommonObj.UpdatedBy;
                        cmd.Parameters.Add("@UpdatedDate", SqlDbType.DateTime).Value = fileUploadObj.CommonObj.UpdatedDate;
                        outputStatus = cmd.Parameters.Add("@Status", SqlDbType.SmallInt);
                        outputStatus.Direction = ParameterDirection.Output;
                        outputID = cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier);
                        outputID.Direction = ParameterDirection.Output;
                        outputParentID = cmd.Parameters.Add("@DemoID", SqlDbType.UniqueIdentifier);
                        outputParentID.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();


                    }
                }

                switch (outputStatus.Value.ToString())
                {
                    case "0":
                        AppConst Cobj = new AppConst();
                        throw new Exception(Cobj.InsertFailure);
                    case "1":
                        fileUploadObj.ID = Guid.Parse(outputID.Value.ToString());
                        fileUploadObj.ParentID = Guid.Parse(outputParentID.Value.ToString());
                        break;
                    case "2":
                        AppConst Cobj1 = new AppConst();
                        throw new Exception(Cobj1.Duplicate);
                    default:
                        break;
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return fileUploadObj;
        }
        public List<FileUpload> GetAttachments(Guid ID)
        {
            List<FileUpload> AttachmentList = null;
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
                        cmd.CommandText = "[AMC].[GetAttachments]";
                        cmd.Parameters.Add("@ParentID", SqlDbType.UniqueIdentifier).Value = ID;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                AttachmentList = new List<FileUpload>();
                                while (sdr.Read())
                                {
                                    FileUpload FileObj = new FileUpload();
                                    {
                                        FileObj.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : FileObj.ID);
                                        FileObj.ParentID = (sdr["ParentID"].ToString() != "" ? Guid.Parse(sdr["ParentID"].ToString()) : FileObj.ParentID);
                                        FileObj.ParentType = (sdr["ParentType"].ToString());
                                        FileObj.FileName = sdr["FileName"].ToString();
                                        FileObj.FileType = sdr["FileType"].ToString();
                                        FileObj.FileSize = sdr["FileSize"].ToString();
                                        FileObj.AttachmentURL = sdr["AttachmentURL"].ToString();
                                    }
                                    AttachmentList.Add(FileObj);
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

            return AttachmentList;
        }
        public object DeleteFile(Guid ID)
        {
            AppConst Cobj = new AppConst();
            try
            {
                SqlParameter outputStatus = null;
                SqlParameter OutparameterURL = null;
                using (SqlConnection con = _databaseFactory.GetDBConnection())
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        if (con.State == ConnectionState.Closed)
                        {
                            con.Open();
                        }
                        cmd.Connection = con;
                        cmd.CommandText = "[AMC].[DeleteFile]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = ID;
                        outputStatus = cmd.Parameters.Add("@Status", SqlDbType.SmallInt);
                        outputStatus.Direction = ParameterDirection.Output;
                        OutparameterURL = cmd.Parameters.Add("@AttacmentURL", SqlDbType.NVarChar, -1);
                        OutparameterURL.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();
                    }
                }

                switch (outputStatus.Value.ToString())
                {
                    case "1":
                        if (OutparameterURL.Value.ToString() != "")
                        {
                            System.IO.File.Delete(HttpContext.Current.Server.MapPath(OutparameterURL.Value.ToString()));
                        }
                        break;
                    case "0":
                        throw new Exception(Cobj.InsertFailure);
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return new { Message = Cobj.DeleteSuccess };
        }
    }
}
