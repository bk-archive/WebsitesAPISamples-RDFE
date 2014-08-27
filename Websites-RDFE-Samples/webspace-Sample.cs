using Microsoft.WindowsAzure.Management.WebSites;
using Microsoft.WindowsAzure.Management.WebSites.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Websites_RDFE_Samples
{
    class RDFE_WebSpace_Sample
    {
        public WebSiteManagementClient client = null;
        public string resourceGroupName { get; set; }
        private List<RDFE_WebSpace> webspaceList { get; set; }
        private string clearBuffer = "";
        public RDFE_WebSpace_Sample(WebSiteManagementClient c, string n)
        {
            client = c;
            resourceGroupName = n;
            webspaceList = new List<RDFE_WebSpace>();
            intializeList();
        }
        private void intializeList()
        {
            var maxLength = 0;
            WebSpacesListResponse response = client.WebSpaces.ListAsync(new CancellationToken()).Result;
            
            foreach (var item in response.WebSpaces)
            {
                if(item.Name.Contains(resourceGroupName))
                { 
                    webspaceList.Add(new RDFE_WebSpace( item.Name, item.GeoRegion ));
                    maxLength = maxLength < item.ToString().Length ? item.ToString().Length : maxLength;
                }
            }
            
            for (int i = 0; i < maxLength; i++)
            {
                clearBuffer += " ";
            }
        }


        internal string getLocation()
        {
            var i = 0;
            var positionX = Console.CursorLeft;
            var positionY = Console.CursorTop;

            while (true)
            {
                Console.CursorLeft = positionX;
                Console.CursorTop = positionY;
                Console.Write(webspaceList.ElementAt(i).name);
                var index = Console.ReadKey(true);

                if (index.Key == ConsoleKey.UpArrow || index.Key == ConsoleKey.RightArrow)
                {
                    i++;
                    i = i > webspaceList.Count() - 1 ? i % webspaceList.Count() : i;
                }
                else if (index.Key == ConsoleKey.DownArrow || index.Key == ConsoleKey.LeftArrow)
                {
                    i--;
                    i = i < 0 ? webspaceList.Count - 1 : i;
                }
                else if (index.Key == ConsoleKey.Enter)
                {
                    Console.Write("\n");
                    return webspaceList.ElementAt(i).name;

                }
                else
                {
                    //Other key, ignore it.
                }
                Console.CursorLeft = positionX;
                Console.CursorTop = positionY;
                Console.Write(clearBuffer);
            }
        }
    }

    public class RDFE_WebSpace
    {
        public RDFE_WebSpace(string n, string r)
        {
            name = n;
            geoRegion = r;
        }
        public string name { get; set; }
        public string geoRegion { get; set; }
    }
}
