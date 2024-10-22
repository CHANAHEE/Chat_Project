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
    public partial class MessageControl : UserControl
    {
        public MessageControl(string Message, DateTime Time)
        {
            InitializeComponent();

            this.label_Message.Text = Message;
            string FormattedTime = Time.ToString("tt h:mm", CultureInfo.CurrentCulture);

            this.label_SendTime.Text = FormattedTime.Replace("AM","오전").Replace("PM", "오후");
        }
    }
}
