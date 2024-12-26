using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ZhukBGGClubRanking.Core;
using ZhukBGGClubRanking.WinApp.Core;

namespace ZhukBGGClubRanking.WinApp
{
    public partial class Form1 : Form
    {
        public GamesNamesTranslateList GamesTranslate { get; set; }
        public BGGCollection CommonCollection { get; set; }
        private List<GameRatingListFile> usersRatingListFiles;
        private GameRatingList currentAvarageRatingList;

        public UserSettings userSettings { get; set; }

        public Form1()
        {
            InitializeComponent();
            userSettings = UserSettings.GetUserSettings();
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
            SetTrBarTopXValue();
            SetFormCaption();
            UpdateAvarateRating();
            ClearSelectionInAllGrids();
        }

        void ClearSelectionInAllGrids()
        {
            dataGridView1.ClearSelection();
            foreach (TabPage page in tabControl1.TabPages)
            {
                var grid = page.Controls[0] as DataGridView;
                grid.ClearSelection();
            }
        }

        void LoadCommonCollection()
        {
            CommonCollection = BGGCollection.LoadFromFile();
            CommonCollection.UpplyTranslation(GamesTranslate);
        }

        void LoadGamesTranslate()
        {
            GamesTranslate = GamesNamesTranslateFile.LoadFromFile().GamesTranslate;
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
                var currentListUser = currentList.UserNames.FirstOrDefault();

                    foreach (var list in allLists.Where(c=>string.IsNullOrWhiteSpace(currentListUser) || c.UserNames.FirstOrDefault()!= currentListUser))
                    {
                        var game = list.GameList.FirstOrDefault(
                            c => c.GameEng.ToUpper() == gameToFill.GameEng.ToUpper());
                        if (game != null)
                        {
                            gameToFill.UsersRating.UserRating.Add(new UserRating
                                {UserName = list.UserNames.FirstOrDefault(), Rating = game.Rating});
                        }
                    }

                    if (gameToFill.UsersRating.UserRating.Any())
                    {
                        var splitter = "; ";
                        var resultStr = "";
                        foreach (var item in gameToFill.UsersRating.UserRating.OrderBy(c => c.Rating).ThenBy(c => c.UserName))
                            resultStr += string.Format("{0} - {1}{2}", item.Rating, item.UserName, splitter);
                        gameToFill.UserRatingString = resultStr.Substring(0, resultStr.Length - splitter.Length);
                    }
                
            }
        }

        public void CreateGridColumns(DataGridView grid, bool isUserRatingGrid)
        {
            TableSettings settings;
            if (isUserRatingGrid)
                settings = userSettings.Tables.UserRatingTable;
            else
                settings = userSettings.Tables.AverageRatingTable;

            grid.Columns.Clear();

            var colName = new DataGridViewLinkColumn();
            colName.Width = 310;
            colName.HeaderText = "Название";
            colName.Name = "Game";
            colName.DataPropertyName = "Game";
            colName.TrackVisitedState = false;
            colName.LinkBehavior = LinkBehavior.HoverUnderline;
            colName.LinkColor = DefaultForeColor;
            grid.Columns.Add(colName);

            if (settings.ShowRating)
            {
                var colRate = new DataGridViewTextBoxColumn();
                colRate.Width = 50;
                colRate.HeaderText = "Рейтинг";
                colRate.Name = "Rating";
                colRate.DataPropertyName = "Rating";
                colRate.ReadOnly = true;
                grid.Columns.Add(colRate);
            }

            if (settings.ShowUsersRating)
            {
                var col = new DataGridViewTextBoxColumn();
                col.HeaderText = "Рейтинг у игроков";
                col.Name = "UserRatingString";
                col.DataPropertyName = "UserRatingString";
                col.Width = 193;
                grid.Columns.Add(col);
            }

            if (settings.ShowOwners)
            {
                var col1 = new DataGridViewTextBoxColumn();
                col1.HeaderText = "Владеют";
                col1.Name = "BGGComments";
                col1.DataPropertyName = "BGGComments";
                col1.Width = 120;
                grid.Columns.Add(col1);
            }

            //TODO
            //AddImagesColumn(grid);
            //AddTestColumn(grid);
        }

