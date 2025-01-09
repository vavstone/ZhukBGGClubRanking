using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZhukBGGClubRanking.Core;
using ZhukBGGClubRanking.Core.Model;
using ZhukBGGClubRanking.WinApp.Core;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ZhukBGGClubRanking.WinApp
{
    public partial class ManageMyCollection : Form
    {
        public AppCache Cache { get; set; }
        public User CurrentUser { get; set; }
        public UserSettings UserSettings { get; set; }

        Timer timer1 = new Timer();
        bool cbxSearchCanUpdate = true;
        bool cbxSearchNeedUpdate = false;


        public ManageMyCollection()
        {
            InitializeComponent();
            timer1.Interval = 1000;
            timer1.Tick += timer1_Tick;
            //lblNewGameFullName.BackColor = Color.LightGray;
            lblNewGameFullName.ForeColor = Color.Black;
            lblNewGameFullName.ActiveLinkColor = Color.Black;
            lblNewGameFullName.LinkColor = Color.Black;
            lblNewGameFullName.LinkClicked += LblNewGameFullName_LinkClicked;
        }

        

        private void ManageMyCollection_Load(object sender, EventArgs e)
        {
            PrepareDataGrid();
            FillDataGrid();
            //FillRawGamesSelector();
        }



        #region GridMethods

        void FillDataGrid()
        {
            var myGames = Cache.Games.Where(c => c.Owners.Select(c1 => c1.UserId).Contains(CurrentUser.Id)).OrderBy(c => c.Name)
                .ToList();
            dgvMyCollection.DataSource = myGames;
        }

        private void Grid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var grid = sender as DataGridView;
            var gamesList = grid.DataSource as List<Game>;
            if (e.ColumnIndex == 0)
            {
                if (dgvMyCollection.Columns[e.ColumnIndex] is DataGridViewLinkColumn && e.RowIndex != -1)
                {

                    var game = gamesList[e.RowIndex];
                    string url = string.Empty;
                    var teseraUrl = Utils.GetTeseraCardUrl(game.TeseraKey);
                    var bggUrl = Utils.GetBGGCardUrl(game.BGGObjectId);
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

        public void PrepareDataGrid()
        {
            CreateGridColumns();
            dgvMyCollection.CellContentClick += Grid_CellContentClick;
            //grid.ColumnHeaderMouseClick += Grid_ColumnHeaderMouseClick;
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
        public void AddLinkColumn(DataGridView grid, string caption, string dataPropertyName, int width)
        {
            var col = new DataGridViewLinkColumn();
            col.Width = width;
            col.HeaderText = caption;
            col.Name = dataPropertyName;
            col.DataPropertyName = dataPropertyName;
            col.TrackVisitedState = false;
            col.LinkBehavior = LinkBehavior.HoverUnderline;
            col.LinkColor = DefaultForeColor;
            grid.Columns.Add(col);
        }

        public void CreateGridColumns()
        {
            AddLinkColumn(dgvMyCollection, "Название", "Name", 400);
            AddTextColumn(dgvMyCollection, "Владеют", "OwnersString", 200);
        }

        #endregion

        #region ComboboxMethods

        //void FillRawGamesSelector()
        //{
        //    cbSelectRawGame.DataSource = Cache.RawGames;
        //    cbSelectRawGame.DisplayMember = "FullName";
        //    cbSelectRawGame.ValueMember = "Id";
        //}

        private void UpdateData()
        {
            if (cbSelectRawGame.Text.Length > 1)
            {
                var searchData = TeseraBGGRawGame.FilterRawGames(Cache.RawGames, cbSelectRawGame.Text, 3);
                HandleTextChanged(searchData);
            }
        }


        //While timer is running don't start search
        //timer1.Interval = 1500;
        private void RestartTimer()
        {
            timer1.Stop();
            cbxSearchCanUpdate = false;
            timer1.Start();
        }

        //Update data when timer stops
        private void timer1_Tick(object sender, EventArgs e)
        {
            cbxSearchCanUpdate = true;
            timer1.Stop();
            UpdateData();
        }

        //Update combobox with new data
        private void HandleTextChanged(List<TeseraBGGRawGame> dataSource)
        {
            var text = cbSelectRawGame.Text;

            if (dataSource.Count() > 0)
            {
                cbSelectRawGame.DataSource = dataSource;

                var sText = cbSelectRawGame.Items[0].ToString();
                cbSelectRawGame.SelectionStart = text.Length;
                cbSelectRawGame.SelectionLength = sText.Length - text.Length;
                cbSelectRawGame.DroppedDown = true;
                // Restore default cursor
                Cursor.Current = Cursors.Default;
            }
            else
            {
                cbSelectRawGame.DroppedDown = false;
                cbSelectRawGame.SelectionStart = text.Length;
            }
        }

        private void cbSelectRawGame_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbxSearchNeedUpdate = false;
            UpdateNewGameInfoBlock();
        }

        //Update data only when the user (not program) change something
        private void cbSelectRawGame_TextUpdate(object sender, EventArgs e)
        {
            cbxSearchNeedUpdate = true;
        }

        //If text has been changed then start timer
        //If the user doesn't change text while the timer runs then start search
        private void cbSelectRawGame_TextChanged(object sender, EventArgs e)
        {
            if (cbxSearchNeedUpdate)
            {
                if (cbxSearchCanUpdate)
                {
                    cbxSearchCanUpdate = false;
                    UpdateData();
                }
                else
                {
                    RestartTimer();
                }
            }
        }
        #endregion


        void UpdateNewGameInfoBlock()
        {
            var selectedGame = cbSelectRawGame.SelectedItem as TeseraBGGRawGame;
            if (selectedGame != null)
            {
                lblNewGameFullName.Text = selectedGame.ToString();
                lblNewGameFullName.Tag = selectedGame;
                var picUrl = "";
                if (selectedGame.TeseraInfo != null && !string.IsNullOrWhiteSpace(selectedGame.TeseraInfo.PhotoUrl))
                    picUrl = selectedGame.TeseraInfo.PhotoUrl;
                if (!string.IsNullOrWhiteSpace(picUrl))
                {
                    //picUrl = "http://www.gravatar.com/avatar/6810d91caff032b202c50701dd3af745?d=identicon&r=PG";
                    //picBoxGame.ImageLocation = picUrl;
                    var request = WebRequest.Create(picUrl);
                    using (var response = request.GetResponse())
                    using (var stream = response.GetResponseStream())
                    {
                        picBoxGame.Image = Bitmap.FromStream(stream,false,false);
                    }
                }
            }
        }

        private void LblNewGameFullName_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var game = (sender as LinkLabel).Tag as TeseraBGGRawGame;
            string url = string.Empty;
            var teseraUrl = "";
            var bggUrl = "";
            if (game.TeseraInfo!=null)
                teseraUrl = Utils.GetTeseraCardUrl(game.TeseraInfo.Alias);
            if (game.BGGInfo!=null)
                bggUrl = Utils.GetBGGCardUrl(game.BGGInfo.Id);
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
