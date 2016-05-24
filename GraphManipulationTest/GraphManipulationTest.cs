﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.SqlClient;
using System.Collections.Generic;
using GraphManipulation;

namespace GraphManipulatorTest
{
    [TestClass]
    public class GraphManipulationTest
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
        public void TestIsolatedPoints()
        {
            var labels = new Dictionary<string, string>();

            labels["first"] = "January";
            labels["second"] = "February";

            try
            {
                connection.Open();

                Clear();

                foreach (var key in labels.Keys)
                    InsertLabel(key, labels[key]);

                var manipulator = new CalculatorShortestPath();
                var path = manipulator.CalculateShortestPath("first", "second");

                Assert.IsNull(path);
            }
            finally
            {
                connection.Close();
            }
        }

        private void Clear()
        {
            string deleteAllQuery = "DELETE FROM Labels";

            using (var command = new SqlCommand(deleteAllQuery, connection))
            {
                command.CommandType = System.Data.CommandType.Text;
                command.ExecuteNonQuery();
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
    }
}
