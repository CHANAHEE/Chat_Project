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
        private void AdjustScrollBars()
        {
            tableLayoutPanel_Message.VerticalScroll.Enabled = true;
            tableLayoutPanel_Message.VerticalScroll.Visible = true;
            // ���� ��ũ���� �ʿ����� Ȯ��
            bool verticalScrollRequired = tableLayoutPanel_Message.VerticalScroll.Visible;

            if (verticalScrollRequired)
            {
                // ���� ��ũ���� ���� �� ���� ��ũ���� ����ϴ�.
                tableLayoutPanel_Message.HorizontalScroll.Enabled = false;  // ���� ��ũ�� ��Ȱ��ȭ
                tableLayoutPanel_Message.HorizontalScroll.Visible = false;  // ���� ��ũ�� ����
            }
        }
        private void button_Send_Click(object sender, EventArgs e)
        {
            // ������ ������
            client.SendMessage(this.richTextBox_Message.Text);

            SendMessageControl NewSendMessage = new SendMessageControl(this.richTextBox_Message.Text, DateTime.Now);
            NewSendMessage.Dock = DockStyle.Top;

            this.tableLayoutPanel_Message.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            this.tableLayoutPanel_Message.Controls.Add(NewSendMessage);
            this.tableLayoutPanel_Message.ScrollControlIntoView(NewSendMessage);

            this.button_Send.Enabled = false;
            this.richTextBox_Message.Clear();

            AdjustScrollBars();
        }

        public void Update_ReceiveMessage(string Message)
        {
            if (this.tableLayoutPanel_Message.InvokeRequired)
            {
                this.tableLayoutPanel_Message.Invoke(new MethodInvoker(delegate ()
                {
                    ReceiveMessageControl NewRecvMessage = new ReceiveMessageControl(Message, DateTime.Now);
                    NewRecvMessage.Dock = DockStyle.Top;

                    this.tableLayoutPanel_Message.RowCount++;
                    this.tableLayoutPanel_Message.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                    this.tableLayoutPanel_Message.Controls.Add(NewRecvMessage, 0, tableLayoutPanel1.RowCount - 1);
                }));
            }
            else
            {
                ReceiveMessageControl NewRecvMessage = new ReceiveMessageControl(Message, DateTime.Now);
                NewRecvMessage.Dock = DockStyle.Top;

                this.tableLayoutPanel_Message.RowCount++;
                this.tableLayoutPanel_Message.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                this.tableLayoutPanel_Message.Controls.Add(NewRecvMessage, 0, tableLayoutPanel1.RowCount - 1);
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
            // Ŭ���̾�Ʈ �ʱ�ȭ
            client = new Client();

            // ���� ����
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
