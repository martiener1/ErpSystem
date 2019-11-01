package com.martijn.orderapplication;

import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.View;
import android.widget.EditText;
import android.widget.RadioGroup;
import android.widget.TextView;

import com.jjoe64.graphview.GraphView;
import com.jjoe64.graphview.series.DataPoint;
import com.jjoe64.graphview.series.LineGraphSeries;
import com.martijn.orderapplication.Util.HttpCall;

public class MainActivity extends AppCompatActivity {

    private DataConnection dataConnection;
    private Product currentProduct;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        dataConnection = new DataConnectionDummy();
        HttpCall.test(this);
    }

    public void onBtnGoClick(View v) {
        String barCodeOrProductNumber = ((EditText)findViewById(R.id.inputProduct)).getText().toString();
        currentProduct = dataConnection.getProduct(barCodeOrProductNumber);
        displayProduct(currentProduct);
        redrawGraph();
    }

    public void onBtnScanClick(View v) {
        // currently not implemented
    }

    public void onBtnMutationClick(View v) {
        // currently not implemented
        // should give an pop-up where you can add a mutation
    }

    public void onRadioButtonTypeSwitch(View v) {
        redrawGraph();
    }

    public void onRadioButtonDurationSwtich(View v) {
        redrawGraph();
    }

    public void redrawGraph() {
        int days;
        int selectedDuration = ((RadioGroup)findViewById(R.id.rgDuration)).getCheckedRadioButtonId();

        if (selectedDuration == R.id.rbWeek) { days = 7;}
        else if (selectedDuration == R.id.rbMonth) { days = 30;}
        else if (selectedDuration == R.id.rbYear) { days = 365;}
        else return;

        int selectedType = ((RadioGroup)findViewById(R.id.rgType)).getCheckedRadioButtonId();
        DataPoint[] dataPoints;

        if (currentProduct == null) {
            return;
        }
        if (selectedType == R.id.rbStockHistory) {
            dataPoints = dataConnection.getStockHistory(currentProduct.id, days);
        }
        else if (selectedType == R.id.rbMutationHistory) {
            dataPoints = dataConnection.getMutationHistory(currentProduct.id, days);
        }
        else return;

        redrawGraph(dataPoints);
    }

    private void redrawGraph(DataPoint[] dataPoints) {
        GraphView graph = (GraphView) findViewById(R.id.graph);
        LineGraphSeries<DataPoint> series = new LineGraphSeries<DataPoint>(dataPoints);
        graph.removeAllSeries();
        graph.getViewport().setXAxisBoundsManual(true);
        graph.getViewport().setMaxX(dataPoints[dataPoints.length-1].getX());
        graph.addSeries(series);
    }

    private void displayProduct(Product product) {
        // fill in all the textviews
        ((TextView)findViewById(R.id.lblProductName)).setText(product.productName);
        ((TextView)findViewById(R.id.lblBrandName)).setText(product.brandName);
        ((TextView)findViewById(R.id.lblProductGroup)).setText(product.productGroup);
        ((TextView)findViewById(R.id.lblProductCategory)).setText(product.productCategory);
        Integer currentStock = getCurrentStock();
        String currentStockString = (currentStock == null) ? "N/A" : currentStock.toString();
        ((EditText) findViewById(R.id.inputStockAmount)).setText(currentStockString);
        ((EditText)findViewById(R.id.inputNextOrder)).setText(getNextOrder() + "");
    }

    private Integer getCurrentStock() {
        if (currentProduct != null) {
            try {
                return dataConnection.getStock(currentProduct.id);
            }
            catch(Exception e){
                return 66;
            }
        };
        return 100;
    }

    private int getNextOrder() {
        if (currentProduct != null) {
            return dataConnection.getNextOrder(currentProduct.id);
        };
        return -1;
    }


}
