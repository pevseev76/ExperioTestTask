using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Description;
using DataManagement;
using DataProvider;

namespace Host
{
    class Program
    {
        private const string dataManagerServiceName = "DataManager";
        private const string dataManagerServiceAddress = "http://localhost:8000/DataManagement/";

        private const string dataProviderServiceName = "DataProvider";
        private const string dataProviderServiceAddress = "http://localhost:8000/DataProvider/";
        
        static void Main(string[] args)
        {
            /* var baseAddress = new Uri("http://localhost:8000/DataManagement/");
            var selfHost = new ServiceHost(typeof(DataManager), baseAddress);

            try
            {
                selfHost.AddServiceEndpoint(typeof(IDataManager), new WSHttpBinding(), "DataManager");

                var smb = new ServiceMetadataBehavior() { HttpGetEnabled = true };
                selfHost.Description.Behaviors.Add(smb);

                selfHost.Open();

                Console.WriteLine("The DataManager service has been started. Please <ENTER> to terminate it.");
                Console.WriteLine();
                Console.ReadLine();

                selfHost.Close();
            }
            catch (CommunicationException e)
            {
                Console.WriteLine("An exception occured {0} ", e.Message);
                selfHost.Abort();
            } */

            var dataManagerHost = CreateHost(dataManagerServiceAddress, typeof(DataManager), typeof(IDataManager), dataManagerServiceName);
            var dataProviderHost = CreateHost(dataProviderServiceAddress, typeof(DataProvider.DataProvider), typeof(IDataProvider), dataProviderServiceName);

            try
            {
                dataManagerHost.Open();
                dataProviderHost.Open();

                Console.WriteLine("The " + dataManagerServiceName + "service has been started.");
                Console.WriteLine("The " + dataProviderServiceName + "service has been started.");
                Console.WriteLine("Please <ENTER> to terminate its.");
                Console.WriteLine();
                Console.ReadLine();

                dataManagerHost.Close();
                dataProviderHost.Close();
            }
            catch (CommunicationException e)
            {
                Console.WriteLine("An exception occured {0} ", e.Message);
                dataManagerHost.Abort();
                dataProviderHost.Abort();
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
