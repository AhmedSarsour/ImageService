package com.wezz.imageserviceapp;

import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
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

    public void connect() throws Exception {
        try {
            //here you must put your computer's IP address.
            InetAddress serverAddr = InetAddress.getByName(ip);
            //create a socket to make the connection with the server
            socket = new Socket(serverAddr, port);

        } catch (Exception e) {
            Log.e("TCP", "Client: Error", e);
            throw new Exception(e);
        }
    }

    public void close() {
        try {
            this.socket.close();
        } catch (IOException e) {
            Log.e("TCP", "Client: Error", e);
        }
    }

    //name: the name of the picture
    //image - the image itself
    public void sendPicture(File pic) {
        byte [] name = pic.getName().getBytes();
        try {
            //Sends the mesage to the server
            output = socket.getOutputStream();
            //First send 1 in order to notify we want connection
            output.write(1);
            output.flush();
            //Sends the picture name
            //Sends the length of picture name
            //     output.write((name.length + "").getBytes());
            byte [] sizeName = (name.length + "").getBytes();
            //Sends the size of size name
            output.write(sizeName.length);
            output.write(sizeName);
            output.flush();
            System.out.println("We want to move " + name.length + "bytes");
            //Sends the name
            output.write(name);
            output.flush();

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

        } catch (Exception e) {
            Log.e("TCP", "SERVER:Error", e);
        }
    }

    private byte[] getBytesFromBitmap(Bitmap bitmap) {
        ByteArrayOutputStream stream = new ByteArrayOutputStream();
        bitmap.compress(Bitmap.CompressFormat.PNG, 70, stream);
        return stream.toByteArray();
    }

    public void finishPictures() {
        try {
            //Sends the mesage to the server
            //First send 1 in order to notify we want connection
            output.write(0);
            output.flush();

        } catch (Exception e) {
            Log.e("TCP", "SERVER:Error", e);
        } finally {
            try {
                socket.close();
            } catch (Exception e) {
                Log.e("TCP", "Problem closing the socket", e);

            }
        }
    }


}
