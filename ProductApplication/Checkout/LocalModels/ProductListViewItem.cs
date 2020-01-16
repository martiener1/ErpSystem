using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Checkout.LocalModels
{
    class ProductListViewItem : ListViewItem
    {
        public int amountOfProduct { get; private set; }
        public Product product;

        public ProductListViewItem(Product product) : base(new String[] { 1+"", product.name, product.sellingPrice + ""})
        {
            this.amountOfProduct = 1;
            this.product = product;
        }

        public void SetAmountOfProduct(int amount)
        {
            base.SubItems[0].Text = amount + "";
            amountOfProduct = amount;
        }
    }
}
