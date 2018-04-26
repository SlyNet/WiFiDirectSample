using System;
using System.IO;
using System.Linq;
using System.Net;
using Android.App;
using Android.Content;
using Android.Util;
using HttpMultipartParser;
using Unosquare.Labs.EmbedIO;
using Unosquare.Labs.EmbedIO.Constants;
using Unosquare.Labs.EmbedIO.Modules;
using Unosquare.Swan;
using File = Java.IO.File;

namespace com.example.android.wifidirect.Communication
{
    public class SyncController : WebApiController
    {
        private readonly Activity _activity;

        public SyncController(Activity activity)
        {
            _activity = activity;
        }

        [WebApiHandler(HttpVerbs.Post, "/api/images/{name}")]
        public bool UploadImage(WebServer serve, HttpListenerContext context, string name)
        {
            var path = serve.Module<StorageModule>().GetFile(name);

            var parser = new MultipartFormDataParser(context.Request.InputStream);
            var file = parser.Files.First();
            string filename = file.FileName;
            Stream data = file.Data;

            var f = new File(path);
            var dirs = new File(f.Parent);
            if (!dirs.Exists())
                dirs.Mkdirs();

            f.CreateNewFile();
            Log.Debug(WiFiDirectActivity.Tag, "Server: copying files " + f);

            using (var stream = new FileStream(f.ToString(), FileMode.OpenOrCreate))
            {
                data.CopyTo(stream);
            }

            var intent = new Intent();
            intent.SetAction(Intent.ActionView);
            intent.SetDataAndType(Android.Net.Uri.Parse("file://" + path), "image/*");
            _activity.StartActivity(intent);
            
            return context.JsonResponse(new { Success = "OK"});
        }

        protected bool HandleError(HttpListenerContext context, Exception ex, int statusCode = 500)
        {
            var errorResponse = new
            {
                Title = "Unexpected Error",
                ErrorCode = ex.GetType().Name,
                Description = ex.ExceptionMessage(),
            };
            context.Response.StatusCode = statusCode;
            return context.JsonResponse(errorResponse);
        }
    }
}