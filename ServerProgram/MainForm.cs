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
using System.Net.Sockets;
using System.Net;
using System.Diagnostics;
using System.Reflection.Emit;

namespace ServerProgram
{
    public partial class MainForm : Form
    {
        Server server;
        bool IsStartServer = false;

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

        private void Create_Server()
        {
            server = new Server(2, this);
        }

        public void Update_Message(string Time, string Message) 
        {
            if (this.listView_Log.InvokeRequired)
            {
                this.listView_Log.Invoke(new MethodInvoker(delegate ()
                {
                    ListViewItem Item = new ListViewItem(Time);
                    Item.SubItems.Add(Message);
                    this.listView_Log.Items.Add(Item);
                }));
            }
            else
            {
                ListViewItem Item = new ListViewItem(Time);
                Item.SubItems.Add(Message);
                this.listView_Log.Items.Add(Item);
            }            
        }

        /// <summary>
        /// Event Function
        /// </summary>

        private void button_Start_Click(object sender, EventArgs e)
        {
            this.button_Start.BackColor = Color.SeaGreen;
            this.button_Start.ForeColor = Color.WhiteSmoke;
            this.button_Stop.BackColor = SystemColors.Control;
            this.button_Stop.ForeColor = Color.Black;

            if(IsStartServer == false)
            {
                IsStartServer = true;

                Create_Server();
                IPEndPoint EndPoint = new IPEndPoint(IPAddress.Any, 5200);
                server.Start(EndPoint);
            }            
        }

        private void button_Stop_Click(object sender, EventArgs e)
        {
            this.button_Start.BackColor = SystemColors.Control;
            this.button_Start.ForeColor = Color.Black;
            this.button_Stop.BackColor = Color.Red;
            this.button_Stop.ForeColor = Color.WhiteSmoke;

            if (IsStartServer == true)
            {
                IsStartServer = false;

                server.CloseAllClientSocket();
            }            
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Process.GetCurrentProcess().Kill();
        }
    }
}
