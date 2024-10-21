namespace ClientProgram
{
    partial class MessageControl
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
            label1 = new Label();
            label2 = new Label();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.Gold;
            label1.Dock = DockStyle.Right;
            label1.Location = new Point(663, 0);
            label1.MaximumSize = new Size(250, 0);
            label1.Name = "label1";
            label1.Padding = new Padding(10);
            label1.Size = new Size(107, 35);
            label1.TabIndex = 2;
            label1.Text = "label1label1lab";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = Color.Transparent;
            label2.Dock = DockStyle.Right;
            label2.Font = new Font("맑은 고딕", 6F);
            label2.ForeColor = SystemColors.ControlDarkDark;
            label2.Location = new Point(601, 0);
            label2.MaximumSize = new Size(250, 0);
            label2.Name = "label2";
            label2.Padding = new Padding(20, 20, 0, 0);
            label2.Size = new Size(62, 31);
            label2.TabIndex = 3;
            label2.Text = "오후 10:30";
            label2.TextAlign = ContentAlignment.BottomRight;
            // 
            // MessageControl
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(label2);
            Controls.Add(label1);
            Name = "MessageControl";
            Size = new Size(770, 35);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label label1;
        private Label label2;
    }
}
