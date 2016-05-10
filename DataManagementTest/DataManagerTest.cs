using System;
using System.Linq;
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
            labels["fifth"] = "May";
            labels["sixth"] = "June";
            labels["seventh"] = "July";
            labels["eighth"] = "August";
            labels["nineth"] = "September";
            labels["tenth"] = "October";
            labels["eleventh"] = "November";
            labels["twelveth"] = "Desember";

            var manager = new DataManager();
            manager.LoadLabels(labels);

            var actualLabels = GetAllLabels();

            foreach (var key in labels.Keys)
            {
                string actualKey = key.PadRight(idLength);

                if (!actualLabels.Keys.Contains(actualKey))
                    Assert.Fail();

                Assert.AreEqual(labels[key], actualLabels[actualKey]);
            }

            var adjancentNodes = new Dictionary<string, List<string>>();

            adjancentNodes["first"] = (new string[] {"second", "third", "fourth" }).ToList();
            adjancentNodes["second"] = (new string[] { "first", "third", "fourth", "sixth" }).ToList();
            adjancentNodes["third"] = (new string[] { "seventh", "third", "first" }).ToList();
            adjancentNodes["fourth"] = (new string[] { "seventh", "third", "fourth" }).ToList();
            adjancentNodes["eleventh"] = (new string[] { "second", "third", "fourth" }).ToList();
            adjancentNodes["seventh"] = (new string[] { "second", "fourth", "sixth", "tenth" }).ToList();

            foreach (var key in adjancentNodes.Keys)
                manager.LoadAdjancentNodes(key, adjancentNodes[key]);

            var actualAdjancentNodes = GetAllAdjancentNodes();

            foreach (var key in adjancentNodes.Keys)
            {
                string actualKey = key.PadRight(idLength);

                if (!actualAdjancentNodes.Keys.Contains(actualKey))
                    Assert.Fail();

                for (int i = 0; i < adjancentNodes[key].Count; i++)
                {
                    string adjancentNode = adjancentNodes[key][i].PadRight(idLength);

                    if (!actualAdjancentNodes[actualKey].Contains(adjancentNode))
                        Assert.Fail();
                }
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

        private IDictionary<string, List<string>> GetAllAdjancentNodes()
        {
            var result = new Dictionary<string, List<string>>();

            try
            {
                SqlDataReader dataReader;

                connection.Open();

                string selectAllAdjancentNodesQuery = "SELECT Labels.IdenticalOfNode AS Id, Labels1.IdenticalOfNode AS AdjancentId " +
                    "FROM Labels CROSS JOIN Labels AS Labels1 "+
                    "INNER JOIN AdjancentNodes ON " +
                    "AdjancentNodes.NodeID = Labels.ID AND AdjancentNodes.AdjancentNode = Labels1.ID " +
                    "ORDER BY AdjancentNodes.NodeId";

                using (var command = new SqlCommand(selectAllAdjancentNodesQuery, connection))
                {
                    command.CommandType = System.Data.CommandType.Text;
                    dataReader = command.ExecuteReader();
                }

                string currentId = string.Empty;

                while (dataReader.Read())
                {
                    string key = dataReader["Id"].ToString();

                    if (!string.Equals(currentId, dataReader["Id"]))
                    {
                        result[key] = new List<string>();
                        currentId = key;
                    }
                    
                    result[key].Add(dataReader["AdjancentId"].ToString());
                }

                return result;
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