        public void PrepareDataGrid(DataGridView grid, bool isUserRatingGrid)
        {
            

            grid.AutoGenerateColumns = false;
            grid.AllowUserToAddRows = false;
            grid.AllowUserToDeleteRows = false;
            grid.ShowEditingIcon = false;
            grid.RowHeadersVisible = false;

            CreateGridColumns(grid,isUserRatingGrid);
            
            grid.CellContentClick += Grid_CellContentClick;

            grid.RowsDefaultCellStyle.SelectionBackColor = Color.LightGray;
            grid.RowsDefaultCellStyle.SelectionForeColor = Color.Black;

            grid.MouseWheel += DataGridView_MouseWheel;
            grid.MouseEnter += DataGridView_MouseEnter;

            grid.ColumnHeaderMouseClick += Grid_ColumnHeaderMouseClick;
        }

        private int _previousIndex;
        private bool _sortDirection;
        private void Grid_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            var grid = sender as DataGridView;
            if (e.ColumnIndex == _previousIndex)
                _sortDirection ^= true; // toggle direction

            grid.DataSource = SortData((List<GameRating>)grid.DataSource, grid.Columns[e.ColumnIndex].Name, _sortDirection);

            _previousIndex = e.ColumnIndex;
            if (grid == dataGridView1)
                UpdateDataGridViewColors();

