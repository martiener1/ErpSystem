package com.martijn.orderapplication;

import android.content.Intent;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.Gravity;
import android.view.LayoutInflater;
import android.view.MotionEvent;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.LinearLayout;
import android.widget.PopupWindow;
import android.widget.TextView;

import com.martijn.orderapplication.Util.ApiCaller;

import org.jetbrains.annotations.NotNull;

import java.io.IOException;

import okhttp3.Call;
import okhttp3.Callback;
import okhttp3.Response;

public class LoginActivity extends AppCompatActivity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_startup);
    }



    public void OnLoginButtonClicked(View view) {
        setButtonLoginEnabled(false);
        EditText editTextUsername   = (EditText)findViewById(R.id.editTextUsername);
        EditText editTextPassword   = (EditText)findViewById(R.id.editTextPassword);
        String username = editTextUsername.getText().toString();
        String password = editTextPassword.getText().toString();

        ApiCaller.GetTokenByCredentials(username, password, new Callback(){

            @Override
            public void onResponse(@NotNull Call call, @NotNull Response response) throws IOException {
                int responseCode = response.code();
                if (responseCode == 200) {
                    String token = response.body().string();
                    Intent intent = new Intent(getBaseContext(), MainActivity.class);
                    intent.putExtra("token", token);
                    startActivity(intent);
                }
                else if (responseCode == 400) {
                    displayErrorMessage("No account found with these credentials");
                }
                else {
                    displayErrorMessage("Unknown error occurred");
                }
                setButtonLoginEnabled(true);
            }

            @Override
            public void onFailure(@NotNull Call call, @NotNull IOException e) {
                displayErrorMessage("Unknown error occurred");
                setButtonLoginEnabled(true);
            }
        });
    }

    private void setButtonLoginEnabled(boolean enable) {
        runOnUiThread(new Runnable(){

            @Override
            public void run() {
                Button buttonLogin = (Button)findViewById(R.id.btnLogIn);
                buttonLogin.setEnabled(enable);
            }
        });
    }

    private void displayErrorMessage(String errorMessage) {
        runOnUiThread(new Runnable(){

            @Override
            public void run() {
                TextView errorTextView = findViewById(R.id.textViewError);
                errorTextView.setText(errorMessage);
            }
        });
    }

}
