using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ZhukBGGClubRanking.Core;

namespace ZhukBGGClubRanking.WinApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private List<GameRatingListFile> usersRatingListFiles;

        private void Form1_Load(object sender, EventArgs e)
        {
            PrepareDataGrid(dataGridView1);
            usersRatingListFiles = GameRatingListFile.LoadFromFolder(Settings.ListsDir);
            foreach (var item in usersRatingListFiles)
            {
                checkedListBox1.Items.Add(item.FileNameWithoutExt,true);
                var tabPage = new TabPage();
                tabPage.Text = item.FileNameWithoutExt;
                tabControl1.TabPages.Add(tabPage);
                var grid = new DataGridView();
                grid.Dock = DockStyle.Fill;
                PrepareDataGrid(grid);
                grid.DataSource = item.File.GameList.OrderBy(c => c.Rating).ToList();
                tabPage.Controls.Add(grid);
            }
        }

        public void PrepareDataGrid(DataGridView grid)
        {
            grid.AutoGenerateColumns = false;
            var colName = new DataGridViewTextBoxColumn();
            colName.Width = 350;
            colName.HeaderText = "Название";
            colName.DataPropertyName = "Game";
            grid.Columns.Add(colName);
            var colRate = new DataGridViewTextBoxColumn();
            colRate.Width = 60;
            colRate.HeaderText = "Рейтинг";
            colRate.DataPropertyName = "Rating";
            grid.Columns.Add(colRate);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var checkedUserNames = checkedListBox1.CheckedItems.OfType<string>().ToList();
            var checkedLists = usersRatingListFiles.Where(c=>checkedUserNames.Contains(c.File.UserNames.First())).Select(c => c.File).ToList();
            var result = GameRatingList.CalculateAvarageRating(checkedLists);
            dataGridView1.DataSource = result.GameList.OrderBy(c => c.Rating).ToList();
        }
    }
}
