using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace SoundModBuilder
{
    internal class AppUpdater
    {
        internal Version AvailVersion = Assembly.GetExecutingAssembly().GetName().Version;
        internal AppUpdater()
        {
            CheckAvailVersion();
        }

        internal async void CheckAvailVersion()
        {
            using (var httpClient = new HttpClient())
            {
                const string fileToDownload = "https://gitflic.ru/project/charsi82/soundmodbuildergui/blob/raw?file=Properties/AssemblyInfo.cs";
                string content = await httpClient.GetStringAsync(fileToDownload);
                Regex re = new Regex("AssemblyVersion\\(\"(\\d[\\d\\.]+)");
                var match = re.Match(content);
                if (match.Success)
                {
                    AvailVersion = new Version(match.Groups[1].Value);
                }
            }
        }

        internal bool Check()
        {
            return AvailVersion.CompareTo(Assembly.GetExecutingAssembly().GetName().Version) > 0;
        }

        internal void UpdateConfigs()
        {
            const string gitflic = "https://gitflic.ru/project/charsi82/soundmodbuildergui/blob/raw?file=bin/Release/";
            void UpdateFile(string fname)
            {
                var webRequest = HttpWebRequest.Create($"{gitflic}{fname}");
                webRequest.Method = "HEAD";
                using (var webResponse = webRequest.GetResponse())
                {
                    //var fileSize2 = webResponse.Headers.Get("Content-Length");
                    var fileSizeInByte = Convert.ToInt64(webResponse.ContentLength);
                    var fi = new FileInfo(fname);
                    if (fi.Length != fileSizeInByte)
                    {
                        using (WebClient webClient = new WebClient())
                        {
                            webClient.DownloadFile($"{gitflic}{fname}", fname);
                        }
                    }
                }
            }
            UpdateFile("commanders_ids.xml");
            UpdateFile("game_events.xml");
        }

        internal async void Run()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    const string gitflic = "https://gitflic.ru/project/charsi82/soundmodbuildergui/blob/raw?file=bin/Release/";

                    using (var s = await client.GetStreamAsync($"{gitflic}SoundModBuilder.exe"))
                    {
                        using (var fs = new FileStream("new.exe", FileMode.OpenOrCreate))
                        {
                            await s.CopyToAsync(fs);
                        }
                    }

                    if (File.Exists("new.exe"))
                    {
                        string exename = AppDomain.CurrentDomain.FriendlyName;
                        string exepath = Assembly.GetEntryAssembly().Location;
                        string args = $"taskkill /f /im \"{exename}\" && timeout /t 1 && del \"{exepath}\" && ren new.exe \"{exename}\" && \"{exepath}\" \"{Properties.Settings.Default.LastProject}\"";
                        var psi = new ProcessStartInfo
                        {
                            FileName = "cmd.exe",
                            Arguments = $"/c {args}",
                            UseShellExecute = false,
                            CreateNoWindow = true
                        };
                        Process.Start(psi);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка обновления");
            }
        }
    }
}