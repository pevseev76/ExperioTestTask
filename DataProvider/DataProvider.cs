using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace DataProvider
{
    public class DataProvider : IDataProvider
    {
        public IDictionary<string, string> GetLabels()
        {
            throw new NotImplementedException();
        }

        public IList<string> GetAdjancentNodes(string nodeId)
        {
            throw new NotImplementedException();
        }
    }
}
