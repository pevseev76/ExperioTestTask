using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Description;
using DataManagement;
using DataProvider;
using GraphManipulation;

namespace Host
{
    class Program
    {
        private const string dataManagerServiceName = "DataManager";
        private const string dataManagerServiceAddress = "http://localhost:8000/DataManagement/";

        private const string dataProviderServiceName = "DataProvider";
        private const string dataProviderServiceAddress = "http://localhost:8000/DataProvider/";

        private const string graphManipulationServiceName = "GraphManipulation";
        private const string graphManipulationServiceAddress = "http://localhost:8000/GraphManipulation/";
        
        static void Main(string[] args)
        {
            var dataManagerHost = CreateHost(dataManagerServiceAddress, typeof(DataManager), typeof(IDataManager), dataManagerServiceName);
            var dataProviderHost = CreateHost(dataProviderServiceAddress, typeof(DataProvider.DataProvider), typeof(IDataProvider), dataProviderServiceName);
            var graphManipulationHost = CreateHost(graphManipulationServiceAddress, typeof(CalculatorShortestPath), typeof(ICalculatorShortestPath), graphManipulationServiceName);

            try
            {
                dataManagerHost.Open();
                dataProviderHost.Open();
                graphManipulationHost.Open();

                Console.WriteLine("The " + dataManagerServiceName + "service has been started.");
                Console.WriteLine("The " + dataProviderServiceName + "service has been started.");
                Console.WriteLine("The " + graphManipulationServiceName + "service has been started.");
                Console.WriteLine("Please <ENTER> to terminate its.");
                Console.WriteLine();
                Console.ReadLine();

                dataManagerHost.Close();
                dataProviderHost.Close();
                graphManipulationHost.Close();
            }
            catch (CommunicationException e)
            {
                Console.WriteLine("An exception occured {0} ", e.Message);
                dataManagerHost.Abort();
                dataProviderHost.Abort();
                graphManipulationHost.Abort();
            } 
        }

        static ServiceHost CreateHost(string strAddress, Type serviceType, Type contractType, string serviceName)
        {
            var baseAddress = new Uri(strAddress);
            var result = new ServiceHost(serviceType, baseAddress);

            try
            {
                result.AddServiceEndpoint(contractType, new WSHttpBinding(), serviceName);

                var smb = new ServiceMetadataBehavior() { HttpGetEnabled = true };
                result.Description.Behaviors.Add(smb); 
            }
            catch (CommunicationException e)
            {
                Console.WriteLine("An exception occured {0} ", e.Message);
                result.Abort();
                return null;
            }

            return result;
        }
    }
}
