using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shared.Models
{
    [Serializable]
    public class ProductCategory
    {
        public int id;
        public string name;
        public ProductCategory(int id, string name)
        {
            this.id = id;
            this.name = name;
        }

    }
}
