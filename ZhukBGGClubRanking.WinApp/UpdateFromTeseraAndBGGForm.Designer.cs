namespace ZhukBGGClubRanking.WinApp
{
    partial class UpdateFromTeseraAndBGGForm
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tpUpdateFromBGG = new System.Windows.Forms.TabPage();
            this.tpUpdateFromTesera = new System.Windows.Forms.TabPage();
            this.btSaveTeseraRawInfoToDB = new System.Windows.Forms.Button();
            this.lblGamesFromTeseraGetted = new System.Windows.Forms.Label();
            this.btClearTeseraRawInfo = new System.Windows.Forms.Button();
            this.tbUrlAPITesera = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btGetDataFromTesera = new System.Windows.Forms.Button();
            this.tpSaveTeseraAndBGGRawInfo = new System.Windows.Forms.TabPage();
            this.btClearBGGTeseraRawInfo = new System.Windows.Forms.Button();
            this.btSaveBGGTeseraRawInfoToDB = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tpUpdateFromTesera.SuspendLayout();
            this.tpSaveTeseraAndBGGRawInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tpUpdateFromBGG);
            this.tabControl1.Controls.Add(this.tpUpdateFromTesera);
            this.tabControl1.Controls.Add(this.tpSaveTeseraAndBGGRawInfo);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1031, 535);
            this.tabControl1.TabIndex = 0;
            // 
            // tpUpdateFromBGG
            // 
            this.tpUpdateFromBGG.Location = new System.Drawing.Point(4, 25);
            this.tpUpdateFromBGG.Name = "tpUpdateFromBGG";
            this.tpUpdateFromBGG.Padding = new System.Windows.Forms.Padding(3);
            this.tpUpdateFromBGG.Size = new System.Drawing.Size(1023, 506);
            this.tpUpdateFromBGG.TabIndex = 0;
            this.tpUpdateFromBGG.Text = "BGG";
            this.tpUpdateFromBGG.UseVisualStyleBackColor = true;
            // 
            // tpUpdateFromTesera
            // 
            this.tpUpdateFromTesera.Controls.Add(this.btSaveTeseraRawInfoToDB);
            this.tpUpdateFromTesera.Controls.Add(this.lblGamesFromTeseraGetted);
            this.tpUpdateFromTesera.Controls.Add(this.btClearTeseraRawInfo);
            this.tpUpdateFromTesera.Controls.Add(this.tbUrlAPITesera);
            this.tpUpdateFromTesera.Controls.Add(this.label1);
            this.tpUpdateFromTesera.Controls.Add(this.btGetDataFromTesera);
            this.tpUpdateFromTesera.Location = new System.Drawing.Point(4, 25);
            this.tpUpdateFromTesera.Name = "tpUpdateFromTesera";
            this.tpUpdateFromTesera.Padding = new System.Windows.Forms.Padding(3);
            this.tpUpdateFromTesera.Size = new System.Drawing.Size(1023, 506);
            this.tpUpdateFromTesera.TabIndex = 1;
            this.tpUpdateFromTesera.Text = "Tesera";
            this.tpUpdateFromTesera.UseVisualStyleBackColor = true;
            // 
            // btSaveTeseraRawInfoToDB
            // 
            this.btSaveTeseraRawInfoToDB.Location = new System.Drawing.Point(31, 440);
            this.btSaveTeseraRawInfoToDB.Name = "btSaveTeseraRawInfoToDB";
            this.btSaveTeseraRawInfoToDB.Size = new System.Drawing.Size(485, 23);
            this.btSaveTeseraRawInfoToDB.TabIndex = 5;
            this.btSaveTeseraRawInfoToDB.Text = "Загрузить полученные игры в БД (из файлов JSON папки сервера)";
            this.btSaveTeseraRawInfoToDB.UseVisualStyleBackColor = true;
            this.btSaveTeseraRawInfoToDB.Click += new System.EventHandler(this.btSaveTeseraRawInfoToDB_Click);
            // 
            // lblGamesFromTeseraGetted
            // 
            this.lblGamesFromTeseraGetted.AutoSize = true;
            this.lblGamesFromTeseraGetted.Location = new System.Drawing.Point(28, 412);
            this.lblGamesFromTeseraGetted.Name = "lblGamesFromTeseraGetted";
            this.lblGamesFromTeseraGetted.Size = new System.Drawing.Size(44, 16);
            this.lblGamesFromTeseraGetted.TabIndex = 4;
            this.lblGamesFromTeseraGetted.Text = "label2";
            // 
            // btClearTeseraRawInfo
            // 
            this.btClearTeseraRawInfo.Location = new System.Drawing.Point(31, 320);
            this.btClearTeseraRawInfo.Name = "btClearTeseraRawInfo";
            this.btClearTeseraRawInfo.Size = new System.Drawing.Size(485, 23);
            this.btClearTeseraRawInfo.TabIndex = 3;
            this.btClearTeseraRawInfo.Text = "Очистить данные Tesera в БД";
            this.btClearTeseraRawInfo.UseVisualStyleBackColor = true;
            this.btClearTeseraRawInfo.Click += new System.EventHandler(this.btClearTeseraRawInfo_Click);
            // 
            // tbUrlAPITesera
            // 
            this.tbUrlAPITesera.Location = new System.Drawing.Point(129, 349);
            this.tbUrlAPITesera.Name = "tbUrlAPITesera";
            this.tbUrlAPITesera.Size = new System.Drawing.Size(387, 22);
            this.tbUrlAPITesera.TabIndex = 2;
            this.tbUrlAPITesera.Text = "https://api.tesera.ru/";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(28, 352);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "Url API Tesera";
            // 
            // btGetDataFromTesera
            // 
            this.btGetDataFromTesera.Location = new System.Drawing.Point(31, 377);
            this.btGetDataFromTesera.Name = "btGetDataFromTesera";
            this.btGetDataFromTesera.Size = new System.Drawing.Size(485, 23);
            this.btGetDataFromTesera.TabIndex = 0;
            this.btGetDataFromTesera.Text = "Запустить получение данных (в папку WinApp приложения)";
            this.btGetDataFromTesera.UseVisualStyleBackColor = true;
            this.btGetDataFromTesera.Click += new System.EventHandler(this.btGetDataFromTesera_Click);
            // 
            // tpSaveTeseraAndBGGRawInfo
            // 
            this.tpSaveTeseraAndBGGRawInfo.Controls.Add(this.btSaveBGGTeseraRawInfoToDB);
            this.tpSaveTeseraAndBGGRawInfo.Controls.Add(this.btClearBGGTeseraRawInfo);
            this.tpSaveTeseraAndBGGRawInfo.Location = new System.Drawing.Point(4, 25);
            this.tpSaveTeseraAndBGGRawInfo.Name = "tpSaveTeseraAndBGGRawInfo";
            this.tpSaveTeseraAndBGGRawInfo.Padding = new System.Windows.Forms.Padding(3);
            this.tpSaveTeseraAndBGGRawInfo.Size = new System.Drawing.Size(1023, 506);
            this.tpSaveTeseraAndBGGRawInfo.TabIndex = 2;
            this.tpSaveTeseraAndBGGRawInfo.Text = "TeseraAndBGG";
            this.tpSaveTeseraAndBGGRawInfo.UseVisualStyleBackColor = true;
            // 
            // btClearBGGTeseraRawInfo
            // 
            this.btClearBGGTeseraRawInfo.Location = new System.Drawing.Point(26, 368);
            this.btClearBGGTeseraRawInfo.Name = "btClearBGGTeseraRawInfo";
            this.btClearBGGTeseraRawInfo.Size = new System.Drawing.Size(485, 23);
            this.btClearBGGTeseraRawInfo.TabIndex = 4;
            this.btClearBGGTeseraRawInfo.Text = "Очистить данные BGGTesera в БД";
            this.btClearBGGTeseraRawInfo.UseVisualStyleBackColor = true;
            this.btClearBGGTeseraRawInfo.Click += new System.EventHandler(this.btClearBGGTeseraRawInfo_Click);
            // 
            // btSaveBGGTeseraRawInfoToDB
            // 
            this.btSaveBGGTeseraRawInfoToDB.Location = new System.Drawing.Point(26, 417);
            this.btSaveBGGTeseraRawInfoToDB.Name = "btSaveBGGTeseraRawInfoToDB";
            this.btSaveBGGTeseraRawInfoToDB.Size = new System.Drawing.Size(485, 23);
            this.btSaveBGGTeseraRawInfoToDB.TabIndex = 6;
            this.btSaveBGGTeseraRawInfoToDB.Text = "Загрузить игры в общую таблицу";
            this.btSaveBGGTeseraRawInfoToDB.UseVisualStyleBackColor = true;
            this.btSaveBGGTeseraRawInfoToDB.Click += new System.EventHandler(this.btSaveBGGTeseraRawInfoToDB_Click);
            // 
            // UpdateFromTeseraAndBGGForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1031, 535);
            this.Controls.Add(this.tabControl1);
            this.Name = "UpdateFromTeseraAndBGGForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Обновление информации с сайтов Tesera и BGG";
            this.Load += new System.EventHandler(this.UpdateFromTeseraAndBGGForm_Load);
            this.tabControl1.ResumeLayout(false);
            this.tpUpdateFromTesera.ResumeLayout(false);
            this.tpUpdateFromTesera.PerformLayout();
            this.tpSaveTeseraAndBGGRawInfo.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tpUpdateFromBGG;
        private System.Windows.Forms.TabPage tpUpdateFromTesera;
        private System.Windows.Forms.Button btGetDataFromTesera;
        private System.Windows.Forms.TextBox tbUrlAPITesera;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btClearTeseraRawInfo;
        private System.Windows.Forms.Label lblGamesFromTeseraGetted;
        private System.Windows.Forms.Button btSaveTeseraRawInfoToDB;
        private System.Windows.Forms.TabPage tpSaveTeseraAndBGGRawInfo;
        private System.Windows.Forms.Button btClearBGGTeseraRawInfo;
        private System.Windows.Forms.Button btSaveBGGTeseraRawInfoToDB;
    }
}