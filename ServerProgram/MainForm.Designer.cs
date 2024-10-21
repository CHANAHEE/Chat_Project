namespace ServerProgram
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            listView_Log = new ListView();
            button_Start = new Button();
            tableLayoutPanel1 = new TableLayoutPanel();
            button_Stop = new Button();
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // listView_Log
            // 
            listView_Log.Dock = DockStyle.Top;
            listView_Log.Location = new Point(0, 0);
            listView_Log.Name = "listView_Log";
            listView_Log.Size = new Size(603, 390);
            listView_Log.TabIndex = 0;
            listView_Log.UseCompatibleStateImageBehavior = false;
            // 
            // button_Start
            // 
            button_Start.Dock = DockStyle.Fill;
            button_Start.Font = new Font("맑은 고딕 Semilight", 15F);
            button_Start.Location = new Point(3, 3);
            button_Start.Name = "button_Start";
            button_Start.Size = new Size(295, 54);
            button_Start.TabIndex = 1;
            button_Start.Text = "Server Start";
            button_Start.UseVisualStyleBackColor = true;
            button_Start.Click += button_Start_Click;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Controls.Add(button_Stop, 1, 0);
            tableLayoutPanel1.Controls.Add(button_Start, 0, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 390);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.Size = new Size(603, 60);
            tableLayoutPanel1.TabIndex = 2;
            // 
            // button_Stop
            // 
            button_Stop.Dock = DockStyle.Fill;
            button_Stop.Font = new Font("맑은 고딕 Semilight", 15F);
            button_Stop.Location = new Point(304, 3);
            button_Stop.Name = "button_Stop";
            button_Stop.Size = new Size(296, 54);
            button_Stop.TabIndex = 2;
            button_Stop.Text = "Server Stop";
            button_Stop.UseVisualStyleBackColor = true;
            button_Stop.Click += button_Stop_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(603, 450);
            Controls.Add(tableLayoutPanel1);
            Controls.Add(listView_Log);
            Name = "MainForm";
            Text = "Chatting Server";
            FormClosed += MainForm_FormClosed;
            tableLayoutPanel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private ListView listView_Log;
        private Button button_Start;
        private TableLayoutPanel tableLayoutPanel1;
        private Button button_Stop;
    }
}