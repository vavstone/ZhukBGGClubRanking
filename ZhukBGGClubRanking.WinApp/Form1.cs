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

        public GameRatingList GetRatingInOpenedTab()
        {
            var currentTab = tabControl1.SelectedTab.Text;
            return usersRatingListFiles.FirstOrDefault(c=>c.File.UserNames.Contains(currentTab)).File;
        }

        private List<GameRatingListFile> usersRatingListFiles;

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadUsersRatings();
            SetButton2Text();
            PrepareDataGrid(dataGridView1);
        }

        void LoadUsersRatings()
        {
            usersRatingListFiles = GameRatingListFile.LoadFromFolder();
            tabControl1.TabPages.Clear();
            checkedListBox1.Items.Clear();
            foreach (var item in usersRatingListFiles)
            {
                checkedListBox1.Items.Add(item.FileNameWithoutExt, true);
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

        private void button2_Click(object sender, EventArgs e)
        {
            ReorderForm reorderForm = new ReorderForm();
            reorderForm.RatingList = GetRatingInOpenedTab();
            var otherCollections = usersRatingListFiles
                .Where(c => c.File.UserNames.FirstOrDefault() != reorderForm.RatingList.UserNames.FirstOrDefault())
                .Select(c => c.File).ToList();
            var newGames = reorderForm.RatingList.GetGamesNotInCollectionButExistingInOthers(otherCollections);
            if (newGames.Count > 0)
            {
                if (MessageBox.Show("В других коллекциях найдены игры, отсуствующие в вашем рейтинге. Добавить их?",
                        "Подтверждение добавления",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question,
                        MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                {
                    reorderForm.NewGames = newGames;
                }
            }
            reorderForm.ShowDialog(this);
            LoadUsersRatings();
        }

        void SetButton2Text()
        {
            if (tabControl1.SelectedTab != null)
            {
                var selectedTabText = tabControl1.SelectedTab.Text;
                button2.Text = "Пересмотреть рейтинг " + selectedTabText;
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetButton2Text();
        }
    }
}
