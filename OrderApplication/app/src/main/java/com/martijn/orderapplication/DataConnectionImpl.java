package com.martijn.orderapplication;

import com.jjoe64.graphview.series.DataPoint;
import com.martijn.orderapplication.Util.HttpCall;

import java.io.IOException;

public class DataConnectionImpl implements DataConnection {

    private final String productApiUrl = "https://192.168.2.45:45456/api/products";
    private final String orderApiUrl = "";
    private final String stockApiUrl = "https://192.168.2.45:45460/api/stock";


    @Override
    public Product getProduct(String barCodeOrProductNumber) {
        return null;
    }

    @Override
    public int getStock(long productId) throws IOException {
        HttpCall.getData(stockApiUrl + "/current/" + productId);
        return 0;
    }

    @Override
    public int getNextOrder(long productId) {
        return 0;
    }

    @Override
    public void addStockMutation(long productId, int amount, String reason) {

    }

    @Override
    public DataPoint[] getStockHistory(long productId, int days) {
        return new DataPoint[0];
    }

    @Override
    public DataPoint[] getMutationHistory(long productId, int days) {
        return new DataPoint[0];
    }
}
