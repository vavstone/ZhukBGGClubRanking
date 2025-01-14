using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ZhukBGGClubRanking.Core;
using ZhukBGGClubRanking.Core.Model;
using ZhukBGGClubRanking.WinApp.Core;

namespace ZhukBGGClubRanking.WinApp
{
    public partial class Form1 : Form
    {
        UsersRating currentAvarageRatingList;
        UserSettings UserSettings { get; set; }
        AppCache Cache { get; set; }
        User CurrentUser { get; set; }
        private bool adminElementsLoaded = false;

        public Form1()
        {
            InitializeComponent();
            UserSettings = UserSettings.GetUserSettings();
            Cache = new AppCache();
            Cache.AllLoaded += AllLoaded;
            Cache.LoadAll(UserSettings.Hosting);
            Cache.LoadRawGames(UserSettings.Hosting);
            //Cache.RawGamesLoaded += (o, e) => { MessageBox.Show("Raw"); };
        }

        private void AllLoaded(object sender, EventArgs e)
        {
            var result = (e as WebResultEventArgs).Result as WebDataAllListstResultForBW;
            if (!result.Result)
            {
                MessageBox.Show("Ошибка получения данных с сервера. " + result.Message);
            }
            else
            {
                if (CurrentUser == null)
                    CurrentUser = Cache.Users.FirstOrDefault(c => c.Name.ToLower() == JWTPrm.UserName.ToLower());
                LoadAdminElements();
                LoadUsersRatings();
                SetTrBarTopXValue();
                SetFormCaption();
                UpdateAvarateRating();
                ClearSelectionInAllGrids();
                SetTabPageCurrentUser();
            }
        }

        void LoadAdminElements()
        {
            if (CurrentUser.Role == Role.AdminRole && !adminElementsLoaded)
            {
                var adminFunctionsButton = new ToolStripMenuItem("Администрирование");
                var addNewUserMenu = new ToolStripMenuItem("Управление пользователями");
                var updateFromTeseraAndBGGMenu = new ToolStripMenuItem("Обновление данных с tesera и bgg");
                addNewUserMenu.Click += AddNewUserMenu_Click;
                updateFromTeseraAndBGGMenu.Click += UpdateFromTeseraAndBGGMenu_Click;
                adminFunctionsButton.DropDownItems.Add(addNewUserMenu);
                adminFunctionsButton.DropDownItems.Add(updateFromTeseraAndBGGMenu);
                menuStrip1.Items.Add(adminFunctionsButton);
                adminElementsLoaded = true;
            }
        }

        private void UpdateFromTeseraAndBGGMenu_Click(object sender, EventArgs e)
        {
            var updateFromTeseraAndBGGForm = new UpdateFromTeseraAndBGGForm();
            updateFromTeseraAndBGGForm.Settings = UserSettings.Hosting;
            updateFromTeseraAndBGGForm.ShowDialog(this);
        }

        private void AddNewUserMenu_Click(object sender, EventArgs e)
        {
            var manageUsersForm = new ManageUsersForm();
            manageUsersForm.Cache = Cache;
            manageUsersForm.ShowDialog(this);
            //LoadUsersRatings();
            //Cache.LoadAll(UserSettings.Hosting);
        }

