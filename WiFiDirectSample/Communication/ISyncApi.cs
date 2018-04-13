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
        Task<bool> Upload(string name, [AliasAs("image")] StreamPart stream);

        [Get("/api/images")]
        Task<List<string>> List();

        [Get("/api/images/{name}")]
        Task<HttpContent> Get(string name);
    }
}