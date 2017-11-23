using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PollenApi.Logic;
using PollenApi.Models;

namespace PollenApi.Controllers
{
    /* Work on the routing. We want a structure like this:
     * api/pollen/{site}/{location}/{plantcode} ... e.g.
     * api/pollen/dmi/viborg/birk -->Return all values for Dmi/Viborg/Birk
     * api/pollen/dmi/viborg -->Return all values for Dmi/Viborg
     * api/pollen/dmi -->Return all values for Dmi
     * 
     * 
     */

    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private IEnumerable<PollenInfo> GenerateSomeData()
        {
            PollenInfo p1 = new PollenInfo();
            p1.PlantName = "Birk";
            p1.SourceUrl = "www.dmi.dk";
            p1.ObservationTime = DateTime.Now;
            p1.PollenLevel = 25;
            p1.City = "København";

            PollenInfo p2 = new PollenInfo("Græs", "www.dmi.dk", DateTime.Now, 10, "København");

            List<PollenInfo> pollenObjects = new List<PollenInfo>();
            pollenObjects.Add(p1);
            pollenObjects.Add(p2);

            return pollenObjects;
        }

        // GET api/values
        [HttpGet]
        public IEnumerable<PollenInfo> GetAllPollen()
        {
            PollenScraper p = new PollenScraper();
            string[] relevantPollens = { "Bynke", "El", "Elm", "Græs", "Birk", "Hassel" };
            p.RelevantPollens = relevantPollens;
            return p.GetPollenInfo();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
