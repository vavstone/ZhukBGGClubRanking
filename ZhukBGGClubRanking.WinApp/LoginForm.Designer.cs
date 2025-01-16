namespace ZhukBGGClubRanking.WinApp
{
    partial class LoginForm
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
            this.btSubmit = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tbHostingPassword = new System.Windows.Forms.TextBox();
            this.lblHostingPassword = new System.Windows.Forms.Label();
            this.tbHostingLogin = new System.Windows.Forms.TextBox();
            this.lblHostingLogin = new System.Windows.Forms.Label();
            this.tbHostingUrl = new System.Windows.Forms.TextBox();
            this.lblHostingUrl = new System.Windows.Forms.Label();
            this.tbPassword = new System.Windows.Forms.TextBox();
            this.tbLogin = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btSubmit
            // 
            this.btSubmit.Location = new System.Drawing.Point(170, 180);
            this.btSubmit.Name = "btSubmit";
            this.btSubmit.Size = new System.Drawing.Size(86, 23);
            this.btSubmit.TabIndex = 0;
            this.btSubmit.Text = "Подтвердить";
            this.btSubmit.UseVisualStyleBackColor = true;
            this.btSubmit.Click += new System.EventHandler(this.btSubmit_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tbHostingPassword);
            this.groupBox1.Controls.Add(this.lblHostingPassword);
            this.groupBox1.Controls.Add(this.tbHostingLogin);
            this.groupBox1.Controls.Add(this.lblHostingLogin);
            this.groupBox1.Controls.Add(this.tbHostingUrl);
            this.groupBox1.Controls.Add(this.lblHostingUrl);
            this.groupBox1.Controls.Add(this.tbPassword);
            this.groupBox1.Controls.Add(this.tbLogin);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(384, 162);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Параметры входа";
            // 
            // tbHostingPassword
            // 
            this.tbHostingPassword.Location = new System.Drawing.Point(122, 134);
            this.tbHostingPassword.Name = "tbHostingPassword";
            this.tbHostingPassword.Size = new System.Drawing.Size(235, 20);
            this.tbHostingPassword.TabIndex = 10;
            this.tbHostingPassword.TextChanged += new System.EventHandler(this.textBox3_TextChanged);
            // 
            // lblHostingPassword
            // 
            this.lblHostingPassword.AutoSize = true;
            this.lblHostingPassword.Location = new System.Drawing.Point(23, 137);
            this.lblHostingPassword.Name = "lblHostingPassword";
            this.lblHostingPassword.Size = new System.Drawing.Size(93, 13);
            this.lblHostingPassword.TabIndex = 9;
            this.lblHostingPassword.Text = "Пароль хостинга";
            this.lblHostingPassword.Click += new System.EventHandler(this.label5_Click);
            // 
            // tbHostingLogin
            // 
            this.tbHostingLogin.Location = new System.Drawing.Point(122, 108);
            this.tbHostingLogin.Name = "tbHostingLogin";
            this.tbHostingLogin.Size = new System.Drawing.Size(235, 20);
            this.tbHostingLogin.TabIndex = 8;
            this.tbHostingLogin.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
            // 
            // lblHostingLogin
            // 
            this.lblHostingLogin.AutoSize = true;
            this.lblHostingLogin.Location = new System.Drawing.Point(23, 111);
            this.lblHostingLogin.Name = "lblHostingLogin";
            this.lblHostingLogin.Size = new System.Drawing.Size(86, 13);
            this.lblHostingLogin.TabIndex = 7;
            this.lblHostingLogin.Text = "Логин хостинга";
            this.lblHostingLogin.Click += new System.EventHandler(this.label4_Click);
            // 
            // tbHostingUrl
            // 
            this.tbHostingUrl.Location = new System.Drawing.Point(122, 82);
            this.tbHostingUrl.Name = "tbHostingUrl";
            this.tbHostingUrl.Size = new System.Drawing.Size(235, 20);
            this.tbHostingUrl.TabIndex = 6;
            this.tbHostingUrl.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // lblHostingUrl
            // 
            this.lblHostingUrl.AutoSize = true;
            this.lblHostingUrl.Location = new System.Drawing.Point(23, 85);
            this.lblHostingUrl.Name = "lblHostingUrl";
            this.lblHostingUrl.Size = new System.Drawing.Size(77, 13);
            this.lblHostingUrl.TabIndex = 5;
            this.lblHostingUrl.Text = "URL хостинга";
            this.lblHostingUrl.Click += new System.EventHandler(this.label3_Click);
            // 
            // tbPassword
            // 
            this.tbPassword.Location = new System.Drawing.Point(122, 56);
            this.tbPassword.Name = "tbPassword";
            this.tbPassword.PasswordChar = '*';
            this.tbPassword.Size = new System.Drawing.Size(235, 20);
            this.tbPassword.TabIndex = 3;
            this.tbPassword.Text = "VAV";
            this.tbPassword.TextChanged += new System.EventHandler(this.tbPassword_TextChanged);
            // 
            // tbLogin
            // 
            this.tbLogin.Location = new System.Drawing.Point(122, 30);
            this.tbLogin.Name = "tbLogin";
            this.tbLogin.Size = new System.Drawing.Size(235, 20);
            this.tbLogin.TabIndex = 2;
            this.tbLogin.Text = "VAV";
            this.tbLogin.TextChanged += new System.EventHandler(this.tbLogin_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(23, 59);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Пароль";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Логин";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // LoginForm
            // 
            this.AcceptButton = this.btSubmit;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(410, 208);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btSubmit);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LoginForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ZhukBGGClubRanking. Вход";
            this.Load += new System.EventHandler(this.LoginForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btSubmit;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbPassword;
        private System.Windows.Forms.TextBox tbLogin;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbHostingUrl;
        private System.Windows.Forms.Label lblHostingUrl;
        private System.Windows.Forms.TextBox tbHostingPassword;
        private System.Windows.Forms.Label lblHostingPassword;
        private System.Windows.Forms.TextBox tbHostingLogin;
        private System.Windows.Forms.Label lblHostingLogin;
    }
}