        public UsersRating GetRatingInOpenedTab()
        {
            var currentUser = GetCurrentSelectedUser();
            return Cache.UsersRating.FirstOrDefault(c=>c.UserId== currentUser.Id);
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            PrepareDataGrid(dataGridView1,false); 
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

        //void LoadCommonCollection()
        //{
        //    CommonCollection = BGGCollection.LoadFromFile();
        //    CommonCollection.ApplyTranslation(GamesTranslate);
        //}

        //void LoadGamesTranslate()
        //{
        //    GamesTranslate = GamesNamesTranslateFile.LoadFromFile().GamesTranslate;
        //}

        void LoadUsersRatings()
        {
            //usersRatingListFiles = GameRatingListFile.LoadFromFolder(CommonCollection);
            //Cache.LoadAll(UserSettings.Hosting);
            tabControl1.TabPages.Clear();
            checkedListBox1.Items.Clear();
            //foreach (var item in Cache.UsersRating.OrderBy(c => Cache.Users.First(c1 => c1.Id == c.UserId).Name))
            //{
                //UpdateGamesCrossRatings(item.RatingList, usersRatingListFiles.Select(c => c.RatingList));
            //}
            foreach (var item in Cache.UsersRating.OrderBy(c => Cache.Users.First(c1 => c1.Id == c.UserId).Name))
            {
                var ratingUser = Cache.Users.FirstOrDefault(c => c.Id == item.UserId);
                checkedListBox1.Items.Add(ratingUser.Name, true);
                var tabPage = new TabPage(ratingUser.Name);
                tabPage.Name = ratingUser.Name;
                tabControl1.TabPages.Add(tabPage);
                var grid = new DataGridViewCustom();
                grid.Dock = DockStyle.Fill;
                PrepareDataGrid(grid,true);
                grid.DataSource = DataGridViewHelper.CreateDataSourceWrapper(item.Rating.RatingItems,Cache.Games);
                tabPage.Controls.Add(grid);
                grid.ClearSelection();
            }
        }

        

        

        public void CreateGridColumns(DataGridView grid, bool isUserRatingGrid)
        {
            TableSettings settings;
            if (isUserRatingGrid)
                settings = UserSettings.Tables.UserRatingTable;
            else
                settings = UserSettings.Tables.AverageRatingTable;

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

        public void PrepareDataGrid(DataGridViewCustom grid, bool isUserRatingGrid)
        {
            CreateGridColumns(grid,isUserRatingGrid);
            grid.CellContentClick += Grid_CellContentClick;
            grid.ColumnHeaderMouseClick += Grid_ColumnHeaderMouseClick;
        }

        private int _previousIndex;
        private bool _sortDirection;
        private void Grid_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            var grid = sender as DataGridView;
            if (e.ColumnIndex == _previousIndex)
                _sortDirection ^= true; // toggle direction
            grid.DataSource = SortData((List<GridViewDataSourceWrapper>)grid.DataSource, grid.Columns[e.ColumnIndex].Name, _sortDirection);
            _previousIndex = e.ColumnIndex;
            if (grid == dataGridView1)
                UpdateDataGridViewColors();
            grid.ClearSelection();
        }

        public List<GridViewDataSourceWrapper> SortData(List<GridViewDataSourceWrapper> list, string column, bool ascending)
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
                    list = list.OrderBy(c => c.RatingItem).ToList();

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
                    list = list.OrderByDescending(c => c.RatingItem).ToList();
            }
            return list;
        }

