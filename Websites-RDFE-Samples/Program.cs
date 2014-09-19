using System;
using System.Configuration;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Websites_RDFE_Samples
{
    class Program
    {
        static void Main(string[] args)
        {
            var settingsReader = ConfigurationManager.AppSettings;
            var aadConfig = new AzureActiveDirectoryConfig();

            //Get Azure Active Directory Configuration form App Settings
            try
            {
                aadConfig.azureSubscriptionID = settingsReader["subscriptionID"];
                aadConfig.aadApplicationName = settingsReader["ActiveDirectoryApplicationName"];
                aadConfig.aadRedirectURL = settingsReader["ActiveDirectoryApplicationRedirect"];
                aadConfig.addClientID = settingsReader["ActiveDirectoryClientID"];
                aadConfig.addTenant = settingsReader["ActiveDirectoryadTenant"];
                aadConfig.aadResourceURI = settingsReader["ActiveDirectoryResourceUri"];
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Source);
            }

            //Authenticates ARM websites client
            var RDFEClient = new AzureActiveDirectoryHelper(aadConfig);

            //Sets the Resource Group to use for samples
            Console.Write("Resource Group:");
            var resourceGroup = Console.ReadLine();

            //TODO remove this...
            resourceGroup = resourceGroup == string.Empty?"demoRG":resourceGroup;

            //Initialize the Web Hosting Plan Samples
            var webHostingPlanSample = new RDFE_WebHostingPlan_Sample(resourceGroup, RDFEClient.client);

            //Initialize the Website Samples
            var websiteSample = new RDFE_WebSite_Sample(resourceGroup, RDFEClient.client);

            var webjobSample = new RDFE_webjob_Sample();

            var operation = 0;
            var mainMenu = true;

            while (mainMenu)
            {
                Console.Clear();
                Console.WriteLine("...::Select Samples::...");
                Console.WriteLine("1) Web Hosting Plan Operations");
                Console.WriteLine("2) Website Operations");
                Console.WriteLine("3) Webjob Operations");
                Console.WriteLine("0) Quit");

                if (int.TryParse(Console.ReadLine(), out operation))
                {
                    switch (operation)
                    {
                        case 1:
                            var webHostingPlanmMenu = true;
                            while (webHostingPlanmMenu)
                            {
                                webHostingPlanmMenu = webHostingPlanSample.webHostingPlanOperations();
                            }

                            break;
                        case 2:
                            var websiteMenu = true;
                            while (websiteMenu)
                            {
                                websiteMenu = websiteSample.WebSiteOperations();
                            }
                            break;
                        case 3:
                            var webJobMenue = true;
                            while (webJobMenue)
                            {
                                webJobMenue = webjobSample.WebJobOperations();
                            }
                            break;
                        case 0:
                            mainMenu = false;
                            Console.Clear();
                            Console.Write("..::GOODBYE::..");
                            Thread.Sleep(2000);
                            break;
                        default:
                            Console.Clear();
                            Console.Write("Operation Not recognized:");
                            break;
                    }
                }
                else
                {
                    Console.Clear();
                    Console.Write("Operation Not recognized:");
                }
            }
        }
    }
}
