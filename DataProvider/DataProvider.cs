using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Data.SqlClient;

namespace DataProvider
{
    public class DataProvider : IDataProvider, IDisposable
    {
        private const int idLength = 10;

        #region Connection string parameters

        private const string dataSource = "Data Source=.\\SQLEXPRESS;";
        private const string initialCatalog = "Initial Catalog = NodesBD;";
        private const string integratedSecurity = "Integrated Security=true;";
        private const string pooling = "Pooling=false";

        #endregion

        private readonly SqlConnection connection;

        public DataProvider()
        {
            string connectionString = dataSource + initialCatalog + integratedSecurity + pooling;
            connection = new SqlConnection(connectionString);
        }
        
        public IDictionary<string, string> GetLabels()
        {
            var result = new Dictionary<string, string>();

            try
            {
                SqlDataReader dataReader;
                
                connection.Open();

                string selectAllLabelsQuery = "SELECT IdenticalOfNode, Label FROM Labels";

                using (var command = new SqlCommand(selectAllLabelsQuery, connection))
                {
                    command.CommandType = System.Data.CommandType.Text;
                    dataReader = command.ExecuteReader();
                }

                while (dataReader.Read())
                    result[dataReader["IdenticalOfNode"].ToString()] = dataReader["Label"].ToString();
            }
            finally
            {
                connection.Close();
            }

            return result;
        }

        public IList<string> GetAdjancentNodes(string nodeId)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            connection.Dispose();
        }
    }
}
