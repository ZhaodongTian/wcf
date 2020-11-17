using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WCFCorePerfService
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string url = "https://wcfcrank.blob.core.windows.net/app/WcfCorePerfCrankService.zip";

            var bombardierFileName = Path.GetFileName(url);
            var _httpClient = new HttpClient();

            using (var downloadStream = await _httpClient.GetStreamAsync(url))

            using (var fileStream = File.Create(bombardierFileName))
            {
                await downloadStream.CopyToAsync(fileStream);
            }

            ZipFile.ExtractToDirectory(bombardierFileName, @".\",true);
            //if (test.ProcessRunOptions(args))
            //{
            //    BenchmarksEventSource.Log.Metadata("channelopen", "max", "max", "Channel Open Time (ms)", "Time to Open Channel in ms", "n0");
            //    BenchmarksEventSource.Log.Metadata("firstrequest", "max", "max", "First Request (ms)", "Time to first request in ms", "n0");
            //    BenchmarksEventSource.Log.Metadata("bombardier/requests", "max", "sum", "Requests (" + test._paramPerfMeasurementDuration * 1000 + " ms)", "Total number of requests", "n0");
            //    BenchmarksEventSource.Log.Metadata("bombardier/rps/max", "max", "sum", "Requests/sec (max)", "Max requests per second", "n0");

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

                //string arg = "binding:" + test._paramBinding + " transfermode:" + test._paramTransferMode + " perfmeasurementduration:" + test._paramPerfMeasurementDuration + " serviceurl:" + test._paramServiceUrl;
               // process.StartInfo.Arguments = arg;
                process.Start();
               // BenchmarksEventSource.SetChildProcessId(process.Id);
                process.BeginOutputReadLine();
            process.StandardInput.WriteLine();
               process.WaitForExit();

                //var dict = test.ProcessOutPut(stringBuilder.ToString());
                //BenchmarksEventSource.Measure("channelopen", dict["channelopen"]);
                //BenchmarksEventSource.Measure("firstrequest", dict["firstrequest"]);
                //BenchmarksEventSource.Measure("bombardier/requests", dict["bombardier/requests"]);
                //BenchmarksEventSource.Measure("bombardier/rps/max", dict["bombardier/rps/max"]);
            //}
        }
    }
}
