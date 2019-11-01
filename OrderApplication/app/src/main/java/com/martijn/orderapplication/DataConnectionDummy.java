package com.martijn.orderapplication;

import com.jjoe64.graphview.series.DataPoint;

import java.io.IOException;

public class DataConnectionDummy implements DataConnection {
    @Override
    public Product getProduct(String barCodeOrProductNumber) {

        if (barCodeOrProductNumber.equals("1")) {
            return new Product(3, "123456", "Hamer", "Stanley", "HandGereedschap", "Gereedschap");
        }
        else {
            return new Product(94844118, "654987", "Supersonic 3000", "Philips", "Droger", "Witgoed");
        }
    }

    @Override
    public int getStock(long productId) throws IOException {
        return new DataConnectionImpl().getStock(productId);
        //return 8;
    }

    @Override
    public int getNextOrder(long productId) {
        return 2;
    }

    @Override
    public void addStockMutation(long productId, int amount, String reason) {

    }

    @Override
    public DataPoint[] getStockHistory(long productId, int days) {

        DataPoint[] dataPoints = new DataPoint[days];
        for (int i = 0; i < days; i++) {
            dataPoints[i] = new DataPoint(i+1, i%8 + 2);
        }

        return dataPoints;

    }

    @Override
    public DataPoint[] getMutationHistory(long productId, int days) {
        DataPoint[] dataPoints = new DataPoint[days];
        for (int i = 0; i < days; i++) {
            dataPoints[i] = new DataPoint(i+1, i%138 + 2);
        }

        return dataPoints;
    }
}
