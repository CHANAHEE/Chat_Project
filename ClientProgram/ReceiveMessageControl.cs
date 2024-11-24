using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace ClientProgram
{
    public partial class ReceiveMessageControl : UserControl
    {
        public ReceiveMessageControl(string Message, DateTime RecvTime)
        {
            InitializeComponent();

            this.label_ReceiveMessage.Text = Message;
            string FormattedTime = RecvTime.ToString("tt h:mm", CultureInfo.CurrentCulture);

            this.label_ReceiveTime.Text = FormattedTime.Replace("AM", "오전").Replace("PM", "오후");

            this.Height = this.label_ReceiveMessage.Height + this.Padding.Bottom;
        }
    }
}
