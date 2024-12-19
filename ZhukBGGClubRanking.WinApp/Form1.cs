using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
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

        public BGGCollection CommonCollection { get; set; }

        public GameRatingList GetRatingInOpenedTab()
        {
            var currentTab = GetCurrentSelectedUser();
            return usersRatingListFiles.FirstOrDefault(c=>c.File.UserNames.Contains(currentTab)).File;
        }

        private List<GameRatingListFile> usersRatingListFiles;
        private GameRatingList currentAvarageRatingList;

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadUsersRatings();
            SetButton2Text();
            PrepareDataGrid(dataGridView1);
            LoadCommonCollection();
        }

        void LoadCommonCollection()
        {
            CommonCollection = BGGCollection.LoadFromFile();
        }

        void LoadUsersRatings()
        {
            usersRatingListFiles = GameRatingListFile.LoadFromFolder();
            tabControl1.TabPages.Clear();
            checkedListBox1.Items.Clear();
            foreach (var item in usersRatingListFiles)
            {
                checkedListBox1.Items.Add(item.FileNameWithoutExt, true);
                var tabPage = new TabPage(item.FileNameWithoutExt);
                tabPage.Name = item.FileNameWithoutExt;
                tabControl1.TabPages.Add(tabPage);
                var grid = new DataGridView();
                grid.Dock = DockStyle.Fill;
                PrepareDataGrid(grid);
                grid.DataSource = item.File.GameList.OrderBy(c => c.Rating).ToList();
                tabPage.Controls.Add(grid);
                grid.ClearSelection();
            }
        }

        public void PrepareDataGrid(DataGridView grid)
        {
            grid.AutoGenerateColumns = false;
            grid.AllowUserToAddRows = false;
            grid.AllowUserToDeleteRows = false;
            grid.ShowEditingIcon = false;
            //var colName = new DataGridViewTextBoxColumn();
            var colName = new DataGridViewLinkColumn();
            colName.Width = 306;
            //colName.Width =246;
            colName.HeaderText = "Название";
            colName.DataPropertyName = "Game";
            colName.TrackVisitedState = false;
            colName.LinkBehavior = LinkBehavior.HoverUnderline;
            colName.LinkColor = DefaultForeColor;
            grid.Columns.Add(colName);
            var colRate = new DataGridViewTextBoxColumn();
            colRate.Width = 60;
            colRate.HeaderText = "Рейтинг";
            colRate.DataPropertyName = "Rating";
            colRate.ReadOnly = true;
            grid.Columns.Add(colRate);
            AddCommentsColumn(grid);
            grid.CellContentClick += Grid_CellContentClick;
            grid.RowsDefaultCellStyle.SelectionBackColor = Color.LightGray;
            grid.RowsDefaultCellStyle.SelectionForeColor = Color.Black;
            //TODO
            //AddImagesColumn(grid);

            //AddTestColumn(grid);

        }

        private void Grid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var grid = sender as DataGridView;
            if (e.ColumnIndex == 0)
            {
                if (dataGridView1.Columns[e.ColumnIndex] is DataGridViewLinkColumn && e.RowIndex != -1)
                {
                    
                    string cellContent = grid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                    var url = Settings.UrlForGameBGGId(CommonCollection.GetItemByName(cellContent).ObjectId);
                    ProcessStartInfo sInfo = new ProcessStartInfo(url);
                    Process.Start(sInfo);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            UpdateAvarateRating();   
        }

        void UpdateAvarateRating()
        {
            var checkedUserNames = checkedListBox1.CheckedItems.OfType<string>().ToList();
            var checkedLists = usersRatingListFiles.Where(c => checkedUserNames.Contains(c.File.UserNames.First())).Select(c => c.File).ToList();
            currentAvarageRatingList = GameRatingList.CalculateAvarageRating(checkedLists);
            currentAvarageRatingList.SetBGGCollection(CommonCollection);
            CalcComplianceAverateRatingToSelectedUser();
            dataGridView1.DataSource = currentAvarageRatingList.GameList.OrderBy(c => c.Rating).ThenBy(c => c.Game).ToList();
            dataGridView1.ClearSelection();
            UpdateDataGridViewColors();
            //TODO
            //AddImagesToDataGrid(dataGridView1);

        }

        void CalcComplianceAverateRatingToSelectedUser()
        {
            var currentSelectedUser = GetCurrentSelectedUser();
            if (!string.IsNullOrWhiteSpace(currentSelectedUser) && currentAvarageRatingList!=null)
            {
                var userRatingFile =
                    usersRatingListFiles.FirstOrDefault(c => c.File.UserNames.Contains(currentSelectedUser));
                if (userRatingFile != null)
                {
                    foreach (var game in currentAvarageRatingList.GameList.OrderBy(c=>c.Rating))
                    {
                        var averageRating = game.Rating;
                        var userGame =
                            userRatingFile.File.GameList.FirstOrDefault(c => c.Game.ToUpper() == game.Game.ToUpper());
                        if (userGame != null)
                        {
                            var userRating = userGame.Rating;
                            var maxRatingSize = currentAvarageRatingList.GameList.Select(c => c.Rating).Max();
                            game.CompliancePercent = (int)Utils.GetCompliancePercent(averageRating, userRating, maxRatingSize);
                        }
                    }
                }
            }
        }

        //void AddImagesColumn(DataGridView dataGridView)
        //{
        //    var iconColumn = new DataGridViewImageColumn();
        //    iconColumn.Name = "Pic";
        //    iconColumn.Width = 120;
        //    dataGridView.Columns.Add(iconColumn);
        //}

        //void AddImagesToDataGrid(DataGridView dataGridView)
        //{
        //    for (int row = 0; row < dataGridView1.Rows.Count - 1; row++)
        //    {
        //        var uri =
        //            "https://cf.geekdo-images.com/Naw8y8J_s-8cvq1GoTON6w__thumb/img/ieH8A28KJe3truXqQe1nDXSpxUE=/fit-in/200x150/filters:strip_icc()/pic7416519.jpg";
        //        HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(uri);
        //        myRequest.Method = "GET";
        //        HttpWebResponse myResponse = (HttpWebResponse)myRequest.GetResponse();
        //        Image img = Image.FromStream(myResponse.GetResponseStream());
        //        System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(img, new Size(50, 50));
        //        myResponse.Close();
        //        ((DataGridViewImageCell)dataGridView1.Rows[row].Cells[2]).Value = bmp;
        //    }
        //}

        void AddCommentsColumn(DataGridView dataGridView)
        {
            var col = new DataGridViewTextBoxColumn();
            col.Name = "Владеют";
            col.DataPropertyName = "BGGComments";
            col.Width = 130;
            dataGridView.Columns.Add(col);
        }

        //void AddTestColumn(DataGridView dataGridView)
        //{
        //    var col = new DataGridViewTextBoxColumn();
        //    col.Name = "test";
        //    col.DataPropertyName = "CompliancePercent";
        //    col.Width = 50;
        //    dataGridView.Columns.Add(col);
        //}

        private void button2_Click(object sender, EventArgs e)
        {
            ReorderForm reorderForm = new ReorderForm();
            reorderForm.RatingList = GetRatingInOpenedTab();
            var otherCollections = usersRatingListFiles
                .Where(c => c.File.UserNames.FirstOrDefault() != reorderForm.RatingList.UserNames.FirstOrDefault())
                .Select(c => c.File).ToList();
            var newGamesInOthersColl = reorderForm.RatingList.GetGamesNotInCollectionButExistingInOthers(otherCollections);
            if (newGamesInOthersColl.Count > 0)
            {
                if (MessageBox.Show(string.Format("В других коллекциях найдены игры ({0} штук), отсуствующие в вашем рейтинге. Добавить их?",newGamesInOthersColl.Count),
                        "Подтверждение добавления",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question,
                        MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                {
                    reorderForm.NewGames = newGamesInOthersColl;
                }
            }
            reorderForm.ShowDialog(this);
            LoadUsersRatings();
        }

        string GetCurrentSelectedUser()
        {
            if (tabControl1.SelectedTab != null)
                return tabControl1.SelectedTab.Text;
            return string.Empty;
        }

        void SetButton2Text()
        {
            var currentSelectedUser = GetCurrentSelectedUser();
            if (!string.IsNullOrEmpty(currentSelectedUser))
                button2.Text = "Пересмотреть рейтинг " + currentSelectedUser;
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetButton2Text();
            CalcComplianceAverateRatingToSelectedUser();
            UpdateDataGridViewColors();
            SetCheckBoxSelected(GetCurrentSelectedUser());
        }

        void UpdateDataGridViewColors()
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                var complProc = (dataGridView1.DataSource as List<GameRating>)[row.Index].CompliancePercent;
                var colors = Utils.GetGradientColors(Color.White, Color.LightGreen, 102);
                var color = colors.ElementAt(complProc);
                dataGridView1.Rows[row.Index].Cells[0].Style.BackColor = color;
            }
        }

        void SetTabControlSelected(string userName)
        {
            tabControl1.SelectTab(userName);
        }

        void SetCheckBoxSelected(string userName)
        {
            var indexByUserName = 0;
            foreach (var item in checkedListBox1.Items)
            {
                if (item.ToString() == userName)
                {
                    checkedListBox1.SelectedIndex = indexByUserName;
                    return;
                }
                indexByUserName++;
            }
        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedUser = checkedListBox1.SelectedItem.ToString();
            SetTabControlSelected(selectedUser);
        }
    }
}
