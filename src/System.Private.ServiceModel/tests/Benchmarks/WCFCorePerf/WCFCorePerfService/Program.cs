using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WCFCorePerfService
{
    public class Parameters
    {
        public const string Port = "port";
    }

    class Program
    {
        private string _paramPort = "Port";
        static async Task Main(string[] args)
        {
            string url = "https://wcfcrank.blob.core.windows.net/app/WcfCorePerfCrankService.zip";

            var bombardierFileName = Path.GetFileName(url);
            var _httpClient = new HttpClient();

            using (var downloadStream = await _httpClient.GetStreamAsync(url))
            {
                using (var fileStream = File.Create(bombardierFileName))
                {
                    await downloadStream.CopyToAsync(fileStream);
                }
            }

            ZipFile.ExtractToDirectory(bombardierFileName, @".\", true);
            Program test = new Program();
            if (test.ProcessRunOptions(args))
            {
                var process = new Process()
                {
                    StartInfo = {
                    FileName = "WcfCorePerfCrankService.exe",
                    RedirectStandardOutput = true,
                    RedirectStandardInput = true,
                    UseShellExecute = false
                },
                    EnableRaisingEvents = true
                };
                var stringBuilder = new StringBuilder();
                process.OutputDataReceived += (_, e) =>
                {
                    if (e != null && e.Data != null)
                    {
                        Console.WriteLine(e.Data);

                        lock (stringBuilder)
                        {
                            stringBuilder.AppendLine(e.Data);
                        }
                    }
                };

                string arg = "port:" + test._paramPort;
                process.StartInfo.Arguments = arg;
                process.Start();
                process.BeginOutputReadLine();
                process.WaitForExit();
            }
        }

        private bool ProcessRunOptions(string[] args)
        {
            foreach (string s in args)
            {
                Console.WriteLine(s);
                string[] p = s.Split(new char[] { ':' }, count: 2);
                if (p.Length != 2)
                {
                    continue;
                }

                switch (p[0].ToLower())
                {
                    case Parameters.Port:
                        _paramPort = p[1];
                        break;
                    default:
                        Console.WriteLine("unknown argument: " + s);
                        continue;
                }
            }

            return true;
        }
    }
}
