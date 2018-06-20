package com.wezz.imageserviceapp;

import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.View;
import android.widget.EditText;
import android.widget.TextView;
import android.widget.Toast;

public class MainActivity extends AppCompatActivity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);


        }
    public void onStop(View v) {
        Toast.makeText(MainActivity.this,"Service Stopped", Toast.LENGTH_LONG).show();
    }


    public void onStart(View v) {

        Toast.makeText(MainActivity.this,"Service Started", Toast.LENGTH_LONG).show();
        final TcpClient client = new TcpClient(9222, "172.18.21.62");
        TextView edit = (TextView)findViewById(R.id.txtDebug);
        final String [] result = new String[1];

        new Thread(new Runnable() {
            @Override
            public void run() {
                 boolean connected;
                 String errorMessage;
                try {
                    client.connect();
                    connected = true;
                } catch (Exception e) {
                    connected = false;
                    System.out.println("my nigga");
                }

                if (connected) {
                    System.out.println("yo yo yo");
                }

                    //  Toast.makeText(MainActivity.this,"Problem connecting", Toast.LENGTH_LONG).show();

            }

        }).start();
    }


    }

