using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PollenApi.Models
{
    public class PollenInfo : IPollenInfo
    {
        public String PlantName { get; set; }
        public String SourceUrl { get; set; }
        public DateTime ObservationTime { get; set; }
        public Int16 PollenLevel { get; set; }
        public String City { get; set; }
        public Int32 Id { get; set; }

        //Simple constructor
        public PollenInfo()
        {
            PollenLevel = 0;
        }

        //Overload the constructor
        public PollenInfo(String plantName, String sourceUrl, DateTime observationTime, Int16 pollenLevel, String city)
        {
            this.PlantName = plantName;
            this.SourceUrl = sourceUrl;
            this.ObservationTime = observationTime;
            this.PollenLevel = pollenLevel;
            this.City = city;
        }

        public void DoStuff()
        {
            //Do stuff
        }
    }
}
