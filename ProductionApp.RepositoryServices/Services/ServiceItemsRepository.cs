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
    public class ServiceItemsRepository: IServiceItemsRepository
    {

        private IDatabaseFactory _databaseFactory;
        Settings settings = new Settings();
        AppConst _appConst = new AppConst();

        /// <summary>
        /// Constructor Injection:-Getting IDatabaseFactory implementing object
        /// </summary>
        /// <param name="databaseFactory"></param>
        public ServiceItemsRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }

        public List<ServiceItems> GetServiceItemsForSelectList()
        {

            List<ServiceItems> productList = null;
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
                        cmd.CommandText = "[AMC].[GetAllServiceItems]"; 
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                productList = new List<ServiceItems>();
                                while (sdr.Read())
                                {
                                    ServiceItems product = new ServiceItems();
                                    {
                                        product.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : product.ID);
                                        product.ServiceName = (sdr["ServiceName"].ToString() != "" ? sdr["ServiceName"].ToString() : product.ServiceName);
                                        product.Rate = (sdr["Rate"].ToString() != "" ? decimal.Parse(sdr["Rate"].ToString()) : product.Rate);
                                    }
                                    productList.Add(product);
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
            return productList;

        }

        public ServiceItems GetServiceItem(Guid id)
        {
            ServiceItems serviceItems = new ServiceItems();
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
                        cmd.CommandText = "[AMC].[GetServiceItem]";
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = id;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                if (sdr.Read())
                                {
                                    serviceItems.ServiceName = (sdr["ServiceName"].ToString() != "" ? sdr["ServiceName"].ToString() : serviceItems.ServiceName);
                                    serviceItems.Rate = (sdr["Rate"].ToString() != "" ? decimal.Parse(sdr["Rate"].ToString()) : serviceItems.Rate);
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
            return serviceItems;
        }
    }
}
