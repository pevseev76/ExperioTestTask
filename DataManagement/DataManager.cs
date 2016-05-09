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
        private const int idLength = 10;
        
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
                    InsertLabel(key, labels[key]);
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
            try
            {
                connection.Open();

                for (int i = 0; i < adjancentNodes.Count; i++)
                    InsertAdjancentNode(nodeId, adjancentNodes[i]);
            }
            finally
            {
                connection.Close();
            }
        }

        private void InsertAdjancentNode(string id, string adjancent)
        {
            using (var command = new SqlCommand("StoredProcedure1", connection))
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;

                var nodeId = new SqlParameter("@parameter1", System.Data.SqlDbType.Char, idLength);
                nodeId.Value = id;
                command.Parameters.Add(nodeId);

                var adjancentId = new SqlParameter("@parameter2", System.Data.SqlDbType.Char, idLength);
                adjancentId.Value = adjancent;
                command.Parameters.Add(adjancentId);

                command.ExecuteNonQuery();
            }
        }

        public void Dispose()
        {
            connection.Dispose();
        }
    }
}
