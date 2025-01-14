using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        private List<Game> tmpMyCollection = new List<Game>();
        private List<Game> gamesToAdd = new List<Game>();
        private List<Game> gamesToRemove = new List<Game>();

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
            dgvMyCollection.DataBindingComplete +=
                new DataGridViewBindingCompleteEventHandler(DataBindingComplete);
        }

        private void DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            var grid = sender as DataGridView;
            // Loops through each row in the DataGridView, and adds the
            // row number to the header
            foreach (DataGridViewRow dGVRow in grid.Rows)
            {
                dGVRow.HeaderCell.Value = String.Format("{0}", dGVRow.Index + 1);
            }

            // This resizes the width of the row headers to fit the numbers
            grid.AutoResizeRowHeadersWidth(DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders);
        }

        private void CacheLoaded(object sender, EventArgs e)
        {
            var result = (e as WebResultEventArgs).Result as WebDataAllListstResultForBW;
            if (!result.Result)
            {
                MessageBox.Show(this, "Ошибка получения данных с сервера. " + result.Message);
            }
            else
            {
                ResetMyCollection(true);
            }
        }



        private void ManageMyCollection_Load(object sender, EventArgs e)
        {
            Cache.AllLoaded += CacheLoaded;
            PrepareDataGrid();
            ResetMyCollection(true);
        }



        #region GridMethods

        void ResetMyCollection(bool gridToRefresh)
        {
            gamesToRemove.Clear();
            gamesToAdd.Clear();
            tmpMyCollection = CreateDataSetFromCache();
            if (gridToRefresh)
                FillDataGrid();
        }

        List<Game> CreateDataSetFromCache()
        {
            var result = new List<Game>();
            foreach (var game in Cache.Games.Where(c => c.Owners.Select(c1 => c1.UserId).Contains(CurrentUser.Id)).OrderBy(c => c.Name))
            {
                result.Add(game.CreateCopy());
            }

            return result;
        }

        void FillDataGrid()
        {
            dgvMyCollection.DataSource = null;
            dgvMyCollection.DataSource = tmpMyCollection;
        }

        bool AddGameToMyCollection(TeseraBGGRawGame rawGame, out string outMessage)
        {
            var result = true;
            outMessage = string.Empty;
            var gameIsInMyCollection = false;
            int? bggId = null;
            int? teseraId = null;
            if (rawGame.BGGInfo != null)
            {
                bggId = rawGame.BGGInfo.Id;
                gameIsInMyCollection = tmpMyCollection.Any(c => c.BGGObjectId == bggId);
            }
            if (!gameIsInMyCollection && rawGame.TeseraInfo != null)
            {
                teseraId = rawGame.TeseraInfo.Id;
                gameIsInMyCollection = tmpMyCollection.Any(c => c.TeseraId != null && c.TeseraId == teseraId);
            }
            if (gameIsInMyCollection)
            {
                outMessage = "Игра уже присутствует в вашей коллекции";
                return false;
            }
            else
            {
                Game newGame;
                var gameInCommonCollection = Cache.Games.FirstOrDefault(c => (bggId!=null && c.BGGObjectId == bggId) || (teseraId!=null && c.TeseraId != null && c.TeseraId == teseraId));
                if (gameInCommonCollection != null)
                {
                    newGame = gameInCommonCollection;
                    var gameOwner = new GameOwner();
                    gameOwner.UserId = CurrentUser.Id;
                    gameOwner.UserName = CurrentUser.Name;
                    gameOwner.CreateTime = DateTime.Now;
                    newGame.Owners.Add(gameOwner);
                }
                else
                {
                    newGame = Game.CreateGame(Cache.RawGames, new List<User>() { CurrentUser }, true, bggId, teseraId, null, null);
                }

                
                var gameToRemove = gamesToRemove.FirstOrDefault(c => (bggId != null && c.BGGObjectId == bggId) || (teseraId != null && c.TeseraId != null && c.TeseraId == teseraId));
                if (gameToRemove != null)
                    gamesToRemove.Remove(gameToRemove);
                gamesToAdd.Add(newGame);
                tmpMyCollection.Add(newGame);
                FillDataGrid();
            }
            return result;
        }

        bool RemoveGameFromMyCollection(Game game, out string outMessage)
        {
            var result = true;
            outMessage = string.Empty;
            Game gameInMyCollection = null;
            int bggId = game.BGGObjectId;
            int? teseraId = game.TeseraId;
            if (bggId>0)
            {
                gameInMyCollection = tmpMyCollection.FirstOrDefault(c => c.BGGObjectId == bggId);
            }
            if (gameInMyCollection == null)
            {
                gameInMyCollection = tmpMyCollection.FirstOrDefault(c => c.TeseraId != null && c.TeseraId == teseraId);
            }
            if (gameInMyCollection==null)
            {
                outMessage = "Игры пока нет в вашей коллекции";
                return false;
            }
            else
            {
                var myOwner = gameInMyCollection.Owners.FirstOrDefault(c => c.UserId == CurrentUser.Id);
                if (myOwner != null)
                    gameInMyCollection.Owners.Remove(myOwner);

                var gameToAdd = gamesToAdd.FirstOrDefault(c => (bggId != null && c.BGGObjectId == bggId) || (teseraId != null && c.TeseraId != null && c.TeseraId == teseraId));
                if (gameToAdd != null)
                    gamesToAdd.Remove(gameToAdd);

                gamesToRemove.Add(gameInMyCollection);
                tmpMyCollection.Remove(gameInMyCollection);
                FillDataGrid();
            }
            return result;
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
            dgvMyCollection.RowHeadersVisible = true;
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

                string message;
                var result = RemoveGameFromMyCollection(game, out message);
                if (!result)
                    MessageBox.Show(this, message);
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
            AddLinkColumn(dgvMyCollection, "Название", "Name", 525);
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
                        MessageBox.Show(this, "Ошибка получения изображения" + reqResult);
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
                string message;
                var result = AddGameToMyCollection(selectedGame, out message);
                if (!result)
                    MessageBox.Show(this, message);
            }
            else
            {
                MessageBox.Show(this, "Выберите игру для добавления из выпадающего списка выше");
            }
        }

        private void ManageMyCollection_FormClosing(object sender, FormClosingEventArgs e)
        {

            if (!DiscartChangesWithQuestion(false))
            {
                e.Cancel = true;
                return;
            }

            Cache.AllLoaded -= CacheLoaded;
        }

        async Task SaveChanges(bool gridToRefresh)
        {
            //List<Task> tasks = new List<Task>();
            if (gamesToAdd.Any())
            {
                //var resAdding = 
                  await  WebApiHandler.AddGamesForUser(UserSettings.Hosting.Url, UserSettings.Hosting.Login,
                    UserSettings.Hosting.Password, JWTPrm.Token, gamesToAdd);
                //tasks.Add(resAdding);
            }

            if (gamesToRemove.Any())
            {
                //var resRemoving = 
                await WebApiHandler.RemoveGamesFromUser(UserSettings.Hosting.Url, UserSettings.Hosting.Login,
                    UserSettings.Hosting.Password, JWTPrm.Token, gamesToRemove);
                //tasks.Add(resRemoving);
            }

            //Task.WaitAll(tasks.ToArray());

            Cache.LoadAll(UserSettings.Hosting);
            if (gridToRefresh)
            {
                MessageBox.Show(this, "Изменения сохранены!");
            }
        }

        async Task SaveChangesWithQuestion(bool gridToRefresh)
        {
            if (gamesToAdd.Any() || gamesToRemove.Any())
            {
                if (MessageBox.Show(this, ChangesList(true), "Подтверждение изменения коллекции", MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    await SaveChanges(gridToRefresh);
                }
            }
        }

        bool DiscartChangesWithQuestion(bool gridToRefresh)
        {
            if (gamesToAdd.Any() || gamesToRemove.Any())
            {
                if (MessageBox.Show(this, ChangesList(false), "Подтверждение изменения коллекции", MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    ResetMyCollection(gridToRefresh);
                    return true;
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        string ChangesList(bool saveOption)
        {
            StringBuilder sb = new StringBuilder();
            if (gamesToAdd.Any())
            {
                if (saveOption)
                    sb.AppendLine("В вашу коллецию будут добавлены игры:");
                else
                    sb.AppendLine("Вы хотели добавить в вашу коллекцию игры:");
                foreach (var game in gamesToAdd.OrderBy(c=>c.Name))
                {
                    sb.AppendLine(game.Name);
                }
            }
            if (gamesToRemove.Any())
            {
                if (saveOption)
                    sb.AppendLine("Из вашей коллекции будут удалены игры:");
                else
                    sb.AppendLine("Вы хотели удалить из вашей коллекции игры:");
                foreach (var game in gamesToRemove.OrderBy(c => c.Name))
                {
                    sb.AppendLine(game.Name);
                }
            }

            if (saveOption)
                sb.AppendLine("Сохранить эти изменения?");
            else
                sb.AppendLine("Отменить эти изменения?");

            return sb.ToString();
        }

        private async void btSave_Click(object sender, EventArgs e)
        {
            await SaveChangesWithQuestion(true);
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            DiscartChangesWithQuestion(true);
        }

    }
}
