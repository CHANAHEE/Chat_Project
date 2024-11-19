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

            //this.tableLayoutPanel_Message.HorizontalScroll.Visible = false;
            //this.tableLayoutPanel_Message.RowStyles.Add(new RowStyle(SizeType.Absolute, 10));

            //this.Controls.Add(tableLayoutPanel_Message);
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

            SendMessageControl NewSendMessage = new SendMessageControl(this.richTextBox_Message.Text, DateTime.Now);
            //NewSendMessage.AutoSize = true;
            //NewSendMessage.Dock = DockStyle.None;
            //NewSendMessage.Height = 10;
            //this.tableLayoutPanel_Message.Controls.Add(NewSendMessage);
            this.panel_Message.Controls.Add(NewSendMessage);
            NewSendMessage.Dock = DockStyle.Top;

            this.button_Send.Enabled = false;
            this.richTextBox_Message.Clear();
        }

        public void Update_ReceiveMessage(string Message)
        {
            //if (this.tableLayoutPanel_Message.InvokeRequired)
            //{
            //    this.tableLayoutPanel_Message.Invoke(new MethodInvoker(delegate ()
            //    {
            //        ReceiveMessageControl NewRecvMessage = new ReceiveMessageControl(Message, DateTime.Now);
            //        NewRecvMessage.Width = this.Parent.Width;
            //        NewRecvMessage.Height = 50;
            //        NewRecvMessage.Dock = DockStyle.Fill;
            //        NewRecvMessage.AutoSize = true;
            //        this.tableLayoutPanel_Message.Controls.Add(NewRecvMessage);

            //    }));
            //}
            //else
            //{
            //    ReceiveMessageControl NewRecvMessage = new ReceiveMessageControl(Message, DateTime.Now);
            //    NewRecvMessage.Width = this.Parent.Width;
            //    NewRecvMessage.Height = 50;
            //    NewRecvMessage.Dock = DockStyle.Fill;
            //    NewRecvMessage.AutoSize = true;
            //    this.tableLayoutPanel_Message.Controls.Add(NewRecvMessage);

            //}
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
