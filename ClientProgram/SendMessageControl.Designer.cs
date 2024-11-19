namespace ClientProgram
{
    partial class SendMessageControl
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
            label_SendMessage = new Label();
            label_SendTime = new Label();
            SuspendLayout();
            // 
            // label_SendMessage
            // 
            label_SendMessage.AutoSize = true;
            label_SendMessage.BackColor = Color.Gold;
            label_SendMessage.Dock = DockStyle.Right;
            label_SendMessage.Location = new Point(658, 5);
            label_SendMessage.Margin = new Padding(0);
            label_SendMessage.MaximumSize = new Size(250, 0);
            label_SendMessage.Name = "label_SendMessage";
            label_SendMessage.Padding = new Padding(10, 12, 10, 10);
            label_SendMessage.Size = new Size(107, 37);
            label_SendMessage.TabIndex = 2;
            label_SendMessage.Text = "label1label1lab";
            // 
            // label_SendTime
            // 
            label_SendTime.AutoSize = true;
            label_SendTime.BackColor = Color.Transparent;
            label_SendTime.Dock = DockStyle.Right;
            label_SendTime.Font = new Font("맑은 고딕", 6F);
            label_SendTime.ForeColor = SystemColors.ControlDarkDark;
            label_SendTime.Location = new Point(593, 5);
            label_SendTime.MaximumSize = new Size(250, 0);
            label_SendTime.Name = "label_SendTime";
            label_SendTime.Padding = new Padding(20, 20, 3, 0);
            label_SendTime.Size = new Size(65, 31);
            label_SendTime.TabIndex = 3;
            label_SendTime.Text = "오후 10:30";
            label_SendTime.TextAlign = ContentAlignment.BottomRight;
            // 
            // SendMessageControl
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Chocolate;
            Controls.Add(label_SendTime);
            Controls.Add(label_SendMessage);
            Name = "SendMessageControl";
            Padding = new Padding(5);
            Size = new Size(770, 43);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label label_SendMessage;
        private Label label_SendTime;
    }
}
