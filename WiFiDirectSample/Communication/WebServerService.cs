using System.Threading.Tasks;
using Android.App;
using Unosquare.Labs.EmbedIO;
using Unosquare.Labs.EmbedIO.Constants;
using Unosquare.Labs.EmbedIO.Modules;

namespace com.example.android.wifidirect.Communication
{
    public class WebServerService
    {
        private readonly WebServer server;

        public WebServerService(Activity activity, int port = 9696)
        {
            this.server = new WebServer($"http://+:{port}/", RoutingStrategy.Regex);

            server.RegisterModule(new WebApiModule());
            server.RegisterModule(new StorageModule(activity.PackageName));
            server.Module<WebApiModule>().RegisterController(() => new SyncController(activity));
        }

        public Task Start()
        {
            return server.RunAsync();
        }
    }
}