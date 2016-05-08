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
        void LoadLabelsTest()
        {
            var labels = new Dictionary<string, string>();

            labels["first"] = "January";
            labels["second"] = "February";
            labels["third"] = "Marth";
            labels["fourth"] = "April";

            var manager = new DataManager();
            manager.LoadLabels(labels);
        }

        private void Clear()
        {
            try
            {
                connection.Open();

                string deleteAllQuery = "DELETE FROM Labels";

                using (var command = new SqlCommand(deleteAllQuery))
                {
                    command.Connection = connection;
                    command.CommandType = System.Data.CommandType.Text;
                    command.ExecuteNonQuery();
                }
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
