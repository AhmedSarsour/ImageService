package com.wezz.imageserviceapp;

import android.Manifest;
import android.app.ActivityManager;
import android.app.NotificationManager;
import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;
import android.content.IntentFilter;
import android.content.pm.PackageManager;
import android.net.ConnectivityManager;
import android.net.NetworkInfo;
import android.net.wifi.WifiManager;
import android.support.v4.app.ActivityCompat;
import android.support.v4.app.NotificationCompat;
import android.support.v4.content.ContextCompat;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.TextView;
import android.widget.Toast;

public class MainActivity extends AppCompatActivity {
    private BroadcastReceiver yourReceiver;
    private final int REQUEST_PERMISSION = 101; //Could be any number

    private Context context;


    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
      //  intentWifi();

        context = this;
        //Now the application request from the usr permission from reading storage
        if (ContextCompat.checkSelfPermission(context, Manifest.permission.READ_EXTERNAL_STORAGE)
                != PackageManager.PERMISSION_GRANTED) {
            ActivityCompat.requestPermissions(
                    this, new String[] { Manifest.permission.READ_EXTERNAL_STORAGE }
                    , REQUEST_PERMISSION);
        } else {
            System.out.println("Problem with giving permissions to the application\n");
        }

        Button btnStop = (Button) findViewById(R.id.btn_stop);

        Button btnStart = (Button) findViewById(R.id.btn_start);

        if (isMyServiceRunning(ImageServiceService.class)) {
            System.out.println("Service is running\n");
            btnStop.setEnabled(true);
            btnStart.setEnabled(false);
        } else {
            System.out.println("Service is not running\n");
            btnStop.setEnabled(false);
            btnStart.setEnabled(true);
        }

    }



    @Override
    public void onRequestPermissionsResult(int requestCode, String[] permissions, int[] grantResults) {
        int storagePermission = ContextCompat.checkSelfPermission(context, Manifest.permission.READ_EXTERNAL_STORAGE);
        if (storagePermission == PackageManager.PERMISSION_GRANTED) {
            finish();
            startActivity(getIntent());
        } else {
            finish();
        }
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
       // intent.setAction(C.ACTION_START_SERVICE);
        startService(intent);

        Button btnStart = (Button) findViewById(R.id.btn_start);
        btnStart.setEnabled(false);
        Button btnStop = (Button) findViewById(R.id.btn_stop);
        btnStop.setEnabled(true);

    }

    private boolean isMyServiceRunning(Class<?> serviceClass) {
        ActivityManager manager = (ActivityManager) getSystemService(Context.ACTIVITY_SERVICE);
        for (ActivityManager.RunningServiceInfo service : manager.getRunningServices(Integer.MAX_VALUE)) {
            if (serviceClass.getName().equals(service.service.getClassName())) {
                return true;
            }
        }
        return false;
    }



    }

