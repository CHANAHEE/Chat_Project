using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ServerProgram
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            Init_UI();
        }

        private void Init_UI()
        {
            this.listView_Log.View = View.Details;
            this.listView_Log.Columns.Add("Time", 150);
            this.listView_Log.Columns.Add("Message", 500);
        }

        private void button_Start_Click(object sender, EventArgs e)
        {
            this.button_Start.BackColor = Color.SeaGreen;
            this.button_Start.ForeColor = Color.WhiteSmoke;
            this.button_Stop.BackColor = SystemColors.Control;
            this.button_Stop.ForeColor = Color.Black;
        }

        private void button_Stop_Click(object sender, EventArgs e)
        {
            this.button_Start.BackColor = SystemColors.Control;
            this.button_Start.ForeColor = Color.Black;
            this.button_Stop.BackColor = Color.Red;
            this.button_Stop.ForeColor = Color.WhiteSmoke;
        }
    }
}
