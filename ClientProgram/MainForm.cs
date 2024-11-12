using System.Diagnostics;
using System.Net;

namespace ClientProgram
{
    public partial class MainForm : Form
    {
        Client client;

        public MainForm()
        {
            InitializeComponent();

            this.flowLayoutPanel_Message.HorizontalScroll.Visible = false;
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            client.CloseClientSocket();
            Process.GetCurrentProcess().Kill();
        }

        private void button_Send_Click(object sender, EventArgs e)
        {
            // ������ ������
            client.SendMessage(this.richTextBox_Message.Text);

            this.flowLayoutPanel_Message.Controls.Add(new MessageControl(this.richTextBox_Message.Text, DateTime.Now));

            this.richTextBox_Message.Clear();
        }

        private void richTextBox_Message_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.Enter && e.Shift)
            //{

            //}
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;

                this.button_Send.PerformClick();
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // Ŭ���̾�Ʈ �ʱ�ȭ
            client = new Client();

            // ���� ����
            IPEndPoint EndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5000);
            client.Start(EndPoint);
        }
    }
}
