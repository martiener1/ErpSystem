using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shared.Models
{
    public class StoreData
    {

        public int id;
        public string name;
        public string city;
        public string address;

        public StoreData(int id, string name, string city, string address)
        {
            this.id = id;
            this.name = name;
            this.city = city;
            this.address = address;
        }

    }
}
