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
    public class DocumentTypeRepository: IDocumentTypeRepository
    {
        private IDatabaseFactory _databaseFactory;
        AppConst _appConst = new AppConst();

        #region Constructor Injection
        /// <summary>
        /// Constructor Injection:-Getting IDatabaseFactory implementing object
        /// </summary>
        /// <param name="databaseFactory"></param>
        public DocumentTypeRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }
        #endregion Constructor Injection

        #region GetDocumentTypeForSelectList
        /// <summary>
        /// To Get List of All DocumentType for Select List
        /// </summary>
        /// <returns>List</returns>
        public List<DocumentType> GetDocumentTypeForSelectList()
        {
            List<DocumentType> documentTypeList = null;
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
                        cmd.CommandText = "[AMC].[GetDocumentTypeForSelectList]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                documentTypeList = new List<DocumentType>();
                                while (sdr.Read())
                                {
                                    DocumentType documentType = new DocumentType();
                                    {
                                        documentType.Code = (sdr["Code"].ToString() != "" ? sdr["Code"].ToString() : documentType.Code);
                                        documentType.Description = (sdr["Description"].ToString() != "" ? sdr["Description"].ToString() : documentType.Description);
                                    }
                                    documentTypeList.Add(documentType);
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
            return documentTypeList;
        }
        #endregion GetDocumentTypeForSelectList
    }
}
