package com.martijn.orderapplication;

import android.graphics.Color;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.util.Log;
import android.view.Gravity;
import android.view.LayoutInflater;
import android.view.MotionEvent;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.LinearLayout;
import android.widget.PopupWindow;
import android.widget.RadioGroup;
import android.widget.TextView;

import com.google.gson.Gson;
import com.jjoe64.graphview.GraphView;
import com.jjoe64.graphview.series.DataPoint;
import com.jjoe64.graphview.series.LineGraphSeries;
import com.martijn.orderapplication.Models.Product;
import com.martijn.orderapplication.Models.StockMutation;
import com.martijn.orderapplication.Util.ApiCaller;

import org.jetbrains.annotations.NotNull;

import java.io.IOException;

import okhttp3.Call;
import okhttp3.Callback;
import okhttp3.Response;

public class MainActivity extends AppCompatActivity {

    private String token;
    private Product currentProduct;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        this.token = getIntent().getStringExtra("token");
        setContentView(R.layout.activity_main);
    }

    public void onBtnGoClick(View v) {
        String barcodeOrProductNumber = ((EditText)findViewById(R.id.inputProduct)).getText().toString();
        getProductByProductnumberOrBarcode(barcodeOrProductNumber);
    }

    public void onBtnScanClick(View v) {
        // currently not implemented
        displayWarningMessage("Not yet implemented");
    }

    public void onBtnMutationClick(View v) {
        displayWarningMessage("Not yet implemented");
        // currently not implemented
        // should give an pop-up where you can add a mutation
        StockMutation stockMutation = new StockMutation();
        saveNewMutation(stockMutation);
    }

    public void onBtnSaveNextOrder(View v) {
        saveNextOrder();
    }

    public void onRadioButtonTypeSwitch(View v) {
        calculateGraph(currentProduct.id);
    }

    public void onRadioButtonDurationSwtich(View v) {
        calculateGraph(currentProduct.id);
    }

    private void getProductByProductnumberOrBarcode(String barcodeOrProductNumber) {
        setAllButtonsEnable(false);
        ApiCaller.GetProductByProductNumber(barcodeOrProductNumber, token , new Callback(){
            @Override
            public void onResponse(@NotNull Call call, @NotNull Response response) throws IOException {
                int responseCode = response.code();
                if (responseCode == 200) {
                    String json = response.body().string();
                    Gson gson = new Gson();
                    Product product = gson.fromJson(json, Product.class);
                    currentProduct = product;
                    displayProduct(currentProduct);
                }
                else if (responseCode == 401) {
                    sendUserBackToLoginActitivy();
                }
                else if (responseCode == 404) {
                    // no product found by productnumber
                    getProductByBarcode(barcodeOrProductNumber);
                }
                else {
                    displayErrorMessage("Unknown error occurred, GetProductByProductNumber");
                }
            }

            @Override
            public void onFailure(@NotNull Call call, @NotNull IOException e) {
                setAllButtonsEnable(true);
                displayErrorMessage("Unknown error occurred, GetProductByProductNumber");
            }
        });
    }

    private void getProductByBarcode(String barcode) {
        ApiCaller.GetProductByBarcode(barcode, token, new Callback(){
            @Override
            public void onResponse(@NotNull Call call, @NotNull Response response) throws IOException {
                int responseCode = response.code();
                if (responseCode == 200) {
                    // read response and change currentProduct
                    String json = response.body().string();
                    Gson gson = new Gson();
                    Product product = gson.fromJson(json, Product.class);
                    currentProduct = product;
                }
                else if (responseCode == 401) {
                    sendUserBackToLoginActitivy();
                }
                else if (responseCode == 404) {
                    // no product found by productnumber or barcode
                    displayErrorMessage("No product found with inputted productnumber or barcode");
                }
                else {
                    displayErrorMessage("Unknown error occurred, GetProductByBarcode");
                }
            }

            @Override
            public void onFailure(@NotNull Call call, @NotNull IOException e) {
                displayErrorMessage("Unknown error occurred, GetProductByBarcode");
            }
        });
    }

    private void displayNormalMessage(String errorMessage) {
        showPopUp(errorMessage, Log.VERBOSE);
    }

    private void displayWarningMessage(String errorMessage) {
        showPopUp(errorMessage, Log.WARN);
    }

    private void displayErrorMessage(String errorMessage) {
        showPopUp(errorMessage, Log.ERROR);
    }

    private void showPopUp(String message, int logType) {
        int color;
        if (logType == Log.ERROR) {
            color = Color.RED;
        }
        else if (logType == Log.WARN) {
            color = Color.parseColor("#FFA500");
        }
        else  {
            color = Color.WHITE;
        }
        showPopupWindow(message, color);
    }

    private void showPopupWindow(String message, int color) {
        runOnUiThread(new Runnable(){

            @Override
            public void run() {
                // inflate the layout of the popup window
                LayoutInflater inflater = (LayoutInflater)
                        getSystemService(LAYOUT_INFLATER_SERVICE);
                View popupView = inflater.inflate(R.layout.popup_window, null);

                // create the popup window
                int width = LinearLayout.LayoutParams.WRAP_CONTENT;
                int height = LinearLayout.LayoutParams.WRAP_CONTENT;
                boolean focusable = true; // lets taps outside the popup also dismiss it
                final PopupWindow popupWindow = new PopupWindow(popupView, width, height, focusable);
                TextView textViewMessage = popupView.findViewById(R.id.textViewPopUpMessage);
                textViewMessage.setText(message);
                textViewMessage.setTextColor(color);

                // show the popup window
                // which view you pass in doesn't matter, it is only used for the window tolken
                popupWindow.showAtLocation(findViewById(android.R.id.content).getRootView() , Gravity.CENTER, 0, 0);

                // dismiss the popup window when touched
                popupView.setOnTouchListener(new View.OnTouchListener() {
                    @Override
                    public boolean onTouch(View v, MotionEvent event) {
                        popupWindow.dismiss();
                        return true;
                    }
                });
            }
        });

    }

    private void saveNextOrder() {
        EditText nextOrderTextView = (EditText)findViewById(R.id.inputNextOrder);
        int productId = currentProduct.id;
        int nextOrderAmount = Integer.parseInt(nextOrderTextView.getText().toString());

        ApiCaller.SaveNextOrder(productId, nextOrderAmount, token, new Callback(){
            @Override
            public void onResponse(@NotNull Call call, @NotNull Response response) throws IOException {
                int responseCode = response.code();
                if (responseCode == 200) {
                    // success
                    displayNormalMessage("NextOrder changed successfully");
                }
                else if (responseCode == 201) {
                    // did not exist, but it is created, success
                    displayNormalMessage("NextOrder did not exist, created it successfully");
                }
                else if (responseCode == 401) {
                    sendUserBackToLoginActitivy();
                }
                else {
                    displayErrorMessage("Unknown error occurred, SaveNextOrder");
                }
            }

            @Override
            public void onFailure(@NotNull Call call, @NotNull IOException e) {
                displayErrorMessage("Unknown error occurred, SaveNextOrder");
            }
        });
    }

    public void calculateGraph(int productId) {
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
            getAndRedrawStockHistoryGraph(productId, days);
        }
        else if (selectedType == R.id.rbMutationHistory) {
            getAndRedrawMutationHistoryGraph(productId, days);
        }
        else return;
    }

    private void getAndRedrawStockHistoryGraph(int productId, int durationInDays) {
        setAllButtonsEnable(false);
        ApiCaller.GetStockHistory(productId, durationInDays, token, new Callback(){
            @Override
            public void onResponse(@NotNull Call call, @NotNull Response response) throws IOException {
                int responseCode = response.code();
                if (responseCode == 200) {
                    // success
                    String json = response.body().string();
                    int[] stockArray = (new Gson()).fromJson(json, int[].class);
                    DataPoint[] dataPoints = new DataPoint[stockArray.length];
                    for (int i = 0; i < stockArray.length; i++) {
                        dataPoints[i] = new DataPoint(i, stockArray[i]);
                    }
                    redrawGraph(dataPoints);
                }
                else if (responseCode == 401) {
                    sendUserBackToLoginActitivy();
                }
                else {
                    displayErrorMessage("Unknown error occurred, GetStockHistory");
                }
                setAllButtonsEnable(true);
            }

            @Override
            public void onFailure(@NotNull Call call, @NotNull IOException e) {
                displayErrorMessage("Unknown error occurred, GetStockHistory");
                setAllButtonsEnable(true);
            }
        });
    }

    private void getAndRedrawMutationHistoryGraph(int productId, int durationInDays) {
        //TODO: this method
        // Not for now, it might be useless for users
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
        if (product == null) { return; }
        runOnUiThread(new Runnable() {
            @Override
            public void run() {
                ((TextView)findViewById(R.id.lblProductName)).setText(product.name);
                ((TextView)findViewById(R.id.lblBrandName)).setText(product.brand);
                ((TextView)findViewById(R.id.lblProductGroup)).setText(product.productGroup.name);
                ((TextView)findViewById(R.id.lblProductCategory)).setText(product.productGroup.productCategory.name);
            }
        });
        getAndDisplayCurrentStock(product.id);
        calculateAndDisplayNextOrder(product.id);
        calculateGraph(product.id);
    }

    private void getAndDisplayCurrentStock(int productId) {
        if (currentProduct != null) {
            // get currentStock
            // display currentStock
            ApiCaller.GetCurrentStock(productId, token, new Callback(){

                @Override
                public void onResponse(@NotNull Call call, @NotNull Response response) throws IOException {
                    int responseCode = response.code();
                    if (responseCode == 200) {
                        // read response and change currentProduct
                        int currentStockAmount = Integer.parseInt(response.body().string());
                        setCurrentStockAmount(currentStockAmount);
                    }
                    else if (responseCode == 401) {
                        sendUserBackToLoginActitivy();
                    }
                    else if (responseCode == 404) {
                        // no currentStock found by productId
                        setCurrentStockAmount(null);
                        displayErrorMessage("Current stock could not be found or calculated");
                    }
                    else {
                        setCurrentStockAmount(null);
                        displayErrorMessage("Unknown error occurred, GetCurrentStock");
                    }
                }

                @Override
                public void onFailure(@NotNull Call call, @NotNull IOException e) {
                    setCurrentStockAmount(null);
                    displayErrorMessage("Unknown error occurred");
                }
            });
        }

    }

    private void calculateAndDisplayNextOrder(int productId) {
        ApiCaller.GetNextOrder(productId, token, new Callback(){
            @Override
            public void onResponse(@NotNull Call call, @NotNull Response response) throws IOException {
                int responseCode = response.code();
                if (responseCode == 200) {
                    // read response and change currentProduct
                    int nextOrderAmount = Integer.parseInt(response.body().string());
                    setNextOrderAmount(nextOrderAmount);
                }
                else if (responseCode == 401) {
                    sendUserBackToLoginActitivy();
                }
                else if (responseCode == 404) {
                    // no nextorder found by productId
                    setNextOrderAmount(0);
                    //displayErrorMessage("Next order could not be found");
                }
                else {
                    setNextOrderAmount(0);
                    displayErrorMessage("Unknown error occurred, GetNextOrder");
                }
            }

            @Override
            public void onFailure(@NotNull Call call, @NotNull IOException e) {
                setNextOrderAmount(0);
                displayErrorMessage("Unknown error occurred");
            }
        });
    }

    private void getNewMutationFromUser() {
        // pop up to get information
        // construct stockMutation
        // saveNewMutation()
    }

    private void saveNewMutation(StockMutation mutation) {

        ApiCaller.postStockMutation(mutation, token, new Callback(){
            @Override
            public void onResponse(@NotNull Call call, @NotNull Response response) throws IOException {
                int responseCode = response.code();
                if (responseCode == 200) {

                }
                else if (responseCode == 400) {
                    displayErrorMessage("Could not process inputted stockMutation");
                }
                else if (responseCode == 401) {
                    sendUserBackToLoginActitivy();
                }
                else {
                    displayErrorMessage("Unknown error occurred, PostNewStockMutation");
                }
            }

            @Override
            public void onFailure(@NotNull Call call, @NotNull IOException e) {
                setNextOrderAmount(0);
                displayErrorMessage("Unknown error occurred");
            }
        });
    }

    private void setAllButtonsEnable(boolean enable){
        runOnUiThread(new Runnable(){

            @Override
            public void run() {
                findViewById(R.id.btnGo).setEnabled(enable);
                findViewById(R.id.btnScan).setEnabled(enable);
                findViewById(R.id.btnStockMutation).setEnabled(enable);
                findViewById(R.id.btnSaveNextOrder).setEnabled(enable);
                findViewById(R.id.rbStockHistory).setEnabled(enable);
                findViewById(R.id.rbMutationHistory).setEnabled(enable);
                findViewById(R.id.rbWeek).setEnabled(enable);
                findViewById(R.id.rbMonth).setEnabled(enable);
                findViewById(R.id.rbYear).setEnabled(enable);
            }
        });
    }

    private void setCurrentStockAmount(Integer amount) {
        runOnUiThread(new Runnable() {
            @Override
            public void run() {
                EditText editTextCurrentStock = (findViewById(R.id.inputStockAmount));
                editTextCurrentStock.setText(amount + "");
            }
        });
    }

    private void setNextOrderAmount(int amount) {
        runOnUiThread(new Runnable() {

            @Override
            public void run() {
                EditText editTextNextOrder = (EditText)findViewById(R.id.inputNextOrder);
                editTextNextOrder.setText(amount + "");
            }
        });
    }

    private void sendUserBackToLoginActitivy() {
        // show pop up first, then redirect user back to loginscreen
        displayErrorMessage("Session timed out, please log in again, GetProductByBarcode");

        //Intent intent = new Intent(getBaseContext(), LoginActivity.class);
        //startActivity(intent);
    }


}
