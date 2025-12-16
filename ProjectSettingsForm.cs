using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;

namespace SoundModBuilder
{
    public partial class ProjectOptionsWindow : Form
    {
        public string ProjectName { get; set; }
        public string ProjectPath { get; set; }
        public string ProjectSrcPath { get; set; }
        public string CommanderID { get; set; }

        private class Commander
        {
            /// <summary>
            /// SID командира
            /// </summary>
            public string Ids { get; set; }
            /// <summary>
            /// текст строки выпадающего списка
            /// </summary>
            public string CB_ItemName { get; set; }
        }
        /// <summary>
        /// список командиров
        /// </summary>
        private List<Commander> CommanderList;

        public ProjectOptionsWindow()
        {
            InitializeComponent();
            InitializeCommanderList();
        }

        private void InitializeCommanderList()
        {
            string comm_xml = "commanders_ids.xml";
            CommanderList = new List<Commander>();
            if (!File.Exists(comm_xml)) return;
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(comm_xml);
            XmlElement xRoot = xDoc.DocumentElement;
            foreach (XmlNode xnode in xRoot.ChildNodes)
            {
                if (xnode.NodeType != XmlNodeType.Comment)
                {
                    Commander e = new Commander()
                    {
                        Ids = xnode.Attributes["id"].Value,
                        CB_ItemName = $"{xnode.Attributes["ru"].Value} ({xnode.Attributes["id"].Value})",
                    };
                    CommanderList.Add(e);
                }
            }
        }

        private void BtnApply_Click(object sender, EventArgs e)
        {
            ProjectName = textBox1.Text;
            ProjectPath = textBox2.Text;
            ProjectSrcPath = textBox3.Text;

            string cb_text = comboBox1.Text;
            if (cb_text.Length > 0)
            {

                foreach (var commander in CommanderList)
                {
                    if (commander.CB_ItemName == cb_text)
                    {
                        CommanderID = commander.Ids;
                        break;
                    }
                }
            }
            else
            {
                CommanderID = "";
            }
            Close();
        }

        private void BtnGetFolder_Click(object sender, EventArgs e)
        {
            var FolderBrowserDialog = new FolderBrowserDialog
            {
                Description = "Выберите папку с wav файлами:",
                SelectedPath = Directory.Exists(ProjectSrcPath) ? ProjectSrcPath : Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                RootFolder = Environment.SpecialFolder.MyComputer,
                ShowNewFolderButton = false
            };
            if (DialogResult.OK == FolderBrowserDialog.ShowDialog(this))
            {
                textBox3.Text = ProjectSrcPath = FolderBrowserDialog.SelectedPath;
            }
        }

        private string ItemNamebyCommanderID(string Ids)
        {
            if (Ids == "") return "";
            foreach (var commander in CommanderList)
            {
                if (commander.Ids == Ids) return commander.CB_ItemName;
            }
            return $"unknown ({Ids})";
        }

        private void ProjectSettingsWindow_Load(object sender, EventArgs e)
        {
            textBox1.Text = ProjectName;
            textBox2.Text = ProjectPath;
            textBox3.Text = ProjectSrcPath;
            var comboitems = new List<string>
            {
                // 'any commander' item
                ""
            };

            foreach (var commander in CommanderList)
            {
                comboitems.Add(commander.CB_ItemName);
            }

            comboitems.Sort(delegate (string a, string b)
            {
                if (a.Length == 0) return -1;
                if (b.Length == 0) return 1;
                var matcha = Regex.IsMatch(a, @"\p{IsCyrillic}");
                var matchb = Regex.IsMatch(b, @"\p{IsCyrillic}");
                if (matcha && !matchb) return -1;
                if (matchb && !matcha) return 1;
                return a.CompareTo(b);
            });

            comboBox1.Items.AddRange(comboitems.ToArray());
            string item_name = ItemNamebyCommanderID(CommanderID);
            if (!comboBox1.Items.Contains(item_name))
            {
                comboBox1.Items.Insert(1, item_name);
            }
            comboBox1.SelectedItem = item_name;
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
    }
}
