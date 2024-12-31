using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using ZhukBGGClubRanking.Core;
using ZhukBGGClubRanking.Core.Model;
using ZhukBGGClubRanking.WinApp.Core;

namespace ZhukBGGClubRanking.WinApp
{
    public partial class LoadCSVRatingFileForm : Form
    {
        public AppCache Cache { get; set; }
        public User CurrentUser { get; set; }
        public UserSettings UserSettings { get; set; }
        public List<RatingItem> LoadedFromCSVRating { get; set; }
        private BackgroundWorker bwUploadCSVFile = new BackgroundWorker();
        public LoadCSVRatingFileForm()
        {
            InitializeComponent();
            bwUploadCSVFile.WorkerSupportsCancellation = true;
            bwUploadCSVFile.WorkerReportsProgress = true;
            bwUploadCSVFile.DoWork += BwUploadCSVFile_DoWork;
            bwUploadCSVFile.RunWorkerCompleted += BwUploadCSVFile_RunWorkerCompleted;
        }

        private void CacheLoaded(object sender, EventArgs e)
        {
            var result = (e as WebResultEventArgs).Result as WebDataAllListstResultForBW;
            if (!result.Result)
            {
                MessageBox.Show("Ошибка получения данных с сервера. " + result.Message);
            }
            else
            {
                FillDBDataGrid();
            }
        }

        private void BwUploadCSVFile_DoWork(object sender, DoWorkEventArgs e)
        {
            var options = e.Argument as UploadRatingPrmForBW;
            var result = new WebDataResultForBW();
            var reqResult = WebApiHandler.SaveUsersRating(
                options.HostingSettings.Url,
                options.HostingSettings.Login,
                options.HostingSettings.Password,
                JWTPrm.Token,
                options.RatingItems);

            if (reqResult.Result.StatusCode.ToString() == "OK")
            {
                result.Result = true;
            }
            else
            {
                result.Result = false;
                result.Message = reqResult.Result.StatusCode.ToString();
            }
            e.Result = result;
        }

        private void BwUploadCSVFile_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            var result = e.Result as WebDataResultForBW;
            if (result.Result)
            {
                Cache.LoadAll(UserSettings.Hosting);
                ClearCSVDataGrig();
                SetLblInfoTextDefault();
            }
            else
            {
                MessageBox.Show(result.Message);
            }
            LoadedFromCSVRating = null;
        }

        void ClearCSVDataGrig()
        {
            gridCSVData.DataSource = null;
        }

        void FillDBDataGrid()
        {
            var currentRating = Cache.UsersRating.FirstOrDefault(c => c.UserId == CurrentUser.Id);
            gridDBData.DataSource = DataGridViewHelper.CreateDataSourceWrapper(currentRating.Rating.RatingItems, Cache.Games);
            tabControl1.SelectedIndex = 0;
        }

        void FillCSVDataGrid(List<RatingItem> csvRating)
        {
            gridCSVData.DataSource = DataGridViewHelper.CreateDataSourceWrapper(csvRating, Cache.Games);
            tabControl1.SelectedIndex = 1;
        }

        private void btSelectFile_Click(object sender, EventArgs e)
        {
            openFileDialog1.AutoUpgradeEnabled = false;
            openFileDialog1.AddExtension = true;
            openFileDialog1.Filter = "Файлы csv (*.csv)|*.csv";
            if (openFileDialog1.ShowDialog(this) == DialogResult.Cancel)
                return;
            string filename = openFileDialog1.FileName;
            lblInfo.Text = "Выбран файл " + filename;
            var csvRating = UserRatingFile.GetRatingItemsFromCSVFile(filename, Cache.Games);
            LoadedFromCSVRating = csvRating;
            FillCSVDataGrid(csvRating);
        }

        void SetLblInfoTextDefault()
        {
            lblInfo.Text = "Файл не выбран";
        }

        private void btLoadToServer_Click(object sender, EventArgs e)
        {
            if (LoadedFromCSVRating != null)
                bwUploadCSVFile.RunWorkerAsync(new UploadRatingPrmForBW 
                { 
                    RatingItems = LoadedFromCSVRating,
                    HostingSettings = UserSettings.Hosting
                });
        }

        public void PrepareDataGrid(DataGridView grid)
        {
            grid.AutoGenerateColumns = false;
            grid.AllowUserToAddRows = false;
            grid.AllowUserToDeleteRows = false;
            grid.ShowEditingIcon = false;
            grid.RowHeadersVisible = false;
            CreateGridColumns(grid);
            grid.RowsDefaultCellStyle.SelectionBackColor = Color.LightGray;
            grid.RowsDefaultCellStyle.SelectionForeColor = Color.Black;
            grid.MouseWheel += DataGridView_MouseWheel;
            grid.MouseEnter += DataGridView_MouseEnter;
            grid.ColumnHeaderMouseClick += Grid_ColumnHeaderMouseClick;
        }

        public void CreateGridColumns(DataGridView grid)
        {
            //grid.Columns.Clear();
            AddTextColumn(grid, "Название", "Game", 400);
            AddTextColumn(grid, "Рейтинг", "Rating", 200);
            AddTextColumn(grid, "Владеют", "BGGComments", 200);
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

        private int _previousIndex;
        private bool _sortDirection;
        private void Grid_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            var grid = sender as DataGridView;
            if (e.ColumnIndex == _previousIndex)
                _sortDirection ^= true;
            grid.DataSource = SortData((List<GridViewDataSourceWrapper>)grid.DataSource, grid.Columns[e.ColumnIndex].Name, _sortDirection);
            _previousIndex = e.ColumnIndex;
            grid.ClearSelection();
        }

        public List<GridViewDataSourceWrapper> SortData(List<GridViewDataSourceWrapper> list, string column, bool ascending)
        {
            if (ascending)
            {
                if (column == "Game")
                    list = list.OrderBy(c => c.Game).ToList();
                else if (column == "Rating")
                    list = list.OrderBy(c => c.Rating).ToList();
                else if (column == "BGGComments")
                    list = list.OrderBy(c => c.BGGComments).ToList();
            }
            else
            {
                if (column == "Game")
                    list = list.OrderByDescending(c => c.Game).ToList();
                else if (column == "Rating")
                    list = list.OrderByDescending(c => c.Rating).ToList();
                else if (column == "BGGComments")
                    list = list.OrderByDescending(c => c.BGGComments).ToList();
            }
            return list;
        }

        public void AddTextColumn(DataGridView grid, string caption, string dataPropertyName, int width)
        {
            var col = new DataGridViewTextBoxColumn();
            col.Width = width;
            col.HeaderText = caption;
            col.Name = dataPropertyName;
            col.DataPropertyName = dataPropertyName;
            grid.Columns.Add(col);
        }

        private void LoadCSVRatingFileForm_Load(object sender, EventArgs e)
        {
            PrepareDataGrid(gridDBData);
            PrepareDataGrid(gridCSVData);
            FillDBDataGrid();
            Cache.AllLoaded += CacheLoaded;
            SetLblInfoTextDefault();
        }

        private void LoadCSVRatingFileForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Cache.AllLoaded -= CacheLoaded;
        }
    }

}
