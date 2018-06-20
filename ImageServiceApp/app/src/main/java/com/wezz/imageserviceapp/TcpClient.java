package com.wezz.imageserviceapp;

import android.util.Log;

import java.io.File;
import java.io.FileInputStream;
import java.io.OutputStream;
import java.io.PrintWriter;
import java.net.InetAddress;
import java.net.Socket;
import java.util.concurrent.ExecutionException;

public class TcpClient {
    private int port;
    private String ip;
    private Socket socket= null;

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

    //name: the name of the picture
    //image - the image itself
    public void sendPicture(byte [] name, byte[] image) {
      //  PrintWriter pw;
        try {
            //Sends the mesage to the server
            OutputStream output = socket.getOutputStream();
            //Sends the picture name
            //Sends the length of picture name
            output.write(name.length);
            output.flush();
            //Sends the name
            output.write(name);
            output.flush();
            //Sends the length of the picture file bytes
            output.write(image.length);
            output.flush();
            //Sends the picture itself
            output.write(image);
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
