using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shared.Models
{
    [Serializable]
    public class ProductGroup
    {
        public int id;
        public string name;
        public ProductCategory productCategory;

        public ProductGroup(int id, string name, ProductCategory productCategory) {
            this.id = id;
            this.name = name;
            this.productCategory = productCategory;
            }
    }
}
