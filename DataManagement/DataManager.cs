using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Data.SqlClient;

namespace DataManagement
{
    public class DataManager : IDataManager, IDisposable
    {
        #region Connection string parameters

        private const string dataSource = "Data Source=.\\SQLEXPRESS;";
        private const string initialCatalog = "Initial Catalog = NodesBD;";
        private const string integratedSecurity = "Integrated Security=true;";
        private const string pooling = "Pooling=false";
        
        #endregion

        private readonly SqlConnection connection;

        public DataManager()
        {
            string connectionString = dataSource + initialCatalog + integratedSecurity + pooling;
            connection = new SqlConnection(connectionString);
        }

        public void LoadLabels(IDictionary<string, string> labels)
        {
            try
            {
                connection.Open();

                foreach (var key in labels.Keys)
                {
                    InsertLabel(key, labels[key]);
                }

            }
            finally
            {
                connection.Close();
            }
            
        }

        private void InsertLabel(string id, string label)
        {
            string insertLabelQuery = "INSERT INTO Labels (IdenticalOfNode, Label) VALUES (@id, @label)";

            using (var command = new SqlCommand(insertLabelQuery, connection))
            {
                command.CommandType = System.Data.CommandType.Text;
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@label", label);
                command.ExecuteNonQuery();
            }
        }

        public void LoadAdjancentNodes(string nodeId, IList<string> adjancentNodes)
        {
        
        }

        public void Dispose()
        {
            connection.Dispose();
        }
    }
}
