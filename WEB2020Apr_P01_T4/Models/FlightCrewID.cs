using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WEB2020Apr_P01_T4.Models
{
    public class FlightCrewID
    {
        public int id1 { get; set; }
        public int id2 { get; set; }
        public int id3 { get; set; }
        public int id4 { get; set; }
        public int id5 { get; set; }
        public int id6 { get; set; }

        public List<int> fcID()
        {
            List<int> idList = new List<int>() { id1, id2, id3, id4, id5, id6 };
            return idList;
        }
    }
}
