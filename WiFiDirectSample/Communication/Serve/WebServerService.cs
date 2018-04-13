using System.Threading.Tasks;
using Android.App;
using Unosquare.Labs.EmbedIO;
using Unosquare.Labs.EmbedIO.Constants;
using Unosquare.Labs.EmbedIO.Modules;

namespace com.example.android.wifidirect.Communication.Serve
{
    public class WebServerService
    {
        private readonly WebServer _server;

        public WebServerService(Activity activity, int port = 9696)
        {
            this._server = new WebServer($"http://+:{port}/", RoutingStrategy.Regex);

            _server.RegisterModule(new WebApiModule());
            _server.RegisterModule(new StorageModule(activity.PackageName));
            _server.Module<WebApiModule>().RegisterController<SyncController>(() => new SyncController(activity));
        }

        public Task Start()
        {
            return _server.RunAsync();
        }
    }
}