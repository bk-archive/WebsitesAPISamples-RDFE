using Microsoft.WindowsAzure.Management.WebSites;
using Microsoft.WindowsAzure.Management.WebSites.Models;
using Microsoft.WindowsAzure;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace Websites_RDFE_Samples
{
    public class RDFE_WebSite_Sample
    {
        private readonly string _webSpaceName;
        public WebSiteManagementClient _client = null;

        public RDFE_WebSite_Sample(string r, WebSiteManagementClient c)
        {
            _client = c;
            var webSpace = new RDFE_WebSpace_Sample(_client, r);
            Console.Write("WebSpace:");
            _webSpaceName = webSpace.getLocation();
        }

        public bool CreateWebSite()
        {
            var wsParameters = new WebSiteCreateParameters();

            Console.WriteLine("...:::Collect Web Site Parameters:::...");
            Console.Write("Web Site Name:");
            wsParameters.Name = Console.ReadLine();

            Console.Write("Web Hosting Plan Name: ");
            wsParameters.ServerFarm = Console.ReadLine();

            var response = _client.WebSites.Create(_webSpaceName, wsParameters);

            Console.WriteLine("Request ID \t" + response.RequestId + "\n" + "HTTP Status Code : \t" + response.StatusCode);

            ListWebSites();
            return true;
        }

        public bool ListWebSites()
        {

            Console.WriteLine("...:::Web Sites:::...");
            //List all the web sites in a web space

            var webSites = _client.WebSpaces.ListWebSites(_webSpaceName, new WebSiteListParameters());

            webSites.WebSites.ToList().ForEach(item =>
                Console.WriteLine(JsonConvert.SerializeObject(item, Formatting.Indented))
                );

            return true;
        }

        public bool DeleteWebSite()
        {
            Console.WriteLine("...:::Delete Web Site:::...");
            Console.Write("Web Site Name: ");
            var name = Console.ReadLine();

            try
            {
                var response = _client.WebSites.Delete(_webSpaceName, name, new WebSiteDeleteParameters());
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    Console.WriteLine("Request ID \t" + response.RequestId + "\n" + "HTTP Status Code : \t" + response.StatusCode);
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("Web Site:  \"" + name + "\"  not found");
            }

            ListWebSites();
            return true;
        }

        public WebSite GetWebSite(string name)
        {
            var response = _client.WebSites.Get(_webSpaceName, name, new WebSiteGetParameters());

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return response.WebSite;
            }
            
            return null;
        }

        public void GetWebSite()
        {
            Console.WriteLine("Web Site: ");
            var webSiteName = Console.ReadLine();

            var ws = GetWebSite(webSiteName);

            if (ws != null)
            {
                Console.WriteLine(JsonConvert.SerializeObject(ws, Formatting.Indented));
            }
            else
            {
                Console.WriteLine("Error: WebSite \"" + webSiteName + "\" Not Found");
            }
        }


        public bool WebSiteOperations()
        {
            int webSiteOperation;

            //Web Hosting Plan Operations
            Console.Clear();
            Console.WriteLine("...::Web Site Operations::...");

            Console.WriteLine("1) Create a new Web Site");
            Console.WriteLine("2) Delete an Existing Web Site");
            Console.WriteLine("3) List all Web Sites in a Resource Group");
            Console.WriteLine("4) Get a specific Web Site by Name in a Resource Group");
            Console.WriteLine("0) back to previous menu");

            int.TryParse(Console.ReadLine(), out webSiteOperation);

            switch (webSiteOperation)
            {
                case 1:
                    CreateWebSite();
                    Console.ReadLine();
                    return true;
                case 2:
                    DeleteWebSite();
                    Console.ReadLine();
                    return true;
                case 3:
                    ListWebSites();
                    Console.ReadLine();
                    return true;
                case 4:
                    GetWebSite();
                    Console.ReadLine();
                    return true;
                default:
                    return false;
                    
            }
        }

    }
}
