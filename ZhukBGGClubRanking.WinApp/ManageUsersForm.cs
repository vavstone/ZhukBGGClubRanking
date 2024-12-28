using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ZhukBGGClubRanking.Core;
using ZhukBGGClubRanking.Core.Code;
using ZhukBGGClubRanking.WinApp.Core;

namespace ZhukBGGClubRanking.WinApp
{
    public partial class ManageUsersForm : Form
    {
        public UserSettings UserSettings { get; set; }

        private BackgroundWorker bw = new BackgroundWorker();

        public AppCache Cache { get; set; }

        public ManageUsersForm()
        {
            InitializeComponent();
            bw.WorkerSupportsCancellation = true;
            bw.WorkerReportsProgress = true;
            bw.DoWork += Bw_DoWork;
            bw.RunWorkerCompleted += Bw_RunWorkerCompleted;
        }

        private void Bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            var result = e.Result as WebDataResultForBW;
            if (result.Result)
            {
                Cache.LoadUsers(UserSettings.Hosting);
                ClearUsersFields();
            }
            else
            {
                MessageBox.Show(result.Message);
            }
        }

        void ClearUsersFields()
        {
            tbLogin.Text = tbFullName.Text = tbEmail.Text = tbPassword.Text = string.Empty;
            cbxRole.SelectedIndex = 0;
        }

        private async void Bw_DoWork(object sender, DoWorkEventArgs e)
        {
            var options = e.Argument as CreateUserPrmForBW;
            var result = new WebDataResultForBW();
            var reqResult = WebApiHandler.CreateUserByAdmin(
                options.HostingSettings.Url, 
                options.HostingSettings.Login,
                options.HostingSettings.Password,
                JWTPrm.Token, 
                options.NewUser);

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

        private void UsersLoaded(object sender, EventArgs e)
        {
            dataGridView1.DataSource = Cache.Users;
        }

        private async void btCreateUser_Click(object sender, EventArgs e)
        {
            var newUser = new User();
            newUser.Name = tbLogin.Text;
            newUser.FullName = tbFullName.Text;
            newUser.Password = tbPassword.Text;
            newUser.EMail = tbEmail.Text;
            newUser.CreateTime = DateTime.Now;
            newUser.Role = cbxRole.Text;
            newUser.TrimAndNamePasswordToLower();
            string validationErrors;
            var validationResult = newUser.ValidateBeforeCreateOrChange(out validationErrors);
            if (!validationResult)
            {
                MessageBox.Show(validationErrors);
                return;
            }
            newUser.HashPassword();
            if (!bw.IsBusy)
            {
                var prm = new CreateUserPrmForBW
                {
                    HostingSettings = UserSettings.Hosting,
                    NewUser = newUser
                };
                bw.RunWorkerAsync(prm);
            }
            else
            {
                MessageBox.Show("Процесс занят выполнением запроса, ждите");
            }
        }

        

        public void CreateGridColumns(DataGridView grid)
        {
            //grid.Columns.Clear();
            AddTextColumn(grid, "Id", "Id",50);
            AddTextColumn(grid, "Name", "Name", 150);
            //AddTextColumn(grid, "Password", "Password", 100);
            AddTextColumn(grid, "EMail", "EMail", 200);
            AddTextColumn(grid, "FullName", "FullName", 100);
            AddTextColumn(grid, "IsActive", "IsActive", 50);
            AddTextColumn(grid, "CreateTime", "CreateTime", 100);
            AddTextColumn(grid, "Role", "Role", 100);
        }

        private void ManageUsersForm_Load(object sender, EventArgs e)
        {
            ClearUsersFields();
            UserSettings = UserSettings.GetUserSettings();
            PrepareDataGrid(dataGridView1);
            Cache.UsersLoaded += UsersLoaded;
            Cache.LoadUsers(UserSettings.Hosting);
        }
    }

    public class CreateUserPrmForBW:WebPrmForBW
    {
        internal User NewUser { get; set; }
    }

    //internal class LoginResultForBW
    //{
    //    internal bool Result { get; set; }
    //    internal string Token { get; set; }
    //    internal string UserName { get; set; }
    //    internal string Message { get; set; }
    //}
}
