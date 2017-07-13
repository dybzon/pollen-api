using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PollenApi.Models;

namespace PollenApi.Logic
{
    interface IPollenScraper
    {
        void UpdatePollenInfo();
        IEnumerable<PollenInfo> GetPollenInfo(String sourceUrl, String path);
    }
}