            grid.ClearSelection();
        }

        public List<GameRating> SortData(List<GameRating> list, string column, bool ascending)
        {
            if (ascending)
            {
                if (column== "Game")
                 list = list.OrderBy(c => c.Game).ToList();
                else if (column == "Rating")
                    list = list.OrderBy(c => c.Rating).ToList();
                else if (column == "BGGComments")
                    list = list.OrderBy(c => c.BGGComments).ToList();
                else if (column == "UserRatingString")
                    list = list.OrderBy(c => c.UsersRating).ToList();

            }
            else
            {
                if (column == "Game")
                    list = list.OrderByDescending(c => c.Game).ToList();
                else if (column == "Rating")
                    list = list.OrderByDescending(c => c.Rating).ToList();
                else if (column == "BGGComments")
                    list = list.OrderByDescending(c => c.BGGComments).ToList();
                else if (column == "UserRatingString")
                    list = list.OrderByDescending(c => c.UsersRating).ToList();
            }
            return list;
        }



        private void DataGridView_MouseEnter(object sender, EventArgs e)
        {
            var grid = sender as DataGridView;
            grid.Focus();
        }

        void DataGridView_MouseWheel(object sender, MouseEventArgs e)
        {
            var grid = sender as DataGridView;
            int currentIndex = grid.FirstDisplayedScrollingRowIndex;
            int scrollLines = SystemInformation.MouseWheelScrollLines;

            if (e.Delta > 0)
            {
                grid.FirstDisplayedScrollingRowIndex = Math.Max(0, currentIndex - scrollLines);
            }
            else if (e.Delta < 0)
            {
                if (grid.Rows.Count > (currentIndex + scrollLines))
                    grid.FirstDisplayedScrollingRowIndex = currentIndex + scrollLines;
            }
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
                    string url = string.Empty;
                    var teseraUrl = GetTeseraCardUrl(ratingItem.GameEng);
                    var bggUrl = string.Empty;
                    var gameInBGGColl = ratingItem.BGGItem;
                    if (gameInBGGColl != null)
                        bggUrl = GetBGGCardUrl(gameInBGGColl.ObjectId);
                    if (userSettings.TeseraPreferable)
                        url = teseraUrl;
                    if (string.IsNullOrWhiteSpace(url))
                        url = bggUrl;
                    ProcessStartInfo sInfo = new ProcessStartInfo(url);
                    Process.Start(sInfo);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            UpdateAvarateRating();   
        }

        List<string> GetSelectedUsers()
        {
            return checkedListBox1.CheckedItems.OfType<string>().ToList();
        }

        List<GameRatingList> GetSelectedUsersRatings()
        {
            var checkedUserNames = GetSelectedUsers();
            return usersRatingListFiles.Where(c => checkedUserNames.Contains(c.RatingList.UserNames.First())).Select(c => c.RatingList).ToList();
        }

        void UpdateAvarateRating()
        {
            //var isOnlyGamesInAllRatings = IsOnlyGamesInAllRatings();
            var topValue = GetTopXValue();
            var checkedLists = GetSelectedUsersRatings();
            currentAvarageRatingList = GameRatingList.CalculateAvarageRating(checkedLists, CommonCollection, topValue);
            currentAvarageRatingList.SetBGGCollection(CommonCollection);
            UpdateGamesCrossRatings(currentAvarageRatingList, usersRatingListFiles.Select(c => c.RatingList));
            Utils.CalcComplianceAverateRatingToSelectedUser_v2(GetCurrentSelectedUser(),currentAvarageRatingList, usersRatingListFiles);
            dataGridView1.DataSource = currentAvarageRatingList.GameList.OrderBy(c => c.Rating).ThenBy(c => c.Game).ToList();
            dataGridView1.ClearSelection();
            UpdateDataGridViewColors();
            //TODO
            //AddImagesToDataGrid(dataGridView1);

        }

        void SetTrBarTopXValue()
        {
            var checkedLists = GetSelectedUsersRatings();
            var maxCollSize = checkedLists.Select(c => c.GameList.Count).Max();
            trBarOnlyTop.Maximum = maxCollSize;
            SetCurrentTbBarToxValue(maxCollSize);
            SetlblTrBarToxValue();
        }

        void SetCurrentTbBarToxValue(int val)
        {
            trBarOnlyTop.Value = val;
        }

        void SetlblTrBarToxValue()
        {
            lbltrBarTopX.Text = string.Format("Учитывать топ {0} игр рейтинга выбранных участников", trBarOnlyTop.Value);
        }

        int GetTopXValue()
        {
            return trBarOnlyTop.Value;
        }

        //bool IsOnlyGamesInAllRatings()
        //{
        //    return cbGamesOnlyInAllRatings.Checked;
        //}

        //void CalcComplianceAverateRatingToSelectedUser()
        //{
        //    var currentSelectedUser = GetCurrentSelectedUser();
        //    if (!string.IsNullOrWhiteSpace(currentSelectedUser) && currentAvarageRatingList!=null)
        //    {
        //        var userRatingFile =
        //            usersRatingListFiles.FirstOrDefault(c => c.RatingList.UserNames.Contains(currentSelectedUser));
        //        if (userRatingFile != null)
        //        {
        //            foreach (var game in currentAvarageRatingList.GameList.OrderBy(c=>c.Rating))
        //            {
        //                var averageRating = game.Rating;
        //                var userGame =
        //                    userRatingFile.RatingList.GameList.FirstOrDefault(c => c.Game.ToUpper() == game.Game.ToUpper());
        //                if (userGame != null)
        //                {
        //                    var userRating = userGame.Rating;
        //                    var maxRatingSize = currentAvarageRatingList.GameList.Select(c => c.Rating).Max();
        //                    game.CompliancePercent = (int)Utils.GetCompliancePercent(averageRating, userRating, maxRatingSize);
        //                }
        //            }
        //        }
        //    }
        //}

        

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
            Utils.CalcComplianceAverateRatingToSelectedUser_v2(GetCurrentSelectedUser(), currentAvarageRatingList, usersRatingListFiles);
            UpdateDataGridViewColors();
            //SetCheckBoxSelected(GetCurrentSelectedUser());
            SetFormCaption();
            ClearSelectionInAllGrids();
        }

        void SetFormCaption()
        {
            var appName = "ZhukBGGClubRanking";
            var selectedUser = GetCurrentSelectedUser();
            this.Text = string.Format("{0}. Рейтинг участника {1}", appName, selectedUser);
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

        //void SetTabControlSelected(string userName)
        //{
        //    tabControl1.SelectTab(userName);
        //}

        //void SetCheckBoxSelected(string userName)
        //{
        //    var indexByUserName = 0;
        //    foreach (var item in checkedListBox1.Items)
        //    {
        //        if (item.ToString() == userName)
        //        {
        //            checkedListBox1.SelectedIndex = indexByUserName;
        //            return;
        //        }
        //        indexByUserName++;
        //    }
        //}

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (checkedListBox1.CheckedIndices.Count > 0)
            {
                var selectedItem = checkedListBox1.SelectedItem;
                if (selectedItem != null)
                {
                    var selectedUser = selectedItem.ToString();
                    //SetTabControlSelected(selectedUser);
                    SetTrBarTopXValue();
                    SetlblTrBarToxValue();
                }
            }
        }

        private void checkedListBox1_ItemCheck_1(object sender, ItemCheckEventArgs e)
        {
            if (checkedListBox1.CheckedIndices.Count == 1 &&
                e.NewValue == CheckState.Unchecked &&
                e.CurrentValue == CheckState.Checked)
            {
                int index = checkedListBox1.CheckedIndices[0];
                SetItemCallback d = new SetItemCallback(this.SetItem);
                d.BeginInvoke(index, null, null);
            } 
            //else if (checkedListBox1.CheckedIndices.Count == 0)
            //{
            //    var selectedItem = checkedListBox1.SelectedItem;
            //    if (selectedItem != null)
            //    {
            //        var selectedUser = selectedItem.ToString();
            //        SetTabControlSelected(selectedUser);
            //    }
            //}

        }



        private delegate void SetItemCallback(int index);

        private void SetItem(int index)
        {
            if (this.checkedListBox1.InvokeRequired)
            {
                SetItemCallback d = new SetItemCallback(SetItem);
                this.Invoke(d, new object[] { index });
            }
            else
            {
                checkedListBox1.SetItemChecked(index, true);
            }
        }

        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var about = new About();
            about.ShowDialog();
        }

        private void trBarOnlyTop_Scroll(object sender, EventArgs e)
        {
            SetlblTrBarToxValue();
            UpdateAvarateRating();
        }

        //private void cbGamesOnlyInAllRatings_CheckedChanged(object sender, EventArgs e)
        //{
        //    UpdateAvarateRating();
        //}

        private void lbltrBarTopX_Click(object sender, EventArgs e)
        {

        }

        public string GetTeseraCardUrl(string gameEng)
        {
            var teseraName = GamesTranslate.GetTeseraName(gameEng);
            if (!string.IsNullOrEmpty(teseraName))
                return Settings.TeseraCardPrefixUrl + teseraName;
            return null;
        }

        public string GetBGGCardUrl(int bggObjectId)
        {
            return Settings.BGGCardPrefixUrl + bggObjectId;
        }

        private void menuSelectTesera_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem menuItem = sender as ToolStripMenuItem;
            if (menuItem.CheckState == CheckState.Checked)
            {
                userSettings.TeseraPreferable = true;
                menuSelectBGG.Checked = false;
            }
            else if (menuItem.CheckState == CheckState.Unchecked)
            {
                userSettings.TeseraPreferable = false;
                menuSelectBGG.Checked = true;
            }
        }

        private void menuSelectBGG_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem menuItem = sender as ToolStripMenuItem;
            if (menuItem.CheckState == CheckState.Checked)
            {
                userSettings.TeseraPreferable = false;
                menuSelectTesera.Checked = false;
            }
            else if (menuItem.CheckState == CheckState.Unchecked)
            {
                userSettings.TeseraPreferable = true;
                menuSelectTesera.Checked = true;
            }
        }

        private void menuTablesColumns_Click(object sender, EventArgs e)
        {
            var selectTablesColumnsForm = new SelectTablesColumns();
            var result = selectTablesColumnsForm.CustomShow(userSettings.Tables);
            if (result == DialogResult.OK && (selectTablesColumnsForm.UserTableSettingsChanged || selectTablesColumnsForm.AverageTableSettingsChanged))
            {
                userSettings.Tables = selectTablesColumnsForm.Settings;
                if (selectTablesColumnsForm.UserTableSettingsChanged)
                {
                    foreach (var tab in tabControl1.TabPages)
                    {
                       var grid = (tab as TabPage).Controls[0] as DataGridView;
                        CreateGridColumns(grid, true);

                    }
                }
                if (selectTablesColumnsForm.AverageTableSettingsChanged)
                {
                    CreateGridColumns(dataGridView1,false);
                }
                ClearSelectionInAllGrids();
                UpdateDataGridViewColors();
            }
        }
    }
}
