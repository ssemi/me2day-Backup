namespace me2dayBackup
{
    partial class me2dayLoginForm
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
            this.wbMe2dayLogin = new System.Windows.Forms.WebBrowser();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.SuspendLayout();
            // 
            // wbMe2dayLogin
            // 
            this.wbMe2dayLogin.Location = new System.Drawing.Point(12, 12);
            this.wbMe2dayLogin.MinimumSize = new System.Drawing.Size(20, 20);
            this.wbMe2dayLogin.Name = "wbMe2dayLogin";
            this.wbMe2dayLogin.Size = new System.Drawing.Size(597, 407);
            this.wbMe2dayLogin.TabIndex = 22;
            this.wbMe2dayLogin.Navigated += new System.Windows.Forms.WebBrowserNavigatedEventHandler(this.wbMe2dayLogin_Navigated);
            // 
            // me2dayLoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(621, 430);
            this.Controls.Add(this.wbMe2dayLogin);
            this.Name = "me2dayLoginForm";
            this.Text = "me2day Login Form";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.WebBrowser wbMe2dayLogin;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
    }
}