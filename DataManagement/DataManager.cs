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
        private const string connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog = NodesBD;Integrated Security=true; Pooling=false";
        private readonly SqlConnection connection;

        public DataManager()
        {
            connection = new SqlConnection(connectionString);
        }

        public void LoadLabels(IDictionary<string, string> labels)
        {
            try
            {
                connection.Open();

                string insertLabelQuery = "INSERT INTO Labels (IdenticalOfNode, Label) VALUES (@id, @label)";

                using (var command = new SqlCommand(insertLabelQuery, connection))
                {
                    foreach (var key in labels.Keys)
                    {
                        InsertLabel(key, labels[key]);
                    }
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
