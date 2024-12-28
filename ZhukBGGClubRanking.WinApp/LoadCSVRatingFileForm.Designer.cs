namespace ZhukBGGClubRanking.WinApp
{
    partial class LoadCSVRatingFileForm
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
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btSelectFile = new System.Windows.Forms.Button();
            this.btLoadToServer = new System.Windows.Forms.Button();
            this.lblInfo = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.gridDBData = new System.Windows.Forms.DataGridView();
            this.gridCSVData = new System.Windows.Forms.DataGridView();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridDBData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridCSVData)).BeginInit();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lblInfo);
            this.panel1.Controls.Add(this.btLoadToServer);
            this.panel1.Controls.Add(this.btSelectFile);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 488);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(558, 69);
            this.panel1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.tabControl1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(558, 488);
            this.panel2.TabIndex = 1;
            // 
            // btSelectFile
            // 
            this.btSelectFile.Location = new System.Drawing.Point(12, 14);
            this.btSelectFile.Name = "btSelectFile";
            this.btSelectFile.Size = new System.Drawing.Size(203, 23);
            this.btSelectFile.TabIndex = 0;
            this.btSelectFile.Text = "Выбрать файл";
            this.btSelectFile.UseVisualStyleBackColor = true;
            this.btSelectFile.Click += new System.EventHandler(this.btSelectFile_Click);
            // 
            // btLoadToServer
            // 
            this.btLoadToServer.Location = new System.Drawing.Point(341, 14);
            this.btLoadToServer.Name = "btLoadToServer";
            this.btLoadToServer.Size = new System.Drawing.Size(205, 23);
            this.btLoadToServer.TabIndex = 1;
            this.btLoadToServer.Text = "Загрузить на сервер рейтинг";
            this.btLoadToServer.UseVisualStyleBackColor = true;
            this.btLoadToServer.Click += new System.EventHandler(this.btLoadToServer_Click);
            // 
            // lblInfo
            // 
            this.lblInfo.AutoSize = true;
            this.lblInfo.Location = new System.Drawing.Point(12, 47);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(92, 13);
            this.lblInfo.TabIndex = 2;
            this.lblInfo.Text = "Файл не выбран";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(558, 488);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.gridDBData);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(550, 462);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Текущий рейтинг";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.gridCSVData);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(550, 462);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Данные из csv файл (еще не загружены)";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // gridDBData
            // 
            this.gridDBData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridDBData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridDBData.Location = new System.Drawing.Point(3, 3);
            this.gridDBData.Name = "gridDBData";
            this.gridDBData.Size = new System.Drawing.Size(544, 456);
            this.gridDBData.TabIndex = 0;
            // 
            // gridCSVData
            // 
            this.gridCSVData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridCSVData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridCSVData.Location = new System.Drawing.Point(3, 3);
            this.gridCSVData.Name = "gridCSVData";
            this.gridCSVData.Size = new System.Drawing.Size(544, 456);
            this.gridCSVData.TabIndex = 0;
            // 
            // LoadCSVRatingFileForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(558, 557);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "LoadCSVRatingFileForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Загрузка файла рейтинга csv";
            this.Load += new System.EventHandler(this.LoadCSVRatingFileForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridDBData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridCSVData)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btSelectFile;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btLoadToServer;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.DataGridView gridDBData;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.DataGridView gridCSVData;
    }
}