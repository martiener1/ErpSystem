package com.martijn.orderapplication.Models;

public class Product {

    public int id;
    public int storeId;
    public String name;
    public String brand;
    public double buyingPrice;
    public double sellingPrice;
    public String supplier;
    public String productNumber;
    public String ean;
    public ProductGroup productGroup;

    public Product() {

    }
}