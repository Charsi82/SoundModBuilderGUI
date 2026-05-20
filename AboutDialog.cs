using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;

namespace SoundModBuilder
{
    partial class AboutDialog : Form
    {
        public AboutDialog()
        {
            InitializeComponent();
            labelCurrVersion.Text = $"Текущая версия: {Assembly.GetExecutingAssembly().GetName().Version}";
        }

        private void LinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            linkLabel1.LinkVisited = true;
            string url = "https://gitflic.ru/project/charsi82/soundmodbuildergui/release";
            Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
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
