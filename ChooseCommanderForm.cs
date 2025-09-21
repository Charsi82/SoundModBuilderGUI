using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SoundModBuilder
{
    public partial class ChooseCommanderForm : Form
    {
        public ChooseCommanderForm(List<string> items)
        {
            InitializeComponent();
            Items = items;
        }

        public List<string> Items;
        public string Result;

        private void ChooseCommanderForm_Load(object sender, EventArgs e)
        {
            comboBox1.Items.AddRange(Items.ToArray());
            comboBox1.SelectedIndex = 0;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Result = comboBox1.Text;
        }
    }

}
