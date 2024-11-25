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
            NewSendMessage.Dock = DockStyle.Top;

            this.tableLayoutPanel_Message.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            this.tableLayoutPanel_Message.Controls.Add(NewSendMessage);
            this.tableLayoutPanel_Message.ScrollControlIntoView(NewSendMessage);

            this.button_Send.Enabled = false;
            this.richTextBox_Message.Clear();

            if(this.tableLayoutPanel_Message.VerticalScroll.Visible == true)
            {
                this.tableLayoutPanel_Message.Padding = new Padding(0, 0, SystemInformation.VerticalScrollBarWidth, 0);
            }            
        }

        public void Update_ReceiveMessage(string Message)
        {
            if (this.tableLayoutPanel_Message.InvokeRequired)
            {
                this.tableLayoutPanel_Message.Invoke(new MethodInvoker(delegate ()
                {
                    ReceiveMessageControl NewRecvMessage = new ReceiveMessageControl(Message, DateTime.Now);
                    NewRecvMessage.Dock = DockStyle.Top;

                    this.tableLayoutPanel_Message.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                    this.tableLayoutPanel_Message.Controls.Add(NewRecvMessage);

                    if (this.tableLayoutPanel_Message.VerticalScroll.Visible == true)
                    {
                        this.tableLayoutPanel_Message.Padding = new Padding(0, 0, SystemInformation.VerticalScrollBarWidth, 0);
                    }
                }));
            }
            else
            {
                ReceiveMessageControl NewRecvMessage = new ReceiveMessageControl(Message, DateTime.Now);
                NewRecvMessage.Dock = DockStyle.Top;

                this.tableLayoutPanel_Message.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                this.tableLayoutPanel_Message.Controls.Add(NewRecvMessage);

                if (this.tableLayoutPanel_Message.VerticalScroll.Visible == true)
                {
                    this.tableLayoutPanel_Message.Padding = new Padding(0, 0, SystemInformation.VerticalScrollBarWidth, 0);
                }
            }
        }

        public void OnServerConnectionResult(bool IsConnect)
        {
            if (this.button_Send.InvokeRequired)
            {
                this.button_Send.Invoke(new MethodInvoker(delegate ()
                {
                    this.richTextBox_Message.Enabled = IsConnect;
                    this.button_Send.Enabled = IsConnect;
                    if(IsConnect)
                    {
                        this.button_Send.Text = "전 송";
                    }
                    else
                    {
                        this.button_Send.Text = "서버 연결중";
                    }    
                }));
            }
            else
            {
                this.richTextBox_Message.Enabled = IsConnect;
                this.button_Send.Enabled = IsConnect;
                if (IsConnect)
                {
                    this.button_Send.Text = "전 송";
                }
                else
                {
                    this.button_Send.Text = "서버 연결중";
                }
            }
        }

        private void richTextBox_Message_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Shift && e.KeyCode == Keys.Enter)
            {
                e.Handled = true;  

                this.richTextBox_Message.AppendText(Environment.NewLine);                
            }
            else if (e.KeyCode == Keys.Enter)
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
