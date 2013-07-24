namespace me2dayBackup
{
    partial class Form1
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

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다.
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.btnMe2dayBackup = new System.Windows.Forms.Button();
            this.StatusMsg = new System.Windows.Forms.TextBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.btnMe2dayLogin = new System.Windows.Forms.Button();
            this.cbSaveImage = new System.Windows.Forms.CheckBox();
            this.JoinCalendar = new System.Windows.Forms.MonthCalendar();
            this.label1 = new System.Windows.Forms.Label();
            this.tbMe2dayJoinDate = new System.Windows.Forms.TextBox();
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.label6 = new System.Windows.Forms.Label();
            this.tbMe2dayID = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbRecentDate = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.RecentCalendar = new System.Windows.Forms.MonthCalendar();
            this.btnDebug = new System.Windows.Forms.Button();
            this.btnMe2dayExport = new System.Windows.Forms.Button();
            this.cbResultView = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // btnMe2dayBackup
            // 
            this.btnMe2dayBackup.Location = new System.Drawing.Point(141, 6);
            this.btnMe2dayBackup.Name = "btnMe2dayBackup";
            this.btnMe2dayBackup.Size = new System.Drawing.Size(74, 35);
            this.btnMe2dayBackup.TabIndex = 6;
            this.btnMe2dayBackup.Text = "2. 백업";
            this.btnMe2dayBackup.UseVisualStyleBackColor = true;
            this.btnMe2dayBackup.Click += new System.EventHandler(this.btnMe2dayBackup_Click);
            // 
            // StatusMsg
            // 
            this.StatusMsg.BackColor = System.Drawing.Color.Black;
            this.StatusMsg.ForeColor = System.Drawing.Color.Lime;
            this.StatusMsg.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.StatusMsg.Location = new System.Drawing.Point(14, 170);
            this.StatusMsg.Multiline = true;
            this.StatusMsg.Name = "StatusMsg";
            this.StatusMsg.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.StatusMsg.Size = new System.Drawing.Size(271, 138);
            this.StatusMsg.TabIndex = 1;
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // btnMe2dayLogin
            // 
            this.btnMe2dayLogin.Location = new System.Drawing.Point(12, 6);
            this.btnMe2dayLogin.Name = "btnMe2dayLogin";
            this.btnMe2dayLogin.Size = new System.Drawing.Size(123, 35);
            this.btnMe2dayLogin.TabIndex = 1;
            this.btnMe2dayLogin.Text = "1. me2day Login";
            this.btnMe2dayLogin.UseVisualStyleBackColor = true;
            this.btnMe2dayLogin.Click += new System.EventHandler(this.btnMe2dayLogin_Click);
            // 
            // cbSaveImage
            // 
            this.cbSaveImage.AutoSize = true;
            this.cbSaveImage.Checked = true;
            this.cbSaveImage.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbSaveImage.Location = new System.Drawing.Point(14, 148);
            this.cbSaveImage.Name = "cbSaveImage";
            this.cbSaveImage.Size = new System.Drawing.Size(131, 16);
            this.cbSaveImage.TabIndex = 5;
            this.cbSaveImage.Text = "me2photo 저장하기";
            this.cbSaveImage.UseVisualStyleBackColor = true;
            // 
            // JoinCalendar
            // 
            this.JoinCalendar.Location = new System.Drawing.Point(37, 136);
            this.JoinCalendar.MaxDate = new System.DateTime(2015, 12, 31, 0, 0, 0, 0);
            this.JoinCalendar.MaxSelectionCount = 1;
            this.JoinCalendar.MinDate = new System.DateTime(2007, 2, 20, 0, 0, 0, 0);
            this.JoinCalendar.Name = "JoinCalendar";
            this.JoinCalendar.TabIndex = 6;
            this.JoinCalendar.Visible = false;
            this.JoinCalendar.DateSelected += new System.Windows.Forms.DateRangeEventHandler(this.monthCalendar1_DateSelected);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 115);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(81, 12);
            this.label1.TabIndex = 8;
            this.label1.Text = "미투 생성일 : ";
            // 
            // tbMe2dayJoinDate
            // 
            this.tbMe2dayJoinDate.Location = new System.Drawing.Point(101, 112);
            this.tbMe2dayJoinDate.Name = "tbMe2dayJoinDate";
            this.tbMe2dayJoinDate.ReadOnly = true;
            this.tbMe2dayJoinDate.Size = new System.Drawing.Size(100, 21);
            this.tbMe2dayJoinDate.TabIndex = 4;
            this.tbMe2dayJoinDate.Click += new System.EventHandler(this.tbMe2dayJoinDate_Click);
            // 
            // webBrowser1
            // 
            this.webBrowser1.Location = new System.Drawing.Point(26, 314);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.ScrollBarsEnabled = false;
            this.webBrowser1.Size = new System.Drawing.Size(239, 60);
            this.webBrowser1.TabIndex = 13;
            this.webBrowser1.Url = new System.Uri("http://api.ssemi.net/ads/me2daybackup.htm", System.UriKind.Absolute);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(27, 58);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(77, 12);
            this.label6.TabIndex = 15;
            this.label6.Text = "me2day ID : ";
            // 
            // tbMe2dayID
            // 
            this.tbMe2dayID.BackColor = System.Drawing.SystemColors.ControlLight;
            this.tbMe2dayID.ForeColor = System.Drawing.Color.Black;
            this.tbMe2dayID.Location = new System.Drawing.Point(101, 55);
            this.tbMe2dayID.Name = "tbMe2dayID";
            this.tbMe2dayID.ReadOnly = true;
            this.tbMe2dayID.Size = new System.Drawing.Size(100, 21);
            this.tbMe2dayID.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(35, 85);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 12);
            this.label2.TabIndex = 17;
            this.label2.Text = "기준 날짜 : ";
            // 
            // tbRecentDate
            // 
            this.tbRecentDate.Location = new System.Drawing.Point(101, 82);
            this.tbRecentDate.Name = "tbRecentDate";
            this.tbRecentDate.ReadOnly = true;
            this.tbRecentDate.Size = new System.Drawing.Size(100, 21);
            this.tbRecentDate.TabIndex = 3;
            this.tbRecentDate.Click += new System.EventHandler(this.tbRecentDate_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(207, 91);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 19;
            this.label3.Text = "에서";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(207, 121);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 12);
            this.label4.TabIndex = 19;
            this.label4.Text = "까지";
            // 
            // RecentCalendar
            // 
            this.RecentCalendar.Location = new System.Drawing.Point(37, 106);
            this.RecentCalendar.MaxDate = new System.DateTime(2015, 12, 31, 0, 0, 0, 0);
            this.RecentCalendar.MaxSelectionCount = 1;
            this.RecentCalendar.MinDate = new System.DateTime(2007, 2, 22, 0, 0, 0, 0);
            this.RecentCalendar.Name = "RecentCalendar";
            this.RecentCalendar.TabIndex = 20;
            this.RecentCalendar.Visible = false;
            this.RecentCalendar.DateSelected += new System.Windows.Forms.DateRangeEventHandler(this.RecentCalendar_DateSelected);
            // 
            // btnDebug
            // 
            this.btnDebug.Location = new System.Drawing.Point(208, 55);
            this.btnDebug.Name = "btnDebug";
            this.btnDebug.Size = new System.Drawing.Size(75, 23);
            this.btnDebug.TabIndex = 21;
            this.btnDebug.Text = "Debug";
            this.btnDebug.UseVisualStyleBackColor = true;
            this.btnDebug.Visible = false;
            this.btnDebug.Click += new System.EventHandler(this.btnDebug_Click);
            // 
            // btnMe2dayExport
            // 
            this.btnMe2dayExport.Location = new System.Drawing.Point(221, 6);
            this.btnMe2dayExport.Name = "btnMe2dayExport";
            this.btnMe2dayExport.Size = new System.Drawing.Size(62, 35);
            this.btnMe2dayExport.TabIndex = 22;
            this.btnMe2dayExport.Text = "3. 변환";
            this.btnMe2dayExport.UseVisualStyleBackColor = true;
            this.btnMe2dayExport.Click += new System.EventHandler(this.btn_Export_Click);
            // 
            // cbResultView
            // 
            this.cbResultView.AutoSize = true;
            this.cbResultView.Location = new System.Drawing.Point(154, 148);
            this.cbResultView.Name = "cbResultView";
            this.cbResultView.Size = new System.Drawing.Size(120, 16);
            this.cbResultView.TabIndex = 23;
            this.cbResultView.Text = "변환 후 결과 보기";
            this.cbResultView.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(295, 382);
            this.Controls.Add(this.cbResultView);
            this.Controls.Add(this.btnMe2dayExport);
            this.Controls.Add(this.btnDebug);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tbRecentDate);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbMe2dayID);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.webBrowser1);
            this.Controls.Add(this.StatusMsg);
            this.Controls.Add(this.tbMe2dayJoinDate);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbSaveImage);
            this.Controls.Add(this.btnMe2dayLogin);
            this.Controls.Add(this.btnMe2dayBackup);
            this.Controls.Add(this.RecentCalendar);
            this.Controls.Add(this.JoinCalendar);
            this.Name = "Form1";
            this.Text = "me2Backup";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnMe2dayBackup;
        private System.Windows.Forms.TextBox StatusMsg;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button btnMe2dayLogin;
        private System.Windows.Forms.CheckBox cbSaveImage;
        private System.Windows.Forms.MonthCalendar JoinCalendar;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbMe2dayJoinDate;
        private System.Windows.Forms.WebBrowser webBrowser1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tbMe2dayID;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbRecentDate;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.MonthCalendar RecentCalendar;
        private System.Windows.Forms.Button btnDebug;
        private System.Windows.Forms.Button btnMe2dayExport;
        private System.Windows.Forms.CheckBox cbResultView;
    }
}

