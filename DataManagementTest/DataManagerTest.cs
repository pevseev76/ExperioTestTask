using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Data.SqlClient;
using DataManagement;

namespace DataManagementTest
{
    [TestClass]
    public class DataManagerTest
    {
        private const int idLength = 10;
        private const string connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog = NodesBD;Integrated Security=true;Pooling=false";
        private SqlConnection connection = null;

        [TestInitialize]
        public void Initialize()
        {
            connection = new SqlConnection(connectionString);
        }
        
        [TestMethod]
        public void LoadLabelsTest()
        {
            var labels = new Dictionary<string, string>();

            labels["first"] = "January";
            labels["second"] = "February";
            labels["third"] = "Marth";
            labels["fourth"] = "April";

            Clear();

            var manager = new DataManager();
            manager.LoadLabels(labels);

            var actualLabels = GetAllLabels();

            foreach (var key in labels.Keys)
            {
                var actualKey = key.PadRight(idLength);

                if (!actualLabels.Keys.Contains(actualKey))
                    Assert.Fail();

                Assert.AreEqual(labels[key], actualLabels[actualKey]);
            }
        }

        private void Clear()
        {
            try
            {
                connection.Open();

                string deleteAllQuery = "DELETE FROM Labels";

                using (var command = new SqlCommand(deleteAllQuery,connection))
                {
                    command.CommandType = System.Data.CommandType.Text;
                    command.ExecuteNonQuery();
                }
            }
            finally
            {
                connection.Close();
            }
        }

        private IDictionary<string, string> GetAllLabels()
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
                                
                return result;
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
