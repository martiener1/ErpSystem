package com.martijn.orderapplication.Util;

import com.google.gson.Gson;
import com.martijn.orderapplication.Models.StockMutation;

import okhttp3.Callback;

public class ApiCaller {

    private static final String baseUrlLoginApi = "https://martijnloginapi.azurewebsites.net/api/login/";
    private static final String baseUrlProductApi = "https://martijnproductapi.azurewebsites.net/api/products/";
    private static final String baseUrlStockApi = "https://martijnstockapi.azurewebsites.net/api/stock/";
    private static final String baseUrlOrderApi = "https://martijnstockapi.azurewebsites.net/api/orders/";

    public static void GetTokenByCredentials(String username, String password, Callback callback) {
        String url = baseUrlLoginApi + "login/" + username + "/" + password;
        HttpCall.getDataAsync(url, callback);
    }

    public static void GetProductByProductNumber(String productNumber, String token, Callback callback) {
        String url = baseUrlProductApi + "productnumber/" + productNumber;
        HttpCall.getDataAsync(url, token, callback);
    }

    public static void GetProductByBarcode(String barcode, String token, Callback callback) {
        String url = baseUrlProductApi + "ean/" + barcode;
        HttpCall.getDataAsync(url, token, callback);
    }

    public static void GetNextOrder(int productId, String token, Callback callback) {
        String url = baseUrlOrderApi + "nextorder/" + productId;
        HttpCall.getDataAsync(url, token, callback);
    }

    public static void SaveNextOrder(int productId, int nextOrderAmount, String token, Callback callback){
        String url = baseUrlOrderApi + "nextorder/" + productId + "/" + nextOrderAmount;
        HttpCall.putDataAsync(url, "", token, callback);
    }

    public static void GetCurrentStock(int productId, String token, Callback callback) {
        String url = baseUrlStockApi + "current/" + productId;
        HttpCall.getDataAsync(url, token, callback);
    }

    public static void GetStockHistory(int productId, int durationInDays, String token, Callback callback) {
        String url = baseUrlStockApi + "history/" + productId + "/" + durationInDays;
        HttpCall.getDataAsync(url, token, callback);
    }

    public static void postStockMutation(StockMutation mutation, String token, Callback callback) {
        String url = baseUrlStockApi + "mutations";
        String json = (new Gson()).toJson(mutation);
        HttpCall.postDataAsync(url, json, token, callback);

    }

}
