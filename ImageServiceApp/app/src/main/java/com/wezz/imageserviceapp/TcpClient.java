package com.wezz.imageserviceapp;

import android.graphics.Bitmap;
import android.graphics.BitmapFactory;

import java.text.DateFormat;
import java.text.SimpleDateFormat;
import java.util.Base64;

import android.util.Log;

import org.json.JSONObject;

import java.io.ByteArrayOutputStream;
import java.io.File;
import java.io.FileInputStream;
import java.io.IOException;
import java.io.OutputStream;
import java.io.PrintWriter;
import java.net.InetAddress;
import java.net.Socket;
import java.util.Date;
import java.util.concurrent.ExecutionException;

public class TcpClient {
    private int port;
    private String ip;
    private Socket socket= null;
    private OutputStream output;

    public TcpClient(int port, String ip) {
        this.port = port;
        this.ip = ip;
    }

    public boolean connect() {
        try {
            //here you must put your computer's IP address.
            InetAddress serverAddr = InetAddress.getByName(ip);
            //create a socket to make the connection with the server
            socket = new Socket(serverAddr, port);

        } catch (Exception e) {
            Log.e("TCP", "Client: Error", e);
            return false;
        }

        return true;

    }

    public void close() {
        //We can close only socket if it is not null
        if (this.socket != null) {
            try {
                this.socket.close();
            } catch (IOException e) {
                Log.e("TCP", "Client: Error", e);
            }
        }
    }

    //name: the name of the picture
    //image - the image itself
    public void sendPicture(File pic) {
        try {
            //Sends the mesage to the server
            output = socket.getOutputStream();
            //First send 1 in order to notify we want connection
            output.write(1);
            output.flush();
            sendString(pic.getName());

            FileInputStream fis = new FileInputStream(pic);
            Bitmap bm= BitmapFactory.decodeStream(fis);
            byte[] imgbyte = getBytesFromBitmap(bm);


//            //Sends the length of the picture file bytes
//            //We convert to string because the length will be huge so we will send it by array of bytes.
            String imageSize = imgbyte.length + "";
            System.out.println("Size of the image is " + imgbyte.length);
            byte [] size  = imageSize.getBytes();
//            //Getting the size of the size
            output.write(size.length);
            output.flush();
            //Sending the size now
            output.write(size);
            output.flush();
            //Sends the picture itself
            output.write(imgbyte);
            output.flush();
           // DateFormat formatter = new SimpleDateFormat("dd/MM/yyyy");
            DateFormat df = new SimpleDateFormat("MM/dd/yyyy HH:mm:ss");

            //Date date = formatter.parse(formatter.format(pic.lastModified()));

            //Now we will send the creation time of the picture
            System.out.println("The date is " + df.format(pic.lastModified()));
            //Sending the creation date
            sendString(df.format(pic.lastModified()));


        } catch (Exception e) {
            Log.e("TCP", "SERVER:Error", e);
        }
    }

    private byte[] getBytesFromBitmap(Bitmap bitmap) {
        ByteArrayOutputStream stream = new ByteArrayOutputStream();
        bitmap.compress(Bitmap.CompressFormat.PNG, 70, stream);
        return stream.toByteArray();
    }


    private void sendString(String str) {
        byte [] size = (str.length() + "").getBytes();
        try {
            //Sends the size of the string
            output.write(size.length);
            output.write(size);
            output.flush();
            //Sends the string
            output.write(str.getBytes());
            output.flush();
        } catch (IOException e) {
            System.out.println("Problem sending the picture");
        }
    }




}
