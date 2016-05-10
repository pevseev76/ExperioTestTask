using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLoader
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new DataManagement.DataManagerClient();

            var labels = new Dictionary<string, string>();

            labels["one"] = "winter";
            labels["two"] = "spring";
            labels["three"] = "summer";
            labels["four"] = "autumn";

            client.LoadLabels(labels);
        }
    }
}
