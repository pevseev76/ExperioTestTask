using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace DataManagement
{
    [ServiceContract]
    public interface IDataManager
    {
        [OperationContract]
        void LoadLabels(IDictionary<string, string> labels);

        [OperationContract]
        void LoadAdjancentNodes(string nodeId, IList<string> adjancentNodes);
    }
}
