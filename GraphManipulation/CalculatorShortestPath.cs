using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace GraphManipulation
{
    public class CalculatorShortestPath : ICalculatorShortestPath, IDisposable
    {
        private const int idLength = 10;
        
        #region Connection string parameters

        private const string dataSource = "Data Source=.\\SQLEXPRESS;";
        private const string initialCatalog = "Initial Catalog = NodesBD;";
        private const string integratedSecurity = "Integrated Security=true;";
        private const string pooling = "Pooling=false";
        
        #endregion

        private readonly SqlConnection connection;

        public CalculatorShortestPath()
        {
            string connectionString = dataSource + initialCatalog + integratedSecurity + pooling;
            connection = new SqlConnection(connectionString);
        }

        public IList<string> CalculateShortestPath(string firstID, string secondID)
        {
            if (string.IsNullOrWhiteSpace(firstID) || string.IsNullOrWhiteSpace(secondID))
                return null;

            try
            {
                connection.Open();
                return ShortestPath(firstID, secondID);
            }
            finally
            {
                connection.Close();
            }
        }

        private IList<string> ShortestPath(string firstID, string secondID)
        {
            var marked = new SortedSet<string>();
            var path = new Queue<List<string>>();

            var lst = new List<string>();
            lst.Add(firstID);
            path.Enqueue(lst);

            return Short(path, marked, secondID);
        }

        private IList<string> Short(Queue<List<string>> path, ISet<string> marked, string secondID)
        {
            if (path.Count == 0)
                return null;

            var current = path.Dequeue();
            string last = current.Last();
            var adjacentNodes = GetAdjacentNodes(last);

            for (int i = 0; i < adjacentNodes.Count; i++)
            {
                var list = new List<string>(current);

                if (marked.Contains(adjacentNodes[i]))
                    continue;

                list.Add(adjacentNodes[i]);

                if (string.Equals(adjacentNodes[i], secondID))
                    return list;

                marked.Add(adjacentNodes[i]);
                path.Enqueue(list);
            }

            return Short(path, marked, secondID);
        }

        private IList<string> GetAdjacentNodes(string id)
        {
            var result = new List<string>();

            SqlDataReader dataReader;

            using (var command = new SqlCommand("StoredProcedure3", connection))
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;

                var nodeId = new SqlParameter("@parameter1", System.Data.SqlDbType.Char, idLength);
                nodeId.Value = id;
                command.Parameters.Add(nodeId);

                dataReader = command.ExecuteReader();
            }

            try
            {
                while (dataReader.Read())
                    result.Add(dataReader["IdenticalOfNode"].ToString().Trim());
            }
            finally
            {
                dataReader.Dispose();
            }
            
            return result;
        }

        public void Dispose()
        {
            connection.Dispose();
        }
    }
}
