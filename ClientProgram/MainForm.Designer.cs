namespace ClientProgram
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            tableLayoutPanel1 = new TableLayoutPanel();
            button_Send = new Button();
            richTextBox_Message = new RichTextBox();
            tableLayoutPanel_Message = new TableLayoutPanel();
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 90.625F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 9.375F));
            tableLayoutPanel1.Controls.Add(button_Send, 1, 0);
            tableLayoutPanel1.Controls.Add(richTextBox_Message, 0, 0);
            tableLayoutPanel1.Dock = DockStyle.Bottom;
            tableLayoutPanel1.Location = new Point(0, 406);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.Size = new Size(800, 44);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // button_Send
            // 
            button_Send.Dock = DockStyle.Fill;
            button_Send.Enabled = false;
            button_Send.Location = new Point(728, 3);
            button_Send.Name = "button_Send";
            button_Send.Size = new Size(69, 38);
            button_Send.TabIndex = 0;
            button_Send.Text = "전 송";
            button_Send.UseVisualStyleBackColor = true;
            button_Send.Click += button_Send_Click;
            // 
            // richTextBox_Message
            // 
            richTextBox_Message.Dock = DockStyle.Fill;
            richTextBox_Message.Location = new Point(3, 3);
            richTextBox_Message.Name = "richTextBox_Message";
            richTextBox_Message.Size = new Size(719, 38);
            richTextBox_Message.TabIndex = 1;
            richTextBox_Message.Text = "";
            richTextBox_Message.KeyDown += richTextBox_Message_KeyDown;
            richTextBox_Message.KeyUp += richTextBox_Message_KeyUp;
            // 
            // tableLayoutPanel_Message
            // 
            tableLayoutPanel_Message.BackColor = SystemColors.ActiveCaption;
            tableLayoutPanel_Message.ColumnCount = 1;
            tableLayoutPanel_Message.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel_Message.Dock = DockStyle.Fill;
            tableLayoutPanel_Message.Location = new Point(0, 0);
            tableLayoutPanel_Message.Name = "tableLayoutPanel_Message";
            tableLayoutPanel_Message.RowCount = 1;
            tableLayoutPanel_Message.RowStyles.Add(new RowStyle());
            tableLayoutPanel_Message.Size = new Size(800, 406);
            tableLayoutPanel_Message.TabIndex = 1;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(tableLayoutPanel_Message);
            Controls.Add(tableLayoutPanel1);
            Name = "MainForm";
            Text = "채팅 프로그램";
            FormClosed += Form1_FormClosed;
            Load += MainForm_Load;
            tableLayoutPanel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private Button button_Send;
        private RichTextBox richTextBox_Message;
        private TableLayoutPanel tableLayoutPanel_Message;
    }
}
