package com.example.user.myapplication;

import android.app.NotificationChannel;
import android.app.NotificationManager;
import android.app.Service;
import android.os.Build;
import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;
import android.content.IntentFilter;
import android.graphics.Bitmap;
import android.net.ConnectivityManager;
import android.net.NetworkInfo;
import android.net.wifi.WifiManager;
import android.os.Build;
import android.os.IBinder;
import android.support.annotation.Nullable;
import android.support.annotation.RequiresApi;
import android.support.v4.app.NotificationCompat;
import android.util.Log;
import android.widget.Toast;

import java.io.ByteArrayOutputStream;


public class ImagesService extends Service {
    private BroadcastReceiver yourReceiver;
    private TcpAndroidConnection tcp;

    @Override
    public void onCreate() {
        super.onCreate();
    }

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

    /**
     * In this function we checked the connection to the wifi whenever the user
     * clicked of "OnStart" button. If the wifi is on -> we made a TcpAndroidConnection
     * object , made a connection with the service and send all of the images
     * @param intent - prog Intent
     */
    public void checkConnection(Intent intent) {
        final IntentFilter theFilter = new IntentFilter();
        theFilter.addAction("android.net.wifi.supplicant.CONNECTION_CHANGE");
        theFilter.addAction("android.net.wifi.STATE_CHANGE");

        this.yourReceiver = new BroadcastReceiver() {
            @RequiresApi(api = Build.VERSION_CODES.O)
            @Override
            public void onReceive(Context context, Intent intent) {
                WifiManager wifiManager = (WifiManager) context.getSystemService(Context.WIFI_SERVICE);
                NetworkInfo networkInfo = intent.getParcelableExtra(WifiManager.EXTRA_NETWORK_INFO);

                // adding the progress bar
                final NotificationManager notificationManager = (NotificationManager) context.getSystemService(Context.NOTIFICATION_SERVICE);
                NotificationChannel channel = new NotificationChannel("default",
                        "Channel name",
                        NotificationManager.IMPORTANCE_DEFAULT);
                channel.setDescription("Channel description");
                notificationManager.createNotificationChannel(channel);
                final NotificationCompat.Builder builder = new NotificationCompat.Builder(context, "default");
                builder.setSmallIcon(R.drawable.ic_launcher_background);
                builder.setContentTitle("Transferring Images status");
                builder.setContentText("In progress");

                // check connection of wifi
                if (networkInfo != null) {
                    if (networkInfo.getType() == ConnectivityManager.TYPE_WIFI) {
                        // if wifi is connected than start transferring images
                        if (networkInfo.getState() == NetworkInfo.State.CONNECTED) {
                            tcp = new TcpAndroidConnection(notificationManager, builder);
                            tcp.connectToService();
                            tcp.sendPhotos();
                            Log.e("WIFI","Connected");
                        } else { // wifi is off
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
        tcp.closeConnection();
        unregisterReceiver(this.yourReceiver);
    }

}