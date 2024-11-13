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

namespace ClientProgram
{
    public partial class SendMessageControl : UserControl
    {
        public SendMessageControl(string Message, DateTime SendTime)
        {
            InitializeComponent();

            this.label_SendMessage.Text = Message;
            string FormattedTime = SendTime.ToString("tt h:mm", CultureInfo.CurrentCulture);

            this.label_SendTime.Text = FormattedTime.Replace("AM","오전").Replace("PM", "오후");
        }
    }
}
