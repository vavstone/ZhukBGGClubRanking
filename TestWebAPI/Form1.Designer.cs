namespace TestWebAPI
{
    partial class Form1
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
            this.tbUrl = new System.Windows.Forms.TextBox();
            this.tbLogin = new System.Windows.Forms.TextBox();
            this.tbPassword = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbNewFullName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tbNewEmail = new System.Windows.Forms.TextBox();
            this.btCreateUser = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.btGetGameCollection = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tbNewPassword = new System.Windows.Forms.TextBox();
            this.tbNewLogin = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.btGetTestStringAuth = new System.Windows.Forms.Button();
            this.tbTestResult = new System.Windows.Forms.TextBox();
            this.btGetTestString = new System.Windows.Forms.Button();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.btTestWithJwc = new System.Windows.Forms.Button();
            this.tbLoginResult = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btLogin = new System.Windows.Forms.Button();
            this.tbUserName = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.tbUserPassword = new System.Windows.Forms.TextBox();
            this.tbWithTokenResult = new System.Windows.Forms.TextBox();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.tbPathToRatingFile = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.btUploadRatingFile = new System.Windows.Forms.Button();
            this.tbRatingList = new System.Windows.Forms.TextBox();
            this.btShowRatings = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbUrl
            // 
            this.tbUrl.Location = new System.Drawing.Point(13, 13);
            this.tbUrl.Name = "tbUrl";
            this.tbUrl.Size = new System.Drawing.Size(775, 20);
            this.tbUrl.TabIndex = 0;
            this.tbUrl.Text = "http://localhost:5116/";
            // 
            // tbLogin
            // 
            this.tbLogin.Location = new System.Drawing.Point(12, 39);
            this.tbLogin.Name = "tbLogin";
            this.tbLogin.Size = new System.Drawing.Size(222, 20);
            this.tbLogin.TabIndex = 3;
            // 
            // tbPassword
            // 
            this.tbPassword.Location = new System.Drawing.Point(240, 39);
            this.tbPassword.Name = "tbPassword";
            this.tbPassword.Size = new System.Drawing.Size(222, 20);
            this.tbPassword.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(46, 60);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Логин";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(46, 99);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Имя";
            // 
            // tbNewFullName
            // 
            this.tbNewFullName.Location = new System.Drawing.Point(106, 96);
            this.tbNewFullName.Name = "tbNewFullName";
            this.tbNewFullName.Size = new System.Drawing.Size(100, 20);
            this.tbNewFullName.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(46, 175);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(31, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "email";
            // 
            // tbNewEmail
            // 
            this.tbNewEmail.Location = new System.Drawing.Point(106, 172);
            this.tbNewEmail.Name = "tbNewEmail";
            this.tbNewEmail.Size = new System.Drawing.Size(100, 20);
            this.tbNewEmail.TabIndex = 11;
            // 
            // btCreateUser
            // 
            this.btCreateUser.Location = new System.Drawing.Point(49, 218);
            this.btCreateUser.Name = "btCreateUser";
            this.btCreateUser.Size = new System.Drawing.Size(150, 23);
            this.btCreateUser.TabIndex = 13;
            this.btCreateUser.Text = "Создать пользователя";
            this.btCreateUser.UseVisualStyleBackColor = true;
            this.btCreateUser.Click += new System.EventHandler(this.btCreateUser_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Controls.Add(this.tabPage5);
            this.tabControl1.Location = new System.Drawing.Point(13, 232);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1127, 457);
            this.tabControl1.TabIndex = 14;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.textBox2);
            this.tabPage1.Controls.Add(this.btGetGameCollection);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1119, 431);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(15, 46);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox2.Size = new System.Drawing.Size(215, 379);
            this.textBox2.TabIndex = 4;
            // 
            // btGetGameCollection
            // 
            this.btGetGameCollection.Location = new System.Drawing.Point(15, 6);
            this.btGetGameCollection.Name = "btGetGameCollection";
            this.btGetGameCollection.Size = new System.Drawing.Size(75, 23);
            this.btGetGameCollection.TabIndex = 3;
            this.btGetGameCollection.Text = "Игры";
            this.btGetGameCollection.UseVisualStyleBackColor = true;
            this.btGetGameCollection.Click += new System.EventHandler(this.btGetGameCollection_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.tbNewPassword);
            this.tabPage2.Controls.Add(this.tbNewLogin);
            this.tabPage2.Controls.Add(this.btCreateUser);
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Controls.Add(this.label1);
            this.tabPage2.Controls.Add(this.tbNewEmail);
            this.tabPage2.Controls.Add(this.tbNewFullName);
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Controls.Add(this.label2);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1119, 431);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tbNewPassword
            // 
            this.tbNewPassword.Location = new System.Drawing.Point(106, 135);
            this.tbNewPassword.Name = "tbNewPassword";
            this.tbNewPassword.Size = new System.Drawing.Size(100, 20);
            this.tbNewPassword.TabIndex = 15;
            // 
            // tbNewLogin
            // 
            this.tbNewLogin.Location = new System.Drawing.Point(106, 57);
            this.tbNewLogin.Name = "tbNewLogin";
            this.tbNewLogin.Size = new System.Drawing.Size(100, 20);
            this.tbNewLogin.TabIndex = 14;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(46, 135);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Пароль";
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.btGetTestStringAuth);
            this.tabPage3.Controls.Add(this.tbTestResult);
            this.tabPage3.Controls.Add(this.btGetTestString);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(1119, 431);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "tabPage3";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // btGetTestStringAuth
            // 
            this.btGetTestStringAuth.Location = new System.Drawing.Point(7, 45);
            this.btGetTestStringAuth.Name = "btGetTestStringAuth";
            this.btGetTestStringAuth.Size = new System.Drawing.Size(266, 23);
            this.btGetTestStringAuth.TabIndex = 2;
            this.btGetTestStringAuth.Text = "Тестовая строка (аут)";
            this.btGetTestStringAuth.UseVisualStyleBackColor = true;
            this.btGetTestStringAuth.Click += new System.EventHandler(this.btGetTestStringAuth_Click);
            // 
            // tbTestResult
            // 
            this.tbTestResult.Location = new System.Drawing.Point(7, 260);
            this.tbTestResult.Name = "tbTestResult";
            this.tbTestResult.Size = new System.Drawing.Size(188, 20);
            this.tbTestResult.TabIndex = 1;
            // 
            // btGetTestString
            // 
            this.btGetTestString.Location = new System.Drawing.Point(7, 7);
            this.btGetTestString.Name = "btGetTestString";
            this.btGetTestString.Size = new System.Drawing.Size(266, 23);
            this.btGetTestString.TabIndex = 0;
            this.btGetTestString.Text = "Тестовая строка";
            this.btGetTestString.UseVisualStyleBackColor = true;
            this.btGetTestString.Click += new System.EventHandler(this.btGetTestString_Click);
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.tbWithTokenResult);
            this.tabPage4.Controls.Add(this.btTestWithJwc);
            this.tabPage4.Controls.Add(this.label5);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(1119, 290);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "tabPage4";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // btTestWithJwc
            // 
            this.btTestWithJwc.Location = new System.Drawing.Point(6, 7);
            this.btTestWithJwc.Name = "btTestWithJwc";
            this.btTestWithJwc.Size = new System.Drawing.Size(163, 23);
            this.btTestWithJwc.TabIndex = 4;
            this.btTestWithJwc.Text = "Идти с ключом";
            this.btTestWithJwc.UseVisualStyleBackColor = true;
            this.btTestWithJwc.Click += new System.EventHandler(this.btTestWithJwc_Click);
            // 
            // tbLoginResult
            // 
            this.tbLoginResult.Location = new System.Drawing.Point(835, 84);
            this.tbLoginResult.Multiline = true;
            this.tbLoginResult.Name = "tbLoginResult";
            this.tbLoginResult.Size = new System.Drawing.Size(301, 119);
            this.tbLoginResult.TabIndex = 3;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(18, 17);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(0, 13);
            this.label5.TabIndex = 2;
            // 
            // btLogin
            // 
            this.btLogin.Location = new System.Drawing.Point(835, 55);
            this.btLogin.Name = "btLogin";
            this.btLogin.Size = new System.Drawing.Size(75, 23);
            this.btLogin.TabIndex = 0;
            this.btLogin.Text = "Логин";
            this.btLogin.UseVisualStyleBackColor = true;
            this.btLogin.Click += new System.EventHandler(this.btLogin_Click);
            // 
            // tbUserName
            // 
            this.tbUserName.Location = new System.Drawing.Point(985, 14);
            this.tbUserName.Name = "tbUserName";
            this.tbUserName.Size = new System.Drawing.Size(151, 20);
            this.tbUserName.TabIndex = 1;
            this.tbUserName.Text = "vav";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(916, 20);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(34, 13);
            this.label6.TabIndex = 3;
            this.label6.Text = "Юзер";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(916, 46);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(45, 13);
            this.label7.TabIndex = 5;
            this.label7.Text = "Пароль";
            // 
            // tbUserPassword
            // 
            this.tbUserPassword.Location = new System.Drawing.Point(985, 40);
            this.tbUserPassword.Name = "tbUserPassword";
            this.tbUserPassword.Size = new System.Drawing.Size(151, 20);
            this.tbUserPassword.TabIndex = 4;
            this.tbUserPassword.Text = "marsohod";
            // 
            // tbWithTokenResult
            // 
            this.tbWithTokenResult.Location = new System.Drawing.Point(6, 36);
            this.tbWithTokenResult.Multiline = true;
            this.tbWithTokenResult.Name = "tbWithTokenResult";
            this.tbWithTokenResult.Size = new System.Drawing.Size(290, 211);
            this.tbWithTokenResult.TabIndex = 5;
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.btShowRatings);
            this.tabPage5.Controls.Add(this.tbRatingList);
            this.tabPage5.Controls.Add(this.btUploadRatingFile);
            this.tabPage5.Controls.Add(this.label8);
            this.tabPage5.Controls.Add(this.tbPathToRatingFile);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(1119, 431);
            this.tabPage5.TabIndex = 4;
            this.tabPage5.Text = "tabPage5";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // tbPathToRatingFile
            // 
            this.tbPathToRatingFile.Location = new System.Drawing.Point(24, 23);
            this.tbPathToRatingFile.Name = "tbPathToRatingFile";
            this.tbPathToRatingFile.Size = new System.Drawing.Size(1070, 20);
            this.tbPathToRatingFile.TabIndex = 0;
            this.tbPathToRatingFile.Text = "c:\\work\\Git\\ZhukBGGClubRanking\\ZhukBGGClubRanking\\ZhukBGGClubRanking.WinApp\\bin\\D" +
    "ebug\\lists\\VAV.csv";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(21, 7);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(88, 13);
            this.label8.TabIndex = 1;
            this.label8.Text = "Файл рейтинга:";
            // 
            // btUploadRatingFile
            // 
            this.btUploadRatingFile.Location = new System.Drawing.Point(24, 50);
            this.btUploadRatingFile.Name = "btUploadRatingFile";
            this.btUploadRatingFile.Size = new System.Drawing.Size(75, 23);
            this.btUploadRatingFile.TabIndex = 2;
            this.btUploadRatingFile.Text = "Загрузить рейтинг";
            this.btUploadRatingFile.UseVisualStyleBackColor = true;
            this.btUploadRatingFile.Click += new System.EventHandler(this.btUploadRatingFile_Click);
            // 
            // tbRatingList
            // 
            this.tbRatingList.Location = new System.Drawing.Point(24, 79);
            this.tbRatingList.Multiline = true;
            this.tbRatingList.Name = "tbRatingList";
            this.tbRatingList.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbRatingList.Size = new System.Drawing.Size(1070, 330);
            this.tbRatingList.TabIndex = 4;
            // 
            // btShowRatings
            // 
            this.btShowRatings.Location = new System.Drawing.Point(130, 50);
            this.btShowRatings.Name = "btShowRatings";
            this.btShowRatings.Size = new System.Drawing.Size(178, 23);
            this.btShowRatings.TabIndex = 5;
            this.btShowRatings.Text = "Показать рейтинги";
            this.btShowRatings.UseVisualStyleBackColor = true;
            this.btShowRatings.Click += new System.EventHandler(this.btShowRatings_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1152, 701);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.tbLoginResult);
            this.Controls.Add(this.btLogin);
            this.Controls.Add(this.tbUserPassword);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.tbPassword);
            this.Controls.Add(this.tbLogin);
            this.Controls.Add(this.tbUserName);
            this.Controls.Add(this.tbUrl);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            this.tabPage5.ResumeLayout(false);
            this.tabPage5.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbUrl;
        private System.Windows.Forms.TextBox tbLogin;
        private System.Windows.Forms.TextBox tbPassword;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbNewFullName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbNewEmail;
        private System.Windows.Forms.Button btCreateUser;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Button btGetGameCollection;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TextBox tbTestResult;
        private System.Windows.Forms.Button btGetTestString;
        private System.Windows.Forms.Button btGetTestStringAuth;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.TextBox tbUserName;
        private System.Windows.Forms.Button btLogin;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox tbUserPassword;
        private System.Windows.Forms.TextBox tbNewPassword;
        private System.Windows.Forms.TextBox tbNewLogin;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbLoginResult;
        private System.Windows.Forms.Button btTestWithJwc;
        private System.Windows.Forms.TextBox tbWithTokenResult;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.Button btUploadRatingFile;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox tbPathToRatingFile;
        private System.Windows.Forms.TextBox tbRatingList;
        private System.Windows.Forms.Button btShowRatings;
    }
}

