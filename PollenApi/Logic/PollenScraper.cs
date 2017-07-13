using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PollenApi.Models;
using HtmlAgilityPack;

namespace PollenApi.Logic
{
    public class PollenScraper : IPollenScraper
    {
        public void UpdatePollenInfo()
        {
            
        }

        public IEnumerable<PollenInfo> GetPollenInfo(String sourceUrl, String path)
        {
            return getDmiAllPollens();
        }

        //Overload the method for now. It should later be deterministic and be called with a source url
        public IEnumerable<PollenInfo> GetPollenInfo()
        {
            return getDmiAllPollens();
        }


        //Check whether a table row (HtmlNode) contains relevant pollen information
        private bool IncludesRelevantPollenInfo(HtmlNode trNode, string[] relevantPollens)
        {
            bool include = false;
            foreach (HtmlNode tdNode in trNode.SelectNodes("./td"))
            {
                if (relevantPollens.Any(tdNode.InnerHtml.Contains))
                {
                    include = true;
                }
            }
            return include;
        }

        //Get pollen info from a table row
        private PollenInfo GetPollenInfoFromNode(HtmlNode trNode, String city, string[] relevantPollens, int id, String sourceUrl, DateTime observationTime)
        {
            PollenInfo pol = new PollenInfo();
            pol.City = city;
            pol.SourceUrl = sourceUrl;
            pol.ObservationTime = observationTime;

            //Loop through the cells in the row to get the name and count of the pollen
            foreach (HtmlNode tdNode in trNode.SelectNodes("./td"))
            {
                if (relevantPollens.Any(tdNode.InnerHtml.Contains))
                {
                    pol.PlantName = tdNode.InnerHtml;
                    pol.Id = id;
                }
                else
                {
                    Int16 pollenCount;
                    if (Int16.TryParse(tdNode.InnerHtml, out pollenCount))
                    {
                        pol.PollenLevel = pollenCount;
                    }
                }
            }
            return pol;
        }

        private List<PollenInfo> GetInnerPollenInfo(String city, HtmlNode bodyNode, string[] relevantPollens, String sourceUrl, DateTime observationTime)
        {
            var scrapedPollens = new List<PollenInfo>();
            var trNodes = bodyNode.SelectNodes("./tr");
            int counter = 0;
            //Loop through each row inside the table node
            foreach (HtmlNode trNode in trNodes)
            {
                if (trNode.SelectNodes("./td") != null)
                {
                    //If the row contains relevant pollen information, then we will include it in the list of Pollen objects
                    if (IncludesRelevantPollenInfo(trNode, relevantPollens))
                    {
                        //Break the following into a seperate private PollenInfo GetPollenInfoFromNode(HtmlNode trNode){}
                        scrapedPollens.Add(GetPollenInfoFromNode(trNode, city, relevantPollens, counter, sourceUrl, observationTime));
                    }
                    counter++;
                }
            }
            return scrapedPollens;
        }

        private List<PollenInfo> getDmiAllPollens()
        {
            string sourceUrl = "http://www.dmi.dk/vejr/sundhedsvejr/pollen/";
            string[] relevantPollens = { "Bynke", "El", "Elm", "Græs", "Birk", "Hassel" };
            var scrapedPollens = new List<PollenInfo>();
            var getHtmlWeb = new HtmlWeb();
            var document = getHtmlWeb.Load(sourceUrl);
            DateTime observationTime = DateTime.Now;
            var tableNodes = document.DocumentNode.SelectNodes("//table");

            //We want to find the two tables that contain København and Viborg, and add these table elements to a list
            foreach (HtmlNode tableNode in tableNodes)
            {
                if (tableNode.SelectNodes("./tr/th") != null) //Does this part work?
                {
                    HtmlNode thNode = tableNode.SelectNodes("./tr/th")[0];
                    if (thNode?.InnerHtml == "København" || thNode?.InnerHtml == "Viborg")
                    {
                        scrapedPollens.AddRange(GetInnerPollenInfo(thNode.InnerHtml, tableNode, relevantPollens, sourceUrl, observationTime));
                    }
                }
            }
            return scrapedPollens;
        }
    }
}
