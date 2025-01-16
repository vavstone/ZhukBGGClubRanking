using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZhukBGGClubRanking.WinAdminApp
{
    public partial class Form1 : Form
    {
        private BackgroundWorker bwTeseraCollection = new BackgroundWorker();
        string teseraUrl = "https://api.tesera.ru/";
        public Form1()
        {
            InitializeComponent();
            bwTeseraCollection.WorkerSupportsCancellation = true;
            bwTeseraCollection.WorkerReportsProgress = true;
            bwTeseraCollection.DoWork += BwTeseraCollection_DoWork; 
            bwTeseraCollection.RunWorkerCompleted += BwTeseraCollection_RunWorkerCompleted; 
        }

        async void DoGamesRequest(HttpClient client, List<TeseraRawGame> games, WebPrmGetGames prm)
        {
            
                var parameters = new Dictionary<string, string>();
                string addQueryString = string.Empty;
                parameters.Add("offset", prm.Offset.ToString());
                parameters.Add("limit", prm.Limit.ToString());
                if (parameters.Any())
                    addQueryString = "?" + string.Join("&", parameters.Select(p => $"{p.Key}={Uri.EscapeDataString(p.Value)}"));
                var res = await client.GetAsync("games" + addQueryString).Result.Content.ReadAsAsync<List<TeseraRawGame>>();
                if (res.Count > 0 && (prm.PortionsToGet == 0 || prm.Offset < prm.PortionsToGet))
                {
                    games.AddRange(res);
                    prm.Offset++;
                    DoGamesRequest(client, games, prm);
                }
        }

        private async void BwTeseraCollection_DoWork(object sender, DoWorkEventArgs e)
        {
            var result = new WebDataGamesCollectionResult();
            
            var options = e.Argument as WebPrmGetGames;
            /*var reqResult = WebApiHandler.GetGames(options.Url, options.Offset, options.Limit);

            if (reqResult.Result.StatusCode.ToString() == "OK")
            {
                result.Result = true;
                result.Games = await reqResult.Result.Content.ReadAsAsync<List<Game>>();
            }
            else
            {
                result.Result = false;
                result.Message = reqResult.Result.StatusCode.ToString();
            }*/
            var games = new List<TeseraRawGame>();
            var client = WebApiHandler.GetClientWithAuth(options.Url);
            result.Result = true;
            result.Games = games;
            DoGamesRequest(client, games, options);
            foreach (var game in games)
                DBTeseraRawGame.SaveTeseraRawGame(game);
            e.Result = result;
        }

        private void BwTeseraCollection_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            var result = e.Result as WebDataGamesCollectionResult;
            //var res = "";
            var resAr = result.Games;
            dataGridTesera.DataSource = resAr;
        }

        private void btLoadTeseraData_Click(object sender, EventArgs e)
        {
            DBCommon.ClearTeseraRawTable();
            bwTeseraCollection.RunWorkerAsync(new WebPrmGetGames
            {
                Url = teseraUrl,
                Limit = 100,
                Offset = 0,
                PortionsToGet = 10
            });
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

        private void Form1_Load(object sender, EventArgs e)
        {
            dataGridTesera.MouseWheel += DataGridView_MouseWheel;
            dataGridTesera.MouseEnter += DataGridView_MouseEnter;
        }
    }
}
