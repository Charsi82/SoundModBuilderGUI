using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SoundModBuilder
{
    public partial class ChooseCommanderForm : Form
    {
        public List<string> Items;
        public Form1 parent;
        public string CommanderID = string.Empty;

        public ChooseCommanderForm()
        {
            InitializeComponent();
        }

        private void ChooseCommanderForm_Load(object sender, EventArgs e)
        {
            foreach (string CommID in Items)
            {
                comboBox1.Items.Add(parent.ItemNamebyCommanderID(CommID));
                if (CommanderID.Length == 0) CommanderID = CommID;
            }
            comboBox1.SelectedIndex = 0;
        }

        private void ButtonOK_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommanderID = parent.CommanderIDbyItemName(comboBox1.Text, CommanderID);
        }
    }

}
