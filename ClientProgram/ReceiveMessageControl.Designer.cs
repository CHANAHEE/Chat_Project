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
            label_ReceiveTime = new Label();
            label_ReceiveMessage = new Label();
            SuspendLayout();
            // 
            // label_ReceiveTime
            // 
            label_ReceiveTime.AutoSize = true;
            label_ReceiveTime.BackColor = Color.Transparent;
            label_ReceiveTime.Dock = DockStyle.Left;
            label_ReceiveTime.Font = new Font("맑은 고딕", 6F);
            label_ReceiveTime.ForeColor = SystemColors.ControlDarkDark;
            label_ReceiveTime.Location = new Point(107, 0);
            label_ReceiveTime.MaximumSize = new Size(250, 0);
            label_ReceiveTime.Name = "label_ReceiveTime";
            label_ReceiveTime.Padding = new Padding(4, 24, 3, 0);
            label_ReceiveTime.Size = new Size(49, 35);
            label_ReceiveTime.TabIndex = 5;
            label_ReceiveTime.Text = "오후 10:30";
            label_ReceiveTime.TextAlign = ContentAlignment.BottomRight;
            // 
            // label_ReceiveMessage
            // 
            label_ReceiveMessage.AutoSize = true;
            label_ReceiveMessage.BackColor = Color.WhiteSmoke;
            label_ReceiveMessage.Dock = DockStyle.Left;
            label_ReceiveMessage.Location = new Point(0, 0);
            label_ReceiveMessage.MaximumSize = new Size(250, 0);
            label_ReceiveMessage.Name = "label_ReceiveMessage";
            label_ReceiveMessage.Padding = new Padding(10);
            label_ReceiveMessage.Size = new Size(107, 35);
            label_ReceiveMessage.TabIndex = 4;
            label_ReceiveMessage.Text = "label1label1lab";
            // 
            // ReceiveMessageControl
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.IndianRed;
            Controls.Add(label_ReceiveTime);
            Controls.Add(label_ReceiveMessage);
            Name = "ReceiveMessageControl";
            Size = new Size(770, 35);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label_ReceiveTime;
        private Label label_ReceiveMessage;
    }
}
