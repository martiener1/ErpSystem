package com.martijn.orderapplication;

import com.jjoe64.graphview.series.DataPoint;

import java.io.IOException;

public interface DataConnection {

    public Product getProduct(String barCodeOrProductNumber);

    public int getStock(long productId) throws IOException;

    public int getNextOrder(long productId);

    public void addStockMutation(long productId, int amount, String reason);

    public DataPoint[] getStockHistory(long productId, int days);

    public DataPoint[] getMutationHistory(long productId, int days);

}
