using Microsoft.WindowsAzure.Management.WebSites.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Websites_RDFE_Samples
{
    class SKUHelper
    {
        private List<string> skuList { get; set; }
        private int maxLength = 0;
        private string clearBuffer;

        public SKUHelper()
        {
            skuList = new List<string>();
            clearBuffer = "";
            
            foreach (SkuOptions item in Enum.GetValues(typeof(SkuOptions)))
            {
                skuList.Add(item.ToString());
                maxLength = maxLength < item.ToString().Length ? item.ToString().Length : maxLength;
            }

            for (int i = 0; i < maxLength; i++)
            {
                clearBuffer += " ";
            }
        }
        public SkuOptions getSKU()
        {
            var i = 0;
            var positionX = Console.CursorLeft;
            var positionY = Console.CursorTop; 

            while (true)
            {
                Console.CursorLeft = positionX;
                Console.CursorTop = positionY;
                Console.Write(skuList.ElementAt(i));
                var index = Console.ReadKey(true);

                if (index.Key == ConsoleKey.UpArrow || index.Key == ConsoleKey.RightArrow)
                {
                    i++;
                    i = i > skuList.Count() - 1 ? i % skuList.Count() : i;
                }
                else if (index.Key == ConsoleKey.DownArrow || index.Key == ConsoleKey.LeftArrow)
                {
                    i--;
                    i = i < 0 ? skuList.Count - 1 : i;
                }
                else if (index.Key == ConsoleKey.Enter)
                {
                    Console.Write("\n");
                    foreach (SkuOptions item in Enum.GetValues(typeof(SkuOptions)))
                    {
                        if (skuList.ElementAt(i) == item.ToString())
                        {
                            return item;
                        }
                    }
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

        public bool isDeidicated(string sku)
        {
            if (sku.ToLowerInvariant() == SkuOptions.Standard.ToString().ToLowerInvariant() 
                || sku.ToLowerInvariant() == SkuOptions.Basic.ToString().ToLowerInvariant())
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    class SIZEHelper
    {
        private List<string> sizeList { get; set; }
        private int maxLength = 0;
        private string clearBuffer;

        public SIZEHelper()
        {
            sizeList = new List<string>();
            clearBuffer = "";

            foreach (WebSpaceWorkerSize item in Enum.GetValues(typeof(WebSpaceWorkerSize)))
            {
                sizeList.Add(item.ToString());
                maxLength = maxLength < item.ToString().Length ? item.ToString().Length : maxLength;
            }

            for (int i = 0; i < maxLength; i++)
            {
                clearBuffer += " ";
            }
        }

        public WorkerSizeOptions getSize()
        {
            var i = 0;
            var positionX = Console.CursorLeft;
            var positionY = Console.CursorTop;

            while (true)
            {
                Console.CursorLeft = positionX;
                Console.CursorTop = positionY;
                Console.Write(sizeList.ElementAt(i));

                var index = Console.ReadKey(true);

                if (index.Key == ConsoleKey.UpArrow || index.Key == ConsoleKey.RightArrow)
                {
                    i++;
                    i = i > sizeList.Count() - 1 ? i % sizeList.Count() : i;
                }
                else if (index.Key == ConsoleKey.DownArrow || index.Key == ConsoleKey.LeftArrow)
                {
                    i--;
                    i = i < 0 ? sizeList.Count - 1 : i;
                }
                else if (index.Key == ConsoleKey.Enter)
                {
                    Console.Write("\n");
                    switch (i)
                    {
                        case 1:
                            return WorkerSizeOptions.Medium;
                        case 2:
                            return WorkerSizeOptions.Large;
                        case 0:
                            return WorkerSizeOptions.Small;
                    }
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


    //class Regions
    //{
    //    public static List<string> _regions = new List<string>
    //    {
    //        {"East Asia"},
    //        {"Southeast Asia"},
    //        {"Brazil South"},
    //        {"North Europe"},
    //        {"West Europe"},
    //        {"Japan East"},
    //        {"Japan West"},
    //        {"Central US"},
    //        {"East US"},
    //        {"East US 2"}, 
    //        {"North Central US"},
    //        {"West US"},
    //        {"South Central US"}
    //    };

    //    public static string getRegion()
    //    {
    //        var i = 0;
    //        var positionX = Console.CursorLeft;
    //        var positionY = Console.CursorTop;

    //        while (true)
    //        {
    //            Console.CursorLeft = positionX;
    //            Console.CursorTop = positionY;
    //            Console.Write(_regions.ElementAt(i));

    //            var index = Console.ReadKey(true);

    //            if (index.Key == ConsoleKey.UpArrow || index.Key == ConsoleKey.RightArrow)
    //            {
    //                i++;
    //                i = i > _regions.Count()-1 ? i % _regions.Count() : i;
    //            }
    //            else if (index.Key == ConsoleKey.DownArrow || index.Key == ConsoleKey.LeftArrow)
    //            {
    //                i--;
    //                i = i < 0 ? _regions.Count-1 : i;
    //            }
    //            else if (index.Key == ConsoleKey.Enter )
    //            {
    //                Console.Write("\n");
    //                return _regions.ElementAt(i);
    //            }
    //            else
    //            {
    //                //Other key, ignore it.
    //            }
    //            Console.CursorLeft = positionX;
    //            Console.CursorTop = positionY;
    //            Console.Write("                              ");
    //        }
    //    }

    //}



}
