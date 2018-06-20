package com.wezz.imageserviceapp;

import android.app.Activity;
import android.app.Service;
import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;
import android.content.IntentFilter;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.net.ConnectivityManager;
import android.net.NetworkInfo;
import android.net.wifi.WifiManager;
import android.os.Environment;
import android.os.IBinder;
import android.support.annotation.Nullable;
import android.util.Log;
import android.widget.TextView;
import android.widget.Toast;

import java.io.ByteArrayOutputStream;
import java.io.File;
import java.io.FileInputStream;
import java.io.IOException;

public class ImageServiceService extends Service {
    private BroadcastReceiver wifiReceiver;

    @Nullable
    @Override
    public IBinder onBind(Intent intent)
    {
        return null;
    }
    @Override
    public void onCreate() {
        super.onCreate();
// Here put the Code of Service
    }

    public int onStartCommand(Intent intent, int flag, int startId)
    {
        Toast.makeText(this,"Service starting...", Toast.LENGTH_SHORT).show();
        intentWifi();
        return START_STICKY;
    }
    public void onDestroy() {
        unregisterReceiver(this.wifiReceiver);


        Toast.makeText(this,"Service ending...", Toast.LENGTH_SHORT).show();
    }

    public void intentWifi() {
        final IntentFilter theFilter = new IntentFilter();
        theFilter.addAction("android.net.wifi.supplicant.CONNECTION_CHANGE");
        theFilter.addAction("android.net.wifi.STATE_CHANGE");
        this.wifiReceiver = new BroadcastReceiver() {
            @Override
            public void onReceive(Context context, Intent intent) {
                WifiManager wifiManager = (WifiManager) context.getSystemService(Context.WIFI_SERVICE);

                NetworkInfo networkInfo = intent.getParcelableExtra(WifiManager.EXTRA_NETWORK_INFO);
                if (networkInfo != null) {
                    if (networkInfo.getType() == ConnectivityManager.TYPE_WIFI) {
                        //get the different network states
                        if (networkInfo.getState() == NetworkInfo.State.CONNECTED) {
                            System.out.println("WIFI IS CONNECTED");
                              startTransfer();
                        }
                        if (networkInfo.getState() == NetworkInfo.State.DISCONNECTED) {

                            System.out.println("WIFI IS NOT CONNECTED");

                        }
                        // Starting the Transfer                    }

                    }
                }

            }
        };
        // Registers the receiver so that your service will listen for
        // broadcasts
        this.registerReceiver(this.wifiReceiver, theFilter);

    }

    public void startTransfer() {
        final TcpClient client = new TcpClient(9222, "172.18.21.62");
        getPictures();
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

                    //Read the pictures from dcim.
                    File [] pictures = getPictures();
                    //Sends the picture to the socket
                    if (pictures != null) {
                        for (File pic:pictures) {
                            try {
                                FileInputStream fis = new FileInputStream(pic);
                                Bitmap bm= BitmapFactory.decodeStream(fis);
                                byte[] imgbyte = getBytesFromBitmap(bm);
                                //Sends the picture with it's here
                                client.sendPicture(pic.getName().getBytes(), imgbyte);

                            } catch (IOException e ){
                                Log.e("IO", "File input stream error:", e);

                            }

                        }
                    }
                }
            }

        }).start();
    }
    public byte[] getBytesFromBitmap(Bitmap bitmap) {
        ByteArrayOutputStream stream = new ByteArrayOutputStream();
        bitmap.compress(Bitmap.CompressFormat.PNG, 70, stream);
        return stream.toByteArray();
    }
    public File[] getPictures() {
        File dcim = Environment.getExternalStoragePublicDirectory(Environment.DIRECTORY_DCIM);
        if (dcim== null)
        {
            return null;
        }
        File[] pics = dcim.listFiles();
        System.out.println("Count of pictures is " + pics.length);
        return pics;
    }

}
