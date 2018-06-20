package com.example.user.myapplication;

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
import android.util.Base64;
import android.util.Log;
import android.widget.Toast;

import java.io.ByteArrayOutputStream;
import java.io.File;
import java.io.FileInputStream;
import java.io.IOException;
import java.io.OutputStream;
import java.io.OutputStreamWriter;
import java.io.PrintWriter;
import java.net.InetAddress;
import java.net.Socket;
import java.net.URLConnection;
import java.net.UnknownHostException;
import java.sql.SQLOutput;


public class ImagesService extends Service {
    private BroadcastReceiver yourReceiver;

    @Nullable
    @Override
    public IBinder onBind(Intent intent) {
        return null;
    }

    @Override
    public int onStartCommand(Intent intent, int flags, int startID){
        Toast.makeText(this,"Service on...", Toast.LENGTH_LONG).show();
        checkConnection(intent);
        return START_STICKY;
    }


    public void checkConnection(Intent intent) {
        final IntentFilter theFilter = new IntentFilter();
        theFilter.addAction("android.net.wifi.supplicant.CONNECTION_CHANGE");
        theFilter.addAction("android.net.wifi.STATE_CHANGE");
        this.yourReceiver = new BroadcastReceiver() {
            @Override
            public void onReceive(Context context, Intent intent) {
                WifiManager wifiManager = (WifiManager) context
                        .getSystemService(Context.WIFI_SERVICE);
                NetworkInfo networkInfo = intent.getParcelableExtra(WifiManager.EXTRA_NETWORK_INFO);
                if (networkInfo != null) {
                    if (networkInfo.getType() == ConnectivityManager.TYPE_WIFI) {
                        if (networkInfo.getState() == NetworkInfo.State.CONNECTED) {
                            sendImage();
                            Log.e("WIFI","Connected");
                        } else {
                            Log.e("WIFI","Not Connected");
                        }
                    }
                }
            }
        };
        this.registerReceiver(this.yourReceiver, theFilter);
    }

    @Override
    public void onDestroy(){
        Toast.makeText(this,"Service off...", Toast.LENGTH_LONG).show();
    }

    public void sendImage(){


        File dir = new File(Environment.getExternalStoragePublicDirectory(Environment.DIRECTORY_DCIM), "Camera");


        final File[] pics = dir.listFiles();


        if (pics != null)
            new Thread(new Runnable() {
            @Override
            public void run() {
                try {
                    InetAddress serverAddr = InetAddress.getByName("172.18.8.45");
                    //InetAddress serverAddr = InetAddress.getByName("10.0.2.2");
                    try {
                        for (File pic : pics) {
                            if (pic.isDirectory()) continue;
                            Socket socket = new Socket(serverAddr, 1234);
                            OutputStream outputStream = socket.getOutputStream();
                            FileInputStream fis = new FileInputStream(pic);
                            Bitmap bm = BitmapFactory.decodeStream(fis);
                            byte[] imgbyte = getBytesFromBitmap(bm);
                            try{
                                /*int imagelen = ;
                                String sendLength = imagelen +"";*/

                                String imageName = pic.getName();
                                outputStream.write(imageName.getBytes());
                                outputStream.flush();
                                Thread.sleep(100);

                                outputStream.write(imgbyte,0,imgbyte.length);
                                outputStream.flush();
                                Thread.sleep(600);

                                socket.close();

                            } catch (Exception e){
                                Log.e("TCP", "ERROR IN WRITING!");

                            }



                            /*Log.e("TCP", "before loop!");
                            Socket socket = new Socket(serverAddr, 1234);
                            OutputStream outputStream = socket.getOutputStream();
                            Log.e("TCP", "workkkk!");
                            FileInputStream fis = new FileInputStream(pic);
                            Bitmap bm = BitmapFactory.decodeStream(fis);
                            byte[] imgbyte = getBytesFromBitmap(bm);

                            outputStream.write(pic.getName().getBytes());

                            outputStream.write(imgbyte,0,imgbyte.length);

                            outputStream.flush();
                            socket.close();*/
                        }

                    } catch (Exception e) {
                        Log.e("TCP", "S: Error", e);
                    }
                } catch (Exception e) {
                    Log.e("TCP", "C: Error", e);
                }
            }
        }).start();
        }




    byte[] getBytesFromBitmap(Bitmap bitmap) {
        ByteArrayOutputStream stream = new ByteArrayOutputStream();
        bitmap.compress(Bitmap.CompressFormat.JPEG, 70, stream);
        return stream.toByteArray();
    }

}