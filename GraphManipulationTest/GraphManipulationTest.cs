using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
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

        [TestMethod]
        public void TestOneRib()
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

                var adjacentNodes = new Dictionary<string, List<string>>();

                adjacentNodes["first"] = (new string[] { "second" }).ToList();

                foreach (var key in adjacentNodes.Keys)
                    for (int i = 0; i < adjacentNodes[key].Count; i++)
                        InsertAdjacentNode(key, adjacentNodes[key][i]);

                var manipulator = new CalculatorShortestPath();
                var path = manipulator.CalculateShortestPath("first", "second");

                Assert.IsNotNull(path);
                Assert.AreEqual(2, path.Count);
                Assert.AreEqual("first", path[0]);
                Assert.AreEqual("second", path[1]);
            }
            finally
            {
                connection.Close();
            }
        }

        [TestMethod]
        public void TestTwoLength()
        {
            var labels = new Dictionary<string, string>();

            labels["first"] = "January";
            labels["second"] = "February";
            labels["third"] = "Marth";

            try
            {
                connection.Open();

                Clear();

                foreach (var key in labels.Keys)
                    InsertLabel(key, labels[key]);

                var adjacentNodes = new Dictionary<string, List<string>>();

                adjacentNodes["first"] = (new string[] { "second" }).ToList();
                adjacentNodes["second"] = (new string[] { "third" }).ToList();

                foreach (var key in adjacentNodes.Keys)
                    for (int i = 0; i < adjacentNodes[key].Count; i++)
                        InsertAdjacentNode(key, adjacentNodes[key][i]);

                var manipulator = new CalculatorShortestPath();
                var path = manipulator.CalculateShortestPath("first", "third");

                Assert.IsNotNull(path);
                Assert.AreEqual(3, path.Count);
                Assert.AreEqual("first", path[0]);
                Assert.AreEqual("second", path[1]);
                Assert.AreEqual("third", path[2]);
            }
            finally
            {
                connection.Close();
            }
        }

        [TestMethod]
        public void TestThreeLength()
        {
            var labels = new Dictionary<string, string>();

            labels["first"] = "January";
            labels["second"] = "February";
            labels["third"] = "Marth";
            labels["fourth"] = "April";

            try
            {
                connection.Open();

                Clear();

                foreach (var key in labels.Keys)
                    InsertLabel(key, labels[key]);

                var adjacentNodes = new Dictionary<string, List<string>>();

                adjacentNodes["first"] = (new string[] { "second" }).ToList();
                adjacentNodes["second"] = (new string[] { "third" }).ToList();
                adjacentNodes["third"] = (new string[] { "fourth" }).ToList();

                foreach (var key in adjacentNodes.Keys)
                    for (int i = 0; i < adjacentNodes[key].Count; i++)
                        InsertAdjacentNode(key, adjacentNodes[key][i]);

                var manipulator = new CalculatorShortestPath();
                var path = manipulator.CalculateShortestPath("first", "fourth");

                Assert.IsNotNull(path);
                Assert.AreEqual(4, path.Count);
                Assert.AreEqual("first", path[0]);
                Assert.AreEqual("second", path[1]);
                Assert.AreEqual("third", path[2]);
                Assert.AreEqual("fourth", path[3]);
            }
            finally
            {
                connection.Close();
            }
        }

        [TestMethod]
        public void TestWidth()
        {
            var labels = new Dictionary<string, string>();

            labels["first"] = "January";
            labels["second"] = "February";
            labels["third"] = "Marth";
            labels["fourth"] = "April";

            try
            {
                connection.Open();

                Clear();

                foreach (var key in labels.Keys)
                    InsertLabel(key, labels[key]);

                var adjacentNodes = new Dictionary<string, List<string>>();

                adjacentNodes["first"] = (new string[] { "second", "third" }).ToList();
                adjacentNodes["second"] = (new string[] { "third" }).ToList();
                adjacentNodes["third"] = (new string[] { "fourth" }).ToList();

                foreach (var key in adjacentNodes.Keys)
                    for (int i = 0; i < adjacentNodes[key].Count; i++)
                        InsertAdjacentNode(key, adjacentNodes[key][i]);

                var manipulator = new CalculatorShortestPath();
                var path = manipulator.CalculateShortestPath("first", "third");

                Assert.IsNotNull(path);
                Assert.AreEqual(2, path.Count);
                Assert.AreEqual("first", path[0]);
                Assert.AreEqual("third", path[1]);
            }
            finally
            {
                connection.Close();
            }
        }

        [TestMethod]
        public void TestCycle()
        {
            var labels = new Dictionary<string, string>();

            labels["first"] = "January";
            labels["second"] = "February";
            labels["third"] = "Marth";
            labels["fourth"] = "April";

            try
            {
                connection.Open();

                Clear();

                foreach (var key in labels.Keys)
                    InsertLabel(key, labels[key]);

                var adjacentNodes = new Dictionary<string, List<string>>();

                adjacentNodes["first"] = (new string[] { "second" }).ToList();
                adjacentNodes["second"] = (new string[] {"first", "third" }).ToList();
                adjacentNodes["third"] = (new string[] { "first", "fourth" }).ToList();

                foreach (var key in adjacentNodes.Keys)
                    for (int i = 0; i < adjacentNodes[key].Count; i++)
                        InsertAdjacentNode(key, adjacentNodes[key][i]);

                var manipulator = new CalculatorShortestPath();
                var path = manipulator.CalculateShortestPath("first", "fourth");

                Assert.IsNotNull(path);
                Assert.AreEqual(4, path.Count);
                Assert.AreEqual("first", path[0]);
                Assert.AreEqual("second", path[1]);
                Assert.AreEqual("third", path[2]);
                Assert.AreEqual("fourth", path[3]);
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

        private void InsertAdjacentNode(string id, string adjancent)
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
