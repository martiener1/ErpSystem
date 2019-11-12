using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shared.Models
{
    [Serializable]
    public class Product
    {

        public long id;
        public int storeId;
        public string name;
        public string brand;
        public double? buyingPrice;
        public double? sellingPrice;
        public string supplier;
        public string productNumber;
        public string ean;
        public ProductGroup productGroup;

        public Product ()
        {
        }

        public Product (long id, int storeId, string name, string brand, double? buyingPrice, double? sellingPrice, string supplier, string productNumber, string ean, ProductGroup productGroup)
        {
            this.id = id;
            this.storeId = storeId;
            this.name = name;
            this.brand = brand;
            this.buyingPrice = buyingPrice;
            this.sellingPrice = sellingPrice;
            this.supplier = supplier;
            this.productNumber = productNumber;
            this.ean = ean;
            this.productGroup = productGroup;
        }

        public override bool Equals(object obj)
        {
            Product product = obj as Product;
            if (product == null) return false;

            return this.storeId == product.storeId &&
                    this.name == product.name &&
                    this.brand == product.brand &&
                    this.buyingPrice == product.buyingPrice &&
                    this.sellingPrice == product.sellingPrice &&
                    this.supplier == product.supplier &&
                    this.productNumber == product.productNumber &&
                    this.ean == product.ean &&
                    this.productGroup.name == product.productGroup.name;
        }
    }
}
