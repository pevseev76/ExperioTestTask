using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace DataManagement
{
    public class DataManager : IDataManager
    {
        public void LoadLabels(IDictionary<string, string> labels) { }

        public void LoadAdjancentNodes(string nodeId, IList<string> adjancentNodes) { }
    }
}
