namespace ZhukBGGClubRanking.WinApp
{
    partial class SelectTablesColumns
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cbxLeftTableRating = new System.Windows.Forms.CheckBox();
            this.cbxLeftTableUsersRating = new System.Windows.Forms.CheckBox();
            this.cbxLeftTableOwners = new System.Windows.Forms.CheckBox();
            this.cbxRightTableOwners = new System.Windows.Forms.CheckBox();
            this.cbxRightTableUsersRating = new System.Windows.Forms.CheckBox();
            this.cbxRightTableRating = new System.Windows.Forms.CheckBox();
            this.btSave = new System.Windows.Forms.Button();
            this.btDiscard = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cbxLeftTableOwners);
            this.groupBox1.Controls.Add(this.cbxLeftTableUsersRating);
            this.groupBox1.Controls.Add(this.cbxLeftTableRating);
            this.groupBox1.Location = new System.Drawing.Point(13, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(324, 94);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Рейтинги отдельных пользователей (слева)";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cbxRightTableOwners);
            this.groupBox2.Controls.Add(this.cbxRightTableUsersRating);
            this.groupBox2.Controls.Add(this.cbxRightTableRating);
            this.groupBox2.Location = new System.Drawing.Point(354, 13);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(324, 94);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Общий рейтинг (справа)";
            // 
            // cbxLeftTableRating
            // 
            this.cbxLeftTableRating.AutoSize = true;
            this.cbxLeftTableRating.Location = new System.Drawing.Point(9, 19);
            this.cbxLeftTableRating.Name = "cbxLeftTableRating";
            this.cbxLeftTableRating.Size = new System.Drawing.Size(67, 17);
            this.cbxLeftTableRating.TabIndex = 0;
            this.cbxLeftTableRating.Text = "Рейтинг";
            this.cbxLeftTableRating.UseVisualStyleBackColor = true;
            // 
            // cbxLeftTableUsersRating
            // 
            this.cbxLeftTableUsersRating.AutoSize = true;
            this.cbxLeftTableUsersRating.Location = new System.Drawing.Point(9, 43);
            this.cbxLeftTableUsersRating.Name = "cbxLeftTableUsersRating";
            this.cbxLeftTableUsersRating.Size = new System.Drawing.Size(119, 17);
            this.cbxLeftTableUsersRating.TabIndex = 1;
            this.cbxLeftTableUsersRating.Text = "Рейтинг у игроков";
            this.cbxLeftTableUsersRating.UseVisualStyleBackColor = true;
            // 
            // cbxLeftTableOwners
            // 
            this.cbxLeftTableOwners.AutoSize = true;
            this.cbxLeftTableOwners.Location = new System.Drawing.Point(9, 67);
            this.cbxLeftTableOwners.Name = "cbxLeftTableOwners";
            this.cbxLeftTableOwners.Size = new System.Drawing.Size(70, 17);
            this.cbxLeftTableOwners.TabIndex = 2;
            this.cbxLeftTableOwners.Text = "Владеют";
            this.cbxLeftTableOwners.UseVisualStyleBackColor = true;
            // 
            // cbxRightTableOwners
            // 
            this.cbxRightTableOwners.AutoSize = true;
            this.cbxRightTableOwners.Location = new System.Drawing.Point(9, 67);
            this.cbxRightTableOwners.Name = "cbxRightTableOwners";
            this.cbxRightTableOwners.Size = new System.Drawing.Size(70, 17);
            this.cbxRightTableOwners.TabIndex = 5;
            this.cbxRightTableOwners.Text = "Владеют";
            this.cbxRightTableOwners.UseVisualStyleBackColor = true;
            // 
            // cbxRightTableUsersRating
            // 
            this.cbxRightTableUsersRating.AutoSize = true;
            this.cbxRightTableUsersRating.Location = new System.Drawing.Point(9, 43);
            this.cbxRightTableUsersRating.Name = "cbxRightTableUsersRating";
            this.cbxRightTableUsersRating.Size = new System.Drawing.Size(119, 17);
            this.cbxRightTableUsersRating.TabIndex = 4;
            this.cbxRightTableUsersRating.Text = "Рейтинг у игроков";
            this.cbxRightTableUsersRating.UseVisualStyleBackColor = true;
            // 
            // cbxRightTableRating
            // 
            this.cbxRightTableRating.AutoSize = true;
            this.cbxRightTableRating.Location = new System.Drawing.Point(9, 19);
            this.cbxRightTableRating.Name = "cbxRightTableRating";
            this.cbxRightTableRating.Size = new System.Drawing.Size(67, 17);
            this.cbxRightTableRating.TabIndex = 3;
            this.cbxRightTableRating.Text = "Рейтинг";
            this.cbxRightTableRating.UseVisualStyleBackColor = true;
            // 
            // btSave
            // 
            this.btSave.Location = new System.Drawing.Point(118, 145);
            this.btSave.Name = "btSave";
            this.btSave.Size = new System.Drawing.Size(75, 23);
            this.btSave.TabIndex = 3;
            this.btSave.Text = "Сохранить";
            this.btSave.UseVisualStyleBackColor = true;
            this.btSave.Click += new System.EventHandler(this.btSave_Click);
            // 
            // btDiscard
            // 
            this.btDiscard.Location = new System.Drawing.Point(466, 145);
            this.btDiscard.Name = "btDiscard";
            this.btDiscard.Size = new System.Drawing.Size(75, 23);
            this.btDiscard.TabIndex = 4;
            this.btDiscard.Text = "Отменить";
            this.btDiscard.UseVisualStyleBackColor = true;
            this.btDiscard.Click += new System.EventHandler(this.btDiscard_Click);
            // 
            // SelectTablesColumns
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(693, 178);
            this.Controls.Add(this.btSave);
            this.Controls.Add(this.btDiscard);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "SelectTablesColumns";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Выбор отображения колонок";
            this.Load += new System.EventHandler(this.SelectTablesColumns_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox cbxLeftTableOwners;
        private System.Windows.Forms.CheckBox cbxLeftTableUsersRating;
        private System.Windows.Forms.CheckBox cbxLeftTableRating;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox cbxRightTableOwners;
        private System.Windows.Forms.CheckBox cbxRightTableUsersRating;
        private System.Windows.Forms.CheckBox cbxRightTableRating;
        private System.Windows.Forms.Button btSave;
        private System.Windows.Forms.Button btDiscard;
    }
}