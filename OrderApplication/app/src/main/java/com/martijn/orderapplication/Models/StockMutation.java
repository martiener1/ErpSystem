package com.martijn.orderapplication.Models;

import java.util.Date;

public class StockMutation {
    public int id = 1;
    public int storeId = 1;
    public int productId;
    public int amount;
    public MutationReason reason;
    public Date dateTime;

    public StockMutation() {

    }

    public StockMutation(int productId, int amount, MutationReason reason, Date dateTime) {
        this.productId = productId;
        this.amount = amount;
        this.reason = reason;
        this.dateTime = dateTime;
    }
}
