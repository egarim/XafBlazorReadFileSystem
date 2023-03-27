using Microsoft.JSInterop;
using System;
using System.Linq;

namespace XafBlazorReadFileSystem.Blazor.Server.Controllers
{
    public static class FileUtil
    {
        public async static Task SaveAs(IJSRuntime js, string filename, byte[] data)
        {
            await js.InvokeAsync<object>(
                "save",
                filename,
                Convert.ToBase64String(data));
        }
    }
}