        private void Grid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var grid = sender as DataGridView;
            var gameRatingList = grid.DataSource as List<GridViewDataSourceWrapper>;
            if (e.ColumnIndex == 0)
            {
                if (dataGridView1.Columns[e.ColumnIndex] is DataGridViewLinkColumn && e.RowIndex != -1)
                {
                    
                    //string cellContent = grid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                    var ratingItem = gameRatingList[e.RowIndex];
                    string url = string.Empty;
                    var teseraUrl = Utils.GetTeseraCardUrl(ratingItem.TeseraKey);
                    var bggUrl = Utils.GetBGGCardUrl(ratingItem.BGGObjectId);
                    if (UserSettings.TeseraPreferable)
                        url = teseraUrl;
                    if (string.IsNullOrWhiteSpace(url))
                        url = bggUrl;
                    if (!string.IsNullOrWhiteSpace(url))
                    {
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

        List<string> GetSelectedUsers()
        {
            return checkedListBox1.CheckedItems.OfType<string>().ToList();
        }

        List<UsersRating> GetSelectedUsersRatings()
        {
            var checkedUserNames = GetSelectedUsers();
            var selectedUsers = Cache.Users.Where(c => checkedUserNames.Contains(c.Name)).ToList();
            return Cache.UsersRating.Where(c =>selectedUsers.Select(c1=>c1.Id).Contains(c.UserId)).ToList();
        }

        void UpdateAvarateRating()
        {
            //var isOnlyGamesInAllRatings = IsOnlyGamesInAllRatings();
            var topValue = GetTopXValue();
            var checkedLists = GetSelectedUsersRatings();
            currentAvarageRatingList = UsersRating.CalculateAvarageRating(checkedLists, Cache.Games, topValue);
           
            if (currentAvarageRatingList != null)
            {
                currentAvarageRatingList.UpdateGamesCrossRatings(Cache.UsersRating, Cache.Users);
                //currentAvarageRatingList.SetBGGCollection(CommonCollection);

                Utils.CalcComplianceAverateRatingToSelectedUser_v2(GetCurrentSelectedUser(), currentAvarageRatingList, Cache.UsersRating);
                //dataGridView1.DataSource = currentAvarageRatingList.Rating.RatingItems.OrderBy(c => c.RatingOrder).ThenBy(c => c.GameId).ToList();
                dataGridView1.DataSource = DataGridViewHelper.CreateDataSourceWrapper(currentAvarageRatingList.Rating.RatingItems, Cache.Games);
                dataGridView1.ClearSelection();
                UpdateDataGridViewColors();

                //TODO
                //AddImagesToDataGrid(dataGridView1);
            }


        }

        void SetTrBarTopXValue()
        {
            var checkedLists = GetSelectedUsersRatings();
            if (checkedLists.Any())
            {
                var maxCollSize = checkedLists.Select(c => c.Rating.RatingItems.Count).Max();
                trBarOnlyTop.Maximum = maxCollSize;
                SetCurrentTbBarToxValue(maxCollSize);
                SetlblTrBarToxValue();
            }
            
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

        private void button2_Click(object sender, EventArgs e)
        {
            var reorderForm = new ReorderForm();
            var ratingCurrentUser = Cache.UsersRating.FirstOrDefault(c => c.UserId == CurrentUser.Id);
            reorderForm.RatingList = ratingCurrentUser;
            reorderForm.CurrentUser = CurrentUser;
            reorderForm.Cache = Cache;
            reorderForm.Settings = UserSettings.Hosting;


            var newGames = Cache.Games.Where(c => c.IsStandaloneGame && (ratingCurrentUser==null || !ratingCurrentUser.Rating.RatingItems.Any(c1 => c1.GameId == c.Id))).ToList();
            if (newGames.Count > 0)
            {
                if (MessageBox.Show(string.Format("В общей коллекции найдены игры ({0} штук), отсуствующие в вашем рейтинге. Добавить их?", newGames.Count),
                        "Подтверждение добавления",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question,
                        MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                {
                    reorderForm.NewGames = newGames;
                }
            }
            if (reorderForm.ShowDialog(this) == DialogResult.OK)
            {
                Cache.LoadAll(UserSettings.Hosting);
            }
        }

        User GetCurrentSelectedUser()
        {
            if (tabControl1.SelectedTab != null)
                return Cache.Users.FirstOrDefault(c => c.Name == tabControl1.SelectedTab.Name);
            return null;
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Utils.CalcComplianceAverateRatingToSelectedUser_v2(GetCurrentSelectedUser(), currentAvarageRatingList, Cache.UsersRating);
            UpdateDataGridViewColors();
            //SetCheckBoxSelected(GetCurrentSelectedUser());
            SetFormCaption();
            ClearSelectionInAllGrids();
        }

        void SetFormCaption()
        {
            var appName = "ZhukBGGClubRanking";
            var selectedUser = GetCurrentSelectedUser();
            if (selectedUser!=null)
                this.Text = string.Format("{0}. Рейтинг участника {1}", appName, selectedUser.Name);
        }

        void UpdateDataGridViewColors()
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                var complProc = (dataGridView1.DataSource as List<GridViewDataSourceWrapper>)[row.Index].CompliancePercent;
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

        

        private void menuSelectTesera_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem menuItem = sender as ToolStripMenuItem;
            if (menuItem.CheckState == CheckState.Checked)
            {
                UserSettings.TeseraPreferable = true;
                menuSelectBGG.Checked = false;
            }
            else if (menuItem.CheckState == CheckState.Unchecked)
            {
                UserSettings.TeseraPreferable = false;
                menuSelectBGG.Checked = true;
            }
        }

        private void menuSelectBGG_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem menuItem = sender as ToolStripMenuItem;
            if (menuItem.CheckState == CheckState.Checked)
            {
                UserSettings.TeseraPreferable = false;
                menuSelectTesera.Checked = false;
            }
            else if (menuItem.CheckState == CheckState.Unchecked)
            {
                UserSettings.TeseraPreferable = true;
                menuSelectTesera.Checked = true;
            }
        }

        private void menuTablesColumns_Click(object sender, EventArgs e)
        {
            var selectTablesColumnsForm = new SelectTablesColumns();
            var result = selectTablesColumnsForm.CustomShow(UserSettings.Tables);
            if (result == DialogResult.OK && (selectTablesColumnsForm.UserTableSettingsChanged || selectTablesColumnsForm.AverageTableSettingsChanged))
            {
                UserSettings.Tables = selectTablesColumnsForm.Settings;
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

        private void menuUploadCSVFile_Click(object sender, EventArgs e)
        {
            var loadCSVFileForm = new LoadCSVRatingFileForm();
            loadCSVFileForm.CurrentUser = CurrentUser;
            loadCSVFileForm.Cache = Cache;
            loadCSVFileForm.UserSettings = UserSettings;
            if (loadCSVFileForm.ShowDialog(this) == DialogResult.Yes)
                Cache.LoadAll(UserSettings.Hosting);
        }

        void SetTabPageCurrentUser()
        {
            var currentUserPage = tabControl1.TabPages.OfType<TabPage>().FirstOrDefault(tp => tp.Text == CurrentUser.Name);
            if (currentUserPage!=null)
                tabControl1.SelectedTab = currentUserPage;
        }

        private void menuManageMyCollection_Click(object sender, EventArgs e)
        {
            var manageMyCollection = new ManageMyCollection();
            manageMyCollection.Cache = Cache;
            manageMyCollection.CurrentUser = CurrentUser;
            manageMyCollection.UserSettings = UserSettings;
            manageMyCollection.ShowDialog(this);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            UserSettings.SaveUserSettings();
        }
    }
}
