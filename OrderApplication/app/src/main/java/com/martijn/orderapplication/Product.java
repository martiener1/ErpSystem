package com.martijn.orderapplication;

public class Product {

    public long id;
    public String productNumber;
    public String productName;
    public String brandName;
    public String productGroup;
    public String productCategory;

    public Product() {

    }

    public Product(long id, String productNumber, String productName, String brandName, String productGroup, String productCategory) {
        this.id = id;
        this.productNumber = productNumber;
        this.productName = productName;
        this.brandName = brandName;
        this.productGroup = productGroup;
        this.productCategory = productCategory;
    }
}
