using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;

namespace DataLoader
{
    class Program
    {
        static void Main(string[] args)
        {
            string path;

            if (args.Length < 1)
            {
                Console.WriteLine("Input the path to load, please");
                path = Console.ReadLine();
            }
            else
                path = args[0];

            if (!Directory.Exists(path))
            {
                Console.WriteLine("Input path does not exist!");
                return;
            }

            Load(path);
        }

        static void Load(string path)
        {
            var files = Directory.GetFiles(path, "*.xml", SearchOption.TopDirectoryOnly);
            var labels = new Dictionary<string, string>();
            var adjacentNodes = new Dictionary<string, IList<string>>();

            for (int i = 0; i < files.Length; i++)
            {
                KeyValuePair<string, string> idLabel;
                KeyValuePair<string, IList<string>> idAdjacentNodes;

                var fileStream = File.Open(files[i], FileMode.Open);

                Parse(fileStream, out idLabel, out idAdjacentNodes);

                labels.Add(idLabel.Key, idLabel.Value);
                adjacentNodes.Add(idAdjacentNodes.Key, idAdjacentNodes.Value);
            }

            var client = new DataManagement.DataManagerClient();

            client.LoadLabels(labels);

            foreach (var pair in adjacentNodes)
                client.LoadAdjancentNodes(pair.Key, pair.Value.ToArray());
        }

        static void Parse(FileStream file, out KeyValuePair<string, string> idLabel, out KeyValuePair<string, IList<string>> idAdjacentNodes)
        {
            var doc = new XmlDocument();
            doc.Load(file);

            var root = doc.DocumentElement;

            string id = root["id"].InnerText;
            string label = root["label"].InnerText;

            idLabel = new KeyValuePair<string, string>(id, label);

            var adjacentNodes = root["adjacentNodes"].ChildNodes;
            var listAdjacentNodes = new List<string>();

            for (int i = 0; i < adjacentNodes.Count; i++)
                listAdjacentNodes.Add(adjacentNodes[i].InnerText);
            
            idAdjacentNodes = new KeyValuePair<string, IList<string>>(id, listAdjacentNodes);
        }
    }
}
