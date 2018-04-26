using Android.OS;
using Unosquare.Labs.EmbedIO;

namespace com.example.android.wifidirect.Communication
{
    public class StorageModule : WebModuleBase
    {
        private readonly string packageName;

        public StorageModule(string packageName)
        {
            this.packageName = packageName;
            // var f = new File(Environment.ExternalStorageDirectory + "/" + Activity.PackageName + "/wifip2pshared-" + DateTime.Now.Ticks + ".jpg");
        }

        public string GetFile(string name) => $"{Environment.ExternalStorageDirectory}/{packageName}/{name.Replace(":", "")}";

        public override string Name { get; } = "Storage";
    }
}