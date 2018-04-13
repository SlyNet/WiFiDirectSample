using System;
using System.IO;
using Android.App;
using Android.Content;
using Android.Util;
using com.example.android.wifidirect.Communication;
using Java.Net;
using Refit;

namespace com.example.android.wifidirect
{
    [Service]
    public class FileTransferService: IntentService
    {
        public static String ActionSendFile = "com.example.android.wifidirect.SEND_FILE";
        public static String ExtrasFilePath = "file_url";
        public static String ExtrasGroupOwnerAddress = "go_host";
        public static String ExtrasGroupOwnerPort = "go_port";

        public FileTransferService() 
            : base("FileTransferService")
        {
        }

        public FileTransferService(string name)
            : base(name)
        {
        }

        protected override void OnHandleIntent(Intent intent)
        {
            var context = ApplicationContext;
            if (intent.Action.Equals(ActionSendFile))
            {
                var host = intent.GetStringExtra(ExtrasGroupOwnerAddress);
                var port = intent.GetIntExtra(ExtrasGroupOwnerPort, 9696);

                var client = RestService.For<ISyncApi>($"http://{host}:{port}");

                Log.Debug(WiFiDirectActivity.Tag, "Opening client socket - ");

                var fileUri = intent.GetStringExtra(ExtrasFilePath);

                var cr = context.ContentResolver;
                var inputStream = cr.OpenInputStream(Android.Net.Uri.Parse(fileUri));

                try
                {
                    client.Upload(Path.GetFileName(fileUri).Replace(":", "")+".jpg",
                            new StreamPart(inputStream, "photo.jpg", "image/jpeg"))
                        .Wait();

                    Log.Debug(WiFiDirectActivity.Tag, "Client: Data written");
                }
                catch(Exception e)
                {
                    Log.Error(WiFiDirectActivity.Tag, e.ToString());
                    //Log.Debug(WiFiDirectActivity.Tag, "Client: Data written");
                }
            }
        }
    }
}