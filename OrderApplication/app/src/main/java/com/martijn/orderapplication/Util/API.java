package com.martijn.orderapplication.Util;

import android.content.Context;

import com.google.gson.JsonElement;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Retrofit;
import retrofit2.converter.gson.GsonConverterFactory;
import retrofit2.http.GET;

public class API {

    private final Api api;

    public API(String baseUrl) {
        Retrofit retrofit = new Retrofit.Builder()
                .baseUrl(baseUrl)
                .addConverterFactory(GsonConverterFactory.create())
                .build();
        api = retrofit.create(Api.class);
    }

    public void testGet(Context context,
                        RequestListener<JsonElement> listener) {
        api.testGet().enqueue(new ApiCallback(context,listener));
    }

    public interface Api {
        @GET("current/3")
        Call<JsonElement> testGet();
    }

    public interface RequestListener<T> {
        void onSuccess(T response);

        void onResponse();

        void onError();
    }

    public class ApiCallback<T> implements Callback<T> {
        protected RequestListener<T> listener;
        protected Context context;

        public ApiCallback(Context context, RequestListener<T> listener) {
            this.listener = listener;
            this.context = context;
        }

        @Override
        public void onResponse(Call<T> call, retrofit2.Response<T> response) {

            listener.onResponse();
            listener.onSuccess(response.body());

        }

        @Override
        public void onFailure(Call<T> call, Throwable t) {
            listener.onResponse();
            listener.onError();
        }
    }
}