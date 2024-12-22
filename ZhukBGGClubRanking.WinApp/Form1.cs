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
        public GamesNamesTranslateFile GamesTranslate { get; set; }
        public BGGCollection CommonCollection { get; set; }
        private List<GameRatingListFile> usersRatingListFiles;
        private GameRatingList currentAvarageRatingList;

        public Form1()
        {
            InitializeComponent();
            LoadGamesTranslate();
            LoadCommonCollection();
            LoadUsersRatings();
        }

        public GameRatingList GetRatingInOpenedTab()
        {
            var currentTab = GetCurrentSelectedUser();
            return usersRatingListFiles.FirstOrDefault(c=>c.RatingList.UserNames.Contains(currentTab)).RatingList;
        }


        private void Form1_Load(object sender, EventArgs e)
        {
           
            SetButton2Text();
            PrepareDataGrid(dataGridView1,false);
        }

        void LoadCommonCollection()
        {
            CommonCollection = BGGCollection.LoadFromFile(GamesTranslate);
        }

        void LoadGamesTranslate()
        {
            GamesTranslate = GamesNamesTranslateFile.LoadFromFile();
        }

        void LoadUsersRatings()
        {
            usersRatingListFiles = GameRatingListFile.LoadFromFolder(CommonCollection);
            tabControl1.TabPages.Clear();
            checkedListBox1.Items.Clear();
            foreach (var item in usersRatingListFiles)
            {
                UpdateGamesCrossRatings(item.RatingList, usersRatingListFiles.Select(c => c.RatingList));
            }
            foreach (var item in usersRatingListFiles)
            {
                checkedListBox1.Items.Add(item.FileNameWithoutExt, true);
                var tabPage = new TabPage(item.FileNameWithoutExt);
                tabPage.Name = item.FileNameWithoutExt;
                tabControl1.TabPages.Add(tabPage);
                var grid = new DataGridView();
                grid.Dock = DockStyle.Fill;
                PrepareDataGrid(grid,true);
                grid.DataSource = item.RatingList.GameList.OrderBy(c => c.Rating).ThenBy(c => c.Game).ToList();
                tabPage.Controls.Add(grid);
                grid.ClearSelection();
            }

        }

        private void UpdateGamesCrossRatings(GameRatingList currentList, IEnumerable<GameRatingList> allLists)
        {
            foreach (var gameToFill in currentList.GameList)
            {
                foreach (var list in allLists)
                {
                    var game = list.GameList.FirstOrDefault(c => c.GameEng.ToUpper() == gameToFill.GameEng.ToUpper());
                    if (game != null)
                    {
                        gameToFill.UserRating.Add(new UserRating {UserName = list.UserNames.FirstOrDefault(), Rating = game.Rating});
                    }
                }

                if (gameToFill.UserRating.Any())
                {
                    var splitter = "; ";
                    var resultStr = "";
                    foreach (var item in gameToFill.UserRating.OrderBy(c => c.Rating).ThenBy(c => c.UserName))
                        resultStr += string.Format("{0} - {1}{2}", item.Rating, item.UserName, splitter);
                    gameToFill.UserRatingString = resultStr.Substring(0, resultStr.Length - splitter.Length);
                }

            }
        }

        public void PrepareDataGrid(DataGridView grid, bool generateAllRatingsColumn)
        {
            grid.AutoGenerateColumns = false;
            grid.AllowUserToAddRows = false;
            grid.AllowUserToDeleteRows = false;
            grid.ShowEditingIcon = false;
            //var colName = new DataGridViewTextBoxColumn();
            var colName = new DataGridViewLinkColumn();
            colName.Width = 330;
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
            if (generateAllRatingsColumn)
                AddAllRatingsColumn(grid);
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
            var gameRatingList = grid.DataSource as List<GameRating>;
            if (e.ColumnIndex == 0)
            {
                if (dataGridView1.Columns[e.ColumnIndex] is DataGridViewLinkColumn && e.RowIndex != -1)
                {
                    
                    //string cellContent = grid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                    var ratingItem = gameRatingList[e.RowIndex];
                    var gameInBGGColl = ratingItem.BGGItem;
                    if (gameInBGGColl != null)
                    {
                        var url = Settings.UrlForGameBGGId(gameInBGGColl.ObjectId);
                        ProcessStartInfo sInfo = new ProcessStartInfo(url);
                        Process.Start(sInfo);
                    }
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
            var checkedLists = usersRatingListFiles.Where(c => checkedUserNames.Contains(c.RatingList.UserNames.First())).Select(c => c.RatingList).ToList();
            currentAvarageRatingList = GameRatingList.CalculateAvarageRating(checkedLists, CommonCollection);
            currentAvarageRatingList.SetBGGCollection(CommonCollection);
            CalcComplianceAverateRatingToSelectedUser_v2();
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
                    usersRatingListFiles.FirstOrDefault(c => c.RatingList.UserNames.Contains(currentSelectedUser));
                if (userRatingFile != null)
                {
                    foreach (var game in currentAvarageRatingList.GameList.OrderBy(c=>c.Rating))
                    {
                        var averageRating = game.Rating;
                        var userGame =
                            userRatingFile.RatingList.GameList.FirstOrDefault(c => c.Game.ToUpper() == game.Game.ToUpper());
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

        void CalcComplianceAverateRatingToSelectedUser_v2()
        {
            var currentSelectedUser = GetCurrentSelectedUser();
            if (!string.IsNullOrWhiteSpace(currentSelectedUser) && currentAvarageRatingList != null)
            {
                var userRatingFile =
                    usersRatingListFiles.FirstOrDefault(c => c.RatingList.UserNames.Contains(currentSelectedUser));
                if (userRatingFile != null)
                {
                    foreach (var game in currentAvarageRatingList.GameList)
                    {
                        //var averageRating = game.Rating;
                        var userGame =
                            userRatingFile.RatingList.GameList.FirstOrDefault(c => c.GameEng.ToUpper() == game.GameEng.ToUpper());
                        if (userGame != null)
                        {
                            var userRating = userGame.Rating;
                            var maxRatingSize = userRatingFile.RatingList.GameList.Select(c => c.Rating).Max();
                            game.CompliancePercent = (int)Utils.GetCompliancePercent_v2(userRating, maxRatingSize);
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

        void AddAllRatingsColumn(DataGridView dataGridView)
        {
            var col = new DataGridViewTextBoxColumn();
            col.Name = "Рейтинг у игроков";
            col.DataPropertyName = "UserRatingString";
            col.Width = 229;
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
            var reorderForm = new ReorderForm();
            reorderForm.RatingList = GetRatingInOpenedTab();
            var otherCollections = usersRatingListFiles
                .Where(c => c.RatingList.UserNames.FirstOrDefault() != reorderForm.RatingList.UserNames.FirstOrDefault())
                .Select(c => c.RatingList).ToList();
            var newGamesInOthersColl = reorderForm.RatingList.GetGamesNotInCollectionButExistingInOthers(otherCollections,CommonCollection);
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
            CalcComplianceAverateRatingToSelectedUser_v2();
            UpdateDataGridViewColors();
            SetCheckBoxSelected(GetCurrentSelectedUser());
        }

        void UpdateDataGridViewColors()
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                var complProc = (dataGridView1.DataSource as List<GameRating>)[row.Index].CompliancePercent;
                var colors = Utils.GetGradientColors(Color.White, Color.LimeGreen, 102);
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
            var selectedItem = checkedListBox1.SelectedItem;
            if (selectedItem != null)
            {
                var selectedUser = selectedItem.ToString();
                SetTabControlSelected(selectedUser);
            }
        }
    }
}
