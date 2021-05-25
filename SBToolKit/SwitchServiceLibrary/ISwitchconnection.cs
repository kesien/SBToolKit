using System;
using System.IO;
using System.Threading.Tasks;

namespace SwitchServiceLibrary
{
    public interface ISwitchconnection
    {
        Uri BaseAddress { get; init; }

        Task<Stream> DownloadListAsync(params string[] courseNumbers);
        Task LoginAsync(string username, string password);
    }
}