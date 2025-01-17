namespace ZhukBGGClubRanking.WinApp
{
    partial class GamesListForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.grid = new ZhukBGGClubRanking.WinApp.DataGridViewCustom();
            this.cbOnlyClubCollection = new System.Windows.Forms.CheckBox();
            this.btApply = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
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
            this.splitContainer1.Panel1.Controls.Add(this.grid);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.cbOnlyClubCollection);
            this.splitContainer1.Panel2.Controls.Add(this.btApply);
            this.splitContainer1.Size = new System.Drawing.Size(1262, 603);
            this.splitContainer1.SplitterDistance = 420;
            this.splitContainer1.TabIndex = 0;
            // 
            // grid
            // 
            this.grid.AllowUserToAddRows = false;
            this.grid.AllowUserToDeleteRows = false;
            this.grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grid.Location = new System.Drawing.Point(0, 0);
            this.grid.Name = "grid";
            this.grid.RowHeadersVisible = false;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.LightGray;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.Black;
            this.grid.RowsDefaultCellStyle = dataGridViewCellStyle1;
            this.grid.ShowEditingIcon = false;
            this.grid.Size = new System.Drawing.Size(1262, 420);
            this.grid.TabIndex = 0;
            // 
            // cbOnlyClubCollection
            // 
            this.cbOnlyClubCollection.AutoSize = true;
            this.cbOnlyClubCollection.Checked = true;
            this.cbOnlyClubCollection.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbOnlyClubCollection.Location = new System.Drawing.Point(13, 12);
            this.cbOnlyClubCollection.Name = "cbOnlyClubCollection";
            this.cbOnlyClubCollection.Size = new System.Drawing.Size(123, 17);
            this.cbOnlyClubCollection.TabIndex = 1;
            this.cbOnlyClubCollection.Text = "Только игры клуба";
            this.cbOnlyClubCollection.UseVisualStyleBackColor = true;
            // 
            // btApply
            // 
            this.btApply.Location = new System.Drawing.Point(1166, 12);
            this.btApply.Name = "btApply";
            this.btApply.Size = new System.Drawing.Size(75, 23);
            this.btApply.TabIndex = 0;
            this.btApply.Text = "Применить";
            this.btApply.UseVisualStyleBackColor = true;
            this.btApply.Click += new System.EventHandler(this.btApply_Click);
            // 
            // GamesListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1262, 603);
            this.Controls.Add(this.splitContainer1);
            this.Name = "GamesListForm";
            this.Text = "GamesListForm";
            this.Load += new System.EventHandler(this.GamesListForm_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private DataGridViewCustom grid;
        private System.Windows.Forms.CheckBox cbOnlyClubCollection;
        private System.Windows.Forms.Button btApply;
    }
}