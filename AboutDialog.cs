using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace SoundModBuilder
{
    public partial class AboutDialog : Form
    {
        readonly Version CurrVersion = Assembly.GetExecutingAssembly().GetName().Version;

        public AboutDialog()
        {
            InitializeComponent();
            var CurrVersion = Assembly.GetExecutingAssembly().GetName().Version;
            label4.Text = CurrVersion.ToString();
            label5.Text = CurrVersion.ToString();
            CheckNewVersion();
        }

        private void LinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            linkLabel1.LinkVisited = true;
            String url = "https://gitflic.ru/project/charsi82/soundmodbuildergui/release";
            Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
        }

        private async void CheckNewVersion()
        {
            try
            {
                string fileToDownload = "https://gitflic.ru/project/charsi82/soundmodbuildergui/blob/raw?file=Properties/AssemblyInfo.cs";
                using (var httpClient = new HttpClient())
                {
                    string content = await httpClient.GetStringAsync(fileToDownload);
                    Regex re = new Regex("AssemblyVersion\\(\"(\\d[\\d\\.]+)");
                    var match = re.Match(content);
                    if (match.Success)
                    {
                        label5.Text = $"{match.Groups[1].Value}";
                    }
                }
            }
            catch
            {
                // обновление не доступно
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                Close();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
