package com.wezz.imageserviceapp;

import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;
import android.content.IntentFilter;
import android.net.ConnectivityManager;
import android.net.NetworkInfo;
import android.net.wifi.WifiManager;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.TextView;
import android.widget.Toast;

public class MainActivity extends AppCompatActivity {
private BroadcastReceiver yourReceiver;
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
      //  intentWifi();


        }
    public void onStopService(View v) {
       // Toast.makeText(MainActivity.this,"Service Stopped", Toast.LENGTH_LONG).show();
        Intent intent= new Intent(this, ImageServiceService.class);
        stopService(intent);
        //After we stopped we can start
        Button btnStop = (Button) findViewById(R.id.btn_stop);
        btnStop.setEnabled(false);

        Button btnStart = (Button) findViewById(R.id.btn_start);
        btnStart.setEnabled(true);
    }


    public void onStartService(View v) {

        Intent intent= new Intent(this, ImageServiceService.class);
        startService(intent);

        Button btnStart = (Button) findViewById(R.id.btn_start);
        btnStart.setEnabled(false);
        Button btnStop = (Button) findViewById(R.id.btn_stop);
        btnStop.setEnabled(true);

    }



    }

