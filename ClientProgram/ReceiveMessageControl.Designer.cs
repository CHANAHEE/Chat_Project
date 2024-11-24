namespace ClientProgram
{
    partial class ReceiveMessageControl
    {
        /// <summary> 
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 구성 요소 디자이너에서 생성한 코드

        /// <summary> 
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            label_ReceiveMessage = new Label();
            label_ReceiveTime = new Label();
            panel1 = new Panel();
            panel2 = new Panel();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // label_ReceiveMessage
            // 
            label_ReceiveMessage.AutoSize = true;
            label_ReceiveMessage.BackColor = Color.WhiteSmoke;
            label_ReceiveMessage.Location = new Point(0, 2);
            label_ReceiveMessage.Margin = new Padding(0);
            label_ReceiveMessage.MaximumSize = new Size(250, 0);
            label_ReceiveMessage.Name = "label_ReceiveMessage";
            label_ReceiveMessage.Padding = new Padding(10, 7, 10, 15);
            label_ReceiveMessage.Size = new Size(107, 37);
            label_ReceiveMessage.TabIndex = 2;
            label_ReceiveMessage.Text = "label1label1lab";
            // 
            // label_ReceiveTime
            // 
            label_ReceiveTime.BackColor = Color.Transparent;
            label_ReceiveTime.Dock = DockStyle.Bottom;
            label_ReceiveTime.Font = new Font("맑은 고딕", 6F);
            label_ReceiveTime.ForeColor = SystemColors.ControlDarkDark;
            label_ReceiveTime.Location = new Point(0, 0);
            label_ReceiveTime.Name = "label_ReceiveTime";
            label_ReceiveTime.Padding = new Padding(20, 26, 3, 0);
            label_ReceiveTime.Size = new Size(66, 37);
            label_ReceiveTime.TabIndex = 3;
            label_ReceiveTime.Text = "오후 10:30";
            label_ReceiveTime.TextAlign = ContentAlignment.BottomRight;
            // 
            // panel1
            // 
            panel1.BackColor = Color.Transparent;
            panel1.Controls.Add(label_ReceiveTime);
            panel1.Dock = DockStyle.Left;
            panel1.Location = new Point(112, 5);
            panel1.Name = "panel1";
            panel1.Size = new Size(66, 37);
            panel1.TabIndex = 6;
            // 
            // panel2
            // 
            panel2.AutoSize = true;
            panel2.Controls.Add(label_ReceiveMessage);
            panel2.Dock = DockStyle.Left;
            panel2.Location = new Point(5, 5);
            panel2.Name = "panel2";
            panel2.Padding = new Padding(0, 0, 0, 5);
            panel2.Size = new Size(107, 37);
            panel2.TabIndex = 7;
            // 
            // ReceiveMessageControl
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Transparent;
            Controls.Add(panel1);
            Controls.Add(panel2);
            Name = "ReceiveMessageControl";
            Padding = new Padding(5);
            Size = new Size(770, 47);
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label_ReceiveMessage;
        private Label label_ReceiveTime;
        private Panel panel1;
        private Panel panel2;
    }
}
