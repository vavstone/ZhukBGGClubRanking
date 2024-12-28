namespace ZhukBGGClubRanking.WinApp
{
    partial class ManageUsersForm
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
            this.btCreateUser = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cbxRole = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tbEmail = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tbFullName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbPassword = new System.Windows.Forms.TextBox();
            this.tbLogin = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // btCreateUser
            // 
            this.btCreateUser.Location = new System.Drawing.Point(647, 90);
            this.btCreateUser.Name = "btCreateUser";
            this.btCreateUser.Size = new System.Drawing.Size(189, 23);
            this.btCreateUser.TabIndex = 0;
            this.btCreateUser.Text = "Создать нового пользователя";
            this.btCreateUser.UseVisualStyleBackColor = true;
            this.btCreateUser.Click += new System.EventHandler(this.btCreateUser_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cbxRole);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.tbEmail);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.tbFullName);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.btCreateUser);
            this.groupBox1.Controls.Add(this.tbPassword);
            this.groupBox1.Controls.Add(this.tbLogin);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 342);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(852, 124);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Атрибуты пользователя";
            // 
            // cbxRole
            // 
            this.cbxRole.FormattingEnabled = true;
            this.cbxRole.Items.AddRange(new object[] {
            "user",
            "admin"});
            this.cbxRole.Location = new System.Drawing.Point(363, 63);
            this.cbxRole.Name = "cbxRole";
            this.cbxRole.Size = new System.Drawing.Size(191, 21);
            this.cbxRole.TabIndex = 10;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(289, 63);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(32, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Роль";
            // 
            // tbEmail
            // 
            this.tbEmail.Location = new System.Drawing.Point(363, 31);
            this.tbEmail.Name = "tbEmail";
            this.tbEmail.Size = new System.Drawing.Size(191, 20);
            this.tbEmail.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(289, 31);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(32, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Email";
            // 
            // tbFullName
            // 
            this.tbFullName.Location = new System.Drawing.Point(89, 95);
            this.tbFullName.Name = "tbFullName";
            this.tbFullName.Size = new System.Drawing.Size(160, 20);
            this.tbFullName.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 95);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(68, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Полное имя";
            // 
            // tbPassword
            // 
            this.tbPassword.Location = new System.Drawing.Point(89, 63);
            this.tbPassword.Name = "tbPassword";
            this.tbPassword.Size = new System.Drawing.Size(160, 20);
            this.tbPassword.TabIndex = 3;
            // 
            // tbLogin
            // 
            this.tbLogin.Location = new System.Drawing.Point(89, 31);
            this.tbLogin.Name = "tbLogin";
            this.tbLogin.Size = new System.Drawing.Size(160, 20);
            this.tbLogin.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 63);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Пароль";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Логин";
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(12, 13);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(866, 323);
            this.dataGridView1.TabIndex = 2;
            // 
            // ManageUsersForm
            // 
            this.AcceptButton = this.btCreateUser;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(876, 478);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ManageUsersForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ZhukBGGClubRanking. Вход";
            this.Load += new System.EventHandler(this.ManageUsersForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btCreateUser;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbPassword;
        private System.Windows.Forms.TextBox tbLogin;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbFullName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.ComboBox cbxRole;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbEmail;
        private System.Windows.Forms.Label label4;
    }
}