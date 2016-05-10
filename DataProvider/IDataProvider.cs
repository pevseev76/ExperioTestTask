using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace DataProvider
{
    [ServiceContract]
    public interface IDataProvider
    {
        [OperationContract]
        IDictionary<string, string> GetLabels();

        [OperationContract]
        IList<string> GetAdjancentNodes(string nodeId);
    }
}
