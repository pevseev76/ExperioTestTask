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
        private string connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog = NodesBD;Integrated Security=true; Pooling=false";
        private SqlConnection connection;

        [TestInitialize]
        void Initialize()
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
                if (!actualLabels.Keys.Contains(key))
                    Assert.Fail();

                Assert.AreEqual(labels[key], actualLabels[key]);
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
