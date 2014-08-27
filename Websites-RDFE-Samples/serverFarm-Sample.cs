using Microsoft.WindowsAzure.Management.WebSites;
using Microsoft.WindowsAzure.Management.WebSites.Models;
using Microsoft.WindowsAzure;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Websites_RDFE_Samples
{
    public class RDFE_WebHostingPlan_Sample
    {
        private string webSpaceName;
        private string resourceGroupName;
        public WebSiteManagementClient client = null;


        public RDFE_WebHostingPlan_Sample(string r, WebSiteManagementClient c)
        {
            resourceGroupName = r;
            client = c;
            var webSpace = new RDFE_WebSpace_Sample(client, r);
            Console.Write("WebSpace:");
            webSpaceName = webSpace.getLocation();
        }

        

        public bool createWebHostingPlan()
        {
            var sku = new SKUHelper();
            var size = new SIZEHelper();
            WebHostingPlanCreateParameters whpParameters = new WebHostingPlanCreateParameters();

            Console.WriteLine("...:::Collect Web Hosting Plan Parameters:::...");
            Console.Write("Web Hosting Plan Name:");
            whpParameters.Name = Console.ReadLine();

            Console.Write("Web Hosting Plan SKU: ");
            whpParameters.SKU = sku.getSKU();

            if (sku.isDeidicated(whpParameters.SKU.ToString()))
            {
                Console.Write("Worker Size: ");
                whpParameters.WorkerSize = size.getSize();

                Console.Write("Number of Workers:");
                var number = 1;
                int.TryParse(Console.ReadLine(), out number);
                whpParameters.NumberOfWorkers = number;
            }

            WebHostingPlanCreateResponse response =  client.WebHostingPlans.Create(webSpaceName, whpParameters);

            Console.WriteLine("Request ID \t" + response.RequestId + "\n" + "HTTP Status Code : \t" + response.StatusCode);

            listWebHostingPlan();
            return true;
        }

        public bool listWebHostingPlan()
        {

            Console.WriteLine("...:::Web Hosting Plans:::...");
            //List all the web hosting plans in a resource group

            WebHostingPlanListResponse whplr = client.WebHostingPlans.List(webSpaceName);

            whplr.WebHostingPlans.ToList<WebHostingPlan>().ForEach(item =>
                Console.WriteLine(JsonConvert.SerializeObject(item, Formatting.Indented))
                );

            return true;
        }

        public bool deleteWebHostingPlan()
        {
            Console.WriteLine("...:::Delete Web Hosting Plan:::...");
            Console.Write("Web Hosting Plan Name: ");
            var name = Console.ReadLine();

            try
            {
                var response = client.WebHostingPlans.Delete(webSpaceName,name);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    Console.WriteLine("Request ID \t" + response.RequestId + "\n" + "HTTP Status Code : \t" + response.StatusCode);
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("Web Hosting Plan:  \"" + name + "\"  not found");
            }
            listWebHostingPlan();
            return true;
        }

        public WebHostingPlan getWebHostingPlan(string name)
        {
            var response = client.WebHostingPlans.Get(webSpaceName, name);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return response.WebHostingPlan;
            }
            else
            {
                return null;
            }
        }

        public void getWebHostingPlan()
        {
            Console.WriteLine("Web Hosting Plan: ");
            var whpName = Console.ReadLine();

            var whp = getWebHostingPlan(whpName);

            if (whp != null)
            {
                Console.WriteLine(JsonConvert.SerializeObject(whp, Formatting.Indented));
            }
            else
            {
                Console.WriteLine("Error: WebHostingPlan \"" + whpName + "\" Not Found");
            }
        }


        public bool webHostingPlanOperations()
        {
            var webHostingPlanOperation = 0;

            //Web Hosting Plan Operations
            Console.Clear();
            Console.WriteLine("...::Web Hosting Plan Operations::...");

            Console.WriteLine("1) Create a new Web Hosting Plan");
            Console.WriteLine("2) Delete an Existing Web Hosting Plan");
            Console.WriteLine("3) List all Web Hosting Plans in a Resource Group");
            Console.WriteLine("4) Get a specific Web Hosting Plan by Name in a Resource Group");
            Console.WriteLine("0) back to previous menu");

            int.TryParse(Console.ReadLine(), out webHostingPlanOperation);

            switch (webHostingPlanOperation)
            {
                case 1:
                    createWebHostingPlan();
                    Console.ReadLine();
                    return true;
                case 2:
                    deleteWebHostingPlan();
                    Console.ReadLine();
                    return true;
                case 3:
                    listWebHostingPlan();
                    Console.ReadLine();
                    return true;
                case 4:
                    getWebHostingPlan();
                    Console.ReadLine();
                    return true;
                default:
                    return false;
                    
            }
        }

    }
}
