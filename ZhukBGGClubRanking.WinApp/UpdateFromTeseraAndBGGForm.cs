using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using ZhukBGGClubRanking.Core.Model;
using ZhukBGGClubRanking.WinApp.Core;

namespace ZhukBGGClubRanking.WinApp
{
    public partial class UpdateFromTeseraAndBGGForm : Form
    {
        public HostingSettings Settings { get; set; }
        private TeseraRawInfoWrapper teseraRawInfoWrapper = new TeseraRawInfoWrapper();
        List<TeseraRawGame> teseraRawGames = new List<TeseraRawGame>();

        #region Common
        public UpdateFromTeseraAndBGGForm()
        {
            InitializeComponent();
            teseraRawInfoWrapper.RawInfoLoaded += OnRawInfoLoaded;
            teseraRawInfoWrapper.ProgressChanged += OnRawInfoProgressChanged;
        }
        
        private void UpdateFromTeseraAndBGGForm_Load(object sender, EventArgs e)
        {
            SetLblGamesFromTeseraGetted();
        }
        #endregion
        
        #region TeseraTab
        private void btClearTeseraRawInfo_Click(object sender, EventArgs e)
        {
            var clearTeseraRawInfoWrapper = new SimpleAsyncRequestToAPIWrapper();
            clearTeseraRawInfoWrapper.WorkCompleted += (sender2, e2) =>
            {
                var ea = e2 as WebResultEventArgs;
                var result = ea.Result;
                if (result.Result)
                {
                    MessageBox.Show("Выполнена очистка!");
                }
                else
                {
                    MessageBox.Show(result.Message);
                }
            };
            clearTeseraRawInfoWrapper.MethodToExecute = WebApiHandler.ClearTeseraRawTable;
            clearTeseraRawInfoWrapper.DoWork(Settings);
        }
        private void btGetDataFromTesera_Click(object sender, EventArgs e)
        {
            var prm = new WebPrmGetTeseraGames(null)
            {
                Url = tbUrlAPITesera.Text,
                Limit = 100,
                Offset = 0,
                PortionsToGet = 0
            };
            teseraRawInfoWrapper.StartRequests(prm);
        }
        private void OnRawInfoProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            var ea = e as WebDataTeseraGamesProgressChangedEventArgs;
            var portionsOfGames = ea.PortionOfGames;
            var rawJson = ea.RawJson;
            var fileNameShort = teseraRawGames.Count + ".json";
            var fileNameFull = Path.Combine("c:\\tmp\\1\\", fileNameShort);
            File.WriteAllText(fileNameFull, rawJson);
            teseraRawGames.AddRange(portionsOfGames);
            SetLblGamesFromTeseraGetted();
        }
        private void OnRawInfoLoaded(object sender, EventArgs e)
        {
            var ea = e as WebResultEventArgs;
            var result = ea.Result;
            if (result.Result)
            {
                MessageBox.Show("Информация из тесеры получена!");
            }
            else
            {
                MessageBox.Show(result.Message);
            }
        }
        private void btSaveTeseraRawInfoToDB_Click(object sender, EventArgs e)
        {
            var saveTeseraRawInfoWrapper = new SimpleAsyncRequestToAPIWrapper();
            saveTeseraRawInfoWrapper.WorkCompleted += (sender2, e2) =>
            {
                var ea = e2 as WebResultEventArgs;
                var result = ea.Result;
                if (result.Result)
                {
                    MessageBox.Show("Загрузка игр тесеры в БД выполнена!");
                }
                else
                {
                    MessageBox.Show(result.Message);
                }
            };
            saveTeseraRawInfoWrapper.MethodToExecute = WebApiHandler.SaveTeseraGamesRawInfo;
            saveTeseraRawInfoWrapper.DoWork(Settings);
        }
        void SetLblGamesFromTeseraGetted()
        {
            lblGamesFromTeseraGetted.Text = string.Format("Получено {0} игр", teseraRawGames.Count);
        }
        #endregion


        #region BGGTeseraTab
        private void btClearBGGTeseraRawInfo_Click(object sender, EventArgs e)
        {
            var clearBGGTeseraRawInfoWrapper = new SimpleAsyncRequestToAPIWrapper();
            clearBGGTeseraRawInfoWrapper.WorkCompleted += (sender2, e2) =>
            {
                var ea = e2 as WebResultEventArgs;
                var result = ea.Result;
                if (result.Result)
                {
                    MessageBox.Show("Выполнена очистка!");
                }
                else
                {
                    MessageBox.Show(result.Message);
                }
            };
            clearBGGTeseraRawInfoWrapper.MethodToExecute = WebApiHandler.ClearBGGTeseraRawTable;
            clearBGGTeseraRawInfoWrapper.DoWork(Settings);
        }

        private void btSaveBGGTeseraRawInfoToDB_Click(object sender, EventArgs e)
        {
            var saveBGGTeseraRawInfoWrapper = new SimpleAsyncRequestToAPIWrapper();
            saveBGGTeseraRawInfoWrapper.WorkCompleted += (sender2, e2) =>
            {
                var ea = e2 as WebResultEventArgs;
                var result = ea.Result;
                if (result.Result)
                {
                    MessageBox.Show("Загрузка игр в общую таблицу БД выполнена!");
                }
                else
                {
                    MessageBox.Show(result.Message);
                }
            };
            saveBGGTeseraRawInfoWrapper.MethodToExecute = WebApiHandler.SaveBGGAndTeseraGamesRawInfo;
            saveBGGTeseraRawInfoWrapper.DoWork(Settings);
        }

        #endregion


    }
}
