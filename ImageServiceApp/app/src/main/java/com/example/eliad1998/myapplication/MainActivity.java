package com.example.eliad1998.myapplication;

import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;

public class MainActivity extends AppCompatActivity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        System.out.println("yoyo");
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
    }
}
