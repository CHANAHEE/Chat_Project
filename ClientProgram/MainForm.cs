using System.Diagnostics;
using System.Net;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

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
            // 서버로 보내기
            client.SendMessage(this.richTextBox_Message.Text);

            this.flowLayoutPanel_Message.Controls.Add(new SendMessageControl(this.richTextBox_Message.Text, DateTime.Now));

            this.richTextBox_Message.Clear();
        }

        public void Update_ReceiveMessage(string Message)
        {
            if (this.flowLayoutPanel_Message.InvokeRequired)
            {
                this.flowLayoutPanel_Message.Invoke(new MethodInvoker(delegate ()
                {
                    this.flowLayoutPanel_Message.Controls.Add(new ReceiveMessageControl(Message, DateTime.Now));
                }));
            }
            else
            {
                this.flowLayoutPanel_Message.Controls.Add(new ReceiveMessageControl(Message, DateTime.Now));
            }
        }

        private void richTextBox_Message_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;

                this.button_Send.PerformClick();
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // 클라이언트 초기화
            client = new Client();

            // 서버 연결
            IPEndPoint EndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5000);
            client.Start(EndPoint, this);
        }

        private void richTextBox_Message_KeyUp(object sender, KeyEventArgs e)
        {
            if (this.richTextBox_Message.Text == string.Empty)
            {
                this.button_Send.Enabled = false;
            }
            else
            {
                this.button_Send.Enabled = true;
            }
        }
    }
}
