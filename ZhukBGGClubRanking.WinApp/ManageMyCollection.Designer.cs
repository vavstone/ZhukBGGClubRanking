namespace ZhukBGGClubRanking.WinApp
{
    partial class ManageMyCollection
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ManageMyCollection));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.dgvMyCollection = new ZhukBGGClubRanking.WinApp.DataGridViewCustom();
            this.btCancel = new System.Windows.Forms.Button();
            this.btSave = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.picBoxGame = new System.Windows.Forms.PictureBox();
            this.lblNewGameFullName = new System.Windows.Forms.LinkLabel();
            this.btAddGame = new System.Windows.Forms.Button();
            this.cbSelectRawGame = new System.Windows.Forms.ComboBox();
            this.lblShortDesctiption = new System.Windows.Forms.Label();
            this.lblIsStandalone = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMyCollection)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxGame)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.dgvMyCollection);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.btCancel);
            this.splitContainer1.Panel2.Controls.Add(this.btSave);
            this.splitContainer1.Panel2.Controls.Add(this.groupBox1);
            this.splitContainer1.Size = new System.Drawing.Size(1054, 708);
            this.splitContainer1.SplitterDistance = 455;
            this.splitContainer1.TabIndex = 0;
            // 
            // dgvMyCollection
            // 
            this.dgvMyCollection.AllowUserToAddRows = false;
            this.dgvMyCollection.AllowUserToDeleteRows = false;
            this.dgvMyCollection.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMyCollection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvMyCollection.Location = new System.Drawing.Point(0, 0);
            this.dgvMyCollection.Name = "dgvMyCollection";
            this.dgvMyCollection.RowHeadersVisible = false;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.LightGray;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Black;
            this.dgvMyCollection.RowsDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvMyCollection.ShowEditingIcon = false;
            this.dgvMyCollection.Size = new System.Drawing.Size(1054, 455);
            this.dgvMyCollection.TabIndex = 0;
            // 
            // btCancel
            // 
            this.btCancel.Location = new System.Drawing.Point(852, 220);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(190, 23);
            this.btCancel.TabIndex = 2;
            this.btCancel.Text = "Отменить изменения";
            this.btCancel.UseVisualStyleBackColor = true;
            // 
            // btSave
            // 
            this.btSave.Location = new System.Drawing.Point(852, 191);
            this.btSave.Name = "btSave";
            this.btSave.Size = new System.Drawing.Size(191, 23);
            this.btSave.TabIndex = 1;
            this.btSave.Text = "Сохранить изменения";
            this.btSave.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.lblIsStandalone);
            this.groupBox1.Controls.Add(this.lblShortDesctiption);
            this.groupBox1.Controls.Add(this.picBoxGame);
            this.groupBox1.Controls.Add(this.lblNewGameFullName);
            this.groupBox1.Controls.Add(this.btAddGame);
            this.groupBox1.Controls.Add(this.cbSelectRawGame);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(842, 240);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Добавление новой игры";
            // 
            // picBoxGame
            // 
            this.picBoxGame.ImageLocation = "";
            this.picBoxGame.Location = new System.Drawing.Point(616, 12);
            this.picBoxGame.Name = "picBoxGame";
            this.picBoxGame.Size = new System.Drawing.Size(220, 222);
            this.picBoxGame.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picBoxGame.TabIndex = 4;
            this.picBoxGame.TabStop = false;
            // 
            // lblNewGameFullName
            // 
            this.lblNewGameFullName.AutoSize = true;
            this.lblNewGameFullName.Location = new System.Drawing.Point(6, 49);
            this.lblNewGameFullName.Name = "lblNewGameFullName";
            this.lblNewGameFullName.Size = new System.Drawing.Size(342, 13);
            this.lblNewGameFullName.TabIndex = 3;
            this.lblNewGameFullName.TabStop = true;
            this.lblNewGameFullName.Text = "Выберите в списке выше игру для добавления в свою коллекцию";
            // 
            // btAddGame
            // 
            this.btAddGame.Location = new System.Drawing.Point(7, 208);
            this.btAddGame.Name = "btAddGame";
            this.btAddGame.Size = new System.Drawing.Size(170, 23);
            this.btAddGame.TabIndex = 2;
            this.btAddGame.Text = "Добавить выбранную игру";
            this.btAddGame.UseVisualStyleBackColor = true;
            this.btAddGame.Click += new System.EventHandler(this.btAddGame_Click);
            // 
            // cbSelectRawGame
            // 
            this.cbSelectRawGame.FormattingEnabled = true;
            this.cbSelectRawGame.Location = new System.Drawing.Point(9, 19);
            this.cbSelectRawGame.Name = "cbSelectRawGame";
            this.cbSelectRawGame.Size = new System.Drawing.Size(601, 21);
            this.cbSelectRawGame.TabIndex = 0;
            this.cbSelectRawGame.SelectedIndexChanged += new System.EventHandler(this.cbSelectRawGame_SelectedIndexChanged);
            this.cbSelectRawGame.SelectionChangeCommitted += new System.EventHandler(this.cbSelectRawGame_SelectionChangeCommitted);
            this.cbSelectRawGame.TextUpdate += new System.EventHandler(this.cbSelectRawGame_TextUpdate);
            this.cbSelectRawGame.TextChanged += new System.EventHandler(this.cbSelectRawGame_TextChanged);
            // 
            // lblShortDesctiption
            // 
            this.lblShortDesctiption.AutoSize = true;
            this.lblShortDesctiption.Location = new System.Drawing.Point(6, 76);
            this.lblShortDesctiption.MaximumSize = new System.Drawing.Size(600, 0);
            this.lblShortDesctiption.Name = "lblShortDesctiption";
            this.lblShortDesctiption.Size = new System.Drawing.Size(106, 13);
            this.lblShortDesctiption.TabIndex = 5;
            this.lblShortDesctiption.Text = "Краткое описание: ";
            this.lblShortDesctiption.Click += new System.EventHandler(this.lblShortDesctiption_Click);
            // 
            // lblIsStandalone
            // 
            this.lblIsStandalone.AutoSize = true;
            this.lblIsStandalone.Location = new System.Drawing.Point(6, 139);
            this.lblIsStandalone.MaximumSize = new System.Drawing.Size(580, 0);
            this.lblIsStandalone.Name = "lblIsStandalone";
            this.lblIsStandalone.Size = new System.Drawing.Size(276, 13);
            this.lblIsStandalone.TabIndex = 6;
            this.lblIsStandalone.Text = "Является самостоятельной игрой (не дополнением):";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 163);
            this.label1.MaximumSize = new System.Drawing.Size(600, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(595, 39);
            this.label1.TabIndex = 7;
            this.label1.Text = resources.GetString("label1.Text");
            // 
            // ManageMyCollection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1054, 708);
            this.Controls.Add(this.splitContainer1);
            this.MaximumSize = new System.Drawing.Size(1070, 1200);
            this.Name = "ManageMyCollection";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ManageMyCollection";
            this.Load += new System.EventHandler(this.ManageMyCollection_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvMyCollection)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxGame)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private DataGridViewCustom dgvMyCollection;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.Button btSave;
        private System.Windows.Forms.ComboBox cbSelectRawGame;
        private System.Windows.Forms.Button btAddGame;
        private System.Windows.Forms.LinkLabel lblNewGameFullName;
        private System.Windows.Forms.PictureBox picBoxGame;
        private System.Windows.Forms.Label lblShortDesctiption;
        private System.Windows.Forms.Label lblIsStandalone;
        private System.Windows.Forms.Label label1;
    }
}