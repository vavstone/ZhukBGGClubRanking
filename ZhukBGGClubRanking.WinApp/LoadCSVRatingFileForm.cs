using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZhukBGGClubRanking.Core.Model;
using ZhukBGGClubRanking.Core;
using ZhukBGGClubRanking.WinApp.Core;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Security.Cryptography;
using System.Security.Policy;

namespace ZhukBGGClubRanking.WinApp
{
    public partial class LoadCSVRatingFileForm : Form
    {
        public AppCache Cache { get; set; }
        public User CurrentUser { get; set; }

        public LoadCSVRatingFileForm()
        {
            InitializeComponent();
        }

        private void btSelectFile_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            string filename = openFileDialog1.FileName;
            lblInfo.Text = "Выбран файл " + filename;

            var csvRating = CSVHelper.GetRatingItemsFromCSVFile(filename, Cache.Games);

        }

        private void btLoadToServer_Click(object sender, EventArgs e)
        {
            //var res = await TestWebApi.SaveUsersRating(tbUrl.Text, tbLogin.Text, tbPassword.Text, tbLoginResult.Text, ratingItems);
            this.DialogResult = DialogResult.Yes;
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
                _sortDirection ^= true; // toggle direction

            grid.DataSource = SortData((List<User>)grid.DataSource, grid.Columns[e.ColumnIndex].Name, _sortDirection);

            _previousIndex = e.ColumnIndex;

            grid.ClearSelection();
        }

        public List<User> SortData(List<User> list, string column, bool ascending)
        {
            if (ascending)
            {
                if (column == "Id")
                    list = list.OrderBy(c => c.Id).ToList();
                else if (column == "Name")
                    list = list.OrderBy(c => c.Name).ToList();
                else if (column == "Password")
                    list = list.OrderBy(c => c.Password).ToList();
                else if (column == "EMail")
                    list = list.OrderBy(c => c.EMail).ToList();
                else if (column == "FullName")
                    list = list.OrderBy(c => c.FullName).ToList();
                else if (column == "IsActive")
                    list = list.OrderBy(c => c.IsActive).ToList();
                else if (column == "CreateTime")
                    list = list.OrderBy(c => c.CreateTime).ToList();
                else if (column == "Role")
                    list = list.OrderBy(c => c.Role).ToList();

            }
            else
            {
                if (column == "Id")
                    list = list.OrderByDescending(c => c.Id).ToList();
                else if (column == "Name")
                    list = list.OrderByDescending(c => c.Name).ToList();
                else if (column == "Password")
                    list = list.OrderByDescending(c => c.Password).ToList();
                else if (column == "EMail")
                    list = list.OrderByDescending(c => c.EMail).ToList();
                else if (column == "FullName")
                    list = list.OrderByDescending(c => c.FullName).ToList();
                else if (column == "IsActive")
                    list = list.OrderByDescending(c => c.IsActive).ToList();
                else if (column == "CreateTime")
                    list = list.OrderByDescending(c => c.CreateTime).ToList();
                else if (column == "Role")
                    list = list.OrderByDescending(c => c.Role).ToList();
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
            var settings = UserSettings.GetUserSettings();
            PrepareDataGrid(gridDBData);
            var currentRating = Cache.UsersRating.FirstOrDefault(c => c.UserId == CurrentUser.Id);
            var dataSourceWrapper = new List<GridViewDataSourceWrapper>();
            foreach (var ritem in currentRating.Rating.RatingItems)
            {
                var dataSourceWrapperItem = GridViewDataSourceWrapper.CreateFromCoreGame(ritem, Cache.Games, Cache.UsersRating);
                dataSourceWrapper.Add(dataSourceWrapperItem);
            }
            
            gridDBData.DataSource = dataSourceWrapper.OrderBy(c => c.Rating).ThenBy(c => c.Game).ToList();

        }
    }
}
