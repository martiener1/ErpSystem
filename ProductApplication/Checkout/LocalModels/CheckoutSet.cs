using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkout.LocalModels
{
    class CheckoutSet
    {
        private List<CheckoutProduct> products;

        public CheckoutSet()
        {
            products = new List<CheckoutProduct>();
        }

        public void AddProduct(CheckoutProduct product)
        {
            products.Add(product);
        }

        public decimal CalculateTotalCost()
        {
            decimal totalCount = 0;
            foreach (CheckoutProduct product in products)
            {
                totalCount += (decimal)(product.amountOfProduct * product.product.sellingPrice);
            }
            return totalCount;
        }

        public Receipt GetReceipt()
        {
            return null;
        }

    }
}
