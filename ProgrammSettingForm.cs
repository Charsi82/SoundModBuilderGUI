using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace SoundModBuilder
{
    public partial class SettingsWindow : Form
    {
        public SettingsWindow()
        {
            InitializeComponent();
            toolTip1.SetToolTip(label4,
                "Путь к папке с игрой (содержит папку bin и файл preferences.xml)\nНапример: D:\\Games\\Korabli");
            toolTip1.SetToolTip(label5,
                "Путь к файлу WwiseCLI.exe\nНапример: D:\\utils\\Wwise 2019.2.15.7667\\Authoring\\x64\\Release\\bin\\WwiseCLI.exe");
            toolTip1.SetToolTip(label6,
                "Путь к проекту Wwise 2019\nНапример: D:\\utils\\wows_conversion_project19_Only_Windows\\conv_19_WIN.wproj");

            textBox1.Text = Properties.Settings.Default.GamePath;
            textBox2.Text = Properties.Settings.Default.WwisePath;
            textBox3.Text = Properties.Settings.Default.WwiseProject;
            button5.BackColor = Properties.Settings.Default.ColorBackEvtListItem;
            button6.BackColor = Properties.Settings.Default.ColorBackEvtListItemEmpty;
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
                Filter = "Wwise project(conv_19_WIN.wproj)|conv_19_WIN.wproj",
            };
            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                textBox3.Text = dialog.FileName;
            }
        }

        private void SettingsWindow_Load(object sender, EventArgs e)
        {
            UpdateLabelGameVer();
            TextBox1_OnTextChanged();
            TextBox2_OnTextChanged();
            TextBox3_OnTextChanged();
            textBox1.Select();
            textBox1.Select(0, 0);
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
    }

    public partial class Utils
    {
        internal static bool IsValidGamePath(string path) => Directory.Exists($"{path}\\bin") && File.Exists($"{path}\\preferences.xml");
        internal static bool IsValidWwisePath(string path) => path.EndsWith("\\WwiseCLI.exe") && File.Exists(path);
        internal static bool IsValidWwiseProjectPath(string path) => path.EndsWith("\\conv_19_WIN.wproj") && File.Exists(path);
        internal static void UpdateGamePath()
        {
            if (!Directory.Exists(Properties.Settings.Default.GamePath))
            {
                const string lgc_pref = "C:\\ProgramData\\Lesta\\GameCenter\\preferences.xml";
                if (File.Exists(lgc_pref))
                {
                    string[] xml = File.ReadAllLines(lgc_pref);
                    foreach (string str in xml)
                    {
                        if (str.Contains("current_game"))
                        {
                            int from = str.IndexOf('<') + 14;
                            int len = str.LastIndexOf('<') - from;
                            Properties.Settings.Default.GamePath = str.Substring(from, len);
                            break;
                        }
                    }
                }
            }

            string ParentDir =  Directory.GetParent(Assembly.GetEntryAssembly().Location).FullName;        
            if(!Directory.Exists(Properties.Settings.Default.WwisePath))
                Properties.Settings.Default.WwisePath = ParentDir +
                    "\\utils\\Wwise 2019.2.15.7667\\Authoring\\x64\\Release\\bin\\WwiseCLI.exe";

            if(!Directory.Exists(Properties.Settings.Default.WwiseProject))
                Properties.Settings.Default.WwiseProject = ParentDir +
                    "\\utils\\wows_conversion_project19_Only_Windows\\conv_19_WIN.wproj";
        }

        internal static void UpdateGameVersion()
        {
            string pref = $"{Properties.Settings.Default.GamePath}\\preferences.xml";
            if (File.Exists(pref))
            {
                string[] xml = File.ReadAllLines(pref);
                foreach (string str in xml)
                {
                    if (str.Contains("last_server_version"))
                    {
                        int from = str.LastIndexOf(',') + 1;
                        int len = str.LastIndexOf('\t') - from;
                        Properties.Settings.Default.GameVer = str.Substring(from, len);
                        break;
                    }
                }
            }
        }
    }
}
