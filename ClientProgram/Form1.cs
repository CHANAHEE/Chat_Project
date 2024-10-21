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
            // 서버로 보내기
            this.flowLayoutPanel_Message.Controls.Add(new MessageControl());
        }
    }
}
