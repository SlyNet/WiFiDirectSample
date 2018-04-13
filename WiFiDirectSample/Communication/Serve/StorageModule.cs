using Android.OS;
using Unosquare.Labs.EmbedIO;

namespace com.example.android.wifidirect.Communication.Serve
{
    public class StorageModule : WebModuleBase
    {
        private readonly string _packageName;

        public StorageModule(string packageName)
        {
            _packageName = packageName;
            // var f = new File(Environment.ExternalStorageDirectory + "/" + Activity.PackageName + "/wifip2pshared-" + DateTime.Now.Ticks + ".jpg");
        }

        public string GetFile(string name) => $"{Environment.ExternalStorageDirectory}/{_packageName}/{name.Replace(":", "")}";

        public override string Name { get; } = "Storage";
    }
}