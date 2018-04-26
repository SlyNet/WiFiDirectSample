using System.IO;
using Android.App;
using Android.Content;
using Android.Util;
using Java.Lang;
using Refit;

namespace com.example.android.wifidirect.Communication
{
    [Service]
    public class FileTransferService: IntentService
    {
        private const int SocketTimeout = 5000;
        public static string ActionSendFile = "com.example.android.wifidirect.SEND_FILE";
        public static string ExtrasFilePath = "file_url";
        public static string ExtrasGroupOwnerAddress = "go_host";
        public static string ExtrasGroupOwnerPort = "go_port";

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

                var client = RestService.For<ISyncApi>($"http://{host}:{9696}");

                Log.Debug(WiFiDirectActivity.Tag, "Opening client socket - ");

                var fileUri = intent.GetStringExtra(ExtrasFilePath);

                var cr = context.ContentResolver;
                var inputStream = cr.OpenInputStream(Android.Net.Uri.Parse(fileUri));

                try
                {
                    client.Upload(Path.GetFileName(fileUri).Replace(":", "")+".jpg",
                            new StreamPart(inputStream, "photo.jpg", "image/jpeg"))
                         .GetAwaiter().GetResult();

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