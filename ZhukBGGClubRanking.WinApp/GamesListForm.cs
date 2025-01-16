using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public partial class GamesListForm : Form
    {
        public UserSettings Settings;
        public GamesListForm()
        {
            InitializeComponent();
        }

        List<GameFullInfoWrapper> games = new List<GameFullInfoWrapper>();

        private async void GamesListForm_Load(object sender, EventArgs e)
        {
            PrepareDataGrid();
            LoadGames(); 
            grid.DataBindingComplete += new DataGridViewBindingCompleteEventHandler(DataBindingComplete);
        }

        private void DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            var grid = sender as DataGridView;
            foreach (DataGridViewRow dGVRow in grid.Rows)
            {
                dGVRow.HeaderCell.Value = String.Format("{0}", dGVRow.Index + 1);
            }
            grid.AutoResizeRowHeadersWidth(DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders);
        }

        public GamesFilter GetFilter()
        {
            var filter = new GamesFilter();
            filter.GetOnlyClubCollection  = cbOnlyClubCollection.Checked;
            return filter;
        }

        async Task LoadGames()
        {
            var worker = new SimpleAsyncRequestToAPIWrapper2<GamesFilter,List<GameFullInfoWrapper>>();
            worker.MethodToExecute = WebApiHandler.GetGamesWithBGGLinks;
            worker.WorkCompleted += (o, e) =>
            {
                var ea = e as WebResultEventArgs;
                var result = ea.Result;
                if (result.Result)
                {
                    games = worker.ResultContent.Result;
                    FillDataGrid();
                }
                else
                {
                    MessageBox.Show(result.Message);
                }
               
            };
            worker.DoWork(Settings.Hosting.Url, Settings.Hosting.Login,
                Settings.Hosting.Password, JWTPrm.Token, GetFilter());

        }

        public void PrepareDataGrid()
        {
            CreateGridColumns();
            grid.RowHeadersVisible = true;
            grid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
            grid.RowsDefaultCellStyle.WrapMode = DataGridViewTriState.True;
            grid.CellContentClick += Grid_CellContentClick;
            //grid.CellClick += Grid_CellClick;
        }

        private void Grid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var grid = sender as DataGridView;
            var gamesList = grid.DataSource as List<GameFullInfoWrapper>;
            if (e.ColumnIndex == 0)
            {
                if (grid.Columns[e.ColumnIndex] is DataGridViewLinkColumn && e.RowIndex >= 0)
                {
                    var game = gamesList[e.RowIndex];
                    string url = string.Empty;
                    var teseraUrl = Utils.GetTeseraCardUrl(game.TeseraKey);
                    var bggUrl = Utils.GetBGGCardUrl(game.BGGObjectId);
                    if (Settings.TeseraPreferable)
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
            AddLinkColumn(grid, "Название", "Name", 200);
            AddLinkColumn(grid, "Год", "YearPublished", 70);
            AddTextColumn(grid, "Владеют", "OwnersString", 150);
            AddTextColumn(grid, "Авторы", "DesignersString", 100);
            AddTextColumn(grid, "Категория", "CategoriesString", 100);
            //AddTextColumn(grid, "Семейство", "FamiliesString", 100);
            AddTextColumn(grid, "Механики", "MechanicsString", 300);
            AddTextColumn(grid, "Самост-ая игра", "IsStandaloneGameString", 70);
        }



        void FillDataGrid()
        {
            grid.DataSource = null;
            grid.DataSource = games;
        }
    }
}
