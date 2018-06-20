package com.wezz.imageserviceapp;

import android.util.Log;

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

    public void send() {
        PrintWriter pw;
        try {
            //Autoflash is true important!
            pw = new PrintWriter(socket.getOutputStream(),true);
            pw.write("hi im android wezzzz\n");
            pw.close();



            //Sends the mesage to the server
      //      OutputStream output = socket.getOutputStream();
       //     FileInputStream fis = new FileInputStream();
          //  output.write(2);
       //     output.flush();

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
