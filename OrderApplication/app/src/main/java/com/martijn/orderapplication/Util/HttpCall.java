package com.martijn.orderapplication.Util;

import android.content.Context;
import android.util.Log;

import com.google.gson.JsonElement;

import java.io.IOException;

import okhttp3.Call;
import okhttp3.Callback;
import okhttp3.MediaType;
import okhttp3.OkHttpClient;
import okhttp3.Request;
import okhttp3.RequestBody;
import okhttp3.Response;

public class HttpCall {

    private static OkHttpClient client = new OkHttpClient();
    private static final MediaType json
            = MediaType.get("application/json; charset=utf-8");

    public static void getDataAsync(String url, Callback callback) {
        Request request = new Request.Builder()
                .url(url)
                .build();

        client = new OkHttpClient();
        Call call = client.newCall(request);
        call.enqueue(callback);
    }

    public static void getDataAsync(String url, String token, Callback callback) {
        Request request = new Request.Builder()
                .url(url)
                .addHeader("token", token)
                .build();

        Call call = client.newCall(request);
        call.enqueue(callback);
    }

    public static void postDataAsync(String url, String jsonBody, String token, Callback callback) {
        RequestBody body = RequestBody.create(json, jsonBody);
        Request request = new Request.Builder()
                .url(url)
                .addHeader("token", token)
                .post(body)
                .build();
        Call call = client.newCall(request);
        call.enqueue(callback);
    }

    public static void putDataAsync(String url, String jsonBody, String token, Callback callback) {
        RequestBody body = RequestBody.create(json, jsonBody);
        Request request = new Request.Builder()
                .url(url)
                .addHeader("token", token)
                .put(body)
                .build();
        Call call = client.newCall(request);
        call.enqueue(callback);
    }

    public static Response postData(String url, String jsonBody) {
        RequestBody body = RequestBody.create(json, jsonBody);
        Request request = new Request.Builder()
                .url(url)
                .post(body)
                .build();
        try  {
            Response response = client.newCall(request).execute();
            return response;
        } catch(Exception ex) {
            return null;
        }
    }
    public static Response postData(String url, String token, String jsonBody) {
        RequestBody body = RequestBody.create(json, jsonBody);
        Request request = new Request.Builder()
                .url(url)
                .addHeader("token", token)
                .post(body)
                .build();
        try  {
            Response response = client.newCall(request).execute();
            return response;
        } catch(Exception ex) {
            return null;
        }
    }


    public static void test(Context context) {
        //API api = new API("https://httpbin.org");
        API api = new API("https://192.168.2.45:45460/api/stock/");
        api.testGet(context, new API.RequestListener<JsonElement>() {
            @Override
            public void onSuccess(JsonElement response) {
                Log.i("RETROFIT",response.toString());
            }

            @Override
            public void onResponse() {

            }

            @Override
            public void onError() {

            }
        });
    }
}
