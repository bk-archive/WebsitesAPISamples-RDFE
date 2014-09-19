using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Management.WebSites;
using Microsoft.WindowsAzure.Management.WebSites.Models;
using Microsoft.WindowsAzure.WebSitesExtensions;
using Microsoft.WindowsAzure.WebSitesExtensions.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Websites_RDFE_Samples
{
    class RDFE_webjob_Sample
    {

        private BasicAuthenticationCloudCredentials credentials = new BasicAuthenticationCloudCredentials();
        private WebSiteExtensionsClient _extensionClient = null;
        private string WebsiteName = "";

        private void getCredentials()
        {
            ///Use the credentials provided in the publishing profile.
            ///Username is the "userName" for msdeploy, should be something like $sitename
            Console.Write("Publishing User Name:");
            credentials.Username = Console.ReadLine();

            ///Password is the "userPWD", should be a long string like: s2SgoRpwgexHEMLbvjPcvjReD1bftztEzhJLsKskqQwoWGblcZvyzAbYsPgL
            Console.Write("Publishing Password:");
            credentials.Password = Console.ReadLine();
            _extensionClient = new WebSiteExtensionsClient(WebsiteName, credentials);
        }

        private void response(object output)
        {
            Console.WriteLine(JsonConvert.SerializeObject(output, Formatting.Indented));
        }

        public bool WebJobCreate(bool Triggered = false)
        {
            Console.Clear();
            Console.Write("WebJob Name:");
            var webJobName = Console.ReadLine();

            Console.Write("WebJob File:");
            var webJobPath = Console.ReadLine();


            if (Triggered)
            {
                if (Path.GetExtension(webJobPath).ToString().ToLowerInvariant() != ".zip")
                {
                    var x = _extensionClient.TriggeredWebJobs.UploadFile(webJobName, webJobPath,
                        new MemoryStream(File.ReadAllBytes(webJobPath)));

                    response(x);
                }
                else
                {
                    var x = _extensionClient.TriggeredWebJobs.UploadZip(webJobName, webJobPath,
                        new MemoryStream(File.ReadAllBytes(webJobPath)));

                    response(x);
                }
            }
            else //Continous
            {
                if (Path.GetExtension(webJobPath).ToString().ToLowerInvariant() != ".zip")
                {
                    var x = _extensionClient.ContinuousWebJobs.UploadFile(webJobName, webJobPath,
                        new MemoryStream(File.ReadAllBytes(webJobPath)));

                    response(x);
                }
                else
                {
                    var x = _extensionClient.ContinuousWebJobs.UploadZip(webJobName, webJobPath,
                        new MemoryStream(File.ReadAllBytes(webJobPath)));
                    
                    response(x);
                }
 
            }


            

            return true;
        }

        public void WebJobList(bool Triggered = false)
        {
            if (Triggered)
            {
                var WebJobList = _extensionClient.TriggeredWebJobs.List();
                response(WebJobList);
            }
            else //Continious
            {
                var WebJobList = _extensionClient.ContinuousWebJobs.List();
                response(WebJobList);
            }
        }

        public void WebJobGet(bool Triggered = false)
        {
            Console.Write("WebJob Name:");
            var webJobName = Console.ReadLine();

            if (Triggered)
            {
                var WebJobGet = _extensionClient.TriggeredWebJobs.Get(webJobName);
                response(WebJobGet);
            }
            else //Continious
            {
                var WebJobGet = _extensionClient.ContinuousWebJobs.Get(webJobName);
                response(WebJobGet);
            }
        }
        
        public void WebJobGetSettings(bool Triggered = false)
        {
            Console.Write("WebJob Name:");
            var webJobName = Console.ReadLine();

            if (Triggered)
            {
                var WebJobSettings = _extensionClient.TriggeredWebJobs.GetSettings(webJobName);
                response(WebJobSettings);
            }
            else //Continious
            {
                var WebJobSettings = _extensionClient.ContinuousWebJobs.GetSettings(webJobName);
                response(WebJobSettings);
            }
        }

        public void WebJobSetSettings(bool Triggered = false)
        {
            ContinuousWebJobSettingsUpdateParameters cwjSUP = new ContinuousWebJobSettingsUpdateParameters();
            TriggeredWebJobSettingsUpdateParameters twjSUP = new TriggeredWebJobSettingsUpdateParameters();
            int shutdownGracePeriod = 0;
            string singleton = "";

            Console.Write("WebJob Name:");
            var webJobName = Console.ReadLine();
            Console.Write("Shutdown Grace Time In Seconds:");
            int.TryParse(Console.ReadLine(), out shutdownGracePeriod);

            if (Triggered)
            {
                twjSUP.ShutdownGraceTimeInSeconds = shutdownGracePeriod;
                var WebJobSettings = _extensionClient.TriggeredWebJobs.SetSettings(webJobName,twjSUP);
                response(WebJobSettings);
            }
            else //Continious
            {
                Console.Write("Single Instance job?(YES|no):");
                singleton = Console.ReadLine();
                cwjSUP.IsSingleton = singleton == string.Empty || singleton.ToLowerInvariant() == "yes" ? true : false;
                var WebJobSettings = _extensionClient.ContinuousWebJobs.SetSettings(webJobName, cwjSUP);
                response(WebJobSettings);
            }
        }

        public void WebJobDelete(bool Triggered = false)
        {
            Console.Write("WebJob Name:");
            var webJobName = Console.ReadLine();

            if (Triggered)
            {
                var WebJobSettings = _extensionClient.TriggeredWebJobs.Delete(webJobName);
                response(WebJobSettings);
            }
            else //Continious
            {
                var WebJobSettings = _extensionClient.ContinuousWebJobs.Delete(webJobName);
                response(WebJobSettings);
            }
        }

        public void WebJobTriggerListRuns()
        {
            Console.Write("WebJob Name:");
            var webJobName = Console.ReadLine();

            var WebJobList = _extensionClient.TriggeredWebJobs.ListRuns(webJobName);
                response(WebJobList);
        }

        public void WebJobTriggerGetRun()
        {
            Console.Write("WebJob Name:");
            var WebJobName = Console.ReadLine();
            Console.Write("WebJob Run:");
            var Run = Console.ReadLine();

            var WebJobGetRun = _extensionClient.TriggeredWebJobs.GetRun(WebJobName, Run);
            response(WebJobGetRun);
        }

        public void WebJobTriggerRun()
        {
            Console.Write("WebJob Name:");
            var WebJobName = Console.ReadLine();
            var WebJobRun = _extensionClient.TriggeredWebJobs.Run(WebJobName);
            response(WebJobRun);
        }

        public void WebJobContinousStart()
        {
            Console.Write("WebJob Name:");
            var WebJobName = Console.ReadLine();
            var WebJobStart = _extensionClient.ContinuousWebJobs.Start(WebJobName);
            response(WebJobStart);
        }

        public void WebJobContinousStop()
        {
            Console.Write("WebJob Name:");
            var WebJobName = Console.ReadLine();
            var WebJobStop = _extensionClient.ContinuousWebJobs.Stop(WebJobName);
            response(WebJobStop);
        }

        public bool WebJobOperations()
        {
            int webJobOperation;

            //WebJob Operations
            Console.Clear();
            Console.WriteLine("...::WebJob Operations::...");

            if (WebsiteName == string.Empty)
            {
                Console.Write("Target Website:");
                WebsiteName = Console.ReadLine();
                getCredentials();
            }
            else
            {
                Console.WriteLine("Target Website:" + WebsiteName);
            }

            Console.WriteLine("1)\tCreate a new Continous WebJob");
            Console.WriteLine("2)\tCreate a new Triggered WebJob");
            Console.WriteLine("3)\tList Continous WebJob");
            Console.WriteLine("4)\tList Triggered WebJob");
            Console.WriteLine("5)\tGet a Continous WebJob by Name");
            Console.WriteLine("6)\tGet a Triggered WebJob by Name");
            Console.WriteLine("7)\tGet Continous WebJob Settings");
            Console.WriteLine("8)\tGet Triggered WebJob Settings");
            Console.WriteLine("9)\tRun a Triggered WebJob"); 
            Console.WriteLine("10)\tList Triggered WebJob Runs");
            Console.WriteLine("11)\tGet Triggered WebJob Run");
            Console.WriteLine("12)\tStart a Continous WebJob");
            Console.WriteLine("13)\tStop a Continous WebJob");
            Console.WriteLine("0)\tback to previous menu");

            int.TryParse(Console.ReadLine(), out webJobOperation);

            switch (webJobOperation)
            {
                case 1:
                    WebJobCreate(false);
                    Console.ReadLine();
                    return true;
                case 2:
                    WebJobCreate(true);
                    Console.ReadLine();
                    return true;
                case 3:
                    WebJobList(false);
                    Console.ReadLine();
                    return true;
                case 4:
                    WebJobList(true);
                    Console.ReadLine();
                    return true;
                case 5:
                    WebJobGet(false);
                    Console.ReadLine();
                    return true;
                case 6:
                    WebJobGet(true);
                    Console.ReadLine();
                    return true;
                case 7:
                    WebJobGetSettings(false);
                    Console.ReadLine();
                    return true;
                case 8:
                    WebJobGetSettings(true);
                    Console.ReadLine();
                    return true;
                case 9:
                    WebJobTriggerRun();
                    Console.ReadLine();
                    return true;
                case 10:
                    WebJobTriggerListRuns();
                    Console.ReadLine();
                    return true;
                case 11:
                    WebJobTriggerGetRun();
                    Console.ReadLine();
                    return true;
                case 12:
                    WebJobContinousStart();
                    Console.ReadLine();
                    return true;
                case 13:
                    WebJobContinousStop();
                    Console.ReadLine();
                    return true;
                default:
                    return false;

            }
        }
    }
}




