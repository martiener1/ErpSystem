using Checkout.LocalModels;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Checkout
{
    public partial class Form1 : Form
    {

        private ProductListViewItem selectedProduct = null;

        public Form1()
        {
            InitializeComponent();
            Product product = new Product(1, 1, "name1", "brand1", 1d, 2.5d, "supplier1", "123456", "8718123456721", new ProductGroup(1, "group1", new ProductCategory(1, "category1")));
            var item = new ProductListViewItem(product);
            listView1.Items.Add(item);
        }

        private void registerPaydToStoreAPI()
        {
            // create CheckoutSet
            // send checkoutSet to CheckoutAPI
        }

        private void DoTransaction(Decimal amount)
        {
            ExternalPaymentSystem.DoPayment(amount);
        }

        private void addProductByBarcode(string barcode)
        {
            //get product from productapi
            // create correct productlistviewitem
            // add that to the listview
            // select the item that just got added
            updateShowTotalPrice();
        }

        private decimal calculateTotalPrice()
        {
            decimal totalCount = 0;
            foreach (ProductListViewItem item in listView1.Items)
            {
                totalCount += (decimal)(item.amountOfProduct * item.product.sellingPrice);
            }
            return totalCount;
        }

        private void updateShowTotalPrice()
        {
            decimal totalCount = calculateTotalPrice();
            lblTotalPrice.Text = String.Format("€ {0:0.00}", totalCount);
        }

        private void showSelectedItem(ProductListViewItem itemToShow)
        {
            lblProduct.Text = itemToShow.product.name;
            lblBrand.Text = itemToShow.product.brand;
            nudAmount.Value = itemToShow.amountOfProduct;
            nudAmount.Enabled = true;
            btnRemove.Enabled = true;
        }

        private void clearSelectedItem()
        {
            lblProduct.Text = "Product";
            lblBrand.Text = "Brand";
            nudAmount.Value = 1;
            nudAmount.Enabled = false;
            btnRemove.Enabled = false;
        }


        private void clearTextBoxBarcode()
        {
            textBox1.Text = "";
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            clearTextBoxBarcode();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                addProductByBarcode(textBox1.Text);
                clearTextBoxBarcode();
            }
        }

        private void listView1_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (e.IsSelected)
            {
                selectedProduct = (ProductListViewItem)e.Item;
                showSelectedItem(selectedProduct);
            } else
            {
                selectedProduct = null;
                clearSelectedItem();
            }
        }

        private void nudAmount_ValueChanged(object sender, EventArgs e)
        {
            if (selectedProduct == null)
            {
                return;
            }
            int newAmount = Decimal.ToInt32(nudAmount.Value);
            selectedProduct.SetAmountOfProduct(newAmount);
            updateShowTotalPrice();
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            listView1.Items.Remove(selectedProduct);
        }

        private void btnPay_Click(object sender, EventArgs e)
        {
            DoTransaction(calculateTotalPrice());
            registerPaydToStoreAPI();
            //TODO: print or show receipt
        }
    }
}
