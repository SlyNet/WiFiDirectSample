using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Refit;

namespace com.example.android.wifidirect.Communication
{
    public interface ISyncApi
    {
        [Multipart]
        [Post("/api/images/{name}")]
        Task<UploadResponse> Upload(string name, [AliasAs("image")] StreamPart stream);
    }

    public class UploadResponse
    {
        public string Success { get; set; }
    }
}