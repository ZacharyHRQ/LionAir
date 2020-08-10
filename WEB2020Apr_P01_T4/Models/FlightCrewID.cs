using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WEB2020Apr_P01_T4.Models
{
    public class FlightCrewID
    {
        public int ID1 { get; set; }
        public int ID2 { get; set; }
        public int ID3 { get; set; }
        public int ID4 { get; set; }
        public int ID5 { get; set; }
        public int ID6 { get; set; }

        public List<int> fcID()
        {
            List<int> idList = new List<int>() { ID1, ID2, ID3, ID4, ID5, ID6 };
            return idList;
        }
    }
}
