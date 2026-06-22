using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace SoundModBuilder
{
    public partial class Commander
    {
        public string ID { get; set; } // name
        public string Name { get; set; } // IDS_NAME
        public string Name_RU { get; set; } // Localized
        public string CBItemName() { return $"{Name_RU} ({ID})"; }
    }

    public partial class ProjectOptionsWindow : Form
    {
        public string ProjectName { get; set; }
        public string ProjectPath { get; set; }
        public string ProjectSrcPath { get; set; }
        public string CommanderID { get; set; }

        public Form1 parent;

        public ProjectOptionsWindow()
        {
            InitializeComponent();
        }

        private void BtnApply_Click(object sender, EventArgs e)
        {
            ProjectName = textBox1.Text;
            ProjectPath = textBox2.Text;
            ProjectSrcPath = textBox3.Text;
            CommanderID = parent.CommanderIDbyItemName(comboBox1.Text, CommanderID);
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

            foreach (var commander in parent.CommanderList)
            {
                comboitems.Add(commander.CBItemName());
            }

            comboBox1.Items.AddRange(comboitems.ToArray());
            string item_name = parent.ItemNamebyCommanderID(CommanderID);
            if (!comboBox1.Items.Contains(item_name))
            {
                comboBox1.Items.Insert(1, item_name);
            }
            comboBox1.SelectedItem = item_name;
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
