using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Data.SqlClient;
using DataProvider;

namespace DataProviderTest
{
    [TestClass]
    public class DataProviderTest
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
        public void GeLabelsTest()
        {
            var labels = new Dictionary<string, string>();

            labels["first"] = "January";
            labels["second"] = "February";
            labels["third"] = "Marth";
            labels["fourth"] = "April";
            labels["fifth"] = "May";
            labels["sixth"] = "June";
            labels["seventh"] = "July";
            labels["eighth"] = "August";
            labels["nineth"] = "September";
            labels["tenth"] = "October";
            labels["eleventh"] = "November";
            labels["twelveth"] = "Desember";

            try
            {
                connection.Open();

                Clear();
                
                foreach (var key in labels.Keys)
                    InsertLabel(key, labels[key]);

                var provider = new DataProvider.DataProvider();
                var actualLabels = provider.GetLabels();

                foreach (var key in labels.Keys)
                {
                    string actualKey = key.PadRight(idLength);

                    if (!actualLabels.Keys.Contains(actualKey))
                        Assert.Fail();

                    Assert.AreEqual(labels[key], actualLabels[actualKey]);
                }

                var adjacentNodes = new Dictionary<string, List<string>>();

                adjacentNodes["first"] = (new string[] { "second", "third", "fourth" }).ToList();
                adjacentNodes["second"] = (new string[] { "first", "third", "fourth", "sixth" }).ToList();
                adjacentNodes["third"] = (new string[] { "seventh", "third", "first" }).ToList();
                adjacentNodes["fourth"] = (new string[] { "seventh", "third", "fourth" }).ToList();
                adjacentNodes["eleventh"] = (new string[] { "second", "third", "fourth" }).ToList();
                adjacentNodes["seventh"] = (new string[] { "second", "fourth", "sixth", "tenth" }).ToList();

                foreach (var key in adjacentNodes.Keys)
                    for (int i = 0; i < adjacentNodes[key].Count; i++)
                        InsertAdjancentNode(key, adjacentNodes[key][i]);

                var actualAdjacentNodes = provider.GetAdjacentNodes();

                foreach (var key in adjacentNodes.Keys)
                {
                    string actualKey = key.PadRight(idLength);

                    if (!actualAdjacentNodes.Keys.Contains(actualKey))
                        Assert.Fail();

                    for (int i = 0; i < adjacentNodes[key].Count; i++)
                    {
                        string adjancentNode = adjacentNodes[key][i].PadRight(idLength);

                        if (!actualAdjacentNodes[actualKey].Contains(adjancentNode))
                            Assert.Fail();
                    }
                }
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
    }
}
