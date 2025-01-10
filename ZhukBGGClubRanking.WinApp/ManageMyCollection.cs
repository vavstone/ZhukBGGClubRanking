using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ZhukBGGClubRanking.Core;
using ZhukBGGClubRanking.Core.Model;
using ZhukBGGClubRanking.WinApp.Core;

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
                if (dgvMyCollection.Columns[e.ColumnIndex] is DataGridViewLinkColumn && e.RowIndex >= 0)
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
            dgvMyCollection.CellClick += DgvMyCollection_CellClick;
            //grid.ColumnHeaderMouseClick += Grid_ColumnHeaderMouseClick;
        }

        private void DgvMyCollection_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            var grid = sender as DataGridView;
            var gamesList = grid.DataSource as List<Game>;
            if (e.ColumnIndex == 2 && e.RowIndex >= 0)
            {
                var game = gamesList[e.RowIndex];
                var message = string.Format("Вы действительно хотите удалить игру {0} из своей коллекции?",
                    game);
                if (MessageBox.Show(message, "Подтверждение удаления", MessageBoxButtons.YesNo,
                        MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    MessageBox.Show("Здесь произойдут действия по удалению игры");
                }
            }
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
        public void AddActionColumn(DataGridView grid, string caption, string buttonText, int width)
        {
            var col = new DataGridViewButtonColumn();
            col.Width = width;
            col.HeaderText = caption;
            col.Text = buttonText;
            col.UseColumnTextForButtonValue = true;
            grid.Columns.Add(col);
        }

        public void CreateGridColumns()
        {
            AddLinkColumn(dgvMyCollection, "Название", "Name", 600);
            AddTextColumn(dgvMyCollection, "Владеют", "OwnersString", 334);
            AddActionColumn(dgvMyCollection, "Действия", "Удалить игру", 100);
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


        async void UpdateNewGameInfoBlock()
        {
            picBoxGame.Image = null;
            lblShortDesctiption.Text = "Краткое описание:";
            lblIsStandalone.Text = "Является самостоятельной игрой (не дополнением):";
            var selectedGame = cbSelectRawGame.SelectedItem as TeseraBGGRawGame;
            if (selectedGame != null)
            {
                
                lblNewGameFullName.Text = selectedGame.ToString();
                lblNewGameFullName.Tag = selectedGame;
                lblIsStandalone.Text += selectedGame.IsAddition ? " НЕТ" : " ДА";
                if (selectedGame.TeseraInfo != null)
                {
                    lblShortDesctiption.Text = selectedGame.TeseraInfo.DescriptionShort;
                }
                if (selectedGame.BGGInfo != null)
                {
                    //var bgg = new BoardGameGeekXmlApi2Client(new HttpClient());
                    //var request = new ThingRequest(new[] { selectedGame.BGGInfo.Id });
                    //var response = await bgg.GetThingAsync(request);
                    //var item = response.Result.FirstOrDefault();
                    //if (item != null)
                    //    picBoxGame.ImageLocation = item.Image;

                    var reqResult = await WebApiHandler.GetGameImage(
                        UserSettings.Hosting.Url,
                        UserSettings.Hosting.Login,
                        UserSettings.Hosting.Password,
                        JWTPrm.Token,
                        selectedGame.BGGInfo.Id);
                    if (reqResult.StatusCode.ToString() == "OK")
                    {
                        var imgBytes = await reqResult.Content.ReadAsByteArrayAsync();
                        picBoxGame.Image = Utils.ByteToImage(imgBytes);
                    }
                    else
                    {
                        MessageBox.Show("Ошибка получения изображения" + reqResult);
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

        private void lblShortDesctiption_Click(object sender, EventArgs e)
        {

        }

        private void cbSelectRawGame_SelectionChangeCommitted(object sender, EventArgs e)
        {
            UpdateNewGameInfoBlock();
        }

        private void btAddGame_Click(object sender, EventArgs e)
        {
            var selectedGame = cbSelectRawGame.SelectedItem as TeseraBGGRawGame;
            if (selectedGame != null)
            {
                var gameIsAlreadyInMyCollection = false;
                if (selectedGame.BGGInfo != null)
                {
                    gameIsAlreadyInMyCollection = Cache.Games
                        .Any(c => c.BGGObjectId == selectedGame.BGGInfo.Id && c.Owners.Any(c1 => c1.UserId == CurrentUser.Id));
                }
                //TODO получать tesera_id
                if (!gameIsAlreadyInMyCollection && selectedGame.TeseraInfo!=null)
                {
                    gameIsAlreadyInMyCollection = Cache.Games
                        .Any(c => c.TeseraKey == selectedGame.TeseraInfo.Alias && c.Owners.Any(c1 => c1.UserId == CurrentUser.Id));
                }
                if (gameIsAlreadyInMyCollection)
                {
                    MessageBox.Show("Игра уже присутствует в вашей коллекции");
                }
                else
                {
                    var message = string.Format("Вы действительно хотите добавить игру {0} в свою коллекцию?",
                        selectedGame);
                    if (MessageBox.Show(message, "Подтверждение удаления", MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                    {
                        MessageBox.Show("Здесь произойдут действия по добавлению игры");
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите игру для добавления из выпадающего списка выше");
            }
        }
    }
}
