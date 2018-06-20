package com.example.user.myapplication;

import android.app.NotificationManager;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.os.Environment;
import android.support.v4.app.NotificationCompat;
import android.util.Log;


import java.io.ByteArrayOutputStream;
import java.io.File;
import java.io.FileInputStream;
import java.io.IOException;
import java.io.OutputStream;
import java.net.InetAddress;
import java.net.Socket;

/**
 * This class is responsible of making the connection with the imageService server
 * and sending it all of the images
 */
public class TcpAndroidConnection {

    private Socket socket;
    private OutputStream outputStream;
    private NotificationManager notificationManager;
    private NotificationCompat.Builder builder;

    /**
     * c'tor
     * @param notificationManager - as NotificationManager
     * @param builder - as Builder
     */
    public TcpAndroidConnection(final NotificationManager notificationManager, final NotificationCompat.Builder builder) {
        this.notificationManager = notificationManager;
        this.builder = builder;
    }

    /**
     * In this method we made the connection with the server of the image service
     * in a new thread
     */
    public void connectToService() {
        Thread thread = new Thread(new Runnable() {
            @Override
            public void run() {
                try {
                    InetAddress serverAddr = InetAddress.getByName("10.0.2.2");
                    socket = new Socket(serverAddr, 1234);
                    try {
                        outputStream = socket.getOutputStream();
                    } catch (Exception e) {
                        socket.close();
                        Log.e("TCP", "Failed creating an output stream: ", e);
                    }
                } catch (Exception e) {
                    Log.e("TCP", "Failed creating the socket: ", e);
                }
            }
        });
        thread.start();
    }

    /**
     * This function sends all of the images in the camera directory to the sever of image service
     */
    public void sendPhotos() {
        Thread thread = new Thread(new Runnable() {
            @Override
            public void run() {
                // get file of images
                File dcim = new File(Environment.getExternalStoragePublicDirectory(Environment.DIRECTORY_DCIM), "Camera");
                if (dcim == null) {
                    return;
                }
                //get the list of images in the dcim
                File[] pics = dcim.listFiles();
                double listLength = pics.length;
                double count = 0;
                if (pics != null) {
                    for (File pic : pics) {
                        try {
                            FileInputStream fis = new FileInputStream(pic);
                            Bitmap bm = BitmapFactory.decodeStream(fis);
                            byte[] imgByte = getBytesFromBitmap(bm);
                            try {
                                // write the number of bytes of the image
                                int imageLen = imgByte.length;
                                String sendImageLen = imageLen + "";
                                outputStream.write(sendImageLen.getBytes(), 0, sendImageLen.getBytes().length);
                                outputStream.flush();
                                Thread.sleep(100);

                                // write the image name
                                String fileName = pic.getName();
                                outputStream.write(fileName.getBytes(), 0, fileName.getBytes().length);
                                outputStream.flush();
                                Thread.sleep(120);

                                // send image bytes
                                outputStream.write(imgByte, 0, imageLen);
                                outputStream.flush();
                                Thread.sleep(600);
                            } catch (Exception e) {
                                Log.e("TCP", "Failed writing: ", e);
                            }
                        } catch (Exception e) {
                            Log.e("TCP", "Failed creating FileInputStream", e);
                        }
                        count++;
                        int progBar = (int) ((count / listLength) * 100);
                        String msg = progBar + "%";
                        builder.setProgress(100, progBar, false).setContentText(msg);
                        notificationManager.notify(1, builder.build());
                    }
                    try {
                        String toSend = "End\n";
                        outputStream.write(toSend.getBytes(), 0, toSend.getBytes().length);
                        outputStream.flush();
                        builder.setContentTitle("finished transferring.").
                                setContentText("All images delivered :)");
                        notificationManager.notify(1, builder.build());
                    } catch (Exception e) {
                        Log.e("TCP", "fail to write:", e);
                        builder.setContentTitle("Error").
                                setContentText("Error on sending the photos");
                        notificationManager.notify(1, builder.build());
                    }
                }
            }
        });
        thread.start();
    }

    /**
     * In this function we close the socket
     */
    public void closeConnection() {
        try {
            this.socket.close();
        } catch (IOException e) {
            Log.e("TCP", "Error closing the socket..", e);
        }
    }

    /**
     * This function convert from Bitmap to an array of bytes
     *
     * @param bitmap - a Bitmap obj
     * @return an array of bytes representing the image
     */
    public byte[] getBytesFromBitmap(Bitmap bitmap) {
        ByteArrayOutputStream stream = new ByteArrayOutputStream();
        bitmap.compress(Bitmap.CompressFormat.PNG, 70, stream);
        return stream.toByteArray();
    }

}
