using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Description;
using DataManagement;

namespace Host
{
    class Program
    {
        static void Main(string[] args)
        {
            var baseAddress = new Uri("http://localhost:8000/DataManagement/");
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
            }
        }
    }
}
