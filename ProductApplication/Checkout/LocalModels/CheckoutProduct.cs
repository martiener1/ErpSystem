using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkout.LocalModels
{
    class CheckoutProduct
    {
        public Product product;
        public int amountOfProduct;

        public CheckoutProduct(Product product, int amountOfProduct)
        {
            this.product = product;
            this.amountOfProduct = amountOfProduct;
        }

    }
}
