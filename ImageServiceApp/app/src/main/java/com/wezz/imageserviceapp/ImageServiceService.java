package com.wezz.imageserviceapp;

import android.app.Activity;
import android.app.AlarmManager;
import android.app.NotificationManager;
import android.app.PendingIntent;
import android.app.Service;
import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;
import android.content.IntentFilter;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.media.Image;
import android.net.ConnectivityManager;
import android.net.NetworkInfo;
import android.net.wifi.WifiManager;
import android.os.Environment;
import android.os.IBinder;
import android.os.SystemClock;
import android.support.annotation.Nullable;
import android.support.v4.app.NotificationCompat;
import android.util.Log;
import android.view.View;
import android.widget.Button;
import android.widget.TextView;
import android.widget.Toast;

import java.io.ByteArrayOutputStream;
import java.io.File;
import java.io.FileFilter;
import java.io.FileInputStream;
import java.io.IOException;
import java.util.ArrayList;
import java.util.List;

public class ImageServiceService extends Service {
    private BroadcastReceiver wifiReceiver;
    private final TcpClient client = new TcpClient(9222, "10.100.102.12");
    private Double currentPercent = 0.0;
    @Nullable
    @Override
    public IBinder onBind(Intent intent) {
        return null;
    }

    @Override
    public void onCreate() {
        super.onCreate();
// Here put the Code of Service
    }

    public int onStartCommand(Intent intent, int flag, int startId) {
        Toast.makeText(this, "Service starting...", Toast.LENGTH_SHORT).show();
        intentWifi();
        return START_STICKY;
    }

    public void onDestroy() {
        unregisterReceiver(this.wifiReceiver);
        //  client.close();

        Toast.makeText(this, "Service ending...", Toast.LENGTH_SHORT).show();
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

                    }
                }

            }
        };
        // Registers the receiver so that your service will listen for
        // broadcasts
        this.registerReceiver(this.wifiReceiver, theFilter);

    }


    public void startTransfer() {

        //    final TcpClient client = new TcpClient(9222, "172.18.21.62");
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
                }

                if (connected) {
                    //Read the pictures from dcim.
                    final List<File> pictures = getPictures();
                    //Sends the picture to the socket
                    if (pictures != null) {
                        int countPictures = pictures.size();
                        System.out.println("We have " + countPictures + " pictures");
                        Double percent = (1.0 / countPictures) * 100;
                        displayNotification(pictures,percent);
                    }
                    //Notify to server we just finished to send pictures
                }
            }

        }).start();
    }
    public boolean isPicture(File file) {
        final String[] okFileExtensions = new String[]{"jpg", "png", "gif", "jpeg"};
        for (String extension : okFileExtensions) {
            if (file.getName().toLowerCase().endsWith(extension)) {
                return true;
            }
        }
        return false;

    }
//Getting all the pictures from the dcip
    public List<File> getPictures() {
        File dcim = Environment.getExternalStoragePublicDirectory(Environment.DIRECTORY_DCIM);
        if (dcim == null) {
            return null;
        }
        List<File> pics = new ArrayList<File>();
        listPictures(dcim , pics);

        return pics;
    }
//Getting all the pictures in the dcim and subfolders of it
    public void listPictures(File directory, List<File> files) {
        // Get all the files from a directory.
        File[] fList = directory.listFiles();
        for (File file : fList) {
            //Check if file is picture
            if (file.isFile() && isPicture(file)) {
                files.add(file);
            } else if (file.isDirectory()) {
                listPictures(new File(file.getAbsolutePath()), files);
            }
        }
    }



    public void displayNotification(final List<File> pictures, Double percent) {
        //it shows here that NotificationCompat is deprecated and must repalce with NotifcationCompat.builder(this, channelId)
        //dunno whatis the channelID...
        final NotificationCompat.Builder builder = new NotificationCompat.Builder(this);
        final int notify_id = 1;
        final NotificationManager NM = (NotificationManager) getSystemService(Context.NOTIFICATION_SERVICE);
        //builder.setSmallIcon(R.drawable.ic_launcher_background);
        builder.setSmallIcon(R.mipmap.ic_launcher);//ic_launcher doesn't exit for me ...dunno why.
        builder.setContentTitle("Started moving the photos...");
        builder.setContentText("Moving is in progress...");
        final Double per = percent;
        new Thread(new Runnable() {
            @Override
            public void run() {
                builder.setProgress(100, 0, false);
                NM.notify(notify_id, builder.build());

                for (File pic : pictures) {
                    client.sendPicture(pic);
                    currentPercent += per;

                    builder.setProgress(100, currentPercent.intValue(), false);
                    NM.notify(notify_id, builder.build());
                    try {
                        Thread.sleep(2000);
                    } catch (InterruptedException e) {
                        e.printStackTrace();
                    }
                }
                client.finishPictures();
                builder.setProgress(0, 0, false);
                builder.setContentText("Download Complete...");
                NM.notify(notify_id, builder.build());
            }
        }).start();
    }
}
