using System.Diagnostics;

namespace ClientProgram
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            this.flowLayoutPanel_Message.HorizontalScroll.Visible = false;
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Process.GetCurrentProcess().Kill();
        }

        private void button_Send_Click(object sender, EventArgs e)
        {
            // ������ ������
            this.flowLayoutPanel_Message.Controls.Add(new MessageControl());
        }
    }
}
