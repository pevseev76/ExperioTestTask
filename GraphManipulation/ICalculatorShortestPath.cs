using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace GraphManipulation
{
    [ServiceContract]
    public interface ICalculatorShortestPath
    {
        [OperationContract]
        IList<string> CalculateShortestPath(string firstID, string secondID);
    }
}
