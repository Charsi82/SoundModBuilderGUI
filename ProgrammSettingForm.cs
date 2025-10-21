using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;

namespace SoundModBuilder
{
    public partial class SettingsWindow : Form
    {
        public Form1 parent;
        public SettingsWindow()
        {
            InitializeComponent();
        }

        private void BtnSelectGamePath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog
            {
                ShowNewFolderButton = false,
                SelectedPath = Properties.Settings.Default.GamePath,
                Description = "Укажите путь к папке с игрой:"
            };
            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                textBox1.Text = dialog.SelectedPath;
            }
        }

        private void BtnSelectWwisePath_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                Title = "Укажите путь к WwiseCLI.exe",
                FileName = "WwiseCLI.exe",
                Filter = "WwiseCLI.exe|WwiseCLI.exe"
            };
            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                textBox2.Text = dialog.FileName;
            }
        }

        private void BtnSelectWwisePrj_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                Title = "Укажите путь к файлу conv_19_WIN.wproj",
                FileName = "conv_19_WIN",
                Filter = "Wwise project (conv_19_WIN.wproj)|conv_19_WIN.wproj",
            };
            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                textBox3.Text = dialog.FileName;
            }
        }

        private void SettingsWindow_Load(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(label4,
                "Путь к папке с игрой (содержит папку bin и файл preferences.xml)\nНапример: D:\\Games\\Korabli");
            toolTip1.SetToolTip(label5,
                "Путь к файлу WwiseCLI.exe\nНапример: D:\\utils\\Wwise 2019.2.15.7667\\Authoring\\x64\\Release\\bin\\WwiseCLI.exe");
            toolTip1.SetToolTip(label6,
                "Путь к проекту Wwise 2019\nНапример: D:\\utils\\wows_conversion_project19_Only_Windows\\conv_19_WIN.wproj");

            textBox1.Text = Properties.Settings.Default.GamePath;
            textBox2.Text = Properties.Settings.Default.WwisePath;
            textBox3.Text = Properties.Settings.Default.WwiseProject;
            updateColors();
            textBox3.Select(0, 0);
        }

        private void updateColors()
        {
            parent.UpdateTheme();
            parent.ApplyTheme(this);
            ButtonDarkMode.BackgroundImage = Properties.Settings.Default.DarkTheme ?
                Properties.Resources.sun : Properties.Resources.moon;
            button5.BackColor = Properties.Settings.Default.ColorBackEvtListItem;
            button6.BackColor = Properties.Settings.Default.ColorBackEvtListItemEmpty;
            button7.BackColor = Properties.Settings.Default.DarkModeBack;
            button8.BackColor = Properties.Settings.Default.DarkModeFore;
            button7.Enabled = Properties.Settings.Default.DarkTheme;
            button8.Enabled = Properties.Settings.Default.DarkTheme;
            label11.Enabled = Properties.Settings.Default.DarkTheme;
            label12.Enabled = Properties.Settings.Default.DarkTheme;
        }

        private void TextBox1_OnTextChanged()
        {
            if (Utils.IsValidGamePath(textBox1.Text))
            {
                Properties.Settings.Default.GamePath = textBox1.Text;
                Utils.UpdateGameVersion();
                label4.ImageIndex = 0;
            }
            else
            {
                label4.ImageIndex = 1;
            }
            UpdateLabelGameVer();
        }

        private void TextBox2_OnTextChanged()
        {
            if (Utils.IsValidWwisePath(textBox2.Text))
            {
                Properties.Settings.Default.WwisePath = textBox2.Text;
                label5.ImageIndex = 0;
            }
            else
            {
                label5.ImageIndex = 1;
            }
        }

        private void TextBox3_OnTextChanged()
        {
            if (Utils.IsValidWwiseProjectPath(textBox3.Text))
            {
                Properties.Settings.Default.WwiseProject = textBox3.Text;
                label6.ImageIndex = 0;
            }
            else
            {
                label6.ImageIndex = 1;
            }
        }

        private void UpdateLabelGameVer()
        {
            string gv = Properties.Settings.Default.GameVer;
            if (gv.Length == 0) gv = "не определена";
            labelGameVer.Text = $"Версия игры: {gv}";
        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            TextBox1_OnTextChanged();
        }

        private void TextBox2_TextChanged(object sender, EventArgs e)
        {
            TextBox2_OnTextChanged();
        }

        private void TextBox3_TextChanged(object sender, EventArgs e)
        {
            TextBox3_OnTextChanged();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Escape:
                    {
                        Close();
                        return true;
                    }
                default:
                    break;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void InitColorDialog()
        {
            Color clrB = Properties.Settings.Default.ColorBackEvtListItem;
            Color clrBE = Properties.Settings.Default.ColorBackEvtListItemEmpty;
            colorDialog1.CustomColors = new int[] {
                Color.FromArgb(0, 144, 238, 144 ).ToArgb(), // lightgreen
                Color.FromArgb(0, 211, 211, 211).ToArgb(), // silver
                0XFFFFFF,0XFFFFFF,0XFFFFFF,0XFFFFFF,0XFFFFFF,0XFFFFFF,
                Color.FromArgb(0, clrB.B, clrB.G, clrB.R).ToArgb(),
                Color.FromArgb(0, clrBE.B, clrBE.G, clrBE.R).ToArgb(),
            };
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            colorDialog1.Color = Properties.Settings.Default.ColorBackEvtListItem;
            InitColorDialog();
            if (colorDialog1.ShowDialog(this) == DialogResult.OK)
            {
                button5.BackColor = Properties.Settings.Default.ColorBackEvtListItem = colorDialog1.Color;
            }
        }

        private void Button6_Click(object sender, EventArgs e)
        {
            colorDialog1.Color = Properties.Settings.Default.ColorBackEvtListItemEmpty;
            InitColorDialog();
            if (colorDialog1.ShowDialog(this) == DialogResult.OK)
            {
                button6.BackColor = Properties.Settings.Default.ColorBackEvtListItemEmpty = colorDialog1.Color;
            }
        }

        private void Button7_Click(object sender, EventArgs e)
        {
            colorDialog1.Color = Properties.Settings.Default.DarkModeBack;
            InitColorDialog();
            if (colorDialog1.ShowDialog(this) == DialogResult.OK)
            {
                button7.BackColor = Properties.Settings.Default.DarkModeBack = colorDialog1.Color;
                updateColors();
            }
        }

        private void Button8_Click(object sender, EventArgs e)
        {
            colorDialog1.Color = Properties.Settings.Default.DarkModeFore;
            InitColorDialog();
            if (colorDialog1.ShowDialog(this) == DialogResult.OK)
            {
                button8.BackColor = Properties.Settings.Default.DarkModeFore = colorDialog1.Color;
                updateColors();
            }
        }

        private void ButtonDarkMode_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.DarkTheme = !Properties.Settings.Default.DarkTheme;
            updateColors();
        }
    }

    internal static class Utils
    {
        internal static bool IsValidGamePath(string path) => Directory.Exists($"{path}\\bin") && File.Exists($"{path}\\game_info.xml");
        internal static bool IsValidWwisePath(string path) => path.EndsWith("\\WwiseCLI.exe") && File.Exists(path);
        internal static bool IsValidWwiseProjectPath(string path) => path.EndsWith("\\conv_19_WIN.wproj") && File.Exists(path);
        internal static void UpdateGamePath()
        {
            if (!Directory.Exists(Properties.Settings.Default.GamePath))
            {
                string lgc_pref = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) +
                    "\\Lesta\\GameCenter\\preferences.xml";
                if (File.Exists(lgc_pref))
                {
                    XmlDocument xDoc = new XmlDocument();
                    xDoc.Load(lgc_pref);
                    var xnode = xDoc.DocumentElement?.SelectSingleNode("application")?.
                    SelectSingleNode("games_manager")?.SelectSingleNode("current_game");
                    if (xnode != null)
                    {
                        Properties.Settings.Default.GamePath = xnode.InnerText;
                    }
                }
            }

            string ParentDir = Directory.GetParent(Assembly.GetEntryAssembly().Location).FullName;
            if (!Directory.Exists(Properties.Settings.Default.WwisePath))
                Properties.Settings.Default.WwisePath = ParentDir +
                    @"\utils\Wwise 2019.2.15.7667\Authoring\x64\Release\bin\WwiseCLI.exe";

            if (!Directory.Exists(Properties.Settings.Default.WwiseProject))
                Properties.Settings.Default.WwiseProject = ParentDir +
                    @"\utils\wows_conversion_project19_Only_Windows\conv_19_WIN.wproj";
        }

        internal static void UpdateGameVersion()
        {
            string xml_path = $"{Properties.Settings.Default.GamePath}\\game_info.xml";
            if (File.Exists(xml_path))
            {
                XmlDocument xDoc = new XmlDocument();
                xDoc.Load(xml_path);
                var xnode = xDoc.DocumentElement?.SelectSingleNode("game")?.
                    SelectSingleNode("part_versions")?.SelectSingleNode("version")?.
                    Attributes?["installed"]?.InnerText;
                if (xnode != null)
                {
                    Regex reg = new Regex(@"\d+$");
                    Match match = reg.Match(xnode);
                    if (match.Success)
                    {
                        Properties.Settings.Default.GameVer = match.Value;
                    }
                }
            }
        }

        internal static bool InternetOk()
        {
            try
            {
                Dns.GetHostEntry(Dns.GetHostName());
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }
    }
}